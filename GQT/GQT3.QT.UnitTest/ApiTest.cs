using System;
using System.Diagnostics;
using Enterprise3.Common.Base.Helpers;
using Enterprise3.Common.Model;
using Enterprise3.WebApi.Client;
using Enterprise3.WebApi.Client.Enums;
using Enterprise3.WebApi.Client.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;

namespace GQT3.QT.UnitTest
{
    [TestClass]
    public class ApiTest
    {
        static AppInfoBase appInfo;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //client = new RestClient();
            //mockServer = MockRestServiceServer.CreateServer(client.RestTemplate);

            //responseHeaders = new HttpHeaders();
            //responseHeaders.ContentType = new MediaType("application", "json");

            appInfo = new AppInfoBase
            {
                AppKey = ConfigHelper.GetString("AppKey", "D31B7F91-3068-4A49-91EE-F3E13AE5C48C"), //必须
                AppSecret = ConfigHelper.GetString("AppSecret", "103CB639-840C-4E4F-8812-220ECE3C4E4D"), //必须
                DbName = "NG0001", //可不传，默认为默认账套
                UserId = 521180820000001,
                OrgId = 521180820000002,
                //DbServerName = "10.0.15.106",
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
        public void GetSysOrganizeList()
        {

            String json = "{}";

            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Restart();

            WebApiClient client = new WebApiClient("http://127.0.0.1:8028/", appInfo, EnumDataFormat.Json);

            //var res = client.Post("api/GCW/SysOrganize/GetSysOrganizeList", json);

            ParameterCollection paras = new ParameterCollection();
            paras.Add("uid", "0001");
            paras.Add("orgid", "521180820000002");
            paras.Add("pagesize", "5");
            paras.Add("pageindex", "1");
            var res = client.Get("api/GCW/SysOrganize/GetSysOrganizeList", paras);

            _stopwatch.Stop();

            Console.WriteLine($"执行时间:{_stopwatch.ElapsedMilliseconds}");

            System.Console.WriteLine(res.Content);
            Assert.IsFalse(res.IsError, res.ErrMsg);
        }


        [TestMethod]
        public void GetSysUserList()
        {

            String json = "{}";
            ParameterCollection paras = new ParameterCollection();
            paras.Add("uid", "0001");
            paras.Add("orgid", "521180820000002");
            paras.Add("pagesize", "5");
            paras.Add("pageindex", "1");

            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Restart();

            WebApiClient client = new WebApiClient("http://127.0.0.1:8028/", appInfo, EnumDataFormat.Json);

            //var res = client.Post("api/GCW/SysOrganize/GetSysOrganizeList", json);                       
            var res = client.Get("api/GCW/SysUser/GetSysUserList", paras);

            _stopwatch.Stop();

            Console.WriteLine($"执行时间:{_stopwatch.ElapsedMilliseconds}");

            System.Console.WriteLine(res.Content);
            Assert.IsFalse(res.IsError, res.ErrMsg);
        }


        public class Rootobject
        {
            public int uid { get; set; }
            public int orgid { get; set; }
            public Infodata infodata { get; set; }
        }

        public class Infodata
        {
            public string Account { get; set; }
            public string Password { get; set; }
            public string RealName { get; set; }
            public string NickName { get; set; }
            public string HeadIcon { get; set; }
            public string Gender { get; set; }
            public string MobilePhone { get; set; }
            public string Email { get; set; }
            public string Description { get; set; }
        }


        [TestMethod]
        public void PostAdd()
        {
            string json = "{\"uid\":\"0\",\"orgid\":\"0\", \"infoData\":{\"Account\":\"LiMing20181108\",\"Password\":\"123456\",\"RealName\":\"哈哈\",\"NickName\":\"往事随风2\",\"HeadIcon\":\"\",\"Gender\":\"1\",\"MobilePhone\":\"110\",\"Email\":\"457775122@qq.com\",\"Description\":\"20181107注册用的测试用户\"}}";

            var infoData = new Infodata
            {
                Account= "LiMing20181108",
                Password="123456",
                RealName="个广告哈哈哈",
                NickName="hh",
                HeadIcon="",
                Gender="1",
                MobilePhone="1233423233",
                Email="sss@gg.com",
                Description = "单元测试新增用户"
            };

            var root = new Rootobject
            {
                uid = 0,
                orgid = 0,
                infodata = infoData
            };

            //string json = JsonConvert.SerializeObject(root);

            //HttpContent content = new StringContent(json);
            //content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            //var httpClient = new HttpClient();
           


            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Restart();

            //var response = httpClient.PostAsync("http://127.0.0.1:8000/", content).Result;

            WebApiClient client = new WebApiClient("http://127.0.0.1:8000/", appInfo, EnumDataFormat.Json);
            var res = client.Post("api/GCW/SysUser/PostAdd", json);

            _stopwatch.Stop();

            Console.WriteLine($"执行时间:{_stopwatch.ElapsedMilliseconds}");
            //System.Console.WriteLine(response);

            System.Console.WriteLine(res.Content);
            Assert.IsFalse(res.IsError, res.ErrMsg);
        }

    }
}
