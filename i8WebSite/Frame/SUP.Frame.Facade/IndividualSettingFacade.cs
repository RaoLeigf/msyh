using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.Rule;
using NG3;
using SUP.Frame.DataEntity;
using System.Data;


namespace SUP.Frame.Facade
{
    public class IndividualSettingFacade : IIndividualSettingFacade
    {
        private IndividualSettingRule rule = new IndividualSettingRule();
        [DBControl]
        public DataTable LoadSysSetting(long userid)
        {
            return rule.LoadSysSetting(userid);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool SaveSysSetting(long userid, string masterdt)
        {
            return rule.SaveSysSetting(userid, masterdt);
        }
        [DBControl]
        public IList<TreeJSONBase> LoadDefaultOpenTab(long userid, string nodeid)
        {
            return rule.LoadDefaultOpenTab(userid,nodeid);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveDefaultOpenTab(long userid, DataTable masterdt)
        {
            return rule.SaveDefaultOpenTab(userid, masterdt);
        }
        [DBControl]
        public DataTable LoadServerSetting()
        {
            return rule.LoadServerSetting();
        }
        [DBControl]
        public DataTable LoadNetWorkIPMappingInfo()
        {
            return rule.LoadNetWorkIPMappingInfo();
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool SaveServerIpAndNetWorkIpConfig(DataTable dtServerISP, DataTable dtNetWorkIP)
        {
            return rule.SaveServerIpAndNetWorkIpConfig(dtServerISP, dtNetWorkIP);
        }
        [DBControl]
        public string[] LoadDisplaySetting(string ucode)
        {
            return rule.LoadDisplaySetting(ucode);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool SaveDisplaySetting(string ucode, string[] s)
        {
            return rule.SaveDisplaySetting(ucode, s);
        }
        [DBControl]
        //小铃铛
        public string LoadAlertItem()
        {
            return rule.LoadAlertItem();
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool SaveAlertItem(string alertconfig)
        {
            return rule.SaveAlertItem(alertconfig);
        }
        //[DBControl]
        //public string GetSSOOrg()
        //{
        //    return rule.GetSSOOrg();
        //}
        //[DBControl]
        //public void SetSSOOrg(string Value)
        //{
        //    rule.SetSSOOrg(Value);
        //}
        [DBControl]
        public string LoadPictureSet()
        {
            return rule.LoadPictureSet();
        }

        [DBControl]
        public string GetAPPLogoAttachId()
        {
            return rule.GetAPPLogoAttachId();
        }

        [DBControl]
        public bool SaveAPPLogo(string APPlogo, string attachid)
        {
            return rule.SaveAPPLogo(APPlogo, attachid);
        }

        [DBControl]
        public DataTable LoadDefaultOpenTabForMainFrame()
        {
            return rule.LoadDefaultOpenTabForMainFrame();
        }

    }
}
