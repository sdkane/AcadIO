using Autodesk.AutoCAD.DatabaseServices;

namespace AcadIO {    
    class DxfHelper {
        //ACAD supports a large number of types that can be stored.
        public static DxfCode GetFromObject(object value) {
            string objectType = value.GetType().ToString().ToLower();
            DxfCode dxfCode;
            switch (objectType) {
                case "int": dxfCode = DxfCode.Int32; break;
                case "double": dxfCode = DxfCode.Real; break;
                case "string": dxfCode = DxfCode.Text; break;
                case "bool": dxfCode = DxfCode.Bool; break;
                case "handle": dxfCode = DxfCode.Handle; break;
                default: dxfCode = DxfCode.Text; break;
            }
            return dxfCode;
        }
    }
}
