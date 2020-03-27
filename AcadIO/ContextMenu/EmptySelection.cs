using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;

namespace AcadIO.ContextMenu {
    class EmptySelection : IExtensionApplication {
        public void Initialize() { ApplicationMenu.Attach(); }
        public void Terminate() { ApplicationMenu.Detach(); }
        public static void AttachContextMenu() { ApplicationMenu.Attach(); }
        public static void DetachContextMenu() { ApplicationMenu.Detach(); }

        public class ApplicationMenu {
            private static ContextMenuExtension ConMenExt;
            public static void Attach() {
                ConMenExt = new ContextMenuExtension {
                    Title = "AcadIO"
                };
                MenuItem menuSelect = new MenuItem("Select");
                menuSelect.Click += new EventHandler(MenuSelectClick);
                string filename = World.InstallPath + "\\_DemoObjects\\" + "select.ico";
                Icon ic = new Icon(filename);
                menuSelect.Icon = ic;
                menuSelect.Checked = false;
                ConMenExt.MenuItems.Add(menuSelect);
                Application.AddDefaultContextMenuExtension(ConMenExt);

                ConMenExt.Title = "Launch Dock";
                Autodesk.AutoCAD.Windows.MenuItem menuLaunchDock = new Autodesk.AutoCAD.Windows.MenuItem("Launch Dock");
                menuLaunchDock.Click += new EventHandler(MenuLaunchDock);
                filename = World.InstallPath + "\\_DemoObjects\\" + "dock.ico";
                ic = new Icon(filename);
                menuLaunchDock.Icon = ic;
                ConMenExt.MenuItems.Add(menuLaunchDock);
            }

            private BitmapImage GetBitmap(ref System.Exception ex, string filename) {
                BitmapImage bmp = new BitmapImage();
                try {
                    string path = filename;
                    bmp.BeginInit();
                    bmp.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                    bmp.EndInit();
                }
                catch {
                }
                return bmp;
            }

            public static void Detach() { Application.RemoveDefaultContextMenuExtension(ConMenExt); }

            private static void MenuSelectClick(object sender, EventArgs e) {
                AcadIO.Commands.Select.UserSelect();
            }

            private static void MenuLaunchDock(object sender, EventArgs e) {
                AcadIO.Commands.Dock.Launch();
            }
        }
    }
}