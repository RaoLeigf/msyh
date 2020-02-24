using System;
using System.Collections.Generic;
using System.Text;
using SUP.CustomForm.DataEntity.Container;
using SUP.CustomForm.DataEntity.Control;
using SUP.CustomForm.DataEntity.AppControl;
using NG3.Metadata.UI.PowserBuilder.Controls;

namespace SUP.CustomForm.ClientGen
{
    public class CommonConvert
    {
        string sb = string.Empty;
        public string FormTableName
        {
            get;
            set;
        }

        //form列获取
        public string GetngCustomFormHelp(ExtControlBase controlBase)
        {
            NGCommonHelp commonHelp = new NGCommonHelp();
            commonHelp = controlBase as NGCommonHelp;

            string colXtype = commonHelp.XType;

            //是多选帮助
            if (commonHelp.MultiSelect)
            {
                colXtype = "ngCustFormMutilHelp";
            }

            sb = string.Format(@"
                    xtype: '{0}', 
                    fieldLabel: Lang.{2}||'{1}', 
                    name: '{3}', 
                    id: '{3}',
                    itemId: '{3}', 
                    mustInput: {4},
                    colspan: {5},
                    valueField: '{6}',
                    displayField: '{7}',
                    listFields: '{8}',
                    listHeadTexts: '{9}',
                    helpid: '{10}',
                    hidden: {11},
                    readOnly: {12},
                    x: {13},
                    y: {14},
                    width: {15},
                    value: '{16}',
                    fieldStyle:'color:{17}',
                    labelStyle:'color:{18}'
                 ", colXtype, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                    controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                    commonHelp.ValueField, commonHelp.DisplayField, commonHelp.ListFields,
                    commonHelp.ListHeadTexts, commonHelp.Helpid, controlBase.Visible.Equals(true) ? "false" : "true",
                    controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos, controlBase.YPos,
                    controlBase.Width, controlBase.DefaultValue, controlBase.FieldStyle, controlBase.LabelStyle);
            return sb;
        }

        public string GetngRichHelp(ExtControlBase controlBase)
        {
            NGCommonHelp richHelp = new NGCommonHelp();
            richHelp = controlBase as NGCommonHelp;

            //是组件类型
            if (!string.IsNullOrEmpty(richHelp.CmpName))
            {
                string isValidate = "false";
                if (richHelp.CmpName == "WbsHelpField" || richHelp.CmpName == "CbsHelpField")
                {
                    isValidate = "true";
                }

                sb = string.Format(@"
                    xtype: '{0}', 
                    fieldLabel: Lang.{2}||'{1}', 
                    name: '{3}', 
                    id: '{3}',
                    itemId: '{3}',
                    mustInput: {4},
                    colspan: {5},
                    //helpid: '{6}',
                    ORMMode: {7},
                    hidden: {8},
                    readOnly: {9},
                    x: {10},
                    y: {11},
                    width: {12},
                    value: '{13}',
                    fieldStyle: 'color:{14};',
                    labelStyle: 'color:{15}',
                    acceptInput: {16}
                 ", richHelp.CmpName, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                    controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                    richHelp.Helpid, "true", controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false",
                    controlBase.XPos, controlBase.YPos, controlBase.Width, controlBase.DefaultValue, controlBase.FieldStyle, controlBase.LabelStyle, isValidate);
            }
            else
            {
                string colXtype = richHelp.XType;

                //是多选帮助
                if (richHelp.MultiSelect)
                {
                    colXtype = "ngMultiRichHelp";
                }

                sb = string.Format(@"
                    xtype: '{0}', 
                    fieldLabel: Lang.{2}||'{1}', 
                    name: '{3}', 
                    id: '{3}',
                    itemId: '{3}',
                    mustInput: {4},
                    colspan: {5},
                    valueField: '{6}',
                    displayField: '{7}',
                    listFields: '{8}',
                    listHeadTexts: '{9}',
                    helpid: '{10}',
                    ORMMode: {11},
                    hidden: {12},
                    readOnly: {13},
                    x: {14},
                    y: {15},
                    width: {16},
                    value: '{17}',
                    fieldStyle:'color:{18}',
                    labelStyle:'color:{19}'
                 ", colXtype, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                    controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                    richHelp.ValueField, richHelp.DisplayField, richHelp.ListFields, richHelp.ListHeadTexts, richHelp.Helpid,
                    "false", controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false",
                    controlBase.XPos, controlBase.YPos, controlBase.Width, controlBase.DefaultValue, controlBase.FieldStyle, controlBase.LabelStyle);
            }

            return sb;
        }

