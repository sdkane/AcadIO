using System;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO.Dock {
    public partial class Menu : UserControl {
        static internal Autodesk.AutoCAD.Windows.PaletteSet DockPalette = null;
        static public Menu DockRef = new Menu();

        static public ObjectId SelectedId = new ObjectId();

        public Menu() {
            InitializeComponent();
        }

        public static void LaunchDock() {
            if (DockPalette == null) {
                DockPalette = new Autodesk.AutoCAD.Windows.PaletteSet("AcadIO Dock");
                DockPalette.AddVisual("AcadIODock", DockRef);
            }

            DockPalette.MinimumSize = new Size(580, 440); //undocked size
            DockPalette.Visible = true;            

            DockPalette.Dock = Autodesk.AutoCAD.Windows.DockSides.None;
            DockPalette.DockEnabled = Autodesk.AutoCAD.Windows.DockSides.None;
            DockPalette.Location = new Point(100, 100);

            //manually refresh the window to make it appear undocked - acad quirk
            DockPalette.Visible = false;
            DockPalette.Visible = true;


            AcadIO.IOEvents.LoadCurrentDrawing();
            World.AcadIoDockVisible = true;
            
            //
            //everything below here is for the sake of the demo
            //
            DockRef.Instructions.Text = "Use Insert to insert a new block." + System.Environment.NewLine +
                                        "Select the block so the object selected event fires, or use Select Object to manually select the block." + System.Environment.NewLine +
                                        "Retrieve the value (record/property/attribute) associated with the key" + System.Environment.NewLine +
                                        "Rectangle block has " + System.Environment.NewLine +
                                        "properties: length, width, flip" + System.Environment.NewLine +
                                        "records: name" + System.Environment.NewLine +
                                        "attribute: displayname" + System.Environment.NewLine + 
                                        "flip property must be set as 0 or 1";

            DockRef.BlockPath.Text = World.InstallPath + "\\_DemoObjects";
            DockRef.BlockName.Text = "demoRectangle";

            DockRef.BlockRecordKey.Text = "name";
            DockRef.BlockRecordValue.Text = "";

            DockRef.BlockPropertyKey.ItemsSource = new List<string> { "length", "width" };
            DockRef.BlockPropertyValue.Text = "";

            DockRef.BlockFlipPropertyKey.Text = "flip";
            DockRef.BlockFlipPropertyKey.IsEnabled = false;
            DockRef.BlockFlipPropertyValue.Text = "";

            DockRef.BlockAttributeKey.Text = "displayname";
            DockRef.BlockAttributeKey.IsEnabled = false;
            DockRef.BlockAttributeValue.Text = "";

            DockRef.NODRecordKey.Text = "acadio";
            DockRef.NODRecordValue.Text = "";
        }

        private void BlockInsertButton_Click(object sender, System.Windows.RoutedEventArgs e) {            
            Autodesk.AutoCAD.Internal.Utils.SetUndoMark(true); //to make a set of commands a single undo listing wrap them like this at the highest level, you cannot nest this

            SelectedId = AcadIO.Insert.Block.Insert(BlockPath.Text, BlockName.Text);
            AcadIO.Block.Record.Set(SelectedId, BlockRecordKey.Text, "I am the demo");
            
            Autodesk.AutoCAD.Internal.Utils.SetUndoMark(false); //end the undo wrapping when you are done working with acad
        }

        private void BlockSelectButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            AcadIO.Commands.Select.UserSelect();          
        }

        private void BlockRecordRetrieveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockRecordKey.Text != null) {
                BlockRecordValue.Text = AcadIO.Block.Record.Get<string>(SelectedId, BlockRecordKey.Text);
            }
        }

        private void BlockRecordSetButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockRecordValue.Text != null && BlockRecordKey.Text != null) {
                AcadIO.Block.Record.Set(SelectedId, BlockRecordKey.Text, BlockRecordValue.Text);
                BlockRecordValue.Text = default(string);
            }
        }

        private void BlockPropertyRetrieveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockPropertyKey.SelectedItem.ToString() != null) {
                BlockPropertyValue.Text = Convert.ToString(AcadIO.Block.Property.Get(SelectedId, BlockPropertyKey.SelectedItem.ToString()));
            }
        }

        private void BlockPropertySetButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockPropertyValue.Text != null && BlockPropertyKey.SelectedItem.ToString() != null) {
                AcadIO.Block.Property.Set(SelectedId, BlockPropertyKey.SelectedItem.ToString(), Convert.ToDouble(BlockPropertyValue.Text));
                BlockPropertyValue.Text = default(string);
            }
        }

        private void BlockFlipPropertyRetrieveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockFlipPropertyKey.Text != null) {
                BlockFlipPropertyValue.Text = Convert.ToString(AcadIO.Block.Property.Get(SelectedId, BlockFlipPropertyKey.Text));
            }
        }

        private void BlockFlipPropertySetButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockFlipPropertyKey.Text != null && BlockFlipPropertyKey.Text != null) {
                AcadIO.Block.Property.SetFlip(SelectedId, BlockFlipPropertyKey.Text, Convert.ToInt32(BlockFlipPropertyValue.Text));
                BlockFlipPropertyValue.Text = default(string);
            }
        }

        private void BlockAttributeRetrieveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockAttributeKey.Text != null) {
                BlockAttributeValue.Text = AcadIO.Block.Attribute.Get(SelectedId, BlockAttributeKey.Text);
            }
        }

        private void BlockAttributeSetButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (BlockAttributeValue.Text != null && BlockAttributeKey.Text != null) {
                AcadIO.Block.Attribute.Set(SelectedId, BlockAttributeKey.Text, BlockAttributeValue.Text);
                BlockAttributeValue.Text = default(string);
            }
        }

        private void NODRecordRetrieveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (NODRecordKey.Text != null) {
                NODRecordValue.Text = AcadIO.NOD.Record.Get<string>(NODRecordKey.Text);
            }
        }

        private void NODRecordSetButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (NODRecordValue.Text != null && NODRecordKey.Text != null) {
                AcadIO.NOD.Record.Set(NODRecordKey.Text, NODRecordValue.Text);
                NODRecordValue.Text = default(string);
            }
        }
    }
}
