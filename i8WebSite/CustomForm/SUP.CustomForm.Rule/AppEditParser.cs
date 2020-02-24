using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity.Container;
using NG3.Metadata.UI.PowserBuilder;
using SUP.CustomForm.DataEntity.AppContainer;
using NG3.Metadata.UI.PowserBuilder.Controls;

namespace SUP.CustomForm.Rule
{
    public class AppEditParser
    {

        private List<AppFieldSet> fieldSets = new List<AppFieldSet>();
        private AppFormPanel formPanel = new AppFormPanel();

        public AppEditParser(PbBillInfo billInfo)
        {
            SetContainer(billInfo);
            SetFormPanel();
        }

        
        public List<AppFieldSet> FieldSets
        {
            get { return fieldSets; }
            set { fieldSets = value; }
        }

        public AppFormPanel FormPanel
        {
            get { return formPanel; }
            set { formPanel = value; }
        }

        //groupbox转fieldset，没有groupbox转ngFormPanel
        private void SetContainer(PbBillInfo billInfo)
        {

            //含有groupbox表头
            if (billInfo.HeadInfo.PbBaseControlInfos.Count > 0)
            {
                //将容器都先放入containers
                foreach (var headinfo in billInfo.HeadInfo.PbBaseControlInfos)
                {
                    AppFieldSet fieldSet = new AppFieldSet();
                    fieldSet.X = headinfo.XPos;
                    fieldSet.Y = headinfo.YPos;
                    fieldSet.Width = headinfo.Width;
                    fieldSet.Height = headinfo.Height;
                    fieldSet.ID = headinfo.Id;
                    fieldSet.Title = ((PbGroupboxInfo)(headinfo)).Text;
                    fieldSets.Add(fieldSet);
                }
            }

            foreach (var pbColumn in billInfo.HeadInfo.PbColumns)
            {
                //将控件放到指定的容器内
                FindContain(pbColumn, pbColumn.XPos, pbColumn.YPos);
            }

            //因为要赋值回去，所以不能foreach
            for (int i = 0; i < fieldSets.Count; i++)
            {
                Dictionary<int, SortedList<int, PbBaseControlInfo>> dic =
                    new Dictionary<int, SortedList<int, PbBaseControlInfo>>();
                fieldSets[i].Items = this.Sort(fieldSets[i].Items, ref dic);             
            }


            for (int i = 0; i < fieldSets.Count; i++)
            {
                //转类型
                foreach (var item in fieldSets[i].Items)
                {
                    fieldSets[i].AllFields.Add(ControlConverter.ConvertToExtControl(item));                   
                }
            }
 
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

            //若没有容器，放入formpanel
            if (fieldSets.Count == 0)
            {
                formPanel.Items.Add(control);
                return;
            }

            ExtContainer targetContain = new ExtContainer();
            int i = 0;
            do
            {
                targetContain.X = fieldSets[i].X;
                targetContain.Y = fieldSets[i].Y;
                targetContain.Width = fieldSets[i].Width;
                targetContain.Height = fieldSets[i].Height;

                //坐标落入容器里
                if (x >= targetContain.X && x <= targetContain.X + targetContain.Width
                    && y >= targetContain.Y && y <= targetContain.Y + targetContain.Height)
                {
                    fieldSets[i].Items.Add(control);
                    break;
                }
                i++;
            } while (i < fieldSets.Count);


            
        }

        //表单里的控件排序、转换
        public void SetFormPanel()
        {
            Dictionary<int, SortedList<int, PbBaseControlInfo>> dic =
                    new Dictionary<int, SortedList<int, PbBaseControlInfo>>();

            formPanel.Items = this.Sort(formPanel.Items, ref dic);

           
            //转类型
            foreach (var item in formPanel.Items)
            {
                formPanel.Fields.Add(AppControlConverter.ConvertToExtControl(item));
            }
            CommonParser.Log("formPanel转换成功.");            
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
                        item.Value.Add(control.XPos, control);//按x轴排序
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
        public AppToolbar GetEditToolbar(PbBillInfo billInfo)
        {
            var toolbar = new AppToolbar();
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
                        if (toolbar.LButtons.Contains(btname))
                            continue;
                        toolbar.LButtons.Add(btname);
                    }
                    else
                    {
                        btname = ButtonNameConvert.ConvertToExtTpye(button);
                        if (toolbar.RButtons.Contains(btname))
                            continue;
                        toolbar.RButtons.Add(btname);
                    }
                }
            }

            return toolbar;
        }

    }
}
