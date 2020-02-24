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

namespace SUP.CustomForm.Rule
{
    public class AppPbEditFormParser
    {
        public List<FieldSet> FieldSets = new List<FieldSet>(); 
        public List<GridPanel> GridPanels = new List<GridPanel>();
        public TableLayoutForm LayoutForm = new TableLayoutForm();
        public Dictionary<String, String> Expressions = new Dictionary<String, String>(); 
        public Toolbar TB = new Toolbar();
        public List<String> tabinfos = new List<String>();

        private List<FieldSet> containers = new List<FieldSet>();//groupbox转换过来
        private List<GridPanel> grids = new List<GridPanel>();//grid,可能在groupbox里
        private TableLayoutForm layoutForm = new TableLayoutForm(); //表头普通field控件
        private Dictionary<String, String> expressions = new Dictionary<String, String>();  //用来存所有的表达式  <表达式, 类型>
        private Dictionary<String, String> tableRelation = new Dictionary<String, String>(); //用于存原有的tablename与生成的tablename间的关联 
        private Dictionary<String,Col> colRelation = new Dictionary<string, Col>();
        //用来存 col   id , 中文, 控件type

        public AppPbEditFormParser(PbBillInfo billInfo)
        {
            FieldSets = GetFieldSetInfo(billInfo);
            GridPanels = GetGridPanel();
            LayoutForm = GetFormInfo();
            Expressions = GetExpressions(billInfo);
            TB = GetEditToolbar(billInfo);
            tabinfos = GetTabInfos(billInfo);
        }

        //表头普通的
        public TableLayoutForm GetFormInfo()
        {
            Dictionary<int, SortedList<int, PbBaseControlInfo>> dic =
                    new Dictionary<int, SortedList<int, PbBaseControlInfo>>();

            layoutForm.Items = this.Sort(layoutForm.Items, ref dic);
            
            //计算该fieldset 每行最多的colspan
            layoutForm = CalColSpan(layoutForm, dic);

            //转类型
            foreach (var item in layoutForm.Items)
            {
                layoutForm.FieldsApp.Add(AppControlConverter.ConvertToExtControl(item));
            }
            CommonParser.Log("TableLayoutForm转换成功.");
            return layoutForm;
        }

        //panel里面的
        public List<GridPanel> GetGridPanel()
        {
            //region 默认center
            foreach (var gridPanel in grids)
            {
                foreach (var item in gridPanel.Items)
                {
                    grids.Add(ControlConverter.ConvertToExtPanel((PbGridInfo)item));
                }   
            }
            CommonParser.Log("Panel转换成功.");
            return grids;
        }

