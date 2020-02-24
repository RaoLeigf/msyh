using AopAlliance.Intercept;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Core.Expression;
using NG3.Addin.Core.Parameter;
using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;
using NG3.Addin.Model.Enums;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core
{
    public class ExpressionUtils
    {

        /// <summary>
        /// 当前系统支持的函数
        /// </summary>
        /// <returns></returns>
        public static IList<SupportFunctionBizModel> GetSupportFunctions()
        {
            return RegsitryManager.SupportFuncs.ToList<SupportFunctionBizModel>();
        }


        public static bool Eval(ISession session, IMethodInvocation invocation, IList<AddinExpressionModel> exps, IList<AddinExpressionVarModel> expVars)
        {

            if (expVars == null) return false;
            if (exps == null) return false;

            bool rtn = false;

            //表达式变量计算
            IDictionary<string, string> expvarDic = EvalExpVars(session, expVars);
           
            Parser parser = new Parser(); //表达式解析
                                          //进行表达式的循环
            foreach (var item in exps)
            {
                if (string.IsNullOrEmpty(item.ExpText)) continue;
              
                string text = item.ExpText.ToLower();
                //先进行表达式变量替换
                foreach (var dickey in expvarDic.Keys)
                {                    
                    text = text.Replace(dickey, expvarDic[dickey]);
                }
                LogHelper<ExpressionUtils>.Info("当前执行的未替换变量的表达式为：" + text);

                UnResolvedText expressionText = new UnResolvedText { RawText = text, RowsType = EnumUIDataSourceType.All };

                //生成表达式
                string[] expressions = AddinParameterUtils.ReplaceWithParameterValue(expressionText);

                if (AddinParameterUtils.HasEmptyDataUIParameter(expressions))
                {
                    LogHelper<ExpressionUtils>.Info("表达式有空的UI参数：" + text);
                    continue;
                }

                //生成表达式消息提示字符串
                string[] msgs = GetPromptMsgs(expvarDic, item);

                if(!IsMatchMsg(expressions, msgs))
                {
                    throw new AddinException("解析后表达式数量为[" + expressions.Length + "]与提示信息的数量[" + msgs.Length + "]不匹配");
                }

                for (int i = 0; i < expressions.Length; i++)
                {
                    string expression = expressions[i];
                    LogHelper<ExpressionUtils>.Info("已替换变量参数的表达式：" + expression);
                   
                    //计算表达式
                    var expValue = parser.Evaluate(expression);
                    rtn = expValue == null ? false : Convert.ToBoolean(expValue);                                
                    if (!rtn)
                    {
                        var msg = GetMsg(msgs, i);
                        throw new AddinException(msg);                        
                    }
                }
            }
            //进行UI变量替换           

            return rtn;

        }

        #region 私有方法

        /// <summary>
        /// 计算表达式变量
        /// </summary>
        /// <returns></returns>
        private static IDictionary<string,string> EvalExpVars(ISession session, IList<AddinExpressionVarModel> expVars)
        {
            string sqltext = string.Empty;
            object value = null;
            Dictionary<string, string> expvarDic = new Dictionary<string, string>();

            foreach (var item in expVars)
            {
                LogHelper<ExpressionUtils>.Info("当前解析的表达式变量为：" + item.SqlText);

                UnResolvedText rawText = new UnResolvedText { RequestParam = item.FromDs, RowsType = item.RowsType, RawText = item.SqlText };

                //解析完成的sql
                string[] sqls = AddinParameterUtils.ReplaceWithParameterValue(rawText);
                if (sqls.Length > 1)
                {

                    throw new AddinException("表达式变量不支持多值");
                }

                sqltext = sqls[0];

                if(AddinParameterUtils.HasEmptyDataUIParameter(sqls))
                {
                    expvarDic.Add(item.VarName.ToLower(), UIParameter.NO_DATA);
                    LogHelper<ExpressionUtils>.Info(item.VarName.ToLower() +" : "+ UIParameter.NO_DATA);
                    continue;
                }

                if (item.SqlOpType == EnumSqlOpType.Sql)
                {
                    //SQL语句
                    value = SqlUtils.ExecuteScalar(session, sqltext);

                }
                else if (item.SqlOpType == EnumSqlOpType.Func)
                {
                    //FUNC
                    value = SqlUtils.ExecuteFunc(session, sqltext);
                }

                if (value != null)
                {
                    //进行变量替换
                    expvarDic.Add(item.VarName.ToLower(), value.ToString()); //key是小写
                }
                else
                {
                    expvarDic.Add(item.VarName.ToLower(), "null");
                    LogHelper<ExpressionUtils>.Info(item.VarName.ToLower() + "表达式变量的值为：null");
                }

                

            }
            return expvarDic;

        }


        /// <summary>
        /// 生成解析完成的提示信息
        /// </summary>
        /// <param name="expvarDic"></param>
        /// <param name="item"></param>
        /// <param name="expLength"></param>
        /// <returns></returns>
        private static string[] GetPromptMsgs(IDictionary<string, string> expvarDic, AddinExpressionModel item)
        {
            string text = string.Empty;

            if (string.IsNullOrEmpty(item.Msg)) return null;

                //提示信息变量替换
            text = item.Msg.ToLower();
            foreach (var dickey in expvarDic.Keys)
            {
                text = text.Replace(dickey, expvarDic[dickey]);

            }
            UnResolvedText msgText = new UnResolvedText { RawText = text,RowsType = EnumUIDataSourceType.All};

            string[] msgs = AddinParameterUtils.ReplaceWithParameterValue(msgText);

            if(AddinParameterUtils.HasEmptyDataUIParameter(msgs))
            {
                //有空的UI变量
                return msgs;
            }

            var parsedMsgs = EvalMsgExpressions(msgs);
            //解析出提示信息中的表达式          
            return parsedMsgs;
        }


        

        /// <summary>
        /// 判断表达式数量与提示信息的数量是否一样
        /// </summary>
        /// <param name="exps"></param>
        /// <param name="expMsgs"></param>
        /// <returns></returns>
        private static bool IsMatchMsg(string[] exps,string[] expMsgs)
        {
            if (exps == null) return true;
            if (expMsgs == null) return true;

            if (expMsgs.Length > 1 && exps.Length != expMsgs.Length)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 计算出提示消息中的表达式
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        private static string[] EvalMsgExpressions(string[] msgs)
        {

            if (msgs == null || msgs.Length<1) return null;
            
            //在提示信息中不存在表达式则直接返回
            var exps = GetMsgExpressions(msgs[0]);
            if (exps == null) return msgs;
                     
            IList<string> MsgLists = new List<string>();

            Parser parser = new Parser();
            foreach (var m in msgs)
            {
                var msg = m;
                exps = GetMsgExpressions(msg);
 
                //存在表达式
                foreach (var exp in exps)
                {
                    //去提左右的<%,%>
                    var expText = exp.Substring(2, exp.Length - 4);
                    object obj = parser.Evaluate(expText);
                    if(obj==null)
                    {
                        throw new AddinException("消息中表达式["+exp+"]解析出错");
                    }
                    else
                    {
                        msg = msg.Replace(exp, Convert.ToString(obj));
                    }                    
                }
                MsgLists.Add(msg);
            }

            return MsgLists.ToArray() ;
        }
        
        /// <summary>
        /// 取得字符串的表达式
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static string[] GetMsgExpressions(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return null;
            //没有成对出现则不认为是表达式
            var pos = msg.IndexOf("<%");
            if (pos < 0) return null;
            var pos2 = msg.IndexOf("%>");
            if (pos2 < 0) return null;

            
            IList<string> exps = new List<string>();
            //解析
            while (pos >=0)
            {

                pos2 = msg.IndexOf("%>",pos);
                var exp = msg.Substring(pos, pos2 + 2 - pos);
                pos = msg.IndexOf("<%",pos2);

                exps.Add(exp);
            }

            return exps.ToArray() ;
        }


        /// <summary>
        /// 表达式验证不通过取提示消息
        /// </summary>
        /// <param name="msgs"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string GetMsg(string[] msgs,int index)
        {
            string msg;
            if (msgs != null)
            {
                //如果表达式验证为false则给出提示
                if (index >= msgs.Length)
                {
                    msg = msgs[0]; //只有一条提示信息的情况
                }
                else
                {
                    msg = msgs[index];
                }
                //提示信息行号
                msg = msg.Replace("#rowno#", Convert.ToString(index + 1));
                LogHelper<ExpressionUtils>.Info("表达式为假时的提示消息为：" + msg);                
            }
            else
            {
                msg = "表达式验证不通过";
            }

            return msg;
        }

        #endregion

    }
}
