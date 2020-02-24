using System;
using System.Collections.Generic;

using NG3.Data.Service;
using NG3.Metadata.UI.PowserBuilder;
using SUP.CustomForm.ClientGen.Aform;
using SUP.CustomForm.DataEntity.Container;
using SUP.CustomForm.Rule;

namespace SUP.CustomForm.ClientGen
{
    public class Generator
    {
        /// <summary>
        /// ex:
        /// ParseManager parseManager = new ParseManager();
        /// </summary>
        /// <param name="billInfo"> </param>
        /// <returns></returns>
        public static bool Generate(PbBillInfo billInfo, ref string winType, string extJsStr, string ucode)
        {
            string className = ucode + billInfo.Name;        //NG0001pform0000000008
            string pform = billInfo.Name;                    //pform0000000008
            string eform = pform.Replace("pform", "EFORM");  //EFORM0000000008
            string qform = pform.Replace("pform", "w_eform_p_list");  //w_eform_p_list0000600008
            string title = billInfo.Description;
            string istask = billInfo.IsTask;
            string reltable = billInfo.Reltable;
            bool hasTab = billInfo.PbTabInfos.Count > 0 ? true : false;
            string tableNameMst = billInfo.HeadInfo.TableName;


            DbHelper.Open();

            //List窗口 gridPanel
            PbListFormParser PbListParser = new PbListFormParser();
            GridPanel gridPanel = PbListParser.GetGridInfo(billInfo);
            Toolbar toolbar = PbListParser.GetListToolbar(billInfo);

            //Edit窗口 form grid
            PbEditFormParser pbEdit = new PbEditFormParser(billInfo);
            List<String> PanelNames = new List<string>();

            //记录所有的panel
            foreach (var set in pbEdit.FieldSets)
            {
                foreach (var panel in set.Panels)
                {
                    PanelNames.Add(panel.TableName);
                }
            }

            foreach (var panel in pbEdit.GridPanels)
            {
                PanelNames.Add(panel.TableName);
            }

            ////任务分解需求，拼grid属性集合json串并保存到p_form_m表，如{"p_form0000000010_d":"deptid,remarks;deptid A,remarks A;prc@sum,amt@sum"}
            //if (billInfo.IsTask == "1" && pbEdit.AllGrids.Count > 0)
            //{
            //    string grid_detail = "{";

            //    foreach (GridPanel grid in pbEdit.AllGrids)
            //    {
            //        if (!string.IsNullOrEmpty(grid.Subtotal))
            //        {
            //            grid_detail += "\"" + grid.TableName + "\":\"" + grid.Subtotal + "\",";
            //        }               
            //    }

            //    grid_detail = grid_detail.TrimEnd(',') + "}";

            //    DbHelper.ExecuteNonQuery(string.Format("update p_form_m set grid_detail='{0}' where code='{1}'", grid_detail, billInfo.Code));
            //}
            

            string area = "SUP";

            #region list
            if (gridPanel.Columns.Count > 0)
            {
                winType = "List";

                CommonParser.Log("List界面开始生成.");
                //否则不生成
                PageListTemplate PageListTemplate = new PageListTemplate();
                PageListJsTemplate PageListJsTemplate = new PageListJsTemplate();
                PageListTemplate.NameSpacePrefix = area;
                PageListTemplate.NameSpaceSuffix = "CustomForm";
                PageListTemplate.ClassName = className;
                PageListTemplate.Title = title;
                PageListTemplate.WriteEx("SUP");

                PageListJsTemplate.NameSpacePrefix = area;
                PageListJsTemplate.NameSpaceSuffix = "CustomForm";
                PageListJsTemplate.ClassName = className;
                PageListJsTemplate.EForm = eform;
                PageListJsTemplate.PForm = pform;
                PageListJsTemplate.QForm = qform;
                PageListJsTemplate.IsTask = istask;
                PageListJsTemplate.PkPropertyname = "phid";
                PageListJsTemplate.Title = title;
                PageListJsTemplate.Area = area;
                PageListJsTemplate.gridPanel = gridPanel;
                PageListJsTemplate.Toolbar = toolbar;
                PageListJsTemplate.HasBlobdoc = billInfo.HasBlobdoc;
                PageListJsTemplate.HasEppocx = billInfo.HasEppocx;
                PageListJsTemplate.HasReport = billInfo.HasReport;
                PageListJsTemplate.WriteEx("SUP");

                CommonParser.Log("List界面生成成功.");
            }
            #endregion


            #region edit
            CommonParser.Log("Edit界面开始生成.");

            PageEditTemplate PageEditTemplate = new PageEditTemplate();
            PageJsExtTemplate PageJsExtTemplate = new PageJsExtTemplate();
            PageEditTemplate.NameSpacePrefix = area;
            PageEditTemplate.NameSpaceSuffix = "CustomForm";
            PageEditTemplate.ClassName = className;
            PageEditTemplate.Title = title;
            PageEditTemplate.SumRowStyle = billInfo.SumRowStyle;
            PageEditTemplate.NoSumRowStyle = billInfo.NoSumRowStyle;
            PageEditTemplate.WriteEx("SUP");

            //扩展脚本生成
            PageJsExtTemplate.ClassName = className;
            PageJsExtTemplate.ExtJsStr = extJsStr;
            PageJsExtTemplate.WriteEx("SUP");

            if (!hasTab)  //无tab页
            {
                PageEditJsTemplate PageEditJSTemplate = new PageEditJsTemplate();
                PageEditJSTemplate.NameSpacePrefix = area;
                PageEditJSTemplate.NameSpaceSuffix = "CustomForm";
                PageEditJSTemplate.ClassName = className;
                PageEditJSTemplate.PForm = pform;
                PageEditJSTemplate.EForm = eform;
                PageEditJSTemplate.QForm = qform;
                PageEditJSTemplate.IsTask = istask;
                PageEditJSTemplate.Reltable = reltable;
                PageEditJSTemplate.PkPropertyname = "phid";
                PageEditJSTemplate.Title = title;
                PageEditJSTemplate.Area = area;
                PageEditJSTemplate.fieldSets = pbEdit.FieldSets;
                PageEditJSTemplate.panels = pbEdit.GridPanels;
                PageEditJSTemplate.AllGrids = pbEdit.AllGrids;
                PageEditJSTemplate.tableLayouts = pbEdit.LayoutForm;
                PageEditJSTemplate.PanelNames = PanelNames;
                PageEditJSTemplate.Expressions = pbEdit.Expressions;
                PageEditJSTemplate.Toolbar = pbEdit.TB;
                PageEditJSTemplate.TableName = tableNameMst;
                PageEditJSTemplate.BodyCmpCount = billInfo.BodyCmpCount;
                PageEditJSTemplate.HasBlobdoc = billInfo.HasBlobdoc;
                PageEditJSTemplate.HasEppocx = billInfo.HasEppocx;
                PageEditJSTemplate.HasReport = billInfo.HasReport;
                PageEditJSTemplate.FieldSetBlobdoc = pbEdit.FieldSetBlobdoc;
                PageEditJSTemplate.PictureBoxs = pbEdit.PictureBoxs;
                PageEditJSTemplate.HasAsrGrid = billInfo.AsrGridInfo.Visible ? "1" : "0";
                PageEditJSTemplate.AsrGrid = pbEdit.AsrGrid;
                PageEditJSTemplate.HasWfGrid = billInfo.WfGridInfo.Visible ? "1" : "0";
                PageEditJSTemplate.WfGrid = pbEdit.WfGrid;
                PageEditJSTemplate.WriteEx("SUP");
            }
            else
            {
                //有tab页
                PageEditJsWithTabTemplate PageEditJsWithTabTemplate = new PageEditJsWithTabTemplate();
                PageEditJsWithTabTemplate.NameSpacePrefix = area;
                PageEditJsWithTabTemplate.NameSpaceSuffix = "CustomForm";
                PageEditJsWithTabTemplate.ClassName = className;
                PageEditJsWithTabTemplate.PForm = pform;
                PageEditJsWithTabTemplate.EForm = eform;
                PageEditJsWithTabTemplate.QForm = qform;
                PageEditJsWithTabTemplate.IsTask = istask;
                PageEditJsWithTabTemplate.PkPropertyname = "phid";
                PageEditJsWithTabTemplate.Title = title;
                PageEditJsWithTabTemplate.Area = area;
                PageEditJsWithTabTemplate.fieldSets = pbEdit.FieldSets;
                PageEditJsWithTabTemplate.panels = pbEdit.GridPanels;
                PageEditJsWithTabTemplate.AllGrids = pbEdit.AllGrids;
                PageEditJsWithTabTemplate.tabInfos = pbEdit.tabinfos;
                PageEditJsWithTabTemplate.tableLayouts = pbEdit.LayoutForm;
                PageEditJsWithTabTemplate.PanelNames = PanelNames;
                PageEditJsWithTabTemplate.Expressions = pbEdit.Expressions;
                PageEditJsWithTabTemplate.Toolbar = pbEdit.TB;
                PageEditJsWithTabTemplate.TableName = tableNameMst;
                PageEditJsWithTabTemplate.BodyCmpCount = billInfo.BodyCmpCount;
                PageEditJsWithTabTemplate.HasBlobdoc = billInfo.HasBlobdoc;
                PageEditJsWithTabTemplate.HasEppocx = billInfo.HasEppocx;
                PageEditJsWithTabTemplate.HasReport = billInfo.HasReport;
                PageEditJsWithTabTemplate.FieldSetBlobdoc = pbEdit.FieldSetBlobdoc;
                PageEditJsWithTabTemplate.PictureBoxs = pbEdit.PictureBoxs;
                PageEditJsWithTabTemplate.HasAsrGrid = billInfo.AsrGridInfo.PbBaseTextInfos.Count > 0 ? "1" : "0";
                PageEditJsWithTabTemplate.AsrGrid = pbEdit.AsrGrid;
                PageEditJsWithTabTemplate.HasWfGrid = billInfo.WfGridInfo.PbBaseTextInfos.Count > 0 ? "1" : "0";
                PageEditJsWithTabTemplate.WfGrid = pbEdit.WfGrid;
                PageEditJsWithTabTemplate.WriteEx("SUP");
            }
            CommonParser.Log("Edit界面生成成功.");
            #endregion

            DbHelper.Close();

            return true;
        }

