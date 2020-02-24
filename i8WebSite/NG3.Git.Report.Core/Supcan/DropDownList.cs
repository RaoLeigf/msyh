using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Supcan
{
    public class DropDownList:IEquatable<DropDownList>
    {
        private string id;

        private string treelistUrl;

        private string dataUrl;

        private string displayCol;

        private string dataCol;

        //参数，supcan报表关于DropDownList的键值对,符合XML语法
        private string paras;

        private NameValueCollection paramsDic;
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string TreelistUrl
        {
            get
            {
                return treelistUrl;
            }

            set
            {
                treelistUrl = value;
            }
        }

        public string DataUrl
        {
            get
            {
                return dataUrl;
            }

            set
            {
                dataUrl = value;
            }
        }

        public string DisplayCol
        {
            get
            {
                return displayCol;
            }

            set
            {
                displayCol = value;
            }
        }

        public string DataCol
        {
            get
            {
                return dataCol;
            }

            set
            {
                dataCol = value;
            }
        }

        //一些参数的键值以，格式类似于
        //key1=value1|key2=value2
        public string Paras
        {
            get
            {
                return paras;
            }

            set
            {                
                paras = value;
                if (string.IsNullOrEmpty(paras)) return;
                //进行解析
                if(paras.Length >0)
                {
                    if(paramsDic ==null)
                    {
                        paramsDic = new NameValueCollection();
                    }
                    else
                    {
                        paramsDic.Clear();
                    }
                    string[] ps = paras.Split(new char[] { '|' });
                    foreach (var p in ps)
                    {
                        int pos = p.IndexOf("=");
                        if (pos < 1) continue;
                        string[] kv = p.Split(new char[] { '=' });
                        if(kv.Length ==2)
                        {                            
                            paramsDic.Add(kv[0], kv[1]);
                        }                        
                    }
                }
            }
        }

        public NameValueCollection ParamsDic
        {
            get
            {
                return paramsDic;
            }
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        //主要是为了去重复的下拉列表信息
        public bool Equals(DropDownList other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id,StringComparison.OrdinalIgnoreCase));
        }
    }
}
