
using NG3.Addin.Core.Cfg;
using NG3.Addin.Core.Parameter;
using NG3.Addin.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Extend
{
    public class ExtendSqlAction : AbstractExtendAction
    {
        public override string Execute()
        {
            try
            {
                string rtn = string.Empty;
                foreach (var item in ConfigureEntity.SqlModels)
                {

                    UnResolvedText rawText = new UnResolvedText();
                    rawText.RequestParam = item.FromDs;
                    rawText.RowsType = item.RowsType;
                    rawText.RawText = item.SqlText;

                    LogHelper<ExtendSqlAction>.Info("当前未解析参数的功能扩展SQL:" + item.SqlText);

                    //解析完成的sql,Sql语句支持多值
                    string[] sqls = AddinParameterUtils.ReplaceWithParameterValue(rawText);
                    

                    if (sqls.Length > 1) throw new AddinException(item.SqlText+"功能扩展的SQL语句不支持根据参数展开成多条SQL语句");

                    string sql = sqls[0];
                    LogHelper<ExtendSqlAction>.Info("已解析完成的功能扩展的SQL" + sql);

                    //包含有空的UI参数则不处理
                    if (AddinParameterUtils.HasEmptyDataUIParameter(sqls))
                    {
                        LogHelper<ExtendSqlAction>.Info("SQL语句有空的UI参数，SQL语句为：" + sql);
                        continue;
                    }
                    
                    //取主键信息?
                    //根据不同SQL类型，
                    if (item.SqlType == EnumSqlOpType.Sql)
                    {
                        rtn = SqlUtils.Execute(Session, sql, 0, 20);
                        //调用出错则记录日志
                    }
                    else if (item.SqlType == EnumSqlOpType.Func)
                    {
                        //调用函数
                        object brtn = SqlUtils.ExecuteFunc(Session, sql);
                        if(brtn!=null)
                        {
                            rtn = Convert.ToString(brtn);
                        }
                        
                    }
                }
                
                return rtn;
            }
            catch (Exception)
            {

                throw;
            }
                
            
            
        }
        
    }
}
