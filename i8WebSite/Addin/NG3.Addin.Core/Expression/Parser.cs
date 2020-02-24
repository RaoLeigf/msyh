using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    public class Parser
    {
       
        private List<IExpression> postExpression = new List<IExpression>(); //后缀表达式
        private Stack<IExpression> opStack = new Stack<IExpression>(); //堆栈


        //解析表达式，返回后缀表达式
        private bool Parse(string text)
        {
            string expression = text.Trim();

            string literal = string.Empty; //变量/常数/函数名等
            IExpression op = null; //操作符
            bool isStringToken = false;

            int pos = 0;
            while (pos < expression.Length)
            {
                isStringToken = false;
                string token = GetToken(expression, ref pos, ref isStringToken);

                if (RegsitryManager.ISOperator(token))
                {
                    InsertPostfixExpression(new OperatorExprssion(token));
                }
                else if (RegsitryManager.IsFunction(token))
                {
                    //函数其实是一种特殊的运算符
                    InsertPostfixExpression(new FuncExpression(token));
                }
                else
                {
                    //值
                    postExpression.Add(new VariableExpression(token, isStringToken));
                }
            }            
            //AddPostExpressionLiteral();
            //最后stack中的所有数据弹出加入到对应中
            while (opStack.Count > 0)
            {
                op = opStack.Pop();
                if (op.Identity == RegsitryManager.TOKEN_LEFT)
                {
                    //括号不匹配
                    throw new AddinException(text + "Expression invalid!");
                }
                else
                {
                    postExpression.Add(op);

                }


            }
            //去掉逗号运算符
            postExpression.RemoveAll(item =>item.Identity == RegsitryManager.TOKEN_COMMA);
            return true;
        }

        /// <summary>
        /// 计算返回的数值
        /// </summary>
        /// <returns></returns>
        public object Evaluate(string text)
        {
            
            Stack<Operand> operandStack = new Stack<Operand>();

            //解析
            Parse(text);

            foreach (var item in postExpression)
            {
                //获取参数的个数
                int dimension = RegsitryManager.GetDimension(item);
                Operand[] operands = null;
                if (dimension > 0)
                {
                    operands = new Operand[dimension];
                    for (int i = 0; i < dimension; i++)
                    {


                        operands[i] = operandStack.Pop();
                    }
                    //调整正确参数位置
                    Array.Reverse(operands);
                }
                //根据identity
                //对每个节点进行计算，返回操作数             
                operandStack.Push(new Operand(item.Eval(operands)));
            }

            //最后面的值
            return operandStack.Pop().Value;  
        }

        /// <summary>
        /// 添加操作符到后缀表达式
        /// </summary>
        /// <param name="token"></param>
        private void InsertPostfixExpression(IExpression token)
        {
            if (opStack.Count < 1)
            {
                opStack.Push(token);
                return;
            }

            
            IExpression op = null;

            //如果是右括号则右括号到左括号之间的全部弹出到表达式中
            if (token.Identity == RegsitryManager.TOKEN_RIGHT)
            {
                //
                while (opStack.Count > 0)
                {
                    op = opStack.Pop();
                    if (op.Identity != RegsitryManager.TOKEN_LEFT)
                    {
                        postExpression.Add(op);
                    }
                    else
                    {
                        break;
                    }
                }
                return;
            }            

            op = opStack.Peek(); //取

            
         
            if (op.Identity == RegsitryManager.TOKEN_LEFT || Operator.CompareOperator(Operator.OpType(token.Identity), Operator.OpType(op.Identity)) > 0)
            {
                //堆栈中的操作符优化级低，则新加入的表达式则直接入堆栈
                opStack.Push(token);
            }
            else
            {
                //小于等于则直接输出到表达式,依次弹出直到stack中元素大于
                //当前比较的运算符

                op = opStack.Pop();
                postExpression.Add(op);
                while (opStack.Count > 0)
                {
                    op = opStack.Peek();
                    if (op.Identity == RegsitryManager.TOKEN_LEFT) break; //弹出到左括号为至
                    //当前的运算符的优先级大于stack中的优先级时,停止弹出
                    if (Operator.CompareOperator(Operator.OpType(token.Identity), Operator.OpType(op.Identity)) > 0) break;

                    op = opStack.Pop();
                    postExpression.Add(op);
                }

                opStack.Push(token);

            }
        }



        /// <summary>
        /// 返回token
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string GetToken(string exp, ref int pos,ref bool isStringToken)
        {
            string token = string.Empty;

            StringBuilder tokenBuffer = new StringBuilder();
            int pos2 = 0;
            string ch2 = string.Empty;
            bool isInString = false; //是否是在字符串内
           

            while (pos < exp.Length)
            {
                string ch = exp.Substring(pos, 1);
                if (string.IsNullOrWhiteSpace(ch))
                {
                    pos++;
                    continue;
                }
                #region switch
                switch (ch)
                {
                    case RegsitryManager.TOKEN_LEFT:
                        {
                            token = RegsitryManager.TOKEN_LEFT;
                            break;
                        }
                    case RegsitryManager.TOKEN_RIGHT:
                        {
                            token = RegsitryManager.TOKEN_RIGHT;
                            break;
                        }
                    case RegsitryManager.TOKEN_ADD: //"+"
                        {
                            token = RegsitryManager.TOKEN_ADD;
                            break;
                        }
                    case RegsitryManager.TOKEN_SUB:
                        {
                            if(!isInString)
                            {
                                //如果在字符串内的减号应该是连接符
                                token = RegsitryManager.TOKEN_SUB;
                            }
                            else
                            {
                                tokenBuffer.Append(ch);
                            }
                            
                            break;
                        }
                    case RegsitryManager.TOKEN_MUL:
                        {
                            if (!isInString)
                            {
                                token = RegsitryManager.TOKEN_MUL;
                            }
                            else
                            {
                                tokenBuffer.Append(ch);
                            }

                            break;
                        }
                    case RegsitryManager.TOKEN_DIV:
                        {
                            if (!isInString)
                            {
                                //如果在字符串内的减号应该是连接符
                                token = RegsitryManager.TOKEN_DIV;
                            }
                            else
                            {
                                tokenBuffer.Append(ch);
                            }
                            break;
                        }
                    case RegsitryManager.TOKEN_COMMA: //逗号
                        {
                            if (!isInString)
                            {
                                //如果在字符串内的减号应该是连接符
                                token = RegsitryManager.TOKEN_COMMA;
                            }
                            else
                            {
                                tokenBuffer.Append(ch);
                            }
                            break;
                        }

                    case RegsitryManager.TOKEN_GT:
                        {
                            //大于的情况，主要是考虑大于等于
                            pos2 = pos;
                            pos2++;
                            ch = exp.Substring(pos2, 1);
                            if (ch == "=")
                            {
                                token = RegsitryManager.TOKEN_GE;
                            }
                            else
                            {
                                token = RegsitryManager.TOKEN_GT;
                            }                            

                            break;
                        }
                    case RegsitryManager.TOKEN_LT:
                        {
                            //要考虑小于，小于等于，不等于
                            pos2 = pos;
                            pos2++;
                            ch = exp.Substring(pos2, 1);
                            //排除空格
                            if (ch == "=")
                            {
                                token = RegsitryManager.TOKEN_LE;
                            }
                            else if (ch == ">")
                            {
                                token = RegsitryManager.TOKEN_UT;
                            }
                            else
                            {
                                token = RegsitryManager.TOKEN_LT;
                            }
                            
                            break;
                        }
                    default:
                        {
                            //还要判断 &&，||运算符，==运算符
                            if (ch == "&")
                            {
                                pos2 = pos;
                                pos2++;
                                ch2 = exp.Substring(pos2, 1);
                                if (ch2 == "&")
                                {
                                    token = RegsitryManager.TOKEN_AND;
                                }
                                else
                                {
                                    tokenBuffer.Append(ch);
                                    pos++;
                                }
                            }
                            else if (ch == "|")
                            {
                                pos2 = pos;
                                pos2++;
                                ch2 = exp.Substring(pos2, 1);
                                if (ch2 == "|")
                                {
                                    //确定是||字符串
                                    token = RegsitryManager.TOKEN_OR;
                                }
                                else
                                {
                                    tokenBuffer.Append(ch);                                    
                                }

                            }
                            else if (ch == "=")
                            {
                                pos2 = pos;
                                pos2++;
                                ch2 = exp.Substring(pos2, 1);
                                if (ch2 == "=")
                                {
                                    //确定是||字符串
                                    token = RegsitryManager.TOKEN_ET;
                                }
                                else
                                {
                                    //如果是在单引号当中，则表示的是字符串
                                    tokenBuffer.Append(ch);                                   
                                }

                            }                           
                            else
                            {
                                if(ch=="'")
                                {
                                    //单引号配对使用
                                    if (!isInString)
                                        isInString = true;
                                    else
                                        isInString = false;
                                    //返回的token是否是字符串
                                    isStringToken = true;

                                }
                                else
                                {
                                    tokenBuffer.Append(ch);
                                }
                                                                
                            }
                           
                            break;
                        }

                }
                pos++;
                #endregion

                //表示存在结束的TOKEN
                if (!string.IsNullOrEmpty(token))
                {
                    //在后面处理
                    if (tokenBuffer.Length > 0)
                    {
                        pos--; //回退一位
                        //存放的是变量，函数名，常量
                        return tokenBuffer.ToString();
                    }
                    else
                    {       
                        //存放的是运算符
                        pos = pos + (token.Length - 1); //运算符的位置指针向前一位，主要是解决长度大于1的运算符
                        return token;
                    }                    
                }
                //返回最后的值
                if(pos>= exp.Length)
                {
                    if (tokenBuffer.Length > 0)
                    {                        
                        //存放的是变量，函数名，常量
                        return tokenBuffer.ToString();
                    }
                }
               

            }
            return token;


        }
    }
}
