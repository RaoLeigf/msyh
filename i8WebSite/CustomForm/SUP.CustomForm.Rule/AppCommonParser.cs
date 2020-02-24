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
    public class AppCommonParser
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
                    col.Header = textInfo.LeftText;
                    col.DataIndex = textInfo.Name;
                    col.Datatype = SqlTypeConverter.ConvertToExtControl(textInfo.ColumnInfo.ColumnDataType);
                    ExtControlBase column = ControlConverter.ConvertToExtControl(textInfo); //转换下xtype的类型;
                    col.editor.XType = column.XType;
                    if (col.editor.XType == "ngRadio")
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
        /// App表单信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static PbFormInfo GetBillInfo(string fileName)
        {
            PbFormInfo info = new PbFormInfo();

            PbBillInfo billInfo = GetBillBase(fileName);

            info.BillName = billInfo.Name;
            info.Title = billInfo.Description;

            //列表;
            info.ListTable = billInfo.PbList.TableName;
            info.ListSql = billInfo.PbList.Sql;

            //表头;
            info.MasterTable = billInfo.HeadInfo.TableName;
            info.MasterSql = billInfo.HeadInfo.Sql;
            info.HeaderName = billInfo.HeadInfo.Name;

            return info;

        }

        public static PbBillInfo GetBillBase(string fileName)
        {
            ParseManager parseManagerApp = new ParseManager();
            PbBillInfo billInfo = parseManagerApp.ParseBillFileApp(fileName);
            fName = fileName.Substring(fileName.LastIndexOf("\\") + 2,
                                       fileName.LastIndexOf(".") - fileName.LastIndexOf("\\") - 1);

            Log("------>>>> 开始解析" + fName + ".ini");
            return billInfo;
        }

        //异常日志;
        public static void Log(string msg)
        {
            if (string.IsNullOrEmpty(fName))
                return;

            string path = "C://logs//CustomFormLog//" + fName + ".txt";
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
