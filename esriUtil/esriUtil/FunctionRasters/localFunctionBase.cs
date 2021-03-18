﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace esriUtil.FunctionRasters
{
    public abstract class localFunctionBase: IRasterFunction
    {
        private IRasterInfo myRasterInfo; // Raster Info for the log Function
        private rstPixelType myPixeltype = rstPixelType.PT_UNKNOWN; // Pixel Type of the log Function.
        private string myName = "Local Standard Deviation Function"; // Name of the log Function.
        private string myDescription = "Transforms a raster using local Standard Deviation value transformation"; // Description of the log Function.
        private IFunctionRasterDataset inrs = null;
        private IFunctionRasterDataset inrsBands = null;
        private IRasterFunctionHelper myFunctionHelper = new RasterFunctionHelperClass();
        //private IRasterFunctionHelper myFunctionHelperCoef = new RasterFunctionHelperClass();// Raster Function Helper object.
        public IRasterInfo RasterInfo { get { return myRasterInfo; } }
        public rstPixelType PixelType { get { return myPixeltype; } set { myPixeltype = value; } }
        public string Name { get { return myName; } set { myName = value; } }
        public string Description { get { return myDescription; } set { myDescription = value; } }
        public bool myValidFlag = false;
        public bool Valid { get { return myValidFlag; } }
        public void Bind(object pArgument)
        {
            if (pArgument is LocalFunctionArguments)
            {
                LocalFunctionArguments arg = (LocalFunctionArguments)pArgument;
                inrsBands = arg.InRaster;
                inrs = arg.outRaster;
                myFunctionHelper.Bind(inrs);
                //myFunctionHelperCoef.Bind(inrsBands);
                myRasterInfo = myFunctionHelper.RasterInfo;
                myPixeltype = myRasterInfo.PixelType;
                myValidFlag = true;
            }
            else
            {
                throw new System.Exception("Incorrect arguments object. Expected: LocalFunctionArguments");
            }
        }
        /// <summary>
        /// Read pixels from the input Raster and fill the PixelBlock provided with processed pixels.
        /// The RasterFunctionHelper object is used to handle pixel type conversion and resampling.
        /// The log raster is the natural log of the raster. 
        /// </summary>
        /// <param name="pTlc">Point to start the reading from in the Raster</param>
        /// <param name="pRaster">Reference Raster for the PixelBlock</param>
        /// <param name="pPixelBlock">PixelBlock to be filled in</param>
        public void Read(IPnt pTlc, IRaster pRaster, IPixelBlock pPixelBlock)
        {
            try
            {
                // Call Read method of the Raster Function Helper object.
                myFunctionHelper.Read(pTlc, null, pRaster, pPixelBlock);
                int pBHeight = pPixelBlock.Height;
                int pBWidth = pPixelBlock.Width;
                IPnt pbSize = new PntClass();
                pbSize.SetCoords(pBWidth, pBHeight);
                IPixelBlock3 ipPixelBlock = (IPixelBlock3)pPixelBlock;
                System.Array[] inArr = new System.Array[inrsBands.RasterInfo.BandCount];
                IRasterBandCollection rsBc = (IRasterBandCollection)inrsBands;
                for (int p = 0; p < inArr.Length; p++)
                {
                    IRasterBand rsB = rsBc.Item(p);
                    IRawPixels rP = (IRawPixels)rsB;
                    IPixelBlock pb = rP.CreatePixelBlock(pbSize);
                    IPixelBlock3 pb3 = (IPixelBlock3)pb;
                    rP.Read(pTlc,pb);
                    inArr[p] = (System.Array)pb3.get_PixelData(0);
                }
                System.Array outArr = (System.Array)pPixelBlock.get_SafeArray(0);
                rstPixelType rsPt = pPixelBlock.get_PixelType(0);
                for (int r = 0; r < ipPixelBlock.Height; r++)
                {
                    for (int c = 0; c < ipPixelBlock.Width; c++)
                    {
                        object outVlObj = ipPixelBlock.GetVal(0, c, r);
                        if (outVlObj != null)
                        {
                            float outVl;
                            if (getOutPutVl(inArr, c, r, out outVl))
                            {
                                object newVl = rasterUtil.getSafeValue(outVl, rsPt);//convertVl(outVl,rsPt);
                                outArr.SetValue(newVl, c, r);
                            }
                        }
                    }
                }
                ipPixelBlock.set_PixelData(0, outArr);

            }
            catch (Exception exc)
            {
                System.Exception myExc = new System.Exception("Exception caught in Read method of localMean Function. " + exc.Message, exc);
                Console.Write(exc.ToString());
                throw myExc;
            }
        }
        public void Update()
        {
            try
            {
            }
            catch (Exception exc)
            {
                System.Exception myExc = new System.Exception("Exception caught in Update method of local Function", exc);
                throw myExc;
            }
        }

        public abstract bool getOutPutVl(System.Array[] inArr, int c, int r, out float outvl);
        private object convertVl(object outVl, rstPixelType pType)
        {
            //Console.WriteLine(myFunctionHelper.RasterInfo.PixelType.ToString());
            object newVl = 0;
            switch (pType)
            {
                case rstPixelType.PT_CHAR:
                    newVl = System.Convert.ToSByte(outVl);
                    break;
                case rstPixelType.PT_LONG:
                    newVl = System.Convert.ToInt32(outVl);
                    break;
                case rstPixelType.PT_SHORT:
                    newVl = System.Convert.ToInt16(outVl);
                    break;
                case rstPixelType.PT_U1:
                    newVl = System.Convert.ToBoolean(outVl);
                    break;
                case rstPixelType.PT_U2:
                case rstPixelType.PT_U4:
                case rstPixelType.PT_UCHAR:
                    //Console.WriteLine("Converting Values");
                    newVl = System.Convert.ToByte(outVl);
                    break;
                case rstPixelType.PT_ULONG:
                    newVl = System.Convert.ToUInt32(outVl);
                    break;
                case rstPixelType.PT_USHORT:
                    newVl = System.Convert.ToUInt16(outVl);
                    break;
                default:
                    newVl = outVl;
                    break;
            }
            return newVl;
        }

    }
}