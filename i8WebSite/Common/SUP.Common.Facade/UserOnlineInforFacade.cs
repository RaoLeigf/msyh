using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SUP.Common.Rule;

using NG3.Data.Service;

namespace GE.BusinessFacade.Common
{
   public class UserOnlineInforFacade
    {

       private UserOnlineInfor instance = UserOnlineInfor.Instance;

       /// <summary>
       /// 构造函数
       /// </summary>
       public UserOnlineInforFacade()
       {

       }

       /// <summary>
       /// 重启缓存服务后重新添加登录信息
       /// </summary>
       /// <param name="product"></param>
       /// <param name="suitInfo"></param>
       /// <param name="mac"></param>      
       public void AddLogInfoIfLost(string product, string suitInfo, string mac)
       {
           instance.AddLogInfoIfLost(product, suitInfo, mac);
       }

       /// <summary>
       /// 登录检测
       /// </summary>
       /// <param name="product">产品(w3,i6,GE)</param>
       /// <param name="suitInfo">套件,采用i6Hr,i6WM,..格式,用逗号分开,i6需要传，其他产品请传空</param>
       /// <returns></returns>      
       public bool CheckValidUser(string product, string suitInfo)
       {
           return instance.CheckValidUser(product, suitInfo);
       }

        /// <summary>
        /// 生成存放登录用户信息列表，从Hashtable生成
        /// </summary>
        /// <returns></returns>       
       public DataTable GetOnlineUserDt()
       {
           return instance.GetDt();
       }
       
      
         /// <summary>
        /// 移除登录信息
        /// </summary>
        /// <param name="sSessionID"></param>
        /// <returns></returns>       
       public bool RemoveLoginUser(string sSessionID)
       {
           return instance.RemoveLoginUser(sSessionID);
       }

       /// <summary>
       /// 清除在线用户
       /// </summary>
       /// <param name="sSessionID"></param>       
       public void KillLoginUser(string sSessionID)
       {
           instance.KillLoginUser(sSessionID);
       }

       /// <summary>
       /// 刷新在线用户列表,如果session还存在,会自动修复在线列表
       /// </summary>      
       public string Refresh()
       {
           return instance.Refresh("");
       }

       /// <summary>
       /// 写入退出时间
       /// </summary>
       /// <returns></returns>       
       public int WriteLogOutDt(string code)
       {
           int iret = -1;

           //try
           //{
           //    DbHelper.Open();
           //    DbHelper.BeginTran();
           //    iret = new UserDac().WriteLogOutDt(code);
           //    DbHelper.CommitTran();
           //}
           //catch (Exception ex)
           //{
           //    DbHelper.RollbackTran();
           //    throw;
           //}
           //finally
           //{
           //    DbHelper.Close();
           //}

           return iret;
       }


       /// <summary>
       /// 取得登陆历史记录
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>      
       public DataTable GetLogHistory(string where)
       {
           DataTable dt = null;
           //try
           //{
           //    DbHelper.Open();
           //    dt = new UserDac().GetLogHistory(where);
           //}
           //catch (Exception ex)
           //{
           //    throw;
           //}
           //finally
           //{
           //    DbHelper.Close();
           //}
           return dt;
       }

      
        /// <summary>
        /// 分页取数方法
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="totalrowCount">总行数</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>      
       public DataTable GetLoginHistoryList(int pageSize, int pageIndex, int totalRowCount, string where)
       {
           DataTable dt = null;
           //try
           //{
           //    DbHelper.Open();
           //    dt = new UserDac().GetLoginHistoryList(pageSize, pageIndex, totalRowCount, where);
           //}
           //catch (Exception ex)
           //{
           //    throw;
           //}
           //finally
           //{
           //    DbHelper.Close();
           //}
           return dt;
       }

        /// <summary>
        /// 取总行数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>       
       public int GetAllRowsCount(string where)
       {
           int iret = 0;
           //try
           //{
           //    DbHelper.Open();
           //    iret = new UserDac().GetAllRowsCount(where);
           //}
           //catch (Exception ex)
           //{               
           //    throw;
           //}
           //finally
           //{
           //    DbHelper.Close();
           //}
           return iret;
       }

       /// <summary>
       /// 获取在线人数
       /// </summary>
       /// <returns></returns>       
       public int GetOnlineUserCount()
       {
           return UserOnlineInfor.Instance.GetOnlineUserCount();
       }

       /// <summary>
       /// 移除登录用户信息
       /// </summary>      
       public bool RemoveUser()
       {
           bool flg = instance.RemoveLoginUser();
           System.Web.HttpContext.Current.Session.RemoveAll();
           return flg;
       }

       /// <summary>
       /// 移除登录用户信息
       /// </summary>       
       public void SetOaUser()
       {
           UserOnlineInfor.Instance.SetOaUser();
       }

   }
}
