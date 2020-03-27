using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO {
    public class IOEvents : Autodesk.AutoCAD.Runtime.IExtensionApplication {
        public void Initialize() {
            try {
                if (Application.DocumentManager.MdiActiveDocument != null && Application.DocumentManager.MdiActiveDocument.IsActive == true) {
                    World.Docu = Application.DocumentManager.MdiActiveDocument;
                    World.Docu.Editor.SelectionAdded += new Autodesk.AutoCAD.EditorInput.SelectionAddedEventHandler(ObjectSelected);
                }
                
                Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(DrawingChanged);  //load event to detect when the user changes a drawing

                AcadIO.ContextMenu.EmptySelection.ApplicationMenu.Attach(); //load right click context menu

                AcadIO.World.AcadIoLoaded = true;
            }
            catch (System.Exception ex) {
                Err.Log(ex);
            }
        }
        public void Terminate() { }

        public static void DrawingChanged(object sender, Autodesk.AutoCAD.ApplicationServices.DocumentCollectionEventArgs e) {
            try {
                if (AcadIO.World.AcadIoLoaded == true) {
                    if (AcadIO.World.AcadIoDockVisible == true) {
                        if (Application.DocumentManager.MdiActiveDocument != null) {
                            World.Docu = Application.DocumentManager.MdiActiveDocument;    //setting the global active document

                            //unable to check whether the drawing that is activated already has a handler, remove if it exists (or doesnt, it wont error), then re-add                    
                            World.Docu.Editor.SelectionAdded -= new Autodesk.AutoCAD.EditorInput.SelectionAddedEventHandler(ObjectSelected);
                            World.Docu.Editor.SelectionAdded += new Autodesk.AutoCAD.EditorInput.SelectionAddedEventHandler(ObjectSelected);
                        }
                    }
                }
            }
            catch (System.Exception ex) { Err.Log(ex); }
        }

        public static void LoadCurrentDrawing() {
            try {
                if (AcadIO.World.AcadIoLoaded == true) {
                    if (AcadIO.World.AcadIoDockVisible == true) {
                        if (Application.DocumentManager.MdiActiveDocument != null) {
                            World.Docu = Application.DocumentManager.MdiActiveDocument;    //setting the global active document

                            //unable to check whether the drawing that is activated already has a handler, remove if it exists, then re-add                    
                            World.Docu.Editor.SelectionAdded -= new Autodesk.AutoCAD.EditorInput.SelectionAddedEventHandler(ObjectSelected);
                            World.Docu.Editor.SelectionAdded += new Autodesk.AutoCAD.EditorInput.SelectionAddedEventHandler(ObjectSelected);
                        }
                    }
                }
            }
            catch (System.Exception ex) { Err.Log(ex); }
        }

        private static bool _processing = false;
        public static void ObjectSelected(object sender, EventArgs e) {
            try {
                if (AcadIO.World.AcadIoDockVisible == true) {
                    if (_processing == false) {
                        _processing = true;
                        PromptSelectionResult promptRes = World.Docu.Editor.SelectImplied();
                        if (promptRes.Status == PromptStatus.OK) {
                            if (promptRes.Value != null & promptRes.Value.Count == 1) {
                                foreach (ObjectId id in promptRes.Value.GetObjectIds()) {
                                    AcadIO.Dock.Menu.SelectedId = id;
                                    goto EndSelectedItem;                                   
                                }
                            }
                        }
                    }
                }
                EndSelectedItem:;
                _processing = false;
            }
            catch (System.Exception ex) {
                Err.Log(ex);
            }
        }
    }
}