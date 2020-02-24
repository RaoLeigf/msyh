using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.PBDesignerParse;
using NG3.Metadata.UI.PowserBuilder;
using NG3.Metadata.UI.PowserBuilder.Controls;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;
using SUP.CustomForm.DataEntity.Container;
using SUP.CustomForm.DataEntity.Control;
using SUP.CustomForm.Rule.Expression;
using NG3.Log.Log4Net;

namespace SUP.CustomForm.Rule
{
    public class PbEditFormParser
    {
        //最终返回变量
        public List<FieldSet> FieldSets = new List<FieldSet>();     //内含form和grid，一般不会有grid
        public TableLayoutForm LayoutForm = new TableLayoutForm();  //fieldset外的form
        public List<GridPanel> GridPanels = new List<GridPanel>();  //fieldset外的grid        
        public Dictionary<String, String> Expressions = new Dictionary<String, String>();
        public Toolbar TB = new Toolbar();
        public List<PbTabInfo> tabinfos = new List<PbTabInfo>();
        public FieldSet FieldSetBlobdoc = new FieldSet();  //金格控件所在的fieldset
        public List<GridPanel> AllGrids = new List<GridPanel>();  //所有grid
        public List<NGPictureBox> PictureBoxs = new List<NGPictureBox>();  //图片控件
        public GridPanel AsrGrid = new GridPanel(); //附件单据体
        public GridPanel WfGrid = new GridPanel(); //审批单据体

        //临时变量
        private List<FieldSet> containers = new List<FieldSet>();//groupbox转换过来
        private List<GridPanel> grids = new List<GridPanel>();//grid,可能在groupbox里
        private TableLayoutForm layoutForm = new TableLayoutForm(); //表头普通field控件
        private Dictionary<String, String> expressions = new Dictionary<String, String>();  //用来存所有的表达式  <表达式, 类型>
        private Dictionary<String, String> tableRelation = new Dictionary<String, String>(); //用于存原有的tablename与生成的tablename间的关联 
        private Dictionary<String, Col> colRelation = new Dictionary<string, Col>();
        //用来存 col   id , 中文, 控件type


