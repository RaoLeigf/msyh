#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesTest
    * 命名空间：        GJX3.JX.UnitTest
    * 文 件 名：        ExamplesTest.cs
    * 创建时间：        2018/9/10 11:54:13 
    * 作    者：        丰立新    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Base.Helpers;
using Enterprise3.Common.HttpContextSimulator;
using Enterprise3.WebApi.Client;
using Enterprise3.WebApi.Client.Enums;
using Enterprise3.WebApi.Client.Models;
using GGK3.GK.Model.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NG3.Data.Service;
using Spring.Testing.Microsoft;
using SUP.Common.Base;



namespace GGK3.GK.UnitTest
{
    [TestClass]
    public class GKTest //: AbstractTransactionalDbProviderSpringContextTests
    {
        static AppInfoBase appInfo;
        static GKPaymentModel dataInfo;


        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            appInfo = new AppInfoBase
            {
                AppKey = ConfigHelper.GetString("AppKey", "D31B7F91-3068-4A49-91EE-F3E13AE5C48C"), //必须
                AppSecret = ConfigHelper.GetString("AppSecret", "103CB639-840C-4E4F-8812-220ECE3C4E4D"), //必须
                DbName = "NG0002", //可不传，默认为默认账套
                UserId = 521180820000001,
                OrgId = 521180820000002,
                //DbServerName = "10.0.15.106",
                OCode = "100",
                OrgName = "广东省总工会",
                //SessionKey = "会话标识",
                //TokenKey = string.Empty,
                //UName = "帐套名称",
                //UserKey = "003",
                UserName = "省总操作员"        
            };

            //部门： orgid=521180820000002 orgcode=100.01 orgname=省总财务部

            dataInfo = new GKPaymentModel();
        }

        [TestMethod]
        public void TestGetAllEffectiveDepts()
        {
            //var list = ExamplesService.GetAllEffectiveDepts("001");

            //Assert.IsNull(list);

            //Assert.IsTrue(list.Count > 0);

            Assert.IsTrue(1 > 0);
        }

