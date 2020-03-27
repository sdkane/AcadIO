using System;
using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO.Block {
    /// <summary>xrecords are non-volatile key/value pairs this stores them in a block reference</summary>
    public class Record {
        public static void Set(ObjectId id, string key, object value) {            
            try {                
                if (id.IsErased == false & id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {                            
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForWrite)) {
                                    if (br.ExtensionDictionary == ObjectId.Null) {
                                        br.CreateExtensionDictionary();
                                    }
                                    using (DBDictionary dict = (DBDictionary)tr.GetObject(br.ExtensionDictionary, OpenMode.ForWrite, false)) {
                                        DxfCode code = AcadIO.DxfHelper.GetFromObject(value);
                                        if (dict.Contains(key)) {
                                            Xrecord xRec = (Xrecord)tr.GetObject(dict.GetAt(key), OpenMode.ForWrite);
                                            ResultBuffer resBuf = new ResultBuffer(new TypedValue(Convert.ToInt32(code), value));
                                            xRec.Data = resBuf;
                                        }
                                        else {
                                            Xrecord xRec = new Xrecord();
                                            ResultBuffer resBuf = new ResultBuffer(new TypedValue(Convert.ToInt32(code), value));
                                            xRec.Data = resBuf;
                                            dict.SetAt(key, xRec);
                                            tr.AddNewlyCreatedDBObject(xRec, true);
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

        public static T Get<T>(ObjectId id, string key) {
            try {
                if (id.IsErased == false & id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForRead)) {
                                    if (br.ExtensionDictionary == ObjectId.Null) {
                                        return default(T);
                                    }
                                    using (DBDictionary dict = (DBDictionary)tr.GetObject(br.ExtensionDictionary, OpenMode.ForRead, false)) {
                                        if (dict.Contains(key)) {
                                            Xrecord xRec = (Xrecord)tr.GetObject(dict.GetAt(key), OpenMode.ForRead);
                                            ResultBuffer resBuf = xRec.Data;
                                            if (resBuf == null) { return default(T); }
                                            TypedValue[] typ = resBuf.AsArray();
                                            return (T)Convert.ChangeType(typ[0].Value, typeof(T));
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
            return default(T);
        }

        public static void Delete(ObjectId id, string key) {
            try {
                if (id.IsErased == false && id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForWrite)) {
                                    if (br.ExtensionDictionary != ObjectId.Null) {
                                        using (DBDictionary dict = (DBDictionary)tr.GetObject(br.ExtensionDictionary, OpenMode.ForWrite, false)) {
                                            if (dict.Contains(key)) {
                                                ObjectId deleteId = dict.GetAt(key);
                                                Xrecord xRec = (Xrecord)tr.GetObject(dict.GetAt(key), OpenMode.ForWrite, false);
                                                xRec.Erase();
                                                dict.Remove(deleteId);
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
    }
}