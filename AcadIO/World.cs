using System;
using System.IO;
using System.Reflection;

namespace AcadIO {
    public class World {
        public static string InstallPath = GetInstallPath();
        private static string GetInstallPath() {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            return Path.GetFullPath(Path.Combine(path, "..", "..")); 
        }

        public static bool AcadIoLoaded = false;
        public static bool AcadIoDockVisible = false;

        public static Autodesk.AutoCAD.ApplicationServices.Document Docu;
        public static double UnitFactor = 1;

        public static bool IsNumeric(object a) {
            return Double.TryParse(Convert.ToString(a), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out double retnum);
        }
    }
}
