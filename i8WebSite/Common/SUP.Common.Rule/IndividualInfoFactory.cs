using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataEntity.Individual;

namespace SUP.Common.Rule
{
    public class IndividualInfoFactory
    {

        public static ExtControlInfoBase GetControlInfo(string xtype, string name, string label,string fieldtype,int length,int declen)
        {
            ExtControlInfoBase control = null;
            switch (xtype)
            {
                case "ngText" :
                    NGText text = new NGText();
                    text.maxLength = length;
                    control = text;
                    break;
                case "ngTextArea" :
                    NGTextArea ctl = new NGTextArea();
                    ctl.maxLength = length;
                    control = ctl;
                    break;
                case "ngDate" : control = new NGDate();
                    break;
                case "ngDateTime" : control = new NGDateTime();
                    break;
                case "ngNumber": control = GetNumberCtl(fieldtype, length, declen,false);               
                    break;
                case "ngPercent":
                    control = GetNumberCtl(fieldtype, length, declen,true);                    
                    break;
                case "ngComboBox": control = new NGComboBox();
                    break;
                case "ngCommonHelp": control = new NGCommonHelp();
                    break;
                case "ngRichHelp":
                    control = new NGRichHelp();
                    break;
                case "ngRadio": control = new NGComboBox();
                    break;
                case "ngCheckbox": control = new NGCheckbox();
                    break;                    
                default: control = new NGText();
                    break;
            }

            if (xtype == "ngPercent")
            {
                control.xtype = "ngNumber";
            }
            else
            {
                control.xtype = xtype;
            }
            control.name = name;
            if (name.IndexOf("*") > 0)
            {
                control.itemId = name.Split('*')[0];
            }
            else {
                control.itemId = name;
            }
            control.fieldLabel = label;

            return control;
        }

        public static ExtControlInfoBase GetNumberCtl(string fieldtype, int length, int declen,bool showPercent)
        {
            ExtControlInfoBase ctl = null;


            if ("06" == fieldtype)
            {
                ctl = new NGNumber(fieldtype, length, declen,showPercent);
            }
            else {
                ctl = new NGInt(fieldtype);
            }

            return ctl;
        }

    }
}
