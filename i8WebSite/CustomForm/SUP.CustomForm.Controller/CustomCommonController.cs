using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Mvc;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3;
using NG3.Bill.Base;
using NG3.Data.Service;
using NG3.Web.Controller;

namespace SUP.CustomForm.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomCommonController : AFController
    {
        //private ICustomCommonFacade Fac;

        public CustomCommonController()
        {
            //Fac = AopObjectProxy.GetObject<ICustomCommonFacade>(new CustomCommonFacade());
        }

        /// <summary>
        /// 从报表仓库打开自定义表单查看窗口
        /// </summary>
        /// <returns></returns>
        public ActionResult OpenFromReport()
        {
            string p_form = System.Web.HttpContext.Current.Request.Params["p_form"];
            string ucode = System.Web.HttpContext.Current.Request.Params["ucode"];
            string code = System.Web.HttpContext.Current.Request.Params["code"];

            //如 htt2p://10.0.18.21/i8/SUP/CustomCommon/OpenFromReport?ucode=ng0002&p_form=0000000010&code=448180531000001
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            int suploc = url.IndexOf("SUP", StringComparison.OrdinalIgnoreCase);

            //先判断这个表单是否已发布
            string issue = DbHelper.GetString(string.Format("select issue from p_form_m where code = '{0}'", p_form));
            if (issue != "1")
            {
                url = string.Format(url.Substring(0, suploc) + "SUP/Error/Error?error={0}", "该表单不存在或未发布");
                Response.Redirect(url);
                return View();
            }

            //判断这条数据是否存在
            string phid = DbHelper.GetString(string.Format("select phid from p_form{0}_m where phid = '{1}'", p_form, code));
            if (string.IsNullOrWhiteSpace(phid))
            {
                url = string.Format(url.Substring(0, suploc) + "SUP/Error/Error?error={0}", "该主键在表中查不到数据");
                Response.Redirect(url);
                return View();
            }

            //参数无误跳转的自定义表单
            url = string.Format(url.Substring(0, suploc) + "SUP/{0}pform{1}Edit?otype=view&id={2}", ucode, p_form, code);
            Response.Redirect(url);

            return View();
        }

        /// <summary>
        /// 调Rest接口
        /// </summary>
        /// <returns></returns>
        public string CallRest()
        {
            string formXml = System.Web.HttpContext.Current.Request.Params["formXml"];
            string restUrl = System.Web.HttpContext.Current.Request.Params["restUrl"];
            string tablename = System.Web.HttpContext.Current.Request.Params["tablename"];
            string phid = System.Web.HttpContext.Current.Request.Params["phid"];
            JObject jResult = new JObject();

            //XmlDocument xmlD = JsonConvert.DeserializeXmlNode(formJson);
            //Console.WriteLine(xmlD.OuterXml);

            try
            {
                if (restUrl.StartsWith("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }

                HttpContent httpContent = new StringContent(formXml);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = httpClient.PostAsync(restUrl, httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    //更新调接口标识
                    DbHelper.ExecuteNonQuery(string.Format("update {0} set istf='1' where phid='{1}'", tablename, phid));

                    string result = response.Content.ReadAsStringAsync().Result;

                    jResult.Add("Status", "OK");
                    jResult.Add("Data", result);
                    return JsonConvert.SerializeObject(jResult);
                }
                else
                {
                    jResult.Add("Status", "Fail");
                    return JsonConvert.SerializeObject(jResult);
                }

                //var httpClient = new HttpClient();
                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                //HttpResponseMessage response = httpClient.GetAsync(restUrl).Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    string result = response.Content.ReadAsStringAsync().Result;

                //    jResult.Add("Status", "OK");
                //    jResult.Add("Data", result);
                //    return JsonConvert.SerializeObject(jResult);
                //}
                //else
                //{
                //    jResult.Add("Status", "Fail");
                //    return JsonConvert.SerializeObject(jResult);
                //}
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 申请去审
        /// </summary>
        /// <returns></returns>
        public string ApplyCheckParam()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);
            string billno = System.Web.HttpContext.Current.Request.Params["billno"];
            long receiverid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["receiverid"]);
            string receivername = System.Web.HttpContext.Current.Request.Params["receivername"];
            long ocodeid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["ocode"]);
            string url = System.Web.HttpContext.Current.Request.Params["urlLink"];
            string busTitle = System.Web.HttpContext.Current.Request.Params["busTitle"];

            if (string.IsNullOrEmpty(receivername))
            {
                receivername = DbHelper.GetString(string.Format("select username from fg3_user where phid = {0}", receiverid));
            }

            string ocode = DbHelper.GetString(string.Format("select ocode from fg_orglist where phid = {0}", ocodeid));
            string msg = string.Format("您有一份{0}: 编号[{1}]的申请去审消息", busTitle, billno);

            var param = new
            {
                paramvalue = url,
                msgdescription = msg,
                sortdate = DateTime.Now.ToString("yyyy-MM-dd"),
                receiverName = receivername,
                receiver = receiverid,
                sender = NG3.AppInfoBase.UserName,
                targetcboo = ocode
            };

            string result = Url.Encode(JsonConvert.SerializeObject(param));

            return result;
        }

        /// <summary>
        /// 保存单个附件
        /// </summary>
        /// <param name="tablename">附件所在表名</param>
        /// <param name="phid">主键id</param>
        /// <param name="busguid">附件会话id</param>
        /// <param name="asrflg">附件标志值</param>
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public string SaveOneAttach()
        {
            string tablename = System.Web.HttpContext.Current.Request.Params["tablename"];
            string busguid = System.Web.HttpContext.Current.Request.Params["busguid"];
            string phid = System.Web.HttpContext.Current.Request.Params["phid"];
            string asrflg = System.Web.HttpContext.Current.Request.Params["asrflg"];  //1有附件；0无附件；3附件标志无变化

            //附件保存
            string saveparms = string.Empty;

            if (!string.IsNullOrEmpty(busguid) && busguid.Length > 30)  //guid长度为36
            {
                saveparms += busguid + "||" + phid + "@@##";
            }

            if (!string.IsNullOrEmpty(saveparms))
            {
                saveparms = saveparms.TrimEnd('#', '@');
                NG3UploadFileService.BatchSave(saveparms, NG3.AppInfoBase.UserConnectString);
            }

            //更新表上的附件标志asr_flg
            int iret = 0;
            if (asrflg != "3")
            {
                DataTable dt = DbHelper.GetDataTable(string.Format("select * from {0} where 1=2", tablename));

                if (dt.Columns.Contains("asr_flg"))
                {
                    iret = DbHelper.ExecuteNonQuery(string.Format("update {0} set asr_flg = {1} where phid = {2}", tablename, asrflg, phid));
                }
            }

            JObject jResult = new JObject();

            if (iret != -1)
            {
                jResult.Add("status", "OK");
                return JsonConvert.SerializeObject(jResult);
            }
            else
            {
                jResult.Add("status", "Fail");
                return JsonConvert.SerializeObject(jResult);
            }
        }



        //----------------------------------------下面这几个函数不是前端调用过来的------------------------------------
        /// <summary>
        /// 保存物料事物特性
        /// </summary>
        public void SaveItemDetail(string itemdetailJson)
        {
            //try
            //{
            //    var client = new WebApiClient(ServiceURL, ApiReqParam, EnumDataFormat.Json);

            //    var param = new ParameterCollection();
            //    param.Add("itemdetail", itemdetailJson);

            //    var res = client.Get<string>("PMS/PMM/{controller}/{action}", param);

            //    if (res.IsError || res.Data == null)
            //        throw new Exception(res.ErrMsg + System.Environment.NewLine + res.SubErrMsg);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        /// <summary>
        /// 统一处理合同类型树 组织项目树过滤条件
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="cnttypes">合同类型ids</param>
        /// <param name="orgs">组织ids</param>
        /// <param name="projs">项目ids</param>
        public void AnalyOrgQuery(Dictionary<string, object> dic, ref long[] cnttypes, ref long[] orgs, ref long[] projs)
        {
            string[] strarray;

            if (dic.ContainsKey("CntType") && !string.IsNullOrEmpty(dic["CntType"].ToString()))
            {
                strarray = dic["CntType"].ToString().Split(',');
                if (strarray.Length > 0)
                {
                    cnttypes = new long[strarray.Length];
                    for (int i = 0; i < strarray.Length; i++)
                    {
                        if (strarray[i] != "")
                            cnttypes[i] = Convert.ToInt64(strarray[i]);
                    }
                }
            }

            if (dic.ContainsKey("OrgQuery") && !string.IsNullOrEmpty(dic["OrgQuery"].ToString()))
            {
                //var str = "#type:" + curNode.raw.nodeType + "#curOrg:" + curorg + "#subOrg:" + subOrg + "#curProj:" + curproj + "#cubProj:" + subProj + "#parentNode:" + parentNode.data['PhId'];
                strarray = dic["OrgQuery"].ToString().Split('#');
                if (strarray.Length == 7)
                {
                    string str_proj = string.Empty;
                    string str_org = string.Empty;
                    string[] proj_array;
                    string[] org_array;

                    if (strarray[1].Substring(5, 3) == "PRO")//项目节点
                    {
                        if (strarray[5].Length > 8)
                        {
                            str_proj = strarray[4].Substring(8, strarray[4].Length - 8) + "," + strarray[5].Substring(8, strarray[5].Length - 8);
                        }
                        else
                        {
                            str_proj = strarray[4].Substring(8, strarray[4].Length - 8);
                        }
                        proj_array = str_proj.Split(',');
                        projs = new long[proj_array.Length];
                        for (int i = 0; i < proj_array.Length; i++)
                        {
                            if (proj_array[i] != "")
                                projs[i] = Convert.ToInt64(proj_array[i]);
                        }

                        if (strarray[6].Length > 11)
                        {
                            str_org = strarray[6].Substring(11, strarray[6].Length - 11);
                            orgs = new long[1];
                            orgs[0] = Convert.ToInt64(str_org);
                        }
                    }
                    else
                    {
                        if (strarray[3].Length > 7)
                        {
                            str_org = strarray[2].Substring(7, strarray[2].Length - 7) + "," + strarray[3].Substring(7, strarray[3].Length - 7);
                        }
                        else
                        {
                            str_org = strarray[2].Substring(7, strarray[2].Length - 7);
                        }
                        org_array = str_org.Split(',');
                        orgs = new long[org_array.Length];
                        for (int i = 0; i < org_array.Length; i++)
                        {
                            if (org_array[i] != "")
                                orgs[i] = Convert.ToInt64(org_array[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 任务分解汇总下级单据
        /// </summary>
        /// <param name="dts">下级单据合集</param>
        /// <param name="id">上级单据主表主键</param>
        /// <param name="dt_subtotal">分组小计数据</param>
        /// <param name="bustype">业务类型1汇总, 2查看原始单据</param>
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public DataTable MergeGrid(DataTable dts, string id, DataTable dt_subtotal, string bustype)
        {
            DataTable dt = dts.Clone();
            Dictionary<string, string> groupdic = new Dictionary<string, string>();  //分组列
            Dictionary<string, string> subdic = new Dictionary<string, string>();    //小计列

            //得到所有分组列和小计列
            foreach (DataRow dr in dt_subtotal.Rows)
            {
                string dbfield = dr["dbfield"].ToString();
                string isgroup = dr["isgroup"].ToString();
                string issum = dr["issum"].ToString();
                string func = dr["func"].ToString();

                if (isgroup == "1")
                {
                    groupdic.Add(dbfield, dbfield);
                }

                if (issum == "1")
                {
                    subdic.Add(dbfield, func);
                }
            }

            //grid有分组小计则合并分组列值相同的小计列
            if (subdic.Count > 0)
            {
                //增加分组大列方便select
                dt.Columns.Add("task_groupfield");
                dts.Columns.Add("task_groupfield");
                foreach (DataRow dr in dts.Rows)
                {
                    foreach (KeyValuePair<string, string> col in groupdic)
                    {
                        dr["task_groupfield"] += dr[col.Value].ToString() + "|";
                    }
                }

                foreach (DataRow dr in dts.Rows)
                {
                    string groupfield = dr["task_groupfield"].ToString();
                    DataRow[] drs = dt.Select("task_groupfield = '" + groupfield + "'");

                    if (drs.Length > 0)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            //找出参与小计的列
                            if (subdic.ContainsKey(dc.ColumnName))
                            {
                                //sum,avg,max,min
                                switch (subdic[dc.ColumnName])
                                {
                                    case "sum":
                                        drs[0][dc.ColumnName] = Convert.ToDecimal(drs[0][dc.ColumnName]) + Convert.ToDecimal(dr[dc.ColumnName]);
                                        break;
                                    case "avg":
                                        drs[0][dc.ColumnName] = Convert.ToDecimal(drs[0][dc.ColumnName]) + Convert.ToDecimal(dr[dc.ColumnName]);
                                        break;
                                    case "max":
                                        if (Convert.ToDecimal(dr[dc.ColumnName]) > Convert.ToDecimal(drs[0][dc.ColumnName]))
                                        {
                                            drs[0][dc.ColumnName] = Convert.ToDecimal(dr[dc.ColumnName]);
                                        }
                                        break;
                                    case "min":
                                        if (Convert.ToDecimal(dr[dc.ColumnName]) < Convert.ToDecimal(drs[0][dc.ColumnName]))
                                        {
                                            drs[0][dc.ColumnName] = Convert.ToDecimal(dr[dc.ColumnName]);
                                        }
                                        break;
                                }

                                drs[0]["lineid"] = Convert.ToInt32(drs[0]["lineid"]) + 1;
                            }
                        }
                    }
                    else
                    {
                        DataRow dr_tmp = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (subdic.ContainsKey(dc.ColumnName) || groupdic.ContainsKey(dc.ColumnName))
                            {
                                dr_tmp[dc.ColumnName] = dr[dc.ColumnName];
                            }
                        }

                        if (dt.Columns.Contains("s_tree_no"))
                        {
                            dr_tmp["s_tree_id"] = dr["s_tree_id"];
                            dr_tmp["s_tree_pid"] = dr["s_tree_pid"];
                            dr_tmp["s_tree_no"] = dr["s_tree_no"];
                            dr_tmp["s_tree_name"] = dr["s_tree_name"];
                        }

                        //如果有按物料id列汇总则物料名称也要赋值
                        if (dt.Columns.Contains("itemid") && dt.Columns.Contains("c_name") && groupdic.ContainsKey("phid_itemdata"))
                        {
                            dr_tmp["c_name"] = dr["c_name"];
                        }

                        dr_tmp["s_groupfield"] = dr["s_groupfield"];
                        dr_tmp["task_groupfield"] = dr["task_groupfield"];
                        dr_tmp["m_code"] = id;
                        dr_tmp["lineid"] = 1;
                        dt.Rows.Add(dr_tmp);
                    }
                }

                //设置平均值，清空lineid，删掉分组大列
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        //找出参与小计的平均列
                        if (subdic.ContainsKey(dc.ColumnName) && subdic[dc.ColumnName] == "avg")
                        {
                            dr[dc.ColumnName] = Convert.ToDecimal(dr[dc.ColumnName]) / Convert.ToInt32(dr["lineid"]);
                        }
                    }
                    dr["lineid"] = DBNull.Value;
                }

                dt.Columns.Remove("task_groupfield");
            }
            //没有分组小计则返回所有下级单据
            else
            {
                foreach (DataRow dr in dts.Rows)
                {
                    DataRow dr_tmp = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dr_tmp[dc.ColumnName] = dr[dc.ColumnName];
                    }

                    dr_tmp["m_code"] = id;
                    dt.Rows.Add(dr_tmp);
                }
            }

            //删除上级单据明细数据，保存新汇总上来的数据
            if (bustype == "1")
            {
                try
                {
                    bool exception = false;  //调主键服务是否发生异常
                    string detailcode = string.Empty;

                    string sql = "delete from " + dts.TableName + " where m_code = '" + id + "'";
                    DbHelper.ExecuteNonQuery(sql);

                    var billId = new Enterprise3.NHORM.Rule.BillNoCommon();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr.RowState == DataRowState.Added)
                        {
                            if (!exception)
                            {
                                try
                                {
                                    detailcode = billId.GetBillId(dt.TableName, "phid").ToString();
                                }
                                catch (Exception)
                                {
                                    //因为url的关系可能调不到主键服务，则调自己写的取主键函数
                                    exception = true;
                                    detailcode = GetBillId(dt.TableName);
                                }
                            }
                            else
                            {
                                detailcode = (Convert.ToInt64(detailcode) + 1).ToString();
                            }

                            dr["phid"] = detailcode;
                        }
                    }

                    DbHelper.Update(dt, "select * from " + dt.TableName);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return dt;
        }

        /// <summary>
        /// 获取公共选项设置
        /// <summary>
        public string GetOptionValue(string type)
        {
            NG3.Bill.Base.OptionSetting optionSetting = new NG3.Bill.Base.OptionSetting();
            string optionValue = "";

            if (type == "edit")
            {
                optionValue = optionSetting.GetValueByKey("BillActionStyle", "EditBillDataBySelf", "");
            }
            else if (type == "delete")
            {
                optionValue = optionSetting.GetValueByKey("BillActionStyle", "DeleteBillDataBySelf", "");
            }
            else if (type == "check")
            {
                optionValue = optionSetting.GetValueByKey("BillActionStyle", "CheckBillDataWithoutSelf", "");
            }
            else if (type == "attach")
            {
                optionValue = optionSetting.GetValueByKey("BillActionStyle", "AllowedAttachmentOperationAfterCheck", "");
            }
            return optionValue;
        }

        /// <summary>
		/// 获取BillNo;
		/// <summary>
		public static string GetBillNo(string tablename)
        {
            string time = DateTime.Now.ToString("yyyyMMdd");
            string sql = string.Format("select max(bill_no) from {0} where bill_no like '{1}%'", tablename, time);
            string billno = DbHelper.GetString(sql);
            if (!string.IsNullOrWhiteSpace(billno))
                billno = (Convert.ToInt64(billno) + 1).ToString();
            else
                billno = string.Format("{0}0001", time);
            return billno;
        }

        /// <summary>
		/// 获取BillId;
		/// <summary>
		public static string GetBillId(string tablename)
        {
            string billid = DbHelper.GetString(string.Format("select max(phid) + 1 from {0} ", tablename));

            if (string.IsNullOrWhiteSpace(billid))
            {
                billid = "1";
            }

            return billid;
        }

        /// <summary>
        /// 获取Guid
        /// <summary>
        public string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
