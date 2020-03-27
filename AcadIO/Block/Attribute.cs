using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace AcadIO.Block {
    public static class Attribute {     
        public static void Set(ObjectId id, string key, string value) {            
            try {
                if (id.IsErased == false & id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForWrite)) {
                                    foreach (ObjectId attributeId in br.AttributeCollection) {
                                        using (AttributeReference attributeRef = (AttributeReference)tr.GetObject(attributeId, OpenMode.ForWrite)) {
                                            if (attributeRef.Tag == key) {
                                                attributeRef.TextString = value;
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
            catch (System.Exception ex) {
                Err.Log(ex);
            }
        }

        public static string Get(ObjectId id, string key) {            
            try {
                if (id.IsErased == false & id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForRead)) {
                                    foreach (ObjectId attributeId in br.AttributeCollection) {
                                        using (AttributeReference attributeRef = (AttributeReference)tr.GetObject(attributeId, OpenMode.ForRead)) {
                                            if (attributeRef.Tag == key) {                                                
                                                return attributeRef.TextString;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Err.Log(ex);
            }
            return default(string);
        }

        public static void Delete(ObjectId id, string key) {
            try {
                if (id.IsErased == false & id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForRead)) {
                                    foreach (ObjectId attributeId in br.AttributeCollection) {
                                        using (AttributeReference attributeRef = (AttributeReference)tr.GetObject(attributeId, OpenMode.ForRead)) {
                                            if (attributeRef.Tag == key) {
                                                attributeRef.Erase();
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
            catch (System.Exception ex) {
                Err.Log(ex);
            }      
        }

        public static void Position(ObjectId id, string key, Point3d position) {
            try {
                if (id.IsErased == false & id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForWrite)) {
                                    foreach (ObjectId attributeId in br.AttributeCollection) {
                                        using (AttributeReference attributeRef = (AttributeReference)tr.GetObject(attributeId, OpenMode.ForWrite)) {
                                            if (attributeRef.Tag == key) {
                                                attributeRef.Position = position;
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
            catch (System.Exception ex) {
                Err.Log(ex);
            }
        }

        public static Point3d GetBlockAttributeLocation(ObjectId id, string key) {
            try {
                if (id.IsErased == false & id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForRead)) {
                                    foreach (ObjectId attributeId in br.AttributeCollection) {
                                        using (AttributeReference attributeRef = (AttributeReference)tr.GetObject(attributeId, OpenMode.ForRead)) {
                                            if (attributeRef.Tag == key) {
                                                return attributeRef.Position;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Err.Log(ex);
            }            
            return new Autodesk.AutoCAD.Geometry.Point3d(0, 0, 0);
        }
    }
}