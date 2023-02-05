using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_3_6_3_1
{
    public class MainViewViewModel
    {   // Свойства
        private ExternalCommandData _commandData;
        public List<FamilySymbol> Furniture { get; } = new List<FamilySymbol>();
        public FamilySymbol SelectedFurniture { get; set; }
        public int CopyCount { get; set; }
        public List<XYZ> Points { get; } = new List<XYZ>();
        public DelegateCommand SaveCommand { get; }

        // Конструктор
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Furniture = FamilySymbolUtils.GetFamilySymbols(commandData);
            CopyCount = 3;
            SaveCommand = new DelegateCommand(OnSaveCommand);
            Points = SelectionUtils.GetPoints(_commandData, "Выберете точку", ObjectSnapTypes.Points);
        }

        // Методы
        public void OnSaveCommand()
        {
            // обрращаемся к документу
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // создаю переменные точек 1 и 2.
            // Переменная обращается к созданному классу,
            // прописывающему логику создания точки.
            XYZ startpoint = Points[0];
            XYZ endpoint = Points[0];

            //Определаю расстояние между точками копии по осям
            int ratioX = (int)(startpoint.X / CopyCount - endpoint.X / CopyCount);
            int ratioY = (int)(startpoint.Y / CopyCount - endpoint.Y / CopyCount);
            int ratioZ = (int)(startpoint.Z / CopyCount - endpoint.Z / CopyCount);

            // создаю копиии экземпляров семейст в указанных точках, для чего создаю переменную точки.
            for (int i=1; i< CopyCount+1; i++)
            {
                var instancePoint = new XYZ(ratioX * i, ratioY * i, ratioZ * i);
                FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurniture, instancePoint);
            }
            RaiseCloseRequest();
        }

        //событие создается для скрытие окна на время выбора.
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        //событие создается для повторного открытия окна после отработки программы.
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
        //событие создается для закрытия программы.
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        

    }

}
