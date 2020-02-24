using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Parameter
{
    /// <summary>
    /// 系统可以提供的参数
    /// </summary>
    public class SystemParameter :IAddinParameter
    {
        //系统参数前缀
        public static readonly string PREFIX= "SYS"; 

        //全名称
        public string Name { set; get; }

        //.
        public string FirstPart { set; get; }

        //
        public string SecondPart { set; get; }
        public string ThirdPart { set; get; }

        public string[] GetValues()
        {
            throw new NotImplementedException();
        }

        public string GetValue(int index)
        {
            if("Ocode".Equals(ThirdPart,StringComparison.OrdinalIgnoreCase))
            {
                return NG3.AppInfoBase.OCode;
            }
            if ("LoginID".Equals(ThirdPart, StringComparison.OrdinalIgnoreCase))
            {
                return NG3.AppInfoBase.LoginID;
            }
            if ("OrgID".Equals(ThirdPart, StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToString(NG3.AppInfoBase.OrgID);
            }
            if ("OrgName".Equals(ThirdPart, StringComparison.OrdinalIgnoreCase))
            {
                return NG3.AppInfoBase.OrgName;
            }
            if ("UserID".Equals(ThirdPart, StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToString(NG3.AppInfoBase.UserID);
            }
            if ("UserName".Equals(ThirdPart, StringComparison.OrdinalIgnoreCase))
            {
                return NG3.AppInfoBase.UserName;
            }

            throw new AddinException("无法解析出系统参数["+ThirdPart+"]");


        }

        //判断是不是系统参数
        public static bool IsSystemParameter(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix)) return false;
            if (PREFIX.Equals(prefix, StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }
    }
}
