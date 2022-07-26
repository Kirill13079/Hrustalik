using System.ComponentModel.DataAnnotations;

namespace HealthApp.Utils
{
    public static class EnumUtility
    {
        public static class BehaviorState
        {
            public enum EmailEntryState
            {
                [Display(Description = "Entry filled in with an error")]
                Error,
                [Display(Description = "Entry filled in with correctly")]
                Success,
                [Display(Description = "Default state entry")]
                None
            }
        }

        public static class AppSettingsState
        {
            public enum AppPrefrences
            {
                AppTheme,
                User,
                Authors,
                Categories,
                Token
            }

            public enum Theme
            {
                LightTheme,
                DarkTheme,
                SystemPreferred
            }
        }
    }
}
