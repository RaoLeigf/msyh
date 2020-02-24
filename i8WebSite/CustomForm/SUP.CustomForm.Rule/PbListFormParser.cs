using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.PBDesignerParse;
using NG3.Metadata.UI.PowserBuilder;
using NG3.Metadata.UI.PowserBuilder.Events;
using SUP.CustomForm.DataEntity.Container;

using NG3.Metadata.UI.PowserBuilder.Controls;

namespace SUP.CustomForm.Rule
{
    public class PbListFormParser
    {
        public PbListFormParser()
        {
 
        }

        public GridPanel GetGridInfo(PbBillInfo billInfo)
        {
            GridPanel listgrid = new GridPanel();
            listgrid.ID = billInfo.PbList.Id;
            listgrid.Region = "center";//默认自适应
            listgrid.TableName = billInfo.PbList.TableName;
            listgrid.Sql = billInfo.PbList.Sql;

            listgrid.Columns = CommonParser.GetListColumns(billInfo.PbList.PbBaseTextInfos);

            //用于判断grid中某些列是否存在
            foreach (var item in listgrid.Columns)
            {
                listgrid.ColumnNames.Add(item.DataIndex);
            }

            CommonParser.Log("ListGrid转换成功.");

            return listgrid;
        }

        public Toolbar GetListToolbar(PbBillInfo billInfo)
        {
            Toolbar tb = new Toolbar();
            string btname = string.Empty;
            foreach (var buttons in billInfo.ListToolbarInfo.ToolbarButtonGroupInfosInfos)
            {
                if (buttons == null)
                {
                    continue;
                }
                    
                foreach (var button in buttons.ToolbarButtonInfos)
                {
                    if (button.IsDockLeft)
                    {
                        btname = ButtonNameConvert.ConvertToExtTpye(button);
                        if (!tb.LNgButtons.Contains(btname))
                        {
                            tb.LNgButtons.Add(btname);
                        }
                    }
                    else
                    {
                        btname = ButtonNameConvert.ConvertToExtTpye(button);
                        if (!tb.RNgButtons.Contains(btname))
                        {
                            tb.RNgButtons.Add(btname);
                        }
                    }
                }
            }

            return tb;
        }
    }

    //常见的按钮 还有些特殊的没写上去
    public class ButtonNameConvert
    {
        public static string ConvertToExtTpye(PbToolbarButtonInfo info)
        {
            string bname = string.Empty;
            switch (info.EventImpType)
            {
                case PbEventImpType.Add:
                    bname = "add";
                    break;
                case PbEventImpType.AddRow:
                    bname = "addrow";
                    break;
                case PbEventImpType.View:
                    bname = "view";
                    break;
                case PbEventImpType.Edit:
                    bname = "edit";
                    break;
                case PbEventImpType.Import:
                    bname = "import";
                    break;
                case PbEventImpType.Attachment:
                    bname = "attachment";
                    break;
                case PbEventImpType.Verify:
                    bname = "verify";
                    break;
                case PbEventImpType.UnVerify:
                    bname = "unverify";
                    break;
                case PbEventImpType.Check:
                    bname = "check";
                    break;
                case PbEventImpType.History:
                    bname = "history";
                    break;
                case PbEventImpType.ApplyCheck:
                    bname = "applycheck";
                    break;
                case PbEventImpType.Subbill:
                    bname = "subbill";
                    break;
                case PbEventImpType.Ok:
                    bname = "ok";
                    break;
                case PbEventImpType.Query:
                    bname = "query";
                    break;
                case PbEventImpType.Deal:
                    bname = "deal";
                    break;
                case PbEventImpType.Copy:
                    bname = "copy";
                    break;
                case PbEventImpType.Delete:
                    bname = "delete";
                    break;
                case PbEventImpType.DeleteRow:
                    bname = "deleterow";
                    break;
                case PbEventImpType.Exit:
                    bname = "exit";
                    break;
                case PbEventImpType.Close:
                    bname = "close";
                    break;
                case PbEventImpType.Help:
                    bname = "help";
                    break;
                case PbEventImpType.Insert:
                    bname = "addrow";
                    break;
                case PbEventImpType.Print:
                    bname = "print";
                    break;
                case PbEventImpType.Refresh:
                    bname = "refresh";
                    break;
                case PbEventImpType.Save:
                    bname = "save";
                    break;
                default:
                    bname = "";  //不清楚的时候
                    break;
            }

            return bname;
        }
    }

}
