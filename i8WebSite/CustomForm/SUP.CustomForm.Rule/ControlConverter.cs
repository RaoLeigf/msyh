using System;
using System.Data;
using System.Text.RegularExpressions;
using NG3.Metadata.UI.PowserBuilder.Controls;
using SUP.CustomForm.DataAccess;
using SUP.CustomForm.DataEntity.Container;
using SUP.CustomForm.DataEntity.Control;

namespace SUP.CustomForm.Rule
{
    public class ControlConverter
    {
        public static GridPanel ConvertToExtPanel(PbGridInfo pbinfo)
        {
            GridPanel gridPanel = new GridPanel();
            gridPanel.ID = pbinfo.Id;
            gridPanel.Region = "center";//默认自适应
            gridPanel.TableName = pbinfo.TableName;
            gridPanel.TitleName = pbinfo.FullName;
            gridPanel.Sql = pbinfo.Sql;
            gridPanel.X = pbinfo.XPos;
            gridPanel.Y = pbinfo.YPos;
            gridPanel.Height = pbinfo.Height;
            gridPanel.Width = pbinfo.Width;
            gridPanel.IsInTab = pbinfo.IsInTab;
            gridPanel.Subtotal = pbinfo.Subtotal;
            gridPanel.Sumdic = pbinfo.Sumdic;
            gridPanel.Subdic = pbinfo.Subdic;
            gridPanel.Groupfield = pbinfo.Groupfield;
            gridPanel.Groupcolsdic = pbinfo.Groupcolsdic;
            gridPanel.LevelSum = pbinfo.LevelSum;
            gridPanel.Columns = CommonParser.GetListColumns(pbinfo.PbBaseTextInfos);
            gridPanel.Collapse = pbinfo.Collapse;
            gridPanel.Title = pbinfo.Title;

            //用于判断grid中某些列是否存在
            foreach (var item in gridPanel.Columns)
            {
                gridPanel.ColumnNames.Add(item.DataIndex);

                //分组列是帮助列的话改用名称列分组
                if (item.editor.XType == "ngCustomFormHelp" || item.editor.XType == "ngRichHelp")
                {
                    if (gridPanel.Groupfield.ContainsKey(item.DataIndex))  //有关联的标签和输入框
                    {
                        gridPanel.Groupfield[item.DataIndex] = item.DataIndex + "_name";
                        pbinfo.Groupfield[item.DataIndex] = item.DataIndex + "_name";
                    }
                }
            }

            return gridPanel;
        }


        public static ExtControlBase ConvertToExtControl(PbBaseControlInfo pbcontrol)
        {
            ExtControlBase extControl = null;

            switch (pbcontrol.ControlType)
            {
                case PbControlType.Label:
                    extControl = GetLabel(pbcontrol);
                    break;
                case PbControlType.Button:
                    extControl = GetButton(pbcontrol);
                    break;
                case PbControlType.Checkbox:
                    extControl = GetCheckBox(pbcontrol);
                    break;
                case PbControlType.ComboBox:
                    extControl = GetCombox(pbcontrol);
                    break;
                case PbControlType.DateTimeText:
                    extControl = GetDate(pbcontrol);
                    break;
                case PbControlType.DecimalText:
                    extControl = GetNumber(pbcontrol);
                    break;
                case PbControlType.IntText:
                    extControl = GetNumber(pbcontrol);
                    break;
                case PbControlType.Radiobox:
                    extControl = GetRadio(pbcontrol);
                    break;
                case PbControlType.Text:
                    extControl = GetText(pbcontrol);
                    break;
                case PbControlType.DataHelpEdit:

                    DataRow[] drs = HelpDac.RichHelpDT.Select("helpid='" + ((PbDataHelpEditInfo)pbcontrol).DataHelpId + "'");
                    if (drs.Length > 0)
                    {
                        extControl = GetRichHelp(pbcontrol, drs[0]);
                    }
                    else
                    {
                        extControl = GetCustomFormHelp(pbcontrol);
                    }

                    break;
                default:
                    extControl = GetText(pbcontrol);
                    break;
            }
            extControl.XPos = Convert.ToInt32(pbcontrol.XPos * 1.3);
            extControl.YPos = Convert.ToInt32(pbcontrol.YPos);
            extControl.Width = Convert.ToInt32(pbcontrol.Width * 1.3);
            extControl.Height = Convert.ToInt32(pbcontrol.Height);
            extControl.SingleText = pbcontrol.SingleText;
            extControl.LabelWidth = Convert.ToInt32(pbcontrol.LabelWidth * 1.3);
            extControl.DefaultValue = pbcontrol.DefaultValue;
            extControl.Tag = pbcontrol.Tag;
            extControl.ColSpan = pbcontrol.ColSpan;
            extControl.Format = pbcontrol.Format;
            extControl.EditMask = pbcontrol.EditMask;

            return extControl;
        }

