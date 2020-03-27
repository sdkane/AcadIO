using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO {
    public class Layers {
        /// <summary>checks whether the object is on a locked layer</summary>      
        public static bool IsObjectLayerLocked(ObjectId id) {
            using (Database db = World.Docu.Database) {
                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    if (tr.GetObject(id, OpenMode.ForRead) as BlockReference != null) {
                        using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForRead, false, false)) {
                            using (LayerTableRecord acadLayerRecord = (LayerTableRecord)tr.GetObject(br.LayerId, OpenMode.ForRead)) {
                                if (acadLayerRecord.IsLocked == true) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsActiveLayerLocked() {
            using (Database db = World.Docu.Database) {
                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    using (LayerTableRecord acadLayerRecord = (LayerTableRecord)tr.GetObject(db.Clayer, OpenMode.ForRead)) {
                        if (acadLayerRecord.IsLocked == true) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>unlocks all layers in the drawing, returns list of layers that were unlocked</summary>      
        public static ObjectIdCollection UnlockAllLayers() {
            ObjectIdCollection idCollection = new ObjectIdCollection();
            using (World.Docu.LockDocument()) {
                using (Database db = World.Docu.Database) {
                    using (Transaction tr = db.TransactionManager.StartTransaction()) {
                        using (LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForWrite)) {
                            foreach (ObjectId id in lt) {
                                using (LayerTableRecord lr = (LayerTableRecord)tr.GetObject(id, OpenMode.ForWrite)) {
                                    if (lr.IsLocked == true) {
                                        idCollection.Add(id);
                                        lr.IsLocked = false;
                                    }
                                }
                            }
                        }
                        tr.Commit();
                    }
                }
            }
            return idCollection;
        }

        /// <summary>locks layers identified by id in the objectidcollection</summary>      
        public static void LockLayerCollection(ObjectIdCollection idCollection) {
            using (World.Docu.LockDocument()) {
                using (Database db = World.Docu.Database) {
                    using (Transaction tr = db.TransactionManager.StartTransaction()) {
                        using (LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForWrite)) {
                            foreach (ObjectId id in lt) {
                                if (idCollection.Contains(id)) {
                                    using (LayerTableRecord lr = (LayerTableRecord)tr.GetObject(id, OpenMode.ForWrite)) {
                                        lr.IsLocked = true;
                                    }
                                }
                            }
                        }
                        tr.Commit();
                    }
                }
            }
        }
    }
}
