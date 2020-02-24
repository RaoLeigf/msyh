using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Controls;
using SUP.CustomForm.DataEntity.Control;
using SUP.CustomForm.DataEntity.AppControl;

namespace SUP.CustomForm.DataEntity.AppContainer
{
    public class AppFormPanel: AppContainer
    {
        private List<PbBaseControlInfo> items = new List<PbBaseControlInfo>();
        private List<BaseField> fields = new List<BaseField>();

        public AppFormPanel()
        { 
        }

        
        /// <summary>
        /// 转换后，容器的子元素
        /// </summary>
        public List<BaseField> Fields
        {
            get { return fields; }
            set { fields = value; }
        }


    }
}
