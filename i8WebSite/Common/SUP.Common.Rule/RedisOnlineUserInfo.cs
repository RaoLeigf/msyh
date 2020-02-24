using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;
using NG3;
using NG3.Data.Service;
using NG3.RedisCache.Client;

namespace SUP.Common.Rule
{
    public class RedisOnlineUserInfo
    {
        private static NG3.Cache.Interface.IRedisCache _redisClient = null;
        private static ROnlineTimer _timer;
        private const string ONLINEUSERKEY = "ONLINEUSER";
        private const int MAXUSERS = 1000;

        private static RedisOnlineUserInfo _instance = null;
        private static object _syncLock = new object();

        public RedisOnlineUserInfo()
        {
            if (_redisClient == null)
            {
                _redisClient = new RedisCache();
            }

            if (_timer == null)
            {
                _timer = new ROnlineTimer();
                _timer.Start();
            }
        }

        public static RedisOnlineUserInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        _instance = new RedisOnlineUserInfo();
                    }
                }

                return _instance;
            }
        }

        #region 在线检测;

        /// <summary>
        /// 当前用户是否在线;
        /// </summary>
        /// <returns></returns>
        public bool IsOnline()
        {
            return CurrentOnlineUserInfo() != null;
        }

        public OnlineUserInfo CurrentOnlineUserInfo()
        {
            return (OnlineUserInfo)GetOnlineUserInfo(RCurrentSessionInfo.UniqueClientId);
        }

        #endregion 在线检测;

        #region 用户登录,登记上线;

        /// <summary>
        /// 用户登记上线;
        /// </summary>
        /// <returns></returns>
        public bool MarkLoginUserOnline()
        {
            var mutex = GetMutex();
            try
            {
                mutex.WaitOne(15000);// 超时等待时间;
                int iOnlineUserCount = GetOnlineUserCount();
                if (iOnlineUserCount < MAXUSERS)
                {
                    var info = new OnlineUserInfo
                    {
                        SessionId = RCurrentSessionInfo.SessionId,
                        LogId = RCurrentSessionInfo.LoginId,
                        LogName = RCurrentSessionInfo.LoginName,
                        Product = "",
                        LoginTime = DateTime.Now,
                        LastRefTime = DateTime.Now,
                        IsKilled = false,
                    };

                    // 同一用户在多客户端登录时，将其他客户端登录的用户标记为强制下线;
                    IEnumerable<string> sessionIdArray = this.GetUserSeesionId(info.LogId);
                    foreach (var s in sessionIdArray)
                    {
                        var killedInfo = GetOnlineUserInfo(s) as OnlineUserInfo;
                        if (killedInfo != null)
                        {
                            // 将该用户信息标记为Killed强制下线;
                            this.KilledOnlineUser(killedInfo.UniqueClientId);
                        }
                    }

                    SaveOnlineUserInfoToCache(info.UniqueClientId, info);

                    return true;
                }
                else
                {
                    throw new Exception("最大用户人数上线");
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OnlineUser.MarkUserOnline", e.Message);
                throw;
            }
            finally
            {
                mutex.Close();
            }
        }

        /// <summary>
        /// 获取登录用户所有在线客户端sessionid;
        /// </summary>
        /// <param name="logid">登录id;</param>
        /// <returns></returns>
        private IEnumerable<string> GetUserSeesionId(string logid)
        {
            Hashtable data = GetAllOnlineUser();
            IList<string> idArray = new List<string>();
            IDictionaryEnumerator myEnumerator = data.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if (myEnumerator.Current == null) continue;

                var userInfo = myEnumerator.Value as OnlineUserInfo;

                if (userInfo == null) continue;

                if (userInfo.IsKilled) continue;

                if (userInfo.LogId == logid)
                {
                    idArray.Add(userInfo.UniqueClientId);
                }
            }

            return idArray.ToArray();
        }

        /// <summary>
        /// 获得Mutex;
        /// </summary>
        /// <returns></returns>
        private static Mutex GetMutex()
        {
            bool createdNew;
            var mutex = new Mutex(false, "NG3_NNSERVERMAXUSERCONTROLKEY", out createdNew);
            if (!createdNew)
            {
                mutex = Mutex.OpenExisting("NG3_NNSERVERMAXUSERCONTROLKEY");
            }
            return mutex;
        }

        #endregion 用户登录,登记上线;

        #region 用户下线;

        /// <summary>
        /// 移除当前登录用户信息;
        /// </summary>
        /// <returns></returns>
        public bool RemoveLoginUser()
        {
            lock (ObjLock)
            {
                OnlineUserInfo info = CurrentOnlineUserInfo();

                if (info != null)
                {
                    RemoveOnlineUserInfo(info.UniqueClientId);
                    return true;
                }
                return false;
            }
        }

        public bool RemoveAllOnlineUser()
        {
            lock (ObjLock)
            {
                RemoveAllOnlineUserInfo();
                return true;
            }
        }

        public void KilledOnlineUser(string id)
        {
            lock (ObjLock)
            {
                var info = GetOnlineUserInfo(id) as OnlineUserInfo;
                if (info != null)
                {
                    info.IsKilled = true;
                }

                UpdateOnlineUserInfoToCache(id, info);
            }
        }

        #endregion

        #region 当前在线;

        /// <summary>
        /// 获取在线人数;
        /// </summary>
        /// <returns></returns>
        public int GetOnlineUserCount()
        {
            var hash = (Hashtable)GetAllOnlineUser();
            return hash.Count;
        }

        /// <summary>
        /// 获得全部在线用户;
        /// </summary>
        /// <returns></returns>
        public Hashtable GetAllOnlineUserInfo()
        {
            return GetAllOnlineUser();
        }

        #endregion

        #region 刷新;

        public void RefreshCurrent()
        {
            var currentInfo = CurrentOnlineUserInfo();
            if (currentInfo != null)
            {
                currentInfo.LastRefTime = DateTime.Now;
                this.UpdateOnlineUserInfoToCache(currentInfo.UniqueClientId, currentInfo);
            }
        }

        private static readonly object ObjLock = new object();
        private OnlineUserInfo _lInfo;
        public void RefreshAll()
        {
            Hashtable dt = this.GetAllOnlineUserInfo();

            lock (ObjLock)
            {
                IDictionaryEnumerator myEnumerator = dt.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    _lInfo = (OnlineUserInfo)myEnumerator.Value;
                    if (DateTime.Now > _lInfo.LastRefTime.AddMinutes(5) || _lInfo.IsKilled)// 将超时或强制剔除的用户移除;
                    {
                        RemoveOnlineUserInfo(_lInfo.UniqueClientId);
                    }
                }
            }
        }

        #endregion 刷新;

        #region cache操作;

        /// <summary>
        /// 保存在线用户到缓存;
        /// </summary>
        /// <param name="subKey"></param>
        /// <param name="obj"></param>
        private void SaveOnlineUserInfoToCache(string subKey, object obj)
        {
            _redisClient.HSet(ONLINEUSERKEY, subKey, obj);
        }

        /// <summary>
        /// 获取全部在线用户;
        /// </summary>
        /// <returns></returns>
        private Hashtable GetAllOnlineUser()
        {
            return _redisClient.HGetAll(ONLINEUSERKEY);
        }

        /// <summary>
        /// 更新当前在线用户信息;
        /// </summary>
        /// <param name="subKey"></param>
        /// <param name="obj"></param>
        private void UpdateOnlineUserInfoToCache(string subKey, object obj)
        {
            SaveOnlineUserInfoToCache(subKey, obj);
        }

        /// <summary>
        /// 获取当前在线用户信息;
        /// </summary>
        /// <param name="subKey"></param>
        /// <returns></returns>
        private object GetOnlineUserInfo(string subKey)
        {
            return _redisClient.HGet(ONLINEUSERKEY, subKey);
        }

        /// <summary>
        /// 移除当前在线用户信息;
        /// </summary>
        /// <param name="subKey"></param>
        private void RemoveOnlineUserInfo(string subKey)
        {
            _redisClient.HRemove(ONLINEUSERKEY, subKey);
        }

        /// <summary>
        /// 清除缓存中全部在线用户信息;
        /// </summary>
        private void RemoveAllOnlineUserInfo()
        {
            _redisClient.HRemoveHash(ONLINEUSERKEY);
        }

        #endregion cache操作;
    }

    public class ROnlineTimer
    {
        private System.Timers.Timer _time;

        public void Start()
        {
            if (_time == null)
            {
                _time = new System.Timers.Timer { Interval = 5000 };
                _time.Elapsed += Time_Elapsed;
            }

            _time.Start();
        }

        public void Stop()
        {
            _time.Stop();
            _time.Close();
            _time.Dispose();
        }

        protected void Time_Elapsed(object obj, ElapsedEventArgs e)
        {
            RedisOnlineUserInfo.Instance.RefreshAll();
        }
    }

    [Serializable]
    public class OnlineUserInfo
    {
        public string SessionId { get; set; }
        public string Product { get; set; }
        public string LogId { get; set; }
        public string LogName { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LastRefTime { get; set; }
        public bool IsKilled { get; set; }
        public string UniqueClientId { get { return this.SessionId + this.LogId; } }
    }

    public class RCurrentSessionInfo
    {
        public static string SessionId
        {
            get
            {
                return CheckThrowException(HttpContext.Current.Session.SessionID);	//Session ID;
            }
        }

        public static string IpAddr
        {
            get
            {
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
        /// 当前登录用户ID;
        /// </summary>
        public static string LoginId
        {
            get
            {
                return AppInfoBase.LoginID;
            }
        }

        /// <summary>
        /// 当前登录用户名称;
        /// </summary>
        public static string LoginName
        {
            get
            {
                return AppInfoBase.UserName;
            }
        }

        /// <summary>
        /// 当前登录帐套号;
        /// </summary>
        public static string LoginUCode
        {
            get
            {
                return AppInfoBase.UCode;
            }
        }

        /// <summary>
        /// 当前登录帐套名称;
        /// </summary>
        public static string LoginUName
        {
            get
            {
                return AppInfoBase.UName;
            }
        }

        /// <summary>
        /// 当前登录组织代码;
        /// </summary>
        public static string LoginOCode
        {
            get
            {
                return AppInfoBase.OCode;
            }
        }

        /// <summary>
        /// 当前登录组织名称;
        /// </summary>
        public static string LoginOName
        {
            get
            {
                return AppInfoBase.OrgName;
            }
        }

        /// <summary>
        /// 操作员所属部门;
        /// </summary>
        public static string DeptNo
        {
            get
            {
                try
                {
                    DbHelper.Open();
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
        /// 标识唯一客户端;
        /// </summary>
        public static string UniqueClientId
        {
            get { return SessionId + LoginId; }
        }

        /// <summary>
        /// 检测当前值是否为null ,为空则抛出异常,否则返回;
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        private static string CheckThrowException(string sValue)
        {
            if (sValue == null || sValue.Trim().Length == 0)
            {
                //throw new Exception("当前会话信息丢失,需要重新登录!");
                throw new Exception("");
            }

            return sValue;
        }
    }
}
