using System;
using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO.Block {    
	public class Property {
        /// <summary>most properties are typed as double, this currently only supports setting double</summary>
        public static void Set(ObjectId id, string key, double value) {
            try {
                if (id.IsErased == false && id.IsEffectivelyErased == false && id.IsNull == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForWrite)) {
                                    foreach (DynamicBlockReferenceProperty blockProperty in br.DynamicBlockReferencePropertyCollection) {
                                        if (blockProperty.PropertyName == key & World.IsNumeric(blockProperty.Value)) {                                            
                                            blockProperty.Value = value;
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

        /// <summary>0 for default, 1 for flipped </summary>
        public static void SetFlip(ObjectId id, string key, Int32 value) {
            try {
                if (id.IsErased == false && id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForWrite)) {
                                    foreach (DynamicBlockReferenceProperty blockProperty in br.DynamicBlockReferencePropertyCollection) {
                                        if (blockProperty.PropertyName == key & World.IsNumeric(blockProperty.Value)) {
                                            var pos = blockProperty.GetAllowedValues();
                                            blockProperty.Value = pos[value];                                            
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

        public static double Get(ObjectId id, string key) {            
            try {
                if (id.IsErased == false && id.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        if (id != new ObjectId()) {
                            using (Database db = World.Docu.Database) {
                                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                    using (BlockReference br = (BlockReference)tr.GetObject(id, OpenMode.ForRead)) {
                                        foreach (DynamicBlockReferenceProperty blockProperty in br.DynamicBlockReferencePropertyCollection) {
                                            if (blockProperty.PropertyName == key & World.IsNumeric(blockProperty.Value)) {
                                                return Convert.ToDouble(blockProperty.Value);
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
            return default(double);
        }
    }
}