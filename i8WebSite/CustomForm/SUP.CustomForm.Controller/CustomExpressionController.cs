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
    public class CustomExpressionController : AFController
    {
        private IExpressionFacade Fac;

        public CustomExpressionController()
        {
            Fac = AopObjectProxy.GetObject<IExpressionFacade>(new ExpressionFacade());
        }

        /// <summary>
        /// 获取数据;
        /// </summary>
        /// <returns></returns>
        public string GetSqlValue()
        {
            var sqlStr = System.Web.HttpContext.Current.Request.Params["sql"];
            var sqlStrDt = System.Web.HttpContext.Current.Request.Params["sqldt"];

            if (!string.IsNullOrEmpty(sqlStrDt))
            {
                sqlStr = sqlStrDt;
            }

            DataTable dt = Fac.GetDataTable(sqlStr);

            if (!string.IsNullOrEmpty(sqlStrDt))
            {
                /*此处返回DataTable的JSON串对象;*/
                string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
                return json;
            }
            else
            {
                string value = null;
                if (dt.Rows.Count > 0)
                {
                    value = dt.Rows[0][0].ToString();
                }

                if (value == string.Empty)
                {
                    return "{status : \"fail\", value:\"" + string.Empty + "\"}";
                }

                return "{status : \"ok\", value:\"" + value + "\"}";
            }
        }

        /// <summary>
        /// 唯一性验证;
        /// </summary>
        /// <returns></returns>
        public string UniqueCheck()
        {
            // fields:"userdefine_1:g5879532;usedefine_2:5642312sda54|userdefine_1:g5879532;usedefine_2:5642312sda54";
            var tableName = System.Web.HttpContext.Current.Request.Params["tablename"]; // 表名;
            var fields = System.Web.HttpContext.Current.Request.Params["fields"]; // 字段;
            var together = System.Web.HttpContext.Current.Request.Params["together"]; // 单独验证或者组合验证;

            /*检查参数是否已传入;*/
            if (string.IsNullOrEmpty(tableName)) return "{status:\"fail\",msg:\"缺少tableName参数\"}";
            if (string.IsNullOrEmpty(fields)) return "{status:\"ok\"}";

            /*解析fields的内容;传入的可能是好几组需要验证的字段组，有'|'分隔开;*/
            string[] fieldsArrays = fields.Split('|');
            var connect = together == "1" ? "and" : "or"; // 连接词;

            //sql语句拼接 select count(*) from tablename where fields[0] = 'fields[0].Value' and|or fields[1] = 'fields[1].Value';
            for (int j = 0; j < fieldsArrays.Length; j++)
            {
                var fieldsArray = fieldsArrays[j].Split(',');
                var sqlStr = new StringBuilder();
                sqlStr.Append(string.Format("select count(*) from {0} where {1} = {2}",
                                            tableName,
                                            fieldsArray[0].Split(':')[0],
                                            fieldsArray[0].Split(':')[1]));
                for (int i = 1; i < fieldsArray.Length; i++)
                {
                    sqlStr.Append(string.Format(" {0} {1}={2}",
                                                connect,
                                                fieldsArray[i].Split(':')[0],
                                                fieldsArray[i].Split(':')[1]));
                }

                DataTable dt = Fac.GetDataTable(sqlStr.ToString());
                var res = dt.Rows[0][0].ToString();

                if (res != "0")
                {
                    return "{status:\"fail\",message:\"" + j + "\"}";
                }
            }

            return "{status:\"ok\"}";
        }

        /// <summary>
        /// 汇总校验;
        /// </summary>
        /// <returns></returns>
        public string ValidationComplex()
        {
            //select itemname from itemdata where itemno='xxxx';
            //select sum(textcol_1) from tablename where fields[0]==fields[1] and fields[0]==fields[2];

            var leftSqlStr = System.Web.HttpContext.Current.Request.Params["leftsql"]; //左值sql;
            var sum = System.Web.HttpContext.Current.Request.Params["sum"]; //需要汇总的值;
            var tablename = System.Web.HttpContext.Current.Request.Params["tablename"]; //表名;
            var where = System.Web.HttpContext.Current.Request.Params["where"]; // 查询条件;
            var opera = System.Web.HttpContext.Current.Request.Params["opera"]; // 运算符;

            var wheres = where.Replace("&", " and ");


            var rightSqlStr = new StringBuilder();
            rightSqlStr.Append(string.Format("select sum({0}) from {1} where {2}", sum.Split('.')[1], tablename, wheres));

            var isValidate = Fac.ValidationCheck(leftSqlStr, rightSqlStr.ToString(), opera);

            var status = isValidate ? "ok" : "fail";
            return "{status:\"" + status + "\"}";
        }

    }
}
