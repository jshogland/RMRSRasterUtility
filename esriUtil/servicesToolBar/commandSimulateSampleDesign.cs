using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;

namespace servicesToolBar
{
    public class commandSimulateSampleDesign : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public commandSimulateSampleDesign()
        {
        }

        protected override void OnClick()
        {
            IMxDocument mxDoc = ArcMap.Document;
            IMap map = mxDoc.FocusMap;
            esriUtil.Forms.Sampling.frmSampleIntensity frm = new esriUtil.Forms.Sampling.frmSampleIntensity(map);
            frm.Show();
        }

        protected override void OnUpdate()
        {
        }
    }
}
