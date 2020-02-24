using NG3.Addin.Core.Cfg;
using NG3.Addin.Core.Interceptor;
using NG3.Addin.Model.Domain.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Parameter
{
    public class ParameterManager
    {
       private static SupportVariableBizModel[] SystemVars =
       {
            new SupportVariableBizModel("@SYS.Ocode","字符串类型，当前登录的组织号"),
            new SupportVariableBizModel("@SYS.LoginID","字符串类型，当前登录的用户编码"),
            new SupportVariableBizModel("@SYS.OrgID", "BigInt类型，当前登录的组织ID"),
            new SupportVariableBizModel("@SYS.OrgName","字符串类型，当前登录的组织名称"),
            new SupportVariableBizModel("@SYS.UserID","BigInt类型，当前登录的用户ID"),
            new SupportVariableBizModel("@SYS.UserName","字符串类型，当前登录的用户名称")
        };

        private static SupportVariableBizModel[] bizVars =
        {
            new SupportVariableBizModel("@PK.t.c","返回值为BigInt,取表的主键,t:表示表名,c:表示列名"),
            new SupportVariableBizModel("@BPK.rule","取单据编码,rule是单据编码规则名称,返回是字符串类型"),
        };

        public static IList<SupportVariableBizModel> GetSystemVariables()
        {
            return SystemVars.ToList<SupportVariableBizModel>();
        }


        public static IList<SupportVariableBizModel> GetBizVariables()
        {
            return bizVars.ToList<SupportVariableBizModel>();
        }

    }
}
