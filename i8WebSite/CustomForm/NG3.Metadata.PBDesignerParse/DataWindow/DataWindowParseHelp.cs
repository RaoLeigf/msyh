using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using NG3.Metadata.UI.PowserBuilder.Controls;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;
using SUP.CustomForm.DataAccess;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    class DataWindowParseHelp
    {

        private static string RemoveQuotes(string inputString)
        {
            Debug.Assert(!string.IsNullOrEmpty(inputString));
            string str = inputString.Trim();

            if (str.Length > 2)
                return str.Substring(1, str.Length - 2);
            else if (str == "\"\"")
            {
                return string.Empty;
            }
            else
                return str;
        }

        private static DwColumn ParseDwColumn(string dwColumnStr, IDictionary<string, string> coldic)
        {
            DwColumn dwColumn = new DwColumn();
            try
            {
                string columnString = dwColumnStr.Trim();
                columnString = columnString.Remove(0, DwRes.ColumnSectionStart.Length);
                columnString = columnString.Remove(columnString.Length - 1, 1);
                columnString = columnString.Trim();

                //--替换非分隔用的空格为#号
                if (columnString.IndexOf("[general]", StringComparison.OrdinalIgnoreCase) < 0)  //说明有掩码值
                {
                    int posFormat = columnString.IndexOf("format=", StringComparison.OrdinalIgnoreCase);
                    int posQuot = columnString.IndexOf("\"", posFormat + 8);
                    int posSpace = columnString.IndexOf(" ", posFormat, posQuot);

                    //format值中有空格存在
                    if (posSpace > -1 && posSpace < posQuot)
                    {
                        columnString = columnString.Remove(posSpace, 1).Insert(posSpace, "#");
                    }
                }                
                //--end

                string[] stringArray = columnString.Split(' ');
                int step = 1;

                for (int i = 0; i < stringArray.Length; i += step)
                {
                    string str = stringArray[i].Trim();

                    if (string.IsNullOrWhiteSpace(str))
                    {
                        continue;
                    }

                    string[] stringChildArray = str.Split('=');

                    if (stringChildArray.Length == 3)
                    {
                        stringChildArray[1] += "=" + stringChildArray[2];
                    }

                    if (stringChildArray.Length == 1)
                    {
                        step = 2;
                        str = stringArray[i] + " " + stringArray[i + 1];
                        stringChildArray = str.Split('=');
                    }
                    else if (stringChildArray.Length == 2 || stringChildArray.Length == 3)
                    {
                        step = 1;
                    }
                    else
                    {
                        continue;
                    }

                    Debug.Assert(stringChildArray.Length == 2);
                    switch (stringChildArray[0])
                    {
                        case "color":
                            dwColumn.Color = Convert.ToInt64(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "name":
                            dwColumn.Name = stringChildArray[1];
                            if (coldic != null) dwColumn.Mustinput = Convert.ToBoolean(Convert.ToInt32(coldic[dwColumn.Name]));
                            break;
                        case "visible":
                            dwColumn.Visible = Convert.ToBoolean(Convert.ToInt32(RemoveQuotes(stringChildArray[1])));
                            break;
                        case "protect":
                            //如：protect="1" 和 protect="0	1"
                            if (stringChildArray[1].Contains("1") && !stringChildArray[1].Contains("0"))
                            {
                                dwColumn.Protect = true;
                            }
                            else
                            {
                                dwColumn.Protect = false;
                            }

                            //protect="0	if( ddlbcol_1='9', 0, 1 )"
                            dwColumn.ProtectExp = stringChildArray[1];
                            break;
                        case "x":
                            dwColumn.XPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "y":
                            dwColumn.YPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "width":
                            dwColumn.Width = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "height":
                            dwColumn.Height = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "format":
                            stringChildArray[1] = stringChildArray[1].Replace("#", " ");
                            dwColumn.Format = RemoveQuotes(stringChildArray[1]);
                            break;
                        case "tabsequence":
                            dwColumn.TabSequence = Convert.ToInt32(stringChildArray[1]);
                            break;
                        case "edit.limit":
                            dwColumn.MaxLength = Convert.ToInt32(stringChildArray[1]);
                            break;
                        case "tag":
                            dwColumn.Tag = RemoveQuotes(stringChildArray[1]);
                            break;
                        case "editmask.mask":
                            dwColumn.EditMask = RemoveQuotes(stringChildArray[1]);
                            dwColumn.EditMask = dwColumn.EditMask.Replace('#', '0');
                            break;
                        case "background.color"://列的背景颜色268435456 1073741824
                            dwColumn.BackgroundColor = (Convert.ToInt64(RemoveQuotes(stringChildArray[1]))== 536870912|| Convert.ToInt64(RemoveQuotes(stringChildArray[1])) == 67108864
                                || Convert.ToInt64(RemoveQuotes(stringChildArray[1])) == 268435456 || Convert.ToInt64(RemoveQuotes(stringChildArray[1])) == 1073741824) ? 0: Convert.ToInt64(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "":
                            break;
                        default:
                            break;
                    }

                    if (stringChildArray[0].StartsWith("radiobuttons", StringComparison.OrdinalIgnoreCase))
                    {
                        dwColumn.ControlType = DwControlType.RedioBoxEdit;
                    }
                    else if (stringChildArray[0].StartsWith("checkbox", StringComparison.OrdinalIgnoreCase))
                    {
                        dwColumn.ControlType = DwControlType.CheckBoxEdit;
                    }
                    else if (stringChildArray[0].StartsWith("ddlb", StringComparison.OrdinalIgnoreCase))
                    {
                        dwColumn.ControlType = DwControlType.ComboBoxEdit;
                    }
                    else if (stringChildArray[0].StartsWith("dddw", StringComparison.OrdinalIgnoreCase))
                    {
                        dwColumn.ControlType = DwControlType.DataHelpEdit;
                    }
                    else if (stringChildArray[0].StartsWith("richedit", StringComparison.OrdinalIgnoreCase))
                    {
                        dwColumn.ControlType = DwControlType.RichTextEdit;
                    }
                }

                return dwColumn;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static DwGroupBox ParseDwGroupBox(string dwGroupBoxStr)
        {
            DwGroupBox dwGroupBox = new DwGroupBox();
            try
            {
                string dwTextString = dwGroupBoxStr.Trim();
                dwTextString = dwTextString.Remove(0, DwRes.GroupBoxSectionStart.Length);
                dwTextString = dwTextString.Remove(dwTextString.Length - 1, 1);
                dwTextString = dwTextString.Trim();
                string[] stringArray = dwTextString.Split(' ');
                foreach (string str in stringArray)
                {
                    string[] stringChildArray = str.Split('=');
                    Debug.Assert(stringChildArray.Length == 2);
                    switch (stringChildArray[0])
                    {
                        case "color":
                            dwGroupBox.Color = Convert.ToInt64(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "name":
                            dwGroupBox.Name = stringChildArray[1];
                            break;
                        case "visible":
                            dwGroupBox.Visible = Convert.ToBoolean(Convert.ToInt32(RemoveQuotes(stringChildArray[1])));
                            break;
                        case "x":
                            dwGroupBox.XPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "y":
                            dwGroupBox.YPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "width":
                            dwGroupBox.Width = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "height":
                            dwGroupBox.Height = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "text":
                            dwGroupBox.Text = RemoveQuotes(stringChildArray[1]);
                            break;
                        default:
                            break;
                    }
                }

                return dwGroupBox;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static DwText ParseDwText(string dwTextStr)
        {
            DwText dwText = new DwText();
            try
            {
                string dwTextString = dwTextStr.Trim();
                dwTextString = dwTextString.Remove(0, DwRes.TextSectionStart.Length);
                dwTextString = dwTextString.Remove(dwTextString.Length - 1, 1);
                dwTextString = dwTextString.Trim();
                string[] stringArray = dwTextString.Split(' ');
                foreach (string str in stringArray)
                {
                    string[] stringChildArray = str.Split('=');
                    Debug.Assert(stringChildArray.Length == 2);
                    switch (stringChildArray[0])
                    {
                        case "color":
                            if(stringChildArray[1].Contains("!"))
                            {
                                stringChildArray[1] = "33554432";
                            }

                            //dwText.Color = Convert.ToInt64(RemoveQuotes(stringChildArray[1]));
                            dwText.LabelTextColor = Convert.ToInt64(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "name":
                            dwText.Name = stringChildArray[1];
                            break;
                        case "visible":
                            dwText.Visible = Convert.ToBoolean(Convert.ToInt32(RemoveQuotes(stringChildArray[1])));
                            break;
                        case "x":
                            dwText.XPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "y":

                            if (stringChildArray[1].Contains("."))
                            {
                                stringChildArray[1] = "1";
                            }

                            dwText.YPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "width":
                            dwText.Width = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "height":
                            dwText.Height = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "text":
                            dwText.Text = RemoveQuotes(stringChildArray[1]);
                            break;
                        case "font.height":
                            dwText.Font = Convert.ToInt32(RemoveQuotes(stringChildArray[1])) * (-1);
                            break;
                        case "alignment":
                            dwText.Align = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        default:
                            break;
                    }
                }

                return dwText;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static DwButton ParseDwButton(string dwButtonStr)
        {
            DwButton dwButton = new DwButton();
            try
            {
                string dwButtonString = dwButtonStr.Trim();
                dwButtonString = dwButtonString.Remove(0, DwRes.ColumnSectionStart.Length);
                dwButtonString = dwButtonString.Remove(dwButtonString.Length - 1, 1);
                dwButtonString = dwButtonString.Trim();
                string[] stringArray = dwButtonString.Split(' ');
                foreach (string str in stringArray)
                {
                    string[] stringChildArray = str.Split('=');
                    Debug.Assert(stringChildArray.Length == 2);
                    switch (stringChildArray[0])
                    {
                        case "color":
                            dwButton.Color = Convert.ToInt64(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "name":
                            dwButton.Name = stringChildArray[1];
                            break;
                        case "visible":
                            dwButton.Visible = Convert.ToBoolean(Convert.ToInt32(RemoveQuotes(stringChildArray[1])));
                            break;
                        case "x":
                            dwButton.XPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "y":
                            dwButton.YPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "width":
                            dwButton.Width = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "height":
                            dwButton.Height = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "text":
                            dwButton.Text = RemoveQuotes(stringChildArray[1]);
                            break;
                        default:
                            break;
                    }
                }

                return dwButton;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static DwDbColumn ParseDwDbColumn(string dbColumnStr)
        {
            DwDbColumn dwDbColumn = new DwDbColumn();
            try
            {
                string columnString = dbColumnStr.Trim();
                columnString = columnString.Remove(0, DwRes.DbColumnSectionStart.Length);
                columnString = columnString.Remove(columnString.Length - 1, 1);
                columnString = columnString.Trim();
                string[] stringArray = columnString.Split(' ');
                foreach (string str in stringArray)
                {
                    string[] stringChildArray = str.Split('=');
                    Debug.Assert(stringChildArray.Length == 2);
                    switch (stringChildArray[0])
                    {
                        case "type":
                            dwDbColumn.DataType = stringChildArray[1];
                            break;
                        case "name":
                            dwDbColumn.Name = stringChildArray[1];
                            break;
                        case "dbname":
                            dwDbColumn.DbName = RemoveQuotes(stringChildArray[1]);
                            break;
                        case "values":
                            dwDbColumn.Values = RemoveQuotes(stringChildArray[1]);
                            break;
                        default:
                            break;
                    }
                }

                return dwDbColumn;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static DwBitmap ParseDwBitmap(string dwBitmapStr)
        {
            DwBitmap dwBitmap = new DwBitmap();
            try
            {
                string dwBitmapString = dwBitmapStr.Trim();
                dwBitmapString = dwBitmapString.Remove(0, DwRes.ColumnSectionStart.Length);
                dwBitmapString = dwBitmapString.Remove(dwBitmapString.Length - 1, 1);
                dwBitmapString = dwBitmapString.Trim();
                string[] stringArray = dwBitmapString.Split(' ');
                foreach (string str in stringArray)
                {
                    string[] stringChildArray = str.Split('=');
                    Debug.Assert(stringChildArray.Length == 2);
                    switch (stringChildArray[0])
                    {                       
                        case "name":
                            dwBitmap.Name = stringChildArray[1];
                            break;
                        case "visible":
                            dwBitmap.Visible = Convert.ToBoolean(Convert.ToInt32(RemoveQuotes(stringChildArray[1])));
                            break;
                        case "tag":
                            dwBitmap.Tag = stringChildArray[1];
                            break;
                        case "filename":
                            dwBitmap.FileName = stringChildArray[1];
                            break;
                        case "x":
                            dwBitmap.XPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "y":
                            dwBitmap.YPos = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "width":
                            dwBitmap.Width = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        case "height":
                            dwBitmap.Height = Convert.ToInt32(RemoveQuotes(stringChildArray[1]));
                            break;
                        default:
                            break;
                    }
                }

                return dwBitmap;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DwInfo ParseDataWindow(string dwSyntax, IDictionary<string, string> coldic)
        {
            try
            {
                DwInfo dwInfo = new DwInfo();
                string[] arrayStr = dwSyntax.Split(new string[] { DwRes.Crlf }, StringSplitOptions.RemoveEmptyEntries);
                int step = 1;
                for (int i = 1; i < arrayStr.Length; i += step)
                {

                    string str = arrayStr[i].Trim();

                    if (str.Contains("htmltable"))
                        break;

                    if (str.StartsWith(DwRes.TableSectionStart, StringComparison.OrdinalIgnoreCase))
                    {
                        str = str.Remove(0, DwRes.TableSectionStart.Length);
                    }
                    if (str.EndsWith(")"))
                    {
                        step = 1;
                    }
                    else
                    {
                        step = 2;
                        string tempStr = arrayStr[i + 1].Trim();
                        if (tempStr.StartsWith(DwRes.ValuesSectionStart))
                        {
                            str = str + " " + tempStr;
                        }
                    }

                    if (str.StartsWith(DwRes.DbColumnSectionStart))
                    {
                        dwInfo.DwDbColumns.Add(ParseDwDbColumn(str));
                    }
                    else if (str.StartsWith(DwRes.ColumnSectionStart))
                    {
                        dwInfo.DwColumns.Add(ParseDwColumn(str, coldic));
                    }
                    else if (str.StartsWith(DwRes.GroupBoxSectionStart))
                    {
                        dwInfo.DwGroupBoxs.Add(ParseDwGroupBox(str));
                    }
                    else if (str.StartsWith(DwRes.TextSectionStart))
                    {
                        dwInfo.DwTexts.Add(ParseDwText(str));
                    }
                    else if (str.StartsWith(DwRes.ButtonSectionStart))
                    {
                        dwInfo.DwButtons.Add(ParseDwButton(str));
                    }
                    else if (str.StartsWith(DwRes.BitmapSectionStart))
                    {
                        dwInfo.DwBitmaps.Add(ParseDwBitmap(str));
                    }
                    else if (str.StartsWith(DwRes.SqlSection))
                    {
                        int index = str.IndexOf("=");
                        dwInfo.Sql = RemoveQuotes(str.Substring(index + 1));
                    }
                }
                return dwInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DwInfo ParseDataWindow(string dwSyntax)
        {
            return ParseDataWindow(dwSyntax, null);
        }

        private static void CommonAssignment(PbBaseTextInfo baseTextInfo, DwDbColumn dwDbColumn, DwText dwText, DwColumn dc, SqlDbType dbType, string dwAuthName)
        {
            try
            {
                
                if (string.IsNullOrEmpty(dwAuthName))
                {
                    baseTextInfo.FullName = dc.Name;
                }
                else
                {
                    baseTextInfo.FullName = dwAuthName + "." + dc.Name;
                }

                baseTextInfo.Height = dc.Height;
                baseTextInfo.MaxLength = dc.MaxLength;
                baseTextInfo.Name = dc.Name;
                baseTextInfo.TextColor = dc.Color;
                
                baseTextInfo.Visible = dc.Visible;
                baseTextInfo.DefaultValue = dc.DefaultValue;
                baseTextInfo.Tag = dc.Tag;
                baseTextInfo.Format = dc.Format;
                baseTextInfo.EditMask = dc.EditMask;

                //赋上必输和保护
                baseTextInfo.IsMustInput = dc.Mustinput;
                baseTextInfo.IsProtect = dc.Protect;

                baseTextInfo.SingleText = dc.SingleText;  //是否为单独标签
                baseTextInfo.TextArea = dc.TextArea;      //是否为多行文本框
                baseTextInfo.ColSpan = dc.ColSpan;        //列占位数
                baseTextInfo.MultiSelect = dc.MultiSelect;   //是否多选帮助
                baseTextInfo.YPos = dc.YPos;

                baseTextInfo.Color = dc.Color;//设置grid某一列字体的颜色
                baseTextInfo.backgroundColor = dc.BackgroundColor;//设置grid某一列背景颜色
                
                //设置列的保护属性表达式
                if (dc.ProtectExp.Contains("if"))
                {
                    PbExpressionImp pbExpressionImp = new PbExpressionImp();
                    pbExpressionImp.Expression = dc.ProtectExp;
                    pbExpressionImp.ExpressionType = (PbExpressionType)17;
                    baseTextInfo.IsReadOnlyExpressionImp = pbExpressionImp;
                }

                if (dwText == null)
                {
                    baseTextInfo.LeftText = string.Empty;
                    baseTextInfo.XPos = dc.XPos;
                    baseTextInfo.Width = dc.Width;
                    baseTextInfo.LabelWidth = dc.Width;
                }
                else
                {
                    baseTextInfo.LeftText = dwText.Text;
                    baseTextInfo.XPos = dwText.XPos;
                    baseTextInfo.LabelTextColor = dwText.LabelTextColor;
                    if (dc.ControlType == DwControlType.CheckBoxEdit && dc.XPos < dwText.XPos)//checkbox的情况要单独讨论，考虑到标签是在选择框的右边。
                    {
                        baseTextInfo.Width = dwText.XPos + dwText.Width - dc.XPos;
                        baseTextInfo.LabelWidth = dwText.Width + dc.Width;
                    }
                    else
                    {
                        baseTextInfo.Width = dc.XPos - dwText.XPos + dc.Width;
                        if (dc.XPos == dwText.XPos)
                            baseTextInfo.LabelWidth = dc.Width;
                        else
                        {
                            baseTextInfo.LabelWidth = dc.XPos - dwText.XPos;
                        }
                    }

                }
                baseTextInfo.ColumnInfo.ColumnDataType = dbType;
                string[] arrayStr = dwDbColumn.DbName.Split('.');
                Debug.Assert(arrayStr.Length == 2);
                baseTextInfo.ColumnInfo.ColumnName = arrayStr[1];
                if (string.IsNullOrEmpty(dwAuthName))
                    baseTextInfo.ColumnInfo.TableName = arrayStr[0];
                else
                    baseTextInfo.ColumnInfo.TableName = dwAuthName;

                //if (!string.IsNullOrEmpty(dwDbColumn.SummaryType))
                //{
                //    switch (dwDbColumn.SummaryType)
                //    {
                //        case "avg":
                //            dwDbColumn.SummaryType = "average";
                //            break;
                //        case "count":
                //            dwDbColumn.SummaryType = "count";
                //            break;
                //        case "max":
                //            dwDbColumn.SummaryType = "max";
                //            break;
                //        case "min":
                //            dwDbColumn.SummaryType = "min";
                //            break;
                //        case "sum":
                //            dwDbColumn.SummaryType = "sum";
                //            break;
                //    }
                //    baseTextInfo.SummaryType = dwDbColumn.SummaryType;
                //}
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        public static IList<PbGroupboxInfo> ToPbGroupBoxMetadata(DwInfo dwInfo, string dwAuthName, IDictionary<string, string> collapsedic)
        {
            try
            {
                IList<PbGroupboxInfo> pbGroupboxInfos = new List<PbGroupboxInfo>();
                foreach (DwGroupBox dwGroupBox in dwInfo.DwGroupBoxs)
                {
                    PbGroupboxInfo pbGroupboxInfo = new PbGroupboxInfo();
                    pbGroupboxInfo.FullName = dwAuthName + dwGroupBox.Name;
                    pbGroupboxInfo.Height = Convert.ToInt32(dwGroupBox.Height);
                    pbGroupboxInfo.Name = dwGroupBox.Name;
                    pbGroupboxInfo.Text = dwGroupBox.Text;
                    pbGroupboxInfo.TextColor = dwGroupBox.Color;
                    pbGroupboxInfo.Visible = dwGroupBox.Visible;
                    pbGroupboxInfo.Width = Convert.ToInt32(dwGroupBox.Width * 1.3);
                    pbGroupboxInfo.XPos = Convert.ToInt32(dwGroupBox.XPos * 1.3);
                    pbGroupboxInfo.YPos = Convert.ToInt32(dwGroupBox.YPos);

                    //设置fieldset折叠属性
                    if (collapsedic != null && collapsedic.Count() != 0)
                    {
                        if (collapsedic.ContainsKey(dwGroupBox.Name))  //有关联的标签和输入框
                        {
                            pbGroupboxInfo.Collapse = collapsedic[dwGroupBox.Name];
                        }
                        else
                        {
                            pbGroupboxInfo.Collapse = string.Empty;
                        }
                    }

                    pbGroupboxInfos.Add(pbGroupboxInfo);
                }
                return pbGroupboxInfos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static IList<PbPictureboxInfo> ToPbPictureBoxMetadata(DwInfo dwInfo, PbHeadInfo headInfo)
        {
            try
            {
                IList<PbPictureboxInfo> pbPictureboxInfos = new List<PbPictureboxInfo>();
                foreach (DwBitmap dwBitmap in dwInfo.DwBitmaps)
                {
                    PbPictureboxInfo pbPictureboxInfo = new PbPictureboxInfo();                    
                    pbPictureboxInfo.Name = dwBitmap.Name;
                    pbPictureboxInfo.Visible = dwBitmap.Visible;
                    pbPictureboxInfo.Tag = dwBitmap.Tag;
                    pbPictureboxInfo.FileName = dwBitmap.FileName;
                    pbPictureboxInfo.Width = Convert.ToInt32(dwBitmap.Width * 1.3);
                    pbPictureboxInfo.Height = Convert.ToInt32(dwBitmap.Height);
                    pbPictureboxInfo.XPos = Convert.ToInt32(dwBitmap.XPos * 1.3);
                    pbPictureboxInfo.YPos = Convert.ToInt32(dwBitmap.YPos);
                    pbPictureboxInfo.IsFromAttachment = true;
                    try
                    {
                        pbPictureboxInfo.ColSpan = Convert.ToInt32(headInfo.ColSpanDic[dwBitmap.Name + "_url"]);
                    }
                    catch
                    {
                        pbPictureboxInfo.ColSpan = 1;
                    }
                    pbPictureboxInfos.Add(pbPictureboxInfo);
                }
                return pbPictureboxInfos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static IList<PbPairValueInfo> ConvertToPairValueInfo(string valuesStr)
        {
            try
            {
                IList<PbPairValueInfo> pbPairValueInfos = new List<PbPairValueInfo>();
                if (valuesStr.Trim().Length == 0)
                    return pbPairValueInfos;

                string str = valuesStr;
                string[] strArray = str.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in strArray)
                {
                    string[] childStrArray = s.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    PbPairValueInfo pbPairValueInfo = null;
                    if (childStrArray.Length == 1)
                    {
                        pbPairValueInfo = new PbPairValueInfo(childStrArray[0], childStrArray[0]);
                    }
                    else if (childStrArray.Length == 2)
                    {
                        pbPairValueInfo = new PbPairValueInfo(childStrArray[0], childStrArray[1]);
                    }
                    pbPairValueInfos.Add(pbPairValueInfo);
                }

                return pbPairValueInfos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void SetHelpId(PbDataHelpEditInfo pbDataHelpEditInfo, IDictionary<string, string> dddwSourceDic)
        {
            
            if (dddwSourceDic.ContainsKey(pbDataHelpEditInfo.FullName))
            {
                //设计器上的自定义：   p_form0000000012_m.userdefine_1 = 60041,0
                //设计器上的通用帮助： p_form0000000012_m.userhelp_1=pms3.project_table,1
                //上面两种都存在ini上，格式就是后面的例子
                //设计器上拖出来的自定义和通用帮助列，可手动设置数据源，数据源前者支持customhelp，后者是后面改造的支持richhelp
                string helpidStr = string.Empty;
                string helpSource = dddwSourceDic[pbDataHelpEditInfo.FullName];

                if (helpSource.IndexOf(",") > 0)
                {
                    string[] helpidArray = helpSource.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    pbDataHelpEditInfo.DataHelpId = helpidArray[0];
                }
                else
                {
                    pbDataHelpEditInfo.DataHelpId = helpSource;
                }
            }            
            else
            {
                //设计器上的供应商、部门和班组等帮助列
                //不能手动赋数据源，helpid根据字段名从配置文件NG3Config\\EFormHelp.xml去取，数据源来自richhelp
                DataRow[] drs = HelpDac.XmlHelpDT.Select("Id='" + pbDataHelpEditInfo.Name + "'");
                if (drs.Length > 0)
                {
                    pbDataHelpEditInfo.DataHelpId = drs[0]["HelpId"].ToString();
                }
            }
        }


        //ParseBillHead 调用
        public static IList<PbBaseTextInfo> ToPbColumnsMetadata(DwInfo dwInfo, string dwAuthName, IDictionary<string, string> dddwSourceDic, string titlename, 
            Dictionary<string, string> colrefdic, IDictionary<string, string> textareadic, IDictionary<string, string> colspandic, IDictionary<string, string> multiselectdic)
        {
            IList<PbBaseTextInfo> pbBaseTextInfos = new List<PbBaseTextInfo>();

            //普通列
            foreach (DwColumn dc in dwInfo.DwColumns)
            {
                //如果控件宽度或高度为0，该控件不加入列表
                if (dc.Height == 0 || dc.Width == 0)
                {
                    continue;
                }

                DwDbColumn dwDbColumn = dwInfo.GetDwDbColumnByName(dc.Name);

                if (dwDbColumn == null)
                {
                    continue;
                }

                //if (sumdic != null && sumdic.Count() != 0)
                //{
                //    if (sumdic.ContainsKey(dc.Name))
                //    {
                //        dwDbColumn.SummaryType = sumdic[dc.Name];
                //    }
                //}

                DwText dwText = dwInfo.GetDwTextByName(dc.Name);

                //xkq. 2016-12-8 寻找[window]中ref中是否有对应text
                if (colrefdic != null && colrefdic.Count() != 0)
                {
                    if (dwText == null || dwText.Name == null)
                    {
                        if (colrefdic.ContainsKey(dc.Name))  //有关联的标签和输入框
                        {
                            string textname = colrefdic[dc.Name];
                            if (!string.IsNullOrEmpty(textname))
                            {
                                dwText = dwInfo.GetDwTextByName1(textname);
                            }
                        }
                        else  //单独的标签
                        {
                            dc.SingleText = true;
                        }
                    }
                }

                //add by ljy 2017.07.13 找出类型时textarea的字段
                if (textareadic != null && textareadic.Count() != 0)
                {
                    if (textareadic.ContainsKey(dc.Name))
                    {
                        dc.TextArea = true;
                    }
                }

                //add by ljy 2017.07.14 设置所有列的列占位数
                if (colspandic != null && colspandic.Count() != 0)
                {
                    if (colspandic.ContainsKey(dc.Name))  //有关联的标签和输入框
                    {
                        dc.ColSpan = Convert.ToInt32(colspandic[dc.Name]);
                    }
                }

                //add by ljy 2017.11.03 找出复选帮助字段
                if (multiselectdic != null && multiselectdic.Count() != 0)
                {
                    if (multiselectdic.ContainsKey(dwAuthName + "." + dc.Name))
                    {
                        dc.MultiSelect = true;
                    }
                }

                if (dc.Name.Equals("res_code"))
                {
                    dwDbColumn.DataType = "DataHelpEdit";
                    PbDataHelpEditInfo pbDataHelpEditInfo = new PbDataHelpEditInfo();
                    CommonAssignment(pbDataHelpEditInfo, dwDbColumn, dwText, dc, SqlDbType.NVarChar, dwAuthName);
                    DataRow[] drs = HelpDac.XmlHelpDT.Select("Id='" + pbDataHelpEditInfo.Name + "'");
                    if (drs.Length > 0)//系统内置的通用帮助
                    {
                        pbDataHelpEditInfo.DataHelpId = drs[0]["HelpId"].ToString();
                    }
                    pbBaseTextInfos.Add(pbDataHelpEditInfo);
                    continue;
                }

                if (dc.Name.Equals("c_name"))
                {
                    continue;
                }

                //if(dc.Name.Equals("msunit"))
                //{
                //    sqlDbType = SqlDbType.NVarChar;
                //    dc.ControlType = DwControlType.TextEdit;
                //}

                SqlDbType sqlDbType = DwDbTypeConvert.ToSqlDbType(dwDbColumn.DataType);

                switch (sqlDbType)
                {
                    case SqlDbType.NVarChar:
                        switch (dc.ControlType)
                        {
                            case DwControlType.CheckBoxEdit:
                                PbCheckboxInfo checkboxInfo = new PbCheckboxInfo();
                                CommonAssignment(checkboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(checkboxInfo);
                                break;
                            case DwControlType.RedioBoxEdit:
                                PbRadioboxInfo pbRadioboxInfo = new PbRadioboxInfo();
                                CommonAssignment(pbRadioboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbRadioboxInfo.PbPairValueInfos = ConvertToPairValueInfo(dwDbColumn.Values);
                                pbBaseTextInfos.Add(pbRadioboxInfo);
                                break;
                            case DwControlType.ComboBoxEdit:
                                PbComboboxInfo pbComboboxInfo = new PbComboboxInfo();
                                CommonAssignment(pbComboboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbComboboxInfo.PbComboboxValueInfos = ConvertToPairValueInfo(dwDbColumn.Values);
                                pbBaseTextInfos.Add(pbComboboxInfo);
                                break;
                            case DwControlType.DataHelpEdit:
                                PbDataHelpEditInfo pbDataHelpEditInfo = new PbDataHelpEditInfo();
                                CommonAssignment(pbDataHelpEditInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                SetHelpId(pbDataHelpEditInfo, dddwSourceDic);  //设置helpid
                                pbBaseTextInfos.Add(pbDataHelpEditInfo);
                                break;
                            case DwControlType.RichTextEdit:
                                PbRichTextInfo pbRichTextInfo = new PbRichTextInfo();
                                CommonAssignment(pbRichTextInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(pbRichTextInfo);
                                break;
                            case DwControlType.TextEdit:
                                PbTextInfo textInfo = new PbTextInfo();
                                CommonAssignment(textInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(textInfo);
                                break;
                            default:
                                PbTextInfo textInfo1 = new PbTextInfo();
                                CommonAssignment(textInfo1, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(textInfo1);
                                break;
                        }
                        break;
                    case SqlDbType.DateTime:
                        PbDateTimeTextInfo dateTimeTextInfo = new PbDateTimeTextInfo();
                        CommonAssignment(dateTimeTextInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                        pbBaseTextInfos.Add(dateTimeTextInfo);
                        break;
                    case SqlDbType.Int:
                        switch (dc.ControlType)
                        {
                            case DwControlType.TextEdit:
                                PbIntTextInfo intTextInfo = new PbIntTextInfo();
                                CommonAssignment(intTextInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(intTextInfo);
                                break;
                            case DwControlType.CheckBoxEdit:
                                PbCheckboxInfo checkboxInfo = new PbCheckboxInfo();
                                CommonAssignment(checkboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(checkboxInfo);
                                break;
                            case DwControlType.RedioBoxEdit:
                                PbRadioboxInfo pbRadioboxInfo = new PbRadioboxInfo();
                                CommonAssignment(pbRadioboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(pbRadioboxInfo);
                                break;
                            case DwControlType.ComboBoxEdit:
                                PbComboboxInfo pbComboboxInfo = new PbComboboxInfo();
                                CommonAssignment(pbComboboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbComboboxInfo.PbComboboxValueInfos = ConvertToPairValueInfo(dwDbColumn.Values);
                                pbBaseTextInfos.Add(pbComboboxInfo);
                                break;
                            case DwControlType.DataHelpEdit:
                                PbDataHelpEditInfo pbDataHelpEditInfo = new PbDataHelpEditInfo();
                                CommonAssignment(pbDataHelpEditInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                SetHelpId(pbDataHelpEditInfo, dddwSourceDic);  //设置helpid
                                pbBaseTextInfos.Add(pbDataHelpEditInfo);
                                break;
                            case DwControlType.RichTextEdit:
                                PbRichTextInfo pbRichTextInfo = new PbRichTextInfo();
                                CommonAssignment(pbRichTextInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(pbRichTextInfo);
                                break;
                            default:
                                PbIntTextInfo intTextInfo1 = new PbIntTextInfo();
                                CommonAssignment(intTextInfo1, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(intTextInfo1);
                                break;
                        }
                        break;
                    case SqlDbType.Decimal:
                        switch (dc.ControlType)
                        {
                            case DwControlType.CheckBoxEdit:
                                PbCheckboxInfo checkboxInfo = new PbCheckboxInfo();
                                CommonAssignment(checkboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(checkboxInfo);
                                break;
                            case DwControlType.RedioBoxEdit:
                                PbRadioboxInfo pbRadioboxInfo = new PbRadioboxInfo();
                                CommonAssignment(pbRadioboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbRadioboxInfo.PbPairValueInfos = ConvertToPairValueInfo(dwDbColumn.Values);
                                pbBaseTextInfos.Add(pbRadioboxInfo);
                                break;
                            case DwControlType.ComboBoxEdit:
                                PbComboboxInfo pbComboboxInfo = new PbComboboxInfo();
                                CommonAssignment(pbComboboxInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbComboboxInfo.PbComboboxValueInfos = ConvertToPairValueInfo(dwDbColumn.Values);
                                pbBaseTextInfos.Add(pbComboboxInfo);
                                break;
                            case DwControlType.DataHelpEdit:
                                PbDataHelpEditInfo pbDataHelpEditInfo = new PbDataHelpEditInfo();
                                CommonAssignment(pbDataHelpEditInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                SetHelpId(pbDataHelpEditInfo, dddwSourceDic);  //设置helpid
                                pbBaseTextInfos.Add(pbDataHelpEditInfo);
                                break;
                            case DwControlType.RichTextEdit:
                                PbRichTextInfo pbRichTextInfo = new PbRichTextInfo();
                                CommonAssignment(pbRichTextInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(pbRichTextInfo);
                                break;
                            case DwControlType.TextEdit:
                                PbTextInfo textInfo = new PbTextInfo();
                                CommonAssignment(textInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(textInfo);
                                break;
                            default:
                                PbDecimalTextInfo pbDecimalTextInfo = new PbDecimalTextInfo();
                                CommonAssignment(pbDecimalTextInfo, dwDbColumn, dwText, dc, sqlDbType, dwAuthName);
                                pbBaseTextInfos.Add(pbDecimalTextInfo);
                                break;
                        }                        
                        break;
                    default:
                        break;
                }
            }

            //按钮列
            foreach (DwButton db in dwInfo.DwButtons)
            {
                //如果控件宽度或高度为0，该控件不加入列表
                if (db.Height == 0 || db.Width == 0)
                {
                    continue;
                }

                PbButtonInfo buttonInfo = new PbButtonInfo();

                buttonInfo.Name = db.Name;
                buttonInfo.FullName = db.Name;
                buttonInfo.LeftText = db.Text;
                buttonInfo.Height = db.Height;
                buttonInfo.Width = db.Width;
                buttonInfo.TextColor = db.Color;
                buttonInfo.Visible = db.Visible;

                buttonInfo.XPos = db.XPos;
                buttonInfo.YPos = db.YPos;
                buttonInfo.LabelWidth = db.Width;
                pbBaseTextInfos.Add(buttonInfo);
            }

            //纯文本列
            foreach (DwText dt in dwInfo.DwTexts)
            {
                bool isTitle = false;

                //如果控件宽度或高度为0，该控件不加入列表
                if (dt.Height == 0 || dt.Width == 0)
                {
                    continue;
                }

                //存在于列标签和列单元格的关联关系中，说明不是独立标签，是作为文本框的标签，则跳过
                if (colrefdic != null && colrefdic.ContainsValue(dt.Name)) continue;

                if (dt.Name.Equals(titlename) && !string.IsNullOrEmpty(titlename))
                {
                    isTitle = true;  //该标签作为标题
                }
                else
                {
                    //name最后两位不是"_t"的为独立标签
                    if (dt.Name.Substring(dt.Name.Length - 2, 2) == "_t") continue;
                }

                PbLabelInfo labelInfo = new PbLabelInfo();

                //add by ljy 2018.05.21 设置所有列的列占位数
                if (colspandic != null && colspandic.Count() != 0)
                {
                    if (colspandic.ContainsKey(dt.Name))  //有关联的标签和输入框
                    {
                        labelInfo.ColSpan = Convert.ToInt32(colspandic[dt.Name]);
                    }
                }

                labelInfo.IsTitle = isTitle;
                labelInfo.Name = dt.Name;
                labelInfo.FullName = dt.Name;
                labelInfo.LeftText = dt.Text;
                labelInfo.Height = dt.Height;
                labelInfo.Width = dt.Width;
                labelInfo.TextColor = dt.Color;
                labelInfo.LabelTextColor = dt.LabelTextColor;
                labelInfo.Visible = dt.Visible;

                labelInfo.XPos = dt.XPos;
                labelInfo.YPos = dt.YPos;

                labelInfo.Font = dt.Font;
                labelInfo.Align = dt.Align;
                labelInfo.LabelWidth = dt.Width;
                
                pbBaseTextInfos.Add(labelInfo);
            }

            return pbBaseTextInfos;
        }

        //ParseBillList和ParseBillBody 调用
        public static IList<PbBaseTextInfo> ToPbColumnsMetadata(DwInfo dwInfo, string dwAuthName, IDictionary<string, string> dddwSourceDic, string titlename, IDictionary<string, string> multiselectdic)
        {
            return ToPbColumnsMetadata(dwInfo, dwAuthName, dddwSourceDic, titlename, null, null, null, multiselectdic);
        }

    }
}
