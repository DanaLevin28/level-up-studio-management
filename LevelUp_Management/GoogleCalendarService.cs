using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Util.Store;

namespace LevelUp_Management
{
    // Adds workouts to the user's primary Google Calendar. OAuth 2.0 Desktop App
    // flow: the first call opens the browser for consent, the token is then saved
    // under %AppData% and reused. Not an entity - no DB access, so it sits outside
    // the entity/stored-procedure pattern.
    public static class GoogleCalendarService
    {
        private const string ApplicationName = "LevelUp Management";
        private const string TimeZone = "Asia/Jerusalem";

        private static CalendarService service;

        public static bool IsInitialized
        {
            get { return service != null; }
        }

        public static async Task InitializeAsync()
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
                // Includes the Tasks scope too, and shares tokenDir with
                // GoogleTasksService, so a single consent grants both and
                // whichever service authorizes first caches a token the other
                // can reuse without a second browser prompt.
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { CalendarService.Scope.CalendarEvents, TasksService.Scope.Tasks },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenDir, true));
            }

            service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public static async Task<string> AddWorkoutEventAsync(string workoutName, DateTime startTime,
            int durationMinutes = 60, string location = "Level UP Studio")
        {
            await InitializeAsync();

            Event calendarEvent = new Event
            {
                Summary = workoutName,
                Location = location,
                Start = new EventDateTime { DateTimeDateTimeOffset = new DateTimeOffset(startTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZone).GetUtcOffset(startTime)), TimeZone = TimeZone },
                End = new EventDateTime { DateTimeDateTimeOffset = new DateTimeOffset(startTime.AddMinutes(durationMinutes), TimeZoneInfo.FindSystemTimeZoneById(TimeZone).GetUtcOffset(startTime.AddMinutes(durationMinutes))), TimeZone = TimeZone },
                Reminders = new Event.RemindersData
                {
                    UseDefault = false,
                    Overrides = new List<EventReminder>
                    {
                        new EventReminder { Method = "popup", Minutes = 60 },
                        new EventReminder { Method = "popup", Minutes = 15 }
                    }
                }
            };

            Event created = await service.Events.Insert(calendarEvent, "primary").ExecuteAsync();
            return created.HtmlLink;
        }
    }
}
