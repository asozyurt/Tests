using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using HeadlessBrowser.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace HeadlessBrowser.TestRunner
{
    [TestClass]
    public class TestRunner
    {
        [TestMethod]
        public void TestMethod1()
        {
            // GetNames

           // var allNames = Service.HeadlessBrowserServices.GetAllNames();

            // GetSurnames
           // var allSurNames = Service.HeadlessBrowserServices.GetAllSurnames();

            // CreateUser

            // Create Mail

            // Create Fb Account

            // Export As
        }

        [TestMethod]
        public string TestCreateMailAddress()
        {

            var nameArray = ((List<string>)Service.NameServices.GetAllNames().Result).ToArray();
            var surnameArray = ((List<string>)Service.SurnameServices.GetAllSurnames().Result).ToArray();
            string[] uzanti = { "@mg.q-bitgame.com", "@mg.clubangel.site" };
            Random r = new Random();

            string mailAddress = randomString(10) + uzanti[r.Next(uzanti.Length)];
            string mailLink = "http://yenimail.skindraws.com/api/mail/show/" + mailAddress;

            //https://www.facebook.com/login.php?skip_api_login=1&api_key=2389801228&signed_next=1&next=https%3A%2F%2Fwww.facebook.com%2Fv2.9%2Fdialog%2Foauth%3Fredirect_uri%3Dhttps%253A%252F%252Fapps.facebook.com%252Ftexas_holdem%252F%253Ffb_source%253Dbookmark%2526ref%253Dbookmarks%2526count%253D0%2526fb_bmpos%253D_0%26state%3D7915abdffb671c4d225e94768284355c%26scope%3Demail%252Cuser_friends%26client_id%3D2389801228%26ret%3Dlogin%26logger_id%3D3cc88ac4-6e84-0d29-976f-63b299fc6acb&cancel_url=https%3A%2F%2Fapps.facebook.com%2Ftexas_holdem%2F%3Ffb_source%3Dbookmark%26ref%3Dbookmarks%26count%3D0%26fb_bmpos%3D_0%26error%3Daccess_denied%26error_code%3D200%26error_description%3DPermissions%2Berror%26error_reason%3Duser_denied%26state%3D7915abdffb671c4d225e94768284355c%23_%3D_&display=page&locale=tr_TR&logger_id=3cc88ac4-6e84-0d29-976f-63b299fc6acb


            var options = new PhantomJSOptions();
            string userAgent = "Mozilla/5.0 (Linux; Android 4.4.2; en-us; SAMSUNG SM-G900T Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Version/1.6 Chrome/28.0.1500.94 Mobile Safari/537.36";

            options.AddAdditionalCapability("phantomjs.page.settings.userAgent", userAgent);

            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.AddArgument(string.Format("--proxy-auth={0}:{1}", Constants.PROXY_USER, Constants.PROXY_USER_PASSWORD));
            service.AddArgument(string.Format("--proxy={0}:{1}", "127.0.0.1", 24000));
            service.IgnoreSslErrors = true;
            service.LoadImages = false;
            PhantomJSDriver browser = new PhantomJSDriver(service, options);
            browser.Manage().Cookies.DeleteAllCookies();
            browser.Navigate().GoToUrl("https://api.ipify.org/?format=json");
            string source = browser.PageSource;
            string ipAddress = source.Substring(91, 18);
            ipAddress = ipAddress.Split('"')[0];

            Console.WriteLine(ipAddress);

            browser.Navigate().GoToUrl("https://touch.facebook.com/reg/");
            Console.WriteLine("Facebooka gidiliyor.");
            Thread.Sleep(8000);
            Console.WriteLine(browser.Url);
            var name = browser.FindElement(By.Name("firstname"));
            name.SendKeys(nameArray[r.Next(nameArray.Length)]);

            var surname = browser.FindElement(By.Name("lastname"));
            surname.SendKeys(surnameArray[r.Next(surnameArray.Length)]);

            var ileri = browser.FindElement(By.XPath("//button[@type='submit']"));
            ileri.Click();

            var dayd = browser.FindElement(By.Id("day"));
            var monthd = browser.FindElement(By.Id("month"));
            var yeard = browser.FindElement(By.Id("year"));


            var selectDay = new SelectElement(dayd);
            selectDay.SelectByValue(r.Next(1, 20).ToString());

            var selectMonth = new SelectElement(monthd);
            selectMonth.SelectByValue(r.Next(1, 10).ToString());

            var selectYear = new SelectElement(yeard);
            selectYear.SelectByValue(r.Next(1980, 1990).ToString());

            var ileri1 = browser.FindElement(By.XPath("//button[@type='submit']"));
            ileri1.Click();

            var switchEposta = browser.FindElement(By.XPath("//a[@data-sigil='switch_phone_to_email']"));
            switchEposta.Click();

            var email = browser.FindElement(By.Name("reg_email__"));
            email.SendKeys(mailAddress);

            var ileri2 = browser.FindElement(By.XPath("//button[@type='submit']"));
            ileri2.Click();

            var sex = browser.FindElement(By.Name("sex"));
            sex.Click();

            var ileri3 = browser.FindElement(By.XPath("//button[@type='submit']"));
            ileri3.Click();

            var pass = browser.FindElement(By.Name("reg_passwd__"));
            pass.SendKeys("987654ab");

            var submit = browser.FindElement(By.XPath("//button[@name='submit']"));
            submit.Click();
            Console.WriteLine("Hesap Açılıyor.");

            return "a";
        }
        public static string randomString(int len)
        {
            string EmailRandom = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random r = new Random();
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                sb.Append(EmailRandom[r.Next(EmailRandom.Length)]);
            }
            return sb.ToString();
        }
    }
}
