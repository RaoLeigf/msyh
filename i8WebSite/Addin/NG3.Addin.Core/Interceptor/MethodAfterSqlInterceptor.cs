using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Enums;
using NG3.Addin.Core.Parameter;
using System.Runtime.Remoting.Messaging;

namespace NG3.Addin.Core.Interceptor
{
    public class MethodAfterSqlInterceptor : AbstractMethodAfterInterceptor
    {
        public override bool After(object returnObject, IMethodInvocation invocation)
        {
            try
            {
                bool rtn = false;
                foreach (var item in ConfigureEntity.SqlModels)
                {
                    UnResolvedText rawText = new UnResolvedText();
                    //rawText.RequestParam = item.FromDs;
                    //rawText.RowsType = item.RowsType;
                    rawText.RawText = item.SqlText;

                    
                    //解析完成的sql,Sql语句支持多值
                    string[] sqls = AddinParameterUtils.ReplaceWithParameterValue(rawText);

                    //判断是否有空的UI变量
                    if(AddinParameterUtils.HasEmptyDataUIParameter(sqls))
                    {
                        LogHelper<MethodAfterSqlInterceptor>.Info("SQL语句有空的UI变量：" + sqls[0]);
                        continue;

                    }

                    LogHelper<MethodAfterSqlInterceptor>.Info("解析后的SQL语句记录数为：" + sqls.Length);


                    foreach (var sql in sqls)
                    {
                        LogHelper<MethodAfterSqlInterceptor>.Info("生成的SQL语句：" + sql);
                        //根据不同SQL类型，
                        if (item.SqlType == EnumSqlOpType.Sql)
                        {
                            rtn = SqlUtils.ExecuteUpdate(Session, sql);
                            //调用出错则记录日志
                        }
                        else if (item.SqlType == EnumSqlOpType.Sp)
                        {
                            //调用函数
                            rtn = SqlUtils.ExecuteSP(Session, sql);
                        }
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
