using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Right.Rules
{
   public class PageRight
   {
       /// <summary>
       /// 获取界面上没用权限的按钮
       /// </summary>
       /// <param name="UCode">账套</param>
       /// <param name="organizeId">组织</param>
       /// <param name="userId">账户</param>
       /// <param name="userType"></param>
       /// <param name="pageName">界面</param>
       /// <param name="FuncName">功能按钮</param>
       /// <returns></returns>
       public string GetNonFormRightsItems(string UCode, Int64 orgID, Int64 userID, string userType, string pageName, string FuncName, string connStr)
       {
           return new Services().GetNonFormRightsItems(UCode, orgID, userID, userType, pageName, FuncName,connStr);
       }
    }
}
