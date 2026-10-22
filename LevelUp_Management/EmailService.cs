using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace LevelUp_Management
{
    // Bonus feature (like GoogleCalendarService / GoogleTasksService): outside the
    // entity/SP pattern - no DB access. Sends HTML emails through any SMTP provider
    // (Gmail, Outlook, Yahoo...) configured in App.config appSettings.
    public static class EmailService
    {
        private const string AccentColor = "#2D9F93";

        // Returns true if the email was sent. Shows a MessageBox and returns false
        // if the address is missing or sending fails.
        public static async Task<bool> SendEmailAsync(string toAddress, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toAddress))
            {
                MessageBox.Show("למתאמן אין כתובת מייל", "שגיאה", MessageBoxButtons.OK);
                return false;
            }

            try
            {
                string host = ConfigurationManager.AppSettings["SmtpHost"];
                int port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                string fromEmail = ConfigurationManager.AppSettings["SmtpEmail"];
                string password = ConfigurationManager.AppSettings["SmtpPassword"];
                string displayName = ConfigurationManager.AppSettings["SmtpDisplayName"];

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(displayName, fromEmail));
                message.To.Add(MailboxAddress.Parse(toAddress.Trim()));
                message.Subject = subject;

                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = body;
                message.Body = builder.ToMessageBody();

                // Async variants of Connect/Authenticate/Send/Disconnect so the UI
                // stays responsive while the SMTP server answers.
                using (SmtpClient client = new SmtpClient())
                {
                    await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(fromEmail, password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בשליחת המייל: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return false;
            }
        }

        // Builds a styled RTL Hebrew HTML email: studio header, greeting, intro line,
        // a details table (label/value pairs) and a closing line. All values are HTML-encoded.
        public static string BuildHtmlBody(string traineeName, string intro, string[,] details, string closing)
        {
            StringBuilder rows = new StringBuilder();
            for (int i = 0; i < details.GetLength(0); i++)
            {
                rows.Append("<tr>")
                    .Append("<td style=\"padding:8px 12px;color:#64748b;font-weight:bold;white-space:nowrap;\">")
                    .Append(WebUtility.HtmlEncode(details[i, 0])).Append("</td>")
                    .Append("<td style=\"padding:8px 12px;color:#1e293b;\">")
                    .Append(WebUtility.HtmlEncode(details[i, 1])).Append("</td>")
                    .Append("</tr>");
            }

            return
                "<html><body dir=\"rtl\" style=\"margin:0;padding:0;background:#f1f5f9;font-family:Segoe UI,Arial,sans-serif;\">" +
                "<div style=\"max-width:560px;margin:24px auto;background:#ffffff;border-radius:10px;overflow:hidden;border:1px solid #e2e8f0;\">" +
                "<div style=\"background:" + AccentColor + ";padding:20px;text-align:center;\">" +
                "<div style=\"color:#ffffff;font-size:26px;font-weight:bold;letter-spacing:2px;\">LEVEL UP</div>" +
                "<div style=\"color:#e6f6f4;font-size:13px;\">Studio · באר שבע</div>" +
                "</div>" +
                "<div style=\"padding:24px;text-align:right;\">" +
                "<p style=\"font-size:17px;color:#1e293b;margin:0 0 12px;\">שלום " + WebUtility.HtmlEncode(traineeName) + ",</p>" +
                "<p style=\"font-size:15px;color:#334155;margin:0 0 16px;\">" + WebUtility.HtmlEncode(intro) + "</p>" +
                "<table style=\"width:100%;border-collapse:collapse;background:#f8fafc;border-right:4px solid " + AccentColor + ";\">" +
                rows +
                "</table>" +
                "<p style=\"font-size:15px;color:#334155;margin:16px 0 0;\">" + WebUtility.HtmlEncode(closing) + "</p>" +
                "</div>" +
                "<div style=\"background:#f8fafc;padding:14px;text-align:center;color:#94a3b8;font-size:12px;border-top:1px solid #e2e8f0;\">" +
                "צוות <span style=\"color:" + AccentColor + ";font-weight:bold;\">Level UP Studio</span> 💪" +
                "</div>" +
                "</div></body></html>";
        }
    }
}
