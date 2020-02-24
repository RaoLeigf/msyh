using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using Newtonsoft.Json;

namespace SUP.CustomForm.Rule
{
    public class BillNoHelp
    {
        #region 临时用于调服务;

        private static Enterprise3.WebApi.Client.WebApiClient _client;

        public static string GetId()
        {
            var i6Info = (I6WebAppInfo) System.Web.HttpContext.Current.Session["NGWebAppInfo"];
            var appInfo = new Enterprise3.WebApi.Client.Models.AppInfoBase
                {
                    /**********************************
                     * 从应用信息中读取;
                     **********************************/
                    OCode = i6Info.OCode, //"001",
                    OrgName = "组织名称",
                    TokenKey = string.Empty,
                    //UCode = i6Info.UCode, //"0006",
                    //UName = "",
                    UserKey = i6Info.LoginID, //"9999",
                    UserName = "楼佳寅",
                    AppKey = "D31B7F91-3068-4A49-91EE-F3E13AE5C48C", // 从xml文件中读取;
                    AppSecret = "103CB639-840C-4E4F-8812-220ECE3C4E4D",
                    DbServerName = "10.0.17.118",
                };

            if (_client == null)
            {
                /***********************************************
                     * 目前的服务地址是日志服务器地址;
                     * 之后会从表fg_info中的i6WebApiUri字段读取;
                     ***********************************************/
                _client = new Enterprise3.WebApi.Client.WebApiClient("http://10.0.15.106:9094/", appInfo,
                                                                     Enterprise3.WebApi.Client.Enums.EnumDataFormat.Json);
            }

            var billIdEntity = new Enterprise3.Common.Model.ReqBillIdEntity
                {
                    TableName = "c_pbc_field_service", // 修改成可配置;
                    PrimaryName = "c_code",
                    NeedCount = 1,
                    Step = 1
                };
            var res = _client.Post<Enterprise3.Common.Model.ReqBillIdEntity>("api/common/BillNo/GetBillIdIncrease",
                                                                             billIdEntity);
            var billNo = (BillNoInfo) JsonConvert.DeserializeObject(res.Content, typeof (BillNoInfo));

            return billNo.BillIdList[0].ToString();
        }
        
        #endregion 临时用于调服务;
    }

    public class BillNoInfo
    {
        public ArrayList BillIdList { get; set; }
    }
}