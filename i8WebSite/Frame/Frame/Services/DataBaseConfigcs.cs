using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NG3;
using NG3.Data.Service;

namespace NG3.SUP.Frame
{
    public class DataBaseConfigcs
    {
        private static DataBaseConfig dataBaseConfig;
        static DataBaseConfigcs _instance;

        //add by 徐明建:增加参数允许代码实例化出多个数据库链接
        private string filename = string.Empty;

        public DataBaseConfigcs()
        {
            
        }

        public static DataSet DataBaseInfo
        {
            get
            {
                if (dataBaseConfig == null)
                {
                    try
                    {
                        dataBaseConfig = new DataBaseConfig();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return dataBaseConfig.DataBaseInfo;
            }
        }

        public static string GetDbType(string severName)
        {
            return dataBaseConfig.GetDbType(severName);
        }

        public static string GetUserId(string serverName)
        {
            return dataBaseConfig.GetUserId(serverName);
        }

        public static string GetNGSoftPwd(string serverName)
        {
            return dataBaseConfig.GetNGSoftPwd(serverName);    
        }

        public static string GetDbPwd(string serverName,string Account)
        {
            return dataBaseConfig.GetDbPwd(serverName, Account);    
        }

        public static string GetPubConnString(string serverName)
        {
            return dataBaseConfig.GetPubConnString(serverName);    
        }

        public static string GetUserConnString(string serverName, string Account)
        {
            return dataBaseConfig.GetUserConnString(serverName, Account);    
        }
    }

    /// <summary>
    /// add by 徐明建:增加参数允许代码实例化出多个数据库链接
    /// </summary>
    public class DataBaseConfig
    {
        //oracle
        private const string oraclepubstr = "ConnectType=OracleClient;Data Source={0};User ID=NGSoft;Password={1};;persist security info=false";
        private const string oracleuserstr = "ConnectType=OracleClient;Data Source={0};User ID={1};Password={2};;persist security info=false";

        //sqlserver
        private const string sqlpubstr = "ConnectType=SqlClient;Server={0};Database=NGSoft;User ID={1};Password={2}";
        private const string sqluserstr = "ConnectType=SqlClient;Server={0};Database={1};User ID={2};Password={3}";

        private DataSet ds; //new DataSet();

        static DataBaseConfigcs _instance;

        private string filename = string.Empty;

        public DataBaseConfig()
        {
            filename = HttpContext.Current.Request.PhysicalApplicationPath + @"Config/DataBases.xml";
        }

        public DataBaseConfig(string filename) {
            this.filename = filename;
        }

        public  DataSet DataBaseInfo
        {
            get
            {
                if (ds == null)
                {
                   try
                    {
                        ds = new DataSet();
                        ds.ReadXml(filename);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return ds;
            }
        }

        public  string GetDbType(string severName)
        {
            string where = string.Format("ByName='{0}'", severName);
            DataRow[] dr = ds.Tables["Connect"].Select(where);

            string type = dr[0]["Provider"].ToString();

            if ("SQLOLEDB" == type)
            {
                return "SqlClient";
            }
            else
            {
                return "OracleClient";
            }
        }

        public  string GetUserId(string serverName)
        {
            string where = string.Format("ByName='{0}'", serverName);
            DataRow[] dr = ds.Tables["Connect"].Select(where);
            string pwd = NGEncode.DecodePassword(dr[0]["LogID"].ToString(), 128);

            return pwd;
        }

        public  string GetNGSoftPwd(string serverName)
        {
            string where = string.Format("ByName='{0}'", serverName);
            DataRow[] dr = ds.Tables["Connect"].Select(where);
            string pwd = NGEncode.DecodePassword(dr[0]["LogPass"].ToString(), 128);

            return pwd;
        }

        public  string GetDbPwd(string serverName, string Account)
        {
            string pwd = GetNGSoftPwd(serverName);
            string dbtype = GetDbType(serverName);

            if ("SqlClient" == dbtype)
            {
                return pwd;
            }
            else
            {
                string connstr = "ConnectType=OracleClient;Data Source={0};User ID=NGSoft;Password={1}";

                connstr = string.Format(connstr, serverName, pwd);

                string sql = string.Format("select ucode from ngusers where dbname='{0}'", Account);
                string ucode = DbHelper.ExecuteScalar(connstr, sql).ToString();

                sql = string.Format("select max(uyear) from ngyeardb where ucode='{0}'", ucode);

                string uyear = DbHelper.ExecuteScalar(connstr, sql).ToString();

                sql = string.Format("select dblogpass from ngyeardb where ucode='{0}' and uyear='{1}'", ucode, uyear);

                return DbHelper.ExecuteScalar(connstr, sql).ToString();
            }
        }

        public  string GetPubConnString(string serverName)
        {
            string dbType = GetDbType(serverName);
            string dbUserid = GetUserId(serverName);
            string pwd = GetNGSoftPwd(serverName);

            pwd = NG3.NGEncode.DecodePassword(pwd, 128);

            string pubConn = string.Empty;

            if ("SqlClient" == dbType)
            {
                pubConn = string.Format(sqlpubstr, serverName, dbUserid, pwd);
            }
            else
            {
                pubConn = string.Format(oraclepubstr, serverName, pwd);
            }
            return pubConn;
        }

        public  string GetUserConnString(string serverName, string Account)
        {
            string pubConn = GetPubConnString(serverName);

            string dbType = GetDbType(serverName);
            string dbUserid = GetUserId(serverName);
            string pwd = GetDbPwd(serverName, Account);

            pwd = NG3.NGEncode.DecodePassword(pwd, 128);

            string userConn = string.Empty;

            if ("SqlClient" == dbType)
            {
                userConn = string.Format(sqluserstr, serverName, Account, dbUserid, pwd);
            }
            else
            {
                userConn = string.Format(oracleuserstr, serverName, Account, pwd);
            }

            return userConn;
        }

    }
}