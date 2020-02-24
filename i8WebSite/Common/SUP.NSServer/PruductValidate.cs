using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3;

namespace NG3.SUP.NSServer
{
    public class PruductValidate
    {
        /// <summary>
        /// 数据库串
        /// </summary>
        private string UserConnectString = string.Empty;
        /// <summary>
        /// 产品信息类
        /// </summary>
        private ProductInfo Product;
        /// <summary>
        /// NSServer
        /// </summary>
        private NGNSServerService NSServer;
        /// <summary>
        /// 
        /// </summary>
        private NG.LzwZip lzwZipService = new NG.LzwZip();		
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_userConnectString"></param>
        public PruductValidate(string _userConnectString) 
        {
            this.UserConnectString = _userConnectString;
            this.Product = new ProductInfo();
            SetNSServer();
        }
        /// <summary>
        /// 设置NSServer服务
        /// </summary>
        private void SetNSServer()
        {
            NSServer = new NGNSServerService();
            string Url = "{0}://{1}:{2}"+ "/NSServer/default.aspx";
            Url = string.Format(Url,
                System.Web.HttpContext.Current.Request.Url.Scheme,
                System.Web.HttpContext.Current.Request.Url.Host,
                System.Web.HttpContext.Current.Request.Url.Port);

            NSServer.Url = Url;
        }

        #region 数量控制处理
        /// <summary>
        /// 检测相关的类型正版控制是否超出
        /// </summary>
        /// <param name="bType"></param>
        /// <param name="SqlFilter"></param>
        /// <returns></returns>
        public bool IsDemoEnd(string bType, Dictionary<string,string> SqlFilter)
        {
            int CurRecords = 0;
            int DemoCtrl = 1;
            return this.IsDemoEnd(bType, ref CurRecords, ref DemoCtrl, SqlFilter);
        }
        /// <summary>
        /// 检测相关的类型正版控制是否超出
        /// </summary>
        /// <param name="bType"></param>
        /// <returns></returns>
        public bool IsDemoEnd(string bType)
        {
            int CurRecords = 0;
            int DemoCtrl = 1;
            return this.IsDemoEnd(bType, ref CurRecords, ref DemoCtrl);
        }
        /// <summary>
        /// 检测相关的类型正版控制是否超出
        /// </summary>
        /// <param name="bType"></param>
        /// <param name="curNum"></param>
        /// <param name="ctlNum"></param>
        /// <returns></returns>
        public bool IsDemoEnd(string bType, ref int curNum, ref int ctlNum)
        {
            return IsDemoEnd(bType, ref curNum, ref ctlNum, null);
        }
        /// <summary>
        /// 检测相关的类型正版控制是否超出
        /// </summary>
        /// <param name="bType">控制类型</param>
        /// <param name="CurRecords">当前数量</param>
        /// <param name="DemoCtrl">控制数量</param>
        /// <param name="SqlFilter">过滤串</param>
        /// <returns></returns>
        public bool IsDemoEnd(string bType, ref int CurRecords, ref int DemoCtrl, Dictionary<string, string> SqlFilter)
        {
            CurRecords = GetCurRecords(bType, SqlFilter);
            DemoCtrl =GetDemoCtrl(bType);

            return CurRecords >= DemoCtrl;
        }
        /// <summary>
        /// 获取当前帐套控制数
        /// </summary>
        /// <param name="bType"></param>
        /// <param name="SqlFilter"></param>
        /// <returns></returns>
        private int GetCurRecords(string bType,Dictionary<string,string> SqlFilter)
        {
            string Sql = string.Empty;
            int CurCount=0;
            switch (bType)
            {
                case "PAS"://沙盘活动数
                    Sql = " select count(*) from psp_activity where actstatus='2' or actstatus='3' ";
                    break;
                case "ASS":
                    Sql = "select count(*) from psp_user where activity='{0}' and userid in(select logid from secuser where usergroup='S')";
                    if (SqlFilter.ContainsKey("activity"))
                    {
                        Sql = string.Format(Sql, SqlFilter["activity"]);
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(Sql))
            {
                Object ret = NG3.Data.Service.DbHelper.ExecuteScalar(UserConnectString, Sql);
                try
                {
                    CurCount = Convert.ToInt32(ret);
                }
                catch
                {
 
                }
            }

            return CurCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bType"></param>
        /// <returns></returns>
        public int GetDemoCtrl(string bType)
        {
            int DemoCrtlCount = 0;
            switch (bType)
            {
                case "PAS"://沙盘活动数
                    DemoCrtlCount = GetNum(bType, 1);
                    break;
                case "ASS"://活动规模
                    DemoCrtlCount = GetNum(bType, 5);
                    break;
            }

            return DemoCrtlCount;
        }
        /// <summary>
        /// 获取控制数，如果是演示版直接返回演示版控制数
        /// </summary>
        /// <param name="bType"></param>
        /// <param name="DemoCrtlDefaultCount"></param>
        /// <returns></returns>
        private int GetNum(string bType, int DemoCrtlDefaultCount)
        {
            string sRetu = string.Empty;
            try
            {
                string SN = NSServer.License_GetSN(0, true, Product.ProductNum);
                SN = lzwZipService.Level2DecodeFromBase64(SN);

                if (string.IsNullOrEmpty(SN) || SN.Equals("00000"))
                {
                    return DemoCrtlDefaultCount;
                }
                else
                {
                    sRetu = NSServer.License_GetProperty(0, true, Product.ProductNum, bType);
                    sRetu = lzwZipService.Level2DecodeFromBase64(sRetu);
                    if (string.IsNullOrEmpty(sRetu))
                    {
                        sRetu = DemoCrtlDefaultCount.ToString();
                    }

                    return int.Parse(sRetu);
                }
            }
            catch
            {
                return DemoCrtlDefaultCount;
            }
        }
        /// <summary>
        /// 获取合法单位名
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            try
            {
                var retString = NSServer.License_GetProperty(0, true, Product.ProductNum, "Username");
                retString = lzwZipService.Level2DecodeFromBase64(retString);
                if (string.IsNullOrEmpty(retString))
                {
                    retString = "Temporary Licenced User";
                }

                return retString;
            }
            catch (Exception )
            {
                return "Temporary Licenced User";
            }
        }
        /// <summary>
        /// 获取产品狗编码
        /// </summary>
        /// <returns></returns>
        public string GetSN()
        {
            try
            {
                string SN = NSServer.License_GetSN(0, true, Product.ProductNum);
                SN = lzwZipService.Level2DecodeFromBase64(SN);
                if (string.IsNullOrEmpty(SN))
                {
                    SN = "00000";
                }

                return SN;
            }
            catch (Exception)
            {
                return "000000";
            }
        }

        #endregion
    }
}
