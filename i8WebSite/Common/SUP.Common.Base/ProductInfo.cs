#region Summary
//=========================================================================================
//
// Ower:	Maxl
//
// Project:	HR-3100
//
// Flie:	ProductInfo.cs
//
// Contents:读取Product.xml的类
//
// VSS Archives:\\W3\W3_MAIN\CODES\i6SOLUTION\i6Base\i6Base\
//
// Version:	1.0.0.0
//
// Copyright (C) NewGrand Corporation. All rights reserved.
//
//=========================================================================================
//
// History: 2004/12/15	Created.
//
//=========================================================================================

#endregion

using System;
using System.Text;
using System.IO;
using System.Web;
using System.Data;


namespace SUP.Common.Base
{
	/// <summary>
	/// Product 的摘要说明。
	/// </summary>
	public class ProductInfo
	{
		#region 私有变量
		private string _productFile = "Product.xml";		//产品注册文件
		private string _aboutBMP ="about.jpg";				//帮助图片
		private string _product="W3";						//产品
		private string _productFullName="";					//产品全称
		private string _productVer="";						//产品的版本
		private string _productConfig="";					//产品的关于图片
		private string _applicationPath=null;				//应用的物理路径
		private const string _DEMO = "(演示版)";
     

		private static DataSet _productDS = null;
		#endregion

		#region 构造
        /// <summary>
        /// 
        /// </summary>
		public ProductInfo()
		{
			//初始路径和产品配置文件全路径
            if(HttpContext.Current!=null)
            {
                _applicationPath = HttpContext.Current.Request.PhysicalApplicationPath.ToString();
            }
            else
            {
                //LinL 为了解决M3通过NG.UP.TimerService.exe线程调用导致 HttpContext.Current不存在的问题
                //_applicationPath = AppDomain.CurrentDomain.BaseDirectory.ToString().Replace("DMC\\TimerService","");
                _applicationPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                if (!_applicationPath.ToLower().EndsWith("ngwebsite"))
                {
                    _applicationPath = _applicationPath.Remove(_applicationPath.ToLower().IndexOf("ngwebsite") + 9);
                }
            }
			_productFile= Path.Combine(_applicationPath,"Product.xml");
			
			//读productd.xml的信息
			if (! this.ReadProduct())
			{
                //this.ExistsProductFile = false;
				//throw new Exception("文件读取失败!请检查您的安装");
                //throw new Exception(i6.Biz.RS.CheckInstall);
			}
		}
		
		#endregion

		#region 产品信息 从XML中读取的信息

		/// <summary>
		/// 读产品信息Product.xml
		/// </summary>
		/// <returns></returns>
		public bool ReadProduct()
		{
			//检测是否为null,保证读一次product.xml
			if (_productDS==null)
			{
				try
				{
					_productDS = new DataSet("Newgrand");
					_productDS.ReadXml(_productFile);
                    this.ExistsProductFile = true;
				}
				catch
				{
					return false;
				}
			}
			
			return true;
		}

        public bool ExistsProductFile { get; set; }

