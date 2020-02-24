using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using NG3.Web.Mvc;
using SUP.Common.Base;
using SUP.Frame.Facade;
using System.Drawing;
using System.Drawing.Imaging;
using SUP.Frame.DataEntity;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using Newtonsoft.Json.Linq;

namespace SUP.Frame.Controller
{
    public class QRCodeSetController : AFController
    {
        private IQRCodeSetFacade proxy;

        public QRCodeSetController()
        {
            proxy = AopObjectProxy.GetObject<IQRCodeSetFacade>(new QRCodeSetFacade());
        }
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QRCodeList()
        {
            return View("QRCodeList");
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QRCodeEdit()
        {
            //ViewBag.Products = productProxy.GetProducts();
            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];
            if (ViewBag.OType == "add")
            {
                bool temp;
                ViewBag.ID = proxy.GetMaxID("fg3_qrcode_rule", out temp)+1;
            }
            return View("QRCodeEdit");
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QRCodeRegister()
        {
            bool temp;
            ViewBag.ID = proxy.GetMaxID("fg3_qrcode_style", out temp);
            if (!temp)
            {
                ViewBag.OType = "edit";
            }
            else
            {
                ViewBag.OType = "add";
            }
             
            return View("QRCodeRegister");
        }


        public string GetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];

            int pageSize;
            int.TryParse(limit, out pageSize);
            int pageIndex;
            int.TryParse(page, out pageIndex);

            int totalRecord = 0;

            DataTable dt = proxy.GetList(clientJsonQuery, pageSize, pageIndex, ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }
        public string GetMaster(string id)
        {
            DataTable dt = proxy.GetMaster(id);
            //DataTable dt2 = proxy.GetProduct(id);
            //DataTable dt2 = new DataTable();
            string dtJson = "";
            //string dt2Json = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                dtJson = JsonConvert.SerializeObject(dt);
            }
            //if (dt2 != null && dt2.Rows.Count > 0)
            //{
            //    dt2Json = JsonConvert.SerializeObject(dt2);
            //}
            //string response = "{'master':"+ dtJson + ",'product':"+ dt2Json + " }";
            return dtJson;
        }

        public string GetDetailField(string id)
        {
            DataTable dt = proxy.GetDetailField(id);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }

