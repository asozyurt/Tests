using HeadlessBrowser.Common;
using HeadlessBrowser.Common.Enum;
using System;
using System.Threading;

namespace HeadlessBrowser.MainRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Setup();
            Console.WriteLine("Main Process Started");
            ThreadPool.QueueUserWorkItem(Process);
            ThreadPool.QueueUserWorkItem(StatusCheck);
            Timer timer = new Timer(StatusCheck, null, 0, Constants.STATUS_CHECK_INTERVAL_IN_MINUTES * 1000);
            Console.WriteLine("Main Process Ended");
            Console.Read();
        }

        private static void Setup()
        {
            UserContext.SetContext(Constants.MAIN_RUNNER_USER);
        }

        private static void StatusCheck(object state)
        {
            Console.WriteLine("Status Check Started");

            var createdUsers = Service.HeadlessBrowserServices.GetUsersByStatus(UserStatus.AccountCreated);
            if (createdUsers.Count > Constants.EXPORT_THRESHOLD_COUNT)
            {
                Console.WriteLine("Record Count Exceed " + Constants.EXPORT_THRESHOLD_COUNT);

                // Write records to file
                var fileName = Service.ExportServices.WriteToFile(createdUsers);
                // Update as Exported
                Service.HeadlessBrowserServices.BulkUpdateUserStatus(createdUsers, UserStatus.Exported);
                // Send as Mail Attachement
                Service.NotifyServices.SendMailWithAttachement(fileName);
                // Export to Ftp
                Service.ExportServices.ExportToFtp(fileName);
                // Finally DeleteFile
                Service.ExportServices.DeleteFile(fileName);
            }
        }

        private static void Process(object state)
        {
            while (true)
            {
                Console.WriteLine("Am I lying?");
                Thread.Sleep(3000);
            }
        }
    }
}
