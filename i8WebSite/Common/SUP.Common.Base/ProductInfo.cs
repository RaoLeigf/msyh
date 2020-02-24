#region Summary
//=========================================================================================
//
// Ower:	Maxl
//
// Project:	HR-3100
//
// Flie:	ProductInfo.cs
//
// Contents:��ȡProduct.xml����
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
	/// Product ��ժҪ˵����
	/// </summary>
	public class ProductInfo
	{
		#region ˽�б���
		private string _productFile = "Product.xml";		//��Ʒע���ļ�
		private string _aboutBMP ="about.jpg";				//����ͼƬ
		private string _product="W3";						//��Ʒ
		private string _productFullName="";					//��Ʒȫ��
		private string _productVer="";						//��Ʒ�İ汾
		private string _productConfig="";					//��Ʒ�Ĺ���ͼƬ
		private string _applicationPath=null;				//Ӧ�õ�����·��
		private const string _DEMO = "(��ʾ��)";
     

		private static DataSet _productDS = null;
		#endregion

		#region ����
        /// <summary>
        /// 
        /// </summary>
		public ProductInfo()
		{
			//��ʼ·���Ͳ�Ʒ�����ļ�ȫ·��
            if(HttpContext.Current!=null)
            {
                _applicationPath = HttpContext.Current.Request.PhysicalApplicationPath.ToString();
            }
            else
            {
                //LinL Ϊ�˽��M3ͨ��NG.UP.TimerService.exe�̵߳��õ��� HttpContext.Current�����ڵ�����
                //_applicationPath = AppDomain.CurrentDomain.BaseDirectory.ToString().Replace("DMC\\TimerService","");
                _applicationPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                if (!_applicationPath.ToLower().EndsWith("ngwebsite"))
                {
                    _applicationPath = _applicationPath.Remove(_applicationPath.ToLower().IndexOf("ngwebsite") + 9);
                }
            }
			_productFile= Path.Combine(_applicationPath,"Product.xml");
			
			//��productd.xml����Ϣ
			if (! this.ReadProduct())
			{
                //this.ExistsProductFile = false;
				//throw new Exception("�ļ���ȡʧ��!�������İ�װ");
                //throw new Exception(i6.Biz.RS.CheckInstall);
			}
		}
		
		#endregion

		#region ��Ʒ��Ϣ ��XML�ж�ȡ����Ϣ

		/// <summary>
		/// ����Ʒ��ϢProduct.xml
		/// </summary>
		/// <returns></returns>
		public bool ReadProduct()
		{
			//����Ƿ�Ϊnull,��֤��һ��product.xml
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
		/// ��system�л�ò�Ʒ��Ϣ ��Ʒȫ���� + �汾
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
		/// �ӵ�ǰ��Ʒȫ����
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
		/// ��ǰ��Ʒ�İ汾
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
		/// ��ǰ��Ʒ�Ĺ���ͼƬ
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
		/// ��Ʒ����
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
		/// ��Ʒ�����ļ� һ��i6������,W3����
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
		/// �Ƿ�����׼� һ��i6Ϊyes W3��ƷΪno
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
        /// i6ϵ�У��������̰汾  ="p"
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

		#region ���ģ�������Ϣ ��XML�ж�ȡ,��Install������ʾ���ж�,�û�about
		
		/// <summary>
		/// ȡ���׼��İ�װ��Ϣ
		/// </summary>
		/// <returns></returns>
		public string ReadSuitInfo()
		{
			StringBuilder infoStr = new StringBuilder();
            //NGCOMEX ngCOMEX = NGCOMEX.GetProductInfo(this.ProductCode);

            //if (this.HasSuit)		//��������׼�
            //{
            //    for (int row=0;row<_productDS.Tables["SuitInfo"].Rows.Count;row++)
            //    {
            //        string suitCode= "";
            //        string suitName= "";
            //        string suitVersion = "";
            //        string suitconFile = "";
            //        bool installed=false;

            //        //�׼���ʶ
            //        if (_productDS.Tables["SuitInfo"].Rows[row]["Code"]!=null)
            //        {
            //            suitCode = _productDS.Tables["SuitInfo"].Rows[row]["Code"].ToString();
            //        }
            //        //û�а�װ�򷵻�
            //        if (ngCOMEX.MainModuleInstalled[suitCode]!=null)
            //        {
            //            installed=(bool)ngCOMEX.MainModuleInstalled[suitCode];
            //        }
            //        if ( ! installed)//.IsMainModuleInstalled(suitCode))
            //        {
            //            continue;
            //        }
					
            //        //�׼�����
            //        if (_productDS.Tables["SuitInfo"].Rows[row]["Name"]!=null)
            //        {
            //            suitName = _productDS.Tables["SuitInfo"].Rows[row]["Name"].ToString();
            //        }
            //        //�׼��汾
            //        if (_productDS.Tables["SuitInfo"].Rows[row]["Version"]!=null)
            //        {
            //            suitVersion = _productDS.Tables["SuitInfo"].Rows[row]["Version"].ToString();
            //        }
            //        //�׼�ģ������
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
		/// ͨ��ϵͳע���ļ�System.xml��õ�ǰϵͳ����Ϣ
		/// �Ѿ���װģ�����Ϣ ��Ҫʹ����about.aspx
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
            //    return "    (δ��װģ��)\r\n";
            //    //return i6.Biz.RS.InstallModule;
            //}
            //if (ds.Tables["Modules"]==null) return "    (δ��װģ��)\r\n";
            ////if (ds.Tables["Modules"] == null) return i6.Biz.RS.InstallModule;

            //NGCOMEX ngCOMEX = NGCOMEX.GetProductInfo(this.ProductCode);

            //for(int row=0;row<ds.Tables["Modules"].Rows.Count;row++)
            //{
            //    string moduleNo="";
            //    string ModuleName="";
            //    string Version = "";
            //    bool installed = false;

            //    //ģ����
            //    if (ds.Tables["Modules"].Rows[row]["Module"]!=null)
            //    {
            //        moduleNo = ds.Tables["Modules"].Rows[row]["Module"].ToString();
            //    }
            //    //��ǰģ���Ƿ�װ
            //    //if (! new NGCOM().IsModuleInstalled(moduleNo)) continue;
            //    if (ngCOMEX.ModuleInstalled[moduleNo]!=null)
            //    {
            //        installed=(bool)ngCOMEX.ModuleInstalled[moduleNo];
            //    }
				
            //    if (! installed) continue;


            //    //ģ������
            //    if (ds.Tables["Modules"].Rows[row]["ModuleName"]!=null)
            //    {
            //        ModuleName = ds.Tables["Modules"].Rows[row]["ModuleName"].ToString();
            //    }
            //    //ģ��汾
            //    if (ds.Tables["Modules"].Rows[row]["Version"]!=null)
            //    {
            //        Version = ds.Tables["Modules"].Rows[row]["Version"].ToString();
            //    }
				
            //    //���ģ��汾�Ͳ�Ʒ�汾��һ��,����ʾ
            //    //if (! Version.Equals(suitVersion))
            //    //{
            //        ModuleName +="(Ver "+Version +")";
            //    //}

            //    //��⵱ǰģ���Ƿ�Ϊ����
            //        bool demo = (bool)ngCOMEX.ModuleDemo[moduleNo];
                   
            //    //if ( new NGCOM().IsModuleDemo(moduleNo))
            //    if (demo)
            //    {
            //        ModuleName += _DEMO;
            //    }
				
            //    //About����Ϣ��
            //    infosStr.Append("    "+ModuleName +"\r\n");
            //}
			return infosStr.ToString();
		}
		

		/// <summary>
		/// �õ���ǰ��Ʒ���׼���ģ����
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
        /// ���ݱ�����Ŀ¼�õ�M3��վ���ַ
        /// </summary>
        /// <param></param>
        /// <returns>����M3��վ���ַ</returns>
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
