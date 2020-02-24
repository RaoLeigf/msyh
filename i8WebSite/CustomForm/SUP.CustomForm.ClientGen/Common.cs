using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Controls;
using SUP.CustomForm.DataEntity.AppControl;
using SUP.CustomForm.DataEntity.Container;
using SUP.CustomForm.DataEntity.Control;

namespace SUP.CustomForm.ClientGen
{
    public class Common
    {
        #region app方法
        //panelmodels app
        public static string GetPageModelFieldsApp(GridPanel gridPanel, int ts)
        {
            StringBuilder sb = new StringBuilder();
            string sts = "";
            for (int i = 0; i < ts; i++)
            {
                sts += "\t";
            }

            for (int i = 0; i < gridPanel.Columns.Count; i++)
            {
                if (i == 0)
                {
                    sb.Append("{");
                }
                else
                {
                    sb.Append("\r\n" + sts + "},{");
                }
                sb.Append(string.Format("\r\n" + sts + "\tname: '{0}',", gridPanel.Columns[i].DataIndex));
                sb.Append(string.Format("\r\n" + sts + "\ttype: '{0}',", gridPanel.Columns[i].Datatype));

                if (i == gridPanel.Columns.Count - 1)
                {
                    sb.Append("\r\n" + sts + "}");
                }
            }

            return sb.ToString();
        }

        //itemTpl app
        public static string GetPageItemTplApp(GridPanel gridPanel)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{phid}");

            for (int i = 0; i < gridPanel.Columns.Count; i++)
            {
                sb.Append("\t{" + gridPanel.Columns[i].DataIndex + "}");
            }

            return sb.ToString();
        }

        //返回toolbar的按钮 app
        public static string GetButtonsApp(Toolbar toolbar)
        {
            string s = string.Empty;

            foreach (var button in toolbar.LNgButtons)
            {
                s += "{xtype: 'button', itemId:'" + button + "', text: '" + button + "'},";
            }


            foreach (var button in toolbar.RNgButtons)
            {
                s += "{xtype: 'button', align: 'right', itemId:'" + button + "', text: '" + button + "'},";
            }

            return s.Length > 0 ? s.Substring(0, s.Length - 1) : s;
        }

        //根据返回的toolbar按钮设置事件 app
        public static string GetButtonsEventApp(Toolbar toolbar)
        {
            string s = string.Empty;

            foreach (var button in toolbar.LNgButtons)
            {
                s += ",'button[itemId=" + button + "]': { tap: 'on" + button + "ButtonTap'}";
            }


            foreach (var button in toolbar.RNgButtons)
            {
                s += ",'button[itemId=" + button + "]': { tap: 'on" + button + "ButtonTap'}";
            }

            return s;
        }

        //tableLayoutForm
        public static string GetFormApp(TableLayoutForm form)
        {
            StringBuilder sb = new StringBuilder(); //form

            sb.Append(@"{
                        xtype: 'ngText',
	                    name: 'phid',
                        itemId: 'phid',
	                    hidden: true
                    }");

            foreach (ExtControlBase item in form.AllFields)
            {
                if (item.Visible)
                {
                    sb.Append(",{ " + GetChooseFormItemApp(item) + "}");
                }
            }

            return sb.ToString();
        }

        //根据tableLayoutForm的类型 返回对应的代码
        public static string GetChooseFormItemApp(ExtControlBase controlBase)
        {
            string sb = string.Empty;
            switch (controlBase.XType)
            {
                case "ngHelp":
                    NGHelpBase ngHelp = new NGHelpBase();
                    ngHelp = controlBase as NGHelpBase;
                    sb = string.Format(@"{
                            xtype: 'ngHelp', 
                            itemId: '{0}', 
                            name: '{1}', 
                            label: '{2}', 
                            helpid: '{3}'
                        }", controlBase.ID, controlBase.Name, controlBase.FieldLabel, ngHelp.Helpid);
                    break;
                case "ngRadio":
                    NGRadio ngRadio = new NGRadio();
                    ngRadio = controlBase as NGRadio;


                    for (int i = 0; i < ngRadio.Items.Count; i++)
                    {
                        if (i > 0) sb += ",";

                        sb += "\t\t\t\t\t\t\t\t{" + ngRadio.Items[i] + "}";

                        /*
                        sb += string.Format(@"{
                            xtype: ''{0}'',
                            itemId: '{1}', 
                            name: '{2}',
                            label: '{3}', 
                            value: {4}
                        ", "radiofield", controlBase.ID, controlBase.Name, controlBase.FieldLabel, controlBase.ID); 
                        */
                    }
                    break;

                default:
                    sb = string.Format(@"
                            xtype: '{0}', 
                            itemId: '{1}',                             
                            name: '{2}', 
                            label: '{3}'
                        ", controlBase.XType, controlBase.Name, controlBase.Name, controlBase.FieldLabel);
                    break;
            }
            return sb;
        }
        #endregion app方法


        #region web页面方法
        public static string FormTableName { get; set; }
        public static string IsTask { get; set; }
        public static string JsVersion { get; set; }

        //panelmodels
        public static string GetPageModelFields(GridPanel gridPanel, string winType, int ts)
        {
            StringBuilder sb = new StringBuilder();
            string sts = "";

            //去掉列表中不要的列
            if (winType == "List")
            {
                List<ExtGridColumn> delcolumn = new List<ExtGridColumn>();
                for (int i = 0; i < gridPanel.Columns.Count; i++)
                {
                    if (gridPanel.Columns[i].DataIndex == "table_name" || gridPanel.Columns[i].DataIndex == "level_code" || gridPanel.Columns[i].DataIndex == "wf_temp_code")
                    {
                        delcolumn.Add(gridPanel.Columns[i]);
                    }
                }
                foreach (ExtGridColumn col in delcolumn)
                {
                    gridPanel.Columns.Remove(col);
                }
            }

            for (int i = 0; i < ts; i++)
            {
                sts += "\t";
            }

            for (int i = 0; i < gridPanel.Columns.Count; i++)
            {
                if (i == 0)
                {
                    sb.Append("{");
                }
                else
                {
                    sb.Append("\r\n" + sts + "},{");
                }

                if (gridPanel.Columns[i].DataIndex.Equals("itemid"))
                {
                    sb.Append(string.Format("\r\n" + sts + "\tname: '{0}',", "itemid"));
                    sb.Append(string.Format("\r\n" + sts + "\ttype: '{0}',", "string"));
                    sb.Append(string.Format("\r\n" + sts + "\tmapping: '{0}'", "itemid"));
                    sb.Append("\r\n" + sts + "},{");
                    sb.Append(string.Format("\r\n" + sts + "\tname: '{0}',", "c_name"));
                    sb.Append(string.Format("\r\n" + sts + "\ttype: '{0}',", "string"));
                    sb.Append(string.Format("\r\n" + sts + "\tmapping: '{0}'", "c_name"));
                }
                else
                {
                    sb.Append(string.Format("\r\n" + sts + "\tname: '{0}',", gridPanel.Columns[i].DataIndex));

                    //if (gridPanel.Columns[i].Datatype == "datetime")
                    //{
                    //    sb.Append(string.Format("\r\n" + sts + "\ttype: '{0}',", "string"));
                    //}

                    if (gridPanel.Columns[i].editor.XType == "ngComboBox" || gridPanel.Columns[i].editor.XType == "ngCheckbox" || gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp") //复选框和通用帮助的类型都改成string
                    {
                        sb.Append(string.Format("\r\n" + sts + "\ttype: '{0}',", "string"));
                    }
                    else
                    {
                        if (gridPanel.Sumdic.ContainsKey(gridPanel.Columns[i].DataIndex))
                        {
                            sb.Append("\r\n" + sts + "\ttype: 'float',");
                        }
                        else
                        {
                            sb.Append(string.Format("\r\n" + sts + "\ttype: '{0}',", gridPanel.Columns[i].Datatype));
                        }
                    }

                    sb.Append(string.Format("\r\n" + sts + "\tmapping: '{0}'", gridPanel.Columns[i].DataIndex));

                    //编辑窗口grid
                    if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                    {
                        if (gridPanel.Columns[i].DataIndex != "s_tree_name")
                        {
                            sb.Append("\r\n" + sts + "},{");
                            sb.Append(string.Format("\r\n" + sts + "\tname: '{0}',", gridPanel.Columns[i].DataIndex + "_name"));
                            sb.Append(string.Format("\r\n" + sts + "\ttype: '{0}',", "string"));
                            sb.Append(string.Format("\r\n" + sts + "\tmapping: '{0}'", gridPanel.Columns[i].DataIndex + "_name"));
                        }
                    }

                    if (i == gridPanel.Columns.Count - 1)
                    {
                        sb.Append("\r\n" + sts + "}");
                    }
                }
            }

            //加隐藏列
            AddModelHideColumn(ref sb, gridPanel, winType);

            return sb.ToString();
        }

        //panelcolumns
        public static string GetPageGridColumns(GridPanel gridPanel, string winType, int ts)
        {
            if (winType == "List")
            {
                return GetPageGridColumnsList(gridPanel, ts);
            }
            else
            {
                return GetPageGridColumnsEdit(gridPanel, ts);
            }
        }

        public static string GetPageGridColumnsList(GridPanel gridPanel, int ts)
        {
            StringBuilder sb = new StringBuilder();
            CommonConvert convert = new CommonConvert();
            string sts = "";

            for (int i = 0; i < ts; i++)
            {
                sts += "\t";
            }

            //有合计列或小计列
            if (gridPanel.Sumdic.Count > 0 || gridPanel.Subdic.Count > 0)
            {
                sb.Append("{ xtype: 'rownumberer', text: '序号', width: 45, sortable: false,");
                sb.Append("\r\n" + sts + "\t summaryRenderer: function(value, data, record) {");
                sb.Append("\r\n" + sts + "\t\t if (record.internalId.indexOf('summary') > 0) { ");
                if (gridPanel.Sumdic.Count > 0)
                {
                    sb.Append("\r\n" + sts + "\t\t\t return \"<font color='RoyalBlue'>合计</font>\"; ");
                }
                sb.Append("\r\n" + sts + "\t\t }");
                sb.Append("\r\n" + sts + "\t\t else { ");
                if (gridPanel.Subdic.Count > 0)
                {
                    sb.Append("\r\n" + sts + "\t\t\t return \"<font color='RoyalBlue'>小计</font>\"; ");
                }
                sb.Append("\r\n" + sts + "\t\t }");
                sb.Append("\r\n" + sts + "\t }},");
            }
            else
            {
                sb.Append("{ xtype: 'rownumberer', text: '序号', width: 45, sortable: false },");
            }

            for (int i = 0; i < gridPanel.Columns.Count; i++)
            {
                sb.Append("\r\n" + sts + "{");

                //工作流标志列固定格式
                if (gridPanel.Columns[i].DataIndex == "is_wf")
                {
                    sb.Append(string.Format(@"
        				    header: Lang.{0}is_wf || '工作流',
                            xtype: 'gcWFColumn',
        				    dataIndex: 'is_wf',
        				    itemId: 'is_wf',
        				    width: 50,
        				    sortable: false,
                            listeners: {{
        				        'beforecheckchange': function (chk, rowIndex, checked, eOpts) {{
        					        return false;
        				        }}
                            }}
                        }},"
                        , gridPanel.TableName + "_")
                    );
                    continue;
                }

                //审核标志列固定格式
                if (gridPanel.Columns[i].DataIndex == "ischeck")
                {
                    sb.Append(string.Format(@"
        				    header: Lang.{0}ischeck || '审核',
                            xtype: 'ngcheckcolumn',
        				    dataIndex: 'ischeck',
        				    itemId: 'ischeck',
        				    width: 50,
        				    sortable: false,
                            checkedVal: '1',
					        unCheckedVal: '0',
					        readOnly: true
                        }},"
                        , gridPanel.TableName + "_")
                    );
                    continue;
                }

