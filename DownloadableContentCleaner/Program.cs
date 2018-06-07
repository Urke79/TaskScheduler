using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DownloadableContentCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!TaskScheduler.TaskExists(@"Downloadable Content Cleaner"))
            {
                TaskScheduler.CreateTask();
            }

            // Upload folder location
            var uploadFolderPath = @"E:\CustomerOnline\upload"; 
            var uploadFolder = new DirectoryInfo(uploadFolderPath);

            if (uploadFolder.Exists)
            {
                // Get files older then 30 days
                var dateFilter = DateTime.Now.Date.AddDays(-30);
                var files =
                    uploadFolder.GetFiles()
                        .Where(f => f.CreationTime < dateFilter)
                        .OrderBy(p => p.CreationTime)
                        .ToArray();
                        
                foreach (var file in files)
                {
                    var message = "Deleted " + file.Name + " Created on : " + file.CreationTime;
                    file.Delete();
                    LogActivity(message);
                }
            }
        }

        private static void LogActivity(string message)
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var fileLocation = appDirectory + "DownloadableContentCleanerLog.txt";

            var log = !File.Exists(fileLocation) ? new StreamWriter(fileLocation) : File.AppendText(fileLocation);

            // Write to the file
            log.WriteLine(DateTime.Now);
            log.WriteLine(message);
            log.WriteLine();

            // Close the stream
            log.Close();
        }
    }
}
