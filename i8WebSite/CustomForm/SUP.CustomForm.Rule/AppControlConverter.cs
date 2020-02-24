using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Controls;
using SUP.CustomForm.DataEntity.AppControl;
using System.Data;
using SUP.CustomForm.DataAccess;

namespace SUP.CustomForm.Rule
{
     public class AppControlConverter
    {
         public static BaseField ConvertToExtControl(PbBaseControlInfo pbcontrol)
         {
             BaseField field = null;
             switch (pbcontrol.ControlType)
             {
                 case PbControlType.Button: field = GetButton(pbcontrol);
                     break;
                 case PbControlType.Checkbox: field = GetCheckBox(pbcontrol);
                     break;
                 case PbControlType.ComboBox: field = GetSelect(pbcontrol);
                     break;
                 case PbControlType.DateTimeText: field = GetDate(pbcontrol);
                     break;
                 case PbControlType.DecimalText: field = GetNumber(pbcontrol);
                     break;
                 case PbControlType.IntText: field = GetNumber(pbcontrol);
                     break;
                 case PbControlType.Radiobox: field = GetRadio(pbcontrol);
                     break;
                 case PbControlType.Text: field = GetText(pbcontrol);
                     break;
                 case PbControlType.DataHelpEdit:
                     {
                        DataRow[] drs = HelpDac.XmlHelpDT.Select("Id='" + pbcontrol.Name + "'");
                        if (drs.Length > 0)//系统内置的通用帮助
                         {                             
                             field = GetHelp(pbcontrol, drs[0]);
                         }
                         else
                         {
                             field = GetCustomHelp(pbcontrol);//pb自定义表单定义的帮助
                         }
                     };
                     break;
                 default: field = GetText(pbcontrol);
                     break;

             }

             return field;
         }

         private static BaseField GetButton(PbBaseControlInfo pbCtl)
         {
             PbButtonInfo pbBtn = (PbButtonInfo)pbCtl;
             Button ngBtn = new Button();

             ngBtn.ID = pbBtn.Id;
             ngBtn.Text = pbBtn.LeftText;
             ngBtn.XType = "button";

             return ngBtn;
         }

         private static BaseField GetCheckBox(PbBaseControlInfo pbCtl)
         {
             PbCheckboxInfo pbChk = (PbCheckboxInfo)pbCtl;
             CheckBoxField ngChk = new CheckBoxField();

             ngChk.ID = pbChk.Id;
             ngChk.Name = pbChk.Name;
             ngChk.FieldLabel = pbChk.LeftText;
             ngChk.MustInput = pbChk.IsMustInput;
             ngChk.XType = "ngCheckbox";            
             return ngChk;
         }

         private static BaseField GetSelect(PbBaseControlInfo pbCtl)
         {
             PbComboboxInfo pbComb = (PbComboboxInfo)pbCtl;
             SelectField ngSelect = new SelectField();

             ngSelect.ID = pbComb.Id;
             ngSelect.Name = pbComb.Name;
             ngSelect.FieldLabel = pbComb.LeftText;
             ngSelect.MustInput = pbComb.IsMustInput;
             ngSelect.XType = "ngSelect";           

   
             foreach (var info in pbComb.PbComboboxValueInfos)
             {
                 ngSelect.Options.Add(string.Format(@"text:'{0}',value:'{1}'",
                     info.SaveValue, info.DisplayValue));
             }

             return ngSelect;
         }

         private static BaseField GetDate(PbBaseControlInfo pbCtl)
         {

             PbDateTimeTextInfo pbDate = (PbDateTimeTextInfo)pbCtl;
             DatePickerField ngDate = new DatePickerField();

             ngDate.ID = pbDate.Id;
             ngDate.Name = pbDate.Name;
             ngDate.FieldLabel = pbDate.LeftText;
             ngDate.MustInput = pbDate.IsMustInput;
             ngDate.XType = "ngDatePicker";             
             return ngDate;
         }

         public static BaseField GetNumber(PbBaseControlInfo pbCtl)
         {
             PbBaseTextInfo pbDec = new PbBaseTextInfo();

             if (pbCtl.ControlType == PbControlType.DecimalText)
                 pbDec = (PbDecimalTextInfo)pbCtl;
             if (pbCtl.ControlType == PbControlType.IntText)
                 pbDec = (PbIntTextInfo)pbCtl;

             NumberField ngNum = new NumberField();

             ngNum.ID = pbDec.Id;
             ngNum.Name = pbDec.Name;
             ngNum.MustInput = pbDec.IsMustInput;
             ngNum.FieldLabel = pbDec.LeftText;             
             ngNum.XType = "ngNumber";             
             return ngNum;
         }

