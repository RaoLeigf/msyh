using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public interface IIndividualSettingFacade
    {
        DataTable LoadSysSetting(long userid);

        bool SaveSysSetting(long userid, string masterdt);

        IList<TreeJSONBase> LoadDefaultOpenTab(long userid, string nodeid);

        int SaveDefaultOpenTab(long userid, DataTable masterdt);

        DataTable LoadServerSetting();

        string[] LoadDisplaySetting(string ucode);
        bool SaveDisplaySetting(string ucode, string[] s);

        DataTable LoadNetWorkIPMappingInfo();
        bool SaveServerIpAndNetWorkIpConfig(DataTable dtServerISP, DataTable dtNetWorkIP);

        //小铃铛
        string LoadAlertItem();

        bool SaveAlertItem(string alertconfig);

        //string GetSSOOrg();

        //void SetSSOOrg(string Value);

        string LoadPictureSet();

        string GetAPPLogoAttachId();

        bool SaveAPPLogo(string APPlogo,string attachid);

        DataTable LoadDefaultOpenTabForMainFrame();
    }
}