        //groupbox里面的fieldset
        public List<FieldSet> GetFieldSetInfo(PbBillInfo billInfo)
        {
            //含有groupbox表头
            if (billInfo.HeadInfo.PbBaseControlInfos.Count > 0)
            {
                //将容器都先放入containers
                foreach (var headinfo in billInfo.HeadInfo.PbBaseControlInfos)
                {
                    FieldSet fieldSet = new FieldSet();
                    fieldSet.X = headinfo.XPos;
                    fieldSet.Y = headinfo.YPos;
                    fieldSet.Width = headinfo.Width;
                    fieldSet.Height = headinfo.Height;
                    fieldSet.ID = headinfo.Id;
                    fieldSet.Title = ((PbGroupboxInfo) (headinfo)).Text;
                    containers.Add(fieldSet);
                }
            }

            foreach (var pbColumn in billInfo.HeadInfo.PbColumns)
            {
                //将控件放到指定的容器内
                FindContain(pbColumn, pbColumn.XPos, pbColumn.YPos);
            }

            //因为要赋值回去，所以不能foreach
            for (int i = 0; i < containers.Count; i++)
            {
                Dictionary<int, SortedList<int, PbBaseControlInfo>> dic =
                    new Dictionary<int, SortedList<int, PbBaseControlInfo>>();
                containers[i].Items = this.Sort(containers[i].Items, ref dic);

                //计算该fieldset 每行最多的colspan
                //算占多少  //不能用ref 动态类
                containers[i] = CalColSpan(containers[i], dic);
            }

            //含有表体
            if (billInfo.PbGrids.Count > 0)
            {
                foreach (var gridpanel in billInfo.PbGrids)
                {
                    FindContain(gridpanel, gridpanel.XPos, gridpanel.YPos);
                }
            }

            //含有金格控件
            if (billInfo.OfficeInfo.Visible)
            {
                FieldSet fieldSet = new FieldSet();
                fieldSet.X = billInfo.OfficeInfo.XPos;
                fieldSet.Y = billInfo.OfficeInfo.YPos;
                fieldSet.Width = billInfo.OfficeInfo.Width;
                fieldSet.Height = billInfo.OfficeInfo.Height;
                fieldSet.Title = "金格控件";
                containers.Add(fieldSet); 
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

            //为容器内的控件进行布局 region
            SetLayout();

            for (int i = 0; i < containers.Count; i++)
            {
                //转类型
                foreach (var item in containers[i].Items)
                {
                    if (item is PbGridInfo)
                    {
                        containers[i].Panels.Add(ControlConverter.ConvertToExtPanel((PbGridInfo)item));
                    }
                    else
                    {
                        containers[i].AllFields.Add(ControlConverter.ConvertToExtControl(item));
                    }
                    //PbBaseControlInfo pbcontrol = item;
                   // expressions.Add(()pbcontrol.);
                }
            }
            CommonParser.Log("GroupBox转换成功.");
            return containers;
        }

        //获取表达式
        public Dictionary<String, String> GetExpressions(PbBillInfo billInfo)
        {
            GetTableRelation(); //将表关系确定，表达式中可能需要用到
            GetColRelation();
            for (int i = 0; i < containers.Count; i++)
            {
                foreach (var item in containers[i].Items)
                {
                    if (item is PbGridInfo)
                        continue;  //表体内的控件事件暂无

                    var temp = (PbBaseTextInfo)item;
                    ConvertExpression(temp.UpdateEvent.PbImp,"Normal");
                }
            }


            //新增状态打开时的事件
            if (billInfo.EditAddInitEvent.PbImp.Count > 0)
            {
                ConvertExpression(billInfo.EditAddInitEvent.PbImp,"AddInit");
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
        public void ConvertExpression(IList<PbExpressionImp> PbImps,String type, string colname)
        {
            string expression = string.Empty;
            string temptype = string.Empty;

            foreach (var PbImp in PbImps)
            {
                temptype = type;
                switch (PbImp.ExpressionType)
                {
                    case PbExpressionType.ComputeFuncAndFillSpecificCol:
                        {
                            ComputeFuncFillSpecificColExp colExp = new ComputeFuncFillSpecificColExp(PbImp);
                            expression = colExp.GetCodeFromExp(tableRelation);
                            break;
                        }
                    case PbExpressionType.ComputeSqlAndFillSpecificCol:
                        {
                            ComputeSqlFillSpecificColExp colExp = new ComputeSqlFillSpecificColExp(PbImp);
                            expression = colExp.GetCodeFromExp(tableRelation);
                            break;
                        }
                    case PbExpressionType.ComputeExpAndDisBodyCol:
                        {
                            ComputeExpDisBodyColExp colExp = new ComputeExpDisBodyColExp(PbImp);
                            expression = colExp.GetCodeFromExp(tableRelation);
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
                            expression = colExp.GetCodeFromExp(tableRelation,colRelation);
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
                                expression = colExp.GetCodeFromExp(tableRelation);
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
                            expression = colExp.GetCodeFromExp(tableRelation,colRelation, colname);
                            break;
                        }
                    case PbExpressionType.ComputeExpAndFillSpecificCol:
                        {
                            ComputeExpAndFillSpecificCol colExp = new ComputeExpAndFillSpecificCol(PbImp);
                            expression = colExp.GetCodeFromExp(tableRelation);
                            temptype = "Supcan";
                            break;
                        }
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(expression) && !expressions.ContainsKey(expression))
                {
                    expressions.Add(expression, temptype);
                }
            }
        }

        public void ConvertExpression(IList<PbExpressionImp> PbImps, String type)
        {
            ConvertExpression(PbImps, type, null);
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
                        var temp = (PbGridInfo) item;
                        tableRelation.Add(temp.TableName, item.Name);
                    }
                }
            }
            //grids里面的panel
            foreach (var gridPanel in grids)
            {
                foreach (var item in gridPanel.Items)
                {
                    var temp = (PbGridInfo) item;
                    tableRelation.Add(temp.TableName , item.Name);
                }
            }
            
        }

