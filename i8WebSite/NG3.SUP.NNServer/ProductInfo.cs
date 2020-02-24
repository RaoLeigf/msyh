using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.IO;

namespace NG3.SUP.NSServer
{
    public class ProductInfo
    {
        private static DataSet _productDS = null;
        private string _applicationPath = null;
        private string _productFile = "Product.xml";//产品注册文件
        private string _productFullName = "";//产品全称
        private string _productVer = "";//产品的版本
        private string _product = "PSP";//产品
        private int _productNum = 0;
        /// <summary>
        ///构造函数
        /// </summary>
        public ProductInfo()
        {
            //初始路径和产品配置文件全路径
            if (HttpContext.Current != null)
            {
                _applicationPath = HttpContext.Current.Request.PhysicalApplicationPath.ToString();
            }
            else
            {
                _applicationPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                if (!_applicationPath.ToLower().EndsWith("ngwebsite"))
                {
                    _applicationPath = _applicationPath.Remove(_applicationPath.ToLower().IndexOf("ngwebsite") + 9);
                }
            }
            _productFile = Path.Combine(_applicationPath, "Product.xml");

            if (!this.ReadProduct())
            {
                throw new Exception("产品文件读取失败!请检查您的安装");
            }
        }
        /// <summary>
        /// 读产品信息Product.xml
        /// </summary>
        /// <returns></returns>
        public bool ReadProduct()
        {
            if (_productDS == null)
            {
                try
                {
                    _productDS = new DataSet("Newgrand");
                    _productDS.ReadXml(_productFile);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 从system中获得产品信息 产品全名称 + 版本
        /// </summary>
        /// <returns></returns>
        public string ProductInformation
        {
            get
            {
                return ProductFullName + " Ver" + ProductVersion;
            }
        }
        /// <summary>
        /// 从当前产品全名称
        /// </summary>
        /// <returns></returns>
        public string ProductFullName
        {
            get
            {
                try
                {
                    _productFullName = _productDS.Tables["Product"].Rows[0]["FullName"].ToString();
                }
                catch
                { }
                return _productFullName;
            }
        }
        /// <summary>
        /// 当前产品的版本
        /// </summary>
        public string ProductVersion
        {
            get
            {
                try
                {
                    _productVer = _productDS.Tables["Product"].Rows[0]["Version"].ToString();
                }
                catch { }
                return _productVer;
            }
        }
        /// <summary>
        /// 产品代码
        /// </summary>
        public string ProductCode
        {
            get
            {
                try
                {
                    _product = _productDS.Tables["Product"].Rows[0]["Code"].ToString();
                }
                catch { }
                return _product;
            }
        }
        /// <summary>
        /// i6系列，包括工程版本  ="p"
        /// </summary>
        public string Series
        {
            get
            {
                if (_productDS.Tables["Product"].Columns.Contains("Series"))
                    return _productDS.Tables["Product"].Rows[0]["Series"].ToString();
                else
                    return "";


            }
        }
        /// <summary>
        /// 产品编码
        /// </summary>
        public int ProductNum
        {
            get
            {
                return GetProductNum();
            }
        }
        /// <summary>
        /// 获取产品编码
        /// </summary>
        /// <returns></returns>
        private int GetProductNum()
        {
            string ProductLong = this.ProductCode + this.Series;
            if (ProductLong.ToUpper() == "PSP")
            {
                return 28;
            }
            else
            {
                return 0;
            }
        }
    }
}
