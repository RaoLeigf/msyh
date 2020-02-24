using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SUP.Common.DataAccess;
using SUP.Common.Base;
using SUP.Common.DataEntity.Individual;
using Newtonsoft.Json;
using SUP.Frame.DataAccess;
using System.Web.Caching;
using System.Web;
using NG3.Data.Service;

namespace SUP.Common.Rule
{
    public class IndividualPropertyRule
    {
      

        private IndividualPropertyDac dac = new IndividualPropertyDac();

        public IndividualPropertyRule()
        {

        }

        public int Save(DataTable columnregdt)
        {

            dac.CreateColumn(columnregdt);
            dac.ModifyColumn(columnregdt);
            dac.DeleteColumn(columnregdt);

            return dac.Save(columnregdt);
        }


        public int SavePropertyUIInfo(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    //dr["code"] = Guid.NewGuid().ToString();   
                    //dr["phid"] = CommonUtil.GetMaxId("fg_col_uireg"); //CommonUtil.GetPhId("fg_col_uireg");
                    dr["phid"] = CommonUtil.GetPhId("fg_col_uireg");
                }
            }

            return dac.SavePropertyUIInfo(dt);
        }

       
        public IList<TreeJSONBase> GetIndividualFieldTree(string bustype)
        {
            IList<TreeJSONBase> rootlist = new List<TreeJSONBase>();

            DataTable dt = dac.GetTableRegByBusType(bustype);
            DataTable columndt = dac.GetColumnsByBusType(bustype);

            foreach (DataRow dr in dt.Rows)
            {
                //string code = dr["code"].ToString();
                string tname = dr["c_bname"].ToString();//表名


                TreeJSONBase table = new TreeJSONBase();
                table.id = tname;
                table.text = tname;
                table.leaf = false;
                table.allowDrag = false;
                table.expanded = true;

                IList<TreeJSONBase> list = new List<TreeJSONBase>();

                DataRow[] columns = columndt.Select("c_bname='" + tname + "'");

                //string uitype = columns[0]["container_uitype"].ToString();//ui类型，formpanel还是gridpanel
                
                for (int i = 0; i < columns.Length; i++)
                {

                    string uitype = columns[i]["container_uitype"].ToString();//ui类型，formpanel还是gridpanel

                    if ("formpanel" == uitype)
                    {
                        IndividualFieldTreeJson col = new IndividualFieldTreeJson();

                        string xtype = columns[i]["uixtype"].ToString();
                        string fieldname = columns[i]["c_name"].ToString();//字段
                        string label = columns[i]["c_fullname"].ToString();//字段名
                        string helpid = columns[i]["helpid"].ToString();//帮助id
                        string querymode = columns[i]["querymode"].ToString();//remote;local
                        string combodata = columns[i]["combodata"].ToString();//帮助id

                        string fieldtype = columns[i]["c_type"].ToString();//字段类型                        
                        int length;
                        Int32.TryParse(columns[i]["collen"].ToString(), out length);//字段长度
                        int declen;
                        Int32.TryParse(columns[i]["declen"].ToString(), out declen);//小数点位数

                        string fieldUIId = tname +"." + fieldname;
                        col.id = tname + "_" + fieldname;
                        col.text = label;
                        col.leaf = true;
                        col.allowDrag = true;
                        col.control = IndividualInfoFactory.GetControlInfo(xtype, fieldname, label, fieldtype, length, declen);                       
                        col.listColumnInfo = ColumnInfoBuilder.BuildBillListColumn(xtype, fieldname, label, helpid, combodata, declen);
                        col.control.FieldUIId = fieldUIId;
                        col.listColumnInfo.FieldUIId = fieldUIId;
                        col.from = "toolbox";//控件来源标记  
                        col.container_uitype = uitype;

                        if (xtype == "ngRichHelp" && !string.IsNullOrWhiteSpace(helpid))
                        {
                            var help = col.control as NGRichHelp;

                            help.helpid = helpid;
                            help.ORMMode = true;
                            DataTable helpinfoDt = dac.GetHelpInfo(helpid);
                            if (helpinfoDt.Rows.Count > 0)
                            {
                                string codeField = helpinfoDt.Rows[0]["codefield"].ToString().Trim();
                                if (codeField.IndexOf('.') > 0)
                                {
                                    help.valueField = codeField.Split('.')[1];//去掉表名
                                }
                                else
                                {
                                    help.valueField = codeField;
                                }
                                string nameField = helpinfoDt.Rows[0]["namefield"].ToString().Trim();
                                if (nameField.IndexOf('.') > 0)
                                {
                                    help.displayField = nameField.Split('.')[1];
                                }
                                else
                                {
                                    help.displayField = nameField;
                                }
                            }

                        }
                        else if (xtype == "ngComboBox")
                        {
                            var combo = col.control as NGComboBox;
                            combo.data = ColumnInfoBuilder.TranslateData(combodata, ';', '|');
                        }                        

                        list.Add(col);
                    }
                    else //gridpanel
                    {
                        GridColumnTreeJson col = new GridColumnTreeJson();

                        string xtype = columns[i]["uixtype"].ToString();
                        string fieldname = columns[i]["c_name"].ToString();//字段
                        string header = columns[i]["c_fullname"].ToString();//字段名
                        string helpid = columns[i]["helpid"].ToString();//帮助id
                        //string querymode = columns[i]["querymode"].ToString();//remote;local
                        string combodata = columns[i]["combodata"].ToString();//帮助id
                        int length;
                        Int32.TryParse(columns[i]["collen"].ToString(), out length);//字段长度
                        int declen;
                        Int32.TryParse(columns[i]["declen"].ToString(), out declen);//小数点位数

                        string fieldUIId = tname + "." + fieldname;
                        col.id = tname + "_" + fieldname;
                        col.text = header;
                        col.leaf = true;
                        col.allowDrag = true;
                        col.control = ColumnInfoBuilder.Build(xtype, fieldname, header, helpid, combodata,length,declen);
                        col.control.FieldUIId = fieldUIId;
                        col.from = "toolbox";//控件来源标记
                        col.container_uitype = uitype;

                        list.Add(col);
                    }
                }                

                //children为null，树就会一直发请求来取数，把children设置为空数组就好了
                //if(list.Count > 0)
                //{
                table.children = list;
                //}

                rootlist.Add(table);

            }

            return rootlist;

        }

    }

    public class ColumnInfoBuilder
    {

        /// <summary>
        /// 构建单据列表的列信息
        /// </summary>
        /// <param name="xtype"></param>
        /// <param name="fieldname"></param>
        /// <param name="header"></param>
        /// <param name="helpid"></param>
        /// <param name="combodata"></param>
        /// <param name="declen"></param>
        /// <returns></returns>
        public static ExtGridColumnInfoBase BuildBillListColumn(string xtype, string fieldname, string header, string helpid, string combodata, int declen)
        {
            ExtGridColumnInfoBase columnInfo = null;

            if (xtype == "ngRichHelp" && !string.IsNullOrWhiteSpace(helpid))
            {
               var col = new ExtGridHelpColumnInfo(fieldname + "_name", header);
                col.helpid = helpid;
                columnInfo = col;
            }
            else if (xtype == "ngComboBox")
            {
                var col = new ExtGridComboBoxColumnInfoForList(fieldname, header);
                col.renderer = new IndividualUIRule().GetRenderer(combodata);
                columnInfo = col;
            }
            else if (xtype == "ngDate")
            {
                columnInfo = new ExtGridDateColumnInfo(fieldname, header);          
            }
            else if (xtype == "ngDateTime")
            {
                columnInfo = new ExtGridDateTimeColumnInfo(fieldname, header); 
            }
            else if (xtype == "ngNumber")
            {
                columnInfo = new ExtGridNumberColumnInfo(fieldname, header, declen);             
            }
            else
            {
                columnInfo = new ExtGridColumnInfoBase(fieldname, header);              
            }

            return columnInfo;
        }


        public static ExtGridColumnInfoBase Build(string xtype, string fieldname, string header, string helpid, string combodata, int length, int declen)
        {
            ExtGridColumnInfoBase columnInfo = null;

            if (xtype == "ngRichHelp" && !string.IsNullOrWhiteSpace(helpid))
            {
                columnInfo = new ExtGridColumnInfoBase(fieldname + "_name", header);

                IndividualPropertyDac dac = new IndividualPropertyDac();
                ExtGridColumnHelpEditor editor = new ExtGridColumnHelpEditor();
                editor.helpid = helpid;
                DataTable dt = dac.GetHelpInfo(helpid);
                if (dt.Rows.Count > 0)
                {
                    string codeField = dt.Rows[0]["codefield"].ToString().Trim();
                    if (codeField.IndexOf('.') > 0)
                    {
                        editor.valueField = codeField.Split('.')[1];
                    }
                    else {
                        editor.valueField = codeField;
                    }
                    string nameField = dt.Rows[0]["namefield"].ToString();
                    if (nameField.IndexOf('.') > 0)
                    {
                        editor.displayField = nameField.Split('.')[1];
                    }
                    else
                    {
                        editor.displayField = nameField;
                    }                  
                }
                editor.isInGrid = true;
                editor.xtype = xtype;
                editor.ORMMode = false;//自定义字段为false

                Listeners ls = new Listeners();
                ls.helpselected = "function (obj) {var data = this.findParentByType('ngGridPanel').getSelectionModel().getSelection();" + string.Format("data[0].set('{0}', obj.code);  data[0].set('{1}', obj.name); ", fieldname, fieldname + "_name") + "}";
                editor.listeners = ls;

                columnInfo.editor = editor;
            }
            else if (xtype == "ngComboBox")
            {
                var col = new ExtGridComboBoxColumnInfo(fieldname, header);               

                ExtGridComboBoxEditor editor = new ExtGridComboBoxEditor();
                editor.valueField = "code";
                editor.displayField = "name";
                editor.queryMode = "local";
                editor.xtype = xtype;
                editor.data = TranslateData(combodata, ';', '|');
                col.editor = editor;
                col.renderer = new IndividualUIRule().GetRenderer(combodata);
                columnInfo = col;
            }
            else if (xtype == "ngDate")
            {
                columnInfo = new ExtGridDateColumnInfo(fieldname, header);

                ExtGridColumnEditorBase editor = new ExtGridColumnEditorBase();
                editor.xtype = xtype;
                columnInfo.editor = editor;
            }
            else if (xtype == "ngDateTime")
            {
                columnInfo = new ExtGridDateTimeColumnInfo(fieldname, header);

                ExtGridColumnEditorBase editor = new ExtGridColumnEditorBase();
                editor.xtype = xtype;                
                columnInfo.editor = editor;

            }
            else if (xtype == "ngNumber")
            {
                columnInfo = new ExtGridNumberColumnInfo(fieldname, header, declen);

                ExtGridNumberEditor editor = new ExtGridNumberEditor();
                editor.xtype = xtype;
                editor.decimalPrecision = declen;
                editor.maxValue = GetMaxVal(length, declen);
                columnInfo.editor = editor;                
            }
            else if (xtype == "ngPercent")//百分比
            {
                columnInfo = new ExtGridPerCentColumnInfo(fieldname, header, declen);

                ExtGridNumberEditor editor = new ExtGridNumberEditor();
                editor.xtype = "ngNumber";
                editor.decimalPrecision = declen;
                editor.showPercent = true;
                editor.step = 0.01;
                columnInfo.editor = editor;                
            }
            else
            {
                columnInfo = new ExtGridColumnInfoBase(fieldname, header);
                ExtGridColumnEditorBase editor = new ExtGridColumnEditorBase();
                editor.xtype = xtype;
                columnInfo.editor = editor;
            }

            return columnInfo;
        }

        private static decimal GetMaxVal(int length, int declen)
        {
            //根据长度和精度控制最大值，否则oracle报错
            int integerLen = length - declen;//整数位
            string s = ".";
            string maxString = "0";
            if (declen < length)
            {
                maxString = s.PadLeft(integerLen, '9').PadRight(declen, '9');
            }
            else if (declen == length)
            {//等于
                maxString = s.PadLeft(1, '0').PadRight(declen, '9');
            }

            decimal maxValue = Convert.ToDecimal(maxString);
            return maxValue;
        }

        /// <summary>
        /// 数据格式：1|男;2|女
        /// </summary>
        /// <param name="comboData"></param>
        /// <returns></returns>
        public static IList<KeyValueEntity> TranslateData(string comboData,char firstSplitChar, char secondSplitChar)
        {
            string[] arr = comboData.Split(firstSplitChar);

            //JArray jarr = new JArray();
            List<KeyValueEntity> list = new List<KeyValueEntity>();
            foreach (string item in arr)
            {
                string[] strArr = item.Split(secondSplitChar);
                KeyValueEntity entity = new KeyValueEntity();
                entity.code = strArr[0];
                entity.name = strArr[1];

                list.Add(entity);
            }

            return list;
           //return JsonConvert.SerializeObject(list);
        }

    }
}
