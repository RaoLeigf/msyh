using System;
using System.Data;
using NG.Data.DB;
using System.Web;
using i6.Help.Common;


namespace NG3.API.Service
{
    /// <summary>
    /// AccountSet。
    /// </summary>
    public class AccountSet : NGDbCommon
    {
        #region

        private static readonly string ProductXmlPath = @"product.xml";
        private static readonly string DataBaseXmlPath = @"{0}Config\DataBases.xml";
        private string _pubConnectString = string.Empty;
        private string _userConnectString = string.Empty;
        private string _product = string.Empty;

        #endregion

        #region Constructor
        public AccountSet()
        {

        }
        public AccountSet(string connectString)
            : base(connectString)
        {

        }
        #endregion

        #region public DataTable GetNGUsers()
        /// <summary>
        /// 取账套列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetNGUsers()
        {
            const string sSql = "select ucode,uname from ngusers  order by ucode ";
            return this.Sqlca.GetDataTable(sSql);
        }

        #endregion

        #region public string GetDefaultAccount(string fileName)

          /// <summary>
        /// 取缺省账套
        /// </summary>
        /// <returns></returns>
        public string GetDefaultAccount()
        {
            return GetDefaultAccount(GetDataBaseXmlPath());
        }

        /// <summary>
        /// 取缺省账套
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetDefaultAccount(string fileName)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(fileName);
                string defaultAccount = ds.Tables["AccConfig"].Rows[0]["DefaultAccount"].ToString();
                return defaultAccount;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region GetW3ConnectString

        public string GetProduct()
        {
            if (string.IsNullOrEmpty(_product))
            {
                string productXmlPath = ProductXmlPath;
                if (HttpContext.Current != null)
                {
                    productXmlPath = HttpContext.Current.Request.PhysicalApplicationPath + productXmlPath;
                }
                else
                {
                    productXmlPath = "..\\" + productXmlPath;
                }
                string product = InstallHelp.GetProduct(productXmlPath);
                _product = product;
            }
            return _product;
        }

        public string GetW3ConnectString()
        {
            if (string.IsNullOrEmpty(_pubConnectString))
            {
                _pubConnectString = GetW3ConnectString(GetDataBaseXmlPath());
            }
            this.ConnectString = _pubConnectString;
            return _pubConnectString;
        }

        private string GetDataBaseXmlPath()
        {
            //string productXmlPath = ProductXmlPath;
            string dataBaseXmlPath = DataBaseXmlPath;
            if (HttpContext.Current != null)
            {
                dataBaseXmlPath = HttpContext.Current.Request.PhysicalApplicationPath + dataBaseXmlPath;
            }
            else
            {
                dataBaseXmlPath = "..\\" + dataBaseXmlPath;
            }

            string configFile = string.Format(dataBaseXmlPath, GetProduct());
            return configFile;
        }