        public void GetColRelation()
        {
            var col = new Col();
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
                    col.Xtype = field.Datatype;
                    if (colRelation.ContainsKey(col.Id))
                        continue;
                    colRelation.Add(col.Id, col);
                }
            }
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

        }

        //为容器内的控件进行布局 region  borderLayout
        public void SetLayout()
        {
            Dictionary<string,object> region = new Dictionary<string, object>();
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

            if (containers.Count > 4)
            {
                //改用绝对布局
                return;
            }

            for (int i = 0; i < containers.Count; i++)
            {
                if (!region.ContainsKey("center"))
                {
                    containers[i].Region = "center";
                    region.Add("center", containers[i]);
                }
                else
                {
                    FieldSet tempSet = region["center"] as FieldSet;
                    if (region["center"] is String)
                    {
                        if (region.ContainsKey("center"))
                        {
                            if (!region.ContainsKey("north"))
                            {
                                containers[i].Region = "north";
                                region.Add("north", containers[i]);
                                continue;
                            }
                            else
                            {
                                containers[i].Region = "south";
                                region.Add("south",containers[i]);
                                continue;
                            }
                        } 
                    }

                    //起点相同，或者终点相同即为该方向上一致
                    if (Math.Abs(tempSet.X - containers[i].X) < 10 ||
                        Math.Abs(tempSet.X + tempSet.Width - containers[i].X - containers[i].Width) < 10)
                    {//说明不是左右的关系

                        if (Math.Abs(tempSet.Y - containers[i].Y) < 10 ||
                            Math.Abs(tempSet.Y + tempSet.Height - containers[i].Y - containers[i].Height) < 10)
                        {// 且不是上下的关系
                         //对角线关系，先不处理
                            //或者调用绝对布局
                        }
                        else
                        {// 是上下关系
                            if (containers[i].Y < tempSet.Y)
                            {
                                containers[i].Region = "north";
                                region.Add("north", containers[i]);
                            }
                            else
                            {
                                containers[i].Region = "center";
                                tempSet.Region = "north";
                                if (region.ContainsKey("north"))
                                {
                                    //调用绝对布局
                                    break;
                                }
                                region.Add("north", region["center"]);
                                region["center"] = containers[i];
                            }
                        }
                    }
                    else if (Math.Abs(tempSet.Y - containers[i].Y) < 10 ||
                            Math.Abs(tempSet.Y + tempSet.Height - containers[i].Y - containers[i].Height) < 10)
                    {//不是上下关系
                        if (Math.Abs(tempSet.X - containers[i].X) < 10 ||
                            Math.Abs(tempSet.X + tempSet.Width - containers[i].X - containers[i].Width) < 10)
                        {//不是左右关系
                            //对角线关系，先不处理
                        }
                        else
                        {//是左右关系
                            if (containers[i].X < tempSet.X)
                            {
                                containers[i].Region = "west";
                                region.Add("west", containers[i]);
                            }
                            else
                            {
                                containers[i].Region = "center";
                                tempSet.Region = "west";
                                if (region.ContainsKey("west"))
                                {
                                    //调用绝对布局
                                    break;
                                }
                                region.Add("west", region["center"]);
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

        // 将控件放到指定的容器内，
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

            //若没有容器  //单grid的情况 或者是 tableLayout
            if (containers.Count == 0)
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
                return;
            }
            ExtContainer targetContain = new ExtContainer();

            int i = 0;
            do
            {
                targetContain.X = containers[i].X;
                targetContain.Y = containers[i].Y;
                targetContain.Width = containers[i].Width;
                targetContain.Height = containers[i].Height;

                if (x >= targetContain.X && x <= targetContain.X + targetContain.Width
                    && y >= targetContain.Y && y <= targetContain.Y + targetContain.Height)
                {
                    containers[i].Items.Add(control);
                    break;
                }
                i++;
            } while (i < containers.Count);

            //if (i == containers.Count)
            //{
            //    if (control is PbGridInfo)
            //    {
            //        var tempPanel = control as PbGridInfo;
            //        grids.Add(ControlConverter.ConvertToExtPanel(tempPanel));
            //    }
            //    else
            //    {
            //        layoutForm.Items.Add(control);
            //    }
            //}
            

        }

        // 计算span
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
                    if (item.Value.Name == "ocode"||item.Value.Name == "code")
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
 
        // 计算span
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
                pbColumn.Span  = Convert.ToInt32(Math.Ceiling((double)pbColumn.Width / ((double)1.5*sortSet.Min)));
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

        //排序
        private List<PbBaseControlInfo> Sort(List<PbBaseControlInfo> PbColumns, ref Dictionary<int, SortedList<int, PbBaseControlInfo>> dic)
        {
            SortedSet<int> sortset = new SortedSet<int>();
            foreach (PbBaseControlInfo control in PbColumns)
            {
                sortset.Add(control.YPos);
            }

            //行归类
            //Dictionary<int, SortedList<int, PbBaseControlInfo>> dic = new Dictionary<int, SortedList<int, PbBaseControlInfo>>();
            foreach (int item in sortset)
            {
                SortedList<int, PbBaseControlInfo> list = new SortedList<int, PbBaseControlInfo>();
                dic.Add(item, list);
            }

            //计算控件属于哪一行
            foreach (PbBaseControlInfo control in PbColumns)
            {
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

        //获取toolbar
        public Toolbar GetEditToolbar(PbBillInfo billInfo)
        {
            var toolbar = new Toolbar();
            string btname = string.Empty;
            foreach (var buttons in billInfo.DetailToolbarInfo.ToolbarButtonGroupInfosInfos)
            {
                if (buttons == null)
                    continue;
                foreach (var button in buttons.ToolbarButtonInfos)
                {
                    if (button.IsDockLeft)
                    {
                        btname = ButtonNameConvert.ConvertToExtTpye(button);
                        if (toolbar.LNgButtons.Contains(btname))
                            continue;
                        if (grids.Count == 0 && btname.Equals("save"))
                        {
                            toolbar.LNgButtons.Add(btname);
                            break;
                        }
                        else
                        {
                            toolbar.LNgButtons.Add(btname);
                        }
                    }
                    else
                    {
                        btname = ButtonNameConvert.ConvertToExtTpye(button);
                        if (toolbar.RNgButtons.Contains(btname))
                            continue;
                        toolbar.RNgButtons.Add(btname);
                    }
                }
            }

            return toolbar;
        }

        public List<String> GetTabInfos(PbBillInfo billInfo)
        {
            foreach (var tabinfo in billInfo.PbTabInfos)
            {
                foreach (var gridid in tabinfo.GridIds)
                {
                    tabinfos.Add(gridid);
                }
            }
            return tabinfos;
        } 
    }

    //public struct Col
    //{
    //    public string Id;
    //    public string Name;
    //    public string Xtype;
    //}
}