        #region 日志相关
        private static ILogger _logger = null;
        internal static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(PbEditFormParser), LogType.logrules);
                }
                return _logger;
            }
        }
        #endregion

        public PbEditFormParser(PbBillInfo billInfo)
        {
            FieldSets = GetFieldSetInfo(billInfo);
            GridPanels = GetGridPanel();
            LayoutForm = GetFormInfo(billInfo);

            Expressions = GetExpressions(billInfo);
            TB = GetEditToolbar(billInfo);
            tabinfos = GetTabInfos(billInfo);

            FieldSetBlobdoc = GetBlobdoc(billInfo);
            GetPictureBoxs(billInfo);
            GetAsrGrid(billInfo);
            GetWfGrid(billInfo);

            //汇总form和fieldset上所有列名
            foreach (var item in LayoutForm.AllFields)
            {
                if (!LayoutForm.ColumnNames.Contains(item.Name)) { LayoutForm.ColumnNames.Add(item.Name); }
            }
            foreach (var fieldset in FieldSets)
            {
                foreach (var item in fieldset.AllFields)
                {
                    if (!LayoutForm.ColumnNames.Contains(item.Name)) { LayoutForm.ColumnNames.Add(item.Name); }
                }
            }


            //计算表体游离panel数量，包括grid，tab，金格，进度控件等
            int tempCount = 0;

            for (int i = 0; i < GridPanels.Count; i++)
            {
                if (!GridPanels[i].IsInTab)
                {
                    tempCount++;
                }
            }

            if (tabinfos.Count == 1)
            {
                tempCount++;
            }

            if (billInfo.HasBlobdoc == "1")
            {
                tempCount++;
            }

            if (billInfo.HasEppocx == "1")
            {
                tempCount++;
            }

            if (billInfo.AsrGridInfo.Visible)
            {
                tempCount++;
            }

            if (billInfo.WfGridInfo.Visible)
            {
                tempCount++;
            }

            billInfo.BodyCmpCount = tempCount;


            //设置grid和tab的布局region
            SortedList<int, string> sortlist = new SortedList<int, string>();//游离panel排序

            if (tabinfos.Count > 0)  //有tab情况下，金格等控件在tab中
            {
                sortlist.Add(tabinfos[0].YPos, "tabPanel0");
            }
            for (int i = 0; i < GridPanels.Count; i++)
            {
                if (!GridPanels[i].IsInTab)
                {
                    sortlist.Add(GridPanels[i].Y, GridPanels[i].TableName);
                }
            }

            if (sortlist.Count > 0)
            {
                int maxY = Convert.ToInt32(sortlist.Last().Key);
                if (tabinfos.Count > 0)  //有tab情况下，金格等控件在tab中
                {
                    //根据组件y坐标来设置region
                    if (tabinfos[0].YPos == maxY)
                    {
                        tabinfos[0].Region = "center";
                    }
                    else
                    {
                        tabinfos[0].Region = "north";
                    }
                }

                for (int i = 0; i < GridPanels.Count; i++)
                {
                    if (!GridPanels[i].IsInTab)
                    {
                        if (GridPanels[i].Y == maxY && billInfo.HasBlobdoc != "1" && billInfo.HasEppocx != "1")
                        {
                            GridPanels[i].Region = "center";
                        }
                        else
                        {
                            GridPanels[i].Region = "north";
                        }
                    }
                }
            }
        }

        //返回金格fieldset
        public FieldSet GetBlobdoc(PbBillInfo billInfo)
        {
            //含有金格控件
            if (billInfo.OfficeInfo.Visible)
            {
                FieldSet fieldSet = new FieldSet();
                fieldSet.X = billInfo.OfficeInfo.XPos;
                fieldSet.Y = billInfo.OfficeInfo.YPos;
                fieldSet.Width = billInfo.OfficeInfo.Width;
                fieldSet.Height = billInfo.OfficeInfo.Height;
                fieldSet.Title = string.IsNullOrEmpty(billInfo.BlobdocName) ? "文档控件" : billInfo.BlobdocName;

                //判断金格控件在tab中位置
                foreach (var tabinfo in billInfo.PbTabInfos)
                {
                    for (int i = 0; i < tabinfo.GridIds.Count; i++)
                    {
                        if (tabinfo.GridIds[i] == "blobdoc")  //不是金格控件和进度控件才是grid
                        {
                            fieldSet.Region = Convert.ToString(i);
                            break;
                        }
                    }
                }

                return fieldSet;
            }
            else
            {
                return null;
            }
        }

        public void GetPictureBoxs(PbBillInfo billInfo)
        {
            foreach (PbPictureboxInfo picturebox in billInfo.HeadInfo.PbPictureboxInfos)
            {
                if (picturebox.Visible)
                {
                    PictureBoxs.Add(ControlConverter.GetPictureBox(picturebox));
                }
            }
        }

        public void GetAsrGrid(PbBillInfo billInfo)
        {
            if (billInfo.AsrGridInfo.Visible || billInfo.AsrGridInfo.PbBaseTextInfos.Count > 0)
            {
                AsrGrid = ControlConverter.ConvertToExtPanel(billInfo.AsrGridInfo);
            }
        }

        public void GetWfGrid(PbBillInfo billInfo)
        {
            if (billInfo.WfGridInfo.Visible || billInfo.WfGridInfo.PbBaseTextInfos.Count > 0)
            {
                WfGrid = ControlConverter.ConvertToExtPanel(billInfo.WfGridInfo);
            }
        }

        //表头普通的
        public TableLayoutForm GetFormInfo(PbBillInfo billInfo)
        {
            Dictionary<int, SortedList<int, PbBaseControlInfo>> dic =
                    new Dictionary<int, SortedList<int, PbBaseControlInfo>>();

            layoutForm.Items = this.Sort(layoutForm.Items, ref dic);

            /*  comment by ljy 此函数废弃，列宽改成按设计器列属性设置来确定
            //计算该fieldset 每行最多的colspan
            layoutForm = CalColSpan(layoutForm, dic);
            layoutForm.ColumnsPerRow = 4;
            */

            layoutForm.FieldRows = GetFieldsRows(dic, null, layoutForm);   //二维字段信息,放在CalColSpan方法之后

            layoutForm.IsAbsoluteLayout = billInfo.HeadInfo.Abslayout;     //是否绝对布局
            layoutForm.Otid = billInfo.HeadInfo.Otid;                      //文档模板可选项
            layoutForm.ColumnsPerRow = billInfo.HeadInfo.ColumnsPerRow;    //每行分几列
            layoutForm.FormLabelWidth = billInfo.HeadInfo.FormLabelWidth;  //每列占位数

            return layoutForm;
        }

        //panel里面的
        public List<GridPanel> GetGridPanel()
        {
            //grids 按表名排序
            grids.Sort((x, y) => x.Y.CompareTo(y.Y));

            //region 默认center
            foreach (var gridPanel in grids)
            {
                //item值不明？先注释
                //foreach (var item in gridPanel.Items)
                //{
                //    grids.Add(ControlConverter.ConvertToExtPanel((PbGridInfo)item));
                //}                

                AllGrids.Add(gridPanel);
            }

            foreach (var container in containers)
            {
                foreach (var gridPanel in container.Panels)
                {
                    AllGrids.Add(gridPanel);
                }
            }

            CommonParser.Log("Panel转换成功.");

            return grids;
        }

        //groupbox里面的fieldset
        public List<FieldSet> GetFieldSetInfo(PbBillInfo billInfo)
        {
            //1.定义容器：表头的groupbox，转成ng3中fieldset
            if (billInfo.HeadInfo.PbBaseControlInfos.Count > 0)
            {
                //将fieldset都先放入containers
                foreach (var headinfo in billInfo.HeadInfo.PbBaseControlInfos)
                {
                    FieldSet fieldSet = new FieldSet();
                    fieldSet.X = headinfo.XPos;
                    fieldSet.Y = headinfo.YPos;
                    fieldSet.Width = headinfo.Width;
                    fieldSet.Height = headinfo.Height;
                    fieldSet.ID = headinfo.Id;
                    fieldSet.Title = ((PbGroupboxInfo)(headinfo)).Text;

                    //设置fieldset折叠属性，/0、null或空：不能折叠，默认展开，1:能折叠，默认展开，2:能折叠，默认折叠，3:隐藏
                    var collapse = ((PbGroupboxInfo)(headinfo)).Collapse;
                    if (collapse == "1")
                    {
                        fieldSet.Collapsible = true;
                        fieldSet.Collapsed = false;
                    }
                    else if (collapse == "2")
                    {
                        fieldSet.Collapsible = true;
                        fieldSet.Collapsed = true;
                    }
                    else if (collapse == "3")
                    {
                        fieldSet.Collapsible = false;
                        fieldSet.Collapsed = false;
                        fieldSet.Border = false;
                        fieldSet.Title = "";
                    }
                    else
                    {
                        fieldSet.Collapsible = false;  //不允许折叠
                        fieldSet.Collapsed = false;    //默认展开
                    }

                    containers.Add(fieldSet);
                }
            }

            //2.表头所有列(包括fieldset中列)，放到layoutForm或容器
            foreach (var pbColumn in billInfo.HeadInfo.PbColumns)
            {
                //将控件放到指定的容器内
                FindContain(pbColumn, pbColumn.XPos, pbColumn.YPos);
            }

            //3.容器内列排序和生成成二维列表列
            for (int i = 0; i < containers.Count; i++)
            {
                Dictionary<int, SortedList<int, PbBaseControlInfo>> dic =
                    new Dictionary<int, SortedList<int, PbBaseControlInfo>>();
                containers[i].Items = Sort(containers[i].Items, ref dic);

                /*  comment by ljy 此函数废弃，列宽改成按设计器列属性设置来确定
                //计算该fieldset 每行最多的colspan
                //算占多少  //不能用ref 动态类
                containers[i] = CalColSpan(containers[i], dic);
                //containers[i].ColumnsPerRow = 4; 
                */

                containers[i].ColumnsPerRow = billInfo.HeadInfo.ColumnsPerRow;  //每行分几列

                //按行布局二维列表结构, 控件转成ng3类型; 容器内所有列转成ng3类型后赋给Allfields; 每行几列按设计器上实际布局来
                containers[i].FieldRows = GetFieldsRows(dic, containers[i], null);
            }

            //4.表体所有grid(包括fieldset中grid)，转成ng类型并放到grids或容器（fieldset中可能有grid，在设计时就应该避免groupbox中有grid）
            if (billInfo.PbGrids.Count > 0)
            {
                foreach (var gridpanel in billInfo.PbGrids)
                {
                    FindContain(gridpanel, gridpanel.XPos, gridpanel.YPos);
                }
            }


            /*
            //含有金格控件
            if (billInfo.OfficeInfo.Visible)
            {
                FieldSet fieldSet = new FieldSet();
                fieldSet.X = billInfo.OfficeInfo.XPos;
                fieldSet.Y = billInfo.OfficeInfo.YPos;
                fieldSet.Width = billInfo.OfficeInfo.Width;
                fieldSet.Height = billInfo.OfficeInfo.Height;
                fieldSet.Title = "金格控件";
                FieldSetBlobdoc = fieldSet;  //赋给返回的金格控件fieldset
                //containers.Add(fieldSet);
            }

            //含有进度控件
            if (billInfo.ScheduleInfo.Visible)
            {
                FieldSet fieldSet = new FieldSet();
                fieldSet.X = billInfo.ScheduleInfo.XPos;
                fieldSet.Y = billInfo.ScheduleInfo.YPos;
                fieldSet.Width = billInfo.ScheduleInfo.Width;
                fieldSet.Height = billInfo.ScheduleInfo.Height;
                fieldSet.Title = "进度控件";
                containers.Add(fieldSet);
            }
            */

            //为容器内的控件进行布局 region
            //SetLayout();

            //GroupBox根据Y坐标进行排序
            containers.Sort((x, y) => x.Y.CompareTo(y.Y));

            CommonParser.Log("GroupBox转换成功.");

            return containers;
        }

        //获取表达式
        public Dictionary<string, string> GetExpressions(PbBillInfo billInfo)
        {
            GetTableRelation(); //将表关系确定，表达式中可能需要用到
            GetColRelation();

            //groupbox控件的更新事件
            for (int i = 0; i < containers.Count; i++)
            {
                foreach (var item in containers[i].Items)
                {
                    if (item is PbGridInfo)
                        continue;  //表体内的控件事件暂无

                    var temp = (PbBaseTextInfo)item;
                    ConvertExpression(temp.UpdateEvent.PbImp, "Normal", temp.Name);
                    ConvertExpression(temp.VisibleExpressionImp, "Normal", temp.Name);
                    ConvertExpression(temp.IsMustInputExpressionImp, "Normal", temp.Name);
                    ConvertExpression(temp.IsReadOnlyExpressionImp, "Normal", temp.Name);
                }
            }

            //grids列的更新事件
            foreach (var gridPanel in billInfo.PbGrids)
            {
                foreach (var item in gridPanel.PbBaseTextInfos)
                {
                    ConvertExpressionGrid(item.UpdateEvent.PbImp, "GridNormal", item.Name, gridPanel.TableName);
                    ConvertExpressionGrid(item.VisibleExpressionImp, "Normal", item.Name, gridPanel.TableName);
                    ConvertExpressionGrid(item.IsReadOnlyExpressionImp, "Normal", item.Name, gridPanel.TableName);
                }
            }

            //mstform控件的更新事件
            foreach (var item in layoutForm.Items)
            {
                var temp = (PbBaseTextInfo)item;
                ConvertExpression(temp.UpdateEvent.PbImp, "Normal", temp.Name);
                ConvertExpression(temp.VisibleExpressionImp, "Normal", temp.Name);
                ConvertExpression(temp.IsMustInputExpressionImp, "Normal", temp.Name);
                ConvertExpression(temp.IsReadOnlyExpressionImp, "Normal", temp.Name);
            }


            //新增状态打开时的事件
            if (billInfo.EditAddInitEvent.PbImp.Count > 0)
            {
                ConvertExpression(billInfo.EditAddInitEvent.PbImp, "AddInit");
            }

            //保存时更新事件
            if (billInfo.BillSaveUpdateEvent.PbImp.Count > 0)
            {
                ConvertExpression(billInfo.BillSaveUpdateEvent.PbImp, "SaveUpdate");
            }

            //保存前校验事件
            if (billInfo.BillBeforeSaveEvent.PbImp.Count > 0)
            {
                ConvertExpression(billInfo.BillBeforeSaveEvent.PbImp, "BeforeSave");
            }

            return expressions;
        }

        //pb表达式转换为前台代码
        public void ConvertExpression(IList<PbExpressionImp> PbImps, string type, string colname)
        {
            string expression = string.Empty;
            string temptype = string.Empty;

            foreach (var PbImp in PbImps)
            {
                try
                {

                    temptype = type;
                    switch (PbImp.ExpressionType)
                    {
                        case PbExpressionType.ComputeFuncAndFillSpecificCol:
                            {
                                ComputeFuncFillSpecificColExp colExp = new ComputeFuncFillSpecificColExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                break;
                            }
                        case PbExpressionType.ComputeSqlAndFillSpecificCol:
                            {
                                ComputeSqlFillSpecificColExp colExp = new ComputeSqlFillSpecificColExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname, temptype);
                                break;
                            }
                        case PbExpressionType.ComputeExpAndDisBodyCol:
                            {
                                ComputeExpDisBodyColExp colExp = new ComputeExpDisBodyColExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                break;
                            }
                        case PbExpressionType.ExecuteSql:
                            {
                                ExecuteSqlExp colExp = new ExecuteSqlExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation);
                                break;
                            }
                        case PbExpressionType.BillHeadUniqueCheck:
                            {
                                BillHeadUniqueCheck colExp = new BillHeadUniqueCheck(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation);
                                break;
                            }
                        case PbExpressionType.CheckBodyUnique:
                            {
                                CheckBodyUnique colExp = new CheckBodyUnique(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation);
                                break;
                            }

                        case PbExpressionType.BillValidation:
                            {
                                if (PbImp.Expression.Split(',').Length > 3)
                                {
                                    BillValidationComplexExp colExp = new BillValidationComplexExp(PbImp);
                                    expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                    temptype = "BeforeSave";
                                }
                                else
                                {
                                    if (PbImp.Expression.Split(',')[2] == "1")
                                    {
                                        temptype = "Normal";
                                    }
                                    else
                                    {
                                        temptype = "BeforeSave";
                                    }
                                    BillValidationExp colExp = new BillValidationExp(PbImp);
                                    expression = colExp.GetCodeFromExp(tableRelation, colRelation);
                                }
                                break;
                            }
                        case PbExpressionType.ComputeSqlAndFillSpecificBodyCol:
                            {
                                ComputeSqlFillSpecificBodyColExp colExp = new ComputeSqlFillSpecificBodyColExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                break;
                            }
                        case PbExpressionType.ComputeExpAndFillSpecificCol:
                            {
                                ComputeExpAndFillSpecificCol colExp = new ComputeExpAndFillSpecificCol(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                temptype = "Normal"; //"Supcan"; comment by ljy 2016.12.10 事件暂时不支持函数
                                break;
                            }
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("ConvertExpression: type:" + PbImp.ExpressionType.ToString() + "\r\n" + e.StackTrace);
                }


                if (!string.IsNullOrEmpty(expression) && !expressions.ContainsKey(expression))
                {
                    expressions.Add(expression, temptype);
                }
            }
        }

        public void ConvertExpression(IList<PbExpressionImp> PbImps, string type)
        {
            ConvertExpression(PbImps, type, null);
        }

        //pb表达式转换为前台代码 可见、必输、保护
        public void ConvertExpression(PbExpressionImp PbImp, string type, string colname)
        {
            string expression = string.Empty;
            string temptype = string.Empty;

            if (string.IsNullOrEmpty(PbImp.Expression))
            {
                return;
            }

            try
            {
                temptype = type;
                switch (PbImp.ExpressionType)
                {
                    case PbExpressionType.IsVisible:
                        {
                            IsVisibleExp colExp = new IsVisibleExp(PbImp);
                            expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                            break;
                        }
                    case PbExpressionType.IsMustInput:
                        {
                            IsMustInputExp colExp = new IsMustInputExp(PbImp);
                            expression = colExp.GetCodeFromExp(tableRelation, colRelation);
                            break;
                        }
                    case PbExpressionType.IsReadOnly:
                        {
                            IsReadOnlyExp colExp = new IsReadOnlyExp(PbImp);
                            expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("ConvertExpression: type:" + PbImp.ExpressionType.ToString() + "\r\n" + e.StackTrace);
            }


            if (!string.IsNullOrEmpty(expression) && !expressions.ContainsKey(expression))
            {
                expressions.Add(expression, temptype);
            }
        }

        //pb表达式转换为前台代码 Grid
        public void ConvertExpressionGrid(IList<PbExpressionImp> PbImps, string type, string colname, string tablename)
        {
            string expression = string.Empty;
            string temptype = string.Empty;

            foreach (var PbImp in PbImps)
            {
                try
                {

                    temptype = type;
                    switch (PbImp.ExpressionType)
                    {
                        case PbExpressionType.ComputeFuncAndFillSpecificCol:
                            {
                                ComputeFuncFillSpecificColExp colExp = new ComputeFuncFillSpecificColExp(PbImp);
                                expression = colExp.GetCodeFromExpGrid(tableRelation, colRelation, colname, tablename);
                                break;
                            }
                        case PbExpressionType.ComputeSqlAndFillSpecificCol:
                            {
                                ComputeSqlFillSpecificColExp colExp = new ComputeSqlFillSpecificColExp(PbImp);
                                expression = colExp.GetCodeFromExpGrid(tableRelation, colRelation, colname, tablename);
                                break;
                            }
                        case PbExpressionType.ComputeExpAndDisBodyCol:
                            {
                                ComputeExpDisBodyColExp colExp = new ComputeExpDisBodyColExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                break;
                            }
                        case PbExpressionType.ExecuteSql:
                            {
                                ExecuteSqlExp colExp = new ExecuteSqlExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation);
                                break;
                            }
                        case PbExpressionType.BillHeadUniqueCheck:
                            {
                                BillHeadUniqueCheck colExp = new BillHeadUniqueCheck(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation);
                                break;
                            }
                        case PbExpressionType.CheckBodyUnique:
                            {
                                CheckBodyUnique colExp = new CheckBodyUnique(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation);
                                break;
                            }

                        case PbExpressionType.BillValidation:
                            {
                                if (PbImp.Expression.Split(',').Length > 3)
                                {
                                    BillValidationComplexExp colExp = new BillValidationComplexExp(PbImp);
                                    expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                    temptype = "BeforeSave";
                                }
                                else
                                {
                                    if (PbImp.Expression.Split(',')[2] == "1")
                                    {
                                        temptype = "Normal";
                                    }
                                    else
                                    {
                                        temptype = "BeforeSave";
                                    }
                                    BillValidationExp colExp = new BillValidationExp(PbImp);
                                    expression = colExp.GetCodeFromExp(tableRelation);
                                }
                                break;
                            }
                        case PbExpressionType.ComputeSqlAndFillSpecificBodyCol:
                            {
                                ComputeSqlFillSpecificBodyColExp colExp = new ComputeSqlFillSpecificBodyColExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                break;
                            }
                        case PbExpressionType.ComputeExpAndFillSpecificCol:
                            {
                                ComputeExpAndFillSpecificCol colExp = new ComputeExpAndFillSpecificCol(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                temptype = "Normal"; //"Supcan"; comment by ljy 2016.12.10 事件暂时不支持函数
                                break;
                            }
                        case PbExpressionType.IsReadOnly:
                            {
                                IsReadOnlyExp colExp = new IsReadOnlyExp(PbImp);
                                expression = colExp.GetCodeFromExp(tableRelation, colRelation, colname);
                                break;
                            }
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("ConvertExpression: type:" + PbImp.ExpressionType.ToString() + "\r\n" + e.StackTrace);
                }


                if (!string.IsNullOrEmpty(expression) && !expressions.ContainsKey(expression))
                {
                    expressions.Add(expression, temptype);
                }
            }
        }

        //pb表达式转换为前台代码 Grid 可见、必输、保护
        public void ConvertExpressionGrid(PbExpressionImp PbImp, string type, string colname, string tablename)
        {
            string expression = string.Empty;
            string temptype = string.Empty;

            if (string.IsNullOrEmpty(PbImp.Expression))
            {
                return;
            }

            try
            {
                temptype = type;
                switch (PbImp.ExpressionType)
                {
                    case PbExpressionType.IsVisible:
                        {
                            IsVisibleExp colExp = new IsVisibleExp(PbImp);
                            expression = colExp.GetCodeFromExpGrid(tableRelation, colRelation, colname, tablename);
                            break;
                        }
                    case PbExpressionType.IsReadOnly:
                        {
                            IsReadOnlyExp colExp = new IsReadOnlyExp(PbImp);
                            expression = colExp.GetCodeFromExpGrid(tableRelation, colRelation, colname, tablename);
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("ConvertExpression: type:" + PbImp.ExpressionType.ToString() + "\r\n" + e.StackTrace);
            }


            if (!string.IsNullOrEmpty(expression) && !expressions.ContainsKey(expression))
            {
                expressions.Add(expression, temptype);
            }
        }


        //获取表名的对应
        public void GetTableRelation()
        {
            //container里面的panel
            foreach (var container in containers)
            {
                foreach (var item in container.Items)
                {
                    if (item is PbGridInfo)
                    {
                        var temp = (PbGridInfo)item;
                        tableRelation.Add(temp.TableName, item.Name);
                    }
                }
            }

            //grids里面的panel
            foreach (var gridPanel in grids)
            {
                //item值不明？先注释
                //foreach (var item in gridPanel.Items)
                //{
                //    var temp = (PbGridInfo)item;
                //    tableRelation.Add(temp.TableName, item.Name);
                //}

                tableRelation.Add(gridPanel.TableName, gridPanel.TableName);
            }
        }

        public void GetColRelation()
        {
            var col = new Col();

            //LayoutForm
            foreach (var field in LayoutForm.AllFields)
            {
                col.Id = field.Name;
                col.Name = field.FieldLabel;
                col.Xtype = field.XType;
                if (colRelation.ContainsKey(col.Id))
                    continue;
                colRelation.Add(col.Id, col);
            }

            //container里面的panel
            foreach (var container in containers)
            {
                foreach (var field in container.AllFields)
                {
                    col.Id = field.Name;
                    col.Name = field.FieldLabel;
                    col.Xtype = field.XType;
                    if (colRelation.ContainsKey(col.Id))
                        continue;
                    colRelation.Add(col.Id, col);
                }
            }

            //grids里面的panel
            foreach (var gridPanel in grids)
            {
                foreach (var field in gridPanel.Columns)
                {
                    col.Id = field.DataIndex;
                    col.Name = field.Header;
                    col.Xtype = field.editor.XType;
                    if (colRelation.ContainsKey(col.Id))
                        continue;
                    colRelation.Add(col.Id, col);
                }
            }
        }

        //为容器内的控件进行布局 region  borderLayout
        public void SetLayout()
        {
            Dictionary<string, object> region = new Dictionary<string, object>();

            //if (layoutForm.Items.Count > 0)
            //{
            ////    layoutForm.Region = "north";
            ////    region.Add("north", "north");
            //}

            if (grids.Count > 0)
            {
                foreach (var gridPanel in grids)
                {
                    gridPanel.Region = "center";
                }
                region.Add("center", "center");
            }

            //if ( (containers.Count + grids.Count) > 3 )
            //{
            //    //改用绝对布局
            //    LayoutForm.IsAbsoluteLayout = true;
            //    return;
            //}

            for (int i = 0; i < containers.Count; i++)
            {
                if (!region.ContainsKey("center"))
                {
                    containers[i].Region = "center";
                    region.Add("center", containers[i]);
                }
                else
                {
                    //已经有center                    
                    if (region["center"] is string)
                    {
                        containers[i].Region = "north";

                        if (!region.ContainsKey("north"))
                        {
                            region.Add("north", containers[i]);

                        }
                        continue;

                        //else
                        //{
                        //    containers[i].Region = "south";
                        //    region.Add("south", containers[i]);
                        //    continue;
                        //}
                    }

                    FieldSet tempSet = region["center"] as FieldSet;

                    //起点相同，或者终点相同即为该方向上一致
                    //说明不是左右的关系
                    if (Math.Abs(tempSet.X - containers[i].X) < 10 ||
                        Math.Abs(tempSet.X + tempSet.Width - containers[i].X - containers[i].Width) < 10)
                    {
                        //且不是上下的关系
                        if (Math.Abs(tempSet.Y - containers[i].Y) < 10 ||
                            Math.Abs(tempSet.Y + tempSet.Height - containers[i].Y - containers[i].Height) < 10)
                        {
                            ////对角线关系，两个fieldset有重叠，设计失误，先不处理
                            ////或者调用绝对布局
                        }
                        //是上下关系
                        else
                        {

                            if (containers[i].Y < tempSet.Y)
                            {
                                containers[i].Region = "north";
                                if (!region.ContainsKey("north"))
                                {
                                    region.Add("north", containers[i]);
                                }
                            }
                            else
                            {
                                containers[i].Region = "center";
                                tempSet.Region = "north";
                                if (!region.ContainsKey("north"))
                                {
                                    region.Add("north", region["center"]);
                                }
                                region["center"] = containers[i];
                            }
                        }
                    }
                    //不是上下关系
                    else if (Math.Abs(tempSet.Y - containers[i].Y) < 10 ||
                            Math.Abs(tempSet.Y + tempSet.Height - containers[i].Y - containers[i].Height) < 10)
                    {
                        //也不是左右关系
                        if (Math.Abs(tempSet.X - containers[i].X) < 10 ||
                            Math.Abs(tempSet.X + tempSet.Width - containers[i].X - containers[i].Width) < 10)
                        {
                            //对角线关系，先不处理
                        }
                        //是左右关系
                        else
                        {
                            if (containers[i].X < tempSet.X)
                            {
                                containers[i].Region = "west";
                                if (!region.ContainsKey("west"))
                                {
                                    region.Add("west", containers[i]);
                                }
                            }
                            else
                            {
                                containers[i].Region = "center";
                                tempSet.Region = "west";
                                if (!region.ContainsKey("west"))
                                {
                                    region.Add("west", region["center"]);
                                }
                                region["center"] = containers[i];
                            }
                        }
                    }
                    else
                    {
                        //对角线或其他关系
                    }
                }
            }
            region.Clear();
        }

        //将控件放到指定的容器内
        public void FindContain(PbBaseControlInfo control, int x, int y)
        {
            if (control is PbTextInfo)
            {
                var tempControl = control as PbTextInfo;
                if (tempControl.Name == "code")
                {
                    return; //主键不参与布局
                }
            }

            if (control.Visible == false && control.ControlType != PbControlType.Grid)
            {
                layoutForm.Items.Add(control);
                return;
            }

            //若没有容器  //单grid的情况 或者是 tableLayout
            if (containers.Count == 0)
            {
                if (control is PbGridInfo)
                {
                    grids.Add(ControlConverter.ConvertToExtPanel((PbGridInfo)control));
                }
                else
                {
                    layoutForm.Items.Add(control);
                }
                return;
            }
            else
            {
                //有容器（就是pb的groupbox和ng3中的fieldset）
                ExtContainer targetContain = new ExtContainer();

                int i = 0;
                do
                {
                    targetContain.X = containers[i].X;
                    targetContain.Y = containers[i].Y;
                    targetContain.Width = containers[i].Width;
                    targetContain.Height = containers[i].Height;

                    if ((x * 1.3) >= targetContain.X
                        && (x * 1.3) <= targetContain.X + targetContain.Width
                        && y >= targetContain.Y
                        && y <= targetContain.Y + targetContain.Height)
                    {
                        if (control is PbGridInfo)
                        {
                            containers[i].Panels.Add(ControlConverter.ConvertToExtPanel((PbGridInfo)control));  //此处会把容器内的grid放进去，后期看看是否要改
                        }
                        else
                        {
                            containers[i].Items.Add(control);
                        }


                        break;
                    }
                    i++;
                } while (i < containers.Count);

                //容器外的grid或列
                if (i == containers.Count)
                {
                    if (control is PbGridInfo)
                    {
                        var tempPanel = control as PbGridInfo;
                        grids.Add(ControlConverter.ConvertToExtPanel(tempPanel));
                    }
                    else
                    {
                        layoutForm.Items.Add(control);
                    }
                }
            }

        }

        /*  //comment by ljy 此函数废弃，列宽改成按设计器列属性设置来确定
        //计算span
        public TableLayoutForm CalColSpan(TableLayoutForm form, Dictionary<int, SortedList<int, PbBaseControlInfo>> dic)
        {
            var sortSet = new SortedSet<int>();
            var checkSet = new SortedSet<int>(); //为多选框服务的

            //对列宽进行排序
            foreach (var pbColumn in form.Items)
            {
                if (pbColumn is PbCheckboxInfo)
                    continue;
                sortSet.Add(pbColumn.Width);
            }

            foreach (var pbColumn in form.Items)
            {
                if (pbColumn is PbCheckboxInfo)
                    continue;
                pbColumn.Span = Convert.ToInt32(Math.Ceiling((double)pbColumn.Width / ((double)1.5 * sortSet.Min)));
            }

            foreach (var pbColumn in form.Items)
            {
                if (pbColumn is PbCheckboxInfo)
                    pbColumn.Span = checkSet.Min;
            }

            //计算每行最多的列 colspan
            int maxSpan = 0;
            foreach (var items in dic)
            {
                int tempSpan = 0;
                foreach (var item in items.Value)
                {
                    if (item.Value.Name == "ocode" || item.Value.Name == "code")
                        continue;
                    tempSpan += item.Value.Span;
                }

                if (tempSpan > maxSpan)
                {
                    maxSpan = tempSpan;
                }
            }
            form.ColumnsPerRow = maxSpan;

            return form;
        }

        //计算span
        public FieldSet CalColSpan(FieldSet container, Dictionary<int, SortedList<int, PbBaseControlInfo>> dic)
        {
            var sortSet = new SortedSet<int>();
            var checkSet = new SortedSet<int>(); //为多选框服务的

            //对列宽进行排序
            foreach (var pbColumn in container.Items)
            {
                if (pbColumn is PbCheckboxInfo)
                    continue;
                sortSet.Add(pbColumn.Width);
            }

            foreach (var pbColumn in container.Items)
            {
                if (pbColumn is PbCheckboxInfo)
                    continue;
                pbColumn.Span = Convert.ToInt32(Math.Ceiling((double)pbColumn.Width / ((double)1.5 * sortSet.Min)));
                checkSet.Add(pbColumn.Span);
            }

            foreach (var pbColumn in container.Items)
            {
                if (pbColumn is PbCheckboxInfo)
                    pbColumn.Span = checkSet.Min;
            }


            //计算每行最多的列 colspan
            int maxSpan = 0;
            foreach (var items in dic)
            {
                int tempSpan = 0;
                foreach (var item in items.Value)
                {
                    tempSpan += item.Value.Span;
                }

                if (tempSpan > maxSpan)
                {
                    maxSpan = tempSpan;
                }
            }
            container.ColumnsPerRow = maxSpan;

            return container;
        }
        */

        //排序
        private List<PbBaseControlInfo> Sort(List<PbBaseControlInfo> PbColumns, ref Dictionary<int, SortedList<int, PbBaseControlInfo>> dic)
        {
            SortedSet<int> sortset = new SortedSet<int>();

            //隐藏列单独放一行
            sortset.Add(-1);

            //设置行
            foreach (PbBaseControlInfo control in PbColumns)
            {
                if (control.Visible == false) continue;
                sortset.Add(control.YPos);
            }

            //行归类
            foreach (int item in sortset)
            {
                SortedList<int, PbBaseControlInfo> list = new SortedList<int, PbBaseControlInfo>();
                dic.Add(item, list);
            }

            //计算控件属于哪一行
            foreach (PbBaseControlInfo control in PbColumns)
            {
                int n = 1;
                //隐藏列单独放在一起
                if (control.Visible == false)
                {
                    dic[-1].Add(n, control);
                    n++;
                    continue;
                }

                int y = control.YPos;

                foreach (var item in dic)
                {
                    if ((Math.Abs(y - item.Key) < 8))//小于控件高度的一半视为同一行
                    {
                        item.Value.Add(control.XPos, control);
                        break;
                    }
                }
            }

            List<PbBaseControlInfo> allControl = new List<PbBaseControlInfo>();


            foreach (var item in dic)
            {
                foreach (var control in item.Value)
                {
                    allControl.Add(control.Value);
                }
            }

            return allControl;
        }

        //获取二维数组的字段信息
        private List<List<ExtControlBase>> GetFieldsRows(Dictionary<int, SortedList<int, PbBaseControlInfo>> dic, FieldSet fset, TableLayoutForm lForm)
        {
            List<List<ExtControlBase>> fieldRows = new List<List<ExtControlBase>>();
            ExtControlBase extControl = null;

            foreach (var item in dic)
            {
                List<ExtControlBase> list = new List<ExtControlBase>();

                foreach (var control in item.Value)
                {
                    extControl = ControlConverter.ConvertToExtControl(control.Value);
                    list.Add(extControl);

                    if (fset != null)
                    {
                        fset.AllFields.Add(extControl);
                    }
                    if (lForm != null)
                    {
                        lForm.AllFields.Add(extControl);
                    }
                }

                if (list.Count > 0)
                {
                    fieldRows.Add(list);
                }
            }

            return fieldRows;
        }


        //获取toolbar
        public Toolbar GetEditToolbar(PbBillInfo billInfo)
        {
            var toolbar = new Toolbar();
            string btname = string.Empty;
            foreach (var buttons in billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos)
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
                        if (!toolbar.LNgButtons.Contains(btname))
                        {
                            toolbar.LNgButtons.Add(btname);
                        }
                    }
                    else
                    {
                        btname = ButtonNameConvert.ConvertToExtTpye(button);
                        if (!toolbar.RNgButtons.Contains(btname))
                        {
                            toolbar.RNgButtons.Add(btname);
                        }
                    }
                }
            }

            return toolbar;
        }

        public List<PbTabInfo> GetTabInfos(PbBillInfo billInfo)
        {
            List<PbTabInfo> tabinfos = new List<PbTabInfo>();

            //所有tab循环转换，目前只支持一个tab，也就是billInfo.PbTabInfos数量为1
            foreach (var tabinfo in billInfo.PbTabInfos)
            {
                PbTabInfo tab = new PbTabInfo();
                tab.GridIds = tabinfo.GridIds;
                tab.ReportViews = tabinfo.ReportViews;
                tab.ReportParas = tabinfo.ReportParas;
                tab.TabNames = tabinfo.TabNames;
                tab.XPos = tabinfo.XPos;
                tab.YPos = tabinfo.YPos;
                tab.Width = tabinfo.Width;
                tab.Height = tabinfo.Height;
                tab.Name = tabinfo.Name;
                tabinfos.Add(tab);
            }

            return tabinfos;
        }
    }

    public struct Col
    {
        public string Id;
        public string Name;
        public string Xtype;
    }
}
