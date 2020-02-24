using NG3.Data.Service;
using SUP.Frame.DataAccess;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Data;

namespace SUP.Frame.Facade
{
    public class SysMaintainCallFacade : ISysMaintainCallFacade
    {
        private SysMaintainCallDac dac = null;
        private SysMaintainCallRule rule = null;

        public SysMaintainCallFacade()
        {
            dac = new SysMaintainCallDac();
            rule = new SysMaintainCallRule();
        }

        public DataTable GetSysMaintainCall(string clientJsonQuery)
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return rule.GetSysMaintainCall(clientJsonQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public DataTable GetSysMaintainCallByPhid(string phid)
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return dac.GetSysMaintainCallByPhid(phid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public string CheckSysMaintainCallState()
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return dac.CheckSysMaintainCallState();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public bool DelSysMaintainCall(string phid)
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return dac.DelSysMaintainCall(phid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public string StartSysMaintainCall(string phid)
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return dac.StartSysMaintainCall(phid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public bool CloseSysMaintainCall(string phid)
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return dac.CloseSysMaintainCall(phid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public DataTable GetUcodeList()
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return dac.GetUcodeList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public string GetUserConnectString(string ucode)
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return dac.GetUserConnectString(ucode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

        public DataTable GetUserList(string connectString, int pageSize, int pageIndex, string searchkey, ref int totalRecord)
        {
            try
            {
                DbHelper.Open(connectString);
                return dac.GetUserList(connectString, pageSize, pageIndex, searchkey, ref totalRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(connectString);
            }
        }
        
        public string SaveSysMaintainCall(string phid, string title, string starttime, string preenddate, string endtype, string enddate, string runinfo, string endinfo, string netfreecall, string netfreecallucode, string allowlogin, string otype)
        {
            try
            {
                DbHelper.Open(NG3.AppInfoBase.PubConnectString);
                return rule.SaveSysMaintainCall(phid, title, starttime, preenddate, endtype, enddate, runinfo, endinfo, netfreecall, netfreecallucode, allowlogin, otype);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(NG3.AppInfoBase.PubConnectString);
            }
        }

    }

    public interface ISysMaintainCallFacade
    {
        DataTable GetSysMaintainCall(string clientJsonQuery);

        DataTable GetSysMaintainCallByPhid(string phid);

        string CheckSysMaintainCallState();

        bool DelSysMaintainCall(string phid);

        string StartSysMaintainCall(string phid);

        bool CloseSysMaintainCall(string phid);

        DataTable GetUcodeList();

        string GetUserConnectString(string ucode);

        DataTable GetUserList(string connectString, int pageSize, int pageIndex, string searchkey, ref int totalRecord);

        string SaveSysMaintainCall(string phid, string title, string starttime, string preenddate, string endtype, string enddate, string runinfo, string endinfo, string netfreecall, string netfreecallucode, string allowlogin, string otype);
    }
}
