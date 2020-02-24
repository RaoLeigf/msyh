using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.IO;
using NG3.Data;
using NG3.Data.Service;

namespace SUP.Common.Base
{
    public class DBConnectionStringBuilder
    {
        private const string DBXML = "DataBaseConfig";
        private static string isOracle = "1";	//"1"是oracle数据库，"0"是sqlserver数据库
        private static ProductInfo productInfo = null;
        private string _databasefile = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBConnectionStringBuilder()
        {
            try
            {
                productInfo = new ProductInfo();
            }
            catch (Exception)
            {               
                
            }
        }

        public DBConnectionStringBuilder(string databasefile)
        {
            _databasefile = databasefile;
        }

        public DataSet DBConfig
        {
            get
            {
               DataSet ds = HttpRuntime.Cache[DBXML] as DataSet;
               if (ds == null)
               {
                   ds =  Read();

                   //缓存到进程Cache
                   HttpRuntime.Cache.Insert(DBXML, ds, null,
                       System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), System.Web.Caching.CacheItemPriority.NotRemovable, null);
               }

               return ds;
            }
        }

        /// <summary>
        /// 取数据库列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDBserverList()
        {
            return this.DBConfig.Tables["Connect"];
        }

        /// <summary>
        /// 获取dbservername服务器NgSoft中的账套列表
        /// <param></param>
        /// <returns></returns>
        public DataTable GetAccountList(string dbservername)
        {
            string temp = null;
            string pubconnectstring = this.GetMainConnStringElement(dbservername,out temp);

            DataTable accDTable = new DataTable();
            string sqlstr = "select ngusers.ucode as ucode,ngusers.uname as uname,ngusers.dbname from ngusers order by ngusers.ucode asc";
            try
            {
                accDTable = DbHelper.GetDataTable(pubconnectstring, sqlstr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return accDTable;
        }

        /// <summary>
        /// 从_DBXmlDS中取得连接串的元素
        /// </summary>
        /// <param name="selectIndex">所选择的数据的索引</param>
        /// <param name="theResult" ></param>
        /// <returns>连接串</returns>
        public string GetMainConnStringElement(string dbservername, out string theResult)
        {
            return GetMainConnStringElement(dbservername, out theResult, false);
        }

        /// <summary>
        /// 从_DBXmlDS中取得连接串的元素
        /// </summary>
        /// <param name="selectIndex">所选择的数据的索引</param>
        /// <param name="theResult" ></param>
        /// <returns>连接串</returns>
        public string GetMainConnStringElement(string dbservername, out string theResult, bool isADO)
        {

            string provider;	//连接方式
            string serverName;	//服务器名
            string dataBase;	//数据库名
            string logId;	//登录名称
            string logPass;	//密码
            string driver;	//Odbc驱动类型
            string connectType;	//连接类型
            string tempStr = "";
            string pubConnString = "";

            DataSet dBXmlDS = this.DBConfig;

            int selectIndex = GetDBIndex(dBXmlDS, dbservername);

            provider = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Provider"].ToString();
            serverName = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ServerName"].ToString();
            if (serverName == ".")
            {
                serverName = HttpContext.Current.Request.ServerVariables["Local_Addr"].ToString();
            }
            dataBase = dBXmlDS.Tables["Connect"].Rows[selectIndex]["DataBase"].ToString();
            logId = dBXmlDS.Tables["Connect"].Rows[selectIndex]["LogId"].ToString();
            logPass = dBXmlDS.Tables["Connect"].Rows[selectIndex]["LogPass"].ToString();
            logPass = NG3.NGEncode.DecodePassword(logPass, 128);
            driver = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Driver"].ToString();
            connectType = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ConnectType"].ToString();

            if (serverName.Length < 1 && dataBase.Length < 1)
            {
                theResult = "请先设置数据库连接！";
                return "0000";
            }

            pubConnString = SetConStr(provider, serverName, dataBase, logId, logPass, driver, connectType, out theResult, isADO,true);

            tempStr = pubConnString.ToLower();         
            if (provider.Trim().ToUpper().IndexOf("ORA") > 0)
            {
                isOracle = "1";
            }
            else
            {
                isOracle = "0";
            }

            //selectDb = selectIndex;

            return pubConnString;
        }

        public string GetMainConnStringElement(int selectIndex, out string theResult, bool isADO)
        {

            string provider;	//连接方式
            string serverName;	//服务器名
            string dataBase;	//数据库名
            string logId;	//登录名称
            string logPass;	//密码
            string driver;	//Odbc驱动类型
            string connectType;	//连接类型
            string tempStr = "";
            string pubConnString = "";

            DataSet dBXmlDS = this.DBConfig;

            //int selectIndex = GetDBIndex(dBXmlDS, dbservername);

            provider = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Provider"].ToString();
            serverName = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ServerName"].ToString();
            if (serverName == ".")
            {
                serverName = HttpContext.Current.Request.ServerVariables["Local_Addr"].ToString();
            }
            dataBase = dBXmlDS.Tables["Connect"].Rows[selectIndex]["DataBase"].ToString();
            logId = dBXmlDS.Tables["Connect"].Rows[selectIndex]["LogId"].ToString();
            logPass = dBXmlDS.Tables["Connect"].Rows[selectIndex]["LogPass"].ToString();
            logPass = NG3.NGEncode.DecodePassword(logPass, 128);
            driver = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Driver"].ToString();
            connectType = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ConnectType"].ToString();

            if (serverName.Length < 1 && dataBase.Length < 1)
            {
                theResult = "请先设置数据库连接！";
                return "0000";
            }

            pubConnString = SetConStr(provider, serverName, dataBase, logId, logPass, driver, connectType, out theResult, isADO,true);

            tempStr = pubConnString.ToLower();
            if (provider.Trim().ToUpper().IndexOf("ORA") > 0)
            {
                isOracle = "1";
            }
            else
            {
                isOracle = "0";
            }

            //selectDb = selectIndex;

            return pubConnString;
        }

        /// <summary>
        /// 取得默认帐套连接串
        /// </summary>
        /// <returns></returns>
        public string GetDefaultConnString()
        {
            int  selectIndex = Convert.ToInt32(this.LastConnect);//取默认连接             

            string database = this.DefaultDB;//"NG" + this.DefaultDB;

            string result = string.Empty;

            string pubconnectstr = this.GetMainConnStringElement(selectIndex,out result,false);

            string userconnectstr = this.GetAccConnstringElement(selectIndex, database, pubconnectstr, out result);

            return userconnectstr;
        }

        /// <summary>
        /// 取得日志数据库连接串
        /// </summary>
        /// <returns></returns>
        public string GetLogDbConnString()
        {
            int selectIndex = Convert.ToInt32(this.LastConnect);//取默认连接             

            //string database = this.DefaultDB;//"NG" + this.DefaultDB;

            string result = string.Empty;

            string pubconnectstr = this.GetMainConnStringElement(selectIndex, out result, false);

            string userconnectstr = this.GetAccConnstringElement(selectIndex, "NGLog", pubconnectstr, out result);

            return userconnectstr;
        }

        /// <summary>
        /// 得到账套数据的连接串
        /// </summary>
        /// <param name="selectIndex"></param>
        /// <param name="dataBase"></param>
        /// <param name="pubConnectString"></param>
        /// <param name="theResult"></param>
        /// <returns></returns>
        public string GetAccConnstringElement(int selectIndex, string dataBase, string pubConnectString, out string theResult)
        {
            //15.0要求传数据库全名

            //容错，发现以前Intfi不传NG前缀，而现在没有此返回的字符串不符合规范
            //if (dataBase.Length > 0 && dataBase.ToLower().IndexOf("ng") != 0)
            //{
            //    dataBase = "NG" + dataBase;
            //}

            string provider;	//连接方式
            string serverName;	//
            string logId;	//
            string logPass;	//
            string driver;	//
            string connectType;	//
            string passWord = "";
            string UserConnectStr = "";

            DataSet dBXmlDS = this.DBConfig;

            //int selectIndex = this.GetDBIndex(dBXmlDS, dbservername);

            provider = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Provider"].ToString();
            serverName = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ServerName"].ToString();
            driver = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Driver"].ToString();
            connectType = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ConnectType"].ToString();

            passWord = GetDbPW(dataBase, pubConnectString, selectIndex);
            logPass = NG3.NGEncode.DecodePassword(passWord, 128);

            if (provider.Trim().ToUpper().IndexOf("ORA") > 0)
            {
                isOracle = "1";
            }
            else
            {
                isOracle = "0";
            }

            if (isOracle == "1")	//是oracle数据库
            {
                logId = dataBase;
                dataBase = "";
            }
            else	//是sqlserver数据库
                logId = dBXmlDS.Tables["Connect"].Rows[selectIndex]["LogId"].ToString();

            UserConnectStr = SetConStr(provider, serverName, dataBase, logId, logPass, driver, connectType, out theResult);

            return UserConnectStr;
        }

        /// <summary>
        /// 得到账套数据的连接串
        /// </summary>
        /// <param name="selectIndex"></param>
        /// <param name="dataBase"></param>
        /// <param name="pubConnectString"></param>
        /// <param name="theResult"></param>
        /// <returns></returns>
        public string GetAccConnstringElement(string dbservername, string dataBase, string pubConnectString, out string theResult)
        {
            //15.0要求传数据库全名

            //容错，发现以前Intfi不传NG前缀，而现在没有此返回的字符串不符合规范
            //if (dataBase.Length > 0 && dataBase.ToLower().IndexOf("ng") != 0)
            //{
            //    //dataBase = "NG" + dataBase;
            //}


            string provider;	//连接方式
            string serverName;	//
            string logId;	//
            string logPass;	//
            string driver;	//
            string connectType;	//
            string passWord = "";
            string UserConnectStr = "";

            DataSet dBXmlDS = this.DBConfig;

            int selectIndex = this.GetDBIndex(dBXmlDS, dbservername);          

            provider = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Provider"].ToString();
            serverName = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ServerName"].ToString();
            driver = dBXmlDS.Tables["Connect"].Rows[selectIndex]["Driver"].ToString();
            connectType = dBXmlDS.Tables["Connect"].Rows[selectIndex]["ConnectType"].ToString();

            passWord = GetDbPW(dataBase, pubConnectString, selectIndex);
            logPass = NG3.NGEncode.DecodePassword(passWord, 128);

            if (provider.Trim().ToUpper().IndexOf("ORA") > 0)
            {
                isOracle = "1";
            }
            else
            {
                isOracle = "0";
            }

            if (isOracle == "1")	//是oracle数据库
            {
                logId = dataBase;
                dataBase = "";
            }
            else	//是sqlserver数据库
                logId = dBXmlDS.Tables["Connect"].Rows[selectIndex]["LogId"].ToString();

            UserConnectStr = SetConStr(provider, serverName, dataBase, logId, logPass, driver, connectType, out theResult);

            return UserConnectStr;
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
        public string SetConStr(string provider, string serverName, string dataBase, string logId, string passWord, string driver, string connectType, out string errReturn)
        {
            return SetConStr(provider, serverName, dataBase, logId, passWord, driver, connectType, out errReturn, false,true);
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
        /// <param name="IsADO"></param>
        /// <param name="ngPrefix">数据库名称是否加NG前最</param>
        /// <returns>连接串</returns>
        public string SetConStr(string provider, string serverName, string dataBase, string logId, string passWord, string driver, string connectType, out string errReturn, bool IsADO,bool ngPrefix)
        {
            //15.0要求传数据库全名

            //if (ngPrefix)
            //{
            //    //容错，发现以前Intfi不传NG前缀，而现在没有此返回的字符串不符合规范
            //    if (dataBase.Length > 0 && dataBase.ToLower().IndexOf("ng") != 0)
            //    {
            //        //dataBase = "NG" + dataBase;
            //    }
            //}

            string ngConStr = "";	//连接串

            string dbType = "";	//连接类型

            errReturn = "";

            dbType = GetConnType(connectType, provider);

            try
            {
                NG3.Data.NGDbFactory ngDBFact = NG3.Data.NGDbFactory.Instance;
                if (IsADO)
                {
                    if (dbType.ToLower() == "odbc")
                        throw new Exception("can't get oleConectionString from odbc driver");
                    if (dbType.ToLower() == "sqlclient" || provider.ToUpper() == "SQLOLEDB")
                        ngConStr = ngDBFact.ComboOleDbConnString(DbVendor.SQLServer, serverName, dataBase, logId, passWord, "");
                    else
                        if (dbType.ToLower() == "oracleclient" || provider == "MSDAORA.1")
                            ngConStr = ngDBFact.ComboOleDbConnString(DbVendor.Oracle9, serverName, dataBase, logId, passWord, "");
                        else
                            if (provider.ToLower() == "asaprov")
                                ngConStr = string.Format("ConnectType=OleDb;Provider=MSDataShape;Data Source={0};uid={1};pwd={2}", serverName, logId, passWord);
                            else
                                if (provider.ToLower() == "sybase.aseoledbprovider")
                                    ngConStr = ngDBFact.ComboOleDbConnString(DbVendor.ASE, serverName, dataBase, logId, passWord, "");
                                else
                                    if (dbType == "ASEClient")
                                        ngConStr = ngDBFact.ComboOleDbConnString(DbVendor.ASE, serverName, dataBase, logId, passWord, "");


                }
                else
                {
                    if (dbType.ToLower() == "sqlclient")
                        ngConStr = ngDBFact.ComboSqlConnString(serverName, dataBase, logId, passWord, "");
                    if (dbType.ToLower() == "oledb")
                    {
                        ngConStr = ngDBFact.ComboOleDbConnString(provider, serverName, dataBase, logId, passWord, "");
                        if (provider == "Sybase.ASEOLEDBProvider")
                            ngConStr += ";Select Method=Cursor;";
                        if (provider == "ASAProv")
                        {
                            //ngConStr = string.Format("ConnectType=OleDb;Provider=MSDataShape;Data Source={0};uid={1};pwd={2};",serverName , logId, passWord);
                            if (dataBase != "" && dataBase != serverName)
                                serverName = dataBase;
                            ngConStr = "ConnectType=OleDb;Provider=MSDataShape;DSN=" + serverName;
                        }
                    }
                    if (dbType.ToLower() == "odbc")
                        ngConStr = "ConnectType=Odbc;DSN=" + serverName;
                    if (dbType.ToLower() == "oracleclient")
                        ngConStr = ngDBFact.ComboOracleConnString(serverName, logId, passWord, "");
                    //add AsaClient
                    if (dbType.ToLower() == "aseclient")
                        ngConStr = "ConnectType=AseClient;" + string.Format("Data Source='{0}';Port='{1}';UID='{2}';PWD='{3}';Database='{4}';", serverName, "5000", logId, passWord, dataBase);
                    if (dbType.ToLower() == "mysqlclient")
                    {
                        string [] arr = serverName.Split(':');
                        string server = arr[0];
                        string port = arr[1];
                        ngConStr = "ConnectType=MySqlClient;" + string.Format("Server='{0}';Port='{1}';user id='{2}';password='{3}';Database='{4}';", server, port, logId, passWord, dataBase);
                    }
                }
                if (ngConStr != "")
                {
                    if (dbType.ToLower() != "odbc" && dbType.ToLower() != "aseclient" && provider != "ASAProv" && dbType.ToLower() != "mysqlclient")
                        ngConStr = "ConnectType=" + dbType + ";" + ngConStr;

                    #region pb测试报错，先去掉
                    //errReturn = NGDbFactory.TestConnection(ngConStr);

                    //if (errReturn.Trim().Length != 0)
                    //{
                    //    throw new Exception(errReturn);
                    //}
                    #endregion

                }
            }           
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ngConStr;
        }
        
        /// <summary>
        /// 读取数据库连接设置
        /// </summary>
        /// <param name="product">当前产品</param>
        /// <param name="dbXmlPath">当前虚拟路径</param>
        /// <returns></returns>
        public DataSet Read()
        {
            DataSet dbxmlDS = new DataSet();
            string fullpathname = string.Empty;

            string path = string.Empty;
            string xmlFile = string.Empty;

            if (!string.IsNullOrWhiteSpace(this._databasefile))
            {
                fullpathname = this._databasefile;//由exe启动,调用方传入DataBases.xml文件路径               
            }
            else
            {                

               path = HttpContext.Current.Request.PhysicalApplicationPath.ToString();

                //string xmlFile = "Config" + Path.DirectorySeparatorChar + "DataBases.xml"; 

                //if (productInfo.ExistsProductFile)//优先读取产品目录下(i6config)的数据库连接配置文件
                //{
                //    if (string.IsNullOrEmpty(productInfo.Series))
                //    {
                //        xmlFile = productInfo.ProductCode + "Config\\DataBases.xml";
                //    }
                //    else
                //    {
                //        xmlFile = productInfo.ProductCode + productInfo.Series + "Config\\DataBases.xml";
                //    }

                //    fullpathname = Path.Combine(path, xmlFile);
                //}

               
                    xmlFile = "NG3Config" + Path.DirectorySeparatorChar + "DataBases.xml";
                    fullpathname = Path.Combine(path, xmlFile);
             

            }
                   

            dbxmlDS.ReadXml(fullpathname);
            ///升级DataBase.XML
            UpgradeDataBaseXML(dbxmlDS, fullpathname);

            return dbxmlDS;
        }

        private static void UpgradeDataBaseXML(DataSet ds, string xmlName)
        {
            DataTable dt = ds.Tables["Connect"];
            try
            {
                int find = dt.Columns.IndexOf("ByName");

                if (find < 0)
                {
                    dt.Columns.Add("ByName", typeof(string));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.BeginEdit();
                        dr["ByName"] = dr["ServerName"];
                        dr.EndEdit();
                    }


                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 得到数据库密码
        /// </summary>
        /// <param name="ucode"></param>
        /// <param name="PubConnectString"></param>
        /// <returns></returns>
        private string GetDbPW(string ucode, string PubConnectString, int selectIndex)
        {
            string passWord = "";

            DataSet dBXmlDS = this.DBConfig;
            DataTable dt = new DataTable();

            if (isOracle == "0")	//sqlserver数据库
            {
                passWord = dBXmlDS.Tables["Connect"].Rows[selectIndex]["LogPass"].ToString();
            }
            else	//oracle数据库
            {
                passWord = this.GetDbPwd(PubConnectString, ucode);
            }
            return passWord;
        }

        /// <summary>
        /// 对应oracle数据库根据帐套取最大年度帐套的密码
        /// </summary>
        /// <param name="ucode">数据库名</param>
        /// <returns>帐套对应的密码</returns>
        public string GetDbPwd(string PubConnectString, string dbname)
        {

            //string sql = "select ucode from ngusers where dbname=" + DbConvert.ToSqlString(dbname) + "";
            //string ucode = DbHelper.ExecuteScalar(PubConnectString, sql).ToString();

            string sql = string.Empty;
            string ucode = string.Empty;
            string uyear = string.Empty;
            string pwd = string.Empty;
            if (dbname.Length > 6)//ng00012016
            {
                ucode = dbname.Substring(2, dbname.Length - 6);
                uyear = dbname.Substring(dbname.Length-4, 4);
                sql = "select count(*) from ngyeardb where ucode=" + DbConvert.ToSqlString(ucode) + " and uyear=" + DbConvert.ToSqlString(uyear) + "";

                string ret = DbHelper.GetString(PubConnectString,sql);
                if (ret == "0")//没有则取最大
                {
                    ucode = dbname.Substring(2, dbname.Length - 2);//去掉NG前缀
                    sql = "select max(uyear) from ngyeardb where ucode=" + DbConvert.ToSqlString(ucode) + "";
                    uyear = DbHelper.GetString(PubConnectString, sql);
                }
            }
            else//默认取最大年度
            {
                ucode = dbname.Substring(2, dbname.Length - 2);//去掉NG前缀
                sql = "select max(uyear) from ngyeardb where ucode=" + DbConvert.ToSqlString(ucode) + "";
                uyear = DbHelper.GetString(PubConnectString, sql);
            }

            sql = "select dblogpass from ngyeardb where ucode=" + DbConvert.ToSqlString(ucode) + " and uyear=" + DbConvert.ToSqlString(uyear) + "";
            pwd = DbHelper.GetString(PubConnectString,sql);

            if (string.IsNullOrEmpty(pwd))
            {
                throw new Exception(string.Format("密码获取失败,dbname:{0}；ucode:{1}；uyear:{2}",dbname,ucode,uyear));
            }
            return pwd;

        }

        /// <summary>
        /// 得到连接的类型
        /// </summary>
        /// <param name="connectType"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        private string GetConnType(string connectType, string provider)
        {
            string DBType = "";
            if (connectType == "1")
            {
                DBType = "OLEDB";
            }
            else if (connectType == "2")
            {
                DBType = "ODBC";
            }
            else if (connectType == "3")
            {
                DBType = "SqlClient";
                if (provider.ToUpper().IndexOf("ORA") >= 0)
                {
                    DBType = "ORACLEClient";
                }
                if (provider.ToUpper().IndexOf("ASE") >= 0)
                {
                    DBType = "ASEClient";
                }
                if (provider.ToUpper().IndexOf("MYSQL") >= 0)
                {
                    DBType = "MySqlClient";
                }
            }
            return DBType;
        }

        /// <summary>
        /// 取数据库服务器名在database.xml文件中的下标位置
        /// </summary>
        /// <param name="dBXmlDS"></param>
        /// <param name="dbservername"></param>
        /// <returns></returns>
        private int GetDBIndex(DataSet dBXmlDS, string dbservername)
        {
            int selectIndex = 0;

            bool hit = false;
            DataTable dtDB = dBXmlDS.Tables["Connect"];
            for (int i = 0; i < dtDB.Rows.Count; i++)
            {
                if (dtDB.Rows[i]["ServerName"].ToString().Equals(dbservername, StringComparison.OrdinalIgnoreCase))
                {
                    selectIndex = i;
                    hit = true;
                    break;
                }
            }

            if (!hit)
            {
                return Convert.ToInt32(this.LastConnect);//取默认连接 
            }
            return selectIndex;
        }
        

        /// <summary>
        /// 最后一次主数据库连接保存
        /// </summary>
        public string LastConnect
        {
            get
            {
                return DBConfig.Tables["ConnectDB"].Rows[0]["LastDB"].ToString();
            }
            set
            {
                DBConfig.Tables["ConnectDB"].Rows[0]["LastDB"] = value;
            }
        }

        public string DefaultDB
        {
            get
            {
                //return DBConfig.Tables["AccConfig"].Rows[0]["DefaultAccount"].ToString();
                return DBConfig.Tables["AccConfig"].Rows[0]["DefaultDataBase"].ToString();//取数据库全名
            }          
        }

    }
}
