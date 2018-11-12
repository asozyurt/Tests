using HeadlessBrowser.Common;
using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Enum;
using HeadlessBrowser.Service;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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
                    //ThreadPool.QueueUserWorkItem(FacebookAccountCreatorProcess);
                    FacebookAccountCreatorProcess(null);
                    break;
                default:
                    Console.WriteLine(string.Format("Mode is not recognized. Control app.config file. Mode:{0}", Constants.MODE));
                    Console.Read();
                    break;
            }
            // ThreadPool.QueueUserWorkItem(StatusCheck);
            // Timer timer = new Timer(StatusCheck, null, 0, Constants.STATUS_CHECK_INTERVAL_IN_MINUTES * 60 * 1000);
            Console.Read();
        }

        private static void Setup()
        {
            UserContext.SetContext(Constants.CURRENT_USER);
            if (Constants.MODE == Constants.MODE_YANDEX)
            {
                _names = (List<string>)NameServices.GetAllNames().Result;
                _surnames = (List<string>)SurnameServices.GetAllSurnames().Result;
                _passwords = new List<string>(new string[] { "b&LQ9g3n2W", "^Phs7je3yI", "NLttv3xZcoG", "LFsg)A73KM", "R8m$10F#!W", "YFeJbUB)aW", "x4Vla^sVNDpU", "R8CJoj&s@YG", "SeAUdw$ONv", "FHcz*8LP0y", "nDTL3LWgrC&", "$q7YeUtTsJ", "95tv3gt8q77g", "XF1SIFBs3h" });
                _genders = new List<string>(new string[] { "male", "female" });
            }
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
                    try
                    {
                        // Mark as processing (TODO: Make this method work with id and lastupdate)
                        ImaginaryUserServices.UpdateUserStatus(item.Id, UserStatus.Processing);
                        var user = CreateFacebookAccount(item);

                        ImaginaryUserServices.UpdateUser(user);
                    }
                    catch
                    {
                        //TODO: Log error
                    }
                }

                Thread.Sleep(Constants.FIVE_SECONDS_IN_MILLISECOND);
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
            var resultDto = user;
            using (ChromeDriver driver = createChromeDriver())
            {
                resultDto.IpAddress = getIpAddress(driver);
                bool fbAccountCreated = registerFacebookAccount(user, driver);
                Thread.Sleep(5000);

                if (fbAccountCreated)
                {
                    resultDto.Status = UserStatus.AccountCreated;
                    resultDto.Cookie = JsonConvert.SerializeObject(driver.Manage().Cookies.AllCookies);
                }
                else
                {
                    resultDto.Status = UserStatus.Erroneous;
                }
            }
            // Return updated user
            return resultDto;
        }

        private static ChromeDriver createChromeDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("ignore-certificate-errors");
            options.AddArguments("--disable-extensions");
            options.AddArguments("--disable-bundled-ppapi-flash");
            options.AddArguments("--disable-plugins-discovery");
            Proxy proxy = new Proxy
            {
                Kind = ProxyKind.Manual,
                IsAutoDetect = false,
                HttpProxy = "127.0.0.1:24000",
                SslProxy = "127.0.0.1:24000"
            };
            options.Proxy = proxy;

            return new ChromeDriver(options);
        }

        private static bool registerFacebookAccount(ImaginaryUserDto user, ChromeDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.facebook.com/");
            Console.WriteLine("Facebooka gidiliyor.");
            Console.WriteLine(driver.Url);
            var name = driver.FindElement(By.Name("firstname"));
            sendSlowKey(name, user.Name);

            var surname = driver.FindElement(By.Name("lastname"));
            sendSlowKey(surname, user.Surname);

            var email = driver.FindElement(By.Name("reg_email__"));
            sendSlowKey(email, user.Email);

            var reemail = driver.FindElement(By.Name("reg_email_confirmation__"));
            sendSlowKey(reemail, user.Email);

            var pass = driver.FindElement(By.Name("reg_passwd__"));
            sendSlowKey(pass, user.Password);

            var dayd = driver.FindElement(By.Id("day"));
            var monthd = driver.FindElement(By.Id("month"));
            var yeard = driver.FindElement(By.Id("year"));

            var selectDay = new SelectElement(dayd);
            selectDay.SelectByValue(user.BirthDate.Day.ToString());
            Thread.Sleep(500);

            var selectMonth = new SelectElement(monthd);
            selectMonth.SelectByValue(user.BirthDate.Month.ToString());
            Thread.Sleep(500);

            var selectYear = new SelectElement(yeard);
            selectYear.SelectByValue(user.BirthDate.Year.ToString());
            Thread.Sleep(500);

            var sex = driver.FindElement(By.Name("sex"));
            sex.Click();
            Thread.Sleep(500);

            var submit = driver.FindElement(By.Name("websubmit"));
            submit.Click();
            Console.WriteLine("Hesap Açılıyor.");

            try
            {
                if (!driver.FindElement(By.Id("reg_error_inner")).GetAttribute("value").Contains("hata"))
                {
                    return false;
                }
            }
            catch (Exception) { /* No exception, process will continue*/ }

            Thread.Sleep(5000);
            if (!driver.Url.Contains("checkpoint") && !driver.Url.Equals("https://www.facebook.com/"))
            {
                Thread.Sleep(3000);
                return connectTexasHoldem(driver, user);
            }
            return false;
        }

        private static string getIpAddress(ChromeDriver driver)
        {
            try
            {
                driver.Navigate().GoToUrl("http://ip-api.com/json?fields=query");
                var jsonResult = driver.FindElement(By.TagName("body")).Text;
                var ipInfo = JsonConvert.DeserializeObject<IpInfo>(jsonResult);
                return ipInfo.query;
            }
            catch
            {
                driver.Navigate().GoToUrl("https://ipapi.com/?utm_source=adwords&utm_medium=cpc&utm_campaign=adwords&utm_term=ip%20api&gclid=Cj0KCQiAw5_fBRCSARIsAGodhk9awZ9Ld2hETPM-_UjsbdnpWYJM3OIa_vyOjY0SScnUe_4Ikm0yi80aAiv3EALw_wcB");
                return driver.FindElement(By.Name("ip_to_lookup")).GetAttribute("value");
            }

        }

        private static void sendSlowKey(IWebElement inputElement, string text)
        {
            foreach (var key in text)
            {
                inputElement.SendKeys(key.ToString());
                Thread.Sleep(75);
            }
        }

        private static bool connectTexasHoldem(ChromeDriver driver, ImaginaryUserDto user)
        {
            driver.FindElement(By.Name("password")).SendKeys(user.Password);
            driver.FindElement(By.XPath("//button[@class='_42ft _4jy0 _4jy4 _4jy1 selected _51sy']")).Click();
            Thread.Sleep(10000);
            driver.Navigate().GoToUrl("https://www.facebook.com/login.php?skip_api_login=1&api_key=2389801228&signed_next=1&next=https%3A%2F%2Fwww.facebook.com%2Fv2.9%2Fdialog%2Foauth%3Fredirect_uri%3Dhttps%253A%252F%252Fapps.facebook.com%252Ftexas_holdem%252F%253Ffb_source%253Dbookmark%2526ref%253Dbookmarks%2526count%253D0%2526fb_bmpos%253D_0%26state%3D7915abdffb671c4d225e94768284355c%26scope%3Demail%252Cuser_friends%26client_id%3D2389801228%26ret%3Dlogin%26logger_id%3D3cc88ac4-6e84-0d29-976f-63b299fc6acb&cancel_url=https%3A%2F%2Fapps.facebook.com%2Ftexas_holdem%2F%3Ffb_source%3Dbookmark%26ref%3Dbookmarks%26count%3D0%26fb_bmpos%3D_0%26error%3Daccess_denied%26error_code%3D200%26error_description%3DPermissions%2Berror%26error_reason%3Duser_denied%26state%3D7915abdffb671c4d225e94768284355c%23_%3D_&display=page&locale=tr_TR&logger_id=3cc88ac4-6e84-0d29-976f-63b299fc6acb");
            Thread.Sleep(10000);
            if (!driver.Url.Contains("checkpoint"))
            {
                var GameSubmit = driver.FindElement(By.Name("__CONFIRM__"));
                GameSubmit.Click();
                Thread.Sleep(5000);
                return true;
            }

            return false;
        }
    }
}
