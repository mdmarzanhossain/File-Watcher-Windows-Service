
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFirstWindowsService
{
    class MyFirstWindowsService : BackgroundService
    {
        public static string filePath = "logFile.txt";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => {
                watcher();
            });
        }

        private void watcher()
        {
            using (var watcher = new FileSystemWatcher(@"C:\Users\Marzan Hossain\Desktop\WatcherFolder")) 
            {


               /* watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;*/

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;
                watcher.Error += OnError;

                watcher.Filter = "*.txt";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static void LogFile(string msg) 
        {
            File.AppendAllText(filePath, "\n" + DateTimeOffset.Now + ">>" + msg);
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            LogFile($"Changed: {e.FullPath}");
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            LogFile(value);
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            LogFile($"Deleted: {e.FullPath}");
            Console.WriteLine($"Deleted: {e.FullPath}");
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            LogFile($"Renamed:" + $" Old: {e.OldFullPath}" + $" New: {e.FullPath}");
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
