using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NG3;
using System.Xml;
using System.Text;
using NG3.Base;

namespace NG3.SUP.Base
{
    public class CommHelpTree
    {
        #region 属性
        public bool Lazy
        {
            get;
            private set;
        }

        public string KeyField
        {
            get;
            private set;
        }

        public string NameField
        {
            get;
            private set;
        }

        public string PIDField
        {
            get;
            private set;
        }

        public string DataServiceName
        {
            get;
            private set;
        } 
        #endregion

        public CommHelpTree(XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            this.Lazy = node.GetAttrOrDefault("Lazy", false);
            this.KeyField = node.GetAttrOrDefault("KeyField", string.Empty);
            this.NameField = node.GetAttrOrDefault("NameField", string.Empty);
            this.PIDField = node.GetAttrOrDefault("PIDField", string.Empty);
            this.DataServiceName = node.GetAttrOrDefault("DataService", string.Empty);
        }

        public string ToTemplate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<TreeList>");
            sb.Append("</TreeList>");
            return "<Tree Lazy=\"{0}\" KeyField=\"{1}\" PIDField=\"{2}\" NameField=\"{3}\" DataService=\"{4}\"/>"
                .FormatWith(this.Lazy, this.KeyField, this.PIDField, this.NameField, this.DataServiceName);
        }
    }
}