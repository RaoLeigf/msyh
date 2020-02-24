using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NG3.SUP.Frame
{
    [Serializable]
    public class NGAppInfo// : ISerializable
    {
        public NGAppInfo()
        {
        }

        #region private field
        private string _cultureName;
        private string _pubConnectString;
        private string _userConnectString;
        private string _loginId;
        private string _userType;
        private string _oCode;
        private string _resBaseName;
        #endregion

        #region public Attribute
        /// <summary>
        /// 当前语言串，采用标准.Net对culture的命名
        /// </summary>
        public string CultureName
        {
            get { return _cultureName; }
            set { _cultureName = value; }
        }

        /// <summary>
        /// 公共数据库连接串
        /// </summary>
        public string PubConnectString
        {
            get { return _pubConnectString; }
            set { _pubConnectString = value; }
        }

        /// <summary>
        /// 用户数据库连接串
        /// </summary>
        public string UserConnectString
        {
            get { return _userConnectString; }
            set { _userConnectString = value; }
        }

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        public string LoginID
        {
            get { return _loginId; }
            set { _loginId = value; }
        }

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        /// <summary>
        /// 当前组织代码
        /// </summary>
        public string OCode
        {
            get { return _oCode; }
            set { _oCode = value; }
        }

        /// <summary>
        /// 资源文件名，对于一个应用用同一个名称，即使对于不同的项目
        /// </summary>
        public string ResBaseName
        {
            get { return _resBaseName; }
            set { _resBaseName = value; }
        }

        #endregion
    }

    [Serializable]
    public class NGWebAppInfo : NGAppInfo
    {
        public NGWebAppInfo()
            : base()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region private field
        private string _ngCommonPath;
        private string _ngResourcePath;
        #endregion

        #region public Attribute
        /// <summary>
        /// 公共页面项目路径
        /// </summary>
        public string NGCommonPath
        {
            get { return _ngCommonPath; }
            set { _ngCommonPath = value; }
        }

        /// <summary>
        /// 公共资源路径
        /// </summary>
        public string ResourcePath
        {
            get { return _ngResourcePath; }
            set { _ngResourcePath = value; }
        }

        #endregion

        #region public static field
        /// <summary>
        /// 全局应用对象在Session的名称
        /// </summary>
        public static string NameInSession = "NGWebAppInfo";
        #endregion
    }

    [Serializable]
    public class I6WebAppInfo : NGWebAppInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public I6WebAppInfo()
            : base()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region private field

        private string product;							//产品代码：I6|W3
        private string orgName;							//组织名称
        private string userName;						//用户名称
        private string uCode;							//账套号
        private string uName;							//账套名
        private string currentYear;						//当前年度
        private int currentAccper;						//当前会计期
        private int qtyPrecision;						//数量精度
        private int prcPrecision;						//单价精度
        private string userHostName;					//操作终端主机名
        private string userHostAddress;					//操作终端IP地址
        private string physicalApplicationPath;			//应用程序物理路径
        private string applicationPath;					//应用程序路径
        private int listPageSize;						//DataGrid分页时--列表页面每页行数
        private int reportPageSize;						//DataGrid分页时--报表页面每页行数
        private int tradeEditType = 0;					//行业版本类别 0 标准版 1 医药版 2 服装版
        private bool isUSBUser = false;					//当前的登录用户是否为硬加密认证，true为经过硬加密认证
        private bool needUSBVal = false;					//当前的帐套是否启用“USB身份验证”，true为启用验证
        private string coreOrgCode = string.Empty;		//核心组织编码
        private string coreOrgName = string.Empty;		//核心组织名称
        private string customerNo = string.Empty;		//客户编码
        private string navproduce = string.Empty;       //导颔产品
        private int currentFinanceAccper = 0;   //当前财务会计期
        private string series;               //表示i6系列中的某一个变化 ＝“p"表示工程版本
        private int _dbindex;               //某一个数据库服务器名在DataBase.XML数据库列表中的index值, 用于单点登陆时组装连接串
        private string _tokenKey = string.Empty;   //统一身份认证串
        private string usergroup = string.Empty;
        #endregion

        #region public Attribute

        /// <summary>
        /// 产品代码：I6|W3|UIC|GE		2005-11-7日被梁越平修改，修改的目的为支持UIC
        /// </summary>
        public string Product
        {
            get
            {
                //removed by wangjf, 2010.4.2
                //if (product == null || (product.ToUpper() != "W3" && product.ToUpper() != "UIC" && product.ToUpper() != "M3" && product.ToUpper() != "GE")) product = "I6";
                if (product == null)
                {
                    product = "I6";//new ProductInfo().ProductCode;
                }
                return product;
            }
            set
            {
                product = value;
            }
        }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName
        {
            get { return orgName; }
            set { orgName = value; }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    
        /// <summary>
        /// 账套号
        /// </summary>
        public string UCode
        {
            get { return uCode; }
            set { uCode = value; }
        }
        /// <summary>
        /// 账套名
        /// </summary>
        public string UName
        {
            get { return uName; }
            set { uName = value; }
        }
        /// <summary>
        /// 当前年度
        /// </summary>
        public string CurYear
        {
            get { return currentYear; }
            set { currentYear = value; }
        }
        /// <summary>
        /// 当前会计期
        /// </summary>
        public int CurAccper
        {
            get { return currentAccper; }
            set { currentAccper = value; }
        }

        /// <summary>
        /// 数量小数位数
        /// </summary>
        public int QtyPrecision
        {
            get { return qtyPrecision; }
            set { qtyPrecision = value; }
        }
        
        /// <summary>
        /// 单价小数位数
        /// </summary>
        public int PrcPrecision
        {
            get { return prcPrecision; }
            set { prcPrecision = value; }
        }

        /// <summary>
        /// 汇率小数位数
        /// </summary>
        public int ExchRatePrecision
        {
            get { return prcPrecision; }
        }
        
        /// <summary>
        /// 操作终端主机名
        /// </summary>
        public string UserHostName
        {
            get { return userHostName; }
            set { userHostName = value; }
        }
        /// <summary>
        /// 操作终端IP地址
        /// </summary>
        public string UserHostAddress
        {
            get { return userHostAddress; }
            set { userHostAddress = value; }
        }
        
        /// <summary>
        ///	应用程序物理路径
        /// </summary>
        public string PhysicalApplicationPath
        {
            get { return physicalApplicationPath; }
            set { physicalApplicationPath = value; }
        }

        /// <summary>
        ///	应用程序路径
        /// </summary>
        public string ApplicationPath
        {
            get { return applicationPath; }
            set { applicationPath = value; }
        }

        /// <summary>
        ///	核心组织编码
        /// </summary>
        public string CoreOrgCode
        {
            get { return this.coreOrgCode; }
            set { coreOrgCode = value; }
        }

        /// <summary>
        ///	核心组织名称
        /// </summary>
        public string CoreOrgName
        {
            get { return this.coreOrgName; }
            set { coreOrgName = value; }
        }

        /// <summary>
        ///	DataGrid分页时--列表页面每页行数
        /// </summary>
        public int ListPageSize
        {
            get { return listPageSize; }
            set { listPageSize = value; }
        }

        /// <summary>
        ///	DataGrid分页时--报表页面每页行数
        /// </summary>
        public int ReportPageSize
        {
            get { return reportPageSize; }
            set { reportPageSize = value; }
        }

        /// <summary>
        ///	行业版本0 标准版 1 医药版 2 服装版
        /// </summary>
        public int TradeEditType
        {
            get { return tradeEditType; }
            set { tradeEditType = value; }
        }

        /// <summary>
        /// 当前登录用户是否为硬加密身份认证用户
        /// </summary>
        public bool IsUSBUser
        {
            get { return isUSBUser; }
            set { isUSBUser = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool NeedUSBVal
        {
            get { return needUSBVal; }
            set { needUSBVal = value; }
        }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerNo
        {
            get { return this.customerNo; }
            set { this.customerNo = value; }
        }

        /// <summary>
        /// 导颔产品
        /// </summary>
        public string NavProduce
        {
            get { return this.navproduce; }
            set { this.navproduce = value; }
        }

        /// <summary>
        /// 当前财务会计期
        /// </summary>
        public int CurrentFinanceAccper
        {
            get { return this.currentFinanceAccper; }
            set { this.currentFinanceAccper = value; }
        }

        /// <summary>
        /// 产品系列
        /// </summary>
        public string Series
        {
            get
            {
                if (this.series == null)
                {
                    this.series = string.Empty;//new ProductInfo().Series;
                }
                return this.series;
            }
            set { this.series = value; }
        }

        /// <summary>
        /// 数据库提供商
        /// </summary>
        public string DBVendor
        {
            get
            {
                string pubConnectString = PubConnectString.ToUpper();
                if (pubConnectString.IndexOf("SQLCLIENT") > 0)
                {
                    return "SqlServer";
                }
                else if (pubConnectString.IndexOf("ORACLECLIENT") > 0)
                {
                    return "Oracle";
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// 某一个数据库服务器名在DataBase.XML数据库列表中的index值,
        /// 用于单点登陆时组装连接串
        /// </summary>
        public int DBIndex
        {
            get { return _dbindex; }
            set { _dbindex = value; }
        }

        /// <summary>
        /// 检测sqlserver的版本
        /// </summary>
        private string _sqlserverversion;
        public string SqlServerVersion
        {
            get { return _sqlserverversion; }
            set { _sqlserverversion = value; }
        }

        /// <summary>
        /// 统一身份认证串
        /// </summary>
        public string TokenKey
        {
            get { return _tokenKey; }
            set { _tokenKey = value; }
        }

        public string UserGroup
        {
            get { return usergroup; }
            set { usergroup = value; }
        }
        #endregion

    }
}