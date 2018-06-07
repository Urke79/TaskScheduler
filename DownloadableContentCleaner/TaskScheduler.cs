using System;
using Microsoft.Win32.TaskScheduler;

namespace DownloadableContentCleaner
{
   public class TaskScheduler
    {
        public static bool TaskExists(string taskName)
        {
            // Get the service on the local machine
            using (var ts = new TaskService())
            {
                return ts.GetTask(taskName) != null;
            }
        }

        public static void CreateTask()
        {
            const string name = @"Downloadable Content Cleaner";
            const string description = @"Schedule a Downloadable Content Cleaning task";

            using (var ts = new TaskService())
            {
                var taskDefinition = ts.NewTask();
                taskDefinition.RegistrationInfo.Description = description;

                // Create daily trigger
                var daily = new DailyTrigger
                {
                    StartBoundary = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " 17:30:00"),
                    DaysInterval = 1
                };

                taskDefinition.Triggers.Add(daily);

                // App location
                var appLocation = AppDomain.CurrentDomain.BaseDirectory + "DownloadableContentCleaner.exe";
                taskDefinition.Actions.Add(new ExecAction(appLocation));

                ts.RootFolder.RegisterTaskDefinition(name, taskDefinition,
                    TaskCreation.CreateOrUpdate, "SYSTEM", null,
                    TaskLogonType.ServiceAccount);
            }
        }
    }
}