        public string Save()
        {
            string masterId = System.Web.HttpContext.Current.Request.Form["masterid"];
            string masterData = System.Web.HttpContext.Current.Request.Form["masterData"];
            string detailData = System.Web.HttpContext.Current.Request.Form["detailData"];
            string productData = System.Web.HttpContext.Current.Request.Form["productData"];

            DataTable masterDt = new DataTable();
            DataTable detailDt = new DataTable();
            DataTable productDt = new DataTable();

            if (!string.IsNullOrWhiteSpace(masterData))
            {
                masterDt = DataConverterHelper.ToDataTable(masterData, "select * from fg3_qrcode_rule");
            }
            if (!string.IsNullOrWhiteSpace(detailData))
            {
                detailDt = DataConverterHelper.ToDataTable(detailData, "select * from fg3_qrcode_rule_detail");
            }
            if (!string.IsNullOrWhiteSpace(productData))
            {
                productDt = DataConverterHelper.ToDataTable(productData, "select * from fg_busiproduct");
            }
          
            foreach (DataRow dr in masterDt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr["phid"] = masterId;
                    productDt.Rows[0]["code"] = Guid.NewGuid().ToString();
                }
                productDt.Rows[0]["busid"] = "fg3_qrcode_rule";
                productDt.Rows[0]["busikey"] = masterId;
            }
            bool temp;
            long detailPhid = proxy.GetMaxID("fg3_qrcode_rule_detail",out temp);
            foreach (DataRow dr in detailDt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr["phid"] = ++detailPhid;
                    dr["originalphid"] = masterId;
                }               
            }

            ResponseResult result = new ResponseResult();

            try
            {
                result = proxy.Save(masterId, masterDt, detailDt, productDt);
            }
            catch (Exception ex)
            {
                result.Status = "error";
                result.Msg = ex.Message.ToString();
            }

            string response = JsonConvert.SerializeObject(result);
            return response;
        }

        //public int Delete(string masterId)
        //{
        //    int result = proxy.Delete(masterId);
        //    return result;
        //}

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static Bitmap Generate3(string logoPath, string text, int width, int height,string addLogo, Color BackC, Color ForeC)
        {
           
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            //hint.Add(EncodeHintType.MARGIN, 2);//旧版本不起作用，需要手动去除白边

            //生成二维码 
            BitMatrix bm = writer.encode(text, BarcodeFormat.QR_CODE, width + 30, height + 30, hint);
            bm = deleteWhite(bm);
            BarcodeWriter barcodeWriter = new BarcodeWriter();

            //设置二维码颜色
            barcodeWriter.Renderer = new ZXing.Rendering.BitmapRenderer { Background = BackC, Foreground = ForeC };
            Bitmap map = barcodeWriter.Write(bm);
            if(addLogo != "1")
            {
                return map;
            }
            //Logo 图片
            //string logoPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\img\logo.png";         
            if (!System.IO.File.Exists(logoPath))
            {
                return map;
            }
            Bitmap logo = new Bitmap(logoPath);
            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 3), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3), logo.Height);
            int middleL = (map.Width - middleW) / 2;
            int middleT = (map.Height - middleH) / 2;

            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            //return bmpimg;
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(map, 0, 0, map.Width, map.Height);
                //白底将二维码插入图片
                //g.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
                g.FillRectangle(new SolidBrush(ForeC), middleL, middleT, middleW, middleH);
                g.DrawImage(logo, middleL, middleT, middleW, middleH);
            }
            return bmpimg;
        }

        /// <summary>
        /// 删除默认对应的空白
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static BitMatrix deleteWhite(BitMatrix matrix)
        {
            int[] rec = matrix.getEnclosingRectangle();
            int resWidth = rec[2] + 1;
            int resHeight = rec[3] + 1;

            BitMatrix resMatrix = new BitMatrix(resWidth, resHeight);
            resMatrix.clear();
            for (int i = 0; i < resWidth; i++)
            {
                for (int j = 0; j < resHeight; j++)
                {
                    if (matrix[i + rec[0], j + rec[1]])
                        //resMatrix[i, j] = true;
                        resMatrix[i, j] = true;
                }
            }
            return resMatrix;
        }

        public string GetQrStyle(string id)
        {
            DataTable dt = proxy.GetQrStyle(id);
            string dtJson = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                dtJson = JsonConvert.SerializeObject(dt);
            }
            return dtJson;
        }
        public string SaveQrStyle()
        {
            string masterData = System.Web.HttpContext.Current.Request.Form["masterData"];
            DataTable masterDt = new DataTable();
            if (!string.IsNullOrWhiteSpace(masterData))
            {
                masterDt = DataConverterHelper.ToDataTable(masterData, "select * from fg3_qrcode_style");
            }  
            ResponseResult result = new ResponseResult();
            try
            {
                result = proxy.SaveQrStyle(masterDt);
            }
            catch (Exception ex)
            {
                result.Status = "error";
                result.Msg = ex.Message.ToString();
            }

            string response = JsonConvert.SerializeObject(result);
            return response;
        }

        public string AddIconUpload(string name)
        {
            try
            {
                //图片唯一名
                string guid = Guid.NewGuid().ToString();
                name = guid +"logo" + name.Substring(name.LastIndexOf("."));
                proxy.SaveImgName(name);
                //指定路径，不存在则创建文件夹
                string strPath = "NG3Resource\\QRCodeLogo";
                string dirPath = Request.PhysicalApplicationPath + strPath;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                //遍历删除其他文件
                string[] files = Directory.GetFiles(dirPath);
                foreach (var file in files)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                HttpPostedFileBase imgFile = Request.Files["addCustomIcon"];
                string filePath = dirPath + "\\" + name;
                imgFile.SaveAs(filePath);

                //byte[] buffer = GetFileData(filePath);
                //string attachid = AttachUpload(buffer);

                return "true";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string BuildQRCode()
        {
            try
            {
                string masterData = System.Web.HttpContext.Current.Request.Form["masterData"];
                DataTable masterDt = new DataTable();
                if (!string.IsNullOrWhiteSpace(masterData))
                {
                    masterDt = DataConverterHelper.ToDataTable(masterData, "select * from fg3_qrcode_style");
                    string imgName = proxy.GetImgName();
                    string strPath = "NG3Resource\\QRCodeLogo\\"+ imgName;
                    string dirPath = Request.PhysicalApplicationPath + strPath;
                    int imgsize = 0;
                    int.TryParse(masterDt.Rows[0]["imgsize"].ToString(), out imgsize);
                    Color BackC;
                    Color ForeC;
                    switch (masterDt.Rows[0]["backcolor"].ToString())
                    {
                        case "1":
                            BackC = Color.White;
                            break;
                        case "2":
                            BackC = Color.PowderBlue;
                            break;
                        case "3":
                            BackC = Color.NavajoWhite;
                            break;
                        case "4":
                            BackC = Color.LightGray;
                            break;
                        default:
                            BackC = Color.White;
                            break;
                    }
                    switch (masterDt.Rows[0]["frontcolor"].ToString())
                    {
                        case "1":
                            ForeC = Color.Black;
                            break;
                        case "2":
                            ForeC = Color.DodgerBlue;
                            break;
                        case "3":
                            ForeC = Color.Maroon;
                            break;
                        case "4":
                            ForeC = Color.DarkOrange; 
                            break;
                        default:
                            ForeC = Color.Black;
                            break;
                    }
                    Bitmap bitmap = Generate3(dirPath, 
                        masterDt.Rows[0]["content"].ToString(),
                        imgsize,
                        imgsize,
                        masterDt.Rows[0]["addLogo"].ToString(),
                        BackC, 
                        ForeC);
                    strPath = "NG3Resource\\QRCodeLogo\\qrcode.png";
                    dirPath = Request.PhysicalApplicationPath + strPath;
                    bitmap.Save(dirPath, System.Drawing.Imaging.ImageFormat.Png);
                    return "true";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetImgName()
        {
            return proxy.GetImgName();
        }

        public string GetGridByBusTree(string busphid)
        {
            DataTable dt = proxy.GetGridByBusTree(busphid);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }

        public string getUrlByCode(string code)
        {
            DataTable dt = proxy.getUrlByCode(code);
            return DataConverterHelper.ToJson(dt, 1);
        }

        public string GetDataByForm(string code, string formJson)
        {
            string phid = proxy.getPhidByCode(code);
            JArray arr = JsonConvert.DeserializeObject(formJson) as JArray;
            DataTable masterDt = proxy.GetMaster(phid);
            DataTable detailDt = proxy.GetDetailField(phid);

            JObject result = new JObject();
            result.Add("code", code);

            JObject resultData = new JObject();
            for (int i = 0; i < arr.Count; i++)
            {
                string tablename = arr[i].ToObject<JObject>()["bindtable"].ToString();
                DataRow[] drs = detailDt.Select("tablename ='" + tablename + "'");
                if (drs.Length < 1)
                {
                    continue;
                }
                JToken data = arr[i].ToObject<JObject>()["data"];
                JObject dataObj = data as JObject;
                if (dataObj != null)
                {
                    foreach (var item in dataObj)
                    {
                        DataRow[] dataDrs = detailDt.Select("tablename ='" + tablename + "' and property ='" + item.Key + "'");
                        if (dataDrs.Length > 0)
                        {
                            resultData.Add(dataDrs[0]["name"].ToString(), item.Value);
                        }
                    }
                }
            }
            result.Add("data", resultData);
            string resultString = JsonConvert.SerializeObject(result);
            return resultString;
        }
    }

}
