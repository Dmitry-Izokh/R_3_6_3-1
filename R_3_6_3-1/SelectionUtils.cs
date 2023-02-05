using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_3_6_3_1
{
    class SelectionUtils
    {
        public static List<XYZ> GetPoints(ExternalCommandData commandData, string promtmessage, ObjectSnapTypes objectSnapTypes)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            List<XYZ> points = new List<XYZ>();

            // Пока количество точек меньше 1 выполнять создание точек.
            while (points.Count < 1)
            {
                XYZ pickedPoint = null;
                try
                {
                    pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promtmessage);
                }
                catch
                {
                    break;
                }
                points.Add(pickedPoint);
            }
            return points;
        }
    }
}
