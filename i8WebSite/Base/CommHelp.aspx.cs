using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NG3.Web.UI;
using NG3;
using System.Text;
using NG3.Data;
using NG3.Data.Builder;
using NG3.Common.Web.UI;
using NG3.Common;
using NG3.Base;
using NG3.ESB;

namespace NG3.SUP.Base
{
    public partial class CommHelp : NG2Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var help = this.Help;
            var codeField = Pub.Request("codeField");
            var nameField = Pub.Request("nameField");
            if (codeField.Length == 0) codeField = help.CodeField;
            if (nameField.Length == 0) nameField = help.NameField;

            var sb = new StringBuilder("var helpObj={");
            sb.Append("title:'" + help.Title + "',");
            sb.Append("icon:'" + help.Icon + "',");
            sb.Append("codeField:'" + help.CodeField + "',");
            sb.Append("nameField:'" + help.NameField + "',");
            sb.Append("selectMode:" + (int)help.SelectMode + ",");
            sb.Append("t0:'");
            sb.Append(help.GetTreeTemplate().Replace("\r\n", string.Empty).Replace("'", "''"));
            sb.Append("',t1:'");
            sb.Append(help.GetQueryTemplate().Replace("\r\n", string.Empty).Replace("'", "''"));
            sb.Append("',t2:'");
            sb.Append(help.GetListTemplate().Replace("\r\n", string.Empty).Replace("'","''"));
            sb.Append("'\r\n};");

            this.AddOnReadyScript(sb.ToString());
        }

        [UrlMethod]
        public void GetList(string query)
        {
            var help = this.Help;
            var service = NG3.Common.ESB.GetService(help.DataServiceName, true);
            if (service == null)
            {
                Ajax.ShowMessage(AjaxType.Error, "CommHelp.GetList DataService is null[HelpID={0},ServiceID={1}]".FormatWith(Pub.Request("id"), help.DataServiceName));
            }
            
            var meta = this.RequestMeta.AsGridMeta();
            meta.LoadQuery(query);
            var d = service.Invoke(ServiceAction.Read, meta);
            if (Ajax.DealESBData(d))
            {
                Ajax.WriteRaw(((DataPage)d.Value).ToJSON());
            }
        }

        [UrlMethod]
        public void GetTree(string pid)
        {
            var help = this.Help;
            var service = NG3.Common.ESB.GetService(help.Tree.DataServiceName, true);
            if (service == null)
            {
                Ajax.ShowMessage(AjaxType.Error, "CommHelp.GetTree Tree.DataService is null[HelpID={0},ServiceID={1}]".FormatWith(Pub.Request("id"), help.Tree.DataServiceName));
            }

            var meta = this.RequestMeta.AsGridMeta();
            meta.Query = new QueryParam();
            meta.Query.Add(new NameValuePair<object>("pid", pid));
            meta.Rows = int.MaxValue;
            var d = service.Invoke(ServiceAction.Read, meta);
            if (Ajax.DealESBData(d))
            {
                Ajax.WriteRaw(((DataPage)d.Value).ToJSON());
            }
        }

        private CommHelpItem Help
        {
            get
            {
                var helpID = Pub.Request("id");
                return CommHelpCache.Get(helpID);
            }
        }
    }
}