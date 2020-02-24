using i6.Biz;
using i6.Web.i6Service;
using I6.Base.Message;
using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;

namespace SUP.Frame.DataAccess
{
    public class ApplyCheckDac
    {

        /// <summary>
        /// 确认去审按钮事件
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool ConfirmApplyCheck(string _ucode, string _ocode, string _logid, string _ccode, string paramvalue, string msgdescription, DateTime sortdate, string receiver, string sender, string targetcboo)
        {
            try
            {
                _ucode = _ucode == "" ? NG3.AppInfoBase.UCode : _ucode;
                _ocode = _ocode == "" ? NG3.AppInfoBase.OCode : _ocode;
                _logid = _logid == "" ? NG3.AppInfoBase.LoginID : _logid;
                _ccode = _ccode == "" ? System.Guid.NewGuid().ToString() : _ccode;
                sender = sender == "" ? NG3.AppInfoBase.UserID.ToString() : sender;
                BusinessDataPushService businessPush = new BusinessDataPushService();

                return businessPush.BusinessInsertData(_ucode, _ocode, _logid, _ccode, paramvalue, msgdescription, "DMC", "ApplyCheck", DateTime.Now, receiver, sender, targetcboo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 确认去审按钮事件
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string ConfirmApplyCheck(string _ucode, string _ocode, string _logid, string _ccode, string paramvalue, string msgdescription, DateTime sortdate, string receiver, string sender, string targetcboo,out string msg)
        {
            try
            {
                _ucode = _ucode == "" ? NG3.AppInfoBase.UCode : _ucode;
                _ocode = _ocode == "" ? NG3.AppInfoBase.OCode : _ocode;
                _logid = _logid == "" ? NG3.AppInfoBase.LoginID : _logid;
                _ccode = _ccode == "" ? System.Guid.NewGuid().ToString() : _ccode;
                sender = sender == "" ? NG3.AppInfoBase.UserID.ToString() : sender;
                BusinessDataPushService businessPush = new BusinessDataPushService();
                bool success = true;
                int rows = this.GetBusData(paramvalue,receiver);
                if (rows > 0)
                {
                    success = false;
                    msg = "当前单据已经申请去审，请勿重复申请";
                }
                else {
                    msg = "";
                    if (!businessPush.BusinessInsertData(_ucode, _ocode, _logid, _ccode, paramvalue, msgdescription, "DMC", "ApplyCheck", DateTime.Now, receiver, sender, targetcboo))
                    {
                        success = false;
                        msg = "申请去审失败";
                    }
                    else {
                        DesktopPortalRefreshNotice notice = new DesktopPortalRefreshNotice();

                        DataTable timeTriggerDt = MessageTimeManager.GetTimeTriggerObjectDt();

                        DataRow timeTriggerDr = timeTriggerDt.NewRow();
                        timeTriggerDr["uid"] = receiver;// i6SessionInfo.AppInfo.UserID;    //_logid;
                        timeTriggerDr["utype"] = ReceiverType.Type_Ope;
                        timeTriggerDt.Rows.Add(timeTriggerDr);

                        notice.NoticeDesktopRefreshMsg(NG3.AppInfoBase.UserConnectString, _logid, i6.Biz.PortalType.Portal_BusinessAlert, timeTriggerDt);
                    }
                }
               
                return "{\"success\":\"" + success + "\",\"msg\":\"" + msg + "\"}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除申请去审数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool DeleteApplyCheck(string _ucode, string _ocode, string _logid, string paramvalue, string receiver)
        {
            try
            {
                _ucode = _ucode == "" ? NG3.AppInfoBase.UCode : _ucode;
                _ocode = _ocode == "" ? NG3.AppInfoBase.OCode : _ocode;
                _logid = _logid == "" ? NG3.AppInfoBase.UserID.ToString() : _logid;
                BusinessDataPushService businessPush = new BusinessDataPushService();
                return businessPush.BusinessTaskDelete(_ucode, _ocode, _logid, "DMC", "ApplyCheck", paramvalue, receiver);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除申请去审数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int GetBusData(string paramvalue, string receiver)
        {
            try
            {
                int rows = 0;
                string sql = "SELECT * FROM FG_PORTAL_BUSDATA WHERE PARAMVALUE='"+paramvalue+"' AND RECEIVER='"+receiver+"'";
                DataTable dt = DbHelper.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0) {
                    rows = dt.Rows.Count;
                }
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
