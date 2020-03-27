using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace AcadIO.Insert {
    public static class Block {
        public static ObjectId Insert(string blockPath, string blockName, bool scaleBlock = false, Scale3d scaleFactor = default(Scale3d)) {
            ObjectId id = new ObjectId();
                       
            using (World.Docu.LockDocument()) {
                using (Database db = World.Docu.Database) {
                    using (Transaction tr = db.TransactionManager.StartTransaction()) {
                        using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead)) {

                            if (AcadIO.Insert.DB.AddBlock(blockPath + "\\" + blockName) == false) { return id; }
                            
                            using (BlockReference br = new BlockReference(Point3d.Origin, bt[blockName])) {

                                PromptResult pr = World.Docu.Editor.Drag(new Jigs.InsertBlockJig(br));
                                if (pr.Status == PromptStatus.Cancel | pr.Status == PromptStatus.Error | pr.Status == PromptStatus.None) { return id; }

                                pr = World.Docu.Editor.Drag(new Jigs.RotateBlockJig(br));
                                if (pr.Status == PromptStatus.Cancel | pr.Status == PromptStatus.Error | pr.Status == PromptStatus.None) { return id; }

                                using (BlockTableRecord drawingSpace = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)) {                                    

                                    if (scaleBlock == true) {
                                        //br.ScaleFactors = new Autodesk.AutoCAD.Geometry.Scale3d(World.UnitFactor);
                                        br.ScaleFactors = scaleFactor;
                                    }

                                    drawingSpace.AppendEntity(br);
                                    tr.AddNewlyCreatedDBObject(br, true);

                                    if (AcadIO.Layers.IsObjectLayerLocked(br.ObjectId)) { World.Docu.Editor.WriteMessage(Environment.NewLine + "BCAD: Cannot insert on locked layer" + Environment.NewLine); return id; }

                                    using (BlockTableRecord attributeBlockTableRecord = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForWrite)) {
                                        foreach (ObjectId attributeId in attributeBlockTableRecord) {
                                            if (attributeId.ObjectClass.Name == "AcDbAttributeDefinition") {
                                                using (AttributeDefinition attributedef = (AttributeDefinition)tr.GetObject(attributeId, OpenMode.ForWrite)) {
                                                    AttributeReference attributeref = new AttributeReference();
                                                    attributeref.SetAttributeFromBlock(attributedef, br.BlockTransform);
                                                    br.AttributeCollection.AppendAttribute(attributeref);
                                                    tr.AddNewlyCreatedDBObject(attributeref, true);
                                                }
                                            }
                                        }
                                    }

                                    id = br.ObjectId;

                                }
                            }
                        }
                        tr.Commit();
                    }
                }
            }
            
            return id;
        }

        public static ObjectId Insert(string blockPath, string blockName, Point3d position, bool scaleBlock = false, Scale3d scaleFactor = default(Scale3d)) {
            ObjectId id = new ObjectId();
            using (World.Docu.LockDocument()) {
                using (Database db = World.Docu.Database) {
                    using (Transaction tr = db.TransactionManager.StartTransaction()) {
                        using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead)) {

                            if (AcadIO.Insert.DB.AddBlock(blockPath + "\\" + blockName) == false) { return id; }

                            using (BlockReference br = new BlockReference(position, bt[blockPath])) {
                                using (BlockTableRecord drawingSpace = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)) {                                    

                                    if (scaleBlock == true) {
                                        br.ScaleFactors = scaleFactor;
                                    }

                                    drawingSpace.AppendEntity(br);
                                    tr.AddNewlyCreatedDBObject(br, true);

                                    if (AcadIO.Layers.IsObjectLayerLocked(br.ObjectId)) { World.Docu.Editor.WriteMessage(Environment.NewLine + "BCAD: Cannot insert on locked layer" + Environment.NewLine); return id; }

                                    using (BlockTableRecord attributeBlockTableRecord = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForWrite)) {
                                        foreach (ObjectId attributeId in attributeBlockTableRecord) {
                                            if (attributeId.ObjectClass.Name == "AcDbAttributeDefinition") {
                                                using (AttributeDefinition attributedef = (AttributeDefinition)tr.GetObject(attributeId, OpenMode.ForWrite)) {
                                                    AttributeReference attributeref = new AttributeReference();
                                                    attributeref.SetAttributeFromBlock(attributedef, br.BlockTransform);
                                                    br.AttributeCollection.AppendAttribute(attributeref);
                                                    tr.AddNewlyCreatedDBObject(attributeref, true);
                                                }
                                            }
                                        }
                                    }

                                    id = br.ObjectId;

                                }
                            }
                        }
                        tr.Commit();
                    }
                }
            }
            return id;
        }
    }
}