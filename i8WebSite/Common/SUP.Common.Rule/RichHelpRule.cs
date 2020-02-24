using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using System.Data;
using SUP.Common.Base;
using NG3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SUP.Common.Rule
{
    public class RichHelpRule
    {
        
        private RichHelpDac richDac = new RichHelpDac();
        private CommonHelpDac commDac = new CommonHelpDac();

        //批量代码转名称
        public HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list)
        {
            foreach (HelpValueNameEntity item in list)
            {
                item.Name = commDac.GetName(item.HelpID, item.Code, item.HelpType, string.Empty, item.OutJsonQuery, item.HelpType);
            }
            
            //foreach (HelpValueNameEntity item in list)
            //{
            //    if (item.HelpType == "ngRichHelp" || item.HelpType == "ngMultiRichHelp")//从数据库获取帮助信息
            //    {
            //        item.Name = richDac.GetName(item.HelpID, item.Code, item.SelectMode, string.Empty, item.OutJsonQuery, item.HelpType);
            //    }
            //    else//ngCommonHelp,ngComboBox,ngTreeComboBox暂时从xml配置文件获得帮助信息
            //    {
            //        item.Name = commDac.GetName(item.HelpID, item.Code, item.SelectMode, string.Empty, item.OutJsonQuery, item.HelpType);
                   
            //    }
            //}

            return list.ToArray<HelpValueNameEntity>();
        }


        public IList<TreeJSONBase> GetTreeList(string helpid, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, string nodeid, bool ormMode)
        {
            string filter = string.Empty;
            CommonHelpEntity item = richDac.GetCommonHelpItem(helpid);

            TreeListBuilder builder = new TreeListBuilder();

            string treePid = item.TreePid;
            string treeChildId = item.TreeChildId;
            if (ormMode)
            {
                string tableName = item.TableName;
                string[] s = item.TableName.Split(' ');//有别名
                if (s.Length > 0)
                {
                    tableName = s[s.Length - 1].Trim();
                }
                builder.ID = DataConverterHelper.FieldToProperty(tableName, item.CodeField);//item.CodeField;
                builder.Text = DataConverterHelper.FieldToProperty(tableName, item.NameField);//item.NameField;
                treePid = DataConverterHelper.FieldToProperty(tableName, item.TreePid);
                item.TreeChildId = DataConverterHelper.FieldToProperty(tableName, item.TreeChildId);
            }
            else
            {
                builder.ID = item.CodeField;
                builder.Text = item.NameField;
            }

            DataTable dt = richDac.GetTreeList(helpid, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter, nodeid, ormMode);

            string sort = string.Empty;
            if (!string.IsNullOrWhiteSpace(item.SortProperty))
            {
                sort = item.SortProperty + " asc ";
            }

            if (string.IsNullOrEmpty(treePid))
            {
                throw new Exception("树节点pid未设置,请在通用帮助注册设置[父节点id]和[子节点id]!");
            }

            Type type = dt.Columns[treePid].DataType;

            if ("root" == nodeid)//首次加载
            {
                if (type == typeof(Int64) || type == typeof(Int32))
                {
                    filter = "(" + treePid + "=0 or " + treePid + " is null)";
                }
                else
                {
                    filter = "(" + treePid + "='' or " + treePid + "is null)";
                }
                return builder.GetExtTreeList(dt, treePid, treeChildId, filter, sort, TreeDataLevelType.TopLevel, 2);
            }
            else//懒加载
            {
                //return builder.GetExtTreeList(dt, item.TreePid, item.TreeChildId, filter, TreeDataLevelType.LazyLevel);
                return builder.LazyLoadTreeList(dt, treePid, treeChildId, nodeid);
            }

        }

        /// <summary>
        /// 获取分类的树型数据
        /// </summary>
        /// <param name="code">主键</param>
        /// <param name="nodeid">节点id</param>
        /// <returns></returns>
        public IList<TreeJSONBase> GetQueryProTree(string code,string nodeid)
        {
            string filter = string.Empty; 
            bool treeshow = false;
            bool lazyload = false;
            DataTable propertyInfodt = new DataTable();
            DataTable dt = richDac.GetQueryProTree(code, ref propertyInfodt);


            DataRow dr = propertyInfodt.Rows[0];
            //是否能构建成树
            treeshow = (dr["treeshow"].ToString() == "1");
            lazyload = (dr["tree_lazyload"].ToString() == "1"); 

            if (treeshow)
            {
                QueryPropertyTreeBuilder builder = new QueryPropertyTreeBuilder();
                builder.ID = dr["tree_id"].ToString();
                builder.Text = dr["tree_text"].ToString();
                builder.TreeSearchKey = dr["tree_searchkey"].ToString();
                builder.TreeRefKey = dr["list_treerefkey"].ToString();

                if ("root" == nodeid)//首次加载
                {
                    filter = "(" + dr["tree_pid"].ToString() + " is null or " + dr["tree_pid"].ToString() + "='')";
                    if (lazyload)
                    {
                        return builder.GetExtTreeList(dt, dr["tree_pid"].ToString(), dr["tree_id"].ToString(), filter, TreeDataLevelType.TopLevel, 2);
                    }
                    else
                    {
                        return builder.GetExtTreeList(dt, dr["tree_pid"].ToString(), dr["tree_id"].ToString(), filter, TreeDataLevelType.TopLevel);
                    }
                }
                else
                {
                    //return builder.GetExtTreeList(dt, dr["tree_pid"].ToString(), dr["tree_id"].ToString(), filter, TreeDataLevelType.LazyLevel);
                    return builder.LazyLoadTreeList(dt, dr["tree_pid"].ToString(), dr["tree_id"].ToString(), nodeid);
                }
                
            }
            else
            {
                IList<TreeJSONBase> rootlist = new List<TreeJSONBase>();
                IList<TreeJSONBase> list = new List<TreeJSONBase>();

                QueryPropertyTreeJSON root = new QueryPropertyTreeJSON();
                root.id = "root";
                root.cls = "folder";
                root.text = "root";
                root.expanded = true;
                // root.@checked = false;
                root.children = list;

                foreach (DataRow row in dt.Rows)
                {
                    QueryPropertyTreeJSON child = new QueryPropertyTreeJSON();
                    child.id = row[dr["tree_id"].ToString()].ToString();
                    child.text = row[dr["tree_text"].ToString()].ToString();
                    child.cls = "file";
                    if (!string.IsNullOrWhiteSpace(dr["tree_searchkey"].ToString()))
                    {
                        child.treesearchkey = row[dr["tree_searchkey"].ToString()].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(dr["list_treerefkey"].ToString()))
                    {
                        child.treerefkey = dr["list_treerefkey"].ToString();//row[dr["list_treerefkey"].ToString()].ToString();
                    }

                    child.leaf = true;
                    root.children.Add(child);
                }              

                rootlist.Add(root);
                return list;//rootlist;
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public JArray GetRichQueryItems(string helpid)
        {
            JArray arr = new JArray();
            DataTable dt = richDac.GetRichQueryItems(helpid);

            foreach (DataRow dr in dt.Rows)
            {
                JObject jo = ControlCreator.GetControl(dr);
                arr.Add(jo);
            }
            return arr;
        }

        public JObject GetQueryFilter(string helpid,string ocode,string logid)
        {
            JObject obj = new JObject();

            JObject jo = null;
            string str = richDac.GetQueryFilter(helpid);
            if (!string.IsNullOrWhiteSpace(str))
            {
                jo = JsonConvert.DeserializeObject<JObject>(str);
                return jo;
            }
            else
            {
                DataTable dt = richDac.GetRichQueryUIInfo(helpid, ocode, logid);
                foreach (DataRow dr in dt.Rows)
                {
                    string field = dr["tablename"].ToString() + "." + dr["field"].ToString();
                    object val = jo[field];
                    if (jo != null && val != null && !string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        obj[field] = val.ToString();
                    }
                    else
                    {
                        obj[field] = (dr["defaultdata"] == null || dr["defaultdata"] == DBNull.Value) ? string.Empty : dr["defaultdata"].ToString();
                    }
                }

                return obj;
            }

           
        }


        public JObject GetListExtendInfo(string code)
        {
            JObject obj = new JObject();
            DataTable dt = richDac.GetListExtendInfo(code);

            if (dt.Rows.Count > 0)
            {
                obj["extfields"] = dt.Rows[0]["list_extfields"].ToString();
                obj["extheader"] = dt.Rows[0]["list_extheader"].ToString();
            }
            return obj;
        }
        
    }

    //树列表节点信息
    class TreeListJSON : TreeJSONBase
    {
        public string row { get; set; }
    }

    public class TreeListBuilder : ExtJsTreeBuilderBase
    {

        public string ID { get; set; }
        public string Text { get; set; }
        

        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            TreeListJSON node = new TreeListJSON();

            node.id = dr[this.ID].ToString();
            node.text = dr[this.Text].ToString();
            node.row = JsonConvert.SerializeObject(dr.ToJObject());          

            return node;
        }
    }

    //查询属性树节点信息
    class QueryPropertyTreeJSON : TreeJSONBase
    {
        public virtual string treesearchkey { get; set; }
        public virtual string treerefkey { get; set; }        
    }

    public class QueryPropertyTreeBuilder : ExtJsTreeBuilderBase
    {

        public string ID { get; set; }
        public string Text { get; set; }
        public string TreeSearchKey { get; set; }
        public string TreeRefKey { get; set; }

        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            QueryPropertyTreeJSON node = new QueryPropertyTreeJSON();

            node.id = dr[this.ID].ToString();
            node.text = dr[this.Text].ToString();

            if (!string.IsNullOrWhiteSpace(this.TreeSearchKey))
            {
                node.treesearchkey = dr[this.TreeSearchKey].ToString();
            }
            if (!string.IsNullOrWhiteSpace(this.TreeRefKey))
            {
                node.treerefkey = this.TreeRefKey; //dr[this.TreeRefKey].ToString();
            }
            //node.hrefTarget = dr["url"].ToString();
            //node.leaf = (dr["functionnode_flag"].ToString() == "1") ? true : false;

            return node;
        }
    }

    public class ControlCreator
    {
        public ControlCreator()
        {
 
        }

        public static JObject GetControl(DataRow dr)
        {
            string ctltype = dr["controltype"].ToString();
            JObject jo;
            switch (ctltype)
            {
                case "ngText" : jo = ControlCreator.CreatText(dr);
                    break;
                case "ngNumber": jo = ControlCreator.CreatNumber(dr);
                    break;
                case "ngDate": jo = ControlCreator.CreatDate(dr);
                    break;
                case "ngDateTime": jo = ControlCreator.CreatDateTime(dr);
                    break;
                case "ngComboBox": jo = ControlCreator.CreatComboBox(dr);
                    break;
                case "ngCommonHelp": jo = ControlCreator.CreatCommonHelp(dr);
                    break;
                default:jo = ControlCreator.CreatText(dr);
                    break;
            }
            
            return jo;
        }

        public static JObject CreatText(DataRow dr)
        {
            JObject jo = new JObject();

            jo["xtype"] = "ngText";
            jo["fieldLabel"] = dr["fname_chn"].ToString();
            jo["name"] = dr["tablename"].ToString() + "." + dr["field"].ToString();

            return jo;
        }

        public static JObject CreatNumber(DataRow dr)
        {
            JObject jo = new JObject();

            jo["xtype"] = "ngNumber";
            jo["fieldLabel"] = dr["fname_chn"].ToString();
            jo["name"] = dr["tablename"].ToString() + "." + dr["field"].ToString();

            return jo;
        }

        public static JObject CreatDate(DataRow dr)
        {
            JObject jo = new JObject();

            jo["xtype"] = "ngDate";
            jo["fieldLabel"] = dr["fname_chn"].ToString();
            jo["name"] = dr["tablename"].ToString() + "." + dr["field"].ToString();
            jo["format"] = "Y-m-d";
            return jo;
        }

        public static JObject CreatDateTime(DataRow dr)
        {
            JObject jo = new JObject();

            jo["xtype"] = "ngDateTime";
            jo["fieldLabel"] = dr["fname_chn"].ToString();
            jo["name"] = dr["tablename"].ToString() + "." + dr["field"].ToString();

            return jo;
        }

        public static JObject CreatComboBox(DataRow dr)
        {
            JObject jo = new JObject();

            jo["xtype"] = "ngComboBox";
            jo["fieldLabel"] = dr["fname_chn"].ToString();
            jo["name"] = dr["tablename"].ToString() + "." + dr["field"].ToString();

            jo["valueField"] =  dr["helpvaluefield"].ToString();
            jo["displayField"] = dr["helpdisplayfield"].ToString();
            jo["helpid"] = dr["ctlhelpid"].ToString();
            jo["queryMode"] = "remote";

            return jo;
        }

        public static JObject CreatCommonHelp(DataRow dr)
        {
            JObject jo = new JObject();

            jo["xtype"] = "ngCommonHelp";
            jo["fieldLabel"] = dr["fname_chn"].ToString();
            jo["name"] = dr["tablename"].ToString() + "." + dr["field"].ToString();

            jo["ORMMode"] = false;
            //jo["valueField"] = dr["tablename"].ToString() + "." + dr["helpvaluefield"].ToString();
            //jo["displayField"] = dr["tablename"].ToString() + "." + dr["helpdisplayfield"].ToString();
            jo["valueField"] = dr["helpvaluefield"].ToString();
            jo["displayField"] = dr["helpdisplayfield"].ToString();
            jo["helpid"] = dr["ctlhelpid"].ToString();           


            return jo;
        }

    }

}
