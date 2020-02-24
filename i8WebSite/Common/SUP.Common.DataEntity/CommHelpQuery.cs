using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;

using NG3;
using NG3.Base;

namespace SUP.Common.DataEntity
{
    public class CommHelpQuery
    {
        private XmlNode node;
        public CommHelpQuery(XmlNode _node)
        {
            node = _node;

            _childNodes = new List<XmlNode>();
            foreach (XmlNode cn in node.ChildNodes)
            {
                if (cn.NodeType == XmlNodeType.Element && cn.Name.EqualsIgnoreCase("Input"))
                {
                    cn.SetAttr("leftText", cn.GetAttrOrDefault("leftText", string.Empty).EnsureStartsWith(" "));
                    if (cn.GetAttrOrDefault("width", 0) == 0)
                    {
                        cn.SetAttr("width", "2");
                    }
                    if (cn.GetAttrOrDefault("editSize", 0) == 0)
                    {
                        cn.SetAttr("editSize", (cn.GetAttrOrDefault("width", 2) - 1).ToString());
                    }
                    if (cn.GetAttrOrDefault("leftTextAlign", string.Empty) == string.Empty)
                    {
                        cn.SetAttr("leftTextAlign", "left");
                    }

                    _childNodes.Add(cn);
                }

                //Droplists 只有一个
                if (cn.NodeType == XmlNodeType.Element && cn.Name.EqualsIgnoreCase("Droplists"))
                {
                    _childNode_Droplist = cn;
                }
            }
        }

        private XmlNode _childNode_Droplist;

        private List<string> _cols;
        public List<string> Cols
        {
            get
            {
                if (_cols == null)
                {
                    var s = node.GetAttrOrDefault("cols", "100,0.1,100,0.1");
                    _cols = s.Split(',').ToList();
                }
                return _cols;
            }
        }

        private List<XmlNode> _childNodes;
        public List<XmlNode> ChildNodes
        {
            get
            {
                return _childNodes;
            }
        }

        public string ToTemplate()
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("<Freeform><Properties bgColor=\"#FFFFFF\"/>");
            sb.Append("<Objects>");
            sb.Append("<TableLayout width=\"100%\">");

            sb.Append("<col width=\"10\"/>");
            foreach (var c in this.Cols)
            {
                sb.Append("<col width=\"");
                sb.Append(c);
                sb.Append("\"/>");
            }

            sb.Append("<col width=\"5\">");
            sb.Append("<col width=\"75\">");
            sb.Append("<col width=\"5\">");
            sb.Append("<col width=\"75\">");
            sb.Append("<col width=\"10\">");

            ExportChildNodes(sb, 0, 0);

            if (_childNode_Droplist != null)
            {
                sb.Append("<tr height=\"4\"/></TableLayout></Objects>");
                sb.Append(_childNode_Droplist.OuterXml);
                sb.Append("</Freeform>");
            }
            else
            {
                sb.Append("<tr height=\"4\"/></TableLayout></Objects></Freeform>");
            }
            return sb.ToString();
        }

        private void ExportChildNodes(StringBuilder sb, int index, int usedCols)
        {
            if (index >= this.ChildNodes.Count)
            {
                AppendEmptyTD(sb, usedCols, this.Cols.Count);

                sb.Append("<td/>");
                sb.Append("<td><Input id=\"btnquery\" type=\"Button\" tabOrder=\"998\" text=\"搜索\"/></td>");
                sb.Append("<td/>");
                sb.Append("<td><Input id=\"btnreset\" type=\"Button\" tabOrder=\"999\" text=\"清空\"/></td>");
                sb.Append("<td/>");

                sb.Append("</tr>");
                return;
            }

            var child = this.ChildNodes[index];
            var width = child.GetAttrOrDefault("width", 2).TryParseToInt();
            if (usedCols == 0)
            {
                sb.Append("<tr height=\"4\"/><tr height=\"24\"><td/>");
            }

            if (usedCols + width <= this.Cols.Count)
            {
                usedCols += width;
                sb.Append("<td>");
                sb.Append(child.OuterXml);
                sb.Append("</td>");
                AppendEmptyTD(sb, 0, width - 1);

                ExportChildNodes(sb, ++index, usedCols);
            }
            else
            {
                AppendEmptyTD(sb, usedCols, this.Cols.Count);
                sb.Append("<td/></tr>");
                usedCols = 0;

                ExportChildNodes(sb, index, usedCols);
            }

        }

        private void AppendEmptyTD(StringBuilder sb, int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                sb.Append("<td/>");
            }
        }
    }
}