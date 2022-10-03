using System.ComponentModel.DataAnnotations;

namespace HealthApp.Utils
{
    public static class AppEnum
    {
        public enum App
        {
            [Display(Name = "Authors", ResourceType = typeof(Resources.Language.Resource))]
            Authors,
            [Display(Name = "Categories", ResourceType = typeof(Resources.Language.Resource))]
            Categories,
        }
    }

    public static class BehaviorState
    {
        public enum EmailState
        {
            [Display(Name = "ErrorCorrectEmail", ResourceType = typeof(Resources.Language.Resource))]
            Error,
            Success,
            None
        }

        public enum PasswordState
        {
            [Display(Name = "ErrorCorrectPassword", ResourceType = typeof(Resources.Language.Resource))]
            Error,
            [Display(Name = "ErrorConfirmedPassword", ResourceType = typeof(Resources.Language.Resource))]
            ConfirmedError,
            Success,
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
            [Display(Name = "ErrorCorrectPassword", ResourceType = typeof(Resources.Language.Resource))]
            CorrectPassword,
            [Display(Name = "ErrorEmptyEntry", ResourceType = typeof(Resources.Language.Resource))]
            EmptyEntry,
            [Display(Name = "ErrorCorrectEmail", ResourceType = typeof(Resources.Language.Resource))]
            CorrectEmail,
            [Display(Name = "ErrorServer", ResourceType = typeof(Resources.Language.Resource))]
            Server
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
}