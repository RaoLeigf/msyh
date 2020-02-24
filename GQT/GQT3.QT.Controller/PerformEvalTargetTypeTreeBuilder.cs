using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Controller
{
    class PerformEvalTargetTypeTreeBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            PerformEvalTargetTypeTreeJSONBsae node = new PerformEvalTargetTypeTreeJSONBsae();

            node.PhId = Int64.Parse(dr["PhId"].ToString());
            node.id = dr["FCode"].ToString();//注意，这里一定得用表示树id的那个字段赋值才行
            node.text = dr["FName"].ToString();
            //node.hrefTarget = dr["url"].ToString();
            //node.leaf = (dr["DEFSTR1"].ToString() == "1") ? true : false;

            node.curentCode = dr["FCode"].ToString();
            node.parentCode = dr["FParentCode"].ToString();
            //node.isProject = dr["DEFSTR1"].ToString();

            return node;
        }
    }
}
