
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3.Addin.Core.Parameter;
using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core
{
    public static class AddinParameterUtils
    {


        /// <summary>
        /// 进行参数的变量替换
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public static string[] ReplaceWithParameterValue(UnResolvedText rawParam)
        {
            if (rawParam == null) return null;
            string replacedText = rawParam.RawText;

            List<string> ResolvedText = new List<string>();


            //不需要进行数据解析
            if (!HasUIParameter(replacedText))
            {
                ResolvedText.Add(replacedText);
                return ResolvedText.ToArray();
            }


            //解析出变量数组
            IAddinParameter[] parameters = GetPluginUIParameters(replacedText);

            if (parameters.Length < 1)
            {

                throw new AddinException("参数解析出错");
            }


            //UI参数合并
            //MergeUIParameter(ref parameters, rawParam);

            //是否是合法的文本
            IsValidText(parameters);

            int len = GetMaxRows(parameters);
            //行
            for (int i = 0; i < len; i++)
            {
                //列
                string text = replacedText;
                foreach (var parameter in parameters)
                {
                    string value = parameter.GetValue(i);
                    text = text.Replace(parameter.Name, value);
                }
                ResolvedText.Add(text);
            }

            return ResolvedText.ToArray();



        }

        /// <summary>
        ///  是否有空数据参数
        /// </summary>
        /// <returns></returns>
        public static bool HasEmptyDataUIParameter(string[] texts)
        {
            if (texts == null || texts.Length == 0) return false;
            string text = texts[0];
            if (text.IndexOf(UIParameter.NO_DATA) >= 0) return true;

            return false;
            
        }
        private static void GetUIParameterValues(IAddinParameter[] parameters)
        {
            foreach (var param in parameters)
            {
                if(param is UIParameter)
                {
                    param.GetValues();
                }
            }
        }
        /// <summary>
        /// 取参数取值的最大行数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static int GetMaxRows(IAddinParameter[] parameters)
        {
            GetUIParameterValues(parameters);
            return parameters.Max(p =>
            {
                int i = 0;
                if(p is UIParameter)
                {
                    i= p.GetValues().Length;
                }
                return i;
            }             
            );
        }

        private static bool IsValidText(IAddinParameter[] parameters)
        {
            string ds = string.Empty;
            string dstype = string.Empty;

            GetUIParameterValues(parameters);
           
            //判断是否是存在不同的数据源，不同的数据源只允许多个form类型的数据源，和一个table类型的数据源
            foreach (var param in parameters)
            {
                if(param is UIParameter)
                {
                    //UI数据源
                    var p = (UIParameter)param;
                    
                    if(p.RootType == JsonParser.TABLE_ROOT)
                    {
                        if(!string.IsNullOrEmpty(ds))
                        {
                           if(ds!=p.FirstPart)
                            {
                               
                                throw new AddinException("不支持同时具有多行结果集的两个数据源!");
                            } 
                           else
                            {
                                if(dstype!=p.SecondPart)
                                {
                                    
                                    throw new AddinException("不支持同个数据源有不同的数据类型的数据!");
                                }
                            }

                        }else
                        {
                            ds = p.FirstPart;
                            dstype = p.SecondPart;
                        }                       
                    }
                    
                }

            }         
            return true;
        }

        

        /// <summary>
        ///UI参数合并
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="rawParam"></param>
        private static void MergeUIParameter(ref IAddinParameter[] parameters, UnResolvedText rawParam)
        {
           
            foreach (var param in parameters)
            {
                if(param is UIParameter)
                {
                    //第一个参数
                    if (string.IsNullOrEmpty(param.FirstPart))
                    {
                        if(!string.IsNullOrWhiteSpace(rawParam.RequestParam))
                        {
                            //不为空则进行参数填充
                            param.FirstPart = rawParam.RequestParam;
                        }                       
                    }
                    //第二个参数                       
                    if (string.IsNullOrEmpty(param.SecondPart))
                    {
                        param.SecondPart = rawParam.RowsType.ToString();                                                                                                                                                                                                     
                    }                    
                }
            }
        }

        /// <summary>
        /// 从请求中获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        



        private static bool HasUIParameter(string text)
        {
            int iPos = 0;

            //只要SQL语句存在@则认为存在参数
            iPos = text.IndexOf("@");
            if (iPos < 0) return false;

            return true;
        }

       

        /// <summary>
        /// 取得UIParameter数组
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IAddinParameter[] GetPluginUIParameters(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            List<IAddinParameter> parameters = new List<IAddinParameter>();

            int iPos = 0;
            int iPos2 = 0;
            //替换的变量只支持数字与字母,点与下划线
            
            iPos = text.IndexOf("@");
            while (iPos >= 0)
            {
                //取
                char[] chs;
                iPos2 = iPos;
                do
                {
                    iPos2 = iPos2 + 1;
                    if (iPos2 >= text.Length) break;
                    string val = text.Substring(iPos2, 1);
                    chs = val.ToCharArray();                    

                } while (char.IsLetterOrDigit(chs[0]) || chs[0] == '.' || chs[0] == '_');


                string parameter = text.Substring(iPos + 1, iPos2 - iPos - 1);

                UIParameter p = new UIParameter();

                p.Name = "@" + parameter; //参数全名

                //UI参数的格式要么是@mstformdata.NewRow.cntno 三段式,要么是就是一段式，二段式都有可能
                //一段式：@Phid
                //二段式：@mstformdata.phid 表示从mstformdata的所有行中找出phid 

                int iPos3 = 0;
                iPos3 = parameter.IndexOf(".");
                if (iPos3 > 0)
                {
                    //三段式UI格式
                    p.FirstPart = parameter.Substring(0, iPos3);
                    string part = parameter.Substring(iPos3 + 1, parameter.Length - iPos3 - 1);
                    int iPos4 = 0;
                    iPos4 = part.IndexOf(".");
                    if (iPos4 > 0)
                    {
                        p.SecondPart = part.Substring(0, iPos4);
                        p.ThirdPart = part.Substring(iPos4 + 1, part.Length - iPos4 - 1);
                    }
                    else
                    {
                        //二段式，可以省略数据的类型,第二段为空
                        p.SecondPart = string.Empty;
                        p.ThirdPart = part;                     
                    }
                }
                else
                {
                    //一段式UI参数
                    p.FirstPart = string.Empty; //前缀为空
                    p.SecondPart = string.Empty; //来源的数据部分为空
                    p.ThirdPart = parameter;
                }

                //判断是何种参数
                if(BizParameter.IsBizParameter(p.FirstPart))
                {
                    //业务参数
                    BizParameter bp = new BizParameter();
                    bp.Name = p.Name;
                    bp.FirstPart = p.FirstPart;
                    bp.SecondPart = p.SecondPart;
                    bp.ThirdPart = p.ThirdPart;
                    parameters.Add(bp);

                }
                else if(SystemParameter.IsSystemParameter(p.FirstPart))
                {
                    //系统参数
                    SystemParameter sp = new SystemParameter();
                    sp.Name = p.Name;
                    sp.FirstPart = p.FirstPart;
                    sp.SecondPart = p.SecondPart;
                    sp.ThirdPart = p.ThirdPart;
                    parameters.Add(sp);

                }
                else
                {
                    //UI参数
                    //如果第一段不为空，第二段为空,则第二段设置为All;
                    if(!string.IsNullOrEmpty(p.FirstPart)&&string.IsNullOrEmpty(p.SecondPart))
                    {
                        p.SecondPart = EnumUIDataSourceType.All.ToString();
                    }
                    parameters.Add(p);
                }

                iPos = text.IndexOf("@", iPos + 1);
            }

            return parameters.ToArray();

        }



    }
}
