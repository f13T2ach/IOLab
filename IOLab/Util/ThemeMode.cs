namespace IOLab.Util
{
    class ThemeMode
    {
        public static bool GetSystemDarkMode()
        {
            const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string RegistryValueName = "AppsUseLightTheme";
            object? registryValueObject = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryKeyPath)?.GetValue(RegistryValueName);
            if (registryValueObject is null) return false;
            return (int)registryValueObject > 0 ? false : true;
        }

    }
}