        public string GetDefaultServerName()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetDataBaseXmlPath());
            string defaultServerName = ds.Tables["ConnectDB"].Rows[0]["LastDB"].ToString();
            return defaultServerName;
        }

        public string GetW3ConnectString(string fileName)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(fileName);
            string defaultServerName = ds.Tables["ConnectDB"].Rows[0]["LastDB"].ToString();
            int selectDb = int.Parse(defaultServerName);
            string provider = ds.Tables["Connect"].Rows[selectDb]["Provider"].ToString();
            string serverName = ds.Tables["Connect"].Rows[selectDb]["ServerName"].ToString();
            string dataBase = ds.Tables["Connect"].Rows[selectDb]["DataBase"].ToString();
            string logId = ds.Tables["Connect"].Rows[selectDb]["LogId"].ToString();
            string logPass = ds.Tables["Connect"].Rows[selectDb]["LogPass"].ToString();
            logPass = NG.NGEncode.DecodePassword(logPass, 128);
            string driver = ds.Tables["Connect"].Rows[selectDb]["Driver"].ToString();
            string connectType = ds.Tables["Connect"].Rows[selectDb]["ConnectType"].ToString();
            string errorMsg;

            string cnString = SetConStr(provider, serverName, dataBase, logId, logPass, driver, connectType, out errorMsg);
            return cnString;
        }

        /// <summary>
        /// 得到数据库的连接信息
        /// </summary>
        /// <param name="provider">驱动程序提供商</param>
        /// <param name="serverName">服务器</param>
        /// <param name="dataBase">数据库</param>
        /// <param name="logId">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="driver">odbc驱动程序</param>
        /// <param name="connectType">连接类型</param>
        /// <param name="errReturn">错误</param>
        /// <returns>连接串</returns>
        private static string SetConStr(string provider, string serverName, string dataBase, string logId, string passWord, string driver, string connectType, out string errReturn)
        {
            string ngConStr = "";	//连接串

            errReturn = "ConnectionStringEmpty";// "连接串为空";

            string dbType = GetConnType(connectType, provider);

            try
            {
                NGDbFactory ngDBFact = NGDbFactory.Instance;
                if (dbType.ToLower() == "sqlclient")
                    ngConStr = ngDBFact.ComboSqlConnString(serverName, dataBase, logId, passWord, "");
                if (dbType.ToLower() == "oledb")
                {
                    ngConStr = ngDBFact.ComboOleDbConnString(provider, serverName, dataBase, logId, passWord, "");
                    if (provider == "ASAProv")
                        ngConStr = string.Format("ConnectType=OleDb;Provider=MSDataShape;Data Source={0};uid={1};pwd={2}", serverName, logId, passWord);
                }
                if (dbType.ToLower() == "odbc")
                    ngConStr = "ConnectType=Odbc;DSN=" + serverName;
                if (dbType.ToLower() == "oracleclient")
                    ngConStr = ngDBFact.ComboOracleConnString(serverName, logId, passWord, "");
                if (dbType.ToLower() == "aseclient")
                    ngConStr = "ConnectType=AseClient;" + string.Format("Data Source={0};Port={1};UID={2};PWD={3};Database={4};", serverName, "5000", logId, passWord, dataBase);

                if (ngConStr != "")
                {
                    errReturn = NGDbFactory.TestConnection(ngConStr);

                    if (errReturn == "" || errReturn.StartsWith("调用的目标发生了异常"))
                    {
                        if (dbType.ToLower() != "odbc" && dbType.ToLower() != "asaclient" && provider != "ASAProv")
                            ngConStr = "ConnectType=" + dbType + ";" + ngConStr;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ngConStr;
        }

        private static string GetConnType(string connectType, string provider)
        {
            string dbType = "";
            switch (connectType)
            {
                case "1":
                    dbType = "OLEDB";
                    break;
                case "2":
                    dbType = "ODBC";
                    break;
                case "3":
                    dbType = "SqlClient";
                    if (provider.ToUpper().IndexOf("ORA") >= 0)
                    {
                        dbType = "ORACLEClient";
                    }
                    if (provider.ToUpper().IndexOf("ASA") >= 0)
                    {
                        dbType = "AsaClient";
                    }
                    if (provider.ToUpper().IndexOf("ASE") >= 0)
                    {
                        dbType = "AseClient";
                    }
                    break;
            }
            return dbType;
        }
        #endregion

        #region public string GetUserConnectString(string ucode)

        /// <summary>
        /// 得到UserConnectString
        /// </summary>
        /// <param name="ucode">账套代码</param>
        public string GetUserConnectString(string ucode)
        {
            if (string.IsNullOrEmpty(this.ConnectString))
            {
                GetW3ConnectString();
            }
            NGDbTrans db = NGDbFactory.Instance.GetDbTrans(this.ConnectString);
            //数据库连接类型
            DBCType dbcType = db.DBCType;
            //数据库类型
            DbVendor dbVendor = db.Vendor;
            string sServerName = "", sDbName = "", sDbLogID = "", sDbLogPass = "", sUserConnectString = "";

            sServerName = NG.NGConvert.GetKeyValue(this.ConnectString, "Data Source");
            if (sServerName == null || sServerName == "")
            {
                sServerName = NG.NGConvert.GetKeyValue(this.ConnectString, "Server");
            }
            sDbName = this.GetDbName(ucode);
            sDbLogID = NG.NGConvert.GetKeyValue(this.ConnectString, "UID");
            if (sDbLogID == null || sDbLogID == "")
            {
                sDbLogID = NG.NGConvert.GetKeyValue(this.ConnectString, "User ID");
            }
            sDbLogPass = NG.NGConvert.GetKeyValue(this.ConnectString, "PWD");
            if (sDbLogPass == null || sDbLogPass == "")
            {
                sDbLogPass = NG.NGConvert.GetKeyValue(this.ConnectString, "Password");
                if (sDbLogPass.Length == 128)
                {
                    sDbLogPass = NG.NGEncode.DecodePassword(sDbLogPass, 128);
                }
            }

            if (dbcType == DBCType.SqlClient) //SqlClient
            {
                sUserConnectString = NGDbFactory.Instance.ComboSqlConnString(sServerName, sDbName, sDbLogID, sDbLogPass, "");
            }
            else if (dbcType == DBCType.OleDb) //OleDb
            {
                if (dbVendor == DbVendor.SQLServer) //SQLServer
                {
                    sUserConnectString = NGDbFactory.Instance.ComboOleDbConnString(DbVendor.SQLServer, sServerName, sDbName, sDbLogID, sDbLogPass, "");
                }
                else if (dbVendor == DbVendor.Oracle) //Oracle
                {
                    sDbLogPass = this.GetOracleUserPassWord(ucode);
                    sUserConnectString = NGDbFactory.Instance.ComboOleDbConnString(DbVendor.Oracle, sServerName, "", sDbName, sDbLogPass, "");
                }
                string sProvider = NG.NGConvert.GetKeyValue(this.ConnectString, "Provider");
                if (sProvider.ToUpper() == "MSDATASHAPE") //NGServer
                {
                    sUserConnectString = NGDbFactory.Instance.ComboOleDbConnString(sDbName);
                }
            }
            else if (dbcType == DBCType.OracleClient) //OracleClient
            {
                sDbLogPass = this.GetOracleUserPassWord(ucode);
                sUserConnectString = NGDbFactory.Instance.ComboOracleConnString(sServerName, sDbName, sDbLogPass, "");
            }
            if (dbcType == DBCType.AseClient)//AseClient
            {
                sUserConnectString = "ConnectType=AseClient;" + string.Format("Data Source={0};Port={1};UID={2};PWD={3};Database={4};", sServerName, "5000", sDbLogID, sDbLogPass, sDbName);
            }
            else if (dbcType == DBCType.Odbc) //ODBC
            {
                string sDsn = this.GetDbName(ucode);
                sUserConnectString = "Dsn=" + sDsn;
            }
            if (!sUserConnectString.StartsWith("ConnectType=", StringComparison.CurrentCultureIgnoreCase))
            {
                sUserConnectString = "ConnectType=" + dbcType.ToString() + ";" + sUserConnectString;
            }
            return sUserConnectString;
        }



        #endregion

        #region public string GetOracleUserPassWord(string ucode)
        /// <summary>
        /// 数据库为Oracle时,得到帐套用户的密码
        /// </summary>
        /// <param name="ucode">帐套号</param>
        /// <returns></returns>
        public string GetOracleUserPassWord(string ucode)
        {
            string sSql, sPassword;
            try
            {
                sSql = "select max(uyear) from ngyeardb where ucode= '" + ucode + "'";
                string sMaxYear = this.Sqlca.GetString(sSql);
                sSql = "select dblogpass from ngyeardb where  ucode= '" + ucode + "' and uyear='" + sMaxYear + "'";
                sPassword = this.Sqlca.GetString(sSql);
                if (sPassword == null || sPassword == string.Empty) sPassword = "";
            }
            catch
            {
                return null;
            }
            //密码解密
            sPassword = NG.NGEncode.DecodePassword(sPassword, 128);
            return sPassword;
        }

        #endregion

        #region public string GetCIOUserID()
        /// <summary>
        /// 得到CIO的用户代码
        /// </summary>
        /// <returns></returns>
        public string GetCIOUserID()
        {
            string sUserID = "";
            try
            {
                const string sSql = "select cname from ngrights where id=1";
                sUserID = this.Sqlca.GetString(sSql);
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message.ToString();
                return "";
            }
            return sUserID;
        }

        #endregion

        #region public string GetCIOPassword()
        /// <summary>
        /// 取　CIO　密码
        /// </summary>
        /// <returns></returns>
        public string GetCIOPassword()
        {
            string sPassword;
            try
            {
                const string sSql = "select cpwd  from ngrights where id = 1";
                sPassword = this.Sqlca.GetString(sSql);
                if (string.IsNullOrEmpty(sPassword)) sPassword = "";
            }
            catch
            {
                return "";
            }
            if (!string.IsNullOrEmpty(sPassword) && sPassword.Length < 128)
            {
                throw new Exception("invalid password");
            }
            //密码解密
            sPassword = NG.NGEncode.DecodePassword(sPassword, 128);
            return sPassword;
        }

        #endregion

        #region public string GetDbName(AUCode)
        /// <summary>
        /// 得到帐套数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDbName(string auCode)
        {
            string sDbName;
            try
            {
                string sSql = "select dbname from ngusers where ucode='" + auCode + "'";
                sDbName = this.Sqlca.GetString(sSql);
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return "";
            }
            return sDbName;
        }
        #endregion
    }
}
