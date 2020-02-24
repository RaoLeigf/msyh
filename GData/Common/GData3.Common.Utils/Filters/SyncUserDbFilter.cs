using Newtonsoft.Json;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Spring.Data.Common;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using NG3.Data.Service;
using SUP.Common.Base;

namespace GData3.Common.Utils.Filters
{
    public class SyncUserDbFilter: ActionFilterAttribute
    {        
        public override void OnActionExecuting(HttpActionContext actionContext)
        {   
            //获取AppInfo值 头部信息记录
            var base64EncodedBytes = Convert.FromBase64String(HttpContext.Current.Request.Headers.GetValues("AppInfo").First());
            var jsonText = Encoding.UTF8.GetString(base64EncodedBytes);
            var AppInfo = JsonConvert.DeserializeObject<AppInfoBase>(jsonText);

            if (AppInfo != null)
            {
                string curConnect = DbHelper.ConnectString;
                if (curConnect.IndexOf(AppInfo.DbName, StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    //连接串包含了当前数据库
                }
                else
                {
                    DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
                    string result, userConn;
                    var pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);
                    string defaultConn = dbbuilder.GetDefaultConnString();

                    if (AppInfo.DbName.ToLower() == "ngsoft")
                    {
                        userConn = pubConn;
                    }
                    else
                    {
                        userConn = string.IsNullOrWhiteSpace(AppInfo.DbName)
                            ? defaultConn
                            : dbbuilder.GetAccConnstringElement(0, AppInfo.DbName, pubConn,
                                out result);
                    }

                    //设置当前数据库连接信息
                    ConnectionInfoService.SetCallContextConnectString(userConn);
                    MultiDelegatingDbProvider.CurrentDbProviderName = AppInfo.DbName;
                }
            }            

            base.OnActionExecuting(actionContext);
        }
    }
}
