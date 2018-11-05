namespace HeadlessBrowser.Common
{
    public class UserContext
    {
        public static string CurrentUser { get; private set; }

        public static void SetContext(string user)
        {
            CurrentUser = user;
        }
    }
}
