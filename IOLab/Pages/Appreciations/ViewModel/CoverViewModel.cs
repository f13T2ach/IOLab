namespace IOLab.Pages.Appreciations.ViewModel
{
    internal class CoverViewModel
    {
        private static int _ProgressValue;
        public static int ProgressValue
        {
            get
            {
                return _ProgressValue;
            }
            set
            {
                _ProgressValue = value;
            }
        }

        public static int HalfProgressValue
        {
            get
            {
                return (int)(_ProgressValue / 2);
            }
        }
    }
}