        public string GetngRadio(ExtControlBase controlBase)
        {
            NGRadio ngRadio = new NGRadio();
            ngRadio = controlBase as NGRadio;
            sb = string.Format(@"
                    xtype: '{0}',
                    fieldLabel: Lang.{2}||'{1}',
                    id: '{3}_code',
                    itemId: '{3}_code',
                    name: '{3}_name',
                    mustInput: {4},
                    colspan: {5},
                    hidden: {6},
                    readOnly: {7},
                    x:{8},
                    y:{9},
                    width:{10},
                    vertical: true,
                    labelSeparator: '',
                    items:[
                 ", "radiogroup", controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                    controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                    controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos,
                    controlBase.YPos, controlBase.Width);

            for (int i = 0; i < ngRadio.Items.Count; i++)
            {
                //若是必输项，则默认选中第一项
                if (controlBase.MustInput && i == 0)
                {
                    sb += "\t\t\t\t\t\t\t\t{" + ngRadio.Items[i] + ", checked: true" + "}";
                }
                else
                {
                    sb += "\t\t\t\t\t\t\t\t{" + ngRadio.Items[i] + "}";
                }

                if (i == ngRadio.Items.Count - 1)
                {
                    sb += "\r\n";
                }
                else
                {
                    sb += ",\r\n";
                }
            }
            sb += "\t\t\t\t\t\t\t]\r\n\t\t\t\t\t\t";
            return sb;
        }

        public string GetngComboBox(ExtControlBase controlBase)
        {
            if (controlBase is NGComboBox)
            {
                sb = string.Format(@"
                                        xtype: '{0}', 
                                        fieldLabel: Lang.{2}||'{1}', 
                                        id: '{3}', 
                                        name: '{3}',
                                        mustInput: {4},
                                        colspan: {5},
                                        hidden: {6},
                                        readOnly: {7},
                                        x: {8},
                                        y: {9},
                                        width: {10},
                                        value: '{11}',
                                        fieldStyle:'color:{12}',
                                        labelStyle:'color:{13}',
                                        editable: false,
                                        trigger1Cls: 'x-form-clear-trigger',
                                        onTrigger1Click: function () {{ this.clearValue(); }},
                                        queryMode: 'local',
            			                valueField: 'code',
            			                displayField: 'name',
                                        data:[
                                     ", "ngComboBox", controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                                        controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                                        controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false",
                                        controlBase.XPos, controlBase.YPos, controlBase.Width, controlBase.DefaultValue, controlBase.FieldStyle, controlBase.LabelStyle);

                NGComboBox ngComboBox = controlBase as NGComboBox;
                for (int i = 0; i < ngComboBox.Data.Count; i++)
                {
                    sb += "\t\t\t\t\t\t\t\t{" + ngComboBox.Data[i] + "}";
                    if (i == ngComboBox.Data.Count - 1)
                    {
                        sb += "\r\n";
                    }
                    else
                    {
                        sb += ",\r\n";
                    }
                }
            }
            else if (controlBase is NGCommonHelp)
            {
                sb = string.Format(@"
                                    xtype: '{0}', 
                                    fieldLabel: '{1}',
                                    name: '{2}', 
                                    id: '{2}',
                                    itemId: '{2}',                                    
                                    mustInput: {3},
                                    colspan: {4},
                                    hidden: {5},
                                    readOnly: {6},
                                    x: {7},
                                    y: {8},
                                    width: {9},
                                    editable: false,
                                    trigger1Cls: 'x-form-clear-trigger',
                                    onTrigger1Click: function () {{ this.clearValue(); }},
                                    queryMode: 'local',
            			            valueField: 'code',
            			            displayField: 'name',
                                    listFields: 'code,name',
            			            listHeadTexts: '代码,名称',
                                    data:[
                                 ", "ngComboBox", controlBase.FieldLabel, controlBase.Name,
                                    controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                                    controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos,
                                    controlBase.YPos, controlBase.Width);

                NGCommonHelp commonHelp = controlBase as NGCommonHelp;
                for (int i = 0; i < commonHelp.Data.Count; i++)
                {
                    sb += "\t\t\t\t\t\t\t\t{" + commonHelp.Data[i] + "}";
                    if (i == commonHelp.Data.Count - 1)
                    {
                        sb += "\r\n";
                    }
                    else
                    {
                        sb += ",\r\n";
                    }
                }
            }

            sb += "\t\t\t\t\t\t\t]\r\n\t\t\t\t\t\t";

            return sb;
        }

        public string GetngCheckbox(ExtControlBase controlBase)
        {
            NGCheckbox ngcheckbox = new NGCheckbox();
            ngcheckbox = controlBase as NGCheckbox;
            sb = string.Format(@"
                            xtype: '{0}', 
                            boxLabel: Lang.{2}||'{1}', 
                            name: '{3}', 
                            id: '{3}',
                            itemId: '{3}',
                            mustInput: {4},
                            colspan: {5},
                            readOnly: {6},
                            x:{7},
                            y:{8},
                            width:{9},
                            inputValue:1
                         ", ngcheckbox.XType, ngcheckbox.FieldLabel, FormTableName + "_" + ngcheckbox.Name, ngcheckbox.Name,
                            controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                            controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos, controlBase.YPos, controlBase.Width);
            return sb;
        }

        public string GetDate(ExtControlBase controlBase)
        {
            string colXtype = controlBase.XType;

            //掩码是yyyy-mm-dd hh:mm:ss
            if (controlBase.Format.Length > 11)
            {
                colXtype = "ngDateTime";
            }

            sb = string.Format(@"
                            xtype: '{0}', 
                            fieldLabel: Lang.{2}||'{1}', 
                            name: '{3}', 
                            id: '{3}',
                            itemId: '{3}',
                            mustInput: {4},
                            colspan: {5},
                            hidden: {6},
                            readOnly: {7},
                            x:{8},
                            y:{9},
                            width:{10},
                            value:'{11}',
                            fieldStyle:'color:{12}',
                            labelStyle:'color:{13}'
                         ", colXtype, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                            controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                            controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos,
                            controlBase.YPos, controlBase.Width, controlBase.DefaultValue, controlBase.FieldStyle, controlBase.LabelStyle);
            return sb;
        }

        public string GetngTextArea(ExtControlBase controlBase)
        {
            sb = string.Format(@"
                            xtype: '{0}', 
                            fieldLabel: Lang.{2}||'{1}', 
                            name: '{3}', 
                            id: '{3}',
                            itemId: '{3}',
                            mustInput: {4},
                            colspan: {5},
                            hidden: {6},
                            readOnly: {7},
                            x: {8},
                            y: {9},
                            width: {10},
                            height: {11},
                            value: '{12}',
                            emptyText: '{13}',
                            maxLength: {14},
                            fieldStyle:'color:{15}',
                            labelStyle:'color:{16}'
                         ", controlBase.XType, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                            controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                            controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos,
                            controlBase.YPos, controlBase.Width, controlBase.Height, controlBase.DefaultValue, controlBase.Tag,
                            controlBase.MaxLength == 0 ? "Number.MAX_VALUE" : Convert.ToString(controlBase.MaxLength), controlBase.FieldStyle, controlBase.LabelStyle);
            return sb;
        }

        public string GetngText(ExtControlBase controlBase)
        {
            //单据编号设为只读
            if (controlBase.Name == "bill_no")
            {
                controlBase.Protect = true;

                if (controlBase.MaxLength < 40)
                {
                    controlBase.MaxLength = 40;
                }
            }

            sb = string.Format(@"
                            xtype: '{0}', 
                            fieldLabel: Lang.{2}||'{1}', 
                            name: '{3}', 
                            id: '{3}',
                            itemId: '{3}',
                            mustInput: {4},
                            colspan: {5},
                            hidden: {6},
                            readOnly: {7},
                            x: {8},
                            y: {9},
                            width: {10},
                            value: '{11}',
                            emptyText: '{12}',
                            maxLength: {13},
                            fieldStyle:'color:{14}',
                            labelStyle:'color:{15}'
                         ", controlBase.XType, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                            controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                            controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos,
                            controlBase.YPos, controlBase.Width, controlBase.DefaultValue, controlBase.Tag,
                            controlBase.MaxLength == 0 ? "Number.MAX_VALUE" : Convert.ToString(controlBase.MaxLength),
                            controlBase.FieldStyle, controlBase.LabelStyle);

            return sb;
        }

        public string GetngNumber(ExtControlBase controlBase)
        {
            string showPercent = "false";

            //掩码中有%
            if (controlBase.EditMask.IndexOf("%") >= 0)
            {
                showPercent = "true";
            }

            //小数位数
            int decimalPrecision = 2;
            if (string.IsNullOrEmpty(controlBase.EditMask))
            {
                controlBase.EditMask = "0,000";
                decimalPrecision = 0;
            }
            else if (controlBase.EditMask.IndexOf(".") >= 0)
            {
                decimalPrecision = controlBase.EditMask.Split(new string[] { "." }, StringSplitOptions.None)[1].Length;
                if (controlBase.EditMask.IndexOf("%") >= 0)
                {
                    decimalPrecision ++;
                }
            }
            else
            {
                decimalPrecision = 0;
            }

            sb = string.Format(@"
                            xtype: '{0}', 
                            fieldLabel: Lang.{2}||'{1}', 
                            name: '{3}', 
                            id: '{3}',
                            itemId: '{3}',
                            mustInput: {4},
                            colspan: {5},
                            hidden: {6},
                            readOnly: {7},
                            x: {8},
                            y: {9},
                            width: {10},
                            value: '{11}',
                            showPercent: {12},
                            fieldStyle:'text-align:right;color: {13}',
                            decimalPrecision: {14},
                            labelStyle:'color:{15}',
                            isMarginRight: false
                         ", controlBase.XType, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                            controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                            controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos,
                            controlBase.YPos, controlBase.Width, controlBase.DefaultValue, showPercent, controlBase.FieldStyle, decimalPrecision, controlBase.LabelStyle);

            return sb;
        }

        public string GetOtherForm(ExtControlBase controlBase)
        {
            sb = string.Format(@"
                            xtype: '{0}', 
                            fieldLabel: Lang.{2}||'{1}', 
                            name: '{3}', 
                            id: '{3}',
                            itemId: '{3}',
                            mustInput: {4},
                            colspan: {5},
                            hidden: {6},
                            readOnly: {7},
                            x: {8},
                            y: {9},
                            width: {10},
                            value: '{11}'
                         ", controlBase.XType, controlBase.FieldLabel, FormTableName + "_" + controlBase.Name, controlBase.Name,
                            controlBase.MustInput.Equals(true) && controlBase.Visible.Equals(true) ? "true" : "false", controlBase.ColSpan,
                            controlBase.Visible.Equals(true) ? "false" : "true", controlBase.Protect.Equals(true) ? "true" : "false", controlBase.XPos,
                            controlBase.YPos, controlBase.Width, controlBase.DefaultValue);
            return sb;
        }

        public string GetButton(ExtControlBase controlBase)
        {
            NGButton ngButton = new NGButton();
            ngButton = controlBase as NGButton;

            sb = string.Format(@"
                            xtype: '{0}',
                            name: '{1}',
                            id: '{2}',
                            text: Lang.{1}||'{3}',
                            colspan: {4},
                            hidden: {5},
                            x:{6},
                            y:{7},
                            width:{8}
                         ", controlBase.XType, controlBase.Name, controlBase.Name, ngButton.FieldLabel,
                            '2', controlBase.Visible.Equals(true) ? "false" : "true", controlBase.XPos, controlBase.YPos, controlBase.Width);
            return sb;
        }

        public string GetDisplayField(ExtControlBase controlBase, int ColumnsPerRow)
        {
            NGLabel ngLabel = new NGLabel();
            ngLabel = controlBase as NGLabel;

            sb = string.Format(@"
                            xtype: '{0}',
                            value: Lang.{2}||'{1}',
                            name: '{3}',
                            id: '{3}',
                            itemId: '{3}',
                            colspan: {4},
                            hidden: {5},
                            x:{6},
                            y:{7},
                            width:{8},
                            fieldStyle:'{9}',
                            labelSeparator: ''
                         ", controlBase.XType, ngLabel.Text, FormTableName + "_" + controlBase.Name, controlBase.Name,
                            ngLabel.IsTitle.Equals(true) && ColumnsPerRow != 0 ? ColumnsPerRow.ToString() : controlBase.ColSpan.ToString(),
                            controlBase.Visible.Equals(true) ? "false" : "true", controlBase.XPos, controlBase.YPos, controlBase.LabelWidth, controlBase.FieldStyle);

            if (ngLabel.IsTitle)
            {
                sb += ",anchor: '100%'";
            }
            return sb;
        }


        //事件监听函数
        public StringBuilder CheckColumnEvent(string sts, StringBuilder sb)  //只读checkbox
        {
            sb.Append("\r\n" + sts + "\tlisteners: {");
            sb.Append("\r\n" + sts + "\t\tbeforecheckchange: function(chk, rowIndex, checked, eOpts) {");
            sb.Append("\r\n" + sts + "\t\t\treturn false;");
            sb.Append("\r\n" + sts + "\t\t}");
            sb.Append("\r\n" + sts + "\t},");
            return sb;
        }

        public StringBuilder CheckboxEvent(GridPanel gridPanel, int i, string sts, StringBuilder sb)
        {
            sb.Append("\r\n" + sts + "\tlisteners: {");
            sb.Append("\r\n" + sts + "\t\tcheckchange: function(me,rowIndex,checked) {");
            sb.Append("\r\n" + sts + "\t\t\tif(checked == true){");
            sb.Append("\r\n" + sts + "\t\t\t\t" + string.Format("{0}grid.getStore().data.items[rowIndex].data.{1}='1';", gridPanel.TableName, gridPanel.Columns[i].DataIndex));

            sb.Append("\r\n" + sts + "\t\t\t} else if(checked == false){");
            sb.Append("\r\n" + sts + string.Format("\t\t\t\t{0}grid.getStore().data.items[rowIndex].data.{1}='0';", gridPanel.TableName, gridPanel.Columns[i].DataIndex));
            sb.Append("\r\n" + sts + "\t\t\t}");

            sb.Append("\r\n" + sts + "\t\t\t" + "var e = {};");
            sb.Append("\r\n" + sts + "\t\t\t" + "e.field = me.dataIndex;");
            sb.Append("\r\n" + sts + "\t\t\t" + "e.value = checked == true ? '1' : '0';");
            sb.Append("\r\n" + sts + "\t\t\t" + "e.originalValue = checked == true ? '0' : '1';");
            sb.Append("\r\n" + sts + "\t\t\t" + string.Format("e.record = {0}store.getAt(rowIndex);", gridPanel.TableName));
            sb.Append("\r\n" + sts + "\t\t\t" + string.Format("{0}cellEditing.fireEvent('edit', me, e);", gridPanel.TableName));
            sb.Append("\r\n" + sts + "\t\t}");

            //附件多选框只读
            if (gridPanel.Columns[i].DataIndex == "asr_flg")
            {
                sb.Append("\r\n" + sts + "\t\t,");
                sb.Append("\r\n" + sts + "\t\tbeforecheckchange: function(chk, rowIndex, checked, eOpts) {");
                sb.Append("\r\n" + sts + "\t\t\treturn false;");
                sb.Append("\r\n" + sts + "\t\t}");
            }

            sb.Append("\r\n" + sts + "\t},");
            return sb;
        }

        public StringBuilder ComboxEvent(StringBuilder sb, string sts, string dataStr)
        {
            sb.Append("\r\n" + sts + "\trenderer: function (code) {");
            sb.Append(string.Format("\r\n" + sts + "\t\tvar data = {0};", dataStr));
            sb.Append("\r\n" + sts + "\t\tExt.each(data, function (rec) {");
            sb.Append("\r\n" + sts + "\t\t\t\tif (rec['code'] == code)");
            sb.Append("\r\n" + sts + "\t\t\t\t{");
            sb.Append("\r\n" + sts + "\t\t\t\t\t\tcode = rec['name'];");
            sb.Append("\r\n" + sts + "\t\t\t\t\t\treturn false;");
            sb.Append("\r\n" + sts + "\t\t\t\t}");
            sb.Append("\r\n" + sts + "\t\t});");
            sb.Append("\r\n" + sts + "\t\treturn code;");
            sb.Append("\r\n" + sts + "\t}");
            return sb;
        }

        public StringBuilder GetResmaster(GridPanel gridPanel, int i, StringBuilder sb, string sts)
        {
            bool istreegrid = gridPanel.ColumnNames.Contains("s_tree_no");  //是否树grid

            sb.Append(string.Format("\r\n" + sts + "\tdataIndex: '{0}',", "c_name"));
            sb.Append(string.Format("\r\n" + sts + "\twidth: {0},", gridPanel.Columns[i].Width > ((gridPanel.Columns[i].Header.Length + 4) * 17) ? gridPanel.Columns[i].Width : ((gridPanel.Columns[i].Header.Length + 4) * 17)));
            sb.Append(string.Format("\r\n" + sts + "\tsortable: {0},", gridPanel.Columns[i].Sortable.Equals(true) ? "true" : "false"));
            sb.Append(string.Format("\r\n" + sts + "\teditor: "));
            sb.Append("{");
            sb.Append(string.Format("\r\n" + sts + "\t\txtype: '{0}',", "ItemDataHelpField"));
            sb.Append(string.Format("\r\n" + sts + "\t\tvalueField: '{0}',", gridPanel.Columns[i].editor.ValueField));
            sb.Append(string.Format("\r\n" + sts + "\t\tdisplayField: '{0}',", gridPanel.Columns[i].editor.DisplayField));
            sb.Append(string.Format("\r\n" + sts + "\t\tallowBlank: {0},", gridPanel.Columns[i].editor.AllowBlank.Equals(true) ? "true" : "false"));
            sb.Append(string.Format("\r\n" + sts + "\t\tmuilt: {0},", "false"));
            sb.Append(string.Format("\r\n" + sts + "\t\tORMMode: {0},", "false"));

            //if (!gridPanel.Columns[i].Protect)
            //{
            var gridname = gridPanel.TableName + "grid";
            sb.Append("\r\n" + sts + "\t\tlisteners: {");
            sb.Append("\r\n" + sts + "\t\t\thelpselected: function (obj) {");
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\tvar data = {0}.getSelectionModel().getSelection();", gridname));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\tvar oriValue = data[0].get('{0}');", "itemid"));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\t if({2}.getColumn('{0}')) {{ data[0].set('{0}', {1});}}", "itemid", "obj.code", gridname));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\t if({2}.getColumn('{0}')) {{ data[0].set('{0}', {1});}}", "c_name", "obj.data.ItemName", gridname));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\t if({2}.getColumn('{0}')) {{ data[0].set('{0}', {1});}}", "spec", " obj.data.Spec", gridname));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\t if({2}.getColumn('{0}')) {{ data[0].set('{0}', {1});}}", "msunit_name", " obj.data.PhidMsunit_EXName", gridname));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\t if({2}.getColumn('{0}')) {{ data[0].set('{0}', {1});}}", "phid_itemdata", " obj.data.PhidItemdata", gridname));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\t if({2}.getColumn('{0}')) {{ data[0].set('{0}', {1});}}", "phid_resbs", " obj.data.phid_resbs", gridname));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\t if({2}.getColumn('{0}')) {{ data[0].set('{0}', {1});}}", "res_propertys", " obj.data.ResPropertys", gridname));

            sb.Append("\r\n");
            sb.Append("\r\n" + sts + "\t\t\t\t" + "var e = {};");
            sb.Append("\r\n" + sts + "\t\t\t\t" + string.Format("e.field = '{0}';", gridPanel.Columns[i].DataIndex));
            sb.Append("\r\n" + sts + "\t\t\t\t" + string.Format("e.value = data[0].get('{0}');", gridPanel.Columns[i].DataIndex));
            sb.Append("\r\n" + sts + "\t\t\t\t" + "e.originalValue = oriValue;");
            sb.Append("\r\n" + sts + "\t\t\t\t" + "e.record = data[0];");
            sb.Append("\r\n" + sts + "\t\t\t\t" + string.Format("{0}cellEditing.fireEvent('edit', null, e);", gridPanel.TableName));

            sb.Append("\r\n" + sts + "\t\t\t}");
            sb.Append("\r\n" + sts + "\t\t}");
            //}
            sb.Append("\r\n" + sts + "\t},");

            if (!string.IsNullOrEmpty(gridPanel.Columns[i].RgbColor) || !string.IsNullOrEmpty(gridPanel.Columns[i].BackgroundColor))
            {
                //设置字体颜色和背景颜色
                sb.Append("\r\n" + sts + "\trenderer:function(val,m){");
                sb.Append("\r\n" + sts + "\tif(typeof(m)!='undefined'){");
                sb.Append("\r\n" + sts + "\t\tm.style=\"color:" + gridPanel.Columns[i].RgbColor + "background-color:" + gridPanel.Columns[i].BackgroundColor + "\";");
                sb.Append("\r\n" + sts + "\t}");
                sb.Append("\r\n" + sts + "\t\treturn val; ");
                sb.Append("\r\n" + sts + "\t},");
            }

            return sb;
        }

        public StringBuilder GetOtherGridColumn(GridPanel gridPanel, int i, StringBuilder sb, string sts)
        {
            bool istreegrid = gridPanel.ColumnNames.Contains("s_tree_no");  //是否树grid
            if (gridPanel.Columns[i].DataIndex != "s_tree_name")
            {
                sb.Append(string.Format("\r\n" + sts + "\tdataIndex: '{0}',", gridPanel.Columns[i].DataIndex + "_name"));
                sb.Append(string.Format("\r\n" + sts + "\titemId: '{0}',", gridPanel.Columns[i].DataIndex + "_name"));
                sb.Append(string.Format("\r\n" + sts + "\twidth: {0},", gridPanel.Columns[i].Width > ((gridPanel.Columns[i].Header.Length + 4) * 17) ? gridPanel.Columns[i].Width : ((gridPanel.Columns[i].Header.Length + 4) * 17)));
                sb.Append(string.Format("\r\n" + sts + "\tsortable: {0},", gridPanel.Columns[i].Sortable.Equals(true) ? "true" : "false"));
                sb.Append(string.Format("\r\n" + sts + "\teditor: "));
                sb.Append("{");
            }
            else
            {
                sb.Append(string.Format("\r\n" + sts + "\teditor: "));
                sb.Append("{");
            }

            //是组件类型
            if (!string.IsNullOrEmpty(gridPanel.Columns[i].editor.CmpName))
            {
                sb.Append(string.Format("\r\n" + sts + "\t\txtype: '{0}',", gridPanel.Columns[i].editor.CmpName));
                sb.Append(string.Format("\r\n" + sts + "\t\tisInGrid: true,"));
                sb.Append(string.Format("\r\n" + sts + "\t\tORMMode: {0},", "true"));
                sb.Append(string.Format("\r\n" + sts + "\t\tallowBlank: {0}", gridPanel.Columns[i].editor.AllowBlank.Equals(true) ? "true" : "false"));
            }
            else
            {
                string colXtype = gridPanel.Columns[i].editor.XType;

                //是多选帮助
                if (gridPanel.Columns[i].editor.MultiSelect)
                {
                    colXtype = "ngMultiRichHelp";
                }

                sb.Append(string.Format("\r\n" + sts + "\t\txtype: '{0}',", colXtype));
                sb.Append(string.Format("\r\n" + sts + "\t\tvalueField: '{0}',", gridPanel.Columns[i].editor.ValueField));
                sb.Append(string.Format("\r\n" + sts + "\t\tdisplayField: '{0}',", gridPanel.Columns[i].editor.DisplayField));
                sb.Append(string.Format("\r\n" + sts + "\t\tlistFields: '{0}',", gridPanel.Columns[i].editor.ListFields));
                sb.Append(string.Format("\r\n" + sts + "\t\tlistHeadTexts: '{0}',", gridPanel.Columns[i].editor.ListHeadTexts));
                sb.Append(string.Format("\r\n" + sts + "\t\thelpid: '{0}',", gridPanel.Columns[i].editor.Helpid));
                sb.Append(string.Format("\r\n" + sts + "\t\tisInGrid: true,"));
                sb.Append(string.Format("\r\n" + sts + "\t\tORMMode: {0},", "false"));
                sb.Append(string.Format("\r\n" + sts + "\t\tallowBlank: {0}", gridPanel.Columns[i].editor.AllowBlank.Equals(true) ? "true" : "false"));
            }

            //if (!gridPanel.Columns[i].Protect)
            //{
            sb.Append(",");
            sb.Append("\r\n" + sts + "\t\tlisteners: {");
            sb.Append("\r\n" + sts + "\t\t\thelpselected: function (obj) {");
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\tvar data = {0}.getSelectionModel().getSelection();", gridPanel.TableName + "grid"));
            sb.Append(string.Format("\r\n" + sts + "\t\t\t\tvar oriValue = data[0].get('{0}');", gridPanel.Columns[i].DataIndex));

            if (gridPanel.Columns[i].DataIndex != "s_tree_name")
            {
                sb.Append(string.Format("\r\n" + sts + "\t\t\t\tdata[0].set('{0}', obj.code);", gridPanel.Columns[i].DataIndex));
                sb.Append(string.Format("\r\n" + sts + "\t\t\t\tdata[0].set('{0}', obj.name);", gridPanel.Columns[i].DataIndex + "_name"));
            }
            else
            {
                sb.Append(string.Format("\r\n" + sts + "\t\t\t\tdata[0].set('{0}', obj.code);", "s_tree_no"));
                sb.Append(string.Format("\r\n" + sts + "\t\t\t\tdata[0].set('{0}', obj.name);", gridPanel.Columns[i].DataIndex));
                sb.Append(string.Format("\r\n" + sts + "\t\t\t\tdata[0].set('{0}', obj.code);", "s_tree_id"));
            }

            sb.Append("\r\n");
            sb.Append("\r\n" + sts + "\t\t\t\t" + "var e = {};");
            sb.Append("\r\n" + sts + "\t\t\t\t" + string.Format("e.field = '{0}';", gridPanel.Columns[i].DataIndex));
            sb.Append("\r\n" + sts + "\t\t\t\t" + string.Format("e.value = data[0].get('{0}');", gridPanel.Columns[i].DataIndex));
            sb.Append("\r\n" + sts + "\t\t\t\t" + "e.originalValue = oriValue;");
            sb.Append("\r\n" + sts + "\t\t\t\t" + "e.record = data[0];");
            sb.Append("\r\n" + sts + "\t\t\t\t" + string.Format("{0}cellEditing.fireEvent('edit', null, e);", gridPanel.TableName));

            sb.Append("\r\n" + sts + "\t\t\t}");
            sb.Append("\r\n" + sts + "\t\t}");
            //}
            sb.Append("\r\n" + sts + "\t},");

            if (!string.IsNullOrEmpty(gridPanel.Columns[i].RgbColor) || !string.IsNullOrEmpty(gridPanel.Columns[i].BackgroundColor))
            {
                //设置字体颜色和背景颜色
                sb.Append("\r\n" + sts + "\trenderer:function(val,m){");
                sb.Append("\r\n" + sts + "\tif(typeof(m)!='undefined'){");
                sb.Append("\r\n" + sts + "\t\tm.style=\"color:" + gridPanel.Columns[i].RgbColor + "background-color:" + gridPanel.Columns[i].BackgroundColor + "\";");
                sb.Append("\r\n" + sts + "\t}");
                sb.Append("\r\n" + sts + "\t\treturn val; ");
                sb.Append("\r\n" + sts + "\t},");
            }

            return sb;
        }

        public StringBuilder NotAbsForm(TableLayoutForm form, StringBuilder sb)
        {
            sb.Append("{" + "xtype: 'fieldset'," + string.Format(@"
                        columnsPerRow: {0},
                        title: '{1}',
                        collapsible: {2},
                        collapsed: {3},
                        border: {4},
                        ", form.ColumnsPerRow, "", "false", "false", "false"));

            sb.Append(@" fieldDefaults: {
                        margin: '0 10 8 0',
                        anchor: '100%',
                        msgTarget: 'side'
                    },
                    fieldRows:
                    [");
            return sb;
        }

        public StringBuilder NotAbsFieldSet(List<FieldSet> fieldSets, int i, StringBuilder sb)
        {
            FieldSet fieldSet = fieldSets[i];
            sb.Append("{" + "xtype: 'fieldset'," + string.Format(@"
                        columnsPerRow: {0},
                        title: '{1}',
                        collapsible: {2},
                        collapsed: {3},
                        border: {4},
                        listeners: {{
                            afterrender: function (me, eOpts ) {{
                                if ({5}) {{
                                    me.collapse();
                                }}
                            }}
                        }},
                        ", fieldSet.ColumnsPerRow, fieldSet.Title, fieldSet.Collapsible.Equals(true) ? "true" : "false",
                        "false", fieldSet.Border.Equals(true) ? "true" : "false", fieldSet.Collapsed.Equals(true) ? "true" : "false"));

            sb.Append(@" fieldDefaults: {
                        margin: '0 10 8 0',
                        anchor: '100%',
                        msgTarget: 'side'
                    },
                    fieldRows:
                    [");
            return sb;
        }

        public StringBuilder AbsFieldSet(List<FieldSet> fieldSets, int i, StringBuilder sb)
        {
            FieldSet fieldSet = fieldSets[i];
            if (i != fieldSets.Count - 1)
            {
                if (fieldSets[i].X < fieldSets[i + 1].X && fieldSets[i].Y == fieldSets[i + 1].Y)
                {
                    sb.Append("{" + "xtype: 'fieldset'," + string.Format(@"
                        isAbsoluteLayout: true,
                        columnsPerRow: {0},
                        title: '{1}',
                        x: {2},
                        y: {3},
                        height:{4},
                        width:{5},
                        collapsible: {6},
                        collapsed: {7},
                        border: {8},
                        listeners: {{
                            afterrender: function (me, eOpts ) {{
                                if ({9}) {{
                                    me.collapse();
                                }}
                            }}
                        }},
                        ", fieldSet.ColumnsPerRow, fieldSet.Title, fieldSet.X, fieldSet.Y, fieldSet.Height + 10, fieldSet.Width,
                        fieldSet.Collapsible.Equals(true) ? "true" : "false", "false",
                        fieldSet.Border.Equals(true) ? "true" : "false", fieldSet.Collapsed.Equals(true) ? "true" : "false"));

                    sb.Append(@" fieldDefaults: {
                        margin: '0 10 8 0',
                        msgTarget: 'side'
                    },
                    fieldRows:
                    [");
                }
                else
                {
                    sb.Append("{" + "xtype: 'fieldset'," + string.Format(@"
                        isAbsoluteLayout: true,
                        columnsPerRow: {0},
                        title: '{1}',
                        x: {2},
                        y: {3},
                        height:{4},
                        width:{5},
                        collapsible: {6},
                        collapsed: {7},
                        border: {8},
                        listeners: {{
                            afterrender: function (me, eOpts ) {{
                                if ({9}) {{
                                    me.collapse();
                                }}
                            }}
                        }},
                        ", fieldSet.ColumnsPerRow, fieldSet.Title, fieldSet.X, fieldSet.Y, fieldSet.Height + 10, fieldSet.Width,
                        fieldSet.Collapsible.Equals(true) ? "true" : "false", "false",
                        fieldSet.Border.Equals(true) ? "true" : "false", fieldSet.Collapsed.Equals(true) ? "true" : "false"));

                    sb.Append(@" fieldDefaults: {
                        anchor: '100%',
                        margin: '0 10 8 0',
                        msgTarget: 'side'
                    },     
                    fieldRows:
                    [");
                }
            }
            else if (i == fieldSets.Count - 1)
            {
                sb.Append("{" + "xtype: 'fieldset'," + string.Format(@"
                        isAbsoluteLayout: true,
                        columnsPerRow: {0},                   
                        title: '{1}',
                        x: {2},
                        y: {3},
                        height:{4},
                        width:{5},
                        collapsible: {6},
                        collapsed: {7},
                        border: {8},
                        listeners: {{
                            afterrender: function (me, eOpts ) {{
                                if ({9}) {{
                                    me.collapse();
                                }}
                            }}
                        }},
                        ", fieldSet.ColumnsPerRow, fieldSet.Title, fieldSet.X, fieldSet.Y, fieldSet.Height + 10, fieldSet.Width,
                        fieldSet.Collapsible.Equals(true) ? "true" : "false", "false",
                        fieldSet.Border.Equals(true) ? "true" : "false", fieldSet.Collapsed.Equals(true) ? "true" : "false"));

                sb.Append(@" fieldDefaults: {
                        anchor: '100%',
                        margin: '0 10 8 0',
                        msgTarget: 'side'
                    },     
                    fieldRows:
                    [");
            }
            return sb;
        }
    }
}
