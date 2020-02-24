using NG3;
using NG3.Data;
using NG3.Data.Service;
using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

using Enterprise3.Common.Model;
using Enterprise3.WebApi.Client;

namespace SUP.Frame.DataAccess
{
    public class ShortcutMenuDac
    {
        /// <summary>
        /// 获取grid列表数据
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetShortcutMenuList(Int64 phid, int pageSize, int pageIndex, ref int totalRecord)
        {
            try
            {
                long userid = NG3.AppInfoBase.UserID;
                string sortField = " seq asc ";
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select * from fg3_shortcutmenu");
                strbuilder.Append(" where userid=" + userid + "");
                string sqlstr = PaginationAdapter.GetPageDataSql(strbuilder.ToString(), pageSize, ref pageIndex, ref totalRecord, sortField);
                dt = DbHelper.GetDataTable(sqlstr);
                DataTable dtOption = new DataTable();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取grid列表数据
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetShortcutMenuForWeb(Int64 userid)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select a.displayname,a.url,a.name,b.rightkey,b.norightcontrol");
                strbuilder.Append(" from fg3_shortcutmenu a left join fg3_menu b on a.originalcode = b.code");
                strbuilder.Append(" where a.userid=" + userid + "");
                strbuilder.Append(" and (a.url like '%WebBrowseIndividualManager%' or b.apptype='webform' OR b.apptype='mvc') ");
                strbuilder.Append(" order by a.seq asc");
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                dt= this.RightkeyController(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["displayname"] != DBNull.Value)
                            dt.Rows[i]["name"] = dt.Rows[i]["displayname"];
                    }
                }
                dt.Columns.Remove("displayname");
                dt.Columns.Remove("norightcontrol");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加数据到快捷功能表
        /// </summary>
        /// <param name="originalcode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddShortcutMenu(string originalcode, string name, string url,string busphid)
        {
            string userType = NG3.AppInfoBase.UserType;
            long userid = NG3.AppInfoBase.UserID;
            int affectRows = 0;
            int iret = 0;
            StringBuilder strbuilder = new StringBuilder();
            DataTable dt = new DataTable();
            strbuilder.Append("select * from fg3_shortcutmenu");
            strbuilder.Append(" where originalcode='" + originalcode + "' and userid=" + userid + "");
            dt = DbHelper.GetDataTable(strbuilder.ToString());
            if (dt.Rows.Count > 0)
            {
                return "请勿重复添加快捷菜单！";
            }
            else
            {
                IDataParameter[] parameterList = new IDataParameter[7];
                parameterList[0] = new NGDataParameter("originalcode", DbType.AnsiString);
                parameterList[0].Value = originalcode;
                parameterList[1] = new NGDataParameter("name", DbType.AnsiString);
                parameterList[1].Value = name;
                parameterList[2] = new NGDataParameter("seq", DbType.Int32);
                parameterList[2].Value = this.GetMaxSeq() + 1;
                parameterList[3] = new NGDataParameter("userid", DbType.Int64);
                parameterList[3].Value = userid;
                parameterList[4] = new NGDataParameter("url", DbType.AnsiString);
                parameterList[4].Value = url;
                long phid = this.GetSinglePhid("phid", 20, "fg3_shortcutmenu");
                parameterList[5] = new NGDataParameter("phid", DbType.Int64);
                parameterList[5].Value = phid;
                long busid = string.IsNullOrEmpty(busphid) ? 0 : Convert.ToInt64(busphid);
                parameterList[6] = new NGDataParameter("busphid", DbType.Int64);
                parameterList[6].Value = busid;
                if (phid == -9999)
                {
                    return "未能插入phid";
                }
                else
                {
                    string sql = "insert into fg3_shortcutmenu (phid,originalcode, name, userType, userid,seq,url,busphid) VALUES (" + phid + ",'" + originalcode + "','" + name + "',0," + userid + "," + (this.GetMaxSeq() + 1) + ",'" + url + "',"+busid+")";
                    //affectRows = DbHelper.ExecuteNonQuery("insert into fg3_shortcutmenu (originalcode, name, userType, userid) VALUES ({0},{1},0,{2})", parameterList);
                    affectRows = DbHelper.ExecuteNonQuery(sql);
                    return "";
                }
            }
        }

        //获取快捷功能表的seq最大值
        public int GetMaxSeq()
        {
            long userid = NG3.AppInfoBase.UserID;
            string sql = "select max(seq) from fg3_shortcutmenu where userid=" + userid + "";
            object obj = DbHelper.ExecuteScalar(sql);

            int iret = 0;
            if (obj != null && obj != DBNull.Value)
            {
                int.TryParse(obj.ToString(), out iret);
            }
            return iret;
        }