                if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                {
                    sb.Append(string.Format("\r\n" + sts + "\theader: Lang.{0}||'{1}代码',", gridPanel.TableName + "_" + gridPanel.Columns[i].DataIndex, gridPanel.Columns[i].Header));
                }
                else
                {
                    sb.Append(string.Format("\r\n" + sts + "\theader: Lang.{0}||'{1}',", gridPanel.TableName + "_" + gridPanel.Columns[i].DataIndex, gridPanel.Columns[i].Header));
                }

                sb.Append(string.Format("\r\n" + sts + "\tdataIndex: '{0}',", gridPanel.Columns[i].DataIndex));
                sb.Append(string.Format("\r\n" + sts + "\titemId: '{0}',", gridPanel.Columns[i].DataIndex));

                if (i == gridPanel.Columns.Count)
                {
                    sb.Append(string.Format("\r\n" + sts + "\tflex: {0},", gridPanel.Columns[i].Flex));
                }
                else
                {
                    sb.Append(string.Format("\r\n" + sts + "\twidth: {0},", gridPanel.Columns[i].Width > ((gridPanel.Columns[i].Header.Length + 2) * 17) ? gridPanel.Columns[i].Width : ((gridPanel.Columns[i].Header.Length + 2) * 17)));
                }

                if (gridPanel.Columns[i].editor.XType == "ngNumber")
                {
                    sb.Append(string.Format("\r\n" + sts + "\talign: 'right',"));

                    //如果掩码为空（比如整型）添加默认掩码
                    if (string.IsNullOrEmpty(gridPanel.Columns[i].EditMask))
                    {
                        gridPanel.Columns[i].EditMask = "0,000";
                    }

                }

                sb.Append(string.Format("\r\n" + sts + "\tsortable: {0},", gridPanel.Columns[i].Sortable.Equals(true) ? "true" : "false"));

                //列表界面显示数字格式 2018-4-26  by lh
                if (gridPanel.Columns[i].editor.XType == "ngNumber")
                {
                    sb.Append("\r\n" + sts + "\trenderer: function (val) {");

                    //掩码中有%
                    if (gridPanel.Columns[i].EditMask.IndexOf("%") >= 0)
                    {
                        sb.Append("\r\n" + sts + "\t\treturn Ext.util.Format.number(val * 100,'" + gridPanel.Columns[i].EditMask + "'); ");
                    }
                    else
                    {
                        sb.Append("\r\n" + sts + "\t\treturn  Ext.util.Format.number(val,'" + gridPanel.Columns[i].EditMask + "'); ");
                    }

                    sb.Append("\r\n" + sts + "\t},");
                }

                if (gridPanel.Columns[i].Datatype.Equals("datetime"))
                {
                    string formatStr = string.Empty;

                    if (gridPanel.Columns[i].Format.Length > 11)
                    {
                        formatStr = "Y-m-d H:i:s";
                    }
                    else
                    {
                        formatStr = "Y-m-d";
                    }
                    //sb.Append("\r\n" + sts + "\trenderer: Ext.util.Format.dateRenderer('" + formatStr + "'),");
                }

                if (gridPanel.Columns[i].editor.XType == "ngCheckbox")
                {
                    sb.Append(string.Format("\r\n" + sts + "\txtype: '{0}',", "ngcheckcolumn"));
                    sb.Append(string.Format("\r\n" + sts + "\tcheckedVal: '1',"));
                    sb.Append(string.Format("\r\n" + sts + "\tunCheckedVal: '0',"));
                    sb.Append(string.Format("\r\n" + sts + "\treadOnly: true"));
                    //sb = convert.CheckColumnEvent(sts,sb );
                }

                //拼ngComboBox下拉列表数据串
                string dataStr = string.Empty;
                if (gridPanel.Columns[i].editor.XType == "ngComboBox")
                {
                    for (int j = 0; j < gridPanel.Columns[i].editor.Data.Count; j++)
                    {
                        dataStr += "{" + gridPanel.Columns[i].editor.Data[j] + "}";
                        if (j != gridPanel.Columns[i].editor.Data.Count - 1)
                        {
                            dataStr += ",";
                        }
                    }
                    dataStr = "[" + dataStr + "]";
                }

                //--list代码转名称刷新函数配置项
                if (gridPanel.Columns[i].editor.XType == "ngComboBox")
                {
                    sb = convert.ComboxEvent(sb, sts, dataStr);
                }

                //帮助字段代码列隐藏
                if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                {
                    sb.Append(string.Format("\r\n\t" + sts + "hidden: {0}", "true"));
                }

                //帮助字段再加一列名称列
                if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                {
                    //防止末尾多一个逗号
                    sb.Replace(",", "", sb.Length - 1, 1);

                    sb.Append("\r\n" + sts + "},");
                    sb.Append("\r\n" + sts + "{");
                    sb.Append(string.Format("\r\n" + sts + "\theader: Lang.{0}||'{1}',", gridPanel.TableName + "_" + gridPanel.Columns[i].DataIndex, gridPanel.Columns[i].Header));
                    sb.Append(string.Format("\r\n" + sts + "\tdataIndex: '{0}',", gridPanel.Columns[i].DataIndex + "_name"));
                    sb.Append(string.Format("\r\n" + sts + "\titemId: '{0}',", gridPanel.Columns[i].DataIndex + "_name"));

                    if (i == gridPanel.Columns.Count)
                    {
                        sb.Append(string.Format("\r\n" + sts + "\tflex: {0},", gridPanel.Columns[i].Flex));
                    }
                    else
                    {
                        sb.Append(string.Format("\r\n" + sts + "\twidth: {0},", gridPanel.Columns[i].Width > ((gridPanel.Columns[i].Header.Length + 4) * 17) ? gridPanel.Columns[i].Width : ((gridPanel.Columns[i].Header.Length + 4) * 17)));
                    }
                }

                ////修改grid列的颜色
                //if (!string.IsNullOrEmpty(gridPanel.Columns[i].RgbColor)) 
                //{
                //    sb.Append("\r\n" + sts + "\trenderer: function(val) {");
                //    sb.Append("\r\n" + sts + "\tif (val != '')");
                //    sb.Append("\r\n" + sts + "\treturn '<font color=red></font><span style=" + gridPanel.Columns[i].RgbColor + ">' + val + '</span>';}");
                //}

                //防止末尾多一个逗号
                sb.Replace(",", "", sb.Length - 1, 1);

