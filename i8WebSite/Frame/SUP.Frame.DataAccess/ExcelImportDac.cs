using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NG3.Data.Service;
using SUP.Common.Base;
using SUP.Common.DataAccess;
using SUP.Common.DataEntity;

namespace SUP.Frame.DataAccess
{
    public class ExcelImportDac
    {
        RichHelpDac helpdac = new RichHelpDac();

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> LoadMenu(string nodeid, string id)
        {
            string sql = string.Empty;
            int count = 0;
            if ("root" == nodeid) //首次加载
            {
                IList<TreeJSONBase> list = new List<TreeJSONBase>();

                TreeJSONBase bstree = new TreeJSONBase();
                bstree.id = "业务单据";
                bstree.text = "业务单据";
                bstree.expanded = true;
                bstree.leaf = false;
                bstree.children = new List<TreeJSONBase>();
                if (id != string.Empty && id != null)
                    sql = string.Format("select id,name from  billplugininfo  where type like '%{0}%' and id='{1}'", bstree.id, id);
                else
                    sql = string.Format("select id,name from  billplugininfo  where type like '%{0}%'", bstree.id);
                DataTable dt = DbHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TreeJSONBase subtree = new TreeJSONBase();
                        subtree.id = dr["id"].ToString();
                        subtree.text = dr["name"].ToString();
                        subtree.leaf = true;

                        bstree.children.Add(subtree);
                    }
                    list.Add(bstree);
                }

                if (id != string.Empty && id != null)
                {
                    sql = string.Format("select id,name from  billplugininfo  where type like '基础数据' and id='{0}'", id);
                    DataTable dtBase = DbHelper.GetDataTable(sql);
                    if (dtBase.Rows.Count > 0)
                    {
                        count++;
                        TreeJSONBase batree = new TreeJSONBase();
                        batree.id = "基础数据";
                        batree.text = "基础数据";
                        batree.leaf = false;
                        list.Add(batree);
                        return list;
                    }
                }
                else
                {
                    TreeJSONBase batree = new TreeJSONBase();
                    batree.id = "基础数据";
                    batree.text = "基础数据";
                    batree.leaf = false;
                    list.Add(batree);
                }

                if (id != string.Empty && id != null)
                {
                    sql = string.Format("select id,name from  billplugininfo  where type  is null and id='{0}'", id);
                    DataTable dtOther = DbHelper.GetDataTable(sql);
                    if (dtOther.Rows.Count > 0)
                    {
                        TreeJSONBase otherTree = new TreeJSONBase();
                        otherTree.id = "其他";
                        otherTree.text = "其他";
                        otherTree.leaf = false;
                        list.Add(otherTree);
                    }
                }
                else
                {
                    TreeJSONBase otherTree = new TreeJSONBase();
                    otherTree.id = "其他";
                    otherTree.text = "其他";
                    otherTree.leaf = false;
                    list.Add(otherTree);
                }
               
                return list;

            }
            else
            {
                if (nodeid.Equals("其他"))
                {
                    if (id != string.Empty && id != null)
                        sql = "select id,name from  billplugininfo  where type  is null and id='" + id + "'";
                    else
                        sql = "select id,name from  billplugininfo  where type  is null ";
                }
                else
                {
                    if (id != string.Empty && id != null)
                        sql = string.Format("select id,name from  billplugininfo  where type like '%{0}%' and id='{1}'", nodeid, id);
                    else
                        sql = string.Format("select id,name from  billplugininfo  where type like '%{0}%' ", nodeid);
                }
                DataTable dt = DbHelper.GetDataTable(sql);

                IList<TreeJSONBase> list = new List<TreeJSONBase>();

                foreach (DataRow dr in dt.Rows)
                {
                    TreeJSONBase subtree = new TreeJSONBase();
                    subtree.id = dr["id"].ToString();
                    subtree.text = dr["name"].ToString();
                    subtree.leaf = true;

                    list.Add(subtree);
                }
                return list;
            }

        }

        /// <summary>
        /// 获取formPanel界面中元素数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetFormData(string id)
        {
            string sql = string.Format("select name, multipleSheet from billplugininfo where id = '{0}'", id);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetTemplate(string id)
        {
            DataTable template = new DataTable();

            string sqlText = string.Format("select template from billplugininfo where id = '{0}'", id);
            DataTable source = DbHelper.GetDataTable(sqlText);

            XmlDocument doc = new XmlDocument();
            object obj = source.Rows[0]["template"];
            if (obj != null && obj != DBNull.Value)
            {
                byte[] bytes = (byte[])obj;
                MemoryStream stream2 = new MemoryStream(bytes);
                XmlReader xmlreader = new XmlTextReader(stream2);
                doc.Load(xmlreader);
                template.Columns.Add("id");
                template.Columns.Add("name");
                template.Columns.Add("type");
                template.Columns.Add("remark");
                template.Columns.Add("tablename");
                template.Columns.Add("filedname");
                template.Columns.Add("datatype");
                template.Columns.Add("decimalPrecision");
                template.Columns.Add("dataformat");
                template.Columns.Add("primarykey");
                template.Columns.Add("mustinput");
                template.Columns.Add("helptype");                
                template.Columns.Add("refer");
                template.Columns.Add("tabledepiction");
                template.Columns.Add("check");
                XmlNode node = doc.SelectSingleNode("Template");
                foreach (XmlNode xn in node.ChildNodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    DataRow dr = template.NewRow();
                    dr["id"] = xe.GetAttribute("id");
                    dr["name"] = xe.GetAttribute("name");
                    dr["type"] = xe.GetAttribute("type");
                    dr["remark"] = xe.GetAttribute("remark");
                    dr["tablename"] = xe.GetAttribute("tablename");
                    dr["filedname"] = xe.GetAttribute("filedname");
                    dr["datatype"] = xe.GetAttribute("datatype");
                    dr["decimalPrecision"] = xe.GetAttribute("decimalPrecision");
                    dr["dataformat"] = xe.GetAttribute("dataformat");//excel数据格式                    
                    dr["primarykey"] = xe.GetAttribute("primarykey");
                    dr["mustinput"] = xe.GetAttribute("mustinput");
                    dr["helptype"] = (xe.GetAttribute("helptype") == null) ? (string.Empty) : (xe.GetAttribute("helptype"));//通用帮助节点
                    dr["refer"] = (xe.GetAttribute("refer") == null) ? (string.Empty) : (xe.GetAttribute("refer"));//相关导出列
                    dr["tabledepiction"] = (xe.GetAttribute("tabledepiction") == null) ? (string.Empty) : (xe.GetAttribute("tabledepiction"));//相关导出列
                    dr["check"] = dr["mustinput"];
                    template.Rows.Add(dr);
                }
            }
            return template;
        }

        /// <summary>
        /// 得到帮助字段的CodeField,和NameField
        /// </summary>
        /// <param name="helpId"></param>
        /// <returns></returns>
        public KeyValuePair<string, string> GetHelpData(string helpId)
        {
            CommonHelpEntity helper = helpdac.GetHelpItem(helpId);
            KeyValuePair<string, string> dic = new KeyValuePair<string, string>(helper.CodeField, helper.NameField);

            return dic;
        }

        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetPluginInfo(string id)
        {
            string sqlText = string.Format("select pluginname from billplugininfo where id = '{0}'", id);
            return DbHelper.GetString(sqlText);
        }

        /// <summary>
        /// 获取表结构信息
        /// </summary>
        /// <param name="tname">表名</param>
        /// <returns></returns>
        public DataTable GetTableInfo(string tname)
        {
            return DbHelper.GetDataTable(string.Format("select * from {0} where 1=0 ", tname));
        }
    }

}
