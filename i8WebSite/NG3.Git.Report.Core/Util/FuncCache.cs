using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Supcan;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Util
{
    public static class FuncCache
    {
        //与计算任务相关的公式计算结果cache
        private static ConcurrentDictionary<string, ConcurrentDictionary<string, FuncInfo>> taskCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, FuncInfo>>();
       
        //基类计算对象cache
        private static ConcurrentDictionary<string, object> commonCache = new ConcurrentDictionary<string, object>();
        //公式计算上下文对象
        private static ConcurrentDictionary<string, FuncContext> contextCache = new ConcurrentDictionary<string, FuncContext>();

        //公式所属目录缓存
        private static ConcurrentDictionary<string, string> loadCatalogCache = new ConcurrentDictionary<string, string>();

        #region 元数据缓存

        //函数元信息缓存
        private static ConcurrentDictionary<string, IList<FuncInfo>> funcInfoMetaDataCacheByCatalog = new ConcurrentDictionary<string, IList<FuncInfo>>();
        
        //函数
        private static ConcurrentDictionary<string, FuncInfo> funcInfoMetaDataCache = new ConcurrentDictionary<string, FuncInfo>();

        //下拉列表缓存
        private static ConcurrentDictionary<string, IList<DropDownList>> dropListMetaCache = new ConcurrentDictionary<string, IList<DropDownList>>();
        //公式反射缓存
        private static ConcurrentDictionary<string, FuncRefInfo> funcRefInfoMetaCache = new ConcurrentDictionary<string, FuncRefInfo>();
        #endregion
        #region 与计算任务相关的公式计算结果cache
        /// <summary>
        /// 获取函数缓存
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static FuncInfo GetFunc(string taskId,FuncInfo func)
        {
            if (func == null) return null;

            FuncInfo finfo = null;
            ConcurrentDictionary<string, FuncInfo> funcCache = null;
            if (taskCache.TryGetValue(taskId, out funcCache))
            {
                funcCache.TryGetValue(func.Identifier, out finfo);                
            }

            return finfo;


        }


        /// <summary>
        /// 增加函数缓存
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool AddFunc(string taskId,FuncInfo func)
        {
            if (func == null) return false;         
            ConcurrentDictionary<string, FuncInfo> funcCache = null;
            if (taskCache.TryGetValue(taskId, out funcCache))
            {
                funcCache.TryAdd(func.Identifier,func);
            }
            else
            {
                funcCache = new ConcurrentDictionary<string, FuncInfo>();
                funcCache.TryAdd(func.Identifier, func);
                taskCache.TryAdd(taskId,funcCache);
            }

            return true;
        }

        public static bool RemoveFunc(string taskId)
        {
            ConcurrentDictionary<string, FuncInfo> funcCache = null;
            bool b = taskCache.TryRemove(taskId, out funcCache);
            if(funcCache!=null)
            {
                funcCache.Clear();
            }
            return b;            
        }
        #endregion

        #region 目录缓存
        public static bool AddCatalog(string catalog)
        {
            bool rtn = loadCatalogCache.TryAdd(catalog, catalog);
            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public static string GetCatalog(string catalog)
        {
            string value = string.Empty;
            loadCatalogCache.TryGetValue(catalog, out value);
            return value;
        }
        #endregion

        #region 上下文缓存
        public static bool AddContext(string taskId,FuncContext context)
        {
            bool rtn = contextCache.TryAdd(taskId, context);
            return rtn;
        }

        public static FuncContext GetContext(string taskId)
        {
            FuncContext context;
            contextCache.TryGetValue(taskId, out context);
            return context;
        }

        public static FuncContext GetContext()
        {
            string taskId = FuncCalcTask.GetTaskId();
            return GetContext(taskId);

        }

        public static bool RemoveContext(string taksId)
        {
            FuncContext context;
            return contextCache.TryRemove(taksId, out context);
        }
        #endregion

        #region 函数元信息缓存按目录 
        public static bool AddFuncInfoMetaDataByCatalog(string catalog, IList<FuncInfo> funcs)
        {
            return funcInfoMetaDataCacheByCatalog.TryAdd(catalog, funcs);
        }

        public static IList<FuncInfo> GetFuncInfoMetaDataByCatalog(string catalog)
        {
            IList<FuncInfo> funcs;
            funcInfoMetaDataCacheByCatalog.TryGetValue(catalog, out funcs);
            return funcs;
        }
        #endregion

        #region 函数元信息缓存按函数名
        public static bool AddFuncInfoMetaData(string key,FuncInfo func)
        {
            if (string.IsNullOrEmpty(key)) return false;
            return funcInfoMetaDataCache.TryAdd(key.ToUpper(), func);
        }

        public static FuncInfo GetFuncInfoMetaData(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            FuncInfo func;
            funcInfoMetaDataCache.TryGetValue(key.ToUpper(), out func);
            return func;
        }

        #endregion

        #region 下拉列表缓存
        public static bool AddDropDownList(string catalog, IList<DropDownList> lists)
        {
            return dropListMetaCache.TryAdd(catalog, lists);
        }

        public static IList<DropDownList> GetDropDownList(string catalog)
        {
            IList<DropDownList> lists;
            dropListMetaCache.TryGetValue(catalog, out lists);
            return lists;
        }
        #endregion

        #region 公式反射信息缓存
        public static bool AddFuncRefInfo(string funcName, FuncRefInfo refInfo)
        {
            if (string.IsNullOrEmpty(funcName)) return false;
            return funcRefInfoMetaCache.TryAdd(funcName.ToUpper(), refInfo);
        }

        public static FuncRefInfo GetFuncRefInfo(string funcName)
        {
            if (string.IsNullOrEmpty(funcName)) return null;
            FuncRefInfo refInfo;
            funcRefInfoMetaCache.TryGetValue(funcName.ToUpper(), out refInfo);
            return refInfo;
        }
        #endregion


        #region 公共缓存

        public static bool AddObject(string key,object value)
        {
            return commonCache.TryAdd(key, value);
        }

        public static object GetObject(string key)
        {
            object value;
            commonCache.TryGetValue(key, out value);
            return value;
        }

        #endregion

        /// <summary>
        /// 清除与任务ID相关的缓存信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static bool Clear(string taskId)
        {
            if (string.IsNullOrEmpty(taskId)) return false;
            bool b = RemoveContext(taskId);
            RemoveFunc(taskId);
            return b;
        }
    }
}
