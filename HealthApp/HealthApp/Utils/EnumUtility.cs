using System.ComponentModel.DataAnnotations;

namespace HealthApp.Utils
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

    public class MessageEnum
    {
        public enum Error
        {
            [Display(Name = "ErrorNetwork", ResourceType = typeof(Resources.Language.Resource))]
            Network,
            [Display(Name = "ErrorLogin", ResourceType = typeof(Resources.Language.Resource))]
            Login,
            [Display(Name = "ErrorRegister", ResourceType = typeof(Resources.Language.Resource))]
            Register,
            [Display(Name = "ErrorConfirmedPassword", ResourceType = typeof(Resources.Language.Resource))]
            ConfirmedPassword,
            [Display(Name = "ErrorEmptyEntry", ResourceType = typeof(Resources.Language.Resource))]
            EmptyEntry,
            [Display(Name = "ErrorCorrectEmail", ResourceType = typeof(Resources.Language.Resource))]
            CorrectEmail
        }

        public enum Button
        {
            [Display(Name = "ButtonOK", ResourceType = typeof(Resources.Language.Resource))]
            OK
        }
    }

    public class AnimationEnum
    {
        public enum AnimationType
        {
            Scale,
            Opacity,
            TranslationX,
            TranslationY,
            Rotation,
            Layout
        }
    }

    public class ThemeEnum
    {
        public enum Theme
        {
            [Display(Name = "LightTheme", ResourceType = typeof(Resources.Language.Resource))]
            LightTheme,
            [Display(Name = "DarkTheme", ResourceType = typeof(Resources.Language.Resource))]
            DarkTheme,
            [Display(Name = "SystemTheme", ResourceType = typeof(Resources.Language.Resource))]
            SystemPreferred
        }
    }

    public class LanguageEnum
    {
        public enum Language
        {
            [Display(Name = "LanguageEnglish", ResourceType = typeof(Resources.Language.Resource))]
            English,
            [Display(Name = "LanguageRussian", ResourceType = typeof(Resources.Language.Resource))]
            Russian
        }
    }
}
