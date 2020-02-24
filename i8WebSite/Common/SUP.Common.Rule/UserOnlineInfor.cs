using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Threading;
//using i6;
using System.Configuration;
//using i6.Biz.DMC;

using System.Diagnostics;
using System.Text.RegularExpressions;
//using GE.DataEntity.Common;
//using GE.DataAccess.Common;
//using NG.UP.DataServices;

using NG3.Data.Service;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using NG3.Cache.Client;
using NG3.Runtime.Serialization;
using System.IO;
using NG3;

namespace SUP.Common.Rule
{

    /// <summary>
    /// UserOnlineInfor 的摘要说明 ：用户在线信息界面处理。
    /// </summary>
    public class UserOnlineInfor
    {
        #region 字段
        private const string ONLINEINFO_KEY_INSESSION = "__OnLineInfoInSession";
        private const string INFO_KEY = "HSLoginInfo";
        private const string ONLINEUSERINFO_KEY = "ONLINEUSERINFO_KEY";
        private const string NSSERVER_MAX_USERS_INFO = "NSSERVER_MAX_USERS_INFO";
        private static DateTime lastVisitWCacheTime = DateTime.MinValue;

        //使用单件(singleton)设计模式
        private static UserOnlineInfor instance = null;
        private static object objLock = new object();
        private static object syncLock = new object();

        private static OnlineTimer onlineTimer;
        private string _lastError = string.Empty;

        private Hashtable HSLoginInfo = Hashtable.Synchronized(new Hashtable());

        //增加被强制注销用户数据保存 caodz 2008-11-25
        private Hashtable HSKillInfo = Hashtable.Synchronized(new Hashtable());

        private CacheClient cacheClient = null;//状态缓存服务器


        #region 属性

        /// <summary>
        /// 错误信息
        /// </summary>
        public string LastErrorText
        {
            get
            {
                return _lastError;
            }
        }

        #endregion

        #endregion

        #region  构造 取得当前会话的相关信息

        public UserOnlineInfor()
        {
            //LinkToCacheServer();
                       
            cacheClient = CacheClientFactory.GetCacheClient(NG3.Cache.Client.CacheType.StateCache);

            if (onlineTimer == null)
            {
                onlineTimer = new OnlineTimer();
                onlineTimer.Start();
            }
        }