        public static NGLabel GetLabel(PbBaseControlInfo pbCtl)
        {
            PbLabelInfo pbLabel = (PbLabelInfo)pbCtl;
            NGLabel ngLabel = new NGLabel();
            string alignment = string.Empty;
            ngLabel.ID = pbLabel.Name;
            ngLabel.Name = pbLabel.Name;
            ngLabel.Text = pbLabel.LeftText;
            ngLabel.FieldLabel = pbLabel.LeftText;
            ngLabel.XType = "displayfield";
            ngLabel.Visible = pbLabel.Visible;
            ngLabel.Font = pbLabel.Font;
            ngLabel.Align = pbLabel.Align;
            if (pbCtl.IsTitle)
            {
                ngLabel.IsTitle = true;
            }
            switch (ngLabel.Align)
            {
                case 0:
                    alignment = "left";
                    break;
                case 1:
                    alignment = "right";
                    break;
                case 2:
                    alignment = "center";
                    break;
            }

            ngLabel.FieldStyle = string.Format("text-align: {0};font-size: {1}pt !important;color:{2};", alignment, ngLabel.Font, GetRgb(pbLabel.LabelTextColor));

            return ngLabel;
        }

        public static NGButton GetButton(PbBaseControlInfo pbCtl)
        {
            PbButtonInfo pbBtn = (PbButtonInfo)pbCtl;
            NGButton ngBtn = new NGButton();

            ngBtn.ID = pbBtn.Name;
            ngBtn.Name = pbBtn.Name;
            ngBtn.Text = pbBtn.LeftText;
            ngBtn.FieldLabel = pbBtn.LeftText;
            ngBtn.XType = "button";
            ngBtn.Visible = pbBtn.Visible;

            return ngBtn;
        }

        public static NGCheckbox GetCheckBox(PbBaseControlInfo pbCtl)
        {
            PbCheckboxInfo pbChk = (PbCheckboxInfo)pbCtl;
            NGCheckbox ngChk = new NGCheckbox();

            ngChk.ID = pbChk.Id;
            ngChk.Name = pbChk.Name;
            ngChk.FieldLabel = pbChk.LeftText;
            ngChk.XType = "ngCheckbox";
            ngChk.Visible = pbChk.Visible;
            ngChk.MustInput = pbChk.IsMustInput;
            ngChk.Protect = pbChk.IsProtect;
            return ngChk;
        }

        public static NGComboBox GetCombox(PbBaseControlInfo pbCtl)
        {
            PbComboboxInfo pbComb = (PbComboboxInfo)pbCtl;
            NGComboBox ngComb = new NGComboBox();

            ngComb.ID = pbComb.Id;
            ngComb.Name = pbComb.Name;
            ngComb.FieldLabel = pbComb.LeftText;
            ngComb.MustInput = pbComb.IsMustInput;
            ngComb.XType = "ngComboBox";
            ngComb.Visible = pbComb.Visible;
            ngComb.MustInput = pbComb.IsMustInput;
            ngComb.Protect = pbComb.IsProtect;
            ngComb.FieldStyle = GetRgb(pbComb.TextColor);
            ngComb.LabelStyle = GetRgb(pbComb.LabelTextColor);
            ngComb.QueryMode = "local";
            foreach (var info in pbComb.PbComboboxValueInfos)
            {
                ngComb.Data.Add(string.Format(@"code:'{0}', name:'{1}'", info.SaveValue, info.DisplayValue));
            }

            return ngComb;
        }

