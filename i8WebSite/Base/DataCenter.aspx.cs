using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NG3.Web.UI;
using NG3;
using System.Collections;
//using NG2.WCached.Client;
using System.Data;
using NG3.Data;
using NG3.Data.Builder;
using NG3.Common.Web.UI;
using NG3.Common;
using NG3.Base;
using NG3.ESB;

namespace NG3.SUP.Base
{
    public partial class DataCenter : NG2Page
    {
        protected override string DefaultAction
        {
            get { return "List"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        
        [UrlMethod,UrlSecurity(UrlSecurity.Everyone)]
        public void List(string pid)
        {
            var d = NG3.Common.ESB.GetService<ServiceDataCenter>().Invoke("list", pid);
            if (Ajax.DealESBData(d))
            {
                Ajax.WriteRaw(d.Value.TryParseToString());
            }
        }

        [UrlMethod]
        public void Data(string id)
        {
            Ajax.WriteRaw("Data"+id);
        }

        [UrlMethod]
        public void Desc(string id)
        {
            Ajax.WriteRaw("Desc" + id);
        }

        [UrlMethod]
        public void GetData(string serviceID, string fields, string query)
        {
            ServiceBase service = null;
            if (serviceID.IndexOf(".") > 0)
            {//系统服务
                service = NG3.Common.ESB.GetService(serviceID, true);                
            }
            else
            {//自定义服务
                //根据serviceID 去数据库取得改service对应的xml文本返回
                var xml = this.GetServiceXML(serviceID);
                service = new DataService(new string[] { xml });
            }

            if (service == null)
            {
                Ajax.ShowMessage(AjaxType.Error, "CommHelp.GetList DataService is null[HelpID={0},ServiceID={1}]".FormatWith(Pub.Request("id"), serviceID));
            }
            
            var meta = new GridMeta();
            meta.Custom = new GridCustomMeta(fields);
            if (!string.IsNullOrEmpty(query))
            {
                meta.LoadQuery(query);
            }
            meta.Rows = int.MaxValue;
            var d = service.Invoke(ServiceAction.Read, meta);
            if (Ajax.DealESBData(d))
            {
                Ajax.WriteRaw(((DataPage)d.Value).ToJSON());
            }
        }

        //根据serviceID 去数据库取得改service对应的xml文本返回
        private string GetServiceXML(string serviceID)
        {
            return NG3.Common.ESB.GetService<ServiceDataCenter>().GetServiceXML(serviceID);
        }
    }
}