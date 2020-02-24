using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Common
{
    /// <summary>
    /// 装配内容类
    /// </summary>
    public class InstallContext
    {

        private  Dictionary<string, string> kvs = new Dictionary<string, string>();


        public string Get(string index)
        {
            if (kvs.ContainsKey(index))
            {
                return kvs[index];
            }
            else
            {
                return string.Empty;
            }
        }
        public void Set(string index, string value)
        {
            kvs[index] = value;
        }



        //private static InstallContext instance = new InstallContext();

        //private InstallContext()
        //{

        //}

        //public static InstallContext GetInstance()
        //{ 
        //    return instance;
        //}

    }

}