        public static UserOnlineInfor Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        instance = new UserOnlineInfor();
                    }
                }

                return instance;
            }
        }

        ~UserOnlineInfor()
        {
            if (onlineTimer != null) onlineTimer.Stop();
        }
        #endregion

        #region 其他方法

        private DataTable CreateInfo()
        {
            DataTable dtLoginInfo = new DataTable("LoginInfo");
            dtLoginInfo.Columns.Add("SessionID", System.Type.GetType("System.String"));				//Session ID
            dtLoginInfo.Columns.Add("IPAddress", System.Type.GetType("System.String"));				//IP地址
            dtLoginInfo.Columns.Add("MacAddress", System.Type.GetType("System.String"));				//IP地址
            dtLoginInfo.Columns.Add("CompomentID", System.Type.GetType("System.String"));			//组件名W3  
            dtLoginInfo.Columns.Add("UserID", System.Type.GetType("System.String"));					//登录用ID  
            dtLoginInfo.Columns.Add("UserName", System.Type.GetType("System.String"));				//登录用户名
            dtLoginInfo.Columns.Add("LoginTime", System.Type.GetType("System.String"));				//登录时间
            dtLoginInfo.Columns.Add("LastRefTime", System.Type.GetType("System.String"));			//最后刷新时间
            dtLoginInfo.Columns.Add("RefStep", System.Type.GetType("System.String"));				//最长不活动时间

            dtLoginInfo.Columns.Add("LoginSuitInfo", System.Type.GetType("System.String"));				//最长不活动时间

            dtLoginInfo.Columns.Add("UCode", System.Type.GetType("System.String"));
            dtLoginInfo.Columns.Add("UName", System.Type.GetType("System.String"));
            dtLoginInfo.Columns.Add("OCode", System.Type.GetType("System.String"));
            dtLoginInfo.Columns.Add("OrgName", System.Type.GetType("System.String"));
            dtLoginInfo.Columns.Add("deptno", System.Type.GetType("System.String"));
            dtLoginInfo.Columns.Add("deptname", System.Type.GetType("System.String"));
            dtLoginInfo.Columns.Add("isoauser", System.Type.GetType("System.String"));

            dtLoginInfo.Columns.Add("UniqueClientID", System.Type.GetType("System.String"));//标识客户端id，sessionid+logid

            return dtLoginInfo;
        }

        //public bool IsCacheAvailable()
        //{
        //    return WCache.isAvailable(ref _lastError);
        //}

        //private void LinkToCacheServer()
        //{
        //    //检测cacheserver的配置
        //    if (IsCacheAvailable())
        //    {
        //        SyncHSLoginInfoWithCacheServer(0);
        //    }
        //    else
        //    {
        //        _lastError = "Cache server throw an exception!";
        //    }
        //}

        //private void SyncHSLoginInfoWithCacheServer()
        //{
        //    SyncHSLoginInfoWithCacheServer(10);
        //}

        ///// <summary>
        ///// 获取seconds内最新的缓存,seconds主要是为了减少通信次数
        ///// </summary>
        ///// <param name="seconds"></param>
        //private void SyncHSLoginInfoWithCacheServer(int seconds)
        //{
        //    if (IsCacheAvailable())
        //    {
        //        if (DateTime.Now > lastVisitWCacheTime.AddSeconds(seconds))
        //        {
        //            lastVisitWCacheTime = DateTime.Now;
        //            Hashtable tmp = WCache.Get(INFO_KEY) as Hashtable;
        //            if (tmp == null)
        //            {
        //                WCache.Set(INFO_KEY, HSLoginInfo);
        //            }
        //            else
        //            {
        //                HSLoginInfo = tmp;
        //            }
        //        }
        //    }
        //}

        //private void HSLoginInfoUpdate(string key, object value)
        //{
        //    if (IsCacheAvailable())
        //    {
        //        WCache.Set(INFO_KEY, key, value);
        //    }
        //}

        //private void HSLoginInfoRemove(string key)
        //{
        //    if (IsCacheAvailable())
        //    {
        //        WCache.Remove(INFO_KEY, key);
        //    }
        //}

        //private OnlineInfo HSLoginInfoGet(string key)
        //{
        //    if (IsCacheAvailable())
        //    {
        //        HSLoginInfo[key] = WCache.Get(INFO_KEY, key);
        //    }

        //    return HSLoginInfo[key] as OnlineInfo;
        //}

        private void BackupOnlineInfoToSession(OnlineInfo info)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[ONLINEINFO_KEY_INSESSION] = info;
            }
        }

        private OnlineInfo RestoreOnlineInfoFromSession()
        {
            OnlineInfo info = null;
            lock (objLock)
            {
                if (HttpContext.Current.Session != null)
                {
                    info = HttpContext.Current.Session[ONLINEINFO_KEY_INSESSION] as OnlineInfo;
                    if (info != null && !info.IsKilled && info.SessionID == CurrentSessionInfo.SessionID)
                    {
                       // HSLoginInfoUpdate(info.SessionID, info.SessionID);
                        HSLoginInfo[info.SessionID] = info;
                    }
                }
            }

            return info;
        }

        #endregion

        #region 首次登录 检测

        /// <summary>
        /// 是否在线
        /// </summary>
        /// <returns></returns>
        public bool IsOnline()
        {
            return CurrentOnlineInfo() != null;
        }

        public bool IsKickOff(string userid)
        {
            if (IsOnline())
            {
                OnlineInfo info = GetOnlineUserInfo(CurrentSessionInfo.SessionID+userid);
                if (info != null)
                {
                    return info.IsKilled;
                }
            }
            return false;
        }

        public OnlineInfo CurrentOnlineInfo()
        {
            //return HSLoginInfoGet(CurrentSessionInfo.SessionID);
            return GetOnlineUserInfo(CurrentSessionInfo.UniqueClientID);
        }

        public bool CheckValidUser(string product, string suitInfo)
        {

            //如果已经登录过,没有清除,则重新计算套件的可登录用户数
            if (this.IsOnline())
            {
                this.RemoveLoginUser();
            }

            return true;
        }

        #endregion

        #region 移除登录信息

        public void KillLoginUser(string id)
        {
            KillLoginUser(id, string.Empty);
        }

        public void KillLoginUser(string id, string logoutMessage)
        {
            
            lock (objLock)
            {
                OnlineInfo lInfo = this.GetOnlineUserInfo(id);

                if (lInfo != null)
                {
                    if (string.IsNullOrEmpty(logoutMessage))
                    {
                        //"当前登录被强制注销，点击确定后将取消当前登录！";
                        logoutMessage = Resource.ForceLogOff;
                    }
                    else
                    {
                        //return "客户端[" + ipAddr + "]采用强制登录，点击确定后将取消当前登录！";
                        logoutMessage = string.Format(Resource.ClientForceLogOff, logoutMessage);
                    }
                    lInfo.KillMe(logoutMessage);                   
                    this.UpdateOnlineUserInfo(id, lInfo);                    
                }
            }
        }

        /// <summary>
        /// 移除登录用户信息
        /// </summary>
        public bool RemoveLoginUser()
        {
            lock (objLock)
            {
                OnlineInfo lInfo = CurrentOnlineInfo();
                if (lInfo != null)
                {
                    this.RemoveLoginUser(lInfo.UniqueClientID);
                    string[] sArr = GetSessionID(CurrentSessionInfo.LoginID);
                    foreach (string s in sArr)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            this.RemoveLoginUser(s);
                        }
                    }
              
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 暴露给web服务查看是否在线
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        public bool IsOnline(string logid,string ucode)
        {
            string[] sArr = GetSessionID(logid, ucode);
            if (sArr.Length > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// 暴露给web服务移除模拟登陆用户
        /// </summary>
        /// <param name="logids"></param>
        public bool RemoveUser(string logid,string ucode)
        {
            string[] sArr = GetSessionID(logid,ucode);
            foreach (string s in sArr)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    this.RemoveLoginUser(s);
                }
            }
            return true;
        }

        /// <summary>
        /// 提供给web服务调用时获取sesionid
        /// </summary>
        /// <param name="logID"></param>
        /// <param name="ucode"></param>
        /// <returns></returns>
        private string[] GetSessionID(string logID, string ucode)
        {
            this.GetALLOnlineUserFromCache();
            List<string> idArray = new List<string>();
            IDictionaryEnumerator myEnumerator = HSLoginInfo.GetEnumerator();
            OnlineInfo lInfo;
            string sessID = string.Empty;

            while (myEnumerator.MoveNext())
            {
                if (myEnumerator.Current == null) continue;

                lInfo = myEnumerator.Value as OnlineInfo;

                if (lInfo == null) continue;

                if (lInfo.IsKilled) continue;

                if (lInfo.LoginID == logID && lInfo.LoginUCode == ucode)//支持同一个logid登录不同的帐套
                {
                    idArray.Add(lInfo.SessionID);
                }
            }

            return idArray.ToArray();
        }

        /// <summary>
        /// 移除登录信息
        /// </summary>
        /// <param name="sSessionID"></param>
        /// <returns></returns>
        public bool RemoveLoginUser(string sSessionID)
        {
            lock (objLock)
            {
                HSLoginInfo.Remove(sSessionID);
                //HSLoginInfoRemove(sSessionID);
                this.RemoveOnlineUserInfo(sSessionID);
            }
            return true;
        }

        /// <summary>
        /// Session 失效时,自动清除当前操作员的登录信息
        /// </summary>
        /// <param name="sSessionID"></param>
        /// <returns></returns>
        public bool RemoveLoginUserEndSessioin(string sSessionID)
        {
            OnlineInfo lInfo = HSLoginInfo[sSessionID] as OnlineInfo;
            if (lInfo != null)
            {
                //if (lInfo.Product.ToLower() == "i6")
                //{
                //    SetUICExpert(UicUrl);
                //}

                this.RemoveLoginUser(sSessionID);
            }
            return true;
        }

        /// <summary>
        /// 得到登录用户和地址所在的会话ID
        /// </summary>
        /// <param name="logID"></param>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        private string[] GetSessionID(string logID)
        {
            //SyncHSLoginInfoWithCacheServer(0);

            this.GetALLOnlineUserFromCache();

            List<string> idArray = new List<string>();
            IDictionaryEnumerator myEnumerator = HSLoginInfo.GetEnumerator();
            OnlineInfo lInfo;
            string sessID = string.Empty;

            while (myEnumerator.MoveNext())
            {
                if (myEnumerator.Current == null) continue;

                lInfo = myEnumerator.Value as OnlineInfo;

                if (lInfo == null) continue;

                if (lInfo.IsKilled) continue;

                if (lInfo.LoginID == logID && lInfo.LoginUCode == CurrentSessionInfo.LoginUCode)//支持同一个logid登录不同的帐套
                {
                    idArray.Add(lInfo.UniqueClientID);
                }
            }

            return idArray.ToArray();
        }


        #endregion

        #region 得到当前的已登录用户记录数 登录用户列表

        /// <summary>
        /// 显示当前已登录的用户数
        /// </summary>
        public int ShowCurrentLogin()
        {
            //SyncHSLoginInfoWithCacheServer();

            int loginCount = 0;

            OnlineInfo lInfo;
            lock (objLock)
            {
                IDictionaryEnumerator myEnumerator = HSLoginInfo.GetEnumerator();

                while (myEnumerator.MoveNext())
                {
                    if (myEnumerator.Current == null) continue;

                    lInfo = myEnumerator.Value as OnlineInfo;

                    if (lInfo == null) continue;

                    if (lInfo.IsKilled) continue;

                    loginCount++;
                }
            }
            return loginCount;
        }

        /// <summary>
        /// 显示当前登录某套件的用户数
        /// </summary>
        public int ShowCurrentSuitLogin(string suitCode)
        {
            //SyncHSLoginInfoWithCacheServer();

            int loginCount = 0;

            lock (objLock)
            {
                OnlineInfo lInfo;
                IDictionaryEnumerator myEnumerator = HSLoginInfo.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    if (myEnumerator.Current == null) continue;

                    lInfo = myEnumerator.Value as OnlineInfo;

                    if (lInfo == null) continue;

                    if (lInfo.IsKilled) continue;

                    if (lInfo.LoginSuitInfo.IndexOf(suitCode) >= 0)
                        loginCount++;
                }
            }
            return loginCount;
        }

        /// <summary>
        /// 重启缓存服务后重新添加登录信息
        /// </summary>
        /// <param name="product"></param>
        /// <param name="suitInfo"></param>
        /// <param name="mac"></param>
        public void AddLogInfoIfLost(string product, string suitInfo, string mac)
        {
            this.GetALLOnlineUserFromCache();
            IDictionaryEnumerator myEnumerator = HSLoginInfo.GetEnumerator();
            OnlineInfo lInfo;
            bool isexist = false;
            while (myEnumerator.MoveNext())
            {
                if (myEnumerator.Current == null) continue;
                lInfo = myEnumerator.Value as OnlineInfo;
                if (lInfo == null) continue;
                if (lInfo.IsKilled) continue;
                if((lInfo.Product==product)&&(lInfo.MacAddr==mac))
                {
                    isexist=true;
                    break;
                }
            }
            if (isexist)
            {
                return;
            }
            else
            {
                lInfo = new OnlineInfo();
                lInfo.UniqueClientID = CurrentSessionInfo.UniqueClientID;
                lInfo.SessionID = CurrentSessionInfo.SessionID;
                lInfo.IPAddr = CurrentSessionInfo.IPAddr;
                lInfo.MacAddr = mac;
                lInfo.Product = product;
                if (AppInfoBase.LoginID == null)//被T的时候有可能客户端还没关闭定时检测
                {
                    return;
                }
                lInfo.LoginID = CurrentSessionInfo.LoginID;
                lInfo.LoginName = CurrentSessionInfo.LoginName;
                lInfo.LoginTime = DateTime.Now;
                lInfo.LastRefTime = DateTime.Now;
                lInfo.LoginUCode = CurrentSessionInfo.LoginUCode;
                lInfo.LoginUName = CurrentSessionInfo.LoginUName;
                lInfo.LoginOCode = CurrentSessionInfo.LoginOCode;
                lInfo.LoginOName = CurrentSessionInfo.LoginOName;
                lInfo.DeptNo = CurrentSessionInfo.DeptNo;
                lInfo.DeptName = CurrentSessionInfo.DeptName;
                if (suitInfo != null && suitInfo.Trim().Length > 0)
                {
                    lInfo.LoginSuitInfo = suitInfo;
                }
                else
                {
                    lInfo.LoginSuitInfo = string.Empty;
                }
                this.SaveOnlineUserToCache(CurrentSessionInfo.UniqueClientID, lInfo);
            }
        }

        /// <summary>
        /// 生成存放登录用户信息列表，从Hashtable生成
        /// </summary>
        /// <returns></returns>
        public DataTable GetDt()
        {
            DataTable dtLoginInfo = this.CreateInfo();

            //SyncHSLoginInfoWithCacheServer();

            lock (objLock)
            {
                this.GetALLOnlineUserFromCache();

                IDictionaryEnumerator myEnumerator = HSLoginInfo.GetEnumerator();
                OnlineInfo lInfo;
                while (myEnumerator.MoveNext())
                {
                    if (myEnumerator.Current == null) continue;

                    lInfo = myEnumerator.Value as OnlineInfo;

                    if (lInfo == null) continue;

                    if (lInfo.IsKilled) continue;

                    DataRow dr = dtLoginInfo.NewRow();
                    dr.BeginEdit();
                    dr["SessionID"] = lInfo.SessionID;				//Session ID
                    dr["IPAddress"] = lInfo.IPAddr;				//IP地址
                    dr["MacAddress"] = lInfo.MacAddr;         //mac地址
                    dr["CompomentID"] = lInfo.Product;			//组件名W3  
                    dr["UserID"] = lInfo.LoginID;					//登录用ID  
                    dr["UserName"] = lInfo.LoginName;				//登录用户名
                    dr["LoginTime"] = lInfo.LoginTime.ToString("yyyy-MM-dd HH:mm:ss");				//登录时间
                    dr["LastRefTime"] = lInfo.LastRefTime.ToString("yyyy-MM-dd HH:mm:ss");			//最后刷新时间
                    dr["RefStep"] = lInfo.RefStep.ToString();				//最长不活动时间
                    dr["LoginSuitInfo"] = lInfo.LoginSuitInfo;
                    dr["UCode"] = lInfo.LoginUCode;
                    dr["UName"] = lInfo.LoginUName;
                    dr["OCode"] = lInfo.LoginOCode;
                    dr["OrgName"] = lInfo.LoginOName;
                    dr["deptno"] = lInfo.DeptNo;
                    dr["deptname"] = lInfo.DeptName;
                    dr["isoauser"] = lInfo.IsOaUser;
                    dr.EndEdit();

                    dtLoginInfo.Rows.Add(dr);
                }
            }

            return dtLoginInfo;
        }

        /// <summary>
        /// 获取在线人数
        /// </summary>
        /// <returns></returns>
        public int GetOnlineUserCount()
        {
            this.GetALLOnlineUserFromCache();
            return HSLoginInfo.Count;
        }

        #endregion

        #region 刷新
        /// <summary>
        /// 刷新在线用户列表,如果session还存在,会自动修复在线列表
        /// </summary>
        /// <param name="refStep">刷新间隔(ms)</param>
        public string Refresh(string refStep)
        {
            //先从当前在线用户中取, 如果取不到 就从Session 自动修复
            OnlineInfo lInfo = CurrentOnlineInfo();//?? RestoreOnlineInfoFromSession();

            lock (objLock)
            {
                if (lInfo != null)
                {
                    if (lInfo.IsKilled)
                    {
                        string msg = lInfo.LogoutMessage;
                        this.RemoveLoginUser(lInfo.SessionID);
                        if (HttpContext.Current.Session != null)
                        {
                            HttpContext.Current.Session.Abandon();
                        }
                        //throw new Exception(msg);
                        return msg;
                    }

                    long refSteptime = 0;
                    if (!long.TryParse(refStep, out refSteptime))
                    {
                        refSteptime = 180000;									//没有设置为3分钟
                    }
                    else if (refSteptime < 180000)
                    {
                        refSteptime = 180000;									//最小为3分钟
                    }

                    //更新信息
                    lInfo.LastRefTime = DateTime.Now;
                    lInfo.RefStep = refSteptime;

                    //将当前用户的登陆信息更新到哈希表
                    //HSLoginInfo[CurrentSessionInfo.SessionID] = lInfo;
                    //HSLoginInfoUpdate(CurrentSessionInfo.SessionID, lInfo);

                    this.SaveOnlineUserToCache(CurrentSessionInfo.UniqueClientID, lInfo);
                }
            }

            return string.Empty;
        }

        public void RefreshAll()
        {
            //SyncHSLoginInfoWithCacheServer(0);

            this.GetALLOnlineUserFromCache();

            OnlineInfo lInfo = null;
            List<string> delList = new List<string>();
            lock (objLock)
            {
                IDictionaryEnumerator myEnumerator = HSLoginInfo.GetEnumerator();
                int oauser = 0;
                while (myEnumerator.MoveNext())
                {
                    if (myEnumerator.Current == null) continue;

                    lInfo = myEnumerator.Value as OnlineInfo;

                    if (lInfo.IsOaUser == "0")
                    {
                        oauser++;
                    }
                    //删除为空的和打上Kill标记并且90分钟以上都没有刷新的
                    if (lInfo == null || (lInfo != null && lInfo.IsKilled && DateTime.Now > lInfo.LastRefTime.AddMinutes(6)))
                    {
                        delList.Add(myEnumerator.Key.ToString());
                        continue;
                    }

                    //如果超时，则剔除该用户  [写死]超时时间为70分钟
                    if (DateTime.Now > lInfo.LastRefTime.AddMinutes(5))
                    {
                        //delList.Add(lInfo.SessionID);
                        delList.Add(lInfo.UniqueClientID);
                    }
                }
                NG3.Cache.Interface.IMemCachedClient memClient = NG3.MemCached.Client.MemCachedClientFactory.GetMemCachedClient();
                memClient.SetSmallObject("NSSERVERALLUSERSINFO_CURRENT", oauser.ToString());
            }

            foreach (string s in delList)
            {
                this.RemoveLoginUser(s);
            }
        }

        #endregion

        #region 登录注册

        /// <summary>
        /// 设置当前用户登录 :用于记录实际登录的用户信息
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns></returns>
        public bool SetLoginUsers(string product)
        {
            return SetLoginUsers(product, string.Empty, string.Empty);
        }


        /// <summary>
        /// 设置登录套件的信息
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="suitInfo">套件,采用i6Hr,i6WM,..格式,用逗号分开</param>
        /// <returns></returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public bool SetLoginUsers(string product, string suitInfo, string mac)
        {
            //UserDac userDac = new UserDac();
            bool lockedSuccess = false;
            //在此检测登录数
            NG3.Cache.Interface.IMemCachedClient memClient = NG3.MemCached.Client.MemCachedClientFactory.GetMemCachedClient();

            Mutex mutex = GetMutex();
                try
                {
                    //lockedSuccess = userDac.LockByDb(ConnectionInfoService.GetSessionConnectString());
                    //memClient.BeginLock("NNSERVERMAXUSERCONTROLKEY");
                    mutex.WaitOne(15000);//15秒超时
                    int currentUsers = GetOnlineUserCount();
                    string maxUsers = "36984";
#if !DEBUG
                maxUsers = memClient.GetSmallObject(NSSERVER_MAX_USERS_INFO) as string;
                if (maxUsers == null)
                {
                    int oauser = new NGCOM().OAUsers;
                    int alluser = new NGCOM().AllUsers;
                    maxUsers = (oauser + alluser).ToString();
                    memClient.SetSmallObject(NSSERVER_MAX_USERS_INFO, maxUsers);
                }
#endif

                    #region
                    string[] idArr = GetSessionID(CurrentSessionInfo.LoginID);
                    foreach (string id in idArr)
                    {
                        OnlineInfo infoTmp = HSLoginInfo[id] as OnlineInfo;
                        if (infoTmp != null)
                        {
                            if (infoTmp.IsOaUser == "0")//当前坑是非OA
                            {
                                string currentAllusers = memClient.GetSmallObject("NSSERVERALLUSERSINFO_CURRENT") as string;
                                if ((!String.IsNullOrEmpty(currentAllusers)) && (currentAllusers != "0"))//非OA当前用户数-1
                                {
                                    currentAllusers = (int.Parse(currentAllusers) - 1).ToString();
                                    memClient.SetSmallObject("NSSERVERALLUSERSINFO_CURRENT", currentAllusers);
                                }
                            }
                        }
                    }
                    #endregion


                    if (currentUsers < int.Parse(maxUsers))
                    {
                        OnlineInfo lInfo = new OnlineInfo();

                        #region 注册登录信息

                        lInfo.UniqueClientID = CurrentSessionInfo.UniqueClientID;
                        lInfo.SessionID = CurrentSessionInfo.SessionID;
                        lInfo.IPAddr = CurrentSessionInfo.IPAddr;
                        lInfo.MacAddr = mac;
                        lInfo.Product = product;
                        lInfo.LoginID = CurrentSessionInfo.LoginID;
                        lInfo.LoginName = CurrentSessionInfo.LoginName;
                        lInfo.LoginTime = DateTime.Now;
                        lInfo.LastRefTime = DateTime.Now;
                        lInfo.LoginUCode = CurrentSessionInfo.LoginUCode;
                        lInfo.LoginUName = CurrentSessionInfo.LoginUName;
                        lInfo.LoginOCode = CurrentSessionInfo.LoginOCode;
                        lInfo.LoginOName = CurrentSessionInfo.LoginOName;
                        lInfo.DeptNo = CurrentSessionInfo.DeptNo;
                        lInfo.DeptName = CurrentSessionInfo.DeptName;

                        if (suitInfo != null && suitInfo.Trim().Length > 0)
                        {
                            lInfo.LoginSuitInfo = suitInfo;
                        }
                        else
                        {
                            lInfo.LoginSuitInfo = string.Empty;
                        }
                        #endregion

                        //SyncHSLoginInfoWithCacheServer(0);
                        this.GetALLOnlineUserFromCache();
                        //踢掉已登录的用户
                        //string[] idArr = GetSessionID(CurrentSessionInfo.LoginID);
                        foreach (string id in idArr)
                        {
                            OnlineInfo infoTmp = HSLoginInfo[id] as OnlineInfo;
                            if (infoTmp != null)
                            {
                                //if (infoTmp.IPAddr == CurrentSessionInfo.IPAddr)
                                //{//自己登陆
                                //    //HSLoginInfo.Remove(id);
                                //    RemoveOnlineUserInfo(id);
                                //}
                                //else
                                //{//强制登陆,踢掉别人
                                //    this.KillLoginUser(id, CurrentSessionInfo.IPAddr);
                                //}
                                //由于远程登录用户无法获取到真实ip，如果根据ip判断会导致不同用户远程同一台机器的时候用相同的用户名登录而不会踢人
                                this.KillLoginUser(id, CurrentSessionInfo.IPAddr);
                            }
                        }
                        //HSLoginInfo[CurrentSessionInfo.SessionID] = lInfo;
                        //存到缓存服务器中
                        this.SaveOnlineUserToCache(CurrentSessionInfo.UniqueClientID, lInfo);
                        //HSLoginInfoUpdate(CurrentSessionInfo.SessionID, lInfo);
                        //写一份OnlineInfo到Session
                        BackupOnlineInfoToSession(lInfo);
                        //memClient.EndLock("NNSERVERMAXUSERCONTROLKEY");
                        return true;
                    }
                    else//超过最大用户数
                    {  
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //if (lockedSuccess)//加锁成功的才进行解锁
                    //{
                    //    userDac.UnLockByDb(ConnectionInfoService.GetSessionConnectString());
                    //}
                    //memClient.EndLock("NNSERVERMAXUSERCONTROLKEY");
                    mutex.ReleaseMutex();
                }
        }

        private static Mutex GetMutex()
        {
            bool createdNew;
            Mutex mutex = new Mutex(false, "NG3_NNSERVERMAXUSERCONTROLKEY", out createdNew);
            if (!createdNew)
            {
                mutex = Mutex.OpenExisting("NG3_NNSERVERMAXUSERCONTROLKEY");
            }
            return mutex;
        }

        /// <summary>
        /// 设置oa用户
        /// </summary>
        public void SetOaUser()
        {
            OnlineInfo info = CurrentOnlineInfo();
            info.IsOaUser = "0";
            this.SaveOnlineUserToCache(CurrentSessionInfo.UniqueClientID, info);
        }

        /// <summary>
        /// 新增登陆记录
        /// </summary>
        /// <returns></returns>
        public string AddLogInHistory()
        {
            string code = string.Empty;
            //LoginHistoryEntity entity = new LoginHistoryEntity();

            //entity.IPAdrr = CurrentSessionInfo.IPAddr;
            //entity.LoginID = CurrentSessionInfo.LoginID;
            //entity.LoginName = CurrentSessionInfo.LoginName;
            //entity.Ucode = CurrentSessionInfo.LoginUCode;
            //entity.Uname = CurrentSessionInfo.LoginUName;
            //entity.OCode = CurrentSessionInfo.LoginOCode;
            //entity.OName = CurrentSessionInfo.LoginOName;
            //entity.DeptNo = CurrentSessionInfo.DeptNo;
            //entity.DeptName = CurrentSessionInfo.DeptName;

            //try
            //{
            //    DbHelper.Open();
            //    DbHelper.BeginTran();
            //    code = new UserDac().AddLogInHistory(entity);
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

            return code;
        }

        /// <summary>
        /// 记住当前操作员的最后登陆的组织
        /// </summary>
        /// <param name="ocode"></param>
        public int SetLastLogInOcode(string logid, string ocode)
        {
            int iret = -1;
            //try
            //{
            //    DbHelper.Open();
            //    DbHelper.BeginTran();
            //    iret = new UserDac().SetLastLogInOcode(logid, ocode);
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

        #endregion

        #region 缓存服务器

        /// <summary>
        /// 保存在线用户信息到缓存
        /// </summary>
        /// <param name="subkey"></param>
        /// <param name="obj"></param>
        private void SaveOnlineUserToCache(string subkey,object obj)
        {
            if (cacheClient.IsUseWebGarden)
            {
                //byte[] b = SerializerBase.Serialize(obj);
                //MemoryStream m = SerializerBase.BinarySerialize(obj);

                cacheClient.Add(ONLINEUSERINFO_KEY, subkey, obj);
            }
            else
            {
                if (HSLoginInfo.ContainsKey(subkey))
                {
                    HSLoginInfo[subkey] = obj;
                }
                else
                {
                    HSLoginInfo.Add(subkey, obj);
                }               
            }
        }

        /// <summary>
        /// 从缓存中取所有的在线用户信息
        /// </summary>
        private void GetALLOnlineUserFromCache()
        {
            if (cacheClient.IsUseWebGarden)
            {
                Hashtable ht = cacheClient.GetData(ONLINEUSERINFO_KEY) as Hashtable;

                if (ht != null)
                {
                    HSLoginInfo = ht;
                }

                // byte[] b = CacheClient.Instance.GetData(ONLINEUSERINFO_KEY) as byte[];

                //if (b != null && b.Length > 0)
                //{
                //    Hashtable ht = SerializerBase.DeSerialize(b) as Hashtable;

                //    if (ht != null)
                //    {
                //        HSLoginInfo = ht;
                //    }
                //}
            }           
        }

        /// <summary>
        /// 更新在线用户信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void UpdateOnlineUserInfo(string key, object value)
        {
            if (cacheClient.IsUseWebGarden)
            {
                cacheClient.Add(ONLINEUSERINFO_KEY, key, value);
            }
            else
            {
                HSLoginInfo[key] = value;
            }
        }

        /// <summary>
        /// 取得某一个在线用户信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private OnlineInfo GetOnlineUserInfo(string key)
        {
            if (cacheClient.IsUseWebGarden)
            {
                HSLoginInfo[key] = cacheClient.GetData(ONLINEUSERINFO_KEY, key);
            }

            return HSLoginInfo[key] as OnlineInfo;
        }

        /// <summary>
        /// 移除某一个用户信息
        /// </summary>
        /// <param name="key"></param>
        private void RemoveOnlineUserInfo(string key)
        {
            if (cacheClient.IsUseWebGarden)
            {
                cacheClient.Remove(ONLINEUSERINFO_KEY, key);
            }
            else
            {
                HSLoginInfo.Remove(key);
            }
        }

        #endregion

    }


    /// <summary>
    /// 记录某个登录的登录相关信息
    /// </summary>
    [Serializable]
    public class OnlineInfo
    {
        private string _sSessionID;
        private string _sIPAddr;
        private string _sMacAddr;
        private string _product;
        private string _loginID;
        private string _loginName;
        private DateTime _loginTime;
        private DateTime _lastRefTime;
        private long _refStep;
        private string _loginUCode;
        private string _loginUName;
        private string _loginOCode;
        private string _loginOName;
        private string _deptno;
        private string _deptname;
        private string _isOaUser = "1";

        private string uniqueClientID;

        #region 剔除用户相关属性
        private bool _isKilled = false;
        private string _logoutMessage = string.Empty;

        public bool IsKilled
        {
            get { return _isKilled; }
        }

        public string LogoutMessage
        {
            get { return _logoutMessage; }
        }
        #endregion

        private string _loginSuitInfo;

        public string SessionID
        {
            get
            {
                return _sSessionID;
            }
            set
            {
                _sSessionID = value;
            }
        }

        public string IPAddr
        {
            get
            {
                return _sIPAddr;
            }
            set
            {
                _sIPAddr = value;
            }
        }

        public string MacAddr
        {
            get
            {
                return _sMacAddr;
            }
            set
            {
                _sMacAddr = value;
            }
        }

        public string Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        public string LoginID
        {
            get
            {
                return _loginID;
            }
            set
            {
                _loginID = value;
            }
        }

        public string LoginName
        {
            get
            {
                return _loginName;
            }
            set
            {
                _loginName = value;
            }
        }

        public DateTime LoginTime
        {
            get
            {
                return _loginTime;
            }
            set
            {
                _loginTime = value;
            }
        }

        public DateTime LastRefTime
        {
            get
            {
                return _lastRefTime;
            }

            set
            {
                _lastRefTime = value;
            }
        }

        public long RefStep
        {
            get
            {
                return _refStep;
            }
            set
            {
                _refStep = value;
            }
        }
        public string LoginUCode
        {
            get
            {
                return _loginUCode;
            }
            set
            {
                _loginUCode = value;
            }
        }

        public string LoginUName
        {
            get
            {
                return _loginUName;
            }
            set
            {
                _loginUName = value;
            }
        }

        public string LoginOCode
        {
            get
            {
                return _loginOCode;
            }
            set
            {
                _loginOCode = value;
            }
        }

        public string LoginOName
        {
            get
            {
                return _loginOName;
            }
            set
            {
                _loginOName = value;
            }
        }

        public string LoginSuitInfo
        {
            get
            {
                return _loginSuitInfo;
            }
            set
            {
                _loginSuitInfo = value;
            }
        }

        public string DeptNo
        {
            get { return _deptno; }
            set { _deptno = value; }
        }

        public string DeptName
        {
            get { return _deptname; }
            set { _deptname = value; }
        }

        public string IsOaUser
        {
            get { return _isOaUser; }
            set { _isOaUser = value; }
        }

        public string UniqueClientID
        {
            get { return uniqueClientID; }
            set { uniqueClientID = value; }
        }

        /// <summary>
        /// 给自己打上删除标记
        /// </summary>
        /// <param name="logoutMessage"></param>
        public void KillMe(string logoutMessage)
        {
            this._isKilled = true;
            this._logoutMessage = logoutMessage;
        }
    }


    public class CurrentSessionInfo
    {
        public static string SessionID
        {
            get
            {
                return CheckThrowException(HttpContext.Current.Session.SessionID);	//Session ID
            }
        }

        public static string IPAddr
        {
            get
            {
                //				string sIPAddr	= HttpContext.Current.Request.UserHostAddress;			//IP Address
                //				if (sIPAddr == "127.0.0.1" || sIPAddr.ToLower() == "localhost")
                //				{
                //					sIPAddr = "Localhost";
                //				}

                NameValueCollection coll;

                coll = HttpContext.Current.Request.ServerVariables;

                string sIPAddr = string.Empty;
                string[] s = coll.GetValues("HTTP_X_FORWARDED_FOR");

                if (s != null && s.Length > 0)
                {
                    sIPAddr = s[0];
                }
                if (sIPAddr == null || sIPAddr.Trim().Length == 0)
                {
                    s = coll.GetValues("REMOTE_ADDR");
                }
                if (s != null && s.Length > 0)
                {
                    sIPAddr = s[0];
                }
                if (sIPAddr == null || sIPAddr.Trim().Length == 0)
                {
                    sIPAddr = HttpContext.Current.Request.UserHostAddress.ToString();
                }
                if (sIPAddr == "127.0.0.1" || sIPAddr.ToLower() == "localhost")
                {
                    sIPAddr = "Localhost";
                }

                return CheckThrowException(sIPAddr);
            }
        }


        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public static string LoginID
        {
            get
            {
                return AppInfoBase.LoginID;
            }
        }

        /// <summary>
        /// 当前登录用户名称
        /// </summary>
        public static string LoginName
        {
            get
            {
                return AppInfoBase.UserName;
            }
        }

        /// <summary>
        /// 当前登录帐套号
        /// </summary>
        public static string LoginUCode
        {
            get
            {
                return AppInfoBase.UCode;
            }
        }

        /// <summary>
        /// 当前登录帐套名称
        /// </summary>
        public static string LoginUName
        {
            get
            {
                return AppInfoBase.UName;
            }
        }

        /// <summary>
        /// 当前登录组织代码
        /// </summary>
        public static string LoginOCode
        {
            get
            {
                return AppInfoBase.OCode;
            }
        }

        /// <summary>
        /// 当前登录组织名称
        /// </summary>
        public static string LoginOName
        {
            get
            {
                return AppInfoBase.OrgName;
            }
        }

        /// <summary>
        /// 操作员所属部门
        /// </summary>
        public static string DeptNo
        {
            get
            {
                try
                {
                    DbHelper.Open();
                    //return new PubCommonDac().GetDeptNo(i6.Biz.i6SessionInfo.LoginID);

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    DbHelper.Close();
                }

            }
        }

        /// <summary>
        /// 操作员所属部门
        /// </summary>
        public static string DeptName
        {
            get
            {
                try
                {
                    DbHelper.Open();
                    //return new PubCommonDac().GetDeptName(i6.Biz.i6SessionInfo.LoginID);
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    DbHelper.Close();
                }
            }
        }

        /// <summary>
        /// 标识唯一客户端
        /// </summary>
        public static string UniqueClientID 
        {
            get { return SessionID + LoginID; }
        }

        /// <summary>
        /// 检测当前值是否为null ,为空则抛出异常,否则返回
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        private static string CheckThrowException(string sValue)
        {
            if (sValue == null || sValue.Trim().Length == 0)
            {
                //throw new Exception("当前会话信息丢失,需要重新登录!");
                throw new Exception(Resource.SessionLost);
            }

            return sValue;
        }
    }


    /// <summary>
    /// 在线用户定时功能
    /// </summary>
    public class OnlineTimer
    {
        private System.Timers.Timer time;
        private DateTime lastRefreshTime = DateTime.MinValue;
        private const string TIMER_LAST_REFRESH_TIME = "OnlineTimerLashRefreshTime";

        public void Start()
        {
            if (time == null)
            {
                time = new System.Timers.Timer();
                time.Interval = 180000;//三分钟

                time.Elapsed += new System.Timers.ElapsedEventHandler(Time_Elapsed);
            }


            time.Start();
        }

        public void Stop()
        {
            time.Stop();
            time.Close();
            time.Dispose();
        }

        protected void Time_Elapsed(object obj, ElapsedEventArgs e)
        {
            //if (UserOnlineInfor.Instance.IsCacheAvailable())
            //{
            //    object tmp = WCache.Get(TIMER_LAST_REFRESH_TIME);
            //    if (tmp is IConvertible)
            //    {
            //        lastRefreshTime = Convert.ToDateTime(tmp);
            //    }

            //    if (DateTime.Now > lastRefreshTime.AddMilliseconds(time.Interval))
            //    {
            //        WCache.Set(TIMER_LAST_REFRESH_TIME, DateTime.Now);
            //        UserOnlineInfor.Instance.RefreshAll();
            //    }
            //}
            //else
            //{
                UserOnlineInfor.Instance.RefreshAll();
            //}
        }
    }

}
