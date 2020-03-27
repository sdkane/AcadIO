using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;

namespace AcadIO.Block {
    /// <summary>incomplete build out of an alternate referencing form, thinking of ways it could be useful</summary>
    public class ByRef {
        private ObjectId _id;
        public ByRef(ObjectId blockId) {
            _id = blockId;
        }
        public ByRef(Handle handle) {
            Application.DocumentManager.MdiActiveDocument.Database.TryGetObjectId(handle, out _id);
        }

        private static T GetRecord<T>(string key) {
            return default(T);
        }

        private static void SetRecord(string key, object value) {

        }
    }
}
