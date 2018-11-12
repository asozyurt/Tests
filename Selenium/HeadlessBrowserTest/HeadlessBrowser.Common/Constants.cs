using HeadlessBrowser.Common.Utility;

namespace HeadlessBrowser.Common
{
    public class Constants
    {
        public const int STATUS_CHECK_INTERVAL_IN_MINUTES = 5;
        public const int EXPORT_THRESHOLD_COUNT = 250;
        public const int FIVE_SECONDS_IN_MILLISECOND = 5000;
        public const string PROXY_USER_CONFIG_KEY = "PROXY_USER";
        public const string CURRENT_USER_CONFIG_KEY = "CURRENT_USER";
        public const string MODE_CONFIG_KEY = "MODE";
        public const string PROXY_USER_PASSWORD_CONFIG_KEY = "PROXY_USER_PASSWORD";
        public const string MODE_YANDEX = "YANDEX";
        public const string MODE_FACEBOOK = "FACEBOOK";
        public const int MAX_READ_COUNT = 250;
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

        private static string _mode;
        public static string MODE
        {
            get
            {
                _mode = _mode ?? Configuration.Get(MODE_CONFIG_KEY);
                return _mode;
            }
        }

        private static string _currentUser;
        public static string CURRENT_USER
        {
            get
            {
                _currentUser = _currentUser ?? Configuration.Get(CURRENT_USER_CONFIG_KEY);
                return _currentUser;
            }
        }

    }
}
