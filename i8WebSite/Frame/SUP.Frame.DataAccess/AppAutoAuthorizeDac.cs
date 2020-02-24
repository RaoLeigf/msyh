using Enterprise3.Common.Model;
using Enterprise3.WebApi.Client;
using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.DataAccess
{
    public class AppAutoAuthorizeDac
    {

        /// <summary>
        /// 获取app自动授权设置grid列表数据
        /// </summary>
        /// <param name="phid"></param>
        /// <returns></returns>
        public DataTable GetAppAutoAuthorizeList(string rolename)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select a.phid, a.role,a.autoauthorize,a.multipledeviceuse,b.rolename from fg3_app_autoauthorize a");
                strbuilder.Append(" left join fg3_role b on a.role=b.phid ");
                if (!string.IsNullOrEmpty(rolename)) {
                    strbuilder.Append(" where b.rolename like '%"+rolename+"%' ");
                }
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存app自动授权设置grid列表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int Save(DataTable dt)
        {
            try
            {
                int rows = 0;
                if (dt.Rows.Count > 0)
                {
                    IList<long> phidList = this.GetPhidList(dt.Rows.Count, "phid", 1, "fg3_app_autoauthorize");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i].RowState != DataRowState.Deleted)
                        {
                            if (dt.Rows[i]["phid"] == DBNull.Value)
                            {
                                dt.Rows[i]["phid"] = phidList[i];
                            }
                            dt.Rows[i]["orgid"] = 0;
                        }
                    }
                    rows = DbHelper.Update(dt, "select * from fg3_app_autoauthorize");
                }
                return rows;
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
    }
}