        public static NGDate GetDate(PbBaseControlInfo pbCtl)
        {

            PbDateTimeTextInfo pbDate = (PbDateTimeTextInfo)pbCtl;
            NGDate ngDate = new NGDate();

            ngDate.ID = pbDate.Id;
            ngDate.Name = pbDate.Name;
            ngDate.FieldLabel = pbDate.LeftText;
            ngDate.MustInput = pbDate.IsMustInput;
            ngDate.XType = "ngDate";
            ngDate.Visible = pbDate.Visible;
            ngDate.MustInput = pbDate.IsMustInput;
            ngDate.Protect = pbDate.IsProtect;
            ngDate.FieldStyle = GetRgb(pbDate.TextColor);
            ngDate.LabelStyle = GetRgb(pbDate.LabelTextColor);
            return ngDate;
        }

        public static NGNumber GetNumber(PbBaseControlInfo pbCtl)
        {
            PbBaseTextInfo pbDec = new PbBaseTextInfo();

            if (pbCtl.ControlType == PbControlType.DecimalText)
                pbDec = (PbDecimalTextInfo)pbCtl;
            if (pbCtl.ControlType == PbControlType.IntText)
                pbDec = (PbIntTextInfo)pbCtl;

            NGNumber ngNum = new NGNumber();

            ngNum.ID = pbDec.Id;
            ngNum.Name = pbDec.Name;
            ngNum.MustInput = pbDec.IsMustInput;
            ngNum.FieldLabel = pbDec.LeftText;
            ngNum.DecimalPrecision = pbDec.ColumnInfo.Precision;
            ngNum.XType = "ngNumber";
            ngNum.Visible = pbDec.Visible;
            ngNum.MustInput = pbDec.IsMustInput;
            ngNum.Protect = pbDec.IsProtect;
            ngNum.FieldStyle = GetRgb(pbDec.TextColor);
            ngNum.LabelStyle = GetRgb(pbDec.LabelTextColor);
            return ngNum;
        }

        public static NGRadio GetRadio(PbBaseControlInfo pbCtl)
        {
            PbRadioboxInfo pbRadio = (PbRadioboxInfo)pbCtl;
            NGRadio ngRadio = new NGRadio();

            ngRadio.ID = pbRadio.Id;
            ngRadio.Name = pbRadio.Name;
            ngRadio.FieldLabel = pbRadio.LeftText;
            ngRadio.MustInput = pbRadio.IsMustInput;
            ngRadio.XType = "ngRadio";
            ngRadio.Visible = pbRadio.Visible;
            ngRadio.MustInput = pbRadio.IsMustInput;
            ngRadio.Protect = pbRadio.IsProtect;

            foreach (var info in pbRadio.PbPairValueInfos)
            {
                ngRadio.Items.Add(string.Format(@"boxLabel:'{0}',name:'{1}',inputValue:'{2}'",
                    info.DisplayValue, pbRadio.Name, info.SaveValue));
            }
            return ngRadio;
        }

        public static NGText GetText(PbBaseControlInfo pbCtl)
        {

            PbTextInfo pbText = (PbTextInfo)pbCtl;
            NGText ngText = new NGText();
            //long color = pbText.TextColor;
            //int red, green, blue = 0;

            ngText.ID = pbText.Id;
            ngText.Name = pbText.Name;
            ngText.FieldLabel = pbText.LeftText;
            ngText.MustInput = pbText.IsMustInput;
            ngText.XType = pbText.TextArea == true ? "ngTextArea" : "ngText";  //根据设计器列属性设置判断是否文本域
            ngText.Visible = pbText.Visible;
            ngText.MustInput = pbText.IsMustInput;
            ngText.Protect = pbText.IsProtect;
            ngText.MaxLength = pbText.MaxLength;
            ngText.FieldStyle = GetRgb(pbText.TextColor);//输入框字体颜色
            ngText.LabelStyle = GetRgb(pbText.LabelTextColor);//标签字体颜色
            ////将编辑页面的输入框颜色解析转变成前台代码
            //if (color > 0)
            //{
            //    blue = Convert.ToInt32(color / (256 * 256));
            //    green = Convert.ToInt32((color - blue * 256 * 256) / 256);
            //    red = Convert.ToInt32(color - green * 256 - blue * 256 * 256);
            //    ngText.FieldStyle = "color:rgb(" + red + "," + green + "," + blue + ");";
            //}
            //else {
            //    ngText.FieldStyle = "color:rgb(0,0,0);";
            //}

            return ngText;
        }


