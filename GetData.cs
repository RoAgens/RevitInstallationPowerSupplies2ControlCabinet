using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EESetUp
{
    public class GetData
    {
        UIApplication _revit;
        Document _doc;

        IList<FamilyInstance> _epanellist;
        IList<FamilyInstance> _powsupplylist;

        static string PPKName = "ППК-Р_тип модель";
        static string MPName = "Модуль питания ППК-Р - БП 24В";

        public GetData(ExternalCommandData commandData)
        {
            _revit = commandData.Application;
            _doc= _revit.ActiveUIDocument.Document;

            _epanellist = GetPPKList();
            _powsupplylist = GetMPList();
        }

        private List<FamilyInstance> GetPPKList()
        {
            return new FilteredElementCollector(_doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .ToElements().Select(x => (FamilyInstance)x).Where(y => y.Symbol.Family.Name == PPKName).ToList(); //
        }

        private List<FamilyInstance> GetMPList()
        {
            return new FilteredElementCollector(_doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .ToElements().Select(x => (FamilyInstance)x).Where(y => y.Symbol.Family.Name == MPName).ToList(); //
        }

        public ReadOnlyCollection<FamilyInstance> EPanelColl => new ReadOnlyCollection<FamilyInstance>(_epanellist);

        //public IList<FamilyInstance> PPKList => _epanellist;

        public ReadOnlyCollection<FamilyInstance> PowSupplyColl => new ReadOnlyCollection<FamilyInstance>(_powsupplylist);

        //public IList<FamilyInstance> MPList => _powsupplylist;

        public string EPanelCount => _epanellist.Count.ToString();

        public string PowSupplyCount => _powsupplylist.Count.ToString();
    }
}
