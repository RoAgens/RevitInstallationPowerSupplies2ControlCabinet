using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EESetUp
{
    public class EPanel
    {
        private ElementId _id;
        private FamilyInstance _fi;
        private PowSupply _powsupply;

        public EPanel(FamilyInstance fi)
        {
            _fi = fi;
            _id = _fi.Id;
            _powsupply = null;
        }

        public FamilyInstance RVFI => _fi;

        public ElementId Id => _id;

        public PowSupply PowSupply
        {
            get { return _powsupply; }
            set {
                _powsupply = value;
                //AddPowSupply();
                //_powsupply.SetNumber(GetEpanelName);
            }
        }

        public bool IsHavePS => _powsupply is null ? false : true;

        public ElementId GetFIId => _id;

        public FamilyInstance GetFIbyId(EPanel ep)
        {
            return null;
        }

        private string GetEpanelName => _fi.Name;
   
        public void AddPowSupply(PowSupply powsupply)
        {
            _powsupply = powsupply;
            SetEpanelParam();
            SetCircParam();
        }

        private void SetEpanelParam()
        {
            _powsupply.RVFI.LookupParameter("ADSK_Номер устройства").Set(GetEpanelName);
        }

        private void SetCircParam()
        {
            // тут, скорее всего, нужно взять сеть у шкафа, и назначить ее панели

            ElementId elad = new ElementId(3223989);
            _powsupply.RVFI.get_Parameter(BuiltInParameter.RBS_FAMILY_CONTENT_DISTRIBUTION_SYSTEM).Set(elad); //RBS_FAMILY_CONTENT_DISTRIBUTION_SYSTEM
        }

        public Face GetFace(GeometryElement geomel)
        {
            foreach (GeometryObject ge in geomel)
            {
                if (ge is GeometryInstance gi)
                {
                    foreach (GeometryObject go in gi.GetSymbolGeometry()) //gi.GetInstanceGeometry()) - странно, но это Symbol???
                    {
                        if (go is Solid sl)
                        {
                            FaceArray fa = sl.Faces;

                            foreach (Face fc in fa)
                            {
                                if (fc is PlanarFace pf)
                                {
                                    if (pf.MaterialElementId.IntegerValue == 10009613) /// !!! так себе !!!
                                    {
                                        if (pf.FaceNormal.Y == -1) { return (Face)pf; }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
