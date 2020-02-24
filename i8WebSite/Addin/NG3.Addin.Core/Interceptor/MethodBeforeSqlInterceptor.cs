using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Enums;

namespace NG3.Addin.Core.Interceptor
{
    /// <summary>
    /// 功能点进行SQL语句的扩展
    /// </summary>
    public class MethodBeforeSqlInterceptor : AbstractMethodBeforeInterceptor
    {
        public override bool Before(IMethodInvocation invocation)
        {
            try
            {
                bool rtn = false;
                foreach (var item in ConfigureEntity.SqlModels)
                {
                    //取主键信息?
                    //根据不同SQL类型，
                    if (item.SqlType == EnumSqlOpType.Sql)
                    {
                        rtn = SqlUtils.ExecuteUpdate(Session, item.SqlText);
                        //调用出错则记录日志
                    }
                    else if (item.SqlType == EnumSqlOpType.Sp)
                    {
                        //调用函数
                        rtn = SqlUtils.ExecuteSP(Session, item.SqlText);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
       
        
    }
}