        public static NGCommonHelp GetCustomFormHelp(PbBaseControlInfo pbCtl)
        {
            PbDataHelpEditInfo pbDataHelp = (PbDataHelpEditInfo)pbCtl;
            NGCommonHelp ngCustomFormHelp = new NGCommonHelp();

            ngCustomFormHelp.ID = pbDataHelp.Id;
            ngCustomFormHelp.Name = pbDataHelp.Name;
            ngCustomFormHelp.FieldLabel = pbDataHelp.LeftText;
            ngCustomFormHelp.MustInput = pbDataHelp.IsMustInput;
            ngCustomFormHelp.XType = "ngCustomFormHelp";
            ngCustomFormHelp.Visible = pbDataHelp.Visible;
            ngCustomFormHelp.MustInput = pbDataHelp.IsMustInput;
            ngCustomFormHelp.Protect = pbDataHelp.IsProtect;
            ngCustomFormHelp.Helpid = pbDataHelp.DataHelpId;
            ngCustomFormHelp.OutFilter = "";
            ngCustomFormHelp.MultiSelect = pbDataHelp.MultiSelect;
            ngCustomFormHelp.FieldStyle = GetRgb(pbDataHelp.TextColor);
            ngCustomFormHelp.LabelStyle = GetRgb(pbDataHelp.LabelTextColor);

            //查帮助信息
            DataTable dt = SUP.CustomForm.DataAccess.Common.GetHelpInfo(ngCustomFormHelp.Helpid);

            //这时候需要去数据库查 helpid
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["fromsql"].Equals("1")) //弹出帮助类型的
                {
                    ngCustomFormHelp.ValueField = dt.Rows[0]["col_data"].ToString();
                    ngCustomFormHelp.DisplayField = dt.Rows[0]["col_view"].ToString();
                    ngCustomFormHelp.ListFields = dt.Rows[0]["col_data"].ToString() + "," + dt.Rows[0]["col_view"].ToString();
                    ngCustomFormHelp.ListHeadTexts = dt.Rows[0]["datetitle"].ToString() + "," + dt.Rows[0]["viewtitle"].ToString();
                }
                else
                {
                    //下拉式两列的帮助
                    ngCustomFormHelp.XType = "ngComboBox";
                    ngCustomFormHelp.QueryMode = "local";
                    ngCustomFormHelp.ValueField = "code";
                    ngCustomFormHelp.DisplayField = "name";
                    ngCustomFormHelp.ListFields = "code,name";
                    ngCustomFormHelp.ListHeadTexts = "代码,名称";

                    foreach (DataRow info in dt.Rows)
                    {
                        ngCustomFormHelp.Data.Add(string.Format(@"code:'{0}',name:'{1}'", info["phid"].ToString(), info["base_name"].ToString()));
                    }
                }

                //--设置sql中的过滤填充值
                string sql = dt.Rows[0]["sql_str"].ToString() + " ";
                string outfilter = string.Empty;

                //得到sql中:和空格之间的串
                string start = @"\:";
                string end = @"\ ";
                Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                MatchCollection macths = rg.Matches(sql);
                int bracketPos = 0;
                string tempValue = string.Empty;

                if (macths.Count > 0)
                {
                    for (int i = 0; i < macths.Count; i++)
                    {
                        tempValue = macths[i].Value;

                        //去掉可能存在的小括号符
                        do
                        {
                            tempValue = tempValue.Replace(")", " ");
                            bracketPos = tempValue.IndexOf(")");

                        } while (bracketPos != (-1));

                        outfilter += ";" + tempValue.Trim();
                    }

                    outfilter = outfilter.Substring(1, outfilter.Length - 1);  //去掉第一个多余的分号
                    ngCustomFormHelp.OutFilter = outfilter;
                }
                //--end
            }
            else
            {
                ngCustomFormHelp.ValueField = "code";
                ngCustomFormHelp.DisplayField = "name";
                ngCustomFormHelp.ListFields = "code,name";
                ngCustomFormHelp.ListHeadTexts = "代码,名称";
            }

