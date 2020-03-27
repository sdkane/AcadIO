using System;
using System.Windows.Media.Imaging;

using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;

namespace AcadIO.Ribbon {
    public class IORibbon : Autodesk.AutoCAD.Runtime.IExtensionApplication {
        public void Initialize() {
            AddRibbon();
        }
        public void Terminate() {
            RemoveTab();
        }
        public void RemoveTab() {
            Autodesk.Windows.RibbonControl newtab = Autodesk.Windows.ComponentManager.Ribbon;
            newtab.Tabs.Remove(AcacIOTab);
        }

        static internal RibbonTab AcacIOTab = new RibbonTab();

        [CommandMethod("AcadIORibbonLoad")]
        public static void AddRibbon() {
            if (Convert.ToInt32(Autodesk.AutoCAD.ApplicationServices.Application.TryGetSystemVariable("RIBBONSTATE")) == 0) { return; } //ribbonstate returns 0 if the ribbon is closed based on user settings

            foreach (RibbonTab tempTab in ComponentManager.Ribbon.Tabs) {
                if (tempTab.Id == "AcadIOTab") { return; }
            }

            RibbonControl newTab = ComponentManager.Ribbon;
            AcacIOTab.Title = "AcadIO";
            AcacIOTab.Id = "AcadIOTab";
            newTab.Tabs.Add(AcacIOTab);

            RibbonPanelSource panelSource1 = new RibbonPanelSource {
                Title = "Dock"
            };
            RibbonPanel panel1 = new RibbonPanel {
                Source = panelSource1
            };
            AcacIOTab.Panels.Add(panel1);
            RibbonRowPanel RibbonRow1 = new RibbonRowPanel();

            RibbonRow1.Items.Add(new RibbonSeparator());
            RibbonRow1.Items.Add(AddButton("Launch Dock Option 1", "LaunchDock", "Launch the dock using the first command string", World.InstallPath + "\\_DemoObjects\\" + "dock.png"));
            RibbonRow1.Items.Add(new RibbonSeparator());
            RibbonRow1.Items.Add(AddButton("Launch Dock Option 2", "LaunchAcadIO", "Launch the dock using the second command string", World.InstallPath + "\\_DemoObjects\\" + "dock.png"));
            panelSource1.Items.Add(RibbonRow1);

            RibbonPanelSource panelSource2 = new RibbonPanelSource {
                Title = "Select"
            };
            RibbonPanel panel2 = new RibbonPanel {
                Source = panelSource2
            };
            AcacIOTab.Panels.Add(panel2);
            RibbonRowPanel RibbonRow2 = new RibbonRowPanel();
            RibbonRow2.Items.Add(new RibbonSeparator());
            RibbonRow2.Items.Add(AddButton("Select", "", "Select a single object to edit", World.InstallPath + "\\_DemoObjects\\" + "select.png"));
            panelSource2.Items.Add(RibbonRow2);

            AcacIOTab.IsActive = true;
            AcacIOTab.IsAnonymous = true;
        }

        public static RibbonButton AddButton(string text, string id, string tooltip, string picture) {
            RibbonButton button = new RibbonButton {
                Text = text,
                Size = RibbonItemSize.Large
            };

            if (!string.IsNullOrEmpty(picture)) { button.LargeImage = GetBitmap(picture); }
            button.ShowImage = true;
            button.ShowText = true;
            button.Id = id;
            button.ToolTip = tooltip;
            button.Orientation = System.Windows.Controls.Orientation.Vertical;
            button.CommandHandler = new RibbonCommandHandler();
            return button;
        }

        public static BitmapImage GetBitmap(string filename) {
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

    }

    public class RibbonCommandHandler : System.Windows.Input.ICommand {
        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged {
            add { }
            remove { }
        }

        public void Execute(object parameter) {
            if (parameter is RibbonButton) {
                RibbonButton button = parameter as RibbonButton;
                string cmdStr = "._" + button.Id + " ";
                try {
                    World.Docu.SendStringToExecute(cmdStr, true, false, false);
                }
                catch (System.Exception ex) {
                    Err.Log(ex);
                }
            }
        }
    }
}