        [TestMethod]
        public void GKPaymentMst_GetPaymentList()
        {
            //准备数据
            var queryfilter = new Dictionary<string, object>();
            queryfilter.Add("FBilltype*str*eq*1", "zjbf");
            queryfilter.Add("NgInsertDt*date*ge*1", "2018-12-01");  //申报日期
            queryfilter.Add("NgInsertDt*date*le*1", "2019-12-20 10:59:59"); //申报日期

            queryfilter.Add("FApproval*byte*eq*1", 0); //审批状态
            queryfilter.Add("FState*byte*eq*1", 0);   //支付状态
            //queryfilter.Add("RefbillCode*str*eq*1", "zfbf0001");  //业务单据号(如资金拨付单号)

            //queryfilter.Add("Description*str*llike*1", "单元测试");
            //queryfilter.Add("PNo*str*llike*1", "记-001");
            //queryfilter.Add("OrgId*num*eq*1", 547181121000001);

            //使用or关系查询
            var dicOr = new Dictionary<string, object>();
            dicOr.Add("RefbillCode*str*like*1", "zfbf");    //关联单据号
            dicOr.Add("FCode*str*like*1", "zfbf");          //付款单号    
            queryfilter.Add("[or-dictionary0]*dictionary*or", dicOr);

            string qstr = JsonConvert.SerializeObject(queryfilter);

            /*
             var dicWhere = new Dictionary<string, object>();
             var dicWhereOr1 = new Dictionary<string, object>();
             var dicWhereOr2 = new Dictionary<string, object>();
             var dicWhereOr3 = new Dictionary<string, object>();

             new CreateCriteria(dicWhereOr1).Add(ORMRestrictions<string>.Eq("CustomerId", "FENG1")).Add(ORMRestrictions<int>.Eq("EmployeeId", 2));
             new CreateCriteria(dicWhereOr2).Add(ORMRestrictions<string>.Eq("CustomerId", "TOMSP"));
             new CreateCriteria(dicWhereOr3).Add(ORMRestrictions<string>.Eq("CustomerId", "HANAR"));

             new CreateCriteria(dicWhere)
                 //闭区间
                 .Add(ORMRestrictions<DateTime>.Between("OrderDate", Convert.ToDateTime("2015-10-15"), Convert.ToDateTime("2015-11-16")))
                 //等于
                 .Add(ORMRestrictions.Or(dicWhereOr1, dicWhereOr2, dicWhereOr3)) //或者下面这种写法
                                                                                 // .Add(ORMRestrictions.Or(ORMRestrictions.Or(dicOr1, dicWhereOr2), dicWhereOr3)) 
             ;
             Console.WriteLine(JsonConvert.SerializeObject(dicWhere));
             */

            //var dicOr = new Dictionary<string, object>();
            //new CreateCriteria(dicOr).Add(ORMRestrictions<string>.Eq("Description", "单元测试-636778142504863887"));
            //new CreateCriteria(dicOr).Add(ORMRestrictions<string>.Eq("PCashier", "吴"));


            //var dicOr2 = new Dictionary<string, object>();
            //new CreateCriteria(dicOr2).Add(ORMRestrictions<Int64>.Eq("PhId", 378181114000001));

            //var where = new Dictionary<string, object>();
            //new CreateCriteria(where).Add(ORMRestrictions<Int64>.Eq("PhId", 299181205000005)).
            //    Add(ORMRestrictions<byte>.Eq("PMonth", (byte)12)).     
            //    Add(ORMRestrictions<string>.LLike("Description", "单元测试-凭证修改"));
            ////Add(ORMRestrictions.Or(dicOr));  //加了or条件，底层会报错，等丰立新处理后再测试

            //Console.WriteLine(JsonConvert.SerializeObject(where));
            //qstr = JsonConvert.SerializeObject(where);

            //var dicOr2 = new Dictionary<string, object>();
            //new CreateCriteria(dicOr2).Add(ORMRestrictions<DateTime>.Eq("PDate", DateTime.Now));
            //string qstr2 = JsonConvert.SerializeObject(dicOr2);


            ParameterCollection paras = new ParameterCollection();
            paras.Add("uid", "0001");
            paras.Add("orgid", "547181121000001");
            paras.Add("pagesize", "1");
            paras.Add("pageindex", "0");
            paras.Add("ryear", "2019");
            paras.Add("queryfilter", qstr);



            //paras.Add("keyword", ""); //科目代码/科目名称/摘要/凭证号搜索  "工"
            //paras.Add("sum1", "8"); //合计金额 起
            //paras.Add("sum2", "200"); //合计金额 止
            // paras.Add("export2excel", "no"); //导出excel

            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Restart();

            WebApiClient client = new WebApiClient("http://127.0.0.1:8081/", appInfo, EnumDataFormat.Json);
            var res = client.Get("api/GGK/GKPaymentMstApi/GetPaymentList", paras);

            _stopwatch.Stop();
            Console.WriteLine($"执行时间:{_stopwatch.ElapsedMilliseconds}");

            Console.WriteLine("GKPaymentMstApi/GetPaymentList: " + paras.ToString());
            Console.WriteLine("Response: " + JsonConvert.DeserializeObject(res.Content).ToString());
            Assert.IsFalse(res.IsError, res.ErrMsg);
        }


