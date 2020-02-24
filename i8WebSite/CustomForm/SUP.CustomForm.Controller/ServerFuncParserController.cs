using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using SUP.Common.Base;
using SUP.CustomForm.Facade;
using SUP.CustomForm.Facade.Interface;
using System.Web.Mvc;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SUP.CustomForm.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ServerFuncParserController : AFController
    {
        private IServerFuncParserFacade Fac;

        public ServerFuncParserController()
        {
            Fac = AopObjectProxy.GetObject<IServerFuncParserFacade>(new ServerFuncParserFacade());
        }

        /// <summary>
        /// 解析执行元数据函数
        /// </summary>
        /// <returns></returns>
        public string FuncParser()
        {
            var busname = System.Web.HttpContext.Current.Request.Params["busname"];
            var funcname = System.Web.HttpContext.Current.Request.Params["funcname"];
            var paramstr = System.Web.HttpContext.Current.Request.Params["paramstr"];
            var rtntype = System.Web.HttpContext.Current.Request.Params["rtntype"];

            if (busname.Length > 15)
            {
                busname = busname.Substring(busname.Length - 15); //取右边15个字符
            }            

            var resultStr = Fac.FuncParser(busname, funcname, paramstr, rtntype);

            return resultStr;

            //var sql = string.Format("select * from p_form_funcmetadata where busname='{0}' and funcname='{1}'", busname, funcname);
            //DataTable dt = Fac.GetDataTable(sql);

            //if( dt == null || dt.Rows.Count == 0 )
            //{
            //    return "{status : \"fail\", value:\"该功能号不存在\"}";
            //}

            ////功能类型(1：执行sql；2：执行dll中自定义函数)
            //string functype = dt.Rows[0]["functype"].ToString();

            //if (functype == "1")
            //{
            //    //例select compname from enterprise where compno=@aaa.compno and ocode=@ocode
            //    string sqlstr = dt.Rows[0]["sqlstr"].ToString();
            //    JObject jo = JsonConvert.DeserializeObject(paramstr) as JObject;

            //    foreach (var item in jo)
            //    {
            //        sqlstr = sqlstr.Replace("@" + item.Key.ToString(), item.Value.ToString());
            //    }

            //    DataTable sqlStrDt = Fac.GetDataTable(sqlstr);

            //    //此处返回DataTable的JSON串对象
            //    string json = DataConverterHelper.ToJson(sqlStrDt, sqlStrDt.Rows.Count);

            //    //return {totalRows:1,Record:[{"compname":"装饰一分公司"}]}
            //    return "{status : \"ok\", value:" + json + "}";
            //}
            //else
            //{
            //    return "{status : \"fail\", value:\"外部dll暂时不支持\"}";
            //}
        }
    }
}
