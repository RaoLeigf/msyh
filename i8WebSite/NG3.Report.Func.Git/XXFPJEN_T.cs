using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Report.Func.Core.Entity;

namespace NG3.Report.Func.Git
{
    /// <summary>
    /// 销项销售额(内)-含税
    /// </summary>
    public class XXFPJEN_T :XXFPJEN
    {
        public override FuncInfo PreHandle()
        {
            var funcInofParams = Func.Paras.ToArray();

            return base.PreHandle();
        }


        public override IDictionary<string, string> GetSqls()
        {
            var whereDic = GetWhere();

            IDictionary<string, string> sqlDic = new Dictionary<string, string>();

            string sql1 = "select sum(coalesce(taxes, 0)) from invo_bill where (invo_bill.invo_state <> '2' or  invo_bill.invo_state is null) and(invo_bill.split_flag = '' or invo_bill.split_flag is null or invo_bill.split_flag = '0') and invo_bill.inputtype = '1' and invo_bill.bill_type = '0' and " + whereDic["where1"];

            string sql2 = "select sum(coalesce(invo_split.taxes,0)) from invo_bill,invo_split where (invo_bill.invo_state<>'2' or  invo_bill.invo_state is null) and invo_bill.split_flag='1' and invo_bill.id=invo_split.inv_id and invo_bill.inputtype='1' and invo_bill.bill_type='0' and " + whereDic["where2"];

            sqlDic.Add("sql1", sql1);
            sqlDic.Add("sql2", sql2);

            return sqlDic;
        }

        /// <summary>
        /// 如果只是SQL语句不同，则不需要重载
        /// </summary>
        /// <returns></returns>
        //public override FuncCalcResult CalcFuncValue()
        //{


        //    FuncCalcResult result = new FuncCalcResult();
        //    result.Status = EnumFuncActionStatus.Success;
        //    result.Value = "28.50";
        //    return result;
        //}

        /// <summary>
        /// 不重写where条件则不需要override
        /// </summary>
        /// <returns></returns>
        //public override IDictionary<string,string> GetWhere()
        //{
        //    return base.GetWhere();
        //}
    }
}