            return ngCustomFormHelp;
        }

        public static NGCommonHelp GetRichHelp(PbBaseControlInfo pbCtl, DataRow dr)
        {
            PbDataHelpEditInfo pbDataHelp = (PbDataHelpEditInfo)pbCtl;
            NGCommonHelp ngRichHelp = new NGCommonHelp();

            string xtypeStr = string.Empty;
            DataRow[] xmlDrs = HelpDac.XmlHelpDT.Select("Id='" + pbDataHelp.Name + "'");
            if (xmlDrs.Length > 0)
            {
                xtypeStr = xmlDrs[0]["xtype"].ToString().Trim();

                if (xtypeStr == "WbsHelpField" || xtypeStr == "CbsHelpField")
                {
                    ngRichHelp.OutFilter = "pc";
                }
            }

            //组件类型名不为空，说明是组件，组件ValueField等从xml文件取
            if (!string.IsNullOrEmpty(xtypeStr))
            {
                ngRichHelp.ValueField = xmlDrs[0]["Code"].ToString();
                ngRichHelp.DisplayField = xmlDrs[0]["Name"].ToString();
            }
            else
            {
                //project_table.phid，如果带表名则去掉表名
                var codefield = dr["codefield"].ToString().Trim();
                var namefield = dr["namefield"].ToString().Trim();

                if (codefield.IndexOf(".") > 0)
                {
                    codefield = codefield.Substring(codefield.IndexOf(".") + 1).Trim();
                }
                if (namefield.IndexOf(".") > 0)
                {
                    namefield = namefield.Substring(namefield.IndexOf(".") + 1).Trim();
                }

                ngRichHelp.ValueField = codefield;
                ngRichHelp.DisplayField = namefield;
            }

            ngRichHelp.ID = pbDataHelp.Id;
            ngRichHelp.Name = pbDataHelp.Name;
            ngRichHelp.FieldLabel = pbDataHelp.LeftText;
            ngRichHelp.MustInput = pbDataHelp.IsMustInput;
            ngRichHelp.XType = "ngRichHelp";
            ngRichHelp.CmpName = xtypeStr;
            ngRichHelp.Visible = pbDataHelp.Visible;
            ngRichHelp.MustInput = pbDataHelp.IsMustInput;
            ngRichHelp.Protect = pbDataHelp.IsProtect;
            ngRichHelp.Helpid = dr["helpid"].ToString();
            ngRichHelp.ListFields = ngRichHelp.ValueField + "," + ngRichHelp.DisplayField;
            ngRichHelp.ListHeadTexts = "代码,名称";
            ngRichHelp.MultiSelect = pbDataHelp.MultiSelect;
            ngRichHelp.FieldStyle = GetRgb(pbDataHelp.TextColor);
            ngRichHelp.LabelStyle = GetRgb(pbDataHelp.LabelTextColor);

            return ngRichHelp;
        }

        /// <summary>
        /// 解析pb的color值，转换成rgb     2018-3-26 by lh 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetRgb(long color)
        {
            int red, green, blue = 0;
            string fieldStyle = string.Empty;
            if (color > 0 && color != 33554432)
            {
                blue = Convert.ToInt32(color / (256 * 256));
                green = Convert.ToInt32((color - blue * 256 * 256) / 256);
                red = Convert.ToInt32(color - green * 256 - blue * 256 * 256);
                fieldStyle = "rgb(" + red + "," + green + "," + blue + ");";
            }
            return fieldStyle;
        }

        public static NGPictureBox GetPictureBox(PbPictureboxInfo pbCtl)
        {
            NGPictureBox ngPictureBox = new NGPictureBox();

            ngPictureBox.ID = pbCtl.Name;
            ngPictureBox.Name = pbCtl.Name;
            ngPictureBox.XPos = pbCtl.XPos;
            ngPictureBox.YPos = pbCtl.YPos;
            ngPictureBox.Width = pbCtl.Width;
            ngPictureBox.Height = pbCtl.Height;
            ngPictureBox.Visible = pbCtl.Visible;
            ngPictureBox.Tag = pbCtl.Tag;
            ngPictureBox.ColSpan = pbCtl.ColSpan;

            return ngPictureBox;
        }

    }
}
