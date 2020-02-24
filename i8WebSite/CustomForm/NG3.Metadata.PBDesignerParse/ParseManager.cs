using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NG3.Metadata.PBDesignerParse.DataWindow;
using NG3.Metadata.UI.PowserBuilder;
using NG3.Metadata.UI.PowserBuilder.Controls;
using NG3.Metadata.UI.PowserBuilder.Events;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.PBDesignerParse
{

    public class ParseManager
    {
        private PbToolbarButtonGroupInfo ConvertToolbarButtonStr(string toolbarButtonStr, bool isLeftDock)
        {
            PbToolbarButtonInfo pbToolbarButtonInfo = new PbToolbarButtonInfo();
            PbToolbarButtonGroupInfo pbToolbarButtonGroupInfo = new PbToolbarButtonGroupInfo();
            PbEventImpType pbEventImpType = PbEventImpType.Other;
            string name = string.Empty;
            switch (toolbarButtonStr)
            {
                case "cb_save":
                    pbEventImpType = PbEventImpType.Save;
                    name = "保存";
                    break;
                case "cb_add":
                    pbEventImpType = PbEventImpType.AddRow;
                    name = "增行";//"新增";
                    break;
                case "cb_insert":
                    //pbEventImpType = PbEventImpType.Insert;
                    //name = "插入";
                    break;
                case "cb_del":
                    pbEventImpType = PbEventImpType.DeleteRow;
                    name = "删行";
                    break;
                case "cb_import":
                    pbEventImpType = PbEventImpType.Import;
                    name = "导入";
                    break;
                case "cb_attach":
                    pbEventImpType = PbEventImpType.Attachment;
                    name = "附件";
                    break;
                case "cb_check":
                    pbEventImpType = PbEventImpType.Verify;
                    name = "审核";
                    break;
                case "cb_uncheck":
                    pbEventImpType = PbEventImpType.UnVerify;
                    name = "去审核";
                    break;
                case "cb_audit":
                    pbEventImpType = PbEventImpType.Check;
                    name = "送审";
                    break;
                case "cb_trace":
                    pbEventImpType = PbEventImpType.History;
                    name = "送审追踪";
                    break;
                case "cb_apply":
                    pbEventImpType = PbEventImpType.ApplyCheck;
                    name = "申请去审";
                    break;
                case "cb_subbill":
                    pbEventImpType = PbEventImpType.Subbill;
                    name = "下达";
                    break;
                case "cb_ok":
                    pbEventImpType = PbEventImpType.Ok;
                    name = "提交";
                    break;
                case "cb_query":
                    pbEventImpType = PbEventImpType.Query;
                    name = "汇总";
                    break;
                case "cb_deal":
                    pbEventImpType = PbEventImpType.Deal;
                    name = "原汇总信息";
                    break;
                //case "cb_help":
                //    pbEventImpType = PbEventImpType.Help;
                //    name = "帮助";
                //    break;
                case "cb_print":
                    pbEventImpType = PbEventImpType.Print;
                    name = "打印";
                    break;
                case "cb_exit":
                    pbEventImpType = PbEventImpType.Close;
                    name = "关闭";
                    break;
                case "0":
                    pbToolbarButtonGroupInfo.IsSplit = true;
                    return pbToolbarButtonGroupInfo;
                default:
                    return null;
            }

            PbBuildInImp pbBuildInImp = new PbBuildInImp();
            pbBuildInImp.EventImpType = pbEventImpType;
            pbToolbarButtonInfo.EventImpType = pbEventImpType;
            pbToolbarButtonInfo.ClickEvent.PbImp.Add(pbBuildInImp);
            pbToolbarButtonInfo.Name = pbToolbarButtonInfo.FullName = pbToolbarButtonInfo.Description = pbToolbarButtonInfo.TooltipName = name;
            pbToolbarButtonInfo.IsDockLeft = isLeftDock;
            pbToolbarButtonInfo.IsShowDisplayName = true;
            pbToolbarButtonInfo.Visible = true;

            pbToolbarButtonGroupInfo.ToolbarButtonInfos.Add(pbToolbarButtonInfo);
            return pbToolbarButtonGroupInfo;
        }

        /// <summary>
        /// 解析列表工具栏
        /// </summary>
        /// <param name="billInfo"></param>
        private void ParseListToolbar(PbBillInfo billInfo, string fileName)
        {
            billInfo.ListToolbarInfo.Name = "Toolbar";
            billInfo.ListToolbarInfo.Description = "工具栏";
            billInfo.ListToolbarInfo.FullName = "Toolbar";
            billInfo.ListToolbarInfo.Visible = true;

            List<string> leftButtonNames = new string[] { "新增", "修改", "删除", "查看", "刷新", "复制" }.ToList();
            List<PbEventImpType> leftPbEventImpTypes = new PbEventImpType[]
            {
                PbEventImpType.Add, PbEventImpType.Edit, PbEventImpType.Delete, PbEventImpType.View, PbEventImpType.Refresh, PbEventImpType.Copy
            }.ToList();

            //根据编辑界面toolbar上按钮增加列表按钮
            string leftBarStr = IniHelp.ReadValue(DwRes.EditToolBarSection, DwRes.BarLeftSection, fileName);

            if (leftBarStr.IndexOf("cb_attach") > 0)
            {
                leftButtonNames.Add("附件");
                leftPbEventImpTypes.Add(PbEventImpType.Attachment);
            }
            if (leftBarStr.IndexOf("cb_check") > 0)
            {
                leftButtonNames.Add("审核");
                leftPbEventImpTypes.Add(PbEventImpType.Verify);

                leftButtonNames.Add("去审核");
                leftPbEventImpTypes.Add(PbEventImpType.UnVerify);
            }
            if (leftBarStr.IndexOf("cb_audit") > 0)
            {
                leftButtonNames.Add("送审");
                leftPbEventImpTypes.Add(PbEventImpType.Check);

                //有送审按钮自动加上送审追踪按钮
                leftButtonNames.Add("送审追踪");
                leftPbEventImpTypes.Add(PbEventImpType.History);
            }
            if (leftBarStr.IndexOf("cb_apply") > 0)
            {
                //增加申请去审
                leftButtonNames.Add("申请去审");
                leftPbEventImpTypes.Add(PbEventImpType.ApplyCheck);
            }

            string[] rightButtonNames = new string[] { "折叠/展开", "打印", "关闭" };  //, "帮助"
            PbEventImpType[] rightPbEventImpTypes = new PbEventImpType[]
            {
                PbEventImpType.QueryFold, PbEventImpType.Print, PbEventImpType.Close  //, PbEventImpType.Help
            };

            for (int i = 0; i < leftButtonNames.Count; i++)
            {
                PbToolbarButtonInfo pbToolbarButtonInfo = new PbToolbarButtonInfo();
                PbBuildInImp pbBuildInImp = new PbBuildInImp();
                pbBuildInImp.EventImpType = leftPbEventImpTypes[i];
                pbToolbarButtonInfo.EventImpType = leftPbEventImpTypes[i];

                pbToolbarButtonInfo.ClickEvent.PbImp.Add(pbBuildInImp);
                pbToolbarButtonInfo.Name = pbToolbarButtonInfo.FullName = pbToolbarButtonInfo.Description = pbToolbarButtonInfo.TooltipName = leftButtonNames[i];
                pbToolbarButtonInfo.IsDockLeft = true;
                pbToolbarButtonInfo.IsShowDisplayName = true;
                pbToolbarButtonInfo.Visible = true;
                PbToolbarButtonGroupInfo pbToolbarButtonGroupInfo = new PbToolbarButtonGroupInfo();
                pbToolbarButtonGroupInfo.ToolbarButtonInfos.Add(pbToolbarButtonInfo);
                billInfo.ListToolbarInfo.ToolbarButtonGroupInfosInfos.Add(pbToolbarButtonGroupInfo);
            }

            for (int i = 0; i < rightButtonNames.Length; i++)
            {
                PbToolbarButtonInfo pbToolbarButtonInfo = new PbToolbarButtonInfo();

                PbBuildInImp pbBuildInImp = new PbBuildInImp();
                pbBuildInImp.EventImpType = rightPbEventImpTypes[i];

                pbToolbarButtonInfo.ClickEvent.PbImp.Add(pbBuildInImp);
                pbToolbarButtonInfo.EventImpType = rightPbEventImpTypes[i];
                pbToolbarButtonInfo.Name = pbToolbarButtonInfo.FullName = pbToolbarButtonInfo.Description = pbToolbarButtonInfo.TooltipName = rightButtonNames[i];
                pbToolbarButtonInfo.IsDockLeft = false;
                pbToolbarButtonInfo.IsShowDisplayName = false;
                pbToolbarButtonInfo.Visible = true;
                PbToolbarButtonGroupInfo pbToolbarButtonGroupInfo = new PbToolbarButtonGroupInfo();
                pbToolbarButtonGroupInfo.ToolbarButtonInfos.Add(pbToolbarButtonInfo);
                billInfo.ListToolbarInfo.ToolbarButtonGroupInfosInfos.Add(pbToolbarButtonGroupInfo);
            }

        }

        /// <summary>
        /// 解析明细工具栏 
        /// </summary>
        /// <param name="billInfo"></param>
        /// <param name="fileName"></param>
        private void ParseDetailToolbar(PbBillInfo billInfo, string fileName)
        {
            //解析明细的工具栏
            string leftBarStr = IniHelp.ReadValue(DwRes.EditToolBarSection, DwRes.BarLeftSection, fileName);
            string rightBarStr = IniHelp.ReadValue(DwRes.EditToolBarSection, DwRes.BarRightSection, fileName);
            string[] leftBarStrArray = leftBarStr.Split(',');
            string[] rightBarStrArray = rightBarStr.Split(',');

            billInfo.DetailToolbarInfo.Name = "Toolbar";
            billInfo.DetailToolbarInfo.Description = "工具栏";
            billInfo.DetailToolbarInfo.FullName = "Toolbar";
            billInfo.DetailToolbarInfo.Visible = true;

            PbToolbarButtonGroupInfo groupInfo = null;
            foreach (string buttonStr in leftBarStrArray)
            {
                groupInfo = ConvertToolbarButtonStr(buttonStr, true);
                billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);

                if (buttonStr == "cb_check")
                {
                    groupInfo = ConvertToolbarButtonStr("cb_uncheck", true);
                    billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);
                }

                if (buttonStr == "cb_audit")
                {
                    groupInfo = ConvertToolbarButtonStr("cb_trace", true);
                    billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);
                }
            }

            //任务分解四个按钮：下达，提交、汇总和原汇总信息
            if (billInfo.IsTask == "1")
            {
                groupInfo = ConvertToolbarButtonStr("cb_subbill", true);
                billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);

                groupInfo = ConvertToolbarButtonStr("cb_ok", true);
                billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);

                groupInfo = ConvertToolbarButtonStr("cb_query", true);
                billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);

                groupInfo = ConvertToolbarButtonStr("cb_deal", true);
                billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);
            }

            foreach (string buttonStr in rightBarStrArray)
            {
                groupInfo = ConvertToolbarButtonStr(buttonStr, false);
                billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos.Add(groupInfo);
            }
        }

        private void ParseToolbar(PbBillInfo billInfo, string fileName)
        {
            try
            {
                //解析列表的工具栏,固定
                ParseListToolbar(billInfo, fileName);

                //解析明细表工具栏，非固定
                ParseDetailToolbar(billInfo, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ParseOther(PbBillInfo billInfo, string fileName)
        {
            try
            {
                //var info = (Dictionary<string, string>) System.Web.HttpContext.Current.Session["PFORMINFO"];

                Debug.Assert(billInfo != null);
                billInfo.Description = IniHelp.ReadValue("pformInfo", "description", fileName);  //description;// info["description"];
                billInfo.Code = IniHelp.ReadValue("pformInfo", "fileid", fileName);  //info["fileid"];
                billInfo.Name = "pform" + billInfo.Code;

                string widthStr = IniHelp.ReadValue(DwRes.WindowSection, DwRes.WidthSection, fileName);
                string heightStr = IniHelp.ReadValue(DwRes.WindowSection, DwRes.HeightSection, fileName);

                billInfo.Width = Convert.ToInt32(widthStr);
                billInfo.Height = Convert.ToInt32(heightStr);
                

                billInfo.IsTask = IniHelp.ReadValue(DwRes.WindowSection, "istask", fileName);
                billInfo.Reltable = IniHelp.ReadValue(DwRes.WindowSection, "reltable", fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ParseOtherApp(PbBillInfo billInfo, string fileName)
        {
            try
            {
                //var info = (Dictionary<string, string>) System.Web.HttpContext.Current.Session["PFORMINFO"];

                Debug.Assert(billInfo != null);
                billInfo.Description = IniHelp.ReadValue("pformInfo", "description", fileName); //description;// info["description"];
                billInfo.Name = "aform" + IniHelp.ReadValue("pformInfo", "fileid", fileName);// info["fileid"];

                string widthStr = IniHelp.ReadValue(DwRes.WindowSection, DwRes.WidthSection, fileName);
                string heightStr = IniHelp.ReadValue(DwRes.WindowSection, DwRes.HeightSection, fileName);

                billInfo.Width = Convert.ToInt32(widthStr);
                billInfo.Height = Convert.ToInt32(heightStr);
                billInfo.Code = IniHelp.ReadValue("pformInfo", "fileid", fileName);// info["fileid"];

            }
            catch (Exception)
            {

                throw;
            }
        }

        private IDictionary<string, string> ReadDddwSourceDictionary(string fileName)
        {
            try
            {
                IDictionary<string, string> strDictionary = new Dictionary<string, string>();
                string[] strArray = File.ReadAllLines(fileName, Encoding.Default);
                bool bFind = false;
                foreach (string str in strArray)
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        continue;
                    }

                    if (str.Equals("[dddw_source]", StringComparison.OrdinalIgnoreCase))
                    {
                        bFind = true;
                        continue;
                    }

                    if (bFind)
                    {
                        if (str.StartsWith("["))
                            break;
                        else
                        {
                            string[] childStrArray = str.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                            if (childStrArray.Length < 2)
                            {
                                strDictionary.Add(childStrArray[0], "");
                            }
                            else
                            {
                                //部分已删除的帮助列值为空不用添加
                                if (!string.IsNullOrEmpty(childStrArray[1]))
                                {
                                    strDictionary.Add(childStrArray[0], childStrArray[1]);
                                }
                            }
                        }
                    }
                }

                return strDictionary;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 解析列表
        /// </summary>
        /// <param name="pbBillInfo"></param>
        /// <param name="dic"></param>
        /// <param name="fileName"></param>
        private void ParseBillList(PbBillInfo pbBillInfo, IDictionary<string, string> dic, string fileName)
        {
            try
            {
                string syntaxString = IniHelp.ReadValue(DwRes.ListDatawindowName, DwRes.SyntaxSection, fileName);
                string TitleName = IniHelp.ReadValue(DwRes.WindowSection, "title", fileName);
                DwInfo dwInfo = DataWindowParseHelp.ParseDataWindow(syntaxString);

                PbGridInfo pbList = pbBillInfo.PbList;

                pbList.FullName = DwRes.ListDatawindowName;
                pbList.IsInTab = false;
                pbList.Name = DwRes.ListDatawindowName;
                pbList.Sql = dwInfo.Sql;
                pbList.TableName = "p_form" + pbBillInfo.Code + "_m";

                //--得到用复选帮助的字段名
                string multiselect = IniHelp.ReadValue(DwRes.MulSelectSection, DwRes.Cols, fileName);
                if (!string.IsNullOrEmpty(multiselect) && multiselect != "0")
                {
                    string[] multiselectArr = multiselect.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < multiselectArr.Count(); i++)
                    {
                        //p_form0000000005_d1.userhelpmul_1,p_form0000000005_m.userhelpmul_1
                        pbBillInfo.HeadInfo.MultiSelectDic.Add(multiselectArr[i], multiselectArr[i]);
                    }
                }
                //--end

                pbList.PbBaseTextInfos = DataWindowParseHelp.ToPbColumnsMetadata(dwInfo, pbList.TableName, dic, TitleName, pbBillInfo.HeadInfo.MultiSelectDic);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解析表头
        /// </summary>
        /// <param name="pbBillInfo"></param>
        /// <param name="dic"></param>
        /// <param name="fileName"></param>
        private void ParseBillHead(PbBillInfo pbBillInfo, IDictionary<string, string> dic, string fileName)
        {
            try
            {
                //--得到字段是否必输项值
                string colname = IniHelp.ReadValue(DwRes.HeadDwName, "cols", fileName);
                string colmustinput = IniHelp.ReadValue(DwRes.HeadDwName, "colreqs", fileName);
                Dictionary<string, string> coldic = new Dictionary<string, string>();

                string[] colnamearr = colname.Split(new string[] { "," }, StringSplitOptions.None);
                string[] colmustinputarr = colmustinput.Split(new string[] { "," }, StringSplitOptions.None);

                for (int i = 0; i < colnamearr.Count(); i++)
                {
                    coldic.Add(colnamearr[i], colmustinputarr[i]);
                }
                //--end


                //--判断是否绝对布局 
                string isAbslayout = IniHelp.ReadValue(DwRes.WindowSection, DwRes.Abslayout, fileName);
                //--end

                //--得到文档模板可选项
                string otid = IniHelp.ReadValue(DwRes.WindowSection, DwRes.Otid, fileName);
                //--end


                PbHeadInfo headInfo = pbBillInfo.HeadInfo;
                Debug.Assert(headInfo != null);

                //--得到字段和标签关系
                string colref = IniHelp.ReadValue(DwRes.WindowSection, DwRes.Ref, fileName);
                if (!string.IsNullOrEmpty(colref) && colref != "0")
                {
                    string[] colrefArr = colref.Split(new string[] { ";" }, StringSplitOptions.None);

                    for (int i = 0; i < colrefArr.Count(); i++)
                    {
                        string[] col = colrefArr[i].Split(new string[] { "@" }, StringSplitOptions.None);
                        headInfo.Colrefdic.Add(col[0], col[1]);
                    }
                }
                //--end

                //--得到类型textarea的字段名
                string textarea = IniHelp.ReadValue(DwRes.HeadDwName, DwRes.TextArea, fileName);
                if (!string.IsNullOrEmpty(textarea) && textarea != "0")
                {
                    string[] textareaArr = textarea.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < textareaArr.Count(); i++)
                    {
                        headInfo.TextAreaDic.Add(textareaArr[i], textareaArr[i]);
                    }
                }
                //--end

                //--得到所有列的列占位数
                string colspan = IniHelp.ReadValue(DwRes.HeadDwName, DwRes.ColSpan, fileName);
                if (!string.IsNullOrEmpty(colspan) && colspan != "0")
                {
                    string[] colspanArr = colspan.Split(new string[] { ";" }, StringSplitOptions.None);

                    for (int i = 0; i < colspanArr.Count(); i++)
                    {
                        string[] col = colspanArr[i].Split(new string[] { "@" }, StringSplitOptions.None);
                        headInfo.ColSpanDic.Add(col[0], col[1]);
                    }
                }
                //--end

                //--得到fieldset是否能折叠和默认是否折叠，0、null或空：不能折叠，默认展开，1:能折叠，默认展开，2:能折叠，默认折叠
                string collapse = IniHelp.ReadValue(DwRes.HeadDwName, DwRes.Collapse, fileName);
                if (!string.IsNullOrEmpty(collapse) && collapse != "0")
                {
                    string[] collapseArr = collapse.Split(new string[] { "@" }, StringSplitOptions.None);

                    for (int i = 0; i < collapseArr.Count(); i++)
                    {
                        string[] col = collapseArr[i].Split(new string[] { "," }, StringSplitOptions.None);
                        headInfo.CollapseDic.Add(col[0], col[1]);
                    }
                }
                //--end


                string syntaxString = IniHelp.ReadValue(DwRes.HeadDwName, DwRes.SyntaxSection, fileName);
                string TitleName = IniHelp.ReadValue(DwRes.WindowSection, "title", fileName);
                int columnsPerRow = Convert.ToInt32(IniHelp.ReadValue(DwRes.WindowSection, "columnsperrow", fileName));
                int formLabelWidth = Convert.ToInt32(IniHelp.ReadValue(DwRes.WindowSection, "labelwidth", fileName));

                DwInfo dwInfo = DataWindowParseHelp.ParseDataWindow(syntaxString, coldic);
                headInfo.Description = "测试表头";
                headInfo.FullName = DwRes.HeadDwName;
                headInfo.Name = DwRes.HeadDwName;
                headInfo.Sql = dwInfo.Sql.Replace("p_form", "p_form" + pbBillInfo.Code + "_m");
                headInfo.TableName = IniHelp.ReadValue(DwRes.HeadDwName, DwRes.TableSection, fileName);

                foreach (DwColumn dc in dwInfo.DwColumns)
                {
                    string DefaultValue = IniHelp.ReadValue(headInfo.TableName + "." + dc.Name, "def_data", fileName);
                    if (DefaultValue != "0" && !string.IsNullOrEmpty(DefaultValue))
                    {
                        dc.DefaultValue = DefaultValue;
                    }
                }

                headInfo.PbColumns = DataWindowParseHelp.ToPbColumnsMetadata(dwInfo, headInfo.TableName, dic, TitleName, headInfo.Colrefdic, headInfo.TextAreaDic,
                                                                             headInfo.ColSpanDic, headInfo.MultiSelectDic);

                headInfo.Abslayout = isAbslayout == "1" ? true : false;
                headInfo.Otid = otid == "0" ? "" : otid; ;
                headInfo.ColumnsPerRow = columnsPerRow == 0 ? 4 : columnsPerRow;
                headInfo.FormLabelWidth = formLabelWidth == 0 ? 80 : formLabelWidth;

                IList<PbGroupboxInfo> pbGroupboxInfos = DataWindowParseHelp.ToPbGroupBoxMetadata(dwInfo, DwRes.HeadDwAuthName, headInfo.CollapseDic);
                foreach (PbGroupboxInfo groupboxInfo in pbGroupboxInfos)
                {
                    headInfo.PbBaseControlInfos.Add(groupboxInfo);
                }

                //head上的日期列掩码赋值到List日期列
                foreach (PbBaseTextInfo pc in headInfo.PbColumns)
                {
                    if (pc.ControlType == PbControlType.DateTimeText)
                    {
                        foreach (var item in pbBillInfo.PbList.PbBaseTextInfos)
                        {
                            if (pc.Name == item.Name)
                            {
                                item.EditMask = pc.EditMask;
                            }

                        }
                    }
                }

                //图片控件
                headInfo.PbPictureboxInfos = DataWindowParseHelp.ToPbPictureBoxMetadata(dwInfo, headInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 解析表体
        /// </summary>
        /// <param name="pbBillInfo"></param>
        /// <param name="dic"></param>
        /// <param name="fileName"></param>
        private void ParseBillBody(PbBillInfo pbBillInfo, IDictionary<string, string> dic, string fileName)
        {
            try
            {
                //考虑到有可能有tab加上多个在tab外的grid
                //先读取[window]里的detail_table属性（包含所有的grid）,再读dyn_dw（这里是在tab外的grid）
                //把在tab里的grid和外的grid区分开来，分别读取属性再存入到tabinfo和gridinfo中              
                string detail_table = IniHelp.ReadValue(DwRes.WindowSection, "detail_table", fileName);
                string TitleName = IniHelp.ReadValue(DwRes.WindowSection, "title", fileName);                

                //如果没有detai_table属性，则认为只有单独的一个Grid
                if (detail_table != "0" && detail_table != null)
                {
                    string[] gridstr = detail_table.Split(','); //获取所有grid，可能没得到dw2
                    List<string> allgrid = gridstr.ToList();
                    List<string> dyngrid = new List<string>();

                    //获取所有tab外的grid，可能没得到dw2
                    for (int i = 0; i < allgrid.Count; i++)
                    {
                        int number = i + 1;
                        string readdyn = IniHelp.ReadValue(DwRes.WindowSection, "dyn_dw" + number.ToString(), fileName);
                        if (readdyn.Equals("0") || string.IsNullOrEmpty(readdyn))
                            break;
                        else
                        {
                            dyngrid.Add(readdyn.Split(';')[1].ToString());
                        }
                    }

                    //设计器先拖出grid再拖出tab的情况下，dw_2就不会显示在detail_table=和dyn_dw，所以要补进来；先拖tab再拖grid就没事
                    string isvisible = IniHelp.ReadValue(DwRes.BodyDwName, "visible", fileName);
                    if (isvisible == "1")  //string.IsNullOrEmpty(isvisible) || 
                    {
                        //把可能没获取到的dw2补充到allgrid和dyngrid
                        string dw2grid = IniHelp.ReadValue(DwRes.BodyDwName, DwRes.TableSection, fileName);
                        if (!string.IsNullOrEmpty(dw2grid))
                        {
                            if (!allgrid.Contains(dw2grid))
                                allgrid.Add(dw2grid);

                            if (!dyngrid.Contains(dw2grid))
                                dyngrid.Add(dw2grid);
                        }
                    }


                    List<string> tabgrid = new List<string>();
                    List<string> singlegrid = new List<string>();

                    //判断哪些grid在tab中
                    if (dyngrid.Count() != 0)
                    {
                        foreach (string str in allgrid)
                        {
                            if (!dyngrid.Contains(str))  //非独立grid
                                tabgrid.Add(str);
                            else
                                singlegrid.Add(str);  //独立grid
                        }
                    }
                    else
                    {
                        tabgrid = allgrid;
                    }

                    for (int e = 0; e < singlegrid.Count(); e++)
                    {
                        string tempRes = "dw_" + singlegrid[e];
                        string syntaxString = IniHelp.ReadValue(tempRes, DwRes.SyntaxSection, fileName);
                        if (syntaxString.Equals("0") || string.IsNullOrEmpty(syntaxString))
                        {
                            string tablename = IniHelp.ReadValue(DwRes.BodyDwName, DwRes.TableSection, fileName);
                            if (!tablename.Equals(singlegrid[e]))
                                continue;
                            else
                            {
                                syntaxString = IniHelp.ReadValue(DwRes.BodyDwName, DwRes.SyntaxSection, fileName);
                                tempRes = DwRes.BodyDwName;
                            }
                        }

                        //--得到字段是否必输项值
                        string colname = IniHelp.ReadValue(tempRes, "cols", fileName);
                        string colmustinput = IniHelp.ReadValue(tempRes, "colreqs", fileName);
                        Dictionary<string, string> coldic = new Dictionary<string, string>();

                        string[] colnamearr = colname.Split(new string[] { "," }, StringSplitOptions.None);
                        string[] colmustinputarr = colmustinput.Split(new string[] { "," }, StringSplitOptions.None);

                        for (int i = 0; i < colnamearr.Count(); i++)
                        {
                            coldic.Add(colnamearr[i], colmustinputarr[i]);
                        }
                        //--end

                        DwInfo dwInfo = DataWindowParseHelp.ParseDataWindow(syntaxString, coldic);

                        PbGridInfo gridInfo = new PbGridInfo();
                        pbBillInfo.PbGrids.Add(gridInfo);

                        gridInfo.FullName = tempRes; //gridInfo.FullName = DwRes.ListDatawindowName;
                        gridInfo.IsInTab = false;
                        gridInfo.Name = tempRes; //gridInfo.Name = DwRes.ListDatawindowName;
                        gridInfo.Sql = dwInfo.Sql.Replace("p_form_d", singlegrid[e]);

                        gridInfo.TableName = singlegrid[e];
                        gridInfo.Collapse = IniHelp.ReadValue(tempRes, DwRes.GridCollapse, fileName);
                        gridInfo.Title = IniHelp.ReadValue(tempRes, DwRes.Title, fileName);

                        gridInfo.Visible =
                            Convert.ToBoolean(
                                Convert.ToInt32(IniHelp.ReadValue(tempRes, DwRes.VisibleSection, fileName)));
                        gridInfo.XPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.XPosSection, fileName)) * 1.3);
                        gridInfo.YPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.YPosSection, fileName)));
                        gridInfo.Width = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.WidthSection, fileName)) * 1.3);
                        gridInfo.Height = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.HeightSection, fileName)));

                        //获取明细dw其它配置项并设置到gridInfo
                        GetBodyDwConfig(ref gridInfo, fileName);
                        pbBillInfo.SumRowStyle += gridInfo.SumRowStyle;
                        pbBillInfo.NoSumRowStyle += gridInfo.NoSumRowStyle;
                        gridInfo.PbBaseTextInfos = DataWindowParseHelp.ToPbColumnsMetadata(dwInfo, gridInfo.TableName, dic, TitleName, pbBillInfo.HeadInfo.MultiSelectDic);
                    }

                    //目前一个窗口只支持一个tabpanel，里面含有多个tab页和grid
                    PbTabInfo tabinfo = new PbTabInfo();

                    for (int p = 0; p < tabgrid.Count(); p++)
                    {
                        //tabinfo部分值只需赋值一次
                        if (p == 0)
                        {
                            tabinfo.XPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue("tab_1", DwRes.XPosSection, fileName)) * 1.3);
                            tabinfo.YPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue("tab_1", DwRes.YPosSection, fileName)));
                            tabinfo.Width = Convert.ToInt32(int.Parse(IniHelp.ReadValue("tab_1", DwRes.WidthSection, fileName)) * 1.3);
                            tabinfo.Height = Convert.ToInt32(int.Parse(IniHelp.ReadValue("tab_1", DwRes.HeightSection, fileName)));
                        }
                        tabinfo.GridIds.Add(tabgrid[p]); //放到tabinfo里去

                        //如果是报表则获取报表信息
                        if (tabgrid[p].IndexOf("rwgrid") >= 0)
                        {
                            tabinfo.ReportViews.Add(IniHelp.ReadValue(tabgrid[p], "report_view", fileName));
                            tabinfo.ReportParas.Add(IniHelp.ReadValue(tabgrid[p], "report_para", fileName));
                            pbBillInfo.HasReport= "2";
                        }
                        else
                        {
                            tabinfo.ReportViews.Add("0");
                            tabinfo.ReportParas.Add("0");
                        }

                        int num = p + 1;
                        string tempRes = "dw_" + tabgrid[p];
                        string[] fullnameArr = IniHelp.ReadValue("tab_1", "tabpage_" + num, fileName).ToString().Split(';');
                        string fullname = fullnameArr.Length > 1 ? fullnameArr[1] : tempRes;  //tab页标签

                        tabinfo.TabNames.Add(fullname);

                        //有金格控件
                        if (tabgrid[p] == "blobdoc")
                        {
                            pbBillInfo.HasBlobdoc = "2";
                            pbBillInfo.BlobdocName = fullname;
                            continue;
                        }

                        //有进度控件
                        if (tabgrid[p] == "eppocx")
                        {
                            pbBillInfo.HasEppocx = "2";
                            continue;
                        }

                        string syntaxString = IniHelp.ReadValue(tempRes, DwRes.SyntaxSection, fileName);
                        if (syntaxString.Equals("0") || string.IsNullOrEmpty(syntaxString))  //grid串取不到用尝试用dw_2去取
                        {
                            string tablename = IniHelp.ReadValue(DwRes.BodyDwName, DwRes.TableSection, fileName);
                            if (!tablename.Equals(tabgrid[p]))
                                continue;
                            else
                            {
                                syntaxString = IniHelp.ReadValue(DwRes.BodyDwName, DwRes.SyntaxSection, fileName);
                                tempRes = DwRes.BodyDwName;
                            }
                        }

                        //--得到字段是否必输项值
                        string colname = IniHelp.ReadValue(tempRes, "cols", fileName);
                        string colmustinput = IniHelp.ReadValue(tempRes, "colreqs", fileName);
                        Dictionary<string, string> coldic = new Dictionary<string, string>();

                        string[] colnamearr = colname.Split(new string[] { "," }, StringSplitOptions.None);
                        string[] colmustinputarr = colmustinput.Split(new string[] { "," }, StringSplitOptions.None);

                        for (int i = 0; i < colnamearr.Count(); i++)
                        {
                            coldic.Add(colnamearr[i], colmustinputarr[i]);
                        }
                        //--end

                        DwInfo dwInfo = DataWindowParseHelp.ParseDataWindow(syntaxString, coldic);

                        PbGridInfo gridInfo = new PbGridInfo();
                        pbBillInfo.PbGrids.Add(gridInfo);
                        gridInfo.FullName = fullname;
                        gridInfo.IsInTab = true;
                        gridInfo.Name = tempRes;
                        gridInfo.Sql = dwInfo.Sql.Replace("p_form_d", tabgrid[p]);
                        gridInfo.TableName = tabgrid[p];
                        gridInfo.Collapse = IniHelp.ReadValue(tempRes, DwRes.GridCollapse, fileName);
                        gridInfo.Title = fullname;  //IniHelp.ReadValue(tempRes, DwRes.Title, fileName);
                        gridInfo.Visible =
                            Convert.ToBoolean(
                            Convert.ToInt32(IniHelp.ReadValue(tempRes, DwRes.VisibleSection, fileName)));
                        gridInfo.XPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.XPosSection, fileName)) * 1.3);
                        gridInfo.YPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.YPosSection, fileName)));
                        gridInfo.Width = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.WidthSection, fileName)) * 1.3);
                        gridInfo.Height = Convert.ToInt32(int.Parse(IniHelp.ReadValue(tempRes, DwRes.HeightSection, fileName)));

                        //获取明细dw其它配置项并设置到gridInfo
                        GetBodyDwConfig(ref gridInfo, fileName);
                        pbBillInfo.SumRowStyle += gridInfo.SumRowStyle;
                        pbBillInfo.NoSumRowStyle += gridInfo.NoSumRowStyle;
                        gridInfo.PbBaseTextInfos = DataWindowParseHelp.ToPbColumnsMetadata(dwInfo, gridInfo.TableName, dic, TitleName, pbBillInfo.HeadInfo.MultiSelectDic);
                    }

                    if (tabinfo.GridIds.Count > 0)
                    {
                        pbBillInfo.PbTabInfos.Add(tabinfo);
                    }
                }
                else
                {
                    string syntaxString = IniHelp.ReadValue(DwRes.BodyDwName, DwRes.SyntaxSection, fileName);
                    if (syntaxString.Equals("0") || string.IsNullOrEmpty(syntaxString))
                    {
                        return;
                    }

                    string isvisible = IniHelp.ReadValue(DwRes.BodyDwName, "visible", fileName);
                    if (!string.IsNullOrEmpty(isvisible) && isvisible == "0")
                    {
                        return;
                    }

                    //--得到字段是否必输项值
                    string colname = IniHelp.ReadValue(DwRes.BodyDwName, "cols", fileName);
                    string colmustinput = IniHelp.ReadValue(DwRes.BodyDwName, "colreqs", fileName);
                    Dictionary<string, string> coldic = new Dictionary<string, string>();

                    string[] colnamearr = colname.Split(new string[] { "," }, StringSplitOptions.None);
                    string[] colmustinputarr = colmustinput.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < colnamearr.Count(); i++)
                    {
                        coldic.Add(colnamearr[i], colmustinputarr[i]);
                    }
                    //--end

                    DwInfo dwInfo = DataWindowParseHelp.ParseDataWindow(syntaxString, coldic);

                    PbGridInfo gridInfo = new PbGridInfo();
                    pbBillInfo.PbGrids.Add(gridInfo);

                    gridInfo.FullName = DwRes.BodyDwName; //gridInfo.FullName = DwRes.ListDatawindowName;
                    gridInfo.IsInTab = false;
                    gridInfo.Name = DwRes.BodyDwName; //gridInfo.Name = DwRes.ListDatawindowName;
                    gridInfo.Sql = dwInfo.Sql.Replace("p_form_d", "p_form" + pbBillInfo.Code + "_d");

                    gridInfo.TableName = IniHelp.ReadValue(gridInfo.Name, DwRes.TableSection, fileName);
                    gridInfo.Collapse = IniHelp.ReadValue(gridInfo.Name, DwRes.GridCollapse, fileName);
                    gridInfo.Title = IniHelp.ReadValue(gridInfo.Name, DwRes.Title, fileName);

                    gridInfo.Visible =
                        Convert.ToBoolean(
                            Convert.ToInt32(IniHelp.ReadValue(gridInfo.Name, DwRes.VisibleSection, fileName)));
                    gridInfo.XPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(gridInfo.Name, DwRes.XPosSection, fileName)) * 1.3);
                    gridInfo.YPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(gridInfo.Name, DwRes.YPosSection, fileName)));
                    gridInfo.Width = Convert.ToInt32(int.Parse(IniHelp.ReadValue(gridInfo.Name, DwRes.WidthSection, fileName)) * 1.3);
                    gridInfo.Height = Convert.ToInt32(int.Parse(IniHelp.ReadValue(gridInfo.Name, DwRes.HeightSection, fileName)));

                    //获取明细dw其它配置项并设置到gridInfo
                    GetBodyDwConfig(ref gridInfo, fileName);
                    pbBillInfo.SumRowStyle += gridInfo.SumRowStyle;
                    pbBillInfo.NoSumRowStyle += gridInfo.NoSumRowStyle;
                    gridInfo.PbBaseTextInfos = DataWindowParseHelp.ToPbColumnsMetadata(dwInfo, gridInfo.TableName, dic, TitleName, pbBillInfo.HeadInfo.MultiSelectDic);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //pb计算表达式转换成ng3格式
        private string TransCalcExpression(string pbcalc)
        {
            string ngcalc = pbcalc;
            switch (pbcalc)
            {
                case "avg":
                    ngcalc = "average";
                    break;
                case "count":
                    ngcalc = "count";
                    break;
                case "max":
                    ngcalc = "max";
                    break;
                case "min":
                    ngcalc = "min";
                    break;
                case "sum":
                    ngcalc = "sum";
                    break;
            }

            return ngcalc;
        }

        //获取明细dw其它配置项
        private void GetBodyDwConfig(ref PbGridInfo gridInfo, string fileName)
        {
            string dwname = gridInfo.Name;

            Dictionary<string, string> sumdic = new Dictionary<string, string>();  //合计列
            Dictionary<string, string> hbxsdic = new Dictionary<string, string>();  //合并重复行
            Dictionary<string, string> subdic = new Dictionary<string, string>();  //分组小计的小计列
            Dictionary<string, string> groupfield = new Dictionary<string, string>();  //分组小计的分组列         
            Dictionary<string, string> groupcolsdic = new Dictionary<string, string>();  //多表头标签
            List<string> levelsum = new List<string>();  //级次汇总

            try
            {
                //合计，如 summary=qty@sum,amt@sum
                string sumStr = IniHelp.ReadValue(dwname, "summary", fileName);
                if (!string.IsNullOrEmpty(sumStr) && sumStr != "0")
                {
                    string[] sumArr = sumStr.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < sumArr.Count(); i++)
                    {
                        string[] col = sumArr[i].Split(new string[] { "@" }, StringSplitOptions.None);
                        if (col.Length > 1)
                        {                            
                            sumdic.Add(col[0], TransCalcExpression(col[1]));
                        }
                    }
                }

                //分组小计，如 group1=deptid,remarks;deptid A,remarks A; prc@sum,amt@sum
                string subStr = IniHelp.ReadValue(dwname, "group1", fileName);
                if (!string.IsNullOrEmpty(subStr) && subStr != "0")
                {
                    string[] subArr = subStr.Split(new string[] { ";" }, StringSplitOptions.None);

                    if (subArr.Count() == 3)
                    {
                        string[] colgroupfield = subArr[0].Split(new string[] { "," }, StringSplitOptions.None);
                        for (int i = 0; i < colgroupfield.Count(); i++)
                        {
                            groupfield.Add(colgroupfield[i], colgroupfield[i]);
                        }

                        string[] colsub = subArr[2].Split(new string[] { "," }, StringSplitOptions.None);
                        for (int i = 0; i < colsub.Count(); i++)
                        {
                            string[] col = colsub[i].Split(new string[] { "@" }, StringSplitOptions.None);
                            if (col.Length > 1)
                            {
                                subdic.Add(col[0], TransCalcExpression(col[1]));
                            }
                        }
                    }
                }

                //合并重复行
                string hbxsStr = IniHelp.ReadValue(dwname, "hbxs", fileName);
                if (!string.IsNullOrEmpty(hbxsStr) && hbxsStr != "0")
                {
                    string[] hbxsArr = hbxsStr.Split(new string[] { ";" }, StringSplitOptions.None);

                    for (int i = 0; i < hbxsArr.Count(); i++)
                    {
                        if (!string.IsNullOrWhiteSpace(hbxsArr[i]))
                        {
                            hbxsdic.Add(hbxsArr[i], hbxsArr[i]);
                        }
                    }
                }

                //多表头标签
                string firstStr = string.Empty;  //多表头首列
                string lastStr = string.Empty;  //多表头尾列
                string groupcolsStr = IniHelp.ReadValue(dwname, "groupcols", fileName);  //groupcols = 表头1@a,b,c;表头2 @e, f, g
                groupcolsStr = groupcolsStr.Replace("res_code", "itemid");  //物料编码列字段名修改为itemid
                groupcolsStr = groupcolsStr.Replace("c_name", "itemid");    //物料名称列字段名修改为itemid
                if (!string.IsNullOrEmpty(groupcolsStr) && groupcolsStr != "0")
                {
                    string[] groupcolsArr = groupcolsStr.Split(new string[] { ";" }, StringSplitOptions.None);

                    for (int i = 0; i < groupcolsArr.Count(); i++)
                    {
                        if (!string.IsNullOrWhiteSpace(groupcolsArr[i]))
                        {
                            string[] groupArr = groupcolsArr[i].Split(new string[] { "@" }, StringSplitOptions.None);
                            string[] colsArr = groupArr[1].Split(new string[] { "," }, StringSplitOptions.None);

                            for (int j = 0; j < colsArr.Count(); j++)
                            {
                                if (!groupcolsdic.ContainsKey(colsArr[j]))
                                {
                                    groupcolsdic.Add(colsArr[j], groupArr[0]);
                                }
                            }

                            firstStr += colsArr[0] + ",";
                            lastStr += colsArr[colsArr.Length - 1] + ",";
                        }
                    }

                    groupcolsdic.Add("@@first", firstStr.TrimEnd(','));
                    groupcolsdic.Add("@@last", lastStr.TrimEnd(','));
                }

                //级次汇总
                string levelsumStr = IniHelp.ReadValue(dwname, "levelsum", fileName);
                if (!string.IsNullOrEmpty(levelsumStr) && levelsumStr != "0")
                {
                    levelsum = levelsumStr.Split(new string[] { "," }, StringSplitOptions.None).ToList<string>();
                }

                //汇总行和非汇总行背景颜色和字体
                string levelcolor = IniHelp.ReadValue(dwname, "levelcolor", fileName);
                string sumTextColor = string.Empty;
                string sumBgColor = string.Empty;
                string noSumTextColor = string.Empty;
                string noSumBgColor = string.Empty;
                if (!string.IsNullOrEmpty(levelcolor))
                {
                    string[] color = levelcolor.Split(new string[] { ";" }, StringSplitOptions.None);
                    if (color.Length > 1)
                    {
                        if (color[0].IndexOf('@') != -1)
                        {
                            sumBgColor =GetRgb("bg",Convert.ToInt64(color[0].Split('@')[0]));
                            sumTextColor = GetRgb("text", Convert.ToInt64(color[0].Split('@')[1]));
                        }
                        if (color[1].IndexOf('@') != -1)
                        {
                            noSumBgColor = GetRgb("bg", Convert.ToInt64(color[1].Split('@')[0]));
                            noSumTextColor = GetRgb("text", Convert.ToInt64(color[1].Split('@')[1]));
                        }
                    }
                }
                gridInfo.SumRowStyle = sumBgColor + sumTextColor;
                gridInfo.NoSumRowStyle = noSumBgColor + noSumTextColor;
                //----根据获取的值设置gridInfo
                gridInfo.Sumdic = sumdic;
                gridInfo.Subdic = subdic;
                gridInfo.Groupfield = groupfield;
                gridInfo.Groupcolsdic = groupcolsdic;

                if (levelsum.Count > 0)
                {
                    foreach (var item in levelsum)
                    {
                        gridInfo.LevelSum += "'" + item.ToString() + "',";
                    }

                    gridInfo.LevelSum = gridInfo.LevelSum.TrimEnd(',');
                }

                //这个值存到p_form_m，方便任务分解来取值
                if (!string.IsNullOrEmpty(subStr) && subStr != "0")
                {
                    gridInfo.Subtotal = subStr;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 解析金格控件、进度控件、附件单据体和审批单据体
        /// </summary>
        /// <param name="pbBillInfo"></param>
        /// <param name="dic"></param>
        /// <param name="fileName"></param>
        private void ParseOtherControl(PbBillInfo pbBillInfo, IDictionary<string, string> dic, string fileName)
        {
            try
            {
                //金格控件
                PbOfficeInfo officeInfo = pbBillInfo.OfficeInfo;
                officeInfo.Visible = Convert.ToBoolean(Convert.ToInt32(IniHelp.ReadValue("blobdoc", "visible", fileName)));
                officeInfo.XPos = Convert.ToInt32(IniHelp.ReadValue("blobdoc", "x", fileName));
                officeInfo.YPos = Convert.ToInt32(IniHelp.ReadValue("blobdoc", "y", fileName));
                officeInfo.Width = Convert.ToInt32(IniHelp.ReadValue("blobdoc", "width", fileName));
                officeInfo.Height = Convert.ToInt32(IniHelp.ReadValue("blobdoc", "height", fileName));

                //控件可能存在于tab，则这边会取不到控件内容，这种情况控件是否存在是在ParseBillBody函数中判断
                if (officeInfo.Visible == true)
                {
                    pbBillInfo.HasBlobdoc = "1";  //金格在tab外面
                }
                else
                {
                    if (pbBillInfo.HasBlobdoc == "2")  //金格在tab中
                    {
                        officeInfo.Visible = true;
                    }
                }

                //进度控件
                PbScheduleInfo scheduleInfo = pbBillInfo.ScheduleInfo;
                scheduleInfo.Visible = Convert.ToBoolean(Convert.ToInt32(IniHelp.ReadValue("eppocx", "visible", fileName)));
                scheduleInfo.XPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue("eppocx", "x", fileName)) * 1.3);
                scheduleInfo.YPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue("eppocx", "y", fileName)) * 1.3);
                scheduleInfo.Width = Convert.ToInt32(int.Parse(IniHelp.ReadValue("eppocx", "width", fileName)) * 1.3);
                scheduleInfo.Height = Convert.ToInt32(int.Parse(IniHelp.ReadValue("eppocx", "height", fileName)) * 1.3);

                //控件可能存在于tab，则这边会取不到控件内容，这种情况控件是否存在是在ParseBillBody函数中判断
                if (scheduleInfo.Visible == true)
                {
                    pbBillInfo.HasEppocx = "1";  //金格在tab外面
                }
                else
                {
                    if (pbBillInfo.HasEppocx == "2")  //金格在tab中
                    {
                        scheduleInfo.Visible = true;
                    }
                }


                //附件单据体
                PbGridInfo asrGridInfo = pbBillInfo.AsrGridInfo;
                asrGridInfo.Visible = Convert.ToBoolean(Convert.ToInt32(IniHelp.ReadValue(DwRes.AsrGrid, DwRes.VisibleSection, fileName)));
                asrGridInfo.XPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.AsrGrid, DwRes.XPosSection, fileName)) * 1.3);
                asrGridInfo.YPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.AsrGrid, DwRes.YPosSection, fileName)));
                asrGridInfo.Width = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.AsrGrid, DwRes.WidthSection, fileName)) * 1.3);
                asrGridInfo.Height = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.AsrGrid, DwRes.HeightSection, fileName)));

                string asrGridPosition = IniHelp.ReadValue(DwRes.AsrGrid, DwRes.Position, fileName);
                if (asrGridPosition != "0")
                {
                    string[] asrGridColumns = { "asr_name", "logid", "asr_dt" };
                    List<PbTextInfo> asrGridColumnList = new List<PbTextInfo>();
                    for (int i = 0; i < asrGridColumns.Length; i++)
                    {
                        PbTextInfo asrGridColumn = new PbTextInfo();
                        asrGridColumn.Name = asrGridColumns[i];
                        int index1 = asrGridPosition.IndexOf(asrGridColumns[i] + ".x = ") + asrGridColumns[i].Length + 5;
                        int index2 = asrGridPosition.Substring(index1).IndexOf(" ");
                        asrGridColumn.XPos = Convert.ToInt32(int.Parse(asrGridPosition.Substring(index1, index2)));
                        index1 = asrGridPosition.IndexOf(asrGridColumns[i] + ".width = ") + asrGridColumns[i].Length + 9;
                        index2 = asrGridPosition.Substring(index1).IndexOf(" ");
                        asrGridColumn.Width = Convert.ToInt32(int.Parse(asrGridPosition.Substring(index1, index2)) * 1.3);
                        asrGridColumnList.Add(asrGridColumn);
                    }
                    asrGridColumnList.Sort((asrGridColumn1, asrGridColumn2) => asrGridColumn1.XPos - asrGridColumn2.XPos);

                    foreach (var asrGridColumn in asrGridColumnList)
                    {
                        asrGridInfo.PbBaseTextInfos.Add(asrGridColumn);
                    }
                }

                //审批单据体
                PbGridInfo wfGridInfo = pbBillInfo.WfGridInfo;
                wfGridInfo.Visible = Convert.ToBoolean(Convert.ToInt32(IniHelp.ReadValue(DwRes.WfGrid, DwRes.VisibleSection, fileName)));
                wfGridInfo.XPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.WfGrid, DwRes.XPosSection, fileName)) * 1.3);
                wfGridInfo.YPos = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.WfGrid, DwRes.YPosSection, fileName)));
                wfGridInfo.Width = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.WfGrid, DwRes.WidthSection, fileName)) * 1.3);
                wfGridInfo.Height = Convert.ToInt32(int.Parse(IniHelp.ReadValue(DwRes.WfGrid, DwRes.HeightSection, fileName)));
                
                if (IniHelp.ReadValue(DwRes.WfGrid, DwRes.Position, fileName) != "0")
                {
                    wfGridInfo.PbBaseTextInfos.Add(new PbTextInfo());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取值更新事件列表
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="strArray"></param>
        /// <returns></returns>
        private List<string> GetUpdateEventList(string fullName, string[] strArray)
        {
            List<string> strList = new List<string>();
            string str = string.Empty;
            bool bfind = false;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Trim().Equals("[" + fullName + "]", StringComparison.OrdinalIgnoreCase))
                {
                    bfind = true;
                    continue;
                }

                if (bfind)
                {
                    str += strArray[i];

                    //是最后一行
                    if (i >= strArray.Length - 1)
                    {
                        if (str.Contains(";"))
                            strList.Add(str);
                        break;
                    }

                    //下一行是其他字段区域
                    if (strArray[i + 1].Contains("["))
                    {
                        if (str.Contains(";"))
                            strList.Add(str);
                        break;
                    }

                    //下一行还有一个规则
                    if (strArray[i + 1].Contains("rule"))
                    {
                        if (str.Contains(";"))
                            strList.Add(str);
                        str = string.Empty;
                        continue;
                    }
                }
            }
            return strList;
        }

        private IDictionary<string, string> GetClickEventList(string section, string[] strArray)
        {
            bool bFind = false;
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Trim().Equals(section, StringComparison.OrdinalIgnoreCase))
                {
                    bFind = true;
                    continue;
                }

                if (bFind)
                {
                    if (strArray[i].Trim().StartsWith("[", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    if (strArray[i].Contains("=") && strArray[i].Contains(";"))
                    {
                        string[] childArray = strArray[i].Split('=');
                        Debug.Assert(childArray.Length == 2);
                        dictionary.Add(childArray[0], childArray[1]);
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 设置列的可见、必输、（保护表达式在列属性中，不在这处理）等属性表达式
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="strArray"></param>
        /// <returns></returns>
        private void SetPropertyExpList(PbBaseTextInfo obj, string dwname, string fileName)
        {
            PbExpressionImp pbExpressionImp;

            //获取可见表达式
            string expressStr = IniHelp.ReadValue(dwname, obj.Name + ".visible", fileName);
            if (expressStr != "0")
            {
                pbExpressionImp = new PbExpressionImp();
                pbExpressionImp.Expression = obj.Name + ".visible=" + expressStr.Trim();
                pbExpressionImp.ExpressionType = (PbExpressionType)15;
                obj.VisibleExpressionImp = pbExpressionImp;
            }

            //获取必输表达式
            expressStr = IniHelp.ReadValue(dwname, obj.Name + ".required", fileName);
            if (expressStr != "0")
            {
                pbExpressionImp = new PbExpressionImp();
                pbExpressionImp.Expression = obj.Name + ".required=" + expressStr.Trim();
                pbExpressionImp.ExpressionType = (PbExpressionType)16;
                obj.IsMustInputExpressionImp = pbExpressionImp;
            }
        }

        /// <summary>
        /// 解析子控件的事件
        /// </summary>
        /// <param name="pbBillInfo"></param>
        /// <param name="fileName"></param>
        private void ParseControlEvent(PbBillInfo pbBillInfo, string fileName)
        {
            string[] strArray = File.ReadAllLines(fileName, Encoding.Default);
            foreach (var obj in pbBillInfo.HeadInfo.PbColumns)
            {
                obj.UpdateEvent = ExpressionHelp.ParseExpressionList(GetUpdateEventList(obj.FullName, strArray));

                //设置列的可见、必输、（保护表达式在列属性中，不在这处理）等属性表达式
                SetPropertyExpList(obj, DwRes.HeadDwName, fileName);
            }

            if (pbBillInfo.PbGrids.Count > 0)
            {
                foreach (var gridPanel in pbBillInfo.PbGrids)
                {
                    foreach (var obj in gridPanel.PbBaseTextInfos)
                    {
                        obj.UpdateEvent = ExpressionHelp.ParseExpressionList(GetUpdateEventList(obj.FullName, strArray));

                        //设置列的可见、必输、（保护表达式在列属性中，不在这处理）等属性表达式
                        SetPropertyExpList(obj, gridPanel.Name, fileName);
                    }
                }
            }
        }

        /// <summary>
        /// 解析Click和DoubleClick事件
        /// </summary>
        /// <param name="pbBillInfo"></param>
        /// <param name="fileName"></param>
        private void ParseClickEvent(PbBillInfo pbBillInfo, string fileName)
        {
            string[] strArray = File.ReadAllLines(fileName, Encoding.Default);
            IDictionary<string, string> clickDictionary = GetClickEventList(DwRes.ClickEventSection, strArray);
            IDictionary<string, string> doubleClickDictionary = GetClickEventList(DwRes.DoubleClickEventSection, strArray);
            foreach (var obj in pbBillInfo.HeadInfo.PbColumns)
            {
                if (clickDictionary.ContainsKey(obj.FullName))
                {
                    obj.ClickEvent = ExpressionHelp.ParseBuildInImp(clickDictionary[obj.FullName]);
                }

                if (doubleClickDictionary.ContainsKey(obj.FullName))
                {
                    obj.DoubleClickEvent = ExpressionHelp.ParseBuildInImp(doubleClickDictionary[obj.FullName]);
                }
            }

            if (pbBillInfo.PbGrids.Count > 0)
            {
                foreach (var obj in pbBillInfo.PbGrids[0].PbBaseTextInfos)
                {
                    if (clickDictionary.ContainsKey(obj.FullName))
                    {
                        obj.ClickEvent = ExpressionHelp.ParseBuildInImp(clickDictionary[obj.FullName]);
                    }

                    if (doubleClickDictionary.ContainsKey(obj.FullName))
                    {
                        obj.DoubleClickEvent = ExpressionHelp.ParseBuildInImp(clickDictionary[obj.FullName]);
                    }
                }
            }
        }

        /// <summary>
        /// 解析全局事件
        /// </summary>
        /// <param name="pbBillInfo"></param>
        /// <param name="fileName"></param>
        private void ParseGlobalEvent(PbBillInfo pbBillInfo, string fileName)
        {
            string[] strArray = File.ReadAllLines(fileName, Encoding.Default);
            bool bFind = false;
            PbEvent<PbExpressionImp> eventImp = null;
            int step = 1;
            List<string> expressionList = null;
            List<string> editAddInitEvent = new List<string>();
            List<string> billDelCheckEvent = new List<string>();
            List<string> billDelUpdateEvent = new List<string>();
            List<string> billSaveUpdateEvent = new List<string>();
            List<string> billApprovalUpdateEvent = new List<string>();
            List<string> billUnApprovalUpdateEvent = new List<string>();
            List<string> billBeforeSaveEvent = new List<string>();

            for (int i = 0; i < strArray.Length; i += step)
            {
                step = 1;
                if (strArray[i].Equals(DwRes.WindowOpenSection, StringComparison.OrdinalIgnoreCase))
                {
                    expressionList = editAddInitEvent;
                    bFind = true;
                    continue;
                }
                else if (strArray[i].Equals(DwRes.WindowDeleteCheck, StringComparison.OrdinalIgnoreCase))
                {
                    expressionList = billDelCheckEvent;
                    bFind = true;
                    continue;
                }
                else if (strArray[i].Equals(DwRes.WindowDelete, StringComparison.OrdinalIgnoreCase))
                {
                    expressionList = billDelUpdateEvent;
                    bFind = true;
                    continue;
                }
                else if (strArray[i].Equals(DwRes.WindowSave, StringComparison.OrdinalIgnoreCase))
                {
                    expressionList = billSaveUpdateEvent;
                    bFind = true;
                    continue;
                }
                else if (strArray[i].Equals(DwRes.WindowCheck, StringComparison.OrdinalIgnoreCase))
                {
                    expressionList = billApprovalUpdateEvent;
                    bFind = true;
                    continue;
                }
                else if (strArray[i].Equals(DwRes.WindowUncheck, StringComparison.OrdinalIgnoreCase))
                {
                    expressionList = billUnApprovalUpdateEvent;
                    bFind = true;
                    continue;
                }
                else if (strArray[i].Equals(DwRes.WindowBeforeSave, StringComparison.OrdinalIgnoreCase))
                {
                    expressionList = billBeforeSaveEvent;
                    bFind = true;
                    continue;
                }

                if (bFind)
                {
                    string str = strArray[i];
                    while (true)
                    {
                        if ((i + step) > (strArray.Length - 1))
                        {
                            if ((str.Length - 1) != str.IndexOf("=", StringComparison.OrdinalIgnoreCase))
                                expressionList.Add(str);

                            break;
                        }

                        if (!strArray[i + step].Contains("=") && !strArray[i + step].Trim().StartsWith("["))
                        {
                            str += strArray[i + step];
                            step++;
                            bFind = false;
                            break;
                        }
                        else
                        {
                            if (strArray[i + step].Trim().StartsWith("["))
                                bFind = false;
                            step = 1;

                            //modify by ljy 2016.10.18 规则格式为“rule1=2;amt @ qty * prc”，必须有rule和;
                            if (str.IndexOf("rule", StringComparison.OrdinalIgnoreCase) >= 0 && str.IndexOf(";") >= 0)
                                expressionList.Add(str);

                            break;
                        }
                    }
                }

            }

            pbBillInfo.EditAddInitEvent = ExpressionHelp.ParseExpressionList(editAddInitEvent);
            pbBillInfo.BillDelCheckEvent = ExpressionHelp.ParseExpressionList(billDelCheckEvent);
            pbBillInfo.BillDelUpdateEvent = ExpressionHelp.ParseExpressionList(billDelUpdateEvent);
            pbBillInfo.BillSaveUpdateEvent = ExpressionHelp.ParseExpressionList(billSaveUpdateEvent);
            pbBillInfo.BillApprovalUpdateEvent = ExpressionHelp.ParseExpressionList(billApprovalUpdateEvent);
            pbBillInfo.BillApprovalUpdateEvent = ExpressionHelp.ParseExpressionList(billUnApprovalUpdateEvent);
            pbBillInfo.BillBeforeSaveEvent = ExpressionHelp.ParseExpressionList(billBeforeSaveEvent);
        }

        /// <summary>
        /// 解析表单的ini文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public PbBillInfo ParseBillFile(string fileName)
        {
            IDictionary<string, string> dic = ReadDddwSourceDictionary(fileName);
            PbBillInfo billInfo = new PbBillInfo();
            ParseOther(billInfo, fileName);
            ParseToolbar(billInfo, fileName);            
            ParseBillList(billInfo, dic, fileName);
            ParseBillHead(billInfo, dic, fileName);
            ParseBillBody(billInfo, dic, fileName);
            ParseOtherControl(billInfo, dic, fileName); //金格、进度控件、附件单据体、审批单据体

            ParseGlobalEvent(billInfo, fileName);
            ParseControlEvent(billInfo, fileName);
            ParseClickEvent(billInfo, fileName);

            return billInfo;
        }

        /// <summary>
        /// App方法:解析表单的ini文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public PbBillInfo ParseBillFileApp(string fileName)
        {
            IDictionary<string, string> dic = ReadDddwSourceDictionary(fileName);
            PbBillInfo billInfo = new PbBillInfo();
            ParseToolbar(billInfo, fileName);
            ParseOtherApp(billInfo, fileName);
            ParseBillList(billInfo, dic, fileName);
            ParseBillHead(billInfo, dic, fileName);
            ParseBillBody(billInfo, dic, fileName);
            ParseOtherControl(billInfo, dic, fileName); //金格、进度控件、附件单据体、审批单据体


            ParseGlobalEvent(billInfo, fileName);
            ParseControlEvent(billInfo, fileName);
            ParseClickEvent(billInfo, fileName);

            return billInfo;
        }

        /// <summary>
        /// 解析pb的color值，转换成rgb
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetRgb(string type, long color)
        {
            int red, green, blue = 0;
            string style = string.Empty;
            if (color > 0 && color != 33554432)
            {
                blue = Convert.ToInt32(color / (256 * 256));
                green = Convert.ToInt32((color - blue * 256 * 256) / 256);
                red = Convert.ToInt32(color - green * 256 - blue * 256 * 256);
                if (type == "text")//字体颜色
                    style = "color:rgb(" + red + "," + green + "," + blue + ") !important;";
                else if (type == "bg")//背景颜色
                    style = "background-color:rgb(" + red + "," + green + "," + blue + ") !important;";
            }
            return style;
        }
    }
}
