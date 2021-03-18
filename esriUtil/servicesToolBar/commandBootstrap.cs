using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;



namespace servicesToolBar
{
    public class commandBootstrap : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public commandBootstrap()
        {
        }

        protected override void OnClick()
        {
            IMxDocument mxDoc = ArcMap.Document;
            IMap map = mxDoc.FocusMap;
            esriUtil.Forms.Sampling.frmBootstrap frm = new esriUtil.Forms.Sampling.frmBootstrap(map);
            frm.Show();
        }

        protected override void OnUpdate()
        {
        }
    }
}
