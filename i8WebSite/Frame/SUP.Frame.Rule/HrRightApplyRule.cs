using DMC3.Rights.Model.Business;
using DMC3.Rights.Facade.Interface;
using Enterprise3.Common.Model;
using Enterprise3.NHORM.Base.SpringNHBase;
using Newtonsoft.Json.Linq;
using NG3.Data.Service;
using NG3.WorkFlow.Interfaces;
using SUP.Frame.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

namespace SUP.Frame.Rule
{
    public class HrRightApplyRule : IWorkFlowPlugin
    {

        private HrRightApplyDac dac = null;

        public HrRightApplyRule()
        {
            dac = new HrRightApplyDac();
        }

        public DataTable GetHrRightApply(string clientJsonQuery)
        {
            DataTable dt = new DataTable();
            dt.TableName = "HrRightApply";

            dt.Columns.Add(new DataColumn("phid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("rowno", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("checkstate", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("billno", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("billname", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("fillpsnid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("fillpsnname", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("filldate", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("ischeck", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("checkpsn", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("checkdate", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("remark", Type.GetType("System.String")));
            
            //获取下属员工Userid,拼成字符串列表，并加上当前用户
            DataTable underHrUseridDt = dac.GetUnderHrUserid();
            List<string> users = new List<string>();
            for (int i = 0; i < underHrUseridDt.Rows.Count; i++)
            {
                users.Add(underHrUseridDt.Rows[i]["phid"].ToString());
            }
            users.Add(NG3.AppInfoBase.UserID.ToString());

            DataTable hrRightApplyDt = dac.GetQueryHrRightApplyDt(clientJsonQuery);
            int rowno = 1;
            for (int i = 0; i < hrRightApplyDt.Rows.Count; i++)
            {
                string fillpsnid = hrRightApplyDt.Rows[i]["fillpsnid"].ToString();
                if (!users.Contains(fillpsnid))
                {
                    continue;
                }

                DataRow dr = dt.NewRow();
                dr["phid"] = hrRightApplyDt.Rows[i]["phid"];
                dr["rowno"] = rowno;
                rowno++;
                dr["checkstate"] = hrRightApplyDt.Rows[i]["checkstate"];
                dr["ischeck"] = hrRightApplyDt.Rows[i]["ischeck"];
                dr["billno"] = hrRightApplyDt.Rows[i]["billno"];
                dr["billname"] = hrRightApplyDt.Rows[i]["billname"];
                dr["fillpsnid"] = fillpsnid;
                dr["fillpsnname"] = hrRightApplyDt.Rows[i]["fillpsnname"].ToString();
                string filldate = hrRightApplyDt.Rows[i]["filldate"].ToString();
                if (!string.IsNullOrEmpty(filldate))
                {
                    filldate = filldate.Substring(0, filldate.IndexOf(" "));
                    filldate = filldate.Replace("/","-");
                    dr["filldate"] = filldate;
                }
                dr["checkpsn"] = hrRightApplyDt.Rows[i]["checkpsn"].ToString();
                string checkdate = hrRightApplyDt.Rows[i]["checkdate"].ToString();
                if (!string.IsNullOrEmpty(checkdate))
                {
                    checkdate = checkdate.Substring(0, checkdate.IndexOf(" "));
                    checkdate = checkdate.Replace("/", "-");
                    dr["checkdate"] = checkdate;
                }
                dr["remark"] = hrRightApplyDt.Rows[i]["remark"];
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public DataTable GetApplicantInfo(string code)
        {
            DataTable dt = new DataTable();
            dt.TableName = "ApplicantInfo";

            dt.Columns.Add(new DataColumn("hrid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("userid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("hrname", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("dept", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("station", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("remark", Type.GetType("System.String")));

            string[] hrids = code.Split(',');
            for (int i = 0; i < hrids.Length; i++)
            {
                DataRow dr = dt.NewRow();
                DataTable hrDt = dac.GetHrNameDeptDt(hrids[i]);
                dr["hrid"] = hrids[i];
                dr["userid"] = dac.GetUserIdByHrid(hrids[i]);
                dr["hrname"] = hrDt.Rows[0]["cname"];
                dr["dept"] = hrDt.Rows[0]["oname"];
                dr["station"] = dac.GetHrStation(hrids[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public DataTable GetOrgInfo(string userid, string billno)
        {
            DataTable dt = new DataTable();
            dt.TableName = "OrgInfo";

            dt.Columns.Add(new DataColumn("select", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("orgid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("orgname", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("fillpsnorg", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("applicantorg", Type.GetType("System.String")));

            if (!string.IsNullOrEmpty(billno))
            {
                DataTable orgDt = dac.GetOrgDt(userid, billno);
                for (int i = 0; i < orgDt.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["select"] = orgDt.Rows[i]["isselect"].ToString() == "1" ? true : false;
                    dr["orgid"] = orgDt.Rows[i]["orgid"];
                    dr["orgname"] = orgDt.Rows[i]["orgname"];

                    string fillpsnorg = orgDt.Rows[i]["fillpsnorg"].ToString();
                    if (fillpsnorg == "1")
                    {
                        dr["fillpsnorg"] = "登录组织/信息组织";
                    }
                    else if (fillpsnorg == "2")
                    {
                        dr["fillpsnorg"] = "信息组织";
                    }
                    string applicantorg = orgDt.Rows[i]["applicantorg"].ToString();
                    if (applicantorg == "1")
                    {
                        dr["applicantorg"] = "登录组织/信息组织";
                    }
                    else if (applicantorg == "2")
                    {
                        dr["applicantorg"] = "信息组织";
                    }

                    dt.Rows.Add(dr);
                }
            }

            if (string.IsNullOrEmpty(billno) || dt.Rows.Count == 0)
            {
                long curuserid = NG3.AppInfoBase.UserID;
                DataTable fillPsnOrgDt = dac.GetOrgIdName(curuserid);
                for (int i = 0; i < fillPsnOrgDt.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["select"] = false;
                    string orgid = fillPsnOrgDt.Rows[i]["orgid"].ToString();
                    dr["orgid"] = orgid;
                    dr["orgname"] = fillPsnOrgDt.Rows[i]["oname"];

                    string userOrgCount = dac.GetUserOrgCount(curuserid.ToString(), orgid);
                    dr["fillpsnorg"] = userOrgCount == "2" ? "登录组织/信息组织" : "信息组织";

                    userOrgCount = dac.GetUserOrgCount(userid, orgid);
                    if (userOrgCount == "2")
                    {
                        dr["select"] = true;
                        dr["applicantorg"] = "登录组织/信息组织";
                    }
                    else if (userOrgCount == "1")
                    {
                        dr["select"] = true;
                        dr["applicantorg"] = "信息组织";
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public DataTable GetRoleInfo(string fillpsnid, string userid, string orgid, string billno)
        {
            DataTable dt = new DataTable();
            dt.TableName = "RoleInfo";

            dt.Columns.Add(new DataColumn("rowno", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("select", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("roleid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("roleno", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("rolename", Type.GetType("System.String")));

            if (string.IsNullOrEmpty(fillpsnid))
            {
                fillpsnid = NG3.AppInfoBase.UserID.ToString();
            }

            DataTable userRoleOrgDt = dac.GetUserRoleOrg(string.IsNullOrEmpty(billno) ? NG3.AppInfoBase.UserID.ToString() : fillpsnid, orgid);
            for (int i = 0; i < userRoleOrgDt.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["rowno"] = i + 1;
                string roleid = userRoleOrgDt.Rows[i]["phid"].ToString();
                if (string.IsNullOrEmpty(billno))
                {
                    dr["select"] = dac.CheckUserRoleOrg(userid, roleid, orgid);
                }
                else
                {
                    dr["select"] = dac.CheckUserRoleOrg(userid, roleid, orgid, billno);
                }
                dr["roleid"] = roleid;
                dr["roleno"] = userRoleOrgDt.Rows[i]["roleno"];
                dr["rolename"] = userRoleOrgDt.Rows[i]["rolename"];
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public bool SaveHrRightApply(string phid, string billno, string billname, string remark, string applicantObj, string orgObj, string roleObj, string otype, string removeApplicant)
        {
            JObject applicantJObj = JObject.Parse(applicantObj);
            JObject orgJObj = JObject.Parse(orgObj);
            JObject roleJObj = JObject.Parse(roleObj);
            try
            {
                DbHelper.BeginTran();

                if (otype == "add")
                {
                    if (string.IsNullOrEmpty(phid))
                    {
                        phid = SUP.Common.Rule.CommonUtil.GetPhId("fg3_hrrightapply").ToString();
                    }
                    dac.InsertHrRightApplyDt(phid, billno, billname, remark);
                }
                else if (otype == "edit")
                {
                    dac.UpdateHrRightApplyDt(billno, billname, remark);
                }

                JArray applicantJArr = JArray.Parse(applicantJObj["store"].ToString());
                if (otype == "edit")
                {
                    dac.DeleteApplicantDt(billno);
                }
                foreach (var applicant in applicantJArr)
                {
                    long applicantphid = SUP.Common.Rule.CommonUtil.GetPhId("fg3_hrrightapplicant");
                    string hrid = applicant["hrid"].ToString();
                    string userid = applicant["userid"].ToString();
                    if (otype == "edit" && removeApplicant.IndexOf(userid + ",") > -1)
                    {
                        removeApplicant = removeApplicant.Replace(userid + ",", "");
                    }
                    string hrname = applicant["hrname"].ToString();
                    string dept = applicant["dept"].ToString();
                    string station = applicant["station"].ToString();
                    string applicantremark = applicant["remark"].ToString();

                    dac.InsertApplicantDt(applicantphid, billno, hrid, userid, hrname, dept, station, applicantremark);

                    if (orgJObj[userid] != null)
                    {
                        JArray orgJArr = JArray.Parse(orgJObj[userid].ToString());
                        if (otype == "edit")
                        {
                            dac.DeleteOrgDt(billno, userid);
                        }
                        foreach (var org in orgJArr)
                        {
                            long orgphid = SUP.Common.Rule.CommonUtil.GetPhId("fg3_hrrightorg");
                            string orgid = org["orgid"].ToString();
                            string orgname = org["orgname"].ToString();
                            int isselect = org["select"].ToString() == "True" ? 1 : 0;

                            int fillpsnorg = 0;
                            int applicantorg = 0;
                            if (org["fillpsnorg"].ToString() == "登录组织/信息组织")
                            {
                                fillpsnorg = 1;
                            }
                            else if (org["fillpsnorg"].ToString() == "信息组织")
                            {
                                fillpsnorg = 2;
                            }
                            if (org["applicantorg"].ToString() == "登录组织/信息组织")
                            {
                                applicantorg = 1;
                            }
                            else if (org["applicantorg"].ToString() == "信息组织")
                            {
                                applicantorg = 2;
                            }

                            dac.InsertOrgDt(orgphid, billno, userid, orgid, orgname, isselect, fillpsnorg, applicantorg);

                            if (roleJObj[userid + "|" + orgid] != null)
                            {
                                if (otype == "edit")
                                {
                                    dac.DeleteRoleDt(billno, userid, orgid);
                                }
                                JArray roleJArr = JArray.Parse(roleJObj[userid + "|" + orgid].ToString());
                                foreach (var role in roleJArr)
                                {
                                    long rolephid = SUP.Common.Rule.CommonUtil.GetPhId("fg3_hrrightrole");
                                    string roleid = role["roleid"].ToString();
                                    string rolename = role["rolename"].ToString();
                                    if (role["select"].ToString() == "True")
                                    {
                                        dac.InsertRoleDt(rolephid, billno, userid, orgid, roleid, rolename);
                                    }
                                }
                            }
                        }
                    }
                }

                if (otype == "edit" && !string.IsNullOrEmpty(removeApplicant))
                {
                    string[] useridArr = removeApplicant.Split(',');
                    for (int i = 0; i < useridArr.Length - 1; i++)
                    {
                        dac.DeleteRemovedOrgRoleDt(billno, useridArr[i]);
                    }
                }

                if (otype == "add")
                {
                    ResBillNoOrIdEntity entity = SUP.Common.Rule.CommonUtil.GetBillNoIntensive("HrRightApply");
                    SUP.Common.Rule.CommonUtil.CommitBillNo("HrRightApply", entity);//提交单据用户编码，此操作将单据用户编码永久占用
                }

                DbHelper.CommitTran();
            }
            catch
            {
                DbHelper.RollbackTran();
                return false;
            }

            return true;
        }

        public DataTable GetHrRightApplyPrintInfo(string billno)
        {
            DataTable dt = new DataTable();
            dt.TableName = "PrintInfo";

            dt.Columns.Add(new DataColumn("hrname", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("dept", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("station", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("orgname", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("rolename", Type.GetType("System.String")));
           
            DataTable applicantDt = dac.GetApplicantDt(billno);
            DataTable orgDt = dac.GetSelectOrgDt(billno);
            DataTable roledt = dac.GetRoleDtByBillNo(billno);
            for (int i = 0; i < roledt.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                DataRow[] rows = applicantDt.Select("userid = " + roledt.Rows[i]["userid"]);
                dr["hrname"] = rows[0]["hrname"];
                dr["dept"] = rows[0]["dept"];
                dr["station"] = rows[0]["station"];
                rows = orgDt.Select("userid = " + roledt.Rows[i]["userid"] + " and orgid = " + roledt.Rows[i]["orgid"]);
                if (rows.Length == 0)
                {
                    continue;
                }
                string applicantorg = string.Empty;
                if (rows[0]["applicantorg"].ToString() == "1") {
                    applicantorg = "登录组织/信息组织";
                }
                if (rows[0]["applicantorg"].ToString() == "2")
                {
                    applicantorg = "信息组织";
                }
                dr["orgname"] = rows[0]["orgname"] + "[" + applicantorg + "]";
                dr["rolename"] = roledt.Rows[i]["rolename"];
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #region 工作流

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

        //审批
        public void Approve(WorkFlowExecutionContext ec)
        {
            string phid = ec.BillInfo.PK1;

            IUserFacade facade = new SpringObject().GetObject<IUserFacade>("DMC3.Rights.Facade.User");

            List<UserApproveRightsModel> userApproveRights = new List<UserApproveRightsModel>();

            string billno = dac.GetHrRightApplyBillNo(phid);
            DataTable applicantDt = dac.GetApplicantDt(billno);
            for (int i = 0; i < applicantDt.Rows.Count; i++)
            {
                UserApproveRightsModel model = new UserApproveRightsModel();
                model.LoginOrgs = new List<long>();
                model.InfoOrgs = new List<long>();
                model.UserOrgRoles = new List<UserApproveRightsModel.UserOrgRole>();
                model.HrId = long.Parse(applicantDt.Rows[i]["hrid"].ToString());
                model.UserId = long.Parse(applicantDt.Rows[i]["userid"].ToString());
                model.DeptId = long.Parse(dac.GetApplicantDeptId(model.HrId));               
                DataTable OrgDt = GetOrgInfo(model.UserId.ToString(), billno);
                for (int j = 0; j < OrgDt.Rows.Count; j++)
                {
                    if ((bool)OrgDt.Rows[j]["select"] == true)
                    {
                        long orgid = long.Parse(OrgDt.Rows[j]["orgid"].ToString());
                        if (OrgDt.Rows[j]["applicantorg"].ToString() == "登录组织/信息组织")
                        {
                            model.LoginOrgs.Add(orgid);
                            model.InfoOrgs.Add(orgid);
                        }
                        else if (OrgDt.Rows[j]["applicantorg"].ToString() == "信息组织")
                        {
                            model.InfoOrgs.Add(orgid);
                        }
                   }
                }

                DataTable roleDt = dac.GetRoleDtByBillNoNUserId(billno, model.UserId.ToString());
                for (int k = 0; k < roleDt.Rows.Count; k++)
                {
                    UserApproveRightsModel.UserOrgRole userOrgRole = new UserApproveRightsModel.UserOrgRole();
                    userOrgRole.OrgId = long.Parse(roleDt.Rows[k]["orgid"].ToString());
                    userOrgRole.RoleId = long.Parse(roleDt.Rows[k]["roleid"].ToString());
                    model.UserOrgRoles.Add(userOrgRole);
                }

                userApproveRights.Add(model);
            }

            facade.SaveUserApproveRights(userApproveRights);

            dac.UpdateWorkFlowCheckPsnDate(ec.BillInfo.PK1);
        }

        public void CancelApprove(WorkFlowExecutionContext ec)
        {

        }

        public void FlowStart(WorkFlowExecutionContext ec)
        {
            dac.UpdateWorkFlowCheckState(ec.BillInfo.PK1, "1");
        }

        //流程结束
        public void FlowEnd(WorkFlowExecutionContext ec)
        {
            dac.UpdateWorkFlowCheckState(ec.BillInfo.PK1, "2");
        }

        //流程被终止
        public void FlowAbort(WorkFlowExecutionContext ec)
        {

        }

        public List<BizAttachment> GetBizAttachment(WorkFlowExecutionContext ec)
        {
            return new List<BizAttachment>();
        }

        public BizToPdfEntity GetBizToPdfEntity(WorkFlowExecutionContext ec)
        {
            return new BizToPdfEntity();
        }

        public bool SaveBizDataByMobileApp(WorkFlowExecutionContext ec, Dictionary<string, string> jsonData)
        {
            return true;
        }

        #endregion

    }
}
