using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Cfg
{

    public static class AddinCache
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<Int64, AddinServiceModel> _services = new System.Collections.Concurrent.ConcurrentDictionary<long, AddinServiceModel>();

        //操作员缓存
        private static System.Collections.Concurrent.ConcurrentDictionary<string, AddinOperatorModel> _operators = new System.Collections.Concurrent.ConcurrentDictionary<string, AddinOperatorModel>();

        //已经发布的配置
        private static System.Collections.Concurrent.ConcurrentDictionary<Int64, MethodAroundBizModel> _deployconfigures = new System.Collections.Concurrent.ConcurrentDictionary<Int64, MethodAroundBizModel>();

        //所有配置包含未发布的配置
        private static System.Collections.Concurrent.ConcurrentDictionary<Int64, MethodAroundBizModel> _allconfigures = new System.Collections.Concurrent.ConcurrentDictionary<Int64, MethodAroundBizModel>();

        //服务对应的UI参数缓存
        private static System.Collections.Concurrent.ConcurrentDictionary<string, ServiceUIParamBizModel> _uiParamCache = new System.Collections.Concurrent.ConcurrentDictionary<string, ServiceUIParamBizModel>();

        //功能扩展配置缓存
        private static System.Collections.Concurrent.ConcurrentDictionary<string, ExtendFuncBizModel> _extendFuncCache = new System.Collections.Concurrent.ConcurrentDictionary<string, ExtendFuncBizModel>();

        #region 操作员缓存

        public static void ClearAllOpers()
        {
            _operators.Clear();
        }

        public static bool AddOper(AddinOperatorModel oper)
        {
            return _operators.TryAdd(oper.LoginId.ToLower(), oper);
        }

        public static bool ContainOper(string key)
        {
            AddinOperatorModel oper;
            bool b = _operators.TryGetValue(key, out oper);
            return b;
        }

        #endregion

        #region 已经发布的配置缓存

       /// <summary>
       /// 增加已发布的配置
       /// </summary>
       /// <param name="key"></param>
       /// <param name="model"></param>
       /// <returns></returns>
        public static bool AddDeployedCfg(Int64 key, MethodAroundBizModel model)
        {
            return _deployconfigures.TryAdd(key, model);
        }

        /// <summary>
        /// 删除已发布的配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveDeployCfg(Int64 key)
        {
            MethodAroundBizModel model;
            return _deployconfigures.TryRemove(key, out model);

        }

        public static MethodAroundBizModel GetDeployedCfg(Int64 key)
        {
            MethodAroundBizModel model;
            _deployconfigures.TryGetValue(key, out model);
            return model;
        }

        /// <summary>
        /// 取得缓存对象列表
        /// </summary>
        /// <returns></returns>
        public static IList<MethodAroundBizModel> GetDeployedCfgList(string key)
        {
            IList<MethodAroundBizModel> models = new List<MethodAroundBizModel>() ;
            foreach (var item in _deployconfigures.Keys)
            {
                var data = _deployconfigures[item];
                AddinConfigureEntityKey entityKey = new AddinConfigureEntityKey
                (data.MstModel.TargetClassName, data.MstModel.TargetMethodName, data.MstModel.InterceptorType);

                if (entityKey.ToString() == key)
                {
                    models.Add(data);
                }
            }
            return models;

        }
        #endregion

        #region 所有配置缓存

        /// <summary>
        /// 增加配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddCfg(Int64 key, MethodAroundBizModel model)
        {
            return _allconfigures.TryAdd(key, model);
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveCfg(Int64 key)
        {
            MethodAroundBizModel model;
            return _allconfigures.TryRemove(key, out model);
        }


        public static MethodAroundBizModel GetCfg(Int64 key)
        {
            MethodAroundBizModel model;
            _allconfigures.TryGetValue(key, out model);
            return model;
        }

        /// <summary>
        /// 取得缓存对象列表
        /// </summary>
        /// <returns></returns>
        public static IList<MethodAroundBizModel> GetCfgList(string key)
        {
            IList<MethodAroundBizModel> models = new List<MethodAroundBizModel>(); ;
            foreach (var item in _allconfigures.Keys)
            {
                var data = _allconfigures[item];
                AddinConfigureEntityKey entityKey = new AddinConfigureEntityKey
                (data.MstModel.TargetClassName, data.MstModel.TargetMethodName, data.MstModel.InterceptorType);

                if (entityKey.GetKey() == key)
                {
                    models.Add(data);
                }
            }
            return models;

        }

        /// <summary>
        /// 清空所有配置
        /// </summary>
        public static void ClearCfg()
        {
            _allconfigures.Clear();
        }

        #endregion


        #region UI参数缓存
        public static bool AddUIParam(string key, ServiceUIParamBizModel model)
        {
            _uiParamCache.AddOrUpdate(key, model,(k,m) => { return model; });
            return true;
        }


        /// <summary>
        /// 取得所有已经运行过服务的参数集合
        /// </summary>
        /// <returns></returns>
        public static IList<ServiceUIParamBizModel> GetUIParams()
        {
            return _uiParamCache.Values.ToList();
        }

        #endregion


        #region 扩展功能缓存
        public static bool AddExtendFunc(string key, ExtendFuncBizModel model)
        {
            return _extendFuncCache.TryAdd(key, model);
        }

        public static void ClearExtendFunc()
        {
            _extendFuncCache.Clear();
        }

        public static ExtendFuncBizModel GetExtendFuncConfigure(string key)
        {
            ExtendFuncBizModel model;
            _extendFuncCache.TryGetValue(key, out model);
            return model;
        }


        #endregion

        #region 所有已注册的服务
        public static bool AddService(Int64 key, AddinServiceModel model)
        {
            return _services.TryAdd(key, model);
        }

        public static void ClearService()
        {
            _services.Clear();
        }

        public static AddinServiceModel GetService(Int64 key)
        {
            AddinServiceModel model;
            _services.TryGetValue(key, out model);
            return model;
        }

        #endregion


    }
}
