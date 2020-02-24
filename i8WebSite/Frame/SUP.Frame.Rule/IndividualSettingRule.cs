using System;
using System.Collections.Generic;
using NG3;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using SUP.Common.Rule;
using SUP.Common.Interface;
//using GE.DataEntity.Common;
using NG3.Data.Service;
using System.IO;

namespace SUP.Frame.Rule
{
    public class IndividualSettingRule : IUserConfig
    {
        private IndividualSettingDac dac = null;
        public IndividualSettingRule()
        {
            dac = new IndividualSettingDac();
        }
        public DataTable LoadSysSetting(long userid)
        {
            //long phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual","phid");
            return dac.LoadSysSetting(userid);
        }  
        public bool SaveSysSetting(long userid, string masterdt)
        {
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            long phid = 0;
            if (obj == "0")
            {                
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual", "phid");
            }
            return dac.SaveSysSetting(userid, masterdt, phid);
        }        
        public IList<TreeJSONBase> LoadDefaultOpenTab(long userid, string nodeid)
        {
            string sql = string.Empty;
            string filter = string.Empty;

            if ("root" == nodeid)//首次加载
            {
                DataTable dt = this.GetMainTreeData(userid);
                filter = "(pid='root')";
                return new DefaultOpenTabTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);//加载两层
            }
            else
            {
                List<TreeJSONBase> l = new List<TreeJSONBase>();
                return l;
            }
        }
        public DataTable GetMainTreeData(long userid)
        {
            DataTable menudt = dac.LoadDefaultOpenTab(userid).Copy();

            return menudt;
        }
        public int SaveDefaultOpenTab(long userid,DataTable TreeTable)
        {
            List<long> phid = null;
            int count = TreeTable.Rows.Count;
            if(count > 0)           
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_defaultopen_tab", "phid", count);
            }
            return dac.SaveDefaultOpenTab(userid,TreeTable, phid);
        }

        public DataTable LoadServerSetting()
        {
            byte[] fileValue = dac.LoadServerSetting();
            if(fileValue!= null)
            {
                DataTable menudt = NG.Runtime.Serialization.SerializerBase.DeSerialize(fileValue) as DataTable;
                return menudt;
            }
            else
            {
                return null;
            }
           
        }
        public DataTable LoadNetWorkIPMappingInfo()
        {
            return dac.LoadNetWorkIPMappingInfo();
        }
        public bool SaveServerIpAndNetWorkIpConfig(DataTable dtServerISP, DataTable dtNetWorkIP)
        { 
           // Int64 code = SUP.Common.Rule.CommonUtil.GetBillId("fg_systemconfigfile", "code");
            return dac.SaveServerIpAndNetWorkIpConfig(dtServerISP, dtNetWorkIP);
        }

        public string[] LoadDisplaySetting(string ucode)
        {
           return dac.LoadDisplaySetting(ucode);
        }

        public bool SaveDisplaySetting(string ucode,string[] s)
        {
           // Int64 phid = SUP.Common.Rule.CommonUtil.GetBillId("fg_systemconfigfile", "code");
            return (dac.SaveDisplaySettingUcode(ucode, s[0]) && dac.SaveDisplaySettingCodeshowid(s[1]));
        }

        //方案分配
        public bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            List<long> phid = null;
            string sql = String.Format(@"select count(*) from fg3_defaultopen_tab where userid ={0} and usertype = {1}", fromUserId, fromUserType);
            string obj = DbHelper.GetString(sql).ToString();
            int count = 0;
            int.TryParse(obj, out count);
            if (count == 0)
            {
                return true;
            }
            phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_defaultopen_tab", "phid", count);
            return dac.UserConfigCopy(fromUserId, fromUserType, toUserId, toUserType, phid);
        }

        public bool DeleteUserConfig(long userid, int usertype)
        {
            return dac.UserConfigDel(userid, usertype);
        }

        //小铃铛
        public string LoadAlertItem()
        {
            return dac.LoadAlertItem();
        }  

        public bool SaveAlertItem(string alertconfig)
        {
            long userid = AppInfoBase.UserID;
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            long phid = 0;
            if (obj == "0")
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual", "phid");
            }
            return dac.SaveAlertItem(alertconfig, phid);
        }

        public string LoadPictureSet()
        {
            return dac.LoadPictureSet();
        }
        //public string GetSSOOrg()
        //{
        //    string Value = dac.GetSSOOrg();
        //    string EditValue = dac.GetName("fg_orglist", "oname", "ocode", Value, string.Empty, SSOInfoEntity.UserConnectionString);
        //    return EditValue;
        //}

        //public void SetSSOOrg(string Value)
        //{
        //    dac.UpdateSSOOrg(Value);            
        //}

        public string GetAPPLogoAttachId()
        {
            return dac.GetAPPLogoAttachId(AppInfoBase.UserID);
        }

        public bool SaveAPPLogo(string APPlogo, string attachid)
        {
            long userid = AppInfoBase.UserID;
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            long phid = 0;
            if (obj == "0")
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual", "phid");
            }
            return dac.SaveAPPLogo(phid, APPlogo, attachid, AppInfoBase.UserType == UserType.System ? 1 : 0);
        }

        public DataTable LoadDefaultOpenTabForMainFrame()
        {
            return dac.LoadDefaultOpenTabForMainFrame();
        }

    }
    class DefaultOpenTabTreeJSONBase : TreeJSONBase
    {
        public virtual string url { get; set; }
        public virtual string pid { get; set; }
        public virtual string name { get; set; }
        public virtual string originalcode { get; set; }
        public virtual long phid { get; set; }
        public virtual long userid { get; set; }
        public virtual string rightname { get; set; }
        public virtual string managername { get; set; }
        public virtual string moduleno { get; set; }
        public virtual string suite { get; set; }
        public virtual string urlparm { get; set; }
    }
    public class DefaultOpenTabTreeBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            DefaultOpenTabTreeJSONBase node = new DefaultOpenTabTreeJSONBase();
            node.id = dr["id"].ToString();
            node.text = dr["name"].ToString();
            node.phid = Int64.Parse(dr["phid"].ToString());
            node.pid = dr["pid"].ToString();
            node.name = dr["name"].ToString();
            node.originalcode = dr["originalcode"].ToString();
            node.userid = Int64.Parse(dr["userid"].ToString());
            node.leaf = !String.IsNullOrEmpty(dr["url"].ToString());
            node.url = dr["url"].ToString();
            node.rightname = dr["rightname"].ToString();
            node.managername = dr["managername"].ToString();
            node.moduleno = dr["moduleno"].ToString();
            node.suite = dr["suite"].ToString();
            node.urlparm = dr["urlparm"].ToString();
            return node;
        }
    }
}
