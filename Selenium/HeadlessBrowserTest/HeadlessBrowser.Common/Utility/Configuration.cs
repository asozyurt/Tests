using System.Configuration;

namespace HeadlessBrowser.Common.Utility
{
    public class Configuration
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
