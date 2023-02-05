using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_3_6_3
{
    public class FamilyInstanceUtils
    {
        public static FamilyInstanceUtils CreateFamilyInstance(
            ExternalCommandData commandData,
            FamilySymbol oFamSymb,
            XYZ insertionPoint/*,
            Level oLevel*/)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            FamilyInstance familyInstance = null;

            //Создаем family instance (предмет мебели или любое другое не системное симейство)
            using (var t = new Transaction(doc, "Создание несистемного симейства"))
            {
                t.Start();
                // Проверяем выбранно ли семейство
                if (!oFamSymb.IsActive)
                {
                    oFamSymb.Activate();
                    doc.Regenerate();
                }

                // создаем новый элемент на основе выбранных координат, семейства, уровня и (видимо библиотеки Автодеск).
                familyInstance = doc.Create.NewFamilyInstance(
                                                insertionPoint,
                                                oFamSymb,
                                                //oLevel,
                                                Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                t.Commit();
            }
            return (FamilyInstanceUtils)(object)familyInstance;
        }
    }
}
