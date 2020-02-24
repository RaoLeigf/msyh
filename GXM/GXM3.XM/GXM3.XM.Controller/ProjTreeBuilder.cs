using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GXM3.XM.Controller
{
    class ProjTreeBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            ProjTreeJSONBase node = new ProjTreeJSONBase();

            node.PhId = Int64.Parse(dr["PhId"].ToString());
            node.id = dr["DM"].ToString();//注意，这里一定得用表示树id的那个字段赋值才行
            node.text = dr["MC"].ToString();
            //node.hrefTarget = dr["url"].ToString();
            node.leaf = (dr["DEFSTR1"].ToString() == "1") ? true : false;

            node.curentCode = dr["DM"].ToString();
            node.parentCode = dr["DEFSTR2"].ToString();
            node.isProject = dr["DEFSTR1"].ToString();

            return node;
        }


    }
}
