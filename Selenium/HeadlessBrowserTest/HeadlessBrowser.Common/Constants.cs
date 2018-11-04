using HeadlessBrowser.Common.Utility;

namespace HeadlessBrowser.Common
{
    public class Constants
    {
        public const string PROXY_USER_CONFIG_KEY = "PROXY_USER";
        public const string PROXY_USER_PASSWORD_CONFIG_KEY = "PROXY_USER_PASSWORD";

        private static string _proxyUser;
        public static string PROXY_USER
        {
            get
            {
                _proxyUser = _proxyUser ?? Configuration.Get(PROXY_USER_CONFIG_KEY);
                return _proxyUser;
            }
        }

        private static string _proxyUserPassword;
        public static string PROXY_USER_PASSWORD
        {
            get
            {
                _proxyUserPassword = _proxyUserPassword ?? Configuration.Get(PROXY_USER_PASSWORD_CONFIG_KEY);
                return _proxyUserPassword;
            }
        }
    }
}
