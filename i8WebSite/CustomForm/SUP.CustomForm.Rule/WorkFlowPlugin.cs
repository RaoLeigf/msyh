using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.WorkFlow.Interfaces;
using NG3.Data.Service;
using System.Data;
using SUP.Common.Base;
using Newtonsoft.Json;
using NG3.Log.Log4Net;

namespace SUP.CustomForm.Rule
{

    public class WorkFlowPlugin : IWorkFlowPlugin
    {
        #region 日志相关
        private static ILogger _logger = null;
        internal static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(WorkFlowPlugin), LogType.logoperation);
                }
                return _logger;
            }
        }
        #endregion

        //审批
        public void Approve(WorkFlowExecutionContext ec)
        {           
            string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
            string eformid = bizID.Substring(5);
            string tableName = "p_form" + eformid + "_m";
            string code = ec.BillInfo.PK1;
            string checkUserID = ec.UserId;  //送审人
            string pform = "pform" + eformid;

            if (ec.BizAttachData != null)
            {
                new ServerFuncParserRule().FuncParser(pform, JsonConvert.SerializeObject(ec.BizAttachData));
            }

            string sql = string.Format("update " + tableName + " set ischeck=1, checkpsn='{0}', check_dt='{1}' where phid={2}",
                                        checkUserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code);

            int iret = DbHelper.ExecuteNonQuery(sql);
           
        }

        public void CancelApprove(WorkFlowExecutionContext ec)
        {
            string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
            string eformid = bizID.Substring(5);
            string tableName = "p_form" + eformid + "_m";
            string code = ec.BillInfo.PK1;
            string pform = "pform" + eformid;

            if (ec.BizAttachData != null)
            {
                string funcname = ec.BizAttachData["funcname"].ToString();
                if (funcname == "@@##")
                {
                    throw new Exception("取消审批");
                }
                new ServerFuncParserRule().FuncParser(pform, JsonConvert.SerializeObject(ec.BizAttachData));
            }

            string sql = string.Format("update " + tableName + " set ischeck=0, checkpsn='', check_dt=null where phid={0}", code);

            int iret = DbHelper.ExecuteNonQuery(sql);
        }

        public ApproveValidResult CheckApproveValid(WorkFlowExecutionContext ec)
        {
            return ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        public ApproveValidResult CheckBizSaveByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return ApproveValidResult.DefaultValue;
        }

        public ApproveValidResult CheckCancelApproveValid(WorkFlowExecutionContext ec)
        {
            return ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        public void EditUserTaskComplete(string compId, WorkFlowExecutionContext ec)
        {

        }

        public void FlowAbort(WorkFlowExecutionContext ec)
        {
            string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
            string eformid = bizID.Substring(5);
            string tableName = "p_form" + eformid + "_m";
            string code = ec.BillInfo.PK1;
            string checkUserID = ec.UserId;  //送审人

            string sql = string.Format("update " + tableName + " set is_wf=2 where phid={0}", code);

            int iret = DbHelper.ExecuteNonQuery(string.Format(sql));
        }

        //流程结束
        public void FlowEnd(WorkFlowExecutionContext ec)
        {
            string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
            string eformid = bizID.Substring(5);
            string tableName = "p_form" + eformid + "_m";
            string code = ec.BillInfo.PK1;
            string checkUserID = ec.UserId;  //送审人

            string sql = string.Format("update " + tableName + " set is_wf=2 where phid={0}", code);

            int iret = DbHelper.ExecuteNonQuery(string.Format(sql));
        }

        //送审，流程发起
        public void FlowStart(WorkFlowExecutionContext ec)
        {
            string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
            string eformid = bizID.Substring(5);
            string tableName = "p_form" + eformid + "_m";
            string code = ec.BillInfo.PK1;
            string checkUserID = ec.UserId;  //送审人

            string sql = string.Format("update " + tableName + " set is_wf=1 where phid={0}", code);

            int iret = DbHelper.ExecuteNonQuery(string.Format(sql));

        }

        public List<BizAttachment> GetBizAttachment(WorkFlowExecutionContext ec)
        {
            string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
            string eformid = bizID.Substring(5);
            string code = ec.BillInfo.PK1;
            string tableNameM = "p_form" + eformid + "_m";
            string tableNameD = "p_form" + eformid + "_d";
            List<BizAttachment> lbizatts = new List<BizAttachment>();

            //主表附件
            DataTable mstattdt = DbHelper.GetDataTable(string.Format(@"select * from attachment_record where asr_attach_table = 'c_pfc_attachment'
                and asr_table = '{0}' and asr_code = '{1}'", tableNameM, code));

            if (mstattdt != null && mstattdt.Rows.Count > 0)
            {
                foreach (DataRow row in mstattdt.Rows)
                {
                    BizAttachment mstbizatt = new BizAttachment();
                    mstbizatt.AsrAttachTable = "c_pfc_attachment";
                    mstbizatt.AsrCode = code;
                    mstbizatt.AsrTable = tableNameM;
                    mstbizatt.AttachName = row["asr_name"].ToString();
                    mstbizatt.AttachSize = Convert.ToSingle(row["asr_size"]);
                    mstbizatt.AttachTime = Convert.ToDateTime(row["asr_filldt"]);
                    lbizatts.Add(mstbizatt);
                }
            }

            //明细表附件
            DataTable detailList = DbHelper.GetDataTable(string.Format("select tname from ctl_tbl where tname like '{0}%'", tableNameD));  //获取所有明细表dt
            if (detailList != null && detailList.Rows.Count > 0)
            {
                DataTable[] dtD = new DataTable[detailList.Rows.Count];  //所有明细表数据
                DataTable[] detattdt = new DataTable[detailList.Rows.Count];  //每张明细表附件数据

                for (int i = 0; i < detailList.Rows.Count; i++)
                {
                    tableNameD = detailList.Rows[i]["tname"].ToString();
                    dtD[i] = DbHelper.GetDataTable(string.Format("select phid from {0} where m_code = {1}", tableNameD, code));

                    if (dtD[i] != null && dtD[i].Rows.Count > 0)
                    {
                        string detasrcodes = string.Empty;
                        foreach (DataRow row in dtD[i].Rows)
                        {
                            detasrcodes += "'" + row["phid"].ToString() + "'" + ",";
                        }
                        detasrcodes = detasrcodes.TrimEnd(',');

                        detattdt[i] = DbHelper.GetDataTable(string.Format(@"select * from attachment_record where asr_attach_table = 'c_pfc_attachment'
                            and asr_table = '{0}' and asr_code in ({1})", tableNameD, detasrcodes));

                        foreach (DataRow row in detattdt[i].Rows)
                        {
                            BizAttachment detbizatt = new BizAttachment();
                            detbizatt.AsrAttachTable = "c_pfc_attachment";
                            detbizatt.AsrCode = row["asr_code"].ToString();
                            detbizatt.AsrTable = tableNameD;
                            detbizatt.AttachName = row["asr_name"].ToString();
                            detbizatt.AttachSize = Convert.ToSingle(row["asr_size"]);
                            detbizatt.AttachTime = Convert.ToDateTime(row["asr_filldt"]);
                            lbizatts.Add(detbizatt);
                        }
                    }

                }
            }

            return lbizatts;
        }

        public BizToPdfEntity GetBizToPdfEntity(WorkFlowExecutionContext ec)
        {
            try
            {
                string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
                string eformid = bizID.Substring(5);
                string tableNameM = "p_form" + eformid + "_m";
                string tableNameD = "p_form" + eformid + "_d";
                string code = ec.BillInfo.PK1;

                DataSet ds = new DataSet();
                DataTable dtM = DbHelper.GetDataTable(string.Format("select * from {0} where phid={1}", tableNameM, code));
                dtM.TableName = tableNameM;
                ds.Tables.Add(dtM);

                //获取所有明细表dt
                DataTable detailList = DbHelper.GetDataTable(string.Format("select tname from ctl_tbl where tname like '{0}%'", tableNameD));
                if (detailList != null && detailList.Rows.Count > 0)
                {
                    DataTable[] dtD = new DataTable[detailList.Rows.Count];

                    for (int i = 0; i < detailList.Rows.Count; i++)
                    {
                        dtD[i] = DbHelper.GetDataTable(string.Format("select * from {0} where m_code={1}", detailList.Rows[i]["tname"].ToString(), code));
                        dtD[i].TableName = detailList.Rows[i]["tname"].ToString() + "grid";
                        ds.Tables.Add(dtD[i]);
                    }
                }

                BizToPdfEntity btpEntity = new BizToPdfEntity();
                btpEntity.DataSource = ds;
                btpEntity.TemplateId = bizID;

                return btpEntity;
            }
            catch (Exception e)
            {
                string msgStr = "GetBizToPdfEntity：" + e.Message + "\r\n\r\n" + e.StackTrace;
                Logger.Error(msgStr);
                //throw new Exception(msgStr);
                return null;
            }
        }

        public bool SaveBizDataByMobileApp(WorkFlowExecutionContext ec, Dictionary<string, string> jsonData)
        {
            string bizID = ec.BillInfo.BizID;  //业务类型EFORM0000000130
            string eformid = bizID.Substring(5);
            string tableNameM = "p_form" + eformid + "_m";
            string tableNameD = "p_form" + eformid + "_d";

            if (jsonData.ContainsKey(tableNameM) && !string.IsNullOrEmpty(jsonData[tableNameM].ToString()))
            {
                DataTable dtM = DataConverterHelper.ToDataTable(jsonData[tableNameM].ToString(), string.Format("select * from {0}", tableNameM));
                DbHelper.Update(dtM, string.Format("select * from {0}", tableNameM));
            }

            //获取所有明细表dt
            DataTable detailList = DbHelper.GetDataTable(string.Format("select tname from ctl_tbl where tname like '{0}%'", tableNameD));
            if (detailList != null && detailList.Rows.Count > 0)
            {
                DataTable[] dtD = new DataTable[detailList.Rows.Count];

                for (int i = 0; i < detailList.Rows.Count; i++)
                {
                    tableNameD = detailList.Rows[i]["tname"].ToString();

                    if (jsonData.ContainsKey(tableNameD) && !string.IsNullOrEmpty(jsonData[tableNameD].ToString()))
                    {
                        dtD[i] = DataConverterHelper.ToDataTable(jsonData[tableNameD].ToString(), string.Format("select * from {0}", tableNameD));
                        DbHelper.Update(dtD[i], string.Format("select * from {0}", tableNameD));
                    }
                }
            }

            return true;
        }
    }
}
