using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    public class RichHelpListArgEntity
    {
       public string Helpid { get; set; }
       public int PageSize { get; set; } 
       public int PageIndex { get; set; }       
       public string[] Keys { get; set; }
       public string Treerefkey { get; set; }
       public string Treesearchkey { get; set; } 
       public string OutJsonQuery { get; set; }
       public string ClientQuery { get; set; }
       public string ClientSqlFilter { get; set; }
       public string QueryPropertyID { get; set; }
       public string QueryPropertyCode { get; set; }
       public bool IsAutoComplete { get; set; }
       //信息权限注册的容器ID
       public string InfoRightUIContainerID { get; set; }
       //业务类型code
       public string BusCode { get; set; }

    }
}
