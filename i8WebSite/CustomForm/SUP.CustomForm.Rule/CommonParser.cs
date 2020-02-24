using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity;
using SUP.CustomForm.DataEntity.Container;
using SUP.CustomForm.DataEntity.Control;
using NG3.Metadata.UI.PowserBuilder.Controls;
using NG3.Metadata.PBDesignerParse;
using NG3.Metadata.UI.PowserBuilder;

namespace SUP.CustomForm.Rule
{
    public class CommonParser
    {
        private static string fName = string.Empty;

        public static List<ExtGridColumn> GetListColumns(IList<PbBaseTextInfo> pbcontrols)
        {
            SortedList<int, PbBaseControlInfo> list = new SortedList<int, PbBaseControlInfo>();
            List<ExtGridColumn> columns = new List<ExtGridColumn>();

            foreach (PbBaseControlInfo item in pbcontrols)
            {
                if (list.ContainsKey(item.XPos))    //如果x坐标重复，则跳过 //0000000038 这里有重复的 
                    continue;               
                list.Add(item.XPos, item);//以x坐标排序
            }

            foreach (KeyValuePair<int,PbBaseControlInfo> element in list)
            {
                PbBaseTextInfo textInfo = element.Value as PbBaseTextInfo;

                if(textInfo != null)
                {
                    ExtGridColumn col = new ExtGridColumn();
                    if (textInfo.Name.Equals("t_sum"))
                    {
                        continue;
                    }
                    col.Header = textInfo.LeftText;
                    col.DataIndex = textInfo.Name;
                    col.Datatype = SqlTypeConverter.ConvertToExtControl(textInfo.ColumnInfo.ColumnDataType);
                    col.DefaultValue = textInfo.DefaultValue;
                    col.Width = textInfo.Width;  //Grid每一列的宽度
                    col.RgbColor = ControlConverter.GetRgb(textInfo.Color);//grid列的字体颜色
                    col.BackgroundColor= ControlConverter.GetRgb(textInfo.backgroundColor);//grid列的背景颜色

                    ExtControlBase column = ControlConverter.ConvertToExtControl(textInfo); //转换下xtype的类型;
                    col.Protect = column.Protect;
                    col.MustInput = column.MustInput;
                    col.Format = column.Format;
                    col.EditMask = column.EditMask;
                    col.editor.XType = column.XType;
                    

                    if (col.editor.XType == "ngCustomFormHelp" || col.editor.XType == "ngRichHelp")
                    {
                        var temp = column as NGHelpBase;
                        if (temp.Helpid == "itemdata")
                        {
                            col.DataIndex = "itemid";
                            col.Header = "物资";
                            col.editor.ValueField = "phid";
                            col.editor.DisplayField = "itemname";
                            col.editor.ListFields = "itemid,c_name,spec,msname";
                            col.editor.ListHeadTexts = "编码,名称,规格,单位";
                            col.editor.Helpid = temp.Helpid;
                        }
                        else
                        {
                            col.editor.ValueField = temp.ValueField;
                            col.editor.DisplayField = temp.DisplayField;
                            col.editor.ListFields = temp.ListFields;
                            col.editor.ListHeadTexts = temp.ListHeadTexts;
                            col.editor.CmpName = temp.CmpName;
                            col.editor.OutFilter = temp.OutFilter;
                            col.editor.Helpid = temp.Helpid;
                            col.editor.MultiSelect = temp.MultiSelect;
                        }
                    }
                    else if (col.editor.XType == "ngComboBox")
                    {
                        if (column is NGComboBox)
                        {
                            NGComboBox temp = column as NGComboBox;
                            col.editor.QueryMode = temp.QueryMode;
                            col.editor.Data = temp.Data;
                        }
                        else if (column is NGCommonHelp)
                        {
                            NGCommonHelp temp = column as NGCommonHelp;
                            col.editor.QueryMode = temp.QueryMode;
                            col.editor.Data = temp.Data;

                        }
                    }
                    else if (col.editor.XType == "ngRadio")
                    {
                        var temp = column as NGRadio;
                        col.editor.Items = temp.Items;
                    }

                    columns.Add(col);
                }
            }

            return columns;

        }

        /// <summary>
        /// pc web表单  comment by ljy 2016.10.27 此函数暂时废弃
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static PbFormInfo GetBillInfo(string fileName)
        {
            PbFormInfo info = new PbFormInfo();

            PbBillInfo billInfo = GetBillBase(fileName);

            info.BillName = billInfo.Name;
            info.Title = billInfo.Description;
            info.HasTab = billInfo.PbTabInfos.Count > 0 ? true : false;

            //列表;
            info.ListTable = billInfo.PbList.TableName;
            info.ListSql = billInfo.PbList.Sql;

            //表头;
            info.MasterTable = billInfo.HeadInfo.TableName;
            info.MasterSql = billInfo.HeadInfo.Sql;
            info.HeaderName = billInfo.HeadInfo.Name;

            //明细表，目前明细仅支持grid;
            foreach (PbGridInfo grid in billInfo.PbGrids)
            {
                DetailInfo detailInfo = new DetailInfo();
                detailInfo.Name = grid.Name;
                detailInfo.TableName = grid.TableName;
                detailInfo.Sql = grid.Sql;

                //二级明细暂不处理;
                if (info.Detail.ContainsKey(grid.Name))
                    continue;
                info.Detail.Add(grid.Name,detailInfo);
            }

            return info;
        }

        public static PbBillInfo GetBillBase(string fileName)
        {
            ParseManager parseManager = new ParseManager();
            PbBillInfo billInfo = parseManager.ParseBillFile(fileName);
            fName = fileName.Substring(fileName.LastIndexOf("\\") + 2,
                                       fileName.LastIndexOf(".") - fileName.LastIndexOf("\\") - 2);

            Log("------>>>> 开始解析" + fName + ".ini");
            return billInfo;
        }

        //异常日志;
        public static void Log(string msg)
        {
            if (string.IsNullOrEmpty(fName))
                return;

            string path = AppDomain.CurrentDomain.BaseDirectory + "Logs\\CustomFormLog\\" + fName + ".txt";;
            StreamWriter write;

            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            write = new StreamWriter(path, true, System.Text.Encoding.Default);
            write.BaseStream.Seek(0, SeekOrigin.End);
            write.AutoFlush = true;
            if (write != null)
            {
                lock (write)
                {
                    write.WriteLine("Time:" + DateTime.Now.ToString() + "  Msg:" + msg);
                    write.Flush();
                }
            }
            write.Close();
            write = null;
        }
    }
}
