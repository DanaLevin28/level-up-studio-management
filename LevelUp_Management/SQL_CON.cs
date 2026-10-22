using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace LevelUp_Management
{
    /// <summary>
    /// מחלקה שאחראית על החיבור לבסיס הנתונים וביצוע שאילתות.
    /// כל פעולה מול בסיס הנתונים עוברת דרך מחלקה זו.
    ///
    /// שתי סוגי פעולות:
    /// 1. execute_non_query - פעולות שמשנות נתונים (INSERT, UPDATE, DELETE)
    /// 2. execute_query    - פעולות שמחזירות נתונים (SELECT)
    /// </summary>
    class SQL_CON
    {
        SqlConnection conn;

        // =================================================================
        // עמידות בפני תקלות זמניות ב-Azure SQL
        //
        // בסיס הנתונים levelup_studio רץ בתצורת Serverless (GP_S_Gen5_2).
        // תצורה זו מכניסה את בסיס הנתונים להשהיה (auto-pause) לאחר תקופת
        // חוסר פעילות, וההתחברות הראשונה שאחריה נכשלת עם שגיאה 40613
        // ("Database ... is not currently available") בזמן ש-Azure מעיר אותו.
        // ההתעוררות אורכת בדרך כלל 30-60 שניות.
        //
        // לכן כל פתיחת חיבור עוברת דרך openWithRetry: הניסיון חוזר על עצמו
        // עם המתנה גדלה, במקום להיכשל מיד ולהציג שגיאה למשתמש.
        // =================================================================

        // מספרי השגיאות הזמניות המתועדות של Azure SQL.
        private static readonly int[] transientErrorNumbers =
        {
            40613,  // Database is not currently available (auto-pause / failover)
            40197,  // The service has encountered an error processing your request
            40501,  // The service is currently busy
            40143,  // The service has encountered an error
            4060,   // Cannot open database requested by the login
            4221,   // Login to read-secondary failed
            49918,  // Cannot process request. Not enough resources
            49919,  // Cannot process create or update request
            49920,  // Cannot process request. Too many operations in progress
            10928,  // Resource ID limit reached
            10929,  // Minimum guarantee not met
            10053,  // Transport-level error
            10054,  // Existing connection was forcibly closed
            10060,  // Network-related or instance-specific error
            233,    // No process on the other end of the pipe
            64,     // A connection was successfully established but then failed
            1205,   // Deadlock victim
            -2      // Timeout expired
        };

        private const int maxAttempts = 5;

        // תקרת זמן כוללת לכל הניסיונות. מחרוזת החיבור מגדירה
        // Connection Timeout=60, ולכן בלי תקרה כזו תקלת רשת אמיתית (להבדיל
        // מהשהיית Serverless, שמחזירה שגיאה מיד) הייתה עלולה להקפיא את
        // התוכנה למספר דקות. שגיאה 40613 חוזרת מהר, ולכן התקציב הזה מספיק
        // בנוחות כדי לכסות את זמן ההתעוררות.
        private const int totalRetryBudgetSeconds = 90;

        // מוצגת הודעת תקלת-התחברות אחת בלבד בכל הרצה: initLists טוענת 16
        // רשימות, ובלי הדגל הזה המשתמש היה נתקל ב-16 חלונות שגיאה רצופים.
        private static bool connectionErrorShown = false;

        public SQL_CON()
        {
            // מחרוזת החיבור נטענת מ-App.config (connectionStrings/LevelUpDb) ולא
            // מוטבעת בקוד - כך שכל חבר צוות משתמש בקובץ הקונפיגורציה המקומי שלו,
            // שאינו עולה ל-git (ראו .gitignore).
            string connectionString = ConfigurationManager.ConnectionStrings["LevelUpDb"].ConnectionString;
            conn = new SqlConnection(connectionString);
        }

        /// <summary>
        /// פתיחת החיבור עם ניסיונות חוזרים על שגיאות זמניות בלבד.
        /// שגיאה שאינה זמנית (סיסמה שגויה, שם פרוצדורה שגוי) נזרקת מיד -
        /// אין טעם לחזור עליה.
        /// </summary>
        private void openWithRetry()
        {
            int waitSeconds = 4;
            System.Diagnostics.Stopwatch elapsed = System.Diagnostics.Stopwatch.StartNew();

            for (int attempt = 1; ; attempt++)
            {
                try
                {
                    conn.Open();
                    return;
                }
                catch (SqlException ex)
                {
                    bool budgetLeft = elapsed.Elapsed.TotalSeconds + waitSeconds < totalRetryBudgetSeconds;

                    if (attempt >= maxAttempts || !budgetLeft || !isTransient(ex))
                        throw;

                    // סמן למשתמש שהמערכת ממתינה ולא נתקעה.
                    Cursor.Current = Cursors.WaitCursor;
                    System.Threading.Thread.Sleep(waitSeconds * 1000);
                    waitSeconds += 4;   // 4 + 8 + 12 + 16 = עד 40 שניות המתנה מצטברת
                }
            }
        }

        /// <summary>האם השגיאה היא תקלה זמנית של Azure SQL שכדאי לנסות שוב?</summary>
        private static bool isTransient(SqlException ex)
        {
            foreach (SqlError err in ex.Errors)
            {
                if (Array.IndexOf(transientErrorNumbers, err.Number) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// הודעה אחת, ברורה ומוסברת, על תקלת התחברות - במקום הודעת Azure
        /// גולמית שחוזרת על עצמה פעם אחר פעם.
        /// </summary>
        private static void showConnectionErrorOnce(SqlException ex)
        {
            if (connectionErrorShown)
                return;
            connectionErrorShown = true;

            MessageBox.Show(
                "לא ניתן להתחבר כרגע לבסיס הנתונים של הסטודיו." +
                Environment.NewLine + Environment.NewLine +
                "בסיס הנתונים מתארח ב-Azure בתצורת Serverless ונכנס להשהיה לאחר " +
                "תקופה ללא שימוש. ההתחברות הראשונה מעירה אותו, וזה עשוי לקחת עד דקה." +
                Environment.NewLine + Environment.NewLine +
                "נסו להפעיל את התוכנה שוב בעוד כדקה." +
                Environment.NewLine + Environment.NewLine +
                "פירוט טכני: " + ex.Message,
                "שגיאת התחברות",
                MessageBoxButtons.OK);
        }

        /// <summary>
        /// ביצוע פעולה שמשנה נתונים בבסיס הנתונים (INSERT, UPDATE, DELETE).
        /// הפעולה לא מחזירה נתונים - רק מבצעת שינוי.
        /// </summary>
        /// <param name="cmd">פקודת SQL מוכנה עם פרמטרים</param>
        public void execute_non_query(SqlCommand cmd)
        {
            try
            {
                openWithRetry();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                // אין כאן הודעת הצלחה: כל פאנל מציג הודעת הצלחה משלו אחרי הפעולה.
            }
            catch (SqlException ex) when (isTransient(ex))
            {
                showConnectionErrorOnce(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע הפעולה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// ביצוע שאילתה שמחזירה נתונים מבסיס הנתונים (SELECT).
        /// מחזירה SqlDataReader לקריאת התוצאות שורה אחרי שורה.
        /// שימו לב: החיבור לא נסגר כאן - הוא נשאר פתוח כי ה-Reader צריך אותו.
        /// </summary>
        /// <param name="cmd">פקודת SQL מוכנה עם פרמטרים</param>
        /// <returns>SqlDataReader לקריאת התוצאות, או null אם הייתה שגיאה</returns>
        public SqlDataReader execute_query(SqlCommand cmd)
        {
            try
            {
                openWithRetry();
                cmd.Connection = conn;
                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (SqlException ex) when (isTransient(ex))
            {
                showConnectionErrorOnce(ex);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע השאילתה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return null;
            }
        }

        /// <summary>
        /// כמו execute_non_query, אך ללא בליעת חריגות: משמשת עבור Stored Procedures
        /// "מתוזמרים" (למשל sp_register_to_workout) שבהם ה-C# צריך לתפוס את ה-
        /// SqlException עצמו ולבדוק את ex.Number מול קודי השגיאה של תנאי השמירה
        /// הספציפיים - לא לקבל הודעת שגיאה גנרית. הקריאה עוטפת את עצמה ב-try/catch
        /// משלה.
        /// </summary>
        /// <param name="cmd">פקודת SQL מוכנה עם פרמטרים</param>
        public void execute_non_query_flow(SqlCommand cmd)
        {
            try
            {
                openWithRetry();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
