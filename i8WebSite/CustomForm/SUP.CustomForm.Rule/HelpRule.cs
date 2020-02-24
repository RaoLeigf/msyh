using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataEntity;
using SUP.CustomForm.DataAccess;

namespace SUP.CustomForm.Rule
{
    public class HelpRule
    {
        private HelpDac dac = new HelpDac();

        public HelpRule()
        {
            
        }

        /// <summary>
        /// 批量代码转名称;
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list)
        {
            foreach (HelpValueNameEntity item in list)
            {
                item.Name = dac.GetName(item.HelpID, item.Code, item.SelectMode);
            }

            return list.ToArray<HelpValueNameEntity>();
        }
    }
}