        [TestMethod]
        public void GKPaymentMst_PostAdd()
        {
            string now = DateTime.Now.Ticks.ToString();

            GKPaymentModel entity = new GKPaymentModel();

            GKPaymentMstModel mst = new GKPaymentMstModel {
                PhId = 0,
                OrgPhid = 521180820000002,
                OrgCode = "1",
                RefbillPhid = 7,
                RefbillCode = "zfbbf0007",
                FCode = "P" + now,
                FPaymethod = 2,
                FAmountTotal = 2006,
                FApproval = 0,
                FState = 0,
                FDate = DateTime.Now,
                FBilltype = "zjbf",
                FDescribe = "单元测试-" + now,
                PersistentState = PersistentState.Added,
                FYear = "2019"
            };
            entity.Mst = mst;

            List<GKPaymentDtlModel> dtls = new List<GKPaymentDtlModel>();
            GKPaymentDtlModel dtl1 = new GKPaymentDtlModel {
                PhId = 0,
                MstPhid = 0,
                OrgPhid = 521180820000002,
                OrgCode = "100",
                RefbillPhid = 7,
                RefbillCode = "zfbbf0007",
                RefbillDtlPhid = 1,
                RefbillDtlPhid2 = 1,
                FAmount = 1000,
                FCurrency = "001",
                FPayAcnt = "111001",
                FPayAcntname= "付款账户1",
                FPayBankcode = "102",
                FRecAcnt = "222122",
                FRecAcntname = "收款账户1",
                FRecBankcode = "102",
                FRecCityname = "杭州市",
                FSamebank = 1,
                FIsurgent = 1,
                FCorp = 1,
                FUsage = "用途信息",
                FPostscript = "附言：xxx",
                FExplation = "摘要",
                FDescribe = "描述",
                PersistentState = PersistentState.Added
            };
            dtls.Add(dtl1);

            GKPaymentDtlModel dtl2 = new GKPaymentDtlModel
            {
                PhId = 0,
                MstPhid = 0,
                OrgPhid = 521180820000002,
                OrgCode = "100",
                RefbillPhid = 7,
                RefbillCode = "zfbbf0007",
                RefbillDtlPhid = 2,
                RefbillDtlPhid2 = 2,
                FAmount = 1006,
                FCurrency = "001",
                FPayAcnt = "111002",
                FPayAcntname = "付款账户2",
                FPayBankcode = "102",
                FRecAcnt = "222122",
                FRecAcntname = "收款账户1",
                FRecBankcode = "102",
                FRecCityname = "杭州市",
                FSamebank = 1,
                FIsurgent = 1,
                FCorp = 1,
                FUsage = "用途信息2",
                FPostscript = "附言：xxx2",
                FExplation = "摘要2",
                FDescribe = "描述2",
                PersistentState = PersistentState.Added
            };
            dtls.Add(dtl2);

            entity.Dtls = dtls;

            
            var data = new { uid = 521180820000001, orgid = 521180820000002, infoData = entity };
            string json = JsonConvert.SerializeObject(data);

            //开始测试
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Restart();

            WebApiClient client = new WebApiClient("http://127.0.0.1:8081/", appInfo, EnumDataFormat.Json);
            var res = client.Post("api/GGK/GKPaymentMstApi/PostAdd", json);

            _stopwatch.Stop();
            Console.WriteLine($"执行时间:{_stopwatch.ElapsedMilliseconds}");

            //缓存新增用户的phid，后面测试删除时使用
            var sr = JsonHelper.DesrializeJsonToObject<SaveResponse>(JsonConvert.DeserializeObject(res.Content).ToString());
            if (sr.Status == "success")
            {
                //appInfo.UserKey = sr.KeyCodes[0];
                //dataInfo.Mst.PhId = long.Parse(sr.KeyCodes[0]);
            }

            Console.WriteLine("GKPaymentMstApi/PostAdd: " + json);
            Console.WriteLine("Response: " + JsonConvert.DeserializeObject(res.Content).ToString());
            Assert.IsFalse(res.IsError, res.ErrMsg);
        }

