using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.Rule
{
    /*----自定义信息升级合并类---
       1、容器添加新控件
       2、控件从容器p1移动到容器p2(p1和p2绑定同一张表)，这种情况应不应该处理？
       3、添加新的顶层容器，需归并(from,fieldSetForm,grid),否则页面的脚本会报错
       4、fieldSetForm添加了新的fieldSet，需归并
       5、from变为fieldSetForm，或者fieldSetForm变成form，因变动比较大，自动归并蛮困难,建议重新设计自定义界面
       6、之前没有fieldSetForm或grid  ，新版本添加了fieldSetForm或grid   
    */
    public class Merge
    {
        private StringBuilder msg = new StringBuilder();
        private List<string> canNotOverrideFields = new List<string>();//form列不可以覆盖的属性
        private List<string> canNotOverrideColumns = new List<string>();//grid列不可以覆盖的属性

        private List<string> canNotOverrideFormPropertys = new List<string>();//Form不可以覆盖的属性
        private List<string> canNotOverrideFieldSetFormPropertys = new List<string>();//FieldSetForm不可以覆盖的属性
        private List<string> canNotOverrideFieldSetPropertys = new List<string>();//FieldSetForm不可以覆盖的属性

        public List<string> CanNotOverrideFields
        {
            get
            {
                if (canNotOverrideFields.Count == 0)
                {
                    canNotOverrideFields.Add("fieldLabel");
                    canNotOverrideFields.Add("labelWidth");
                    canNotOverrideFields.Add("name");
                    canNotOverrideFields.Add("mustInput");
                    canNotOverrideFields.Add("readOnly");
                    canNotOverrideFields.Add("hidden");
                    canNotOverrideFields.Add("colspan");
                    canNotOverrideFields.Add("decimalSeparator");
                    canNotOverrideFields.Add("matchFieldWidth");
                }
                return canNotOverrideFields;
            }

        }

        public List<string> CanNotOverrideColumns
        {
            get
            {
                if (canNotOverrideColumns.Count == 0)
                {
                    canNotOverrideColumns.Add("width");
                    canNotOverrideColumns.Add("header");
                    canNotOverrideColumns.Add("hidden");
                    canNotOverrideColumns.Add("readOnly");
                    canNotOverrideFields.Add("mustInput");
                }
                return canNotOverrideColumns;
            }

        }
        
        public List<string> CanNotOverrideFormPropertys
        {
            get
            {
                if (canNotOverrideFormPropertys.Count == 0)
                {
                    canNotOverrideFormPropertys.Add("fields");
                    canNotOverrideFormPropertys.Add("labelWidth");
                    canNotOverrideFormPropertys.Add("columnsPerRow");
                }
                return canNotOverrideFormPropertys;
            }
        }

        public List<string> CanNotOverrideFieldSetFormPropertys
        {
            get
            {
                if (canNotOverrideFieldSetFormPropertys.Count == 0)
                {
                    canNotOverrideFieldSetFormPropertys.Add("fieldSets");
                    canNotOverrideFieldSetFormPropertys.Add("labelWidth");
                }
                return canNotOverrideFieldSetFormPropertys;
            }


        }

        public List<string> CanNotOverrideFieldSetPropertys
        {
            get
            {
                if (canNotOverrideFieldSetPropertys.Count == 0)
                {
                    canNotOverrideFieldSetPropertys.Add("allfields");
                    canNotOverrideFieldSetPropertys.Add("labelWidth");
                    canNotOverrideFieldSetPropertys.Add("columnsPerRow");
                }
                return canNotOverrideFieldSetPropertys;
            }

        }


        public StringBuilder Msg
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        public Merge()
        {

        }
        

        private DataTable GetTable()
        {
            DataColumn col = new DataColumn("table", typeof(string));//表名           
            DataColumn col1 = new DataColumn("fieldname", typeof(string));//字段名
            DataColumn col2 = new DataColumn("containerid", typeof(string));//容器id
            DataColumn col3 = new DataColumn("containertype", typeof(string));//容器类型
            DataColumn col4 = new DataColumn("ismovefield", typeof(string));//是否移动列，移动到新panel中或者移动到已经存在的另一个panel中

            DataTable fieldInfoDt = new DataTable();
            fieldInfoDt.Columns.Add(col);
            fieldInfoDt.Columns.Add(col1);
            fieldInfoDt.Columns.Add(col2);
            fieldInfoDt.Columns.Add(col3);
            fieldInfoDt.Columns.Add(col4);
            return fieldInfoDt;
        }

        public void AddField(DataTable fieldInfoDt, string tablename, string fieldName, string containerID, string containerType)
        {
            DataRow dr = fieldInfoDt.NewRow();

            dr.BeginEdit();
            dr["table"] = tablename;
            dr["fieldname"] = fieldName;
            dr["containerid"] = containerID;
            dr["containertype"] = containerType;
            dr["ismovefield"] = "0";
            dr.EndEdit();

            fieldInfoDt.Rows.Add(dr);
        }

        public DataTable InitialNewFieldList(JObject newFormJa, JObject newFieldsetFormJa)
        {
            //新字段列表
            DataTable newFieldList = GetTable();

            if (newFormJa != null)
            {
                foreach (var item in newFormJa)
                {
                    JObject jo = item.Value as JObject;             

                    if (jo["bindtable"] != null)
                    {
                        string containerID = jo["id"].ToString();
                        string table = jo["bindtable"].ToString();
                        JArray newfileds = jo["fields"] as JArray;
                        foreach (JObject field in newfileds)
                        {
                            field.Add("from", "devdefine");//字段来源
                            if (field["name"] != null)
                            {
                                AddField(newFieldList, table, field["name"].ToString(), containerID, ContainerType.Form);
                            }
                        }
                    }                   
                }
            }

            if (newFieldsetFormJa != null)
            {
                foreach (var item in newFieldsetFormJa)
                {
                    JObject jo = item.Value as JObject;

                    if (jo["bindtable"] != null)
                    {
                        string containerID = jo["id"].ToString();
                        string table = jo["bindtable"].ToString();
                        JArray fieldSets = jo["fieldSets"] as JArray;
                        foreach (JObject fieldset in fieldSets)
                        {
                            //string conID = containerID + "#" + fieldset["itemId"].ToString();
                            JArray fields = fieldset["allfields"] as JArray;
                            foreach (JObject field in fields)
                            {
                                field.Add("from", "devdefine");//字段来源
                                if (field["name"] != null)//container装了一堆隐藏控件的
                                {
                                    AddField(newFieldList, table, field["name"].ToString(), containerID, ContainerType.FieldSet);
                                }
                            }
                        }
                    }
                }
            }

            return newFieldList;

        }

        public DataTable InitialIndividualFieldList(JObject individualFormJo, JObject indvidualFieldsetFormJo)
        {
            //模板里的字段列表
            DataTable indFieldList = GetTable();
            if (individualFormJo != null)
            {
                foreach (var item in individualFormJo)
                {
                    JObject jo = item.Value as JObject;
                    if (jo["bindtable"] != null)
                    {
                        string containerID = jo["id"].ToString();
                        string table = jo["bindtable"].ToString();
                        JArray fileds = jo["fields"] as JArray;
                        foreach (JObject field in fileds)
                        {
                            if (field["name"] != null)
                            {
                                AddField(indFieldList, table, field["name"].ToString(), containerID, ContainerType.Form);
                            }
                        }

                    }

                }
            }

            if (indvidualFieldsetFormJo != null)
            {
                foreach (var item in indvidualFieldsetFormJo)
                {
                    JObject jo = item.Value as JObject;
                    if (jo["bindtable"] != null)
                    {
                        string containerID = jo["id"].ToString();
                        string table = jo["bindtable"].ToString();
                        JArray fieldSets = jo["fieldSets"] as JArray;
                        foreach (JObject fieldset in fieldSets)
                        {
                            //string conID = containerID + "#" + fieldset["itemId"].ToString();
                            JArray fields = fieldset["allfields"] as JArray;
                            foreach (JObject field in fields)
                            {
                                if (field["name"] != null)
                                {
                                    AddField(indFieldList, table, field["name"].ToString(), containerID, ContainerType.FieldSet);
                                }
                            }
                        }
                    }
                }
            }

            return indFieldList;

        }


        public  void MergeForm(JObject newFormJa, JObject indvidualFormJo, DataTable indList)
        {
            if (newFormJa != null)
            {
                foreach (var item in newFormJa)
                {
                    JObject jo = item.Value as JObject;

                    if (jo["id"] != null)
                    {
                        string containerID = jo["id"].ToString();
                        string table = jo["bindtable"].ToString();
                        JObject individualContainer = indvidualFormJo[containerID] as JObject;
                        if (individualContainer != null)
                        {
                            JArray newfileds = jo["fields"] as JArray;
                            JArray individualFields = individualContainer["fields"] as JArray;
                            List<JObject> ls = new List<JObject>();
                            foreach (JObject field in newfileds)
                            {
                                string fieldID = field["name"] == null ? string.Empty : field["name"].ToString();

                                if (string.IsNullOrWhiteSpace(fieldID))
                                {
                                    msg.AppendLine(string.Format("容器【{0}】xtype【{1}】的name属性为空", containerID, field["xtype"].ToString()));
                                    continue;
                                }

                                JObject findedIndividualJo = null;
                                if (!IsExists(individualFields, fieldID,ref findedIndividualJo))
                                {
                                    //ls.Add(field);
                                    //msg.AppendLine(string.Format("容器【{0}】新增字段【{1}】", containerID, fieldID));

                                    
                                    DataRow[] drs = indList.Select(string.Format("table='{0}' and fieldname='{1}'", table, fieldID));//表名一样
                                    if (drs.Length == 0)//新字段
                                    {
                                        //field.Add("hidden", true);//隐藏
                                        ls.Add(field);
                                        msg.AppendLine(string.Format("容器【{0}】新增字段【{1}】", containerID, fieldID));
                                    }
                                    //else if (drs.Length > 0)//同一张表放在两个panel，升级全移动到一个panel中了，有问题，不处理了
                                    //{
                                    //    if (drs[0]["ismovefield"].ToString() == "1")//是移动字段，得合并进新容器中
                                    //    {
                                    //        //field.Add("hidden", true);//隐藏
                                    //        ls.Add(field);
                                    //        msg.AppendLine(string.Format("字段【{0}】移动到容器【{1}】", fieldID, containerID));
                                    //    }
                                    //}
                                }
                                else
                                {
                                    MergeFormField(findedIndividualJo, field,msg,containerID);//字段内属性合并
                                }
                            }

                            //新加的放入自定义的容器中
                            foreach (var f in ls)
                            {
                                individualFields.Add(f);
                            }

                            //grid的配置信息升级
                            MergeGridConfig(jo, individualContainer, msg, containerID);
                        }
                        else
                        {
                            //indvidualFormJo.Add(jo);//新版本多了个form,归并
                            indvidualFormJo.Add(item.Key,jo);
                            msg.AppendLine(string.Format("form新增容器【{0}】", item.Key));
                        }
                    }
                }
            }

            //自定义中已经被移动的，需要删除处理， 被移除的则忽略不处理

        }

        /// <summary>
        /// 同步字段的相关属性，放开给用户自定义的属性不能覆盖
        /// </summary>
        private void MergeFormField(JObject individualField, JObject newField,StringBuilder msg,string containerID)
        {
            foreach (var item in newField)
            {
                string propertyName = item.Key;
                if (!CanNotOverrideFields.Contains(propertyName))
                {
                    MergeField(individualField, newField, propertyName,msg,containerID);
                }
            }
        }

        private void MergeField(JObject individualField, JObject newField, string propertyName, StringBuilder msg, string containerID)
        {
            if (individualField[propertyName] == null)
            {
                individualField.Add(propertyName, newField[propertyName]);
                msg.AppendLine(string.Format("容器【{0}】的字段【{1}】添加新属性【{2}:{3}】", containerID, newField["name"].ToString(), 
                                       propertyName,newField[propertyName]));
            }
            else
            {
                if (individualField[propertyName].ToString() != newField[propertyName].ToString())
                {
                    if (newField["name"] != null)
                    {
                        msg.AppendLine(string.Format("容器【{0}】的字段【{1}】的属性【{2}:{3}】变为【{2}:{4}】", containerID, newField["name"].ToString(),
                                       propertyName, individualField[propertyName], newField[propertyName]));
                    }
                    else
                    {
                        msg.AppendLine(string.Format("容器【{0}】的字段【{1}】的属性【{2}:{3}】变为【{2}:{4}】", containerID, "noName",
                                      propertyName, individualField[propertyName], newField[propertyName]));
                    }
                    individualField[propertyName] = newField[propertyName];                  
                }
            }
        }

        private void MergeGridColumn(JObject individualColumn, JObject newColumn, StringBuilder msg, string containerID)
        {
            foreach (var item in newColumn)
            {
                string propertyName = item.Key;
                if (!CanNotOverrideColumns.Contains(propertyName))
                {
                    MergeColumn(individualColumn, newColumn, propertyName,msg,containerID);
                }
            }
        }

        private void MergeColumn(JObject individualColumn, JObject newColumn, string propertyName, StringBuilder msg, string containerID)
        {
            try
            {
                if (individualColumn[propertyName] == null)
                {
                    individualColumn.Add(propertyName, newColumn[propertyName]);

                    string colName = string.Empty;
                    if (newColumn["dataIndex"] == null) {
                        colName = newColumn["header"].ToString();//行号没有dataIndex
                    }
                    msg.AppendLine(string.Format("容器【{0}】的字段【{1}】添加新属性【{2}:{3}】", containerID, colName,
                                         propertyName, newColumn[propertyName]));
                }
                else
                {
                    if (individualColumn[propertyName].ToString() != newColumn[propertyName].ToString())
                    {
                        string colName = string.Empty;
                        if (newColumn["dataIndex"] == null)
                        {
                            colName = newColumn["header"].ToString();
                        }
                        msg.AppendLine(string.Format("容器【{0}】的字段【{1}】的属性【{2}:{3}】变为【{2}:{4}】", containerID, colName,
                                         propertyName, individualColumn[propertyName], newColumn[propertyName]));
                        individualColumn[propertyName] = newColumn[propertyName];
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
         
        }

        //更新Form的配置信息
        private void MergeFormConfig(JObject newForm, JObject indvidualForm, StringBuilder msg, string containerID)
        {
            foreach (var item in newForm)
            {
                string propertyName = item.Key;
                if (!CanNotOverrideFormPropertys.Contains(propertyName))//字段信息不更新
                {
                    if (indvidualForm[propertyName] == null)
                    {
                        indvidualForm.Add(propertyName, item.Value);
                        msg.AppendLine(string.Format("Form容器【{0}】添加新属性【{1}:{2}】", containerID,
                                   propertyName, item.Value));
                    }
                    else
                    {
                        if (indvidualForm[propertyName].ToString() != item.Value.ToString())
                        {
                            indvidualForm[propertyName] = item.Value;
                            msg.AppendLine(string.Format("form容器【{0}】的属性【{1}:{2}】变为【{1}:{3}】", containerID,
                                    propertyName, indvidualForm[propertyName], item.Value));
                        }
                    }
                }
            }
        }

        //更新FieldSetForm的配置信息
        private void MergeFieldSetFormConfig(JObject newForm, JObject indvidualForm, StringBuilder msg, string containerID)
        {
            foreach (var item in newForm)
            {
                string propertyName = item.Key;
                if (!CanNotOverrideFieldSetFormPropertys.Contains(propertyName))//fieldSets信息不更新,labelWidth不更新
                {
                    if (indvidualForm[propertyName] == null)
                    {
                        indvidualForm.Add(propertyName, item.Value);
                        msg.AppendLine(string.Format("FieldSetForm容器【{0}】添加新属性【{1}:{2}】", containerID,
                                   propertyName, item.Value));
                    }
                    else
                    {
                        if (indvidualForm[propertyName].ToString() != item.Value.ToString())
                        {
                            indvidualForm[propertyName] = item.Value;
                            msg.AppendLine(string.Format("FieldSetForm容器【{0}】的属性【{1}:{2}】变为【{1}:{3}】", containerID,
                                    propertyName, indvidualForm[propertyName], item.Value));
                        }
                    }
                }
            }
        }

        //更新FieldSet的配置信息
        private void MergeFieldSetConfig(JObject newFieldSet, JObject indvidualFieldSet, StringBuilder msg, string containerID)
        {
            foreach (var item in newFieldSet)
            {
                string propertyName = item.Key;
                if (!CanNotOverrideFieldSetPropertys.Contains(propertyName))//allfields信息不更新
                {
                    if (indvidualFieldSet[propertyName] == null)
                    {
                        indvidualFieldSet.Add(propertyName, item.Value);
                        msg.AppendLine(string.Format("FieldSet容器【{0}】添加新属性【{1}:{2}】", containerID,
                                   propertyName, item.Value));
                    }
                    else
                    {
                        if (indvidualFieldSet[propertyName].ToString() != item.Value.ToString())
                        {
                            indvidualFieldSet[propertyName] = item.Value;
                            msg.AppendLine(string.Format("FieldSet容器【{0}】的属性【{1}:{2}】变为【{1}:{3}】", containerID,
                                    propertyName, indvidualFieldSet[propertyName], item.Value));
                        }
                    }
                }
            }
        }

        //合并grid的配置信息
        private void MergeGridConfig(JObject newGrid, JObject indvidualGrid, StringBuilder msg, string containerID)
        {
            foreach (var item in newGrid)
            {
                string propertyName = item.Key;
                if (propertyName != "columns")//列信息不更新
                {
                    if (indvidualGrid[propertyName] == null)
                    {
                        indvidualGrid.Add(propertyName, item.Value);
                        msg.AppendLine(string.Format("grid容器【{0}】添加新属性【{1}:{2}】", containerID,
                                   propertyName, item.Value));
                    }
                    else
                    {
                        if (indvidualGrid[propertyName].ToString() != item.Value.ToString())
                        {
                            indvidualGrid[propertyName] = item.Value;
                            msg.AppendLine(string.Format("grid容器【{0}】的属性【{1}:{2}】变为【{1}:{3}】", containerID,
                                    propertyName, indvidualGrid[propertyName], item.Value));
                        }
                    }
                }
            }
        }

        //合并tab的配置信息
        private void MergeTabConfig(JObject newTab, JObject indvidualTab, StringBuilder msg, string containerID)
        {
            foreach (var item in newTab)
            {
                string propertyName = item.Key;
                if (propertyName != "items")//items信息不更新
                {
                    if (indvidualTab[propertyName] == null)
                    {
                        indvidualTab.Add(propertyName, item.Value);
                        msg.AppendLine(string.Format("Tab容器【{0}】添加新属性【{1}:{2}】", containerID,
                                   propertyName, item.Value));
                    }
                    else
                    {
                        if (indvidualTab[propertyName].ToString() != item.Value.ToString())
                        {
                            indvidualTab[propertyName] = item.Value;
                            msg.AppendLine(string.Format("Tab容器【{0}】的属性【{1}:{2}】变为【{1}:{3}】", containerID,
                                    propertyName, indvidualTab[propertyName], item.Value));
                        }
                    }
                }
            }
        }

        public  void MergeGrid(JObject newGridJa, JObject indvidualGridJo)
        {
            if (newGridJa == null) return;

            foreach (var item in newGridJa)
            {
                JObject jo = item.Value as JObject;

                if (jo["id"] != null)
                {
                    string containerID = jo["id"].ToString();
                    JObject individualContainer = indvidualGridJo[containerID] as JObject;
                    if (individualContainer != null)
                    {
                        JArray newColumns = jo["columns"] as JArray;
                        JArray individualColumns = individualContainer["columns"] as JArray;
                        List<JObject> ls = new List<JObject>();
                        foreach (JObject column in newColumns)
                        {
                            string fieldID = column["dataIndex"] == null ? string.Empty : column["dataIndex"].ToString();
                            JObject findedIndividualCol = null;
                            if (!IsColumnExists(individualColumns, fieldID,ref findedIndividualCol))
                            {
                                //column.Add("hidden", true);//隐藏
                                column.Add("from", "devdefine");//字段来源
                                ls.Add(column);
                                msg.AppendLine(string.Format("grid【{0}】新增字段【{1}】", containerID, fieldID));
                            }
                            else
                            {
                                MergeGridColumn(findedIndividualCol, column,msg,containerID);//合并自定义列属性
                            }
                        }

                        //新加的放入自定义的容器中
                        foreach (var f in ls)
                        {
                            individualColumns.Add(f);
                        }
                        //grid的配置信息升级
                        MergeGridConfig(jo, individualContainer, msg, containerID);
                    }
                    else
                    {
                        //设置字段来源
                        JArray newColumns = jo["columns"] as JArray;
                        foreach (JObject column in newColumns)
                        {
                            column.Add("from", "devdefine");//字段来源
                        }

                        indvidualGridJo.Add(item.Key, jo);//新版本多了个grid，归并
                        msg.AppendLine(string.Format("grid新增容器【{0}】", item.Key));
                    }
                }               
            }
        }

        public void MergeTabPanel(JObject newTabJa, JObject indvidualTabJo)
        {
            if (newTabJa == null) return;

            foreach (var item in newTabJa)
            {
                JObject jo = item.Value as JObject;

                if (jo["id"] != null)
                {
                    string containerID = jo["id"].ToString();
                    JObject individualContainer = indvidualTabJo[containerID] as JObject;
                    if (individualContainer != null)
                    {
                        JArray newTabs = jo["items"] as JArray;
                        JArray individualTabs = individualContainer["items"] as JArray;
                        List<JObject> ls = new List<JObject>();
                        foreach (JObject tab in newTabs)
                        {
                            string fieldID = tab["id"] == null ? string.Empty : tab["id"].ToString();
                            JObject findedTab = null;
                            if (!IsTabExists(individualTabs, fieldID,ref findedTab))
                            {                               
                                //tab.Add("from", "devdefine");//字段来源
                                ls.Add(tab);
                                msg.AppendLine(string.Format("TabPanel【{0}】新增Tab【{1}】", containerID, fieldID));
                            }
                            else
                            {
                                //更新tab配置信息
                                MergeTabConfig(tab, findedTab, msg, fieldID);
                            }
                        }

                        //新加的放入自定义的容器中
                        foreach (var f in ls)
                        {
                            individualTabs.Add(f);
                        }
                    }
                    else
                    {
                        ////设置字段来源
                        //JArray newTabs = jo["items"] as JArray;
                        //foreach (JObject tab in newTabs)
                        //{
                        //    tab.Add("from", "devdefine");//字段来源
                        //}

                        indvidualTabJo.Add(item.Key, jo);//新版本多了个tabPanel，归并
                        msg.AppendLine(string.Format("TabPanel新增容器【{0}】", item.Key));
                    }
                }
            }
        }

        public  void MergeFieldsetForm(JObject newFieldsetFormJa, JObject indvidualFieldsetFormJo, DataTable indList)
        {
            if (newFieldsetFormJa == null) return;
            foreach (var item in newFieldsetFormJa)
            {
                JObject jo = item.Value as JObject;

                if (jo["id"] != null)
                {
                    string containerID = jo["id"].ToString();
                    string table = jo["bindtable"].ToString();
                    JObject individualContainer = indvidualFieldsetFormJo[containerID] as JObject;//找自定义方案中对应的fieldsetform
                    if (individualContainer != null)
                    {
                        JArray fieldSets = jo["fieldSets"] as JArray;
                        foreach (JObject fieldset in fieldSets)//遍历插件的fieldset
                        {
                            //if (jo["itemId"] != null)
                            if (fieldset["itemId"] != null)
                            {
                                string fieldsetID = fieldset["itemId"] == null ? string.Empty : fieldset["itemId"].ToString();
                                if (!string.IsNullOrEmpty(fieldsetID))
                                {
                                    bool find = false;//是否在用户定义的ui中找到对应的fieldsetID
                                    JArray individualfieldSets = individualContainer["fieldSets"] as JArray;
                                    foreach (JObject individualfieldset in individualfieldSets)
                                    {
                                        string itemID = individualfieldset["itemId"] == null ? string.Empty : individualfieldset["itemId"].ToString();

                                        if (fieldsetID == itemID)//找到自定义里的同一个fieldset
                                        {
                                            find = true;//找到
                                            JArray newfields = fieldset["allfields"] as JArray;
                                            JArray individualFields = individualfieldset["allfields"] as JArray;
                                            List<JObject> ls = new List<JObject>();
                                            foreach (JObject field in newfields)
                                            {
                                                string fieldID = field["name"] == null ? string.Empty : field["name"].ToString();
                                                if (string.IsNullOrWhiteSpace(fieldID))
                                                {
                                                    msg.AppendLine(string.Format("容器【{0}】xtype:【{1}】的name属性为空", containerID, field["xtype"].ToString()));
                                                    continue;
                                                }

                                                JObject findedIndividualJo = null;
                                                if (!IsExists(individualFields, fieldID, ref findedIndividualJo))//判断自定义方案中是否存在
                                                {
                                                    //ls.Add(field);
                                                    //msg.AppendLine(string.Format("fieldSet容器【{0}】新增字段【{1}】", containerID + "|" + fieldsetID, fieldID));
                                                    
                                                    DataRow[] drs = indList.Select(string.Format("table='{0}' and fieldname='{1}'", table, fieldID));
                                                    if (drs.Length == 0)//是新增字段
                                                    {
                                                        //field.Add("hidden", true);//隐藏
                                                        ls.Add(field);
                                                        msg.AppendLine(string.Format("fieldSet容器【{0}】新增字段【{1}】", containerID + "|" + fieldsetID, fieldID));
                                                    }
                                                    //else if (drs.Length > 0)  //同一张表放在两个panel，升级全移动到一个panel中了，有问题，不处理了
                                                    //{
                                                    //    if (drs[0]["ismovefield"].ToString() == "1")//是移动字段，得合并进新容器中
                                                    //    {
                                                    //        //field.Add("hidden", true);//隐藏
                                                    //        ls.Add(field);
                                                    //        msg.AppendLine(string.Format("字段【{0}】移动到fieldSet容器【{1}】", fieldID, containerID + "|" + fieldsetID));
                                                    //    }
                                                    //}
                                                }
                                                else
                                                {
                                                    MergeFormField(findedIndividualJo, field,msg,containerID);//字段内属性合并
                                                }
                                            }

                                            //新加的放入自定义的容器中
                                            foreach (var f in ls)
                                            {
                                                individualFields.Add(f);
                                            }

                                            //更新fielset基础配置信息
                                            MergeFieldSetConfig(fieldset, individualfieldset, msg, containerID + "|" + fieldsetID);
                                        }                                      
                                    }//foreach

                                    if (!find)//新版本多了个fieldset，归并
                                    {
                                        individualfieldSets.Add(fieldset);
                                        msg.AppendLine(string.Format("fieldSetForm【{0}】新增fieldSet容器【{1}】", containerID,fieldset["itemId"]));
                                    }

                                }//if
                            }//if
                            else {
                                throw new Exception("容器【"+ containerID + "】下的fieldSets没有配置itemId");
                            }
                        }//foreach
                         //更新fieldsetform配置
                        MergeFieldSetFormConfig(jo, individualContainer, msg, containerID);
                    }//if
                    else
                    {
                        indvidualFieldsetFormJo.Add(item.Key, item.Value);//新版本多了个fieldSetForm，归并
                        msg.AppendLine(string.Format("fieldSetForm新增容器【{0}】", item.Key));
                    }
                }
            }
        }


        //检测，某些字段移动到另外增加的容器中，需要从自定义方案的容器中删除这个字段
        //判断条件：table,fieldid是一样的，containerid不一样，则说明是移动的
        public void DeleteField(JObject indvidualFormJo, JObject individualFieldsetFormJo, DataTable newList, DataTable indList)
        {
            foreach (DataRow row in newList.Rows)
            {
                string table = row["table"].ToString();
                string fieldname = row["fieldname"].ToString();
                string containerID = row["containerid"].ToString();
                string containerType = row["containertype"].ToString();

                DataRow[] drs = indList.Select(string.Format("table='{0}' and fieldname='{1}'", table, fieldname));

                if (drs.Length > 0)
                {
                    string conID = drs[0]["containerid"].ToString();
                    if (containerID != conID)//如果容器不一致，得删除掉
                    {
                        string contype = drs[0]["containertype"].ToString();
                        if (contype == ContainerType.Form)
                        {
                            JArray ja = indvidualFormJo.SelectToken(conID + ".fields") as JArray;//FieldsetForm不用处理，设计器可以移动

                            JToken toDeleteJO = null;
                            foreach (var jo in ja)
                            {
                                if (jo["name"].ToString() == fieldname)
                                {
                                    drs[0]["ismovefield"] = "1";
                                    toDeleteJO = jo;
                                    break;
                                }
                            }

                            if (toDeleteJO != null)
                            {
                                ja.Remove(toDeleteJO);
                                msg.AppendLine(string.Format("字段【{0}从容器【1】移除", fieldname, conID));

                            }

                        }
                    }
                }
            }         
        }

        //判断自定义中对应的容器内是否存在某个控件
        public bool IsExists(JArray individualFields, string fieldID, ref JObject findedJo)
        {
            bool find = false;
            foreach (JObject indField in individualFields)
            {
                string indfieldID = indField["name"] == null ? string.Empty : indField["name"].ToString();
                if (fieldID == indfieldID)
                {
                    findedJo = indField;
                    find = true;
                    break;
                }
            }

            return find;

        }

        //判断自定义中对应的grid内是否存在某列
        public bool IsColumnExists(JArray individualFields, string fieldID, ref JObject findedJo)
        {
            bool find = false;
            foreach (JObject indField in individualFields)
            {
                string indfieldID = indField["dataIndex"] == null ? string.Empty : indField["dataIndex"].ToString();
                if (fieldID == indfieldID)
                {
                    findedJo = indField;
                    find = true;
                    break;
                }
            }

            return find;

        }

        //判断自定义中对应的tabpanel内是否存在某tab
        public bool IsTabExists(JArray individualTabs, string fieldID, ref JObject findedTab)
        {
            bool find = false;
            foreach (JObject tab in individualTabs)
            {
                string indfieldID = tab["id"] == null ? string.Empty : tab["id"].ToString();
                if (fieldID == indfieldID)
                {
                    findedTab = tab;
                    find = true;
                    break;
                }
            }

            return find;

        }

        //去掉注释信息
        public string DeleteComments(string s)
        {
            if (s == null) return s;
            int pos = s.IndexOf("//");
            if (pos < 0) return s;
            return s.Substring(0, pos);
        }


        //新版本的模板
        public string GetNewFromPlugin(string file)
        {

            StringBuilder sb = new StringBuilder();
            //FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            //StreamReader sr = new StreamReader(fs, Encoding.UTF8)

            //int lines = 0;//总行数
            //using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            //{
            //    lines = sr.ReadToEnd().Split('\n').Length;//计算总行数
            //}

            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                //int currentLine = 0;
                string sline = sr.ReadLine();
                //while(currentLine < lines)
                while (sr.Peek() > -1)
                {
                    sb.Append(DeleteComments(sline));//去掉注释
                    sline = sr.ReadLine();
                    //currentLine++;
                }
            }

            string s = sb.ToString();
            int begin = s.IndexOf("individualConfigInfo");
            string sub = s.Substring(begin);
            string sub1 = sub.Substring(sub.IndexOf('{'));
            return sub1;
            
        }

    }
    

    public class ContainerType
    {
        public static readonly string Form = "form";
        public static readonly string FieldSet = "fieldset";
    }


    //模板检测类
    public class Checker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFormJa">from节点</param>
        /// <param name="newFieldsetFormJa">fieldSetForm节点</param>
        /// <param name="newGridJa">grid节点</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Check(JObject newFormJa, JObject newFieldsetFormJa, JObject newGridJa,JObject newTabJa, StringBuilder errMsg)
        {
            //errMsg = "\r\n";
            bool flag = true;
            if (newFormJa != null)
            {
                //errMsg += "开始检测form节点：" + "\r\n";
                StringBuilder sb = new StringBuilder();             
                foreach (var item in newFormJa)
                {
                    JObject jo = item.Value as JObject;

                    if (jo.First == null) continue;//form,grid里面是空的

                    if (jo["id"] == null)
                    {
                        //errMsg += item.Key + "无id属性,顶层容器请设置id" + "\r\n";
                        sb.AppendLine(item.Key + "无id属性,顶层容器请设置id");
                        flag = false;                       
                    }
                    if (jo["bindtable"] == null)
                    {
                        //errMsg += item.Key + "无bindtable属性,未绑定表" + "\r\n";
                        sb.AppendLine(item.Key + "无bindtable属性,未绑定表");
                        flag = false;                      
                    }
                    if (jo["desTitle"] == null)
                    {
                        //errMsg += item.Key + "无desTitle属性,顶层容器请设置desTitle" + "\r\n";
                        sb.AppendLine(item.Key + "无desTitle属性,顶层容器请设置desTitle");
                        flag = false;
                    }
                }

                if (sb.Length > 0)
                {
                    errMsg.AppendLine();
                    errMsg.AppendLine("开始检测form节点：");
                    errMsg.AppendLine(sb.ToString());
                }
            }

            if (newFieldsetFormJa != null)
            {
                //errMsg += "开始检测fieldSetForm节点：" + "\r\n";
                StringBuilder sb = new StringBuilder();
                foreach (var item in newFieldsetFormJa)
                {
                    JObject jo = item.Value as JObject;

                    if (jo["bindtable"] == null)
                    {
                        //errMsg += item.Key + "无bindtable属性,未绑定表！" + "\r\n";
                        sb.AppendLine(item.Key + "无bindtable属性,未绑定表！");
                        flag = false;
                    }
                    if (jo["id"] == null)
                    {
                        //errMsg += item.Key + "无id属性,顶层容器请设置id！" + "\r\n";
                        sb.AppendLine(item.Key + "无id属性,顶层容器请设置id！");
                        flag = false;
                    }
                    if (jo["desTitle"] == null)
                    {
                        //errMsg += item.Key + "无desTitle属性,顶层容器请设置desTitle" + "\r\n";
                        sb.AppendLine(item.Key + "无desTitle属性,顶层容器请设置desTitle");
                        flag = false;
                    }

                    JArray fieldSets = jo["fieldSets"] as JArray;
                    int count = 0;
                    foreach (JObject fieldset in fieldSets)//遍历插件的fieldset
                    {
                        count++;
                        if (fieldset["itemId"] == null)
                        {
                            //errMsg += item.Key + "的第" + count + "个fieldset未设置itemId,将会影响将来升级！" + "\r\n";
                            sb.AppendLine(item.Key + "的第" + count + "个fieldset未设置itemId,将会影响将来升级！");
                            flag = false;
                        }
                    }

                }

                if (sb.Length > 0)
                {
                    errMsg.AppendLine();
                    errMsg.AppendLine("开始检测fieldSetForm节点：");
                    errMsg.AppendLine(sb.ToString());
                }
            }

            if (newGridJa != null)
            {
                //errMsg += "开始检测grid节点：" + "\r\n";
                StringBuilder sb = new StringBuilder();
                foreach (var item in newGridJa)
                {
                    JObject jo = item.Value as JObject;

                    if (jo["bindtable"] == null)
                    {
                        //errMsg += item.Key + "无bindtable属性,未绑定表" + "\r\n";
                        sb.AppendLine(item.Key + "无bindtable属性,未绑定表");
                        flag = false;
                    }
                    if (jo["id"] == null)
                    {
                        //errMsg += item.Key + "无id属性,顶层容器请设置id" + "\r\n";
                        sb.AppendLine(item.Key + "无id属性,顶层容器请设置id");
                        flag = false;
                    }
                    if (jo["desTitle"] == null)
                    {
                        //errMsg += item.Key + "无desTitle属性,顶层容器请设置desTitle" + "\r\n";
                        sb.AppendLine(item.Key + "无desTitle属性,顶层容器请设置desTitle");
                        flag = false;
                    }
                }

                if (sb.Length > 0)
                {
                    errMsg.AppendLine();
                    errMsg.AppendLine("开始检测grid节点：");
                    errMsg.AppendLine(sb.ToString());
                }
            }

            if (newTabJa != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in newTabJa)
                {
                    JObject jo = item.Value as JObject;
                    if (jo["id"] == null)
                    {
                        //errMsg += item.Key + "无id属性,顶层容器请设置id" + "\r\n";
                        sb.AppendLine(item.Key + "无id属性,顶层容器请设置id");
                        flag = false;
                    }
                    if (jo["desTitle"] == null)
                    {
                        //errMsg += item.Key + "无desTitle属性,顶层容器请设置desTitle" + "\r\n";
                        sb.AppendLine(item.Key + "无desTitle属性,顶层容器请设置desTitle");
                        flag = false;
                    }

                    JArray tabs = jo["items"] as JArray;
                    int count = 0;
                    foreach (JObject tab in tabs)
                    {
                        count++;
                        if (tab["id"] == null)
                        {
                            //errMsg += item.Key + "的第" + count + "个tab未设置id,将会影响将来升级！" + "\r\n";
                            sb.AppendLine(item.Key + "的第" + count + "个tab未设置id,将会影响将来升级！");
                            flag = false;
                        }
                        if (jo["desTitle"] == null)
                        {
                            //errMsg += item.Key + "的第" + count + "个tab无desTitle属性！" + "\r\n";
                            sb.AppendLine(item.Key + "的第" + count + "个tab无desTitle属性！");
                            flag = false;
                        }
                    }

                }
            }

            return flag;

        }

    }


}
