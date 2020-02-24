using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Data.Service;
using SUP.Right.Rules;

namespace SUP.Right.Facade
{
   public class PageRightFacad
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
           try
           {
               DbHelper.Open();            
               string s = new PageRight().GetNonFormRightsItems(UCode, orgID, userID, userType, pageName, FuncName,connStr);
              
                return s;
           }
           catch (Exception ex)
           {                
               throw ex;
           }
           finally
           {
               DbHelper.Close();
           }
       }
    }
}
