using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO.Insert {
    class DB {
        public static bool AddBlock(string blockPath) {
            //if (blockPath.Contains(".dwg") == false) { blockPath = blockPath + ".dwg"; }
            blockPath = blockPath + ".dwg";

            if (System.IO.File.Exists(blockPath)) {
                using (World.Docu.LockDocument()) {
                    using (Database db = World.Docu.Database) {
                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                            using (BlockTable bt = (BlockTable)tr.GetObject(World.Docu.Database.BlockTableId, OpenMode.ForRead)) {
                                if (bt.Has(blockPath) == false) {
                                    using (Database tempDB = new Database(false, true)) {
                                        tempDB.ReadDwgFile(blockPath, System.IO.FileShare.Read, true, "");
                                        World.Docu.Database.Insert(blockPath, tempDB, false);
                                    }
                                    tr.Commit();
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
