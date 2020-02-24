using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NG3.Metadata.PBDesignerParse.DataWindow;

namespace NG3.Metadata.PBDesignerParse
{
    sealed class IniHelp
    {

        private const int MaxKeyValueSize = 100000;
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //通过winapi取值
        public static string ReadValueDirect(string section, string key, string fileName)
        {
            StringBuilder retBuilder = new StringBuilder(MaxKeyValueSize);
            int i = GetPrivateProfileString(section, key, string.Empty, retBuilder, MaxKeyValueSize, fileName);
            return retBuilder.ToString() == string.Empty ? "0" : retBuilder.ToString();
        }

        public static string ReadValue(string section, string key, string fileName)
        {
            try
            {
                //取语法串特殊处理，因为语法长度超过6万多的时候GetPrivateProfileString会无法正常读取，改用读行的方式
                if (key == DwRes.SyntaxSection)
                {
                    long syntaxlen = Convert.ToInt64(ReadValueDirect(section, DwRes.SyntaxLenSection, fileName));
                    if (syntaxlen > 60000)  //长度大于6万则用读行的方式取值，因为用winapi最多只能取长度是65000左右的值
                    {
                        StreamReader sr = new StreamReader(fileName, Encoding.Default);
                        string rtnStr = string.Empty;
                        string line;
                        bool firstFind = false;
                        int emptylinecount = 0;  //空行数目，避免ini中行之间有空白行就结束循环，暂定空白行连续出现行数为500时退出循环

                        while (emptylinecount < 500)
                        {
                            if (string.IsNullOrEmpty((line = sr.ReadLine())))
                            {
                                emptylinecount++;
                                continue;
                            }

                            emptylinecount = 0;  //出现非空行则重置空行数目

                            if (line.Substring(0, 1) == "[" && line.Substring(line.Length - 1, 1) == "]")
                            {
                                if (line == ("[" + section + "]"))
                                {
                                    firstFind = true;
                                }
                                else
                                {
                                    firstFind = false;
                                }
                            }

                            if (firstFind)
                            {
                                if (line.IndexOf(key) == 0)
                                {
                                    rtnStr = line.Substring(key.Length + 1);
                                    break;
                                }
                            }
                        }

                        return rtnStr;
                    }
                    else
                    {
                        return ReadValueDirect(section, key, fileName);
                    }
                }
                else
                {
                    return ReadValueDirect(section, key, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    
    }
}