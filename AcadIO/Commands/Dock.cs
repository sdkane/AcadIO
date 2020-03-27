using Autodesk.AutoCAD.Runtime;

namespace AcadIO.Commands {
    //you can place any function you like with as many command method calls as you want
    public class Dock {
        [CommandMethod("LaunchDock")]
        [CommandMethod("LaunchAcadIO")]
        public static void Launch() {
            AcadIO.Dock.Menu.LaunchDock();
        }
    }
}