        /// <summary>
        /// ex:
        /// ParseManager parseManager = new ParseManager();
        /// PbBillInfo billInfo = parseManager.ParseBillFile(@"F:\NG3\PowerBuilder\0000000069.ini"); 
        /// </summary>
        /// <param name="billInfo"> </param>
        /// <returns></returns>
        public static bool GenerateApp(PbBillInfo billInfo, ref string winType, string ucode)
        {
            string className = (ucode + billInfo.Name).Replace("pform", "aform");  //NG0001pform0000000008
            string title = billInfo.Description;

            try
            {
                DbHelper.Open(); //ConnectionString

                //List gridPanel
                PbListFormParser PbListParser = new PbListFormParser();
                GridPanel gridPanel = PbListParser.GetGridInfo(billInfo);
                Toolbar toolbar = PbListParser.GetListToolbar(billInfo);

                //Edit
                PbEditFormParser pbEdit = new PbEditFormParser(billInfo);
                List<string> PanelNames = new List<string>();

                //记录所有的panel
                foreach (var set in pbEdit.FieldSets)
                {
                    foreach (var panel in set.Panels)
                    {
                        PanelNames.Add(panel.TableName);
                    }
                }

                foreach (var panel in pbEdit.GridPanels)
                {
                    PanelNames.Add(panel.TableName);
                }

                string area = "SUP";

                try
                {
                    #region list
                    AformListTemplate.ListOrEdit = "viewedit";
                    if (gridPanel.Columns.Count > 0)  //否则不生成列表界面
                    {
                        winType = "List";
                        CommonParser.Log("List界面开始生成.");

                        AformListTemplate.NameSpacePrefix = area;
                        AformListTemplate.NameSpaceSuffix = "CustomForm";
                        AformListTemplate.ClassName = className;
                        AformListTemplate.PkPropertyname = "phid";
                        AformListTemplate.ListOrEdit = "viewlist";
                        AformListTemplate.Title = title;
                        AformListTemplate.Area = area;
                        AformListTemplate.gridPanel = gridPanel;
                        AformListTemplate.Toolbar = toolbar;
                        AformListTemplate.WriteEx(area);

                        CommonParser.Log("List界面生成成功.");
                    }
                    #endregion


                    #region edit
                    CommonParser.Log("Edit界面开始生成.");

                    AformEditTemplate.NameSpacePrefix = area;
                    AformEditTemplate.NameSpaceSuffix = "CustomForm";
                    AformEditTemplate.ClassName = className;
                    AformEditTemplate.PkPropertyname = "phid";
                    AformEditTemplate.ListOrEdit = "viewedit";
                    AformEditTemplate.Title = title;
                    AformEditTemplate.Area = area;
                    AformEditTemplate.gridPanel = gridPanel;
                    AformEditTemplate.fieldSets = pbEdit.FieldSets;
                    AformEditTemplate.panels = pbEdit.GridPanels;
                    AformEditTemplate.tableLayouts = pbEdit.LayoutForm;
                    AformEditTemplate.PanelNames = PanelNames;
                    AformEditTemplate.Expressions = pbEdit.Expressions;
                    AformEditTemplate.Toolbar = pbEdit.TB;
                    AformEditTemplate.WriteEx(area);

                    CommonParser.Log("Edit界面生成成功.");
                    #endregion
                }
                catch (Exception e)
                {
                    CommonParser.Log(e.Message);
                    //throw new Exception(e.Message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close();//ConnectionString);
            }
            return true;
        }
    }
}
