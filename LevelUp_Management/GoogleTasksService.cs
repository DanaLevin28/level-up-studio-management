using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;
using TaskItem = Google.Apis.Tasks.v1.Data.Task;

namespace LevelUp_Management
{
    // Adds fitness goals as tasks in a dedicated "Level UP - יעדים" Google Tasks
    // list. Same OAuth flow and token folder as GoogleCalendarService (both
    // scopes are requested by either service so one consent covers both). Not
    // an entity - no DB access, so it sits outside the entity/stored-procedure
    // pattern, same as GoogleCalendarService.
    public static class GoogleTasksService
    {
        private const string ApplicationName = "LevelUp Management";
        private const string GoalListTitle = "Level UP - יעדים";

        private static TasksService service;
        private static string goalListId;

        public static bool IsInitialized
        {
            get { return service != null; }
        }

        public static async System.Threading.Tasks.Task InitializeAsync()
        {
            if (service != null) return;

            string credentialsPath = Path.Combine(AppContext.BaseDirectory, "credentials.json");
            if (!File.Exists(credentialsPath))
                throw new FileNotFoundException("לא נמצא הקובץ credentials.json", credentialsPath);

            string tokenDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LevelUpManagement", "google-token");

            UserCredential credential;
            using (FileStream stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { CalendarService.Scope.CalendarEvents, TasksService.Scope.Tasks },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenDir, true));
            }

            service = new TasksService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            await ensureGoalListAsync();
        }

        private static async System.Threading.Tasks.Task ensureGoalListAsync()
        {
            TaskLists lists = await service.Tasklists.List().ExecuteAsync();
            if (lists.Items != null)
            {
                foreach (TaskList list in lists.Items)
                {
                    if (list.Title == GoalListTitle)
                    {
                        goalListId = list.Id;
                        return;
                    }
                }
            }

            TaskList created = await service.Tasklists.Insert(new TaskList { Title = GoalListTitle }).ExecuteAsync();
            goalListId = created.Id;
        }

        public static async Task<string> AddGoalAsync(string title, string notes, DateTime dueDate)
        {
            await InitializeAsync();

            TaskItem task = new TaskItem
            {
                Title = title,
                Notes = notes,
                Status = "needsAction",
                Due = dueDate.ToString("yyyy-MM-dd'T'HH:mm:ss.000'Z'")
            };

            TaskItem created = await service.Tasks.Insert(task, goalListId).ExecuteAsync();
            return created.Id;
        }
    }
}
