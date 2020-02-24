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
    public class CommHelpItem : ILoadFromXML
    {
        #region 属性
        public string Remarks
        {
            get;
            private set;
        }

        public string ID
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Icon
        {
            get;
            private set;
        }

        public string CodeField
        {
            get;
            private set;
        }

        public string NameField
        {
            get;
            private set;
        }

        public string SortField
        {
            get;
            private set;
        }

        public string DataServiceName
        {
            get;
            private set;
        }

        public SelectMode SelectMode
        {
            get;
            private set;
        }

        public string[] AllField
        {
            get;
            private set;
        }

        private string AllFieldKey;

        public CommHelpQuery Query
        {
            get;
            private set;
        }

        public XmlNode List
        {
            get;
            private set;
        }

        public CommHelpTree Tree
        {
            get;
            private set;
        } 
        #endregion

        private string _queryTemplate;
        public string GetQueryTemplate()
        {
            if (_queryTemplate == null)
            {
                //Query
                if (this.Query != null)
                {
                    _queryTemplate = this.Query.ToTemplate();
                }
                else
                {
                    _queryTemplate = string.Empty;
                }
            }
            return _queryTemplate;
        }

        private string _treeTemplate;
        public string GetTreeTemplate()
        {
            if (_treeTemplate == null)
            {
                //tree
                if (this.Tree != null)
                {
                    _treeTemplate = this.Tree.ToTemplate();
                }
                else
                {
                    _treeTemplate = string.Empty;
                }
            }
            return _treeTemplate;
        }

        private string _listTemplate;
        public string GetListTemplate()
        {
            if (_listTemplate == null)
            {
                if (this.List == null)
                {
                    if (this.AllField.Length > 0)
                    {
                        #region AllField
                        StringBuilder sb = new StringBuilder("<TreeList>");
                        if (!string.IsNullOrEmpty(this.AllFieldKey))
                        {
                            sb.AppendFormat("<Properties key=\"{0}\"/>", this.AllFieldKey);
                        }
                        sb.Append("<Cols>");
                        foreach (string f in this.AllField)
                        {
                            //字段名,[字段中文名],[类型],[宽度]
                            var fpart = f.Split(',');
                            //字段名
                            sb.Append("<Col name=\"");
                            sb.Append(fpart[0]);
                            sb.Append("\"");
                            //[类型]
                            if (fpart.GetOrDefault<string>(2) != null)
                            {
                                sb.Append(" datatype=\"");
                                sb.Append(fpart.GetOrDefault<string>(2));
                                sb.Append("\"");
                            }
                            //[宽度]
                            if (fpart.GetOrDefault<string>(3) != null)
                            {
                                sb.Append(" width=\"");
                                sb.Append(fpart.GetOrDefault<string>(3));
                                sb.Append("\"");
                            }
                            else
                            {//[最小宽度30]
                                sb.Append(" minWidth=\"30\"");
                            }
                            sb.Append(">");
                            //[字段中文名]
                            sb.Append(fpart.GetOrDefault<string>(1, fpart[0]));
                            sb.Append("</Col>");
                            var fieldTitle = fpart.GetOrDefault<string>(1);
                        }
                        sb.Append("</Cols></TreeList>");
                        _listTemplate = sb.ToString().Replace("'", "\\'");
                        #endregion
                    } 
                }
                else
                {
                    _listTemplate = string.Format("<TreeList>{0}</TreeList>", this.List.InnerXml);
                }
            }

            return _listTemplate;
        }

        public CommHelpItem(XmlNode node)
        {
            this.LoadFromXML(node);
        }

        public void LoadFromXML(System.Xml.XmlNode node)
        {
            //以后要改成 Load出错时,提示信息更加人性化,如告诉用户那个 helpid 的 哪个节点读取出错

            if (node == null) throw new ArgumentNullException("node");
            //this.XmlNode = node;

            this.ID = node.GetAttrOrDefault("id", null);
            
            this.Remarks = this.GetPropFromNode(node, "Remarks", string.Empty);
            this.Title = this.GetPropFromNode(node, "Title", string.Empty);
            this.Icon = this.GetPropFromNode(node, "Icon", string.Empty);
            this.CodeField = this.GetPropFromNode(node, "CodeField", string.Empty);
            this.NameField = this.GetPropFromNode(node, "NameField", string.Empty);
            this.SortField = this.GetPropFromNode(node, "SortField", string.Empty);
            this.DataServiceName = this.GetPropFromNode(node, "DataService", string.Empty);
            this.SelectMode = (SelectMode)Enum.Parse(typeof(SelectMode), this.GetPropFromNode(node, "SelectMode", "Default"), true);

            this.List = node.SelectSingleNode("List") ?? node.SelectSingleNode("TreeList");
            
            var treeNode = node.SelectSingleNode("Tree");
            if (treeNode != null)
            {
                this.Tree = new CommHelpTree(treeNode);
            }

            var queryNode = node.SelectSingleNode("Query");
            if (queryNode != null)
            {
                this.Query = new CommHelpQuery(queryNode);
            }
            
            if (this.List == null)
            {
                var allFieldNode = node.SelectSingleNode("AllField");

                if (allFieldNode != null)
                {
                    this.AllField = allFieldNode.InnerXml.Split('|');
                    this.AllFieldKey = allFieldNode.GetAttrOrDefault("key", string.Empty);
                }
                else
                {
                    this.AllField = new string[0];
                    this.AllFieldKey = string.Empty;
                }                
            }
        }

        private string GetPropFromNode(XmlNode node, string pname, string defV)
        {
            string v = null;
            var t = node.SelectSingleNode(pname);
            if (t != null)
            {
                v = t.InnerXml;
            }

            return v ?? defV;
        }
    }

    public enum SelectMode
    {
        Default=0,
        Single=1,
        Multi=2,
        CrossPage=3
    }
}