using System;
using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO.NOD {
    /// <summary>xrecords are non-volatile key/value pairs this stores them in the Named Object Dictionary (NOD)</summary>
    public class Record {
        public static void Set(string key, object value) {
            try {
                if (World.Docu != null) {
                    if (World.Docu.IsActive == true && World.Docu.IsDisposed == false) {
                        using (World.Docu.LockDocument()) {
                            using (Database db = World.Docu.Database) {                                
                                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                    using (DBDictionary dict = (DBDictionary)tr.GetObject(db.NamedObjectsDictionaryId, OpenMode.ForWrite, false)) {
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
                                    tr.Commit();
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Err.Log(ex);
            }
        }

        //this could be altered to use the typedvalue and retrieve the type from reversing the dxfcode lookup, but we would still need a generic return for the calling function
        public static T Get<T>(string key) {
            try {
                if (World.Docu != null) {
                    if (World.Docu.IsActive == true && World.Docu.IsDisposed == false) {
                        using (World.Docu.LockDocument()) {
                            using (Database db = World.Docu.Database) {                                
                                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                    using (DBDictionary dict = (DBDictionary)tr.GetObject(db.NamedObjectsDictionaryId, OpenMode.ForRead, false)) {
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
                                using (DBDictionary dict = (DBDictionary)tr.GetObject(db.NamedObjectsDictionaryId, OpenMode.ForWrite, false)) {
                                    if (dict.Contains(key)) {
                                        ObjectId deleteId = dict.GetAt(key);
                                        Xrecord xRec = (Xrecord)tr.GetObject(dict.GetAt(key), OpenMode.ForWrite, false);
                                        xRec.Erase();
                                        dict.Remove(deleteId);
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
