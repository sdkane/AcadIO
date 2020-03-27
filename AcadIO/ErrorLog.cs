using System.Runtime.CompilerServices;

namespace AcadIO {
    class Err {        
        public static void Log(System.Exception ex, [CallerMemberName]string memberName = "") {
            try {
                if (Flags.EnableErrorLogging == false) { return; }
                string message = memberName + "  :  " + ex.Message + "  :  " + ex.StackTrace;
                System.IO.File.AppendAllText(System.Environment.SpecialFolder.LocalApplicationData.ToString() + "\\Temp\\AcadIO.log", 
                                             System.DateTime.Now.ToString() + "  :  " + message + System.Environment.NewLine);
            }
            catch { }
        }
    }
}