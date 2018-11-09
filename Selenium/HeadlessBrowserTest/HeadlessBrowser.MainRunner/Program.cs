using HeadlessBrowser.Common;
using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Enum;
using HeadlessBrowser.Service;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HeadlessBrowser.MainRunner
{
    public class Program
    {
        private static List<string> _names;
        private static List<string> _surnames;
        private static List<string> _passwords;
        private static List<string> _genders;

        public static void Main(string[] args)
        {
            Setup();
            Console.WriteLine(string.Format("Main Process Started For User: {0}", UserContext.CurrentUser));
            switch (Constants.MODE)
            {
                case Constants.MODE_YANDEX:
                    ThreadPool.QueueUserWorkItem(YandexAccountCreatorProcess);
                    break;
                case Constants.MODE_FACEBOOK:
                    ThreadPool.QueueUserWorkItem(FacebookAccountCreatorProcess);
                    break;
                default:
                    Console.WriteLine(string.Format("Mode is not recognized. Control app.config file. Mode:{0}", Constants.MODE));
                    Console.Read();
                    break;
            }
            ThreadPool.QueueUserWorkItem(StatusCheck);
            Timer timer = new Timer(StatusCheck, null, 0, Constants.STATUS_CHECK_INTERVAL_IN_MINUTES * 60 * 1000);
            Console.Read();
        }

        private static void Setup()
        {
            UserContext.SetContext(Constants.CURRENT_USER);
            _names = (List<string>)NameServices.GetAllNames().Result;
            _surnames = (List<string>)SurnameServices.GetAllSurnames().Result;
            _passwords = new List<string>(new string[] { "b&LQ9g3n2W", "^Phs7je3yI", "NLttv3xZcoG", "LFsg)A73KM", "R8m$10F#!W", "YFeJbUB)aW", "x4Vla^sVNDpU", "R8CJoj&s@YG", "SeAUdw$ONv", "FHcz*8LP0y", "nDTL3LWgrC&", "$q7YeUtTsJ", "95tv3gt8q77g", "XF1SIFBs3h" });
            _genders = new List<string>(new string[] { "male", "female" });
        }

        private static void StatusCheck(object state)
        {
            Console.WriteLine("Status Check Started");

            var createdUsers = ImaginaryUserServices.GetUsersByStatus(UserStatus.AccountCreated);
            if (!createdUsers.IsError && ((List<ImaginaryUserDto>)createdUsers.Result).Count > Constants.EXPORT_THRESHOLD_COUNT)
            {
                Console.WriteLine("Record Count Exceed " + Constants.EXPORT_THRESHOLD_COUNT);

                // Write records to file
                var fileName = ExportServices.WriteToFile((List<ImaginaryUserDto>)createdUsers.Result);
                // Update as Exported
                ImaginaryUserServices.BulkUpdateUserStatus((List<ImaginaryUserDto>)createdUsers.Result, UserStatus.Exported);
                // Send as Mail Attachement
                NotifyServices.SendMailWithAttachement(fileName);
                // Export to Ftp
                ExportServices.ExportToFtp(fileName);
                // Finally DeleteFile
                ExportServices.DeleteFile(fileName);
            }
        }

        private static void YandexAccountCreatorProcess(object state)
        {
            while (true)
            {
                CreateYandexAccount();
                Thread.Sleep(Constants.THREE_SECONDS_IN_MILLISECOND);
            }
        }
        private static void FacebookAccountCreatorProcess(object state)
        {
            while (true)
            {
                var TAKE_COUNT = 1;
                var mailAccounts = (List<ImaginaryUserDto>)ImaginaryUserServices.GetUsersByStatus(UserStatus.EmailCreated, TAKE_COUNT).Result;

                foreach (var item in mailAccounts)
                {
                    // Mark as processing (TODO: Make this method work with id and lastupdate)
                    ImaginaryUserServices.UpdateUserStatus(item.Id, UserStatus.Processing);

                    var user = CreateFacebookAccount(item);

                    user.Status = UserStatus.AccountCreated;
                    ImaginaryUserServices.UpdateUser(user);
                }

                Thread.Sleep(Constants.THREE_SECONDS_IN_MILLISECOND);
            }
        }

        private static void CreateYandexAccount()
        {
            // Create random user
            ImaginaryUserDto newUser = getNewRandomUser();

            // TODO: Run yandex selenium macro

            // update users mailAddress
            newUser.Username = "Set in macro code";
            newUser.Email = "Set in macro code";
            newUser.Status = UserStatus.EmailCreated;
            var user = ImaginaryUserServices.InsertUser(newUser);

            Console.WriteLine(string.Format("Record created with Id {0}", ((ImaginaryUserDto)user.Result).Id));
        }

        private static ImaginaryUserDto getNewRandomUser()
        {
            return new ImaginaryUserDto
            {
                Name = getRandomName(),
                Surname = getRandomSurname(),
                BirthDate = getRandomBirthDate(),
                Password = getRandomPassword(),
                Gender = getRandomGender(),
                Status = UserStatus.New
            };
        }

        private static string getRandomGender()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return _genders[r.Next(0, 1)];
        }

        private static string getRandomPassword()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return _passwords[r.Next(0, 13)];
        }

        private static DateTime getRandomBirthDate()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return new DateTime(1988, 12, 11).AddDays(r.Next(-1000, 1000));
        }

        private static string getRandomSurname()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return _surnames[r.Next(0, _surnames.Count - 1)];
        }

        private static string getRandomName()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return _names[r.Next(0, _names.Count - 1)];
        }

        private static ImaginaryUserDto CreateFacebookAccount(ImaginaryUserDto user)
        {
            // Create facebook account

            // Run facebook selenium macro

            // Update user cookie and Ip
            return null;
        }

    }
}
