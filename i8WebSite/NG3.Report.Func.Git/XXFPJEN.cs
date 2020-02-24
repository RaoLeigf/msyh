using NG3.Report.Func.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Interface;
using NG3.Report.Func.Core.Util;
using NG3.Report.Func.Git.Common;

namespace NG3.Report.Func.Git
{
    /// <summary>
    /// 销项销售额(内)
    /// </summary>
    public class XXFPJEN:GitFuncBase,IFuncResolve,IFuncTrack
    { 

        public FuncResolveResult Resolve()
        {
            return null;
        }

        public FuncTrackResult Track()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 两个函数只是SQL语句不一样的，可以考虑采用GetSqls这种形式
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string,string> GetSqls()
        {
            var whereDic = GetWhere();

            IDictionary<string, string> sqlDic = new Dictionary<string, string>();

            string sql1 = "select sum(coalesce(amount, 0)) from invo_bill where (invo_bill.invo_state <> '2' or  invo_bill.invo_state is null) and(invo_bill.split_flag = '' or invo_bill.split_flag is null or invo_bill.split_flag = '0') and invo_bill.inputtype = '1' and invo_bill.bill_type = '0' and " + whereDic["where1"];

            string sql2 = "select sum(coalesce(invo_split.amount,0)) from invo_bill,invo_split where (invo_bill.invo_state<>'2' or  invo_bill.invo_state is null) and invo_bill.split_flag='1' and invo_bill.id=invo_split.inv_id and invo_bill.inputtype='1' and invo_bill.bill_type='0' and " + whereDic["where1"];

            sqlDic.Add("sql1", sql1);
            sqlDic.Add("sql2", sql2);

            return sqlDic;


        }

        public override FuncCalcResult CalcFuncValue()
        {
            FuncCalcResult result;

            try
            {
                //var whereDic = GetWhere();
                //string sql1 = "select sum(coalesce(amount, 0)) from invo_bill where (invo_bill.invo_state <> '2' or  invo_bill.invo_state is null) and(invo_bill.split_flag = '' or invo_bill.split_flag is null or invo_bill.split_flag = '0') and invo_bill.inputtype = '1' and invo_bill.bill_type = '0' and " + whereDic["where1"];

                //string sql2 = "select sum(coalesce(invo_split.amount,0)) from invo_bill,invo_split where (invo_bill.invo_state<>'2' or  invo_bill.invo_state is null) and invo_bill.split_flag='1' and invo_bill.id=invo_split.inv_id and invo_bill.inputtype='1' and invo_bill.bill_type='0' and " + whereDic["where1"];

                var sqlDic = GetSqls();

                GitReportFuncDac funcDac = new GitReportFuncDac();

                var d_cost1 = funcDac.QueryForDecimal(sqlDic["sql1"]);
                var d_cost2 = funcDac.QueryForDecimal(sqlDic["sql2"]);

                result = new FuncCalcResult();
                result.Status = EnumFuncActionStatus.Success;
                result.Value = Convert.ToString(d_cost1 + d_cost2);
            }
            catch (Exception ex)
            {

                result = new FuncCalcResult();
                result.Status = EnumFuncActionStatus.Failure;
                result.Fault = new FuncFault { FaultCode = "-1",Faultstring="error", Detail=ex.Message };
            }
           
            return result;
        }

        //函数的预解析,如果不是直接返回函数的则需要重写
        public override FuncInfo PreHandle()
        {
            return base.PreHandle();
        }

        public override IDictionary<string,string> GetWhere()
        {
            //参数化的思路

            IDictionary<string, string> whereDic = new Dictionary<string, string>();

            //转换成需要的数组,也可以直接处理
            FuncParameter[] funcParams = Func.Paras.ToArray();

            if (funcParams.Length < 8) throw new FuncException("函数的参数小于指定的数量8");
            
            //注意PB的数组下标是从1开始，  

            //当前公式计算的上下文
            var context = FuncCache.GetContext();

            int[] accpers = GitCommonHelper.GetAccperFromArea(funcParams[2].Value);


            StringBuilder where1 = new StringBuilder();
            StringBuilder where2 = new StringBuilder();
            where1.Append("1=1");
            where2.Append("1=1");


            //判断会计期
            if (accpers.Length <1)
            {
                //不是会计区间
                string s_mon = funcParams[1].Value;
                if(!string.IsNullOrEmpty(s_mon))
                {
                    where1.Append(" and  invo_bill.mon = " + GitCommonHelper.ToValue(s_mon));
                    where2.Append(" and  invo_bill.mon = " + GitCommonHelper.ToValue(s_mon));
                }
            }
            else
            {
                string s_mon = accpers.Last().ToString();
                if(!string.IsNullOrEmpty(s_mon))
                {
                    where1.Append(" and  invo_bill.mon = " + GitCommonHelper.ToArray(accpers));
                    where2.Append(" and  invo_bill.mon = " + GitCommonHelper.ToArray(accpers));
                }
            }

            //年度的判断
            string s_year = funcParams[0].Value;
            if(string.IsNullOrEmpty(s_year))
            {
                s_year = GitCommonHelper.GetCurrentYear();
            }

            where1.Append(" and invo_bill.uyear = " + GitCommonHelper.ToValue(s_year));
            where2.Append(" and invo_bill.uyear = " + GitCommonHelper.ToValue(s_year));

            decimal d_invo_rate = Convert.ToDecimal(funcParams[2].Value);
            if (d_invo_rate!=0)
            {
                where1.Append(" and invo_bill.invo_rate = " + GitCommonHelper.ToValue(d_invo_rate));
                where2.Append(" and invo_split.invo_rate = " + GitCommonHelper.ToValue(d_invo_rate));
            }
    

            string s_imposecode = funcParams[3].Value;

            if (!string.IsNullOrEmpty(s_imposecode))
            {
                where1.Append(" and invo_bill.imposecode = " + GitCommonHelper.ToValue(s_imposecode));
                where2.Append(" and invo_split.imposecodedet = " + GitCommonHelper.ToValue(s_imposecode));
            }


            string s_invo_type = funcParams[4].Value;
            if (!string.IsNullOrEmpty(s_invo_type))
            {
                where1.Append(" and invo_bill.invo_type  = " + GitCommonHelper.ToValue(s_invo_type));
                where2.Append(" and invo_bill.invo_type  = " + GitCommonHelper.ToValue(s_invo_type));
            }


            string s_taxes_type = funcParams[5].Value;
            if( !string.IsNullOrEmpty(s_taxes_type) )
            {
                where1.Append(" and invo_bill.taxes_type  = " + GitCommonHelper.ToValue(s_taxes_type));
                where2.Append(" and invo_bill.taxes_type  = " + GitCommonHelper.ToValue(s_taxes_type));
            }

            string s_own_compcode = funcParams[6].Value;
            if (!string.IsNullOrEmpty(s_own_compcode))
            {
                where1.Append(" and invo_bill.own_compcode  = " + GitCommonHelper.ToValue(s_own_compcode));
                where2.Append(" and invo_bill.own_compcode  = " + GitCommonHelper.ToValue(s_own_compcode));
            }


            string s_invo_proj = funcParams[7].Value;
            if (!string.IsNullOrEmpty(s_invo_proj))
            {
                where1.Append(" and invo_bill.invo_proj  = " + GitCommonHelper.ToValue(s_invo_proj));
                where2.Append(" and invo_bill.invo_proj  = " + GitCommonHelper.ToValue(s_invo_proj));
            }


            whereDic.Add("where1", where1.ToString());
            whereDic.Add("where2",where2.ToString());
            return whereDic;
        }
    }
}
