using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade.Interface
{
    public interface IOptionSettingFacade
    {

        IList<TreeJSONBase> LoadOptionTree(string moduleid);
        DataTable GetGridList(Int64 phid, int pageSize, int pageIndex, ref int totalRecord,string type);

        DataTable GetTaxOrgGrid(string phid);

        int SaveTaxOrg(DataTable dt);

        int SaveFunData(DataTable dt);

        DataTable GetOptionValue(Int64 phid);

        string GetOptionDetail(string option_group, string option_code);

        Dictionary<string, string> GetArgumentDic(string option_group, string option_code, string[] keys);

        string GetSingleArgument(string option_group, string option_code, string key);

        int SaveDetailData(DataTable dt,string logid);

        int SaveInitSetting();

        int GetInitSetting();

        DataTable GetArgumentByPhid(string detailPhid);

        string GetValueByKey(string option_group, string option_code, string key);

        IList<TreeJSONBase> LoadOrgTree(string detailPhid);
    }
}
