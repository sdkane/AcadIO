using System;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace AcadIO.Commands {
    //you can place any function you like with as many command method calls as you want
    public class Select {
        [CommandMethod("Select")]        
        public static void UserSelect() {
            PromptEntityResult acadSelection = default(PromptEntityResult);
            PromptEntityOptions opts = new PromptEntityOptions(Environment.NewLine + "Select Object Reference: ");
            using (World.Docu.LockDocument()) {
                acadSelection = World.Docu.Editor.GetEntity(opts);
                if (acadSelection.Status == PromptStatus.OK) {
                    AcadIO.Dock.Menu.SelectedId = acadSelection.ObjectId;
                }
            }
        }
    }
}