         public static BaseField GetRadio(PbBaseControlInfo pbCtl)
         {
             PbRadioboxInfo pbRadio = (PbRadioboxInfo)pbCtl;
             RadioField ngRadio = new RadioField();

             ngRadio.ID = pbRadio.Id;
             ngRadio.Name = pbRadio.Name;
             ngRadio.FieldLabel = pbRadio.LeftText;
             ngRadio.MustInput = pbRadio.IsMustInput;
             ngRadio.XType = "ngRadio";             

             foreach (var info in pbRadio.PbPairValueInfos)
             {
                 ngRadio.Items.Add(string.Format(@"xtype:'ngRadio',label:'{0}',name:'{1}',value:'{2}'",
                     info.DisplayValue,pbRadio.Name, info.SaveValue));
             }
             return ngRadio;
         }

         public static BaseField GetText(PbBaseControlInfo pbCtl)
         {
             PbTextInfo pbText = (PbTextInfo)pbCtl;
             TextField ngText = new TextField();

             ngText.ID = pbText.Id;
             ngText.Name = pbText.Name;
             ngText.FieldLabel = pbText.LeftText;
             ngText.MustInput = pbText.IsMustInput;
             ngText.MaxLength = pbText.ColumnInfo.TextLen;
             ngText.XType = pbText.Height > 30 ? "ngTextArea" : "ngText";  //认为大于2行的就是textArea             
             return ngText;
         }


         /// <summary>
         /// pb 自定义表单定义的帮助
         /// </summary>
         /// <param name="pbCtl"></param>
         /// <returns></returns>
         public static BaseField GetCustomHelp(PbBaseControlInfo pbCtl)
         {
             PbDataHelpEditInfo pbDataHelp = (PbDataHelpEditInfo)pbCtl;
             BaseField baseField = null;

             DataTable dt = SUP.CustomForm.DataAccess.Common.GetHelpInfo(pbDataHelp.DataHelpId);
             //这时候需要去数据库查 helpid
             if (dt != null)
             {
                 if (dt.Rows[0]["fromsql"].Equals("1")) //弹出帮助类型的
                 {
                     CustomHelpField ngCustomFormHelp = new CustomHelpField();

                     ngCustomFormHelp.ID = pbDataHelp.Id;
                     ngCustomFormHelp.Name = pbDataHelp.Name;
                     ngCustomFormHelp.FieldLabel = pbDataHelp.LeftText;
                     ngCustomFormHelp.MustInput = pbDataHelp.IsMustInput;
                     ngCustomFormHelp.XType = "ngCustomHelp";
                     ngCustomFormHelp.HelpID = pbDataHelp.DataHelpId;

                     baseField = ngCustomFormHelp;

                     //ngCustomFormHelp.ListHeadTexts = dt.Rows[0]["datetitle"].ToString() + "," +
                     //                             dt.Rows[0]["viewtitle"].ToString();
                     //ngCustomFormHelp.ValueField = dt.Rows[0]["col_data"].ToString();
                     //ngCustomFormHelp.DisplayField = dt.Rows[0]["col_view"].ToString();
                     //ngCustomFormHelp.ListFields = dt.Rows[0]["col_data"].ToString() + "," +
                     //                          dt.Rows[0]["col_view"].ToString();
                 }
                 else
                 { //下拉式两列的帮助

                     SelectField  select = new SelectField();
                     select.XType = "ngSelect";
                                          
                     foreach (DataRow info in dt.Rows)
                     {
                         select.Options.Add(string.Format(@"text:'{0}',value:'{1}'",
                             info["base_code"].ToString(), info["base_name"].ToString()));
                     }

                     baseField = select;
                 }
             }             

             return baseField;
         }


         public static BaseField GetHelp(PbBaseControlInfo pbCtl, DataRow dr)
         {
             PbDataHelpEditInfo pbDataHelp = (PbDataHelpEditInfo)pbCtl;
             HelpField ngHelp = new HelpField();

             ngHelp.ID = pbDataHelp.Id;
             ngHelp.Name = pbDataHelp.Name;
             ngHelp.FieldLabel = pbDataHelp.LeftText;
             ngHelp.MustInput = pbDataHelp.IsMustInput;
             ngHelp.XType = "ngHelp";           

             return ngHelp;
         }
    }
}