		/// <summary>
		/// 从system中获得产品信息 产品全名称 + 版本
		/// </summary>
		/// <returns></returns>
		public string ProductInformation
		{
			get
			{
				return ProductFullName +" Ver" + ProductVersion;
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
					_productFullName= _productDS.Tables["Product"].Rows[0]["FullName"].ToString();
				}
				catch
				{}
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
					_productVer		= _productDS.Tables["Product"].Rows[0]["Version"].ToString();
				}
				catch{}
				return _productVer;
			}
		}
		
		/// <summary>
		/// 当前产品的关于图片
		/// </summary>
		public string ProductAboutBMP
		{
			get
			{
				try
				{
					_aboutBMP		= _productDS.Tables["Product"].Rows[0]["AboutPic"].ToString();
				}
				catch{}
				return _aboutBMP;
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
					_product		= _productDS.Tables["Product"].Rows[0]["Code"].ToString();
				}
				catch{}
				return _product;
			}
		}
		
		/// <summary>
		/// 产品配置文件 一般i6不存在,W3存在
		/// </summary>
		public string ProductConfig
		{
			get
			{
				try
				{
					_productConfig  = _productDS.Tables["Product"].Rows[0]["ConfigFile"].ToString(); 
				}
				catch{}
				return _productConfig;
			}
		}

		/// <summary>
		/// 是否包括套件 一般i6为yes W3产品为no
		/// </summary>
		public bool HasSuit
		{
			get
			{
				string sTemp = "";
				try
				{
					sTemp = _productDS.Tables["Product"].Rows[0]["Suit"].ToString();
				}
				catch{}
				return (sTemp=="yes");
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

		#endregion

		#region 获得模块相关信息 从XML中读取,加Install检测和演示版判断,用户about
		
		/// <summary>
		/// 取得套件的安装信息
		/// </summary>
		/// <returns></returns>
		public string ReadSuitInfo()
		{
			StringBuilder infoStr = new StringBuilder();
            //NGCOMEX ngCOMEX = NGCOMEX.GetProductInfo(this.ProductCode);

            //if (this.HasSuit)		//如果包含套件
            //{
            //    for (int row=0;row<_productDS.Tables["SuitInfo"].Rows.Count;row++)
            //    {
            //        string suitCode= "";
            //        string suitName= "";
            //        string suitVersion = "";
            //        string suitconFile = "";
            //        bool installed=false;

            //        //套件标识
            //        if (_productDS.Tables["SuitInfo"].Rows[row]["Code"]!=null)
            //        {
            //            suitCode = _productDS.Tables["SuitInfo"].Rows[row]["Code"].ToString();
            //        }
            //        //没有安装则返回
            //        if (ngCOMEX.MainModuleInstalled[suitCode]!=null)
            //        {
            //            installed=(bool)ngCOMEX.MainModuleInstalled[suitCode];
            //        }
            //        if ( ! installed)//.IsMainModuleInstalled(suitCode))
            //        {
            //            continue;
            //        }
					
            //        //套件名称
            //        if (_productDS.Tables["SuitInfo"].Rows[row]["Name"]!=null)
            //        {
            //            suitName = _productDS.Tables["SuitInfo"].Rows[row]["Name"].ToString();
            //        }
            //        //套件版本
            //        if (_productDS.Tables["SuitInfo"].Rows[row]["Version"]!=null)
            //        {
            //            suitVersion = _productDS.Tables["SuitInfo"].Rows[row]["Version"].ToString();
            //        }
            //        //套件模块配置
            //        if (_productDS.Tables["SuitInfo"].Rows[row]["ConfigFile"]!=null)
            //        {
            //            suitconFile = _productDS.Tables["SuitInfo"].Rows[row]["ConfigFile"].ToString();
            //        }
					
            //        //
            //        infoStr.Append(suitName);

            //        //if ( ! suitVersion.Equals(ProductVersion))
            //        //{
            //            infoStr.Append( "(Ver "+ suitVersion + ")");
            //        //}
            //        infoStr.Append("\r\n");
            //        infoStr.Append(this.GetInstallInfo( Path.Combine(_applicationPath,this.ProductCode+"Config\\"+suitconFile),suitVersion));

            //    }
            //}
            //else
            //{
            //    infoStr.Append(this.GetInstallInfo( Path.Combine(_applicationPath,this.ProductCode+"Config\\"+this.ProductConfig),ProductVersion));
            //}

			return infoStr.ToString();
		}
		/// <summary>
		/// 通过系统注册文件System.xml获得当前系统的信息
		/// 已经安装模块的信息 主要使用在about.aspx
		/// </summary>
		/// <returns></returns>
		private string GetInstallInfo(string configFile,string suitVersion)
		{
			StringBuilder infosStr = new StringBuilder();
            //DataSet ds = new DataSet("SuitInfo");
            //try
            //{
            //    ds.ReadXml(configFile);
            //}
            //catch
            //{
            //    return "    (未安装模块)\r\n";
            //    //return i6.Biz.RS.InstallModule;
            //}
            //if (ds.Tables["Modules"]==null) return "    (未安装模块)\r\n";
            ////if (ds.Tables["Modules"] == null) return i6.Biz.RS.InstallModule;

            //NGCOMEX ngCOMEX = NGCOMEX.GetProductInfo(this.ProductCode);

            //for(int row=0;row<ds.Tables["Modules"].Rows.Count;row++)
            //{
            //    string moduleNo="";
            //    string ModuleName="";
            //    string Version = "";
            //    bool installed = false;

            //    //模块编号
            //    if (ds.Tables["Modules"].Rows[row]["Module"]!=null)
            //    {
            //        moduleNo = ds.Tables["Modules"].Rows[row]["Module"].ToString();
            //    }
            //    //当前模块是否安装
            //    //if (! new NGCOM().IsModuleInstalled(moduleNo)) continue;
            //    if (ngCOMEX.ModuleInstalled[moduleNo]!=null)
            //    {
            //        installed=(bool)ngCOMEX.ModuleInstalled[moduleNo];
            //    }
				
            //    if (! installed) continue;


            //    //模块名称
            //    if (ds.Tables["Modules"].Rows[row]["ModuleName"]!=null)
            //    {
            //        ModuleName = ds.Tables["Modules"].Rows[row]["ModuleName"].ToString();
            //    }
            //    //模块版本
            //    if (ds.Tables["Modules"].Rows[row]["Version"]!=null)
            //    {
            //        Version = ds.Tables["Modules"].Rows[row]["Version"].ToString();
            //    }
				
            //    //如果模块版本和产品版本不一致,则显示
            //    //if (! Version.Equals(suitVersion))
            //    //{
            //        ModuleName +="(Ver "+Version +")";
            //    //}

            //    //检测当前模块是否为正版
            //        bool demo = (bool)ngCOMEX.ModuleDemo[moduleNo];
                   
            //    //if ( new NGCOM().IsModuleDemo(moduleNo))
            //    if (demo)
            //    {
            //        ModuleName += _DEMO;
            //    }
				
            //    //About的信息串
            //    infosStr.Append("    "+ModuleName +"\r\n");
            //}
			return infosStr.ToString();
		}
		

		/// <summary>
		/// 得到当前产品中套件的模块编号
		/// </summary>
		/// <param name="suitCode"></param>
		/// <returns></returns>
		public string GetSuitVersion(string suitCode)
		{
			string version = "";
			if (this.ProductCode=="W3")
			{
				version = this.ProductVersion;
			}
			else
			{
				if (_productDS.Tables["SuitInfo"]!=null)
				{
					DataView dv=_productDS.Tables["SuitInfo"].DefaultView;
					dv.RowFilter=" Code='"+suitCode+"'";
					if (dv.Count>0) 
					{
						version = dv[0]["version"].ToString();
					}
				}
			}
			return version;
		}

        /// <summary>
        /// 根据本机根目录得到M3的站点地址
        /// </summary>
        /// <param></param>
        /// <returns>返回M3的站点地址</returns>
        public static string GetM3WebSite()
        {
            //string rootPath = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            //string sRegeditItem = "";
            //sRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\M3MidServer";
            //string sReturn = NG.Win32.RegistryService.GetString(sRegeditItem,"WebSite_i6DMC_M3");
            //string port = NG.Win32.RegistryService.GetString(sRegeditItem, "IISPort");
            
            //if (port == "" || port == null)
            //{
            //    return "";
            //}
            //if (sReturn == "" || sReturn == null)
            //    sReturn = "M3";
            //int index;
            //index = rootPath.IndexOf(':');
            //if (index >= 0)
            //{
            //    rootPath = @"http://" + rootPath.Substring(0, index) +"/:"+port;
            //}
            //else
            //{
            //    rootPath = @"http://" + rootPath + ":" + port;
            //}
            //sReturn = rootPath + "/" + sReturn;

            return "";//sReturn;
        }

		#endregion

	}
}
