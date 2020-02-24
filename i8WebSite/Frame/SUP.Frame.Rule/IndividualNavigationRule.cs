using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using SUP.Common.Interface;
using NG3.Data.Service;

namespace SUP.Frame.Rule
{
    public class IndividualNavigationRule : IUserConfig
    {
        private IndividualNavigationDac dac = new IndividualNavigationDac();

        public DataTable LoadTree()
        {
            return dac.LoadTree();
        }

        public string Load(string text)
        {
            return dac.Load(text);
        }

        public string Save(string svgConfig, string text, string saveType)
        {
            long phid = 0;
            if (saveType == "add")
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_function_navigation", "phid");
            }
            return dac.Save(svgConfig, text, saveType, phid);
        }

        public bool Delete(string text)
        {
            return dac.Delete(text);
        }

        public bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            List<long> phid = null;
            string sql = String.Format(@"select count(*) from fg3_function_navigation where userid ={0} and usertype = {1}", fromUserId, fromUserType);
            string obj = DbHelper.GetString(sql).ToString();
            int count = 0;
            int.TryParse(obj, out count);
            if(count == 0)
            {
                return true;
            }
            phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_function_navigation", "phid", count);
            return dac.UserConfigCopy(fromUserId, fromUserType, toUserId, toUserType, phid);
        }

        public bool DeleteUserConfig(long userid, int usertype)
        {
            return dac.UserConfigDel(userid, usertype);
        }
    }
}
