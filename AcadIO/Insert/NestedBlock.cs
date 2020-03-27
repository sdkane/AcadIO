using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace AcadIO.Insert {    
	public static class NestedBlock {               
        /// <summary>this inserts a block into another block, the block designated by oid is the receiver and receives a new copy of the block designated by blockname</summary>        
        public static Handle Insert(ObjectId oid, string blockPath, string blockName, Point3d position = new Point3d()) {            
            Handle hd = new Handle();
            try {
                if (oid.IsErased == false & oid.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(oid, OpenMode.ForWrite)) {

                                    if (AcadIO.Insert.DB.AddBlock(blockPath + "\\" + blockName) == false) { return hd; }

                                    using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead)) {
                                        using (BlockReference bi = new BlockReference(Autodesk.AutoCAD.Geometry.Point3d.Origin, bt[blockPath])) {
                                            using (BlockTableRecord blockSpace = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForWrite)) {                                                    
                                                blockSpace.AppendEntity(bi);
                                                tr.AddNewlyCreatedDBObject(bi, true);
                                                bi.LayerId = br.LayerId;
                                                bi.Position = position;
                                                hd = bi.Handle;                                                    
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
            finally {                
            }
            return hd;            
        }

        /// <summary>this inserts a block into another block, the block designated by oid is the receiver and receives a new copy of the block designated by blockname, this block is rotated by the specified number of degrees around it's x-axis</summary>        
        public static Handle InsertRotated(ObjectId oid, string blockPath, string blockName, int rotation = 90 ) {            
            Handle hd = new Handle();
            try {
                if (oid.IsErased == false & oid.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(oid, OpenMode.ForWrite)) {

                                    if (AcadIO.Insert.DB.AddBlock(blockPath + "\\" + blockName) == false) { return hd; }

                                    using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead)) {
                                        using (BlockReference bi = new BlockReference(Autodesk.AutoCAD.Geometry.Point3d.Origin, bt[blockPath])) {
                                            bi.TransformBy(Autodesk.AutoCAD.Geometry.Matrix3d.Rotation(rotation * Math.PI / 180, bi.BlockTransform.CoordinateSystem3d.Xaxis, new Autodesk.AutoCAD.Geometry.Point3d(0, 0, 0)));
                                            using (BlockTableRecord blockSpace = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForWrite)) {
                                                blockSpace.AppendEntity(bi);
                                                tr.AddNewlyCreatedDBObject(bi, true);
                                                bi.LayerId = br.LayerId;
                                                hd = bi.Handle;
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
            return hd;
        }

        public static void Delete(ObjectId oid) {
            try {
                if (oid.IsErased == false & oid.IsEffectivelyErased == false) {
                    using (World.Docu.LockDocument()) {
                        using (Database db = World.Docu.Database) {
                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                using (BlockReference br = (BlockReference)tr.GetObject(oid, OpenMode.ForWrite)) {
                                    br.Erase();
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