namespace IOLab.Util
{
    class LaTeXImage
    {
        //https://latex.codecogs.com/png.download?\huge\fg{FFFFFF}%20\ce{Ba^2+%20+%20SO4^2-%20=%20BaSO4%20v}
        static string LatexURL = @"https://latex.codecogs.com/png.download?\huge";

        public static string GetRequestUrl(bool isDark, string content)
        {
            string url = LatexURL + @"\fg{" + (isDark ? "FFFFFF" : "000000") + "}%20" + content;
            return url;
        }
    }
}