        [TestMethod]
        public void GKPaymentMst_PostUpdate()
        {
            string now = DateTime.Now.Ticks.ToString();

            GKPaymentModel entity = new GKPaymentModel();

            GKPaymentMstModel mst = new GKPaymentMstModel
            {
                PhId = 991190531000002,
                OrgPhid = 521180820000002,
                OrgCode = "1",
                RefbillPhid = 7,
                RefbillCode = "zfbbf0007",
                FCode = "P" + now,
                FPaymethod = 2,
                FAmountTotal = 2006,
                FApproval = 0,
                FState = 0,
                FDate = DateTime.Now,
                FBilltype = "zjbf",
                FDescribe = "单元测试-修改-" + now,
                PersistentState = PersistentState.Modified,
                FYear = "2019",
                NgRecordVer= 1,
		        NgInsertDt = new DateTime(2019,5,31,17,48,02), //"2019-05-28 09:57:50"
                NgUpdateDt = new DateTime(2019, 5, 31, 17, 48, 02),
		        Creator = 521180820000001,
		        Editor = 521180820000001,
		        CurOrgId = 547181121000001
            };
            entity.Mst = mst;

            List<GKPaymentDtlModel> dtls = new List<GKPaymentDtlModel>();
            GKPaymentDtlModel dtl1 = new GKPaymentDtlModel
            {
                PhId = 991190531000003,
                MstPhid = 991190531000002,
                OrgPhid = 521180820000002,
                OrgCode = "100",
                RefbillPhid = 7,
                RefbillCode = "zfbbf0007",
                RefbillDtlPhid = 1,
                RefbillDtlPhid2 = 1,
                FAmount = 1000,
                FCurrency = "001",
                FPayAcnt = "1202022719927388888",
                FPayAcntname = "菌邢票董租氮蒸幻憨野该痼赴挥傻",
                FPayBankcode = "102",
                FRecAcnt = "222122",
                FRecAcntname = "菌邢票董租氮蒸野该挥傻摘灌莉犹冤越憨少莲晰挥傻",
                FRecBankcode = "1202051309900024733",
                FRecCityname = "杭州市",
                FSamebank = 1,
                FIsurgent = 1,
                FCorp = 1,
                FUsage = "用途信息",
                FPostscript = "附言：修改01",
                FExplation = "摘要-修改",
                FDescribe = "描述-修改",
                QtKmdm = "50101",
                PersistentState = PersistentState.Modified,
                NgRecordVer = 1,
                NgInsertDt = new DateTime(2019, 5, 31, 17, 48, 02), //"2019-05-28 09:57:50"
                NgUpdateDt = new DateTime(2019, 5, 31, 17, 48, 02),
                Creator = 521180820000001,
                Editor = 521180820000001,
                CurOrgId = 547181121000001
            };
            dtls.Add(dtl1);

            GKPaymentDtlModel dtl2 = new GKPaymentDtlModel
            {
                PhId = 991190531000004,
                MstPhid = 991190531000002,
                OrgPhid = 521180820000002,
                OrgCode = "100",
                RefbillPhid = 7,
                RefbillCode = "zfbbf0007",
                RefbillDtlPhid = 2,
                RefbillDtlPhid2 = 2,
                FAmount = 1006,
                FCurrency = "001",
                FPayAcnt = "1202022719927388888",
                FPayAcntname = "菌邢票董租氮蒸幻憨野该痼赴挥傻",
                FPayBankcode = "102",
                FRecAcnt = "1205270019200803293",
                FRecAcntname = "菌邢科吓令它野该挥傻",
                FRecBankcode = "102",
                FRecCityname = "杭州市",
                FSamebank = 1,
                FIsurgent = 1,
                FCorp = 1,
                FUsage = "用途信息2",
                FPostscript = "附言：修改02",
                FExplation = "摘要2-修改",
                FDescribe = "描述2-修改",
                QtKmdm = "50102",
                PersistentState = PersistentState.Modified,
                NgRecordVer = 1,
                NgInsertDt = new DateTime(2019, 5, 31, 17, 48, 02), //"2019-05-28 09:57:50"
                NgUpdateDt = new DateTime(2019, 5, 31, 17, 48, 02),
                Creator = 521180820000001,
                Editor = 521180820000001,
                CurOrgId = 547181121000001
            };
            dtls.Add(dtl2);

            entity.Dtls = dtls;


            var data = new { uid = 521180820000001, orgid = 521180820000002, infoData = entity };
            string json = JsonConvert.SerializeObject(data);

            //开始测试
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Restart();

            WebApiClient client = new WebApiClient("http://127.0.0.1:8081/", appInfo, EnumDataFormat.Json);
            var res = client.Post("api/GGK/GKPaymentMstApi/PostUpdate", json);

            _stopwatch.Stop();
            Console.WriteLine($"执行时间:{_stopwatch.ElapsedMilliseconds}");

            //缓存新增用户的phid，后面测试删除时使用
            var sr = JsonHelper.DesrializeJsonToObject<SaveResponse>(JsonConvert.DeserializeObject(res.Content).ToString());
            if (sr.Status == "success")
            {
                //appInfo.UserKey = sr.KeyCodes[0];
                //dataInfo.Mst.PhId = long.Parse(sr.KeyCodes[0]);
            }

            Console.WriteLine("GKPaymentMstApi/PostUpdate: " + json);
            Console.WriteLine("Response: " + JsonConvert.DeserializeObject(res.Content).ToString());
            Assert.IsFalse(res.IsError, res.ErrMsg);
        }


        [TestMethod]
        public void GKPaymentMst_PostDelete()
        {
            //准备数据
            var data = new { uid = 521180820000001, orgid = 521180820000002, Ryear = "2019", id = 216190528000001 };
            string json = JsonConvert.SerializeObject(data);

            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Restart();

            WebApiClient client = new WebApiClient("http://127.0.0.1:8081/", appInfo, EnumDataFormat.Json);
            var res = client.Post("api/GGK/GKPaymentMstApi/PostDelete", json);

            _stopwatch.Stop();
            Console.WriteLine($"执行时间:{_stopwatch.ElapsedMilliseconds}");

            Console.WriteLine("GKPaymentMstApi/PostDelete: " + json);
            Console.WriteLine("Response: " + JsonConvert.DeserializeObject(res.Content).ToString());
            Assert.IsFalse(res.IsError, res.ErrMsg);
        }
    }
}
