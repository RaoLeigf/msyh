using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Enterprise3.WebApi.Client.Models;
using Enterprise3.Common.Base.Helpers;
using Enterprise3.WebApi.Client;
using Enterprise3.WebApi.Client.Enums;
using System.Reflection;

namespace GSP3.SP.UnitTest
{
    /// <summary>
    /// Test 的摘要说明
    /// </summary>
    [TestClass]
    public class Test
    {
        static AppInfoBase appInfo;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            appInfo = new AppInfoBase
            {
                AppKey = ConfigHelper.GetString("AppKey", "D31B7F91-3068-4A49-91EE-F3E13AE5C48C"), //必须
                AppSecret = ConfigHelper.GetString("AppSecret", "103CB639-840C-4E4F-8812-220ECE3C4E4D"), //必须
                DbName = "NG0001", //可不传，默认为默认账套
                UserId = 631181115000001,
                OrgId = 547181121000001,
                //DbServerName = "10.0.13.168",
                OCode = "",
                //OrgName = "组织名称",
                //SessionKey = "会话标识",
                //TokenKey = string.Empty,
                //UName = "帐套名称",
                //UserKey = "003",
                //UserName = "丰立新",                
            };
        }

        [TestMethod]
        public void test1() {
            ParameterCollection parameter = new ParameterCollection();

            WebApiClient client = new WebApiClient("http://127.0.0.1:8038/", appInfo, EnumDataFormat.Json);
            var res = client.Get("api/GSP/GAppvalRecord/GetUnDoRecordList", parameter);

            Console.WriteLine(res.Content);
        }

        [TestMethod]
        public void test2() {
            var data = new {
                name = "adsad",
                list = new List<string>()
            };

            Type type = data.GetType();

            foreach (PropertyInfo info in type.GetProperties()) {
                var value = info.GetValue(data);
            }
        }
    }
}
