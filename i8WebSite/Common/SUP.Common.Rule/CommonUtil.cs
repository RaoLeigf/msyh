//using Enterprise3.NHORM.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enterprise3.Common.Model;
namespace SUP.Common.Rule
{
    public class CommonUtil
    {
        public static Int64 GetBillId(string tablename, string key)
        {
            try
            {
                var billNoCommon = new Enterprise3.WebApi.SDK.Common.BillNoHelper(); //new BillNoCommon();
                return billNoCommon.GetBillId(tablename, key);
            }
            catch (Exception ex)
            {
                throw new Exception("获取编码失败：" + ex.Message);
            }
           
        }

        public static List<long> GetBillId(string tablename, string key, int needCount = 1)
        {
            try
            {
                var billNoCommon = new Enterprise3.WebApi.SDK.Common.BillNoHelper();
                ResBillNoOrIdEntity en = billNoCommon.GetBillId(tablename, key, needCount);
                return en.BillIdList.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("获取编码失败：" + ex.Message);
            }
          
        }
        public static Int64 GetPhId(string tablename)
        {            
            return GetBillId(tablename, "phid");
        }


        public static ResBillNoOrIdEntity GetBillNo(string billNoReqType)
        {
            try
            {
                var billNoHelper = new Enterprise3.WebApi.SDK.Common.BillNoHelper(billNoReqType);
                return billNoHelper.GetBillNo();
            }
            catch (Exception ex)
            {
                throw new Exception("获取单据编码规则编码失败：" + ex.Message);
            }
        }

        public static ResBillNoOrIdEntity GetBillNoIntensive(string billNoReqType)
        {
            try
            {
                var billNoHelper = new Enterprise3.WebApi.SDK.Common.BillNoHelper(billNoReqType);
                return billNoHelper.GetBillNo(true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取单据编码规则编码失败：" + ex.Message);
            }
        }

        public static void CommitBillNo(string billNoReqType, ResBillNoOrIdEntity entity)
        {
            try
            {
                var billNoHelper = new Enterprise3.WebApi.SDK.Common.BillNoHelper(billNoReqType);
                billNoHelper.CommitBillNo(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("提交单据用户编码失败：" + ex.Message);
            }
        }

        public static void RollbackBillNo(string billNoReqType, ResBillNoOrIdEntity entity)
        {
            try
            {
                var billNoHelper = new Enterprise3.WebApi.SDK.Common.BillNoHelper(billNoReqType);
                billNoHelper.RollbackBillNo(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("回滚单据用户编码失败：" + ex.Message);
            }
        }

        public static Int64 GetMaxId(string tname)
        {
            string count = NG3.Data.Service.DbHelper.GetString("select max(phid) from " + tname);
            if (string.IsNullOrWhiteSpace(count))
            {
                return 0;
            }

            Int64 i = Convert.ToInt64(count);
            return i + 1;
        }


    }


}
