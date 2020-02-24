using NG3;
using SUP.Common.Base;
using SUP.Frame.DataAccess;
using SUP.Frame.Facade.Interface;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade
{
    public class OptionSettingFacade: IOptionSettingFacade
    {

        private OptionSettingDac dac = new OptionSettingDac();
        private OptionSettingRule rule = new OptionSettingRule();

        [DBControl]
        public IList<TreeJSONBase> LoadOptionTree(string moduleid)
        {
            return rule.LoadOptionTree(moduleid); 
        }

        [DBControl]
        public DataTable GetGridList(Int64 phid,int pageSize,int pageIndex, ref int totalRecord,string type)
        {
            return dac.GetGridList(phid,pageSize,pageIndex,ref totalRecord,type);
        }

        [DBControl]
        public DataTable GetTaxOrgGrid(string phid)
        {
            return dac.GetTaxOrgGrid(phid);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveTaxOrg(DataTable dt)
        {
            return dac.SaveTaxOrg(dt);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveFunData(DataTable dt)
        {
            return dac.SaveFunData(dt);
        }
        [DBControl]
        public DataTable GetOptionValue(Int64 phid)
        {
            return dac.GetOptionValue(phid);
        }


        [DBControl]
        public string GetOptionDetail(string option_group, string option_code)
        {
            return rule.GetOptionDetail(option_group, option_code);
        }

        [DBControl]
        public Dictionary<string, string> GetArgumentDic(string option_group, string option_code, string[] keys)
        {
            return rule.GetArgumentDic(option_group, option_code,keys);
        }

        [DBControl]
        public string GetSingleArgument(string option_group, string option_code, string key)
        {
            return rule.GetSingleArgument(option_group, option_code, key);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveDetailData(DataTable dt,string logid)
        {
            return dac.SaveDetailData(dt,logid);
        }


        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public DataTable GetArgumentByPhid(string detailPhid)
        {
            return dac.GetArgumentByPhid(detailPhid);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveInitSetting()
        {
            return dac.SaveInitSetting();
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int GetInitSetting()
        {
            return dac.GetInitSetting();
        }


        [DBControl]
        public string GetValueByKey(string option_group, string option_code, string key)
        {
            return rule.GetValueByKey(option_group, option_code, key);
        }


        [DBControl]
        public IList<TreeJSONBase> LoadOrgTree(string detailPhid)
        {
            return rule.LoadOrgTree(detailPhid);
        }
    }
}
