using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    public class PrintEntity
    {
        public Dictionary<string, string> getMoudleNos()
        {
            Dictionary<string, string> moudleNos = new Dictionary<string, string>();
            moudleNos.Add("HR", "人力资源管理");
            moudleNos.Add("WM", "协同办公");
            moudleNos.Add("EPM", "企业绩效管理");
            moudleNos.Add("PUB", "公共团队");
            moudleNos.Add("ASSP", "售后门户");
            moudleNos.Add("OTHER", "其他");
            return moudleNos;
        }
    }
}
