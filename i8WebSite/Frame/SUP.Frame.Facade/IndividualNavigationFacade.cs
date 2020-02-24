using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.Rule;
using NG3;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public class IndividualNavigationFacade : IIndividualNavigationFacade
    {
        private IndividualNavigationRule rule = new IndividualNavigationRule();
        [DBControl]
        public DataTable LoadTree()
        {
            return rule.LoadTree();
        }

        [DBControl]
        public string Load(string text)
        {
            return rule.Load(text);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public string Save(string svgConfig, string text, string saveType)
        {
            return rule.Save(svgConfig,text, saveType);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool Delete(string text)
        {
            return rule.Delete(text);
        }
    }
}
