#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesTest
    * 命名空间：        GXM3.XM.UnitTest
    * 文 件 名：        ExamplesTest.cs
    * 创建时间：        2018/8/17 9:44:52 
    * 作    者：        丰立新    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using Enterprise3.Common.Base.Helpers;
using Enterprise3.Common.HttpContextSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NG3.Data.Service;
using Spring.Testing.Microsoft;
using SUP.Common.Base;

using GXM3.XM.Service.Interface;

namespace GXM3.XM.UnitTest
{
    [TestClass]
    public class ExamplesTest : AbstractTransactionalDbProviderSpringContextTests
    {
        /// <summary>
        /// 加载Xml配置文件
        /// </summary>
        protected override string[] ConfigLocations
        {
            get
            {
                return new String[] { @"D:\Projects\NG3.0ProjectTemplate\UnitTest\Objects.xml" };
            }
        }

        /// <summary>
        /// 获取Facade层的对象
        /// </summary>
        protected IExamplesService ExamplesService
        {
            get
            {
                //此对象为对应配置文件中的ObjectId值的对象
                return applicationContext.GetObject<IExamplesService>("GXM3.XM.UnitTest.Service.ExamplesService");
            }
        }


        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            HttpSimulator hs = new HttpSimulator("/", @"E:\Enterprise\Enterprise3\Enterprise3.Test\Enterprise3.NHORM.Test");
            hs.SimulateRequest();

            I6WebAppInfo appInfo = new I6WebAppInfo
            {
                UserType = UserType.OrgUser,
                PubConnectString = ConfigHelper.GetString("PubConnectString"),
                UserConnectString = ConfigHelper.GetString("UserConnectString"),
                LoginID = "007",
                UserName = "丰立新",
                OCode = "001",
                OrgName = "001组织",
                UCode = "NG0003" //Northwind
            };

            //MockHttpContext.Init();

            System.Web.HttpContext.Current.Session["NGWebAppInfo"] = appInfo;
            ConnectionInfoService.SetSessionConnectString(ConfigHelper.GetString("UserConnectString"));
        }

        [TestMethod]
        public void TestGetAllEffectiveDepts()
        {
            var list = ExamplesService.GetAllEffectiveDepts("001");

            Assert.IsNull(list);

            Assert.IsTrue(list.Count > 0);
        }
    }
}
