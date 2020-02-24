using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBK3.BK.UnitTest
{
    public class JsonHelper
    {
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        public static T DesrializeJsonToObject<T>(string json) where T : class
        {
            //string s = "{\"KeyCodes\":[\"399181109000002\"],\"SaveRows\":1,\"AttachMsg\":\"\",\"Msg\":\"保存记录成功。\",\"Status\":\"success\",\"Data\":\"\"}";
            //json = s;
            JsonSerializer jsr = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = jsr.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }
    }
}
