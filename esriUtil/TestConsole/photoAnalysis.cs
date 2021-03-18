using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;

namespace DownloadData
{
    public class photoAnalysis
    {
        public enum extensionType { jpg, bmp, gif, tiff, png };
        public photoAnalysis(string InputDir, string PolygonPath, string OutPutCSV, extensionType Extension = extensionType.jpg)
        {
            photoDir = InputDir;
            polyPath = PolygonPath;
            outCSV = OutPutCSV;
            ext = Extension;
        }

        extensionType ext = extensionType.jpg;
        string photoDir = "";
        string polyPath = "";
        string outCSV = "";
        public string PhotoDir { get { return photoDir; } }
        public string PolygonPath { get { return polyPath; } }
        public string OutCSV { get { return outCSV; } }
        esriUtil.geoDatabaseUtility geoUtil = new esriUtil.geoDatabaseUtility();
        esriUtil.rasterUtil rsUtil = new esriUtil.rasterUtil();
        public void runAnalysis()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outCSV))
            {
                string hd = "ROI,DIR,NAME,Year,Month,Day,Time,Red,Green,Blue,GCC,ExG,VIgreen";
                sw.WriteLine(hd);
                IFeatureClass ftrCls = geoUtil.getFeatureClass(polyPath);
                IFeatureCursor fCur = ftrCls.Search(null, true);
                IFeature ftr = fCur.NextFeature();
                int cnt = 1;
                while (ftr != null)
                {
                    IGeometry geo = ftr.Shape;
                    string[] lnArr = new string[13];
                    lnArr[0] = cnt.ToString();
                    lnArr[1] = photoDir.Split(new char[] { '\\' }).Last();
                    foreach (string flPath in System.IO.Directory.GetFiles(photoDir, "*." + ext))
                    {
                        string nm = System.IO.Path.GetFileNameWithoutExtension(flPath);
                        Console.WriteLine("Working On " + nm);
                        lnArr[2] = nm;
                        //Console.WriteLine(flPath);

                        System.IO.FileStream fs = new System.IO.FileStream(flPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                        BitmapSource img = BitmapFrame.Create(fs);
                        BitmapMetadata md = (BitmapMetadata)img.Metadata;
                        string timeStamp = "";
                        if (md != null)
                        {
                            timeStamp = md.DateTaken;
                            DateTime dt = Convert.ToDateTime(timeStamp);
                            lnArr[3] = dt.Year.ToString();
                            lnArr[4] = dt.Month.ToString();
                            lnArr[5] = dt.Day.ToString();
                            lnArr[6] = dt.TimeOfDay.ToString();



                            //Console.WriteLine(date);
                        }
                        else
                        {
                            //Console.WriteLine("md=null");
                        }
                        IFunctionRasterDataset rsDset = rsUtil.clipRasterFunction(flPath, geo, esriRasterClippingType.esriRasterClippingOutside);
                        double r, g, b;
                        bool cntCheck = getRGB(rsDset, out r, out g, out b);
                        lnArr[7] = r.ToString();
                        lnArr[8] = g.ToString();
                        lnArr[9] = b.ToString();
                        double GCC=0;
                        double ExG=0;
                        double VIgreen=0;
                        if(!cntCheck)
                        { 
                            GCC = g / (r + g + b);
                            ExG = 2 * g - (r + b);
                            VIgreen = (g - r) / (g + r);
                        }
                        lnArr[10] = GCC.ToString();
                        lnArr[11] = ExG.ToString();
                        lnArr[12] = VIgreen.ToString();
                        sw.WriteLine(String.Join(",", lnArr));
                    }
                    ftr = fCur.NextFeature();
                }
                sw.Close();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(fCur);
            }

        }

        private bool getRGB(IFunctionRasterDataset rs, out double r, out double g, out double b)
        {
            r = 0;
            g = 0;
            b = 0;
            IRasterBandCollection rsbc = (IRasterBandCollection)rs;
            double[] vlArr = new double[rsbc.Count];
            int[] cntArr = new int[vlArr.Length];
            IRaster2 rs2 = (IRaster2)rsUtil.createRaster(rs);
            IRasterCursor rsCur = rs2.CreateCursorEx(null);
            do
            {
                IPixelBlock pb = rsCur.PixelBlock;
                for (int bd = 0; bd < vlArr.Length; bd++)
                {
                    for (int rw = 0; rw < pb.Height; rw++)
                    {
                        for (int cl = 0; cl < pb.Width; cl++)
                        {
                            object vlObj = pb.GetVal(bd, cl, rw);
                            if (vlObj != null)
                            {
                                vlArr[bd] += System.Convert.ToDouble(vlObj);
                                cntArr[bd] += 1;
                            }

                        }
                    }
                }

            } while (rsCur.Next());
            int cntr = cntArr[0];
            int cntg = cntArr[1];
            int cntb = cntArr[2];
            int bcheck = 0;
            bool cntZerro = false;
            if (cntr > 0)
            {
                r = vlArr[0] / cntr;
            }
            else
            {
                bcheck += 1;
            }
            if (cntg > 0)
            {
                g = vlArr[1] / cntg;
            }
            else
            {
                bcheck += 1;
            }
            if (cntb > 0)
            {
                b = vlArr[2] / cntb;
            }
            if (bcheck == 2)
            {
                cntZerro = true;
            }
            return cntZerro;
        }
        public System.Drawing.Bitmap clipImage(string imgFl, IPolygon poly)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            IPointCollection pc = (IPointCollection)poly;
            int pcCnt = pc.PointCount;
            System.Drawing.PointF[] nwPoly = new System.Drawing.PointF[pcCnt];
            for (int p = 0; p < pcCnt; p++)
            {
                IPoint pnt = pc.get_Point(p);
                nwPoly[p] = new System.Drawing.PointF(System.Convert.ToSingle(pnt.X), System.Convert.ToSingle(pnt.Y));
            }
            gp.AddPolygon(nwPoly);
            System.Drawing.Image img = System.Drawing.Bitmap.FromFile(imgFl);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
            g.Clip = new System.Drawing.Region(gp);
            g.DrawImage(img, 0, 0);
            return new System.Drawing.Bitmap(img);
        }
        public bool getMeans(System.Drawing.Bitmap bm, out double blue, out double green, out double red)
        {
            bool cntZerro = false;
            blue = 0;
            green = 0;
            red = 0;

            BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height),ImageLockMode.ReadOnly,bm.PixelFormat);
            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            long[] totals = new long[] { 0, 0, 0 };

            int width = bm.Width;
            int height = bm.Height;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;
                            int vl = p[idx];

                            totals[color] += p[idx];
                        }
                    }
                }
            }

            blue = totals[2] / (width * height);
            green = totals[1] / (width * height);
            red = totals[0] / (width * height);
            return cntZerro;
        }
    }
}
