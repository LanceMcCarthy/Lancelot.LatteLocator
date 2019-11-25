using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace LatteLocator.Core.Common
{
    public class ExceptionLogger
    {
        public static void LogException(Exception currentException)
        {
            String exceptionMessage = CreateErrorMessage(currentException);
            PurgeLogFiles();
            LogFileWrite(exceptionMessage);
        }

        /// <summary>
        /// This method is for prepare the error message to log using Exception object
        /// </summary>
        /// <param name="currentException"/>
        /// <returns></returns>
        private static String CreateErrorMessage(Exception currentException)
        {
            StringBuilder messageBuilder = new StringBuilder();
            try
            {
                messageBuilder.AppendLine("-----------------------------------------------------------------");
                messageBuilder.AppendLine("Source: " + currentException.Source.Trim());
                messageBuilder.AppendLine("Date Time: " + DateTime.Now);
                messageBuilder.AppendLine("-----------------------------------------------------------------");
                messageBuilder.AppendLine("Method: " + currentException.Message.Trim());
                messageBuilder.AppendLine("Exception :: " + currentException);
                if(currentException.InnerException != null)
                {
                    messageBuilder.AppendLine("InnerException :: " + currentException.InnerException);
                }
                messageBuilder.AppendLine("");
                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.AppendLine("Exception:: Unknown Exception.");
                return messageBuilder.ToString();
            }
        }

        /// <summary>
        /// This method is for writing the Log file with the current exception message
        /// </summary>
        /// <param name="exceptionMessage">
        private static async void LogFileWrite(string exceptionMessage)
        {
            try
            {
                string fileName = "VideoDiaryError-Log" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "log";
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var logFolder = await localFolder.CreateFolderAsync("Logs", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var logFile = await logFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);

                if(!String.IsNullOrEmpty(exceptionMessage))
                {
                    await FileIO.AppendTextAsync(logFile, exceptionMessage);
                }
            }
            catch(Exception)
            {
            }
        }

        /// <summary> 
        /// This method purge old log files in the log folder, which are older than daysToKeepLog. 
        /// </summary> 
        public static async void PurgeLogFiles()
        {
            int daysToKeepLog;
            DateTime todaysDate;
            var logFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                daysToKeepLog = 5;
                todaysDate = DateTime.Now.Date;

                logFolder = await logFolder.GetFolderAsync("Logs");
                IReadOnlyList<StorageFile> files = await logFolder.GetFilesAsync();

                if (files.Count < 1) return;

                foreach (StorageFile file in files)
                {
                    BasicProperties basicProperties = await file.GetBasicPropertiesAsync();
                    if (file.FileType == ".log")
                    {
                        if (DateTime.Compare(todaysDate, basicProperties.DateModified.AddDays(daysToKeepLog).DateTime.Date) >= 0)
                        {
                            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
