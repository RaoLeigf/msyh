using Enterprise3.WebApi.SDK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Parameter
{
    public class BizParameter :IAddinParameter
    {
        //
        public static readonly string PK = "PK"; //主键
        public static readonly string BPK = "BPK"; //业务主键
        public static readonly string[] BizPrefix = new string[] { PK, BPK };
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
            
            if(PK.Equals(FirstPart,StringComparison.OrdinalIgnoreCase))
            {
                //取相关的主键
                BillNoHelper billno = new BillNoHelper();
                if(!string.IsNullOrEmpty(SecondPart) && !string.IsNullOrEmpty(ThirdPart))
                {
                    //表名与列名
                    long phid = billno.GetBillId(SecondPart, ThirdPart);
                    return Convert.ToString(phid);
                }                                              
            }
            if (BPK.Equals(FirstPart, StringComparison.OrdinalIgnoreCase))
            {
                //取相关业务主键
                if(!string.IsNullOrEmpty(SecondPart))
                {
                    BillNoHelper billno = new BillNoHelper(SecondPart);
                    var id = billno.GetBillNo();
                    var obj = id.BillNoList;
                    if (obj != null && obj.Count > 0) return obj[0]; 

                }                                           
            }
            throw new AddinException("无法解析出业务参数["+Name+"]");
        }

        /// <summary>
        /// 判断是否是业务参数
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static bool IsBizParameter(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix)) return false;
            int index = Array.IndexOf(BizPrefix, prefix.ToUpper());
            if (index >= 0) return true;

            return false;
        }
    }
}