        /// <summary>
        /// 获取单个phid
        /// </summary>
        /// <param name="primaryName">主键名</param>
        /// <param name="step">如果传1返回的phid值为目前表里phid最大的值加1，传2返回最大的值加2，以此类推</param>
        /// <param name="tableName">表名</param>
        /// <param name="connstring">数据库连接串</param>
        /// <returns></returns>
        public long GetSinglePhid(string primaryName, int step, string tableName)
        {
#if DEBUG 
            NG3.AppInfoBase ng3AppInfo = new NG3.AppInfoBase();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            long phid = -9999;

            try
            {
                string appkey = System.Configuration.ConfigurationManager.AppSettings["AppKey"] == null ? string.Empty : System.Configuration.ConfigurationManager.AppSettings["AppKey"].ToString();
                string appsecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"] == null ? string.Empty : System.Configuration.ConfigurationManager.AppSettings["AppSecret"].ToString();

                string dbname = string.Empty;

                Enterprise3.WebApi.Client.Models.AppInfoBase appInfo = new Enterprise3.WebApi.Client.Models.AppInfoBase
                {
                    AppKey = appkey, // "D31B7F91-3068-4A49-91EE-F3E13AE5C48C",
                    AppSecret = appsecret, //"103CB639-840C-4E4F-8812-220ECE3C4E4D",
                    DbName = NG3.AppInfoBase.DbName, //可不传，默认为默认账套
                                                     //UserId = 77,
                                                     //OrgId = 166,
                    DbServerName = NG3.AppInfoBase.DbServerName,
                };

                string servicebaseurl = Geti6sUrl();

                WebApiClient client = new WebApiClient(servicebaseurl, appInfo);

                ReqBillIdEntity req = new ReqBillIdEntity
                {
                    NeedCount = 1,
                    PrimaryName = primaryName,
                    Step = step,
                    TableName = tableName
                };

                var res = client.Post<ResBillNoOrIdEntity, ReqBillIdEntity>("api/common/billno/getbillidincrease", req);

                if (res.IsError || res.Data == null)
                {
                    return phid;
                }
                //throw new RuleException(Resources.CallBillNoServiceError + System.Environment.NewLine + res.ErrMsg, BillNoReqType);


                ResBillNoOrIdEntity ReqBillNoOrIdCommitEntity = new ResBillNoOrIdEntity
                {
                    BillIdList = res.Data.BillIdList,
                };

                if (ReqBillNoOrIdCommitEntity.BillIdList.Count <= 0)
                {
                    return phid;
                }

                phid = ReqBillNoOrIdCommitEntity.BillIdList[0];

                return phid;
            }
            catch (Exception ex)
            {
                return phid;
            }
#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("编码规则方法，GetBillId方法执行时间总长：{0}毫秒", stopwatch.ElapsedMilliseconds);
#endif

        }

        /// <summary>
        /// 获取多个phid
        /// </summary>
        /// <param name="needCount">设置需要返回几个phid</param>
        /// <param name="primaryName">主键名</param>
        /// <param name="step">如果传1返回的phid值为目前表里phid最大的值加1，传2返回最大的值加2，以此类推</param>
        /// <param name="tableName">表名</param>
        /// <param name="connstring">数据库连接串</param>
        /// <returns></returns>
        public IList<long> GetPhidList(int needCount, string primaryName, int step, string tableName)
        {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            IList<long> phid = new List<long>();
            try
            {
                string appkey = System.Configuration.ConfigurationManager.AppSettings["AppKey"] == null ? string.Empty : System.Configuration.ConfigurationManager.AppSettings["AppKey"].ToString();
                string appsecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"] == null ? string.Empty : System.Configuration.ConfigurationManager.AppSettings["AppSecret"].ToString();

                string dbname = string.Empty;

                Enterprise3.WebApi.Client.Models.AppInfoBase appInfo = new Enterprise3.WebApi.Client.Models.AppInfoBase
                {
                    AppKey = appkey, // "D31B7F91-3068-4A49-91EE-F3E13AE5C48C",
                    AppSecret = appsecret, //"103CB639-840C-4E4F-8812-220ECE3C4E4D",
                    DbName = NG3.AppInfoBase.DbName, //可不传，默认为默认账套
                                                     //UserId = 77,
                                                     //OrgId = 166,
                    DbServerName = NG3.AppInfoBase.DbServerName,
                };

                string servicebaseurl = Geti6sUrl();

                WebApiClient client = new WebApiClient(servicebaseurl, appInfo);

                ReqBillIdEntity req = new ReqBillIdEntity
                {
                    NeedCount = needCount,
                    PrimaryName = primaryName,
                    Step = step,
                    TableName = tableName
                };

                var res = client.Post<ResBillNoOrIdEntity, ReqBillIdEntity>("api/common/billno/getbillidincrease", req);

                if (res.IsError || res.Data == null)
                {
                    return phid;
                }
                //throw new RuleException(Resources.CallBillNoServiceError + System.Environment.NewLine + res.ErrMsg, BillNoReqType);


                ResBillNoOrIdEntity ReqBillNoOrIdCommitEntity = new ResBillNoOrIdEntity
                {
                    BillIdList = res.Data.BillIdList,
                };

                phid = ReqBillNoOrIdCommitEntity.BillIdList;

                return phid;
            }
            catch (Exception ex)
            {
                return phid;
            }
#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("编码规则方法，GetBillId方法执行时间总长：{0}毫秒", stopwatch.ElapsedMilliseconds);
#endif

        }

        /// <summary>
        /// 获取各产品站点url
        /// </summary>
        /// <returns></returns>
        private static string Geti6sUrl()
        {
            string count = string.Empty;

            string host_url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string product = NG3.AppInfoBase.Product.ToLower();
            int index = host_url.ToLower().IndexOf(product);

            //截取url到product/
            if (index != -1)
            {
                count = host_url.Substring(0, index + product.Length + 1);
            }

            return count;
        }


        /// <summary>
        /// 获取快捷键数据值 
        /// </summary>
        /// <returns></returns>
        public DataTable GetShortcutKey()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("shortcutkey", typeof(string));
            //dt.Columns.Add("shortcutkey_name", typeof(string));
            Dictionary<string, string> keys = new Dictionary<string, string>();
            string[] strs = Enum.GetNames(typeof(Shortcut));
            for (int i = 0; i < strs.Length; ++i)
            {
                DataRow dr = dt.NewRow();
                strs[i] = strs[i].Replace("Ctrl", "Ctrl+").Replace("Alt", "Alt+").Replace("Shift", "Shift+");
                dr["shortcutkey"] = strs[i];
                //dr["shortcutkey_name"] = strs[i];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 批量保存数据到选项明细表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public DataTable SaveShortcutMenu(DataTable dt)
        {
            try
            {
                long userid = NG3.AppInfoBase.UserID;
                DataTable dtShortcut = new DataTable();
                string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                int affectrows = DbHelper.ExecuteNonQuery("delete from fg3_shortcutmenu where userid=" + userid + "");
                if (dt.Rows.Count > 0)
                {
                    IList<long> phidList = this.GetPhidList(dt.Rows.Count, "phid", 1, "fg3_shortcutmenu");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["phid"] = phidList[i];
                        dt.Rows[i]["seq"] = i + 1;
                        dt.Rows[i]["userid"] = NG3.AppInfoBase.UserID;
                    }
                    int rows = DbHelper.Update(dt, "select * from fg3_shortcutmenu");
                    if (rows > 0)
                    {
                        UserConfigDac.UserConfigSave(userid, 0, dateflag);
                        StringBuilder strbuilder = new StringBuilder();
                        strbuilder.Append("select a.displayname,a.shortcutkey,a.url,a.urlparam,a.name as str,b.managername,b.moduleno,b.code as id,b.suite,b.rightkey,b.norightcontrol");
                        strbuilder.Append(" from fg3_shortcutmenu a left join fg3_menu b on a.busphid = b.busphid and a.originalcode = b.code where a.userid=" + userid + " order by a.seq asc");
                        dtShortcut = DbHelper.GetDataTable(strbuilder.ToString());
                        dtShortcut = this.RightkeyController(dtShortcut);
                    }
                }
                return dtShortcut;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //如果不控制权限，把right设置为0
        public DataTable RightkeyController(DataTable menudt)
        {
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                string norightcontrol = dr["norightcontrol"].ToString();
                if (norightcontrol == "1")
                {
                    dr["rightkey"] = "0";
                }
            }
            return menudt;
        }

        public Int64 GetMaxID(string tableName)
        {
            string sql = "select max(phid) from " + tableName;
            object obj = DbHelper.ExecuteScalar(sql);

            Int64 iret = 0;
            if (obj != null && obj != DBNull.Value)
            {
                Int64.TryParse(obj.ToString(), out iret);
            }
            return iret;
        }

        public bool UserConfigCopy(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            try
            {
                string sqlText = "select * from fg3_shortcutmenu where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
                DataTable dt = DbHelper.GetDataTable(sqlText);

                sqlText = "select count(*) from fg3_shortcutmenu where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                string obj = DbHelper.GetString(sqlText);
                if (obj != "0")
                {
                    sqlText = "delete from fg3_shortcutmenu where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                    DbHelper.ExecuteNonQuery(sqlText);
                }
                DataTable newTable = dt.Clone();
                Int64 masterid = this.GetMaxID("fg3_shortcutmenu");
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    newRow.BeginEdit();
                    newRow["phid"] = ++masterid;
                    newRow["userid"] = toUserId;
                    newRow["usertype"] = toUserType;
                    newRow["name"] = dr["name"];
                    newRow["shortcutkey"] = dr["shortcutkey"];
                    newRow["originalcode"] = dr["originalcode"];
                    newRow["seq"] = dr["seq"];
                    newRow["displayname"] = dr["displayname"];
                    newRow["url"] = dr["url"];
                    newRow["urlparam"] = dr["urlparam"];
                    newRow["iconid"] = dr["iconid"];
                    newRow.EndEdit();
                    newTable.Rows.Add(newRow);
                }
                DbHelper.Update(newTable, "select * from fg3_shortcutmenu");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UserConfigDel(long userid, int usertype)
        {
            try
            {
                string sqlText = "delete from fg3_shortcutmenu where userid = '" + userid + "' and usertype = '" + usertype + "'";
                DbHelper.ExecuteNonQuery(sqlText);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
