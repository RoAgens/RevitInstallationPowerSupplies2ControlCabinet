using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EESetUp
{
    public class PowSupply
    {
        private FamilyInstance _fi;
        public PowSupply(FamilyInstance fi)
        {
            _fi = fi;
        }

        public FamilyInstance RVFI => _fi;

        public bool IsHaveHost => _fi.Host != null ? true : false;

        public void SetNumber(string str)
        {
            //_fi.LookupParameter("ADSK_Номер устройства").Set(GetEpanelName);
            //_fi.get_Parameter(BuiltInParameter.RBS_FAMILY_CONTENT_DISTRIBUTION_SYSTEM).Set(str); //ADSK_Номер устройства
        }
    }
}
