using DotNetCasClient;
using NG3.Data.Service;
using Oracle.ManagedDataAccess.Client;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GXM3.XM.Controller
{
    /// <summary>
    /// 获取登录信息
    /// </summary>
    public class LoginInfoController: System.Web.Mvc.Controller
    {
        /// <summary>
        /// cas登陆
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "登录跳转页面";
            return View("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index2()
        {
            ViewBag.Title = "登录跳转页面";
            return View("Index");
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            //FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 根据cas回传信息，获取手机号码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string getUserInfo()
        {
            //string mobileno = System.Web.HttpContext.Current.Request.Params["mobileno"];  //人员手机号码

            string mobileno=CasAuthentication.CurrentPrincipal.Identity.Name;

            //根据手机号码获取人员信息

            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            string theResult;
            string pubConn = dbbuilder.GetMainConnStringElement(0, out theResult, false);//取第一个（默认）服务器
            string userConn = dbbuilder.GetDefaultConnString();//取默认连接串
            DataTable dt = null;
            object userData = null;

            if (string.IsNullOrEmpty(mobileno))
            {
                return DataConverterHelper.SerializeObject(new
                {
                    Status = ResponseStatus.Error,
                    Msg = "没有获取到云平台的cas的手机号码."
                });
            }

            try
            {

                string sqlType = "";
                string connectString = "";

                //ConnectType=ORACLEClient;Data Source=10.0.14.34:1521/DQW;User ID=NG0001;Password=NG0001;Self Tuning=false;Statement Cache Size=0;Metadata Pooling=false 
                //ConnectType=SqlClient;Server=10.0.13.168;Database=NG0012;User ID=sa;Password=123456;
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    sqlType= "sqlclient";
                    string server = NG.NGKeyValueUtility.GetValue(userConn, "Server");
                    string dataBase = NG.NGKeyValueUtility.GetValue(userConn, "Database", "Initial Catalog");
                    string userid = NG.NGKeyValueUtility.GetValue(userConn, "User ID");
                    string password = NG.NGKeyValueUtility.GetValue(userConn, "Password");

                    connectString = string.Format("Server={0};Database={1};User ID={2};Password={3}", server, dataBase, userid, password);
                }

                if (userConn.IndexOf("ConnectType=OracleClient", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    sqlType = "oracle";

                    string source = NG.NGKeyValueUtility.GetValue(userConn, "Data Source");
                    string[] arrySource = source.Split(new char[] { ':', '/' }, StringSplitOptions.RemoveEmptyEntries);
                    string host = arrySource[0];
                    string port = arrySource[1];
                    string Server_name = arrySource[2];
                    string userid = NG.NGKeyValueUtility.GetValue(userConn, "User ID");
                    string password = NG.NGKeyValueUtility.GetValue(userConn, "Password");

                    connectString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));Persist Security Info=True;User ID={3};Password={4};", host, port, Server_name, userid, password);
                }

                //"oracle":"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.6.139)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=PUBDATA;Password=pubdata;";
                //"sqlclient":"Server=218.108.53.111,1433;Database=DMPBase;User ID=sa;Password=newgrand@123";

               

                string SQLString = string.Format("select userno,pwd from fg3_user where mobileno='{0}'", mobileno);

                if (sqlType == "sqlclient")
                {

                    using (SqlConnection connection = new SqlConnection(connectString))
                    {
                        DataSet ds = new DataSet();
                        try
                        {
                            connection.Open();
                            SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);

                            command.Fill(ds, "ds");
                        }
                        catch (SqlException ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                else if (sqlType == "oracle")
                {
                    using (OracleConnection connection = new OracleConnection(connectString))
                    {
                        DataSet ds = new DataSet();
                        try
                        {
                            connection.Open();
                            OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                            command.Fill(ds, "ds");
                        }
                        catch (OracleException ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        dt = ds.Tables[0];
                    }
                }

            }
            catch (Exception e)
            {
                //throw e;
                return DataConverterHelper.SerializeObject(new
                {
                    Status = ResponseStatus.Error,
                    Msg = e.ToString()
                });
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 1)
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "当前的人员的手机号：" + mobileno + "，含有多个数据"
                    });
                }
                string dbpwd = "";
                if (!string.IsNullOrEmpty(dt.Rows[0]["pwd"].ToString()))
                {
                    dbpwd = NG3.NGEncode.DecodePassword(dt.Rows[0]["pwd"].ToString(), 128);
                }


                userData = new
                {
                    logid = dt.Rows[0]["userno"].ToString(),
                    pwd = string.IsNullOrEmpty(dt.Rows[0]["pwd"].ToString()) ? "" : dt.Rows[0]["pwd"].ToString()
                };

            }
            else
            {
                return DataConverterHelper.SerializeObject(new
                {
                    Status = ResponseStatus.Error,
                    Msg = "当前的人员的手机号：" + mobileno + "，没有对应的账号"
                });
            }

            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "",
                Data = userData
            };


            return DataConverterHelper.SerializeObject(data);

        }

        ///编码
        private static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        ///解码
        private static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
    }
}