                sb.Append("\r\n" + sts + "},");
            }

            //防止末尾多一个逗号
            sb.Replace(",", "", sb.Length - 1, 1);

            //加隐藏列
            string winType = "List";
            AddGridHideColumn(ref sb, gridPanel, winType);

            return sb.ToString();
        }

        public static string GetPageGridColumnsEdit(GridPanel gridPanel, int ts)
        {
            StringBuilder sb = new StringBuilder();
            CommonConvert convert = new CommonConvert();
            bool istreegrid = gridPanel.ColumnNames.Contains("s_tree_no");  //是否树grid
            string sts = "";
            for (int i = 0; i < ts; i++)
            {
                sts += "\t";
            }

            //多表头标签
            List<string> firstArr = new List<string>();
            List<string> lastArr = new List<string>();

            if (gridPanel.Groupcolsdic.Count > 0)
            {
                firstArr = gridPanel.Groupcolsdic["@@first"].Split(new string[] { "," }, StringSplitOptions.None).ToList<string>();
                lastArr = gridPanel.Groupcolsdic["@@last"].Split(new string[] { "," }, StringSplitOptions.None).ToList<string>();
            }

            //有合计列或小计列
            if (gridPanel.Sumdic.Count > 0 || gridPanel.Subdic.Count > 0)
            {
                sb.Append("{ xtype: 'rownumberer', text: '序号', width: 45, sortable: false,");
                sb.Append("\r\n" + sts + "\t summaryRenderer: function(value, data, record) {");
                sb.Append("\r\n" + sts + "\t\t if (record.internalId.indexOf('summary') > 0) { ");
                if (gridPanel.Sumdic.Count > 0)
                {
                    sb.Append("\r\n" + sts + "\t\t\t return \"<font color='RoyalBlue'>合计</font>\"; ");
                }
                sb.Append("\r\n" + sts + "\t\t }");
                sb.Append("\r\n" + sts + "\t\t else { ");
                if (gridPanel.Subdic.Count > 0)
                {
                    sb.Append("\r\n" + sts + "\t\t\t return \"<font color='RoyalBlue'>小计</font>\"; ");
                }
                sb.Append("\r\n" + sts + "\t\t }");
                sb.Append("\r\n" + sts + "\t }},");
            }
            else
            {
                sb.Append("{ xtype: 'rownumberer', text: '序号', width: 45, sortable: false },");
            }

            for (int i = 0; i < gridPanel.Columns.Count; i++)
            {
                string dataStr = string.Empty;

                string color = string.IsNullOrEmpty(gridPanel.Columns[i].RgbColor) ? "" : "color:" + gridPanel.Columns[i].RgbColor + "";
                string bgColor = string.IsNullOrEmpty(gridPanel.Columns[i].BackgroundColor) ? "" : "background-color:" + gridPanel.Columns[i].BackgroundColor + "";

                //属于多标签首列
                if (firstArr.Contains(gridPanel.Columns[i].DataIndex))
                {
                    sb.Append("\r\n" + sts + "{ text: '" + gridPanel.Groupcolsdic[gridPanel.Columns[i].DataIndex] + "', dataIndex: '" + gridPanel.Groupcolsdic[gridPanel.Columns[i].DataIndex] + "', width: 300, columns: [{ ");
                }
                else
                {
                    sb.Append("\r\n" + sts + "{");
                }

                if (gridPanel.Columns[i].DataIndex == "s_tree_name")
                {
                    sb.Append(string.Format("\r\n" + sts + "\txtype: 'treecolumn',"));
                }

                if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                {
                    sb.Append(string.Format("\r\n" + sts + "\theader: Lang.{0}||'{1}代码',", gridPanel.TableName + "_" + gridPanel.Columns[i].DataIndex, gridPanel.Columns[i].Header));
                }
                else
                {
                    sb.Append(string.Format("\r\n" + sts + "\theader: Lang.{0}||'{1}',", gridPanel.TableName + "_" + gridPanel.Columns[i].DataIndex, gridPanel.Columns[i].Header));
                }


                sb.Append(string.Format("\r\n" + sts + "\tdataIndex: '{0}',", gridPanel.Columns[i].DataIndex));
                sb.Append(string.Format("\r\n" + sts + "\titemId: '{0}',", gridPanel.Columns[i].DataIndex));

                if (i == gridPanel.Columns.Count - 1)
                {
                    sb.Append(string.Format("\r\n" + sts + "\tflex: {0},", gridPanel.Columns[i].Flex));
                }
                else
                {
                    sb.Append(string.Format("\r\n" + sts + "\twidth: {0},", gridPanel.Columns[i].Width > ((gridPanel.Columns[i].Header.Length + 2) * 17) ? gridPanel.Columns[i].Width : ((gridPanel.Columns[i].Header.Length + 2) * 17)));
                }

                if (gridPanel.Columns[i].editor.XType == "ngNumber")
                {
                    sb.Append(string.Format("\r\n" + sts + "\talign: 'right',"));
                }

                sb.Append(string.Format("\r\n" + sts + "\tsortable: {0},", gridPanel.Columns[i].Sortable.Equals(true) ? "true" : "false"));

                //日期类型增加格式化转换函数
                if (gridPanel.Columns[i].Datatype.Equals("datetime"))
                {
                    sb.Append("\r\n" + sts + "\trenderer: Ext.util.Format.dateRenderer('Y-m-d'),");
                }

                if (gridPanel.Columns[i].editor.XType == "ngCheckbox")
                {
                    sb.Append(string.Format("\r\n" + sts + "\txtype: '{0}',", "ngcheckcolumn"));
                    sb.Append(string.Format("\r\n" + sts + "\tcheckedVal: '1',"));
                    sb.Append(string.Format("\r\n" + sts + "\tunCheckedVal: '0',"));
                }

                //帮助列变成两列，代码列隐藏
                if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                {
                    if (gridPanel.Columns[i].DataIndex == "s_tree_name")
                    {
                        sb.Append("\r\n" + sts + "\thidden: false,");
                    }
                    else
                    {
                        sb.Append("\r\n" + sts + "\thidden: true,");
                    }

                }
                else
                {
                    if (gridPanel.Columns[i].DataIndex.Equals("spec"))
                    {
                        //防止末尾多一个逗号
                        sb.Replace(",", "", sb.Length - 1, 1);
                        sb.Append("\r\n" + sts + "},");
                        continue;
                    }

                    ////可编辑
                    //if (!gridPanel.Columns[i].Protect)
                    //{
                    if (gridPanel.Columns[i].editor.XType == "ngCheckbox")
                    {
                        sb = convert.CheckboxEvent(gridPanel, i, sts, sb);
                    }
                    else
                    {
                        //--grid中的editor配置项
                        sb.Append(string.Format("\r\n" + sts + "\teditor: "));
                        sb.Append("{");
                        if (gridPanel.Columns[i].editor.XType == "ngDate")
                        {
                            sb.Append(string.Format("\r\n" + sts + "\t\txtype: '{0}',", "datefield"));
                            sb.Append("\r\n" + sts + "\tformat: 'Y-m-d',");
                        }
                        else if (gridPanel.Columns[i].editor.XType == "ngRadio")
                        {
                            sb.Append(string.Format("\r\n" + sts + "\t\txtype: '{0}',", "radiogroup"));
                            sb.Append(string.Format("\r\n" + sts + "\t\titems:["));
                            for (int k = 0; k < gridPanel.Columns[i].editor.Items.Count; k++)
                            {
                                sb.Append("\r\n" + sts + "\t\t\t{" + gridPanel.Columns[i].editor.Items[k] + "}");
                                if (k == gridPanel.Columns[i].editor.Items.Count - 1)
                                {
                                    sb.Append("\r\n");
                                }
                                else
                                {
                                    sb.Append(",\r\n");
                                }
                            }
                            sb.Append(sts + "\t\t],");
                        }
                        else if (gridPanel.Columns[i].editor.XType == "ngComboBox")
                        {
                            for (int j = 0; j < gridPanel.Columns[i].editor.Data.Count; j++)
                            {
                                dataStr += "{" + gridPanel.Columns[i].editor.Data[j] + "}";
                                if (j != gridPanel.Columns[i].editor.Data.Count - 1)
                                {
                                    dataStr += ",";
                                }
                            }
                            dataStr = "[" + dataStr + "]";

                            sb.Append("\r\n" + sts + "\t\txtype: 'ngComboBox',");
                            sb.Append("\r\n" + sts + "\t\tvalueField: 'code',");
                            sb.Append("\r\n" + sts + "\t\tdisplayField: 'name',");
                            sb.Append("\r\n" + sts + "\t\tqueryMode: 'local',");
                            sb.Append(string.Format("\r\n" + sts + "\t\tdata: {0},", dataStr));
                        }
                        else
                        {
                            if (gridPanel.Columns[i].editor.XType == "ngNumber")
                            {
                                sb.Append(string.Format("\r\n" + sts + "\t\txtype: '{0}',", "ngNumber"));

                                //掩码中有%
                                if (gridPanel.Columns[i].EditMask.IndexOf("%") >= 0)
                                {
                                    sb.Append(string.Format("\r\n" + sts + "\t\tshowPercent: {0},", "true"));
                                }

                                //小数位数
                                int decimalPrecision = 2;
                                if (string.IsNullOrEmpty(gridPanel.Columns[i].EditMask))
                                {
                                    gridPanel.Columns[i].EditMask = "0,000";
                                    decimalPrecision = 0;
                                }
                                else if (gridPanel.Columns[i].EditMask.IndexOf(".") >= 0)
                                {
                                    decimalPrecision = gridPanel.Columns[i].EditMask.Split(new string[] { "." }, StringSplitOptions.None)[1].Length;
                                    if (gridPanel.Columns[i].EditMask.IndexOf("%") >= 0)
                                    {
                                        decimalPrecision++;
                                    }
                                }
                                else
                                {
                                    decimalPrecision = 0;
                                }

                                sb.Append(string.Format("\r\n" + sts + "\t\tdecimalPrecision: {0},", decimalPrecision));
                            }
                            else
                            {
                                sb.Append(string.Format("\r\n" + sts + "\t\txtype: '{0}',", "ngText"));
                            }
                        }

                        sb.Append(string.Format("\r\n" + sts + "\t\tallowBlank: {0},", gridPanel.Columns[i].editor.AllowBlank.Equals(true) ? "true" : "false"));
                        //sb.Append(string.Format("\r\n" + sts + "\t\tfieldStyle: '{0}',", color+bgColor));//修改某一列grid列的颜色

                        sb.Append("\r\n" + sts + "\t},");
                    }
                    //}

                    if (gridPanel.Columns[i].editor.XType == "ngNumber")
                    {
                        sb.Append("\r\n" + sts + "\trenderer:function(val,m){");

                        //掩码中有%
                        if (gridPanel.Columns[i].EditMask.IndexOf("%") >= 0)
                        {
                            sb.Append("\r\n" + sts + "\tif(typeof(m)!='undefined'){");
                            sb.Append("\r\n" + sts + "\t\tm.style=\"" + color + "" + bgColor + "\";");
                            sb.Append("\r\n" + sts + "\t}");
                            sb.Append("\r\n" + sts + "\t\treturn Ext.util.Format.number(val * 100,'" + gridPanel.Columns[i].EditMask + "'); ");

                        }
                        else
                        {
                            sb.Append("\r\n" + sts + "\tif(typeof(m)!='undefined'){");
                            sb.Append("\r\n" + sts + "\t\tm.style=\"" + color + "" + bgColor + "\";");
                            sb.Append("\r\n" + sts + "\t}");
                            sb.Append("\r\n" + sts + "\t\treturn  Ext.util.Format.number(val,'" + gridPanel.Columns[i].EditMask + "'); ");
                        }

                        sb.Append("\r\n" + sts + "\t},");
                    }

                    if (gridPanel.Columns[i].editor.XType == "ngDate")
                    {
                        sb.Append("\r\n" + sts + "\trenderer:function(val,m){");
                        sb.Append("\r\n" + sts + "\tif(typeof(m)!='undefined'){");
                        sb.Append("\r\n" + sts + "\t\tm.style=\"" + color + "" + bgColor + "\";");
                        sb.Append("\r\n" + sts + "\t}");
                        sb.Append("\r\n" + sts + "\t\treturn  Ext.util.Format.date(val, 'Y-m-d'); ");
                        sb.Append("\r\n" + sts + "\t},");
                    }

                    //设置grid列的字体颜色和背景颜色
                    if (gridPanel.Columns[i].editor.XType == "ngText" && (!string.IsNullOrEmpty(gridPanel.Columns[i].BackgroundColor) || !string.IsNullOrEmpty(gridPanel.Columns[i].RgbColor)))
                    {
                        sb.Append("\r\n" + sts + "\trenderer:function(val,m){");
                        sb.Append("\r\n" + sts + "\tif(typeof(m)!='undefined'){");
                        sb.Append("\r\n" + sts + "\t\tm.style=\"" + color + "" + bgColor + "\";");
                        sb.Append("\r\n" + sts + "\t}");
                        sb.Append("\r\n" + sts + "\t\treturn val; ");
                        sb.Append("\r\n" + sts + "\t},");
                    }
                }

                //不可编辑
                if (gridPanel.Columns[i].Protect)
                {
                    if (gridPanel.Columns[i].editor.XType == "ngCheckbox")
                    {
                        sb = convert.CheckColumnEvent(sts, sb);
                    }
                    else if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                    {
                        //暂时没想到grid中帮助列怎么设为只读
                    }
                }

                if (gridPanel.Sumdic.ContainsKey(gridPanel.Columns[i].DataIndex) || gridPanel.Subdic.ContainsKey(gridPanel.Columns[i].DataIndex))
                {
                    string calc = string.Empty;

                    //取合计或小计的汇总方式，这里有个问题，合计和小计汇总方式只能是相同的
                    if (gridPanel.Sumdic.ContainsKey(gridPanel.Columns[i].DataIndex))
                    {
                        calc = gridPanel.Sumdic[gridPanel.Columns[i].DataIndex];
                    }
                    else if (gridPanel.Subdic.ContainsKey(gridPanel.Columns[i].DataIndex))
                    {
                        calc = gridPanel.Subdic[gridPanel.Columns[i].DataIndex];
                    }

                    sb.Append(string.Format("\r\n" + sts + "\tsummaryType:'{0}',", calc));
                    sb.Append("\r\n" + sts + "\t summaryRenderer: function(value, data, record) {");
                    sb.Append("\r\n" + sts + "\t\t if (record.internalId.indexOf('summary') > 0) { ");
                    if (gridPanel.Sumdic.ContainsKey(gridPanel.Columns[i].DataIndex))
                    {
                        sb.Append("\r\n" + sts + "\t\t\t return \"<font color='RoyalBlue'>\" +Ext.util.Format.number(value,'" + gridPanel.Columns[i].EditMask + "')+ \"</font>\";");
                    }
                    sb.Append("\r\n" + sts + "\t\t }");
                    sb.Append("\r\n" + sts + "\t\t else { ");
                    if (gridPanel.Subdic.ContainsKey(gridPanel.Columns[i].DataIndex))
                    {
                        sb.Append("\r\n" + sts + "\t\t\t return \"<font color='RoyalBlue'>\" +Ext.util.Format.number(value,'" + gridPanel.Columns[i].EditMask + "')+ \"</font>\";");
                    }
                    sb.Append("\r\n" + sts + "\t\t }");
                    sb.Append("\r\n" + sts + "\t }");
                }

                //--grid代码转名称刷新函数配置项
                if (gridPanel.Columns[i].editor.XType == "ngComboBox")
                {
                    sb = convert.ComboxEvent(sb, sts, dataStr);
                }

                //  帮助字段再加一列名称列
                if (gridPanel.Columns[i].editor.XType == "ngCustomFormHelp" || gridPanel.Columns[i].editor.XType == "ngRichHelp")
                {
                    if (gridPanel.Columns[i].DataIndex != "s_tree_name")
                    {
                        //防止末尾多一个逗号
                        sb.Replace(",", "", sb.Length - 1, 1);

                        sb.Append("\r\n" + sts + "},");
                        sb.Append("\r\n" + sts + "{");
                        sb.Append(string.Format("\r\n" + sts + "\theader: Lang.{0}||'{1}',", gridPanel.TableName + "_" + gridPanel.Columns[i].DataIndex, gridPanel.Columns[i].Header));
                    }


                    if (gridPanel.Columns[i].DataIndex.Equals("itemid"))
                    {
                        sb = convert.GetResmaster(gridPanel, i, sb, sts);
                    }
                    else
                    {
                        sb = convert.GetOtherGridColumn(gridPanel, i, sb, sts);
                    }
                }

                //防止末尾多一个逗号
                sb.Replace(",", "", sb.Length - 1, 1);

                //属于多标签首列
                if (lastArr.Contains(gridPanel.Columns[i].DataIndex))
                {
                    sb.Append("\r\n" + sts + "}]");
                }

                sb.Append("\r\n" + sts + "},");
            }

            //防止末尾多一个逗号
            sb.Replace(",", "", sb.Length - 1, 1);

            //加隐藏列
            string winType = "Edit";
            AddGridHideColumn(ref sb, gridPanel, winType);

            //树grid修改header为text
            if (istreegrid)
            {
                sb.Replace("header:", "text:");
            }

            return sb.ToString();
        }

        public static string GetFieldSetsExt(TableLayoutForm form, List<FieldSet> fieldSets, List<PbTabInfo> tabinfos, List<GridPanel> gridPanels, List<NGPictureBox> pictureBoxs)
        {
            CommonConvert convert = new CommonConvert();
            StringBuilder sb = new StringBuilder();
            int lastindex = 0;
            string sts = "";
            for (int i = 0; i < 5; i++)
            {
                sts += "\t";
            }

            //fieldSet外面的form列
            if (form != null && form.AllFields.Count > 0)
            {
                if (form.IsAbsoluteLayout)
                {
                    for (int i = 0; i < form.AllFields.Count; i++)
                    {
                        if (form.IsAbsoluteLayout)
                        {
                            sb.Append("{" + GetChooseFormItem(form.AllFields[i], 0, true) + "},");
                        }
                        else
                        {
                            sb.Append("{" + GetChooseFormItem(form.AllFields[i], 0, false) + "},");
                        }
                    }

                    if (pictureBoxs.Count > 0)
                    {
                        for (int i = 0; i < pictureBoxs.Count; i++)
                        {
                            sb.Append("{");
                            sb.Append(string.Format(@"
                                xtype: 'ngText',
                                fieldLabel: '{0}',
						        name: '{1}',
                                id: '{1}',
                                hidden: true
                                ", pictureBoxs[i].Tag, pictureBoxs[i].Name + "_url"));
                            sb.Append("},");
                        }

                        for (int i = 0; i < pictureBoxs.Count; i++)
                        {
                            sb.Append(pictureBoxs[i].Name + "picPanel");
                            sb.Append(",");
                        }
                    }

                }
                else  //非绝对布局下游离的form列组装成一个无边界框的fieldset
                {
                    sb = convert.NotAbsForm(form, sb);

                    lastindex = form.FieldRows.Count - 1;

                    for (var j = 0; j < form.FieldRows.Count; j++)
                    {
                        List<ExtControlBase> row = form.FieldRows[j];
                        sb.Append("[");
                        int index = row.Count - 1;
                        for (int k = 0; k < row.Count; k++)
                        {
                            if (form.IsAbsoluteLayout)
                            {
                                sb.Append("{" + GetChooseFormItem(row[k], 0, true) + "}");
                            }
                            else
                            {
                                sb.Append("{" + GetChooseFormItem(row[k], 0, false) + "}");
                            }
                            if (k < index)
                            {
                                sb.Append(",");
                            }
                        }
                        sb.Append("]");
                        if (j < lastindex)
                        {
                            sb.Append(",");
                        }
                    }

                    if (pictureBoxs.Count > 0)
                    {
                        sb.Append(",[");
                        for (int i = 0; i < pictureBoxs.Count; i++)
                        {
                            sb.Append("{");
                            sb.Append(string.Format(@"
                                xtype: 'ngText',
                                fieldLabel: '{0}',
						        name: '{1}',
                                id: '{1}',
                                hidden: true
                                ", pictureBoxs[i].Tag, pictureBoxs[i].Name + "_url"));
                            sb.Append("},");
                        }

                        for (int i = 0; i < pictureBoxs.Count; i++)
                        {
                            sb.Append(pictureBoxs[i].Name + "picPanel");

                            if (i != pictureBoxs.Count - 1)
                            {
                                sb.Append(",");
                            }
                        }
                        sb.Append("]");
                    }

                    sb.Append("]}");
                    sb.Append(",");
                }
            }

            //fieldSet中的列按二维数组排列
            for (int i = 0; i < fieldSets.Count; i++)
            {
                FieldSet fieldSet = fieldSets[i];

                if (fieldSet.Title == "金格控件" || fieldSet.Title == "进度控件")
                {
                    continue;
                }

                if (fieldSet.Panels.Count == 0)
                {
                    if (form.IsAbsoluteLayout)
                    {
                        sb = convert.AbsFieldSet(fieldSets, i, sb);
                    }
                    else
                    {
                        sb = convert.NotAbsFieldSet(fieldSets, i, sb);
                    }

                    lastindex = fieldSet.FieldRows.Count - 1;

                    for (var j = 0; j < fieldSet.FieldRows.Count; j++)
                    {
                        List<ExtControlBase> row = fieldSet.FieldRows[j];
                        sb.Append("[");
                        int index = row.Count - 1;
                        for (int k = 0; k < row.Count; k++)
                        {
                            if (form.IsAbsoluteLayout)
                            {
                                sb.Append("{" + GetChooseFormItem(row[k], 0, true) + "}");
                            }
                            else
                            {
                                sb.Append("{" + GetChooseFormItem(row[k], 0, false) + "}");
                            }
                            if (k < index)
                            {
                                sb.Append(",");
                            }
                        }
                        sb.Append("]");
                        if (j < lastindex)
                        {
                            sb.Append(",");
                        }
                    }
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{" + "xtype: 'fieldset'," + string.Format(@"
                                     columnsPerRow: {0},
                                     title: '{1}',
                                     x: {2},
                                     y: {3},
                                     height: {4},
                                     items: [", fieldSet.ColumnsPerRow, fieldSet.Title, fieldSet.X, fieldSet.Y, fieldSet.Height));
                    for (int j = 0; j < fieldSet.Panels.Count; j++)
                    {
                        if (j != 0)
                            sb.Append(",");
                        sb.Append(string.Format("{0}grid", fieldSet.Panels[j].TableName));
                    }
                    sb.Append(@"]
                    }");
                }

                sb.Append(",");
            }

            //绝对布局且有fieldset情况下，grid、tabpanel和金格都放在FieldSetForm里
            if (form.IsAbsoluteLayout)
            {
                if (tabinfos != null && tabinfos.Count > 0)
                {
                    for (int i = 0; i < tabinfos.Count; i++)
                    {
                        sb.Append(string.Format("tabPanel{0},", i));
                    }
                }

                if (gridPanels != null && gridPanels.Count > 0)
                {
                    for (int i = 0; i < gridPanels.Count; i++)
                    {
                        if (!gridPanels[i].IsInTab)
                        {
                            sb.Append(string.Format("{0}grid,", gridPanels[i].TableName));
                        }
                    }
                }

                //加上金格控件
                for (int i = 0; i < fieldSets.Count; i++)
                {
                    FieldSet fieldSet = fieldSets[i];

                    if (fieldSet.Title == "金格控件")
                    {
                        sb.Append("blobdocPanel,");
                    }
                }
            }

            //追加固定隐藏列
            AddFormHideColumn(ref sb, form);

            return sb.ToString();
        }

        //tableLayoutForm  绝对布局
        public static string GetFormAbs(TableLayoutForm form, List<NGPictureBox> pictureBoxs)
        {
            StringBuilder sb = new StringBuilder(); //ngToolbar, form
            string sts = "";

            for (int i = 0; i < 5; i++)
            {
                sts += "\t";
            }

            for (var j = 0; j < form.AllFields.Count; j++)
            {
                sb.Append("{" + GetChooseFormItem(form.AllFields[j], form.ColumnsPerRow, true) + "}");
                sb.Append(",");
            }

            //追加固定隐藏列
            AddFormHideColumn(ref sb, form);

            for (int i = 0; i < pictureBoxs.Count; i++)
            {
                sb.Append(",{");
                sb.Append(string.Format(@"
                        xtype: 'ngText',
                        fieldLabel: '{0}',
						name: '{1}',
                        id: '{1}',
                        hidden: true
                    ", pictureBoxs[i].Tag, pictureBoxs[i].Name + "_url"));
                sb.Append("}");
            }

            if (pictureBoxs.Count > 0)
            {
                sb.Append(",");
                for (int i = 0; i < pictureBoxs.Count; i++)
                {
                    sb.Append(pictureBoxs[i].Name + "picPanel");

                    if (i != pictureBoxs.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }

            return sb.ToString();
        }

        //tableLayoutForm  非绝对布局
        public static string GetForm(TableLayoutForm form, List<NGPictureBox> pictureBoxs)
        {
            StringBuilder sb = new StringBuilder(); //ngToolbar, form
            string sts = "";

            for (int i = 0; i < 5; i++)
            {
                sts += "\t";
            }

            for (var j = 0; j < form.FieldRows.Count; j++)
            {
                List<ExtControlBase> row = form.FieldRows[j];

                sb.Append("[");

                for (int k = 0; k < row.Count; k++)
                {
                    sb.Append("{" + GetChooseFormItem(row[k], form.ColumnsPerRow, false) + "}");

                    //最后一列之前的列
                    if (k < (row.Count - 1))
                    {
                        sb.Append(",");
                    }
                }

                sb.Append("],");
            }

            sb.Append(@" [");
            AddFormHideColumn(ref sb, form);  //追加固定隐藏列

            for (int i = 0; i < pictureBoxs.Count; i++)
            {
                sb.Append(",{");
                sb.Append(string.Format(@"
                        xtype: 'ngText',
                        fieldLabel: '{0}',
						name: '{1}',
                        id: '{1}',
                        hidden: true
                    ", pictureBoxs[i].Tag, pictureBoxs[i].Name + "_url"));
                sb.Append("}");
            }

            sb.Append(@" ]");

            if (pictureBoxs.Count > 0)
            {
                sb.Append(",[");
                for (int i = 0; i < pictureBoxs.Count; i++)
                {
                    sb.Append(pictureBoxs[i].Name + "picPanel");

                    if (i != pictureBoxs.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append(" ]");
            }

            return sb.ToString();
        }

        //根据tableLayoutForm的类型 返回对应的代码
        public static string GetChooseFormItem(ExtControlBase controlBase, int ColumnsPerRow, bool IsAbsolute)
        {
            string sb = string.Empty;
            CommonConvert convert = new CommonConvert();
            convert.FormTableName = FormTableName;

            //防止一些固定隐藏字段y过大使布局空白很大
            if (controlBase.Visible.Equals(false))
            {
                controlBase.YPos = 10;
            }

            //附件列只读
            if (controlBase.Name == "asr_flg")
            {
                controlBase.Protect = true;
            }

            switch (controlBase.XType)
            {
                case "ngCustomFormHelp":
                    sb = convert.GetngCustomFormHelp(controlBase);
                    break;
                case "ngRichHelp":
                    sb = convert.GetngRichHelp(controlBase);
                    break;
                case "ngRadio":
                    sb = convert.GetngRadio(controlBase);
                    break;
                case "ngComboBox":
                    sb = convert.GetngComboBox(controlBase);
                    break;
                case "button":
                    sb = convert.GetButton(controlBase);
                    break;
                case "displayfield":
                    sb = convert.GetDisplayField(controlBase, ColumnsPerRow);
                    break;
                case "ngCheckbox":
                    sb = convert.GetngCheckbox(controlBase);
                    break;
                case "ngDate":
                    sb = convert.GetDate(controlBase);
                    break;
                case "ngTextArea":
                    sb = convert.GetngTextArea(controlBase);
                    break;
                case "ngText":
                    sb = convert.GetngText(controlBase);
                    break;
                case "ngNumber":
                    sb = convert.GetngNumber(controlBase);
                    break;
                default:
                    sb = convert.GetOtherForm(controlBase);
                    break;
            }

            if (IsAbsolute && controlBase.XType != "button" && controlBase.XType != "displayfield")
            {
                sb = sb + string.Format(",labelWidth:{0}", (controlBase.LabelWidth));
            }

            return sb;
        }


        //返回对应的位置
        public static string GetRegion(FieldSet fieldSet)
        {
            return fieldSet.Region;
        }

        //获取tab中的报表
        public static string GetReportView(List<PbTabInfo> tabInfos)
        {
            StringBuilder sb = new StringBuilder();
            string gridName = string.Empty;

            for (int t = 0; t < tabInfos.Count; t++)  //目前tabInfos数量只有一个
            {
                for (int i = 0; i < tabInfos[t].GridIds.Count; i++)
                {
                    gridName = tabInfos[t].GridIds[i];

                    if (gridName.IndexOf("rwgrid") >= 0)  //报表控件
                    {
                        /*
                        [rwgrid1]
                        report_view=rep_code报表编号&rep_format&默认工作表&参数标志(1有参数)
                        report_para=参数来源&目标工作表&函数&函数参数 @参数来源&目标工作表&函数&函数参数
                        */
                        string[] reportView = tabInfos[t].ReportViews[i].Split(new string[] { "&" }, StringSplitOptions.None);

                        var urlparam = "rep_src=0&rep_code=" + reportView[0] + "&rep_format=" + reportView[1];

                        string tarWorkSheetName = reportView[2];
                        string srcDatas = string.Empty;
                        string tarSheets = string.Empty;
                        string tarDSs = string.Empty;
                        string tarParas = string.Empty;

                        //有参数
                        if (reportView[3] == "1")
                        {
                            string[] reportPara = tabInfos[t].ReportParas[i].Split(new string[] { "@" }, StringSplitOptions.None);

                            foreach (var para in reportPara)
                            {
                                string[] paras = para.Split(new string[] { "&" }, StringSplitOptions.None);
                                srcDatas += "Ext.getCmp('" + paras[0] + "').getValue()+" + "'\\\\|'+";
                                tarSheets += paras[1] + "\\\\|";
                                tarDSs += paras[2] + "\\\\|";
                                tarParas += paras[3] + "\\\\|";
                            }

                            srcDatas = srcDatas.Substring(0, srcDatas.Length - 7);
                            tarSheets = tarSheets.Substring(0, tarSheets.Length - 3);
                            tarDSs = tarDSs.Substring(0, tarDSs.Length - 3);
                            tarParas = tarParas.Substring(0, tarParas.Length - 3);
                        }
                        else
                        {
                            srcDatas = "''";
                        }

                        string data = (string.Format(@"{{
                                                            LinkRep_tarWorkSheetName: {0},
                                                            LinkRep_srcDatas: {1},
                                                            LinkRep_tarSheets: '{2}',
                                                            LinkRep_tarDSs: '{3}',
                                                            LinkRep_tarParas: '{4}',
                                                            LinkRep_AutoCalc: 1 
                                                        }}"
                                    , tarWorkSheetName, srcDatas, tarSheets, tarDSs, tarParas)
                        );

                        sb.Append(string.Format(@"var reportFrame{0} = document.createElement('IFRAME');
                                                      reportFrame{0}.scrolling = 'auto';
                                                      reportFrame{0}.frameBorder = 0;
                                                      reportFrame{0}.src = '';
                                                      reportFrame{0}.height = '100%';
                                                      reportFrame{0}.width = '100%';
                                        
                                                      function reportFunc{0}() {{
                                                          data={1};
                                                          var urlparam = '{2}';
                                                          var replinkdata = encodeURIComponent(Ext.encode(data));

                                                          var url = C_ROOT + 'RW/DesignFrame/ReportView?replink=true&superiorpage=efrom&' + urlparam + '&replinkdata=' + replinkdata;
                                                          reportFrame{0}.src = url;
                                                      }};
                                                      virtualPanel.on('afterViewport', function () {{                                   
                                                          mstform.on('dataready', function () {{
                                                              reportFunc{0}();
                                                          }});
                                                      }});
                                                      "
                                                  , i, data, urlparam)
                        );
                    }
                }
            }

            return sb.ToString();
        }

        //获取tab中项目
        public static string GetTabPanelItems(List<PbTabInfo> tabInfos)
        {
            StringBuilder sb = new StringBuilder();
            string gridName = string.Empty;

            for (int i = 0; i < tabInfos.Count; i++)  //目前tabInfos数量只有一个
            {
                for (int j = 0; j < tabInfos[i].GridIds.Count; j++)
                {
                    gridName = tabInfos[i].GridIds[j];

                    if (gridName == "blobdoc")  //金格控件
                    {
                        //第一个tab页是金格则把金格panel填入，否则不填入，tabchange时再填入；现在不用这样处理了，直接把金格放到items，如果不是在第一页也只会在tabchange是切换出来
                        sb.Append(string.Format(@"{{
                                    id: 'blobdoc',
                                    title: '{0}',
                                    layout: 'border',
                                    items: [blobdocPanel]
                                }},"
                                , tabInfos[i].TabNames[j])
                        );
                    }
                    else if (gridName.IndexOf("rwgrid") >= 0)  //报表控件
                    {
                        sb.Append(@"{
                                    id: 'reportFrame" + Convert.ToString(j) + @"',
                                    title: '" + tabInfos[i].TabNames[j] + @"',
                                    layout: 'border',
                                    items: reportFrame" + Convert.ToString(j) + @"
                                },"
                        );
                    }
                    else if (gridName == "eppocx")  //进度控件
                    {
                        sb.Append(string.Format(@"{{
                                    id: 'eppocx',
                                    title: '{0}',
                                    layout: 'border',
                                    items: [eppocxPanel]
                                }},"
                                , tabInfos[i].TabNames[j])
                        );
                    }
                    else if (gridName == "wfgrid")  //审批单据体
                    {

                    }
                    else if (gridName == "asrgrid")  //附件单据体
                    {
                        sb.Append(string.Format("{0},", gridName));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}grid,", gridName));
                    }
                }
            }

            //防止末尾多一个逗号
            sb.Replace(",", "", sb.Length - 1, 1);

            return sb.ToString();
        }

        //viewport导航
        public static string GetViewPortItems(List<FieldSet> fieldSets, List<GridPanel> gridPanels,
                                              TableLayoutForm form, string hasBlobdoc, string hasEppocx)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("mstform");

            //没有fieldset或相对布局
            if (fieldSets.Count == 0 || !form.IsAbsoluteLayout)
            {
                for (int i = 0; i < gridPanels.Count; i++)
                {
                    sb.Append(string.Format(",{0}grid", gridPanels[i].TableName));
                }

                //有金格控件
                if (hasBlobdoc == "1")
                {
                    sb.Append(",blobdocPanel");
                }

                //有进度控件
                if (hasEppocx == "1")
                {
                    sb.Append(",eppocxPanel");
                }
            }

            return sb.ToString();
        }

        public static string GetViewPortItems(List<FieldSet> fieldSets, List<GridPanel> gridPanels,
                                              TableLayoutForm form, List<PbTabInfo> tabinfos)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("mstform");

            //没有fieldset或相对布局
            if (fieldSets.Count == 0 || !form.IsAbsoluteLayout)
            {
                //游离panel排序后加入viewport
                SortedList<int, string> sortlist = new SortedList<int, string>();

                if (tabinfos.Count > 0)
                {
                    sortlist.Add(tabinfos[0].YPos, ",tabPanel0");
                }

                for (int i = 0; i < gridPanels.Count; i++)
                {
                    if (!gridPanels[i].IsInTab)
                    {
                        sortlist.Add(gridPanels[i].Y, string.Format(",{0}grid", gridPanels[i].TableName));
                    }
                }

                foreach (var item in sortlist)
                {
                    sb.Append(item.Value);
                }
            }

            return sb.ToString();
        }

        //Grid的事件监听
        public static string GetClickListener(GridPanel gridpanel)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("{0}grid.addListener('containerclick', function (me, e, eOpts)", gridpanel.TableName)).Append("{\r\n");
            sb.Append(string.Format("curGrid = {0}grid;\r\n", gridpanel.TableName));
            sb.Append(string.Format("curStore = curGrid.store;\r\n"));
            sb.Append("});\r\n");

            sb.Append(string.Format("{0}grid.addListener('cellclick',function(me, rowIndex, columnIndex, e)", gridpanel.TableName)).Append("{\r\n");
            sb.Append(string.Format("curGrid = {0}grid;\r\n", gridpanel.TableName));
            sb.Append(string.Format("curStore = curGrid.store;\r\n"));
            sb.Append("});\r\n");

            return sb.ToString();
        }

        //返回toolbar的按钮
        public static string GetButtons(Toolbar toolbar, string type)
        {
            string s = string.Empty;

            foreach (var button in toolbar.LNgButtons)
            {
                //去审核
                if (button.Equals("unverify"))
                {
                    s += "{ itemId: \"unverify\", langkey: \"unverify\", text: \"去审核\", iconCls: \"icon-Verify\"},";
                    continue;
                }

                //申请去审
                if (button.Equals("applycheck"))
                {
                    s += "{ itemId: \"applycheck\", langkey: \"applycheck\", text: \"申请去审\", iconCls: \"icon-Unvalid\"},";
                    continue;
                }

                //下达
                if (button.Equals("subbill"))
                {
                    s += "{ itemId: \"subbill\", langkey: \"subbill\", text: \"下达\", iconCls: \"icon-Backbill\"},";
                    continue;
                }

                //提交
                if (button.Equals("ok"))
                {
                    s += "{ itemId: \"ok\", langkey: \"ok\", text: \"提交\", iconCls: \"icon-Confirm\"},";
                    continue;
                }

                //汇总
                if (button.Equals("query"))
                {
                    s += "{ itemId: \"query\", langkey: \"query\", text: \"汇总\", iconCls: \"icon-Query\"},";
                    continue;
                }

                //原汇总信息
                if (button.Equals("deal"))
                {
                    s += "{ itemId: \"deal\", langkey: \"deal\", text: \"原汇总信息\", iconCls: \"icon-Operate\"},";
                    continue;
                }

                s += "'" + button + "',";
            }

            if (type == "Edit")
            {
                if (!toolbar.LNgButtons.Contains("unverify"))
                {
                    s += "{ itemId: \"unverify\", langkey: \"unverify\", text: \"去审核\", iconCls: \"icon-Verify\"},";
                }
            }

            if (toolbar.RNgButtons.Count > 0)
            {
                s += " '->', ";
            }

            foreach (var button in toolbar.RNgButtons)
            {
                s += "'" + button + "',";
            }

            return s.Length > 0 ? s.Substring(0, s.Length - 1) : s;
        }


        public static string BatchBindCombox(TableLayoutForm form, List<FieldSet> fieldSets)
        {
            string s = string.Empty;

            foreach (var field in form.AllFields)
            {
                if (field.XType == "ngCustomFormHelp" || field.XType == "ngRichHelp")
                {
                    s += string.Format("codectl.push(Ext.getCmp('{0}'));", field.Name);
                    s += string.Format("\r\n");
                }
            }

            foreach (var fieldSet in fieldSets)
            {
                foreach (var field in fieldSet.AllFields)
                {
                    if (field.XType == "ngCustomFormHelp" || field.XType == "ngRichHelp")
                    {
                        s += string.Format("codectl.push(Ext.getCmp('{0}'));", field.Name);
                        s += string.Format("\r\n");
                    }
                }
            }

            if (!String.IsNullOrEmpty(s))
            {
                s = string.Format("var codectl = [];\r\n") + s + "BatchBindCombox(codectl);";
            }
            return s;
        }

        //设置grid列为只读、必输等状态
        public static string SetGridColStatus(List<GridPanel> gridpanels)
        {
            string s = string.Empty;
            string tablename = string.Empty;

            foreach (GridPanel gridpanel in gridpanels)
            {
                foreach (ExtGridColumn col in gridpanel.Columns)
                {
                    if (col.editor.XType == "ngCustomFormHelp" || col.editor.XType == "ngRichHelp")
                    {
                        tablename = col.DataIndex + "_name";
                    }
                    else
                    {
                        tablename = col.DataIndex;
                    }


                    if (col.Protect)
                    {
                        s += string.Format("{0}grid.setReadOnlyCol('{1}');", gridpanel.TableName, tablename);
                        s += string.Format("\r\n");
                    }

                    if (col.MustInput)
                    {
                        s += string.Format("{0}grid.setMustInputCol('{1}');", gridpanel.TableName, tablename);
                        s += string.Format("\r\n");
                    }
                }
            }

            return s;
        }


        //返回显示，只读，必输事件触发, 如刚打开编辑窗口时，要强制触发下影响三种状态的变动字段的事件
        public static string FireEvents(TableLayoutForm form)
        {
            string s = string.Empty;
            Dictionary<String, String> dic = new Dictionary<String, String>();
            Dictionary<String, String> col = new Dictionary<String, String>();

            //确定事件函数名
            foreach (var item in form.Items)
            {
                string funcname = string.Empty;
                if (item.ControlType == PbControlType.ComboBox || item.ControlType == PbControlType.DataHelpEdit)
                {
                    funcname = "helpselected";
                }
                else
                {
                    funcname = "itemchanged";
                }
                col.Add(item.Name, funcname);
            }

            //确定事件触发字段
            foreach (var item in form.Items)
            {
                var temp = (PbBaseTextInfo)item;

                string[] expressions = new string[3];
                expressions[0] = temp.VisibleExpressionImp.Expression;
                expressions[1] = temp.IsMustInputExpressionImp.Expression;
                expressions[2] = temp.IsReadOnlyExpressionImp.Expression;

                foreach (var exp in expressions)
                {
                    if (!string.IsNullOrEmpty(exp) && exp.IndexOf('(') != (-1) && exp.IndexOf(')') != (-1))
                    {
                        // textcol_2.visible=1	if( ddlbcol_1='2', 0, 1 )
                        int leftBracket = exp.IndexOf('(');
                        int rightBracket = exp.IndexOf(')');
                        string innerStr = exp.Substring(leftBracket + 1, rightBracket - leftBracket - 1).Trim();
                        char[] operatorArr = { '=', '>', '<', '!' };
                        string changeCol = innerStr.Split(operatorArr)[0].Trim();

                        if (!dic.ContainsKey(changeCol))
                        {
                            dic.Add(changeCol, col[changeCol]);
                        }
                    }
                }
            }

            foreach (var item in dic)
            {
                s += string.Format("Ext.getCmp('{0}').fireEvent('{1}');", item.Key, item.Value);
                s += string.Format("\r\n");
            }

            return s;
        }

        //返回显示，只读，必输事件触发
        public static string FireEvents(List<FieldSet> fieldSets)
        {
            string s = string.Empty;
            Dictionary<String, String> dic = new Dictionary<String, String>();
            Dictionary<String, String> col = new Dictionary<String, String>();

            //确定事件函数名
            foreach (var fieldSet in fieldSets)
            {
                foreach (var item in fieldSet.Items)
                {
                    if (item.ControlType == PbControlType.Grid) continue;

                    string funcname = string.Empty;
                    if (item.ControlType == PbControlType.ComboBox || item.ControlType == PbControlType.DataHelpEdit)
                    {
                        funcname = "helpselected";
                    }
                    else
                    {
                        funcname = "itemchanged";
                    }
                    col.Add(item.Name, funcname);
                }
            }

            //确定事件触发字段
            foreach (var fieldSet in fieldSets)
            {
                foreach (var item in fieldSet.Items)
                {
                    if (item.ControlType == PbControlType.Grid) continue;

                    var temp = (PbBaseTextInfo)item;

                    string[] expressions = new string[3];
                    expressions[0] = temp.VisibleExpressionImp.Expression;
                    expressions[1] = temp.IsMustInputExpressionImp.Expression;
                    expressions[2] = temp.IsReadOnlyExpressionImp.Expression;

                    foreach (var exp in expressions)
                    {
                        if (!string.IsNullOrEmpty(exp) && exp.IndexOf('(') != (-1) && exp.IndexOf(')') != (-1))
                        {
                            // textcol_2.visible=1	if( ddlbcol_1='2', 0, 1 )
                            int leftBracket = exp.IndexOf('(');
                            int rightBracket = exp.IndexOf(')');
                            string innerStr = exp.Substring(leftBracket + 1, rightBracket - leftBracket - 1).Trim();
                            char[] operatorArr = { '=', '>', '<', '!' };
                            string changeCol = innerStr.Split(operatorArr)[0].Trim();

                            if (!dic.ContainsKey(changeCol))
                            {
                                dic.Add(changeCol, col[changeCol]);
                            }
                        }
                    }
                }
            }

            foreach (var item in dic)
            {
                s += string.Format("Ext.getCmp('{0}').fireEvent('{1}');", item.Key, item.Value);
                s += string.Format("\r\n");
            }

            return s;
        }

        //自定义帮助列添加外部查询条件
        public static string GetOutFilter(TableLayoutForm form, List<FieldSet> fieldSets, List<GridPanel> gridPanels)
        {
            StringBuilder sb = new StringBuilder();
            string outfilter = string.Empty;

            if (form.AllFields.Count > 0)
            {
                foreach (var item in form.AllFields)
                {
                    if (item.XType == "ngCustomFormHelp" || item.CmpName == "WbsHelpField" || item.CmpName == "CbsHelpField")
                    {
                        SetOutFilterForm(ref sb, item);
                    }
                }
            }

            foreach (var fieldSet in fieldSets)
            {
                foreach (var item in fieldSet.AllFields)
                {
                    if (item.XType == "ngCustomFormHelp" || item.CmpName == "WbsHelpField" || item.CmpName == "CbsHelpField")
                    {
                        SetOutFilterForm(ref sb, item);
                    }
                }
            }

            foreach (var gridPanel in gridPanels)
            {
                foreach (var item in gridPanel.Columns)
                {
                    if (item.editor.XType == "ngCustomFormHelp" || item.editor.CmpName == "WbsHelpField" || item.editor.CmpName == "CbsHelpField")
                    {
                        SetOutFilterGrid(ref sb, item, gridPanel.TableName);
                    }
                }
            }

            return sb.ToString();
        }

        private static void SetOutFilterForm(ref StringBuilder sb, ExtControlBase item)
        {
            string outfilter = string.Empty;

            NGCommonHelp commonHelp = new NGCommonHelp();
            commonHelp = item as NGCommonHelp;

            //wbs和cbs帮助要特殊处理
            if (commonHelp.CmpName == "WbsHelpField" || commonHelp.CmpName == "CbsHelpField")
            {
                sb.Append(string.Format(@"
                    Ext.getCmp('{0}').addListener('beforetriggerclick', function () {{
                        Ext.getCmp('{0}').setPc( Ext.getCmp('{1}').getValue() );
                    }});
                    ", commonHelp.Name, "pc")
                );

                return;
            }

            if (string.IsNullOrEmpty(commonHelp.OutFilter)) return;

            string[] cols = commonHelp.OutFilter.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (var col in cols)
            {
                outfilter += string.Format(",{0}:Ext.getCmp('{0}').getValue()", col);
            }

            outfilter = "{" + outfilter.Substring(1, outfilter.Length - 1) + "}";

            sb.Append(string.Format(@"
                Ext.getCmp('{0}').addListener('beforetriggerclick', function () {{
                    Ext.getCmp('{0}').setOutFilter({1});
                }});
                ", commonHelp.Name, outfilter)
            );
        }

        private static void SetOutFilterGrid(ref StringBuilder sb, ExtGridColumn item, string tableName)
        {
            string outfilter = string.Empty;

            //wbs和cbs帮助要特殊处理
            if (item.editor.CmpName == "WbsHelpField" || item.editor.CmpName == "CbsHelpField")
            {
                sb.Append(string.Format(@"
                    {2}grid.getColumn('{0}').getEditor().addListener('beforetriggerclick', function () {{
                        if ({2}grid.getColumn('pc')) {{
                            var data = {2}grid.getSelectionModel().getSelection();                            
                            {2}grid.getColumn('{0}').getEditor().setPc( data[0].get('{1}') );
                        }}
                        else {{
                            {2}grid.getColumn('{0}').getEditor().setPc( Ext.getCmp('{1}').getValue() );
		                }}
                    }});
                    ", item.DataIndex + "_name", "pc", tableName)
                );

                return;
            }

            if (string.IsNullOrEmpty(item.editor.OutFilter)) return;

            string[] cols = item.editor.OutFilter.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (var col in cols)
            {
                outfilter += string.Format(",{0}:Ext.getCmp('{0}').getValue()", col);
            }

            outfilter = "{" + outfilter.Substring(1, outfilter.Length - 1) + "}";

            sb.Append(string.Format(@"
                {2}grid.getColumn('{0}').getEditor().addListener('beforetriggerclick', function () {{
                    {2}grid.getColumn('{0}').getEditor().setOutFilter({1});
                }});
                ", item.DataIndex + "_name", outfilter, tableName)
            );
        }

        //model中增加固定隐藏列
        private static void AddModelHideColumn(ref StringBuilder sb, GridPanel gridPanel, string winType)
        {
            if (winType == "List")
            {
                if (!gridPanel.ColumnNames.Contains("ischeck"))
                {
                    sb.Append(@", {
                                    name: 'ischeck',
                                    type: 'string',
                                    mapping: 'ischeck'
                               }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("checkpsn"))
                {
                    sb.Append(@", {
                                    name: 'checkpsn',
                                    type: 'string',
                                    mapping: 'checkpsn'
                               }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("ocode"))
                {
                    sb.Append(@", {
                                    name: 'ocode',
                                    type: 'string',
                                    mapping: 'ocode'
                               }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("tr_date"))
                {
                    sb.Append(@", {
                                    name: 'tr_date',
                                    type: 'datetime',
                                    mapping: 'tr_date'
                               }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("voucher_flag"))
                {
                    sb.Append(@", {
                                    name: 'voucher_flag',
                                    type: 'string',
                                    mapping: 'voucher_flag'
                               }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("tr_num"))
                {
                    sb.Append(@", {
                                    name: 'tr_num',
                                    type: 'string',
                                    mapping: 'tr_num'
                               }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("tr_type"))
                {
                    sb.Append(@", {
                                    name: 'tr_type',
                                    type: 'string',
                                    mapping: 'tr_type'
                               }"
                    );
                }
            }
            else if (winType == "Edit")
            {
                //编辑窗口grid的model加固定列：附件会话guid，物资主键，分类主键，组合名称，组合名称，表关联列，分组列
                sb.Append(@", {
                           name: 'code',
                           type: 'string',
                           mapping: 'code'
                       }, {
                           name: 'phid_itemdata',
                           type: 'string',
                           mapping: 'phid_itemdata'
                       }, {
                           name: 'phid_resbs',
                           type: 'string',
                           mapping: 'phid_resbs'
                       }, {
                           name: 'res_propertys',
                           type: 'string',
                           mapping: 'res_propertys'
                       },{
                           name: 'rel_key1',
                           type: 'string',
                           mapping:'rel_key1'
                       },{
                           name: 's_groupfield',
                           type: 'string',
                           mapping:'s_groupfield'
                       }"
                );
            }

            //grid中有树控件，则加上隐藏的树列s_tree_id和s_tree_pid
            if (gridPanel.ColumnNames.Contains("s_tree_no"))
            {
                sb.Append(@", {
                           name: 's_tree_id',
                           type: 'string',
                           mapping: 's_tree_id'
                       }, {
                           name: 's_tree_pid',
                           type: 'string',
                           mapping: 's_tree_pid'
                       }"
                );
            }
        }

        //grid中增加固定隐藏列
        private static void AddGridHideColumn(ref StringBuilder sb, GridPanel gridPanel, string winType)
        {
            if (winType == "List")
            {
                //部分没有的列要加上并设为隐藏列
                if (!gridPanel.ColumnNames.Contains("ischeck"))
                {
                    sb.Append(@", {
                                header: '审核标志',
				                dataIndex: 'ischeck',
                                hidden: true
                           }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("checkpsn"))
                {
                    sb.Append(@", {
                                header: '审核人',
				                dataIndex: 'checkpsn',
                                hidden: true
                           }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("ocode"))
                {
                    sb.Append(@", {
                                header: '组织',
				                dataIndex: 'ocode',
                                hidden: true
                           }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("tr_date"))
                {
                    sb.Append(@", {
                                header: '凭证日期',
				                dataIndex: 'tr_date',
                                hidden: true
                           }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("voucher_flag"))
                {
                    sb.Append(@", {
                                header: '凭证标志',
				                dataIndex: 'voucher_flag',
                                hidden: true
                           }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("tr_num"))
                {
                    sb.Append(@", {
                                header: '凭证编号',
				                dataIndex: 'tr_num',
                                hidden: true
                           }"
                    );
                }
                if (!gridPanel.ColumnNames.Contains("tr_type"))
                {
                    sb.Append(@", {
                                header: '凭证类型',
				                dataIndex: 'tr_type',
                                hidden: true
                           }"
                    );
                }
            }
            else if (winType == "Edit")
            {
                //拼分组列的标签名
                string groupfieldheader = string.Empty;
                foreach (KeyValuePair<string, string> group in gridPanel.Groupfield)
                {
                    foreach (var col in gridPanel.Columns)
                    {
                        if (col.DataIndex == group.Key)
                        {
                            groupfieldheader += col.Header + "|";
                        }
                    }
                }
                if (groupfieldheader.Length > 0)
                {
                    groupfieldheader = groupfieldheader.TrimEnd('|');
                }
                else
                {
                    groupfieldheader = "分组列";
                }

                //加固定列：附件会话guid，物资主键，分类主键，组合名称，表关联列，分组列
                sb.Append(@", {
        				   header: '附件会话guid',
        				   dataIndex: 'code',
        				   itemId: 'code',
                           hidden: true
        			   }, {
        				   header: '物资主键',
        				   dataIndex: 'phid_itemdata',
        				   itemId: 'phid_itemdata',
                           hidden: true
        			   }, {
        				   header: '分类主键',
        				   dataIndex: 'phid_resbs',
        				   itemId: 'phid_resbs',
                           hidden: true
        			   }, {
        				   header: '组合名称',
        				   dataIndex: 'res_propertys',
        				   itemId: 'res_propertys',
                           hidden: true
        			   }, {
        				   header: '表关联列',
        				   dataIndex: 'rel_key1',
        				   itemId: 'rel_key1',
                           hidden: true
        			   }, {
        				   header: '" + groupfieldheader + @"',
        				   dataIndex: 's_groupfield',
        				   itemId: 's_groupfield',
                           hidden: true
        			   }"
                );

                /*
                {
                    header: '附件按钮',
                    xtype: 'actioncolumn',
                    align: 'center',
                    width: 70,
                    items: [{
                        //tooltip: 'attach',
                        icon: C_ROOT + 'NG3Resource/icons/attach.png',
                        handler: function(grid, rowIndex, colIndex) {
                            attgrid = grid;
                            grid.getSelectionModel().select(rowIndex);
                            OpenAttachment('grid');
                        }
                    }]
                }, 
                */
            }

            //grid中有树控件，则加上隐藏的树列s_tree_id和s_tree_pid
            if (gridPanel.ColumnNames.Contains("s_tree_no"))
            {
                sb.Append(@", {
        				   header: '树id',
        				   dataIndex: 's_tree_id',
        				   itemId: 's_tree_id',
                           hidden: true
        			   }, {
        				   header: '树pid',
        				   dataIndex: 's_tree_pid',
        				   itemId: 's_tree_pid',
                           hidden: true
        			   }"
                );
            }
        }

        //表头中增加固定隐藏列
        private static void AddFormHideColumn(ref StringBuilder sb, TableLayoutForm form)
        {
            sb.Append(@"{
                        xtype: 'ngText',
                        fieldLabel: '主键',
						name: 'phid',
                        id:'phid',
                        hidden: true
                    }, {
                        xtype: 'ngText',
                        fieldLabel: '工作流',
                        name: 'is_wf',
						id: 'is_wf',
                        hidden: true
                    }, {
                        xtype: 'ngText',
                        fieldLabel: '审核标志',
                        name: 'ischeck',
						id: 'ischeck',
                        hidden: true
                    }, {
                        xtype: 'ngText',
                        fieldLabel: '附件会话guid',
                        name: 'code',
						id: 'code',
                        hidden: true
                    }, {
                        xtype: 'ngText',
                        fieldLabel: '金格guid',
                        name: 'blobdocid',
						id: 'blobdocid',
                        hidden: true
                    }"
            );

            if (!form.ColumnNames.Contains("asr_flg"))
            {
                sb.Append(@", {
                        xtype: 'ngText',
                        fieldLabel: '附件标志',
                        name: 'asr_flg',
						id: 'asr_flg',
                        hidden: true
                    }"
                );
            }

            if (!form.ColumnNames.Contains("checkpsn"))
            {
                sb.Append(@", {
                        xtype: 'ngText',
                        fieldLabel: '审核人',
                        name: 'checkpsn',
						id: 'checkpsn',
                        hidden: true
                    }"
                );
            }

            if (IsTask == "1")
            {
                sb.Append(@", {
                            xtype: 'ngText',
                            fieldLabel: '流程id',
						    name: 's_task_wf',
                            id:'s_task_wf',
                            hidden: true
                        }, {
                            xtype: 'ngText',
                            fieldLabel: '节点id',
                            name: 's_task_node',
						    id: 's_task_node',
                            hidden: true
                        }, {
                            xtype: 'ngText',
                            fieldLabel: '模板id',
                            name: 's_task_phid',
						    id: 's_task_phid',
                            hidden: true
                        }"
                );
            }
        }

        //获取form的高度
        public static string GetFormHeight(TableLayoutForm form)
        {
            int minHeight = 0;

            foreach (var col in form.AllFields)
            {
                if (col.Visible == true)
                {
                    if (col.YPos + col.Height > minHeight)
                    {
                        minHeight = col.YPos + col.Height;
                    }
                }
            }

            return Convert.ToString(minHeight);
        }

        //获取fieldSet的高度
        public static string GetFormHeight(List<FieldSet> fieldSets)
        {
            int minHeight = 0;

            foreach (var fs in fieldSets)
            {
                foreach (var col in fs.AllFields)
                {
                    if (col.Visible == true)
                    {
                        if (col.YPos + col.Height > minHeight)
                        {
                            minHeight = col.YPos + col.Height;
                        }
                    }
                }
            }

            return Convert.ToString(minHeight);
        }


        //获取grid的合计和小计配置项
        public static string GetSummary(GridPanel gridPanel)
        {
            string s = string.Empty;
            string sumStr = "{ ftype: 'summary', dock: 'bottom' }";
            string subStr = "{ ftype: 'groupingsummary' }";

            if (gridPanel.Sumdic.Count > 0 && gridPanel.Subdic.Count > 0)
            {
                s = string.Format("otype == 'view' ? [{0}] : [{1}]", sumStr + "," + subStr, sumStr);
            }
            else if (gridPanel.Sumdic.Count > 0)
            {
                s = string.Format("[{0}]", sumStr);
            }
            else if (gridPanel.Subdic.Count > 0)
            {
                s = string.Format("otype == 'view' ? [{0}] : []", subStr);
            }
            else
            {
                s = "[]";
            }

            return s;
        }

        //获取guid
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        //获取1999-1-1到现在的秒数，做时间戳用
        public static string GetJsVersion()
        {
            if (string.IsNullOrEmpty(JsVersion))
            {
                TimeSpan ts = DateTime.Now - DateTime.Parse("1999-1-1");
                JsVersion = ts.TotalSeconds.ToString();

                if (JsVersion.IndexOf('.') > 0)
                {
                    JsVersion = JsVersion.Substring(0, JsVersion.IndexOf('.'));
                }
            }

            return string.IsNullOrEmpty(JsVersion) ? "123456" : JsVersion;
        }

        //获取附件单据体列
        public static string GetAsrGridColumns(GridPanel gridPanel)
        {
            StringBuilder sb = new StringBuilder();

            int totalwidth = 0;
            for (int i = 0; i < gridPanel.Columns.Count; i++)
            {
                totalwidth += gridPanel.Columns[i].Width;
            }

            double sumwidth = 0;
            for (int i = 0; i < gridPanel.Columns.Count; i++)
            {
                string header = string.Empty;
                switch (gridPanel.Columns[i].DataIndex)
                {
                    case "asr_name": header = "附件名称"; break;
                    case "logid": header = "上传人"; break;
                    case "asr_dt": header = "上传时间"; break;
                }

                double width = Math.Round(1.00 * gridPanel.Columns[i].Width / totalwidth, 2);

                if (width + sumwidth > 1)
                {
                    width = 1.00 - sumwidth;
                }
                else
                {
                    sumwidth += width;
                }

                sb.Append("{");
                sb.Append(string.Format(
                           @"header: '{0}',
        				   dataIndex: '{1}',
        				   itemId: '{1}',
                           width: '{2}'", header, gridPanel.Columns[i].DataIndex, width * 100 + "%"));
                sb.Append("}");

                if (i != gridPanel.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        #endregion web页面方法
    }
}