using Enterprise3.Common.Model;
using Enterprise3.WebApi.Client;
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
using System.Web;

namespace SUP.Frame.DataAccess
{
    public class OptionSettingDac
    {

        /// <summary>
        /// 获取选项设置左侧树
        /// </summary>
        /// <returns></returns>
        public DataTable LoadTreeData()
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable menudt = new DataTable();
                strbuilder.Append("select phid,name,optiontype,parent_phid,value,url,property");
                strbuilder.Append(" from fg3_option_register");

                menudt = DbHelper.GetDataTable(strbuilder.ToString());
                return menudt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///// <summary>
        ///// 获取grid列表数据
        ///// </summary>
        ///// <param name="phid"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="totalRecord"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public DataTable GetGridList(Int64 phid, int pageSize, int pageIndex, ref int totalRecord, string type)
        //{
        //    try
        //    {
        //        string sortField = " phid asc ";
        //        StringBuilder strbuilder = new StringBuilder();
        //        DataTable dt = new DataTable();
        //        strbuilder.Append("SELECT a.phid, a.moduleid,a.range, a.option_group, a.option_code, a.option_name, a.option_value, a.option_type, a.isunify, a.scope, a.tips, a.createtime, a.createuser, a.modifytime, a.modifyuser, a.property, a.user_mod_flg, a.sysflg, a.control_type, b.username FROM fg3_option_detail a LEFT JOIN fg3_user b ON a.modifyuser = b.userno");
        //        if (type == "0")//初始化设置
        //            strbuilder.Append(" where moduleid=" + phid + " and property in ('2','3')");
        //        else//选项设置
        //            strbuilder.Append(" where moduleid=" + phid + " and property in ('1','3')");
        //        strbuilder.Append(" order by phid asc");

        //        string sqlstr = PaginationAdapter.GetPageDataSql(strbuilder.ToString(), pageSize, ref pageIndex, ref totalRecord, sortField);
        //        dt = DbHelper.GetDataTable(strbuilder.ToString());
        //        DataTable dtOption = new DataTable();

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            DataColumn dc = new DataColumn("option_value_name", typeof(string));
        //            dt.Columns.Add(dc);
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                if (dt.Rows[i]["modifyuser"] != DBNull.Value && dt.Rows[i]["modifyuser"].ToString() != "")
        //                {
        //                    if (dt.Rows[i]["username"] == DBNull.Value || dt.Rows[i]["username"].ToString() == "")
        //                    {
        //                        dt.Rows[i]["username"] = "系统管理员";
        //                    }
        //                }
        //                if (dt.Rows[i]["control_type"].ToString() == "0")
        //                {
        //                    dtOption = GetOptionValue(Convert.ToInt64(dt.Rows[i]["phid"]));
        //                    DataRow dr = dtOption.Select("option_code='" + dt.Rows[i]["option_value"].ToString() + "'").FirstOrDefault();
        //                    if (dr != null)
        //                    {
        //                        dt.Rows[i]["option_value_name"] = dr["option_name"];
        //                    }
        //                }
        //                else
        //                {
        //                    dt.Rows[i]["option_value_name"] = dt.Rows[i]["option_value"];
        //                }
        //            }
        //        }
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        /// <summary>
        /// 获取grid列表数据
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetGridList(Int64 phid, int pageSize, int pageIndex, ref int totalRecord, string type)
        {
            try
            {
                string sortField = " phid asc ";
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("SELECT c.phid, c.moduleid,c.range, c.option_group, c.option_code, c.option_name, c.option_value, c.option_type, c.isunify, ");
                strbuilder.Append(" c.scope, c.tips, c.createtime,c.option_value_name,c.createuser, c.modifytime, c.modifyuser,c.property,c.control_type, d.username ");
                strbuilder.Append(" FROM (SELECT a.phid, a.moduleid,a.range, a.option_group, a.option_code, a.option_name, a.option_value, a.option_type, a.isunify, ");
                strbuilder.Append(" a.scope, a.tips, a.createtime, a.createuser, a.modifytime, a.modifyuser, a.property, a.user_mod_flg, a.sysflg, ");
                strbuilder.Append(" a.control_type,b.OPTION_NAME AS option_value_name FROM FG3_OPTION_DETAIL a LEFT JOIN fg3_option_value b ON a.PHID=b.DETAIL_PHID ");
                strbuilder.Append(" AND a.OPTION_VALUE=b.OPTION_CODE) c LEFT JOIN FG3_USER d ON c.modifyuser=d.USERNO ");
                if (type == "0")//初始化设置
                    strbuilder.Append(" where moduleid=" + phid + " and property in ('2','3')");
                else//选项设置
                    strbuilder.Append(" where moduleid=" + phid + " and property in ('1','3')");
                strbuilder.Append(" order by phid asc");

                string sqlstr = PaginationAdapter.GetPageDataSql(strbuilder.ToString(), pageSize, ref pageIndex, ref totalRecord, sortField);
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                DataTable dtOption = new DataTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["modifyuser"] != DBNull.Value && dt.Rows[i]["modifyuser"].ToString() != "")
                        {
                            if (dt.Rows[i]["username"] == DBNull.Value || dt.Rows[i]["username"].ToString() == "")
                            {
                                dt.Rows[i]["username"] = "系统管理员";
                            }
                        }
                        if (dt.Rows[i]["control_type"].ToString() == "1")
                        {
                            dt.Rows[i]["option_value_name"] = dt.Rows[i]["option_value"];
                        }
                    }
                }
                             
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取纳税组织grid列表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetTaxOrgGrid(string phid)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                string sortField = " phid asc ";
                strbuilder.Append("SELECT a.phid id, a.ocode,a.oname name,b.phid,b.detail_phid,b.type,b.argument from fg_orglist a left join  ");
                strbuilder.Append("(SELECT * FROM fg3_argument_setting WHERE detail_phid=" + Convert.ToInt64(phid) + ") b on a.phid = b.id ");
                strbuilder.Append(" where a.isactive = '1' and a.ifcorp = 'Y' and a.is_ratepay = '1'");
                int pageIndex = 0;
                int totalRecord = 0;
                string sqlstr = PaginationAdapter.GetPageDataSql(strbuilder.ToString(), 20, ref pageIndex, ref totalRecord, sortField);
                dt = DbHelper.GetDataTable(strbuilder.ToString());


                DataTable dtOption = new DataTable();

                if (dt != null && dt.Rows.Count > 0)
                {

                    DataColumn dc = new DataColumn("option_value_name", typeof(string));
                    dt.Columns.Add(dc);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["detail_phid"] != DBNull.Value)
                        {
                            dtOption = GetOptionValue(Convert.ToInt64(dt.Rows[i]["detail_phid"]));
                            DataRow dr = dtOption.Select("option_code='" + dt.Rows[i]["argument"].ToString() + "'").FirstOrDefault();
                            if (dr != null)
                            {
                                dt.Rows[i]["argument"] = dr["option_code"];
                                dt.Rows[i]["option_value_name"] = dr["option_name"];
                            }
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 保存数据到选项明细表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public int SaveTaxOrg(DataTable dt)
        {
            try
            {
                int rows = 0;
                if (dt.Rows.Count > 0)
                {
                    IList<long> phidList = this.GetPhidList(dt.Rows.Count, "phid", 1, "fg3_argument_setting");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["phid"] == DBNull.Value)
                        {
                            dt.Rows[i]["phid"] = phidList[i];
                        }
                        dt.Rows[i]["user_mod_flg"] = 0;
                        dt.Rows[i]["sysflg"] = 1;
                    }
                    rows = DbHelper.Update(dt, "select * from fg3_argument_setting");
                }
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 保存数据到选项明细表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public int SaveFunData(DataTable dt)
        {
            try
            {
                int rows = 0;
                int affectrows = 0;
                if (dt.Rows.Count > 0)
                {
                    IList<long> phidList = this.GetPhidList(dt.Rows.Count, "phid", 1, "fg3_argument_setting");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //dt.Rows[i]["user_mod_flg"] = 0;
                        //dt.Rows[i]["sysflg"] = 0;
                        if (dt.Rows[i]["phid"] == DBNull.Value)
                        {
                            dt.Rows[i]["phid"] = phidList[i];
                            IDataParameter[] parameterList = new IDataParameter[6];
                            parameterList[0] = new NGDataParameter("phid", DbType.Int64);
                            parameterList[0].Value = phidList[i];
                            parameterList[1] = new NGDataParameter("detail_phid", DbType.Int64);
                            parameterList[1].Value = dt.Rows[i]["detail_phid"];
                            parameterList[2] = new NGDataParameter("type", DbType.AnsiString);
                            parameterList[2].Value = dt.Rows[i]["type"];
                            parameterList[3] = new NGDataParameter("name", DbType.AnsiString);
                            parameterList[3].Value = dt.Rows[i]["name"];
                            parameterList[4] = new NGDataParameter("id", DbType.AnsiString);
                            parameterList[4].Value = dt.Rows[i]["id"];
                            parameterList[5] = new NGDataParameter("argument", DbType.AnsiString);
                            parameterList[5].Value = dt.Rows[i]["argument"];
                            rows += DbHelper.ExecuteNonQuery(" INSERT INTO fg3_argument_setting (phid, detail_phid, type, name, id, argument, user_mod_flg, sysflg) VALUES ({0},{1},{2},{3},{4},{5},0,1)", parameterList);
                        }
                        else
                        {
                            IDataParameter[] parameterList = new IDataParameter[4];
                            parameterList[0] = new NGDataParameter("name", DbType.AnsiString);
                            parameterList[0].Value = dt.Rows[i]["name"];
                            parameterList[1] = new NGDataParameter("id", DbType.AnsiString);
                            parameterList[1].Value = dt.Rows[i]["id"];
                            parameterList[2] = new NGDataParameter("argument", DbType.AnsiString);
                            parameterList[2].Value = dt.Rows[i]["argument"];
                            parameterList[3] = new NGDataParameter("phid", DbType.Int64);
                            parameterList[3].Value = Convert.ToInt64(dt.Rows[i]["phid"]);
                            //parameterList[3] = new NGDataParameter("modifytime", DbType.DateTime);
                            //parameterList[3].Value = DateTime.Now;
                            //parameterList[4] = new NGDataParameter("modifyuser", DbType.AnsiString);
                            //parameterList[4].Value = logid;
                            affectrows = DbHelper.ExecuteNonQuery(" update fg3_argument_setting set name={0} , id={1},argument={2} where phid={3} ", parameterList);
                        }
                    }
                    //rows = DbHelper.Update(dt, "select * from fg3_argument_setting");
                }
                return rows + affectrows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
        /// 获取i6s站点url
        /// </summary>
        /// <returns></returns>
        private static string Geti6sUrl()
        {
            //string result = string.Empty;
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
        /// 获取选项值下拉列表数据
        /// </summary>
        /// <param name="phid"></param>
        /// <returns></returns>
        public DataTable GetOptionValue(Int64 phid)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable menudt = new DataTable();
                strbuilder.Append("select option_code,option_name");
                strbuilder.Append(" from fg3_option_value");
                strbuilder.Append(" where detail_phid=" + phid + "");
                menudt = DbHelper.GetDataTable(strbuilder.ToString());

                return menudt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据分组和选项代码获取选项列表
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public DataTable GetOptionDetail(string option_group, string option_code)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable menudt = new DataTable();
                strbuilder.Append("select phid,option_code,option_name,option_group,isunify,option_type,option_value,range");
                strbuilder.Append(" from fg3_option_detail");
                strbuilder.Append(" where option_group='" + option_group + "'");
                if (!string.IsNullOrEmpty(option_code))
                    strbuilder.Append(" and option_code='" + option_code + "'");

                menudt = DbHelper.GetDataTable(strbuilder.ToString());

                return menudt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据分组和选项代码获取选项列表
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public DataTable GetOptionDetail(string conn,string option_group, string option_code)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable menudt = new DataTable();
                strbuilder.Append("select phid,option_code,option_name,option_group,isunify,option_type,option_value,range");
                strbuilder.Append(" from fg3_option_detail");
                strbuilder.Append(" where option_group='" + option_group + "'");
                if (!string.IsNullOrEmpty(option_code))
                    strbuilder.Append(" and option_code='" + option_code + "'");

                menudt = DbHelper.GetDataTable(conn,strbuilder.ToString());

                return menudt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据分组和选项代码获取选项列表
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public DataTable GetOptionDetail(string option_group)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable menudt = new DataTable();
                strbuilder.Append("select phid,option_code,option_name,option_group,isunify,option_type,option_value,range");
                strbuilder.Append(" from fg3_option_detail");
                strbuilder.Append(" where option_group='" + option_group + "'");
                menudt = DbHelper.GetDataTable(strbuilder.ToString());

                return menudt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据选项代码获取选项列表
        /// </summary>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public DataTable GetValueByCode(string option_code)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable menudt = new DataTable();
                strbuilder.Append("select phid,option_code,option_name,option_group,isunify,option_type,option_value,range");
                strbuilder.Append(" from fg3_option_detail");
                strbuilder.Append(" where option_code='" + option_code + "'");
                menudt = DbHelper.GetDataTable(strbuilder.ToString());

                return menudt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetArgValue(string phid)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select id,argument");
                strbuilder.Append(" from fg3_argument_setting");
                strbuilder.Append(" where detail_phid=" + Convert.ToInt64(phid) + "");
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetArgValue(string conn,string phid)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select id,argument");
                strbuilder.Append(" from fg3_argument_setting");
                strbuilder.Append(" where detail_phid=" + Convert.ToInt64(phid) + "");
                dt = DbHelper.GetDataTable(conn,strbuilder.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetDicValue(string phid, string[] keys)
        {
            try
            {
                string results = string.Join(",", keys);
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select id,argument");
                strbuilder.Append(" from fg3_argument_setting");
                strbuilder.Append(string.Format(" where detail_phid={0} and id in ({1})", Convert.ToInt64(phid), results));
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetSingleValue(string phid, string key)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select argument");
                strbuilder.Append(" from fg3_argument_setting");
                strbuilder.Append(" where detail_phid=" + Convert.ToInt64(phid) + " and id='" + key + "'");
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存数据到选项明细表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public int SaveDetailData(DataTable dt, string logid)
        {
            try
            {
                string cacheKey = "option_value";
                int affectrows = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["modifyuser"] = NG3.AppInfoBase.LoginID;
                        dt.Rows[i]["modifytime"] = DateTime.Now;
                        dt.Rows[i]["user_mod_flg"] = 1;
                    }
                }
                affectrows = DbHelper.Update(dt, "select * from fg3_option_detail");

                DataTable dtChche = DbHelper.GetDataTable("select * from fg3_option_detail");
                HttpRuntime.Cache.Remove(cacheKey);//先remove
                HttpRuntime.Cache.Add(cacheKey,
                                      dtChche,
                                      null,
                                      DateTime.Now.AddYears(1),
                                      System.Web.Caching.Cache.NoSlidingExpiration,
                                      System.Web.Caching.CacheItemPriority.NotRemovable,
                                      null);

                return affectrows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetArgumentByPhid(string detailPhid)
        {

            DataTable dt = DbHelper.GetDataTable("select * from fg3_argument_setting where detail_phid=" + Convert.ToInt64(detailPhid) + "");
            return dt;
        }

        public int SaveInitSetting()
        {
            try
            {
                int rows = DbHelper.ExecuteNonQuery("update fg3_option_detail set isEnable=1 where property=2");
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int GetInitSetting()
        {
            try
            {
                int isEnable = 0;
                DataTable dt = DbHelper.GetDataTable("select isEnable from fg3_option_detail where property=2");
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["isEnable"] != DBNull.Value && dt.Rows[0]["isEnable"] != null)
                    {
                        isEnable = Convert.ToInt32(dt.Rows[0]["isEnable"]);
                    }
                }
                return isEnable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取选项设置左侧树
        /// </summary>
        /// <returns></returns>
        public DataTable LoadOrgTree(string detailPhid)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable menudt = new DataTable();
                strbuilder.Append("select a.parentorg,a.parent_orgid,a.org_id,a.ocode,a.oname, b.id,b.name,b.argument,b.phid from(");
                strbuilder.Append("select fg_orgrelatitem.parentorg,fg_orgrelatitem.parent_orgid,fg_orgrelatitem.org_id, fg_orglist.ocode, fg_orglist.oname");
                strbuilder.Append(" from fg3_userorg,fg_orglist,fg_orgrelatitem");
                strbuilder.Append(" where fg3_userorg.orgid = fg_orglist.phid and fg_orgrelatitem.ocode=fg_orglist.ocode");
                strbuilder.Append(" and fg3_userorg.inner_type = 1 and fg_orgrelatitem.attrcode = 'lg'");
                strbuilder.Append(" and fg3_userorg.userid=" + NG3.AppInfoBase.UserID + "");
                if (NG3.AppInfoBase.UserConnectString.IndexOf("ORACLEClient") > -1)
                    strbuilder.Append(") a left join (select * from fg3_argument_setting where type=1 and detail_phid=" + Convert.ToInt64(detailPhid) + ") b on to_char(a.org_id)=b.id");
                else
                    strbuilder.Append(") a left join (select * from fg3_argument_setting where type=1 and detail_phid=" + Convert.ToInt64(detailPhid) + ") b on a.org_id=b.id");
                strbuilder.Append(" order by ocode");
                menudt = DbHelper.GetDataTable(strbuilder.ToString());

                return menudt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
