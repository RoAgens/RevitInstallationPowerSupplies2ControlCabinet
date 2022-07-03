using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace EESetUp
{
    public class MainPro
    {
        ExternalCommandData _commandData;
        Document _doc;
        UserWin _userwin;
        IList<EPanel> _epanellist;
        IList<EPanel> _epanelforcompllist;
        FamilySymbol _symbol;

        public MainPro(ExternalCommandData commandData)
        {
            _commandData = commandData;
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            _doc = uidoc.Document;
        }

        public void Srart()
        {
            Face needface = null;
            XYZ direction = new XYZ(0, 0, 0);

            IniMWin();

            GetData getdata = new GetData(_commandData);

            IList<PowSupply> powsupplylist = getdata.PowSupplyColl.Select(x => new PowSupply(x)).ToList();
            _epanellist = getdata.EPanelColl.Select(x => new EPanel(x)).ToList();

            IList<EPanel> epaneforaddllist = new List<EPanel>();

            _symbol = powsupplylist[0].RVFI.Symbol;

            foreach (PowSupply ps in powsupplylist)
            {
                if (ps.IsHaveHost) { _ = _epanellist.Where(x => x.Id == ps.RVFI.Host.Id).First().PowSupply = ps; }
            }

            //TaskDialog.Show("найдено", $"Найдено ППК - {getdata.EPanelCount} + модулей - {getdata.PowSupplyCount}");

            _userwin.lb1.Content = $"Найдено ППК - {getdata.EPanelCount}";
            _userwin.lb2.Content = $"Найдено модулей - {getdata.PowSupplyCount}";

            _ = getdata.EPanelCount == getdata.PowSupplyCount ? _userwin.DrawPM.IsEnabled = false : _userwin.DrawPM.IsEnabled = true;

            _userwin.ShowDialog();

            using (Transaction tx = new Transaction(_doc))
            {
                tx.Start("Transaction Name");

                foreach (EPanel ep in _epanellist)
                {
                    if (!ep.IsHavePS)
                    {
                        View view = _doc.ActiveView;

                        Options geomOptions = new Options();
                        geomOptions.View = view;
                        geomOptions.IncludeNonVisibleObjects = true;
                        geomOptions.ComputeReferences = true;

                        GeometryElement wallGeom = ep.RVFI.get_Geometry(geomOptions);

                        needface = ep.GetFace(wallGeom);

                        XYZ fiorient = ep.RVFI.HandOrientation;

                        XYZ location = ((LocationPoint)ep.RVFI.Location).Point;
                        XYZ location1 = new XYZ(location.X + (.35) * fiorient.X, location.Y + (.35) * fiorient.Y, location.Z - .35);

                        FamilyInstance newFI = _doc.Create.NewFamilyInstance(needface, location1, fiorient, _symbol);

                        PowSupply newps = new PowSupply(newFI);
                        ep.AddPowSupply(newps);
                    }
                }

                tx.Commit();
            }
        }

        private void IniMWin()
        {
            _userwin = new UserWin();

            _userwin.WinClose.Click += Button_ClickClose;
            _userwin.DrawPM.Click += Button_ClickDrawPM;
        }

        private void Button_ClickClose(object sender, System.Windows.RoutedEventArgs e)
        {
            _userwin.Close();
        }

        private void Button_ClickDrawPM(object sender, System.Windows.RoutedEventArgs e)
        {
            _userwin.Close();
        }
    }
}
