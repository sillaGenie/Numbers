using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numbers
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; private set; }
        public int Number { get; set; }=0;
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;

            SaveCommand = new DelegateCommand(OnSaveCommand);



        }
        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var rooms = new FilteredElementCollector(doc, doc.ActiveView.Id)
              .OfCategory(BuiltInCategory.OST_Rooms)
              .Cast<Room>()
              .ToList();
            
            Transaction ts = new Transaction(doc, "Нумерация");
            ts.Start();
            for (int i = 0; i < rooms.Count; i++)
            {
                int num = i + Number;
                string n = num.ToString();
                rooms[i].get_Parameter(BuiltInParameter.ROOM_NUMBER).Set(n);
            }
            ts.Commit();
            RaiseCloseRequest();
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
