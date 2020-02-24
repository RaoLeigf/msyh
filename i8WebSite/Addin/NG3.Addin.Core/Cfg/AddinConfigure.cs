using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;
using NG3.Addin.Model.Enums;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Cfg
{
    public static class AddinConfigure
    {
        private static object lockobject = new object();
        private static object alllockobject = new object();

        private static bool _inited = false;
         
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="session"></param>
        public static void Init(ISession session)
        {
            if (_inited) return;
            lock(lockobject)
            {
                //所有服务加入到缓存中
                var serviceData = LoadAllPluginService(session);
                serviceData.ToList().ForEach(p => { AddinCache.AddService(p.Phid, p); });


                if (_inited) return;
                IList<MethodAroundBizModel> configures = LoadAllPluginConfigureFromDataSource(session);

                
                foreach (var item in configures)
                {
                    //通过ID填充Service记录
                    var service = AddinCache.GetService(item.MstModel.TargetServiceId);
                    if(service!=null)
                    {
                        item.MstModel.TargetAssemblyName = service.TargetAssemblyName;
                        item.MstModel.TargetClassName = service.TargetClassName;
                        item.MstModel.TargetMethodName = service.TargetMethodName;
                    }                   
                    AddinCache.AddCfg(item.MstModel.Phid, item);

                }
                              
                //已经发布了的程序
                var deployconfigures = configures.Where(p => p.MstModel.DeployFlag == 1).ToList();
                foreach (var item in deployconfigures)
                {
                    AddinCache.AddDeployedCfg(item.MstModel.Phid, item);
                }
                _inited = true; //设置初始化完成标志
            }
        }


        /// <summary>
        /// 发布配置
        /// </summary>
        /// <returns></returns>
        public static bool DeployConfigure(long mstPhid)
        {

            var data = AddinCache.GetCfg(mstPhid);
            if (data == null) return false;
             
            data.MstModel.DeployFlag = 1;
            return AddinCache.AddDeployedCfg(data.MstModel.Phid, data);
          
            
        }

        /// <summary>
        /// 取消发布配置
        /// </summary>
        /// <returns></returns>
        public static bool UnDeployConfigure(long mstPhid)
        {
            var data = AddinCache.GetDeployedCfg(mstPhid);
            if (data == null) return false;

            data.MstModel.DeployFlag = 0;

            return AddinCache.RemoveDeployCfg(data.MstModel.Phid);
           
        }


        /// <summary>
        /// 重新加载配置
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static bool ReloadConfigure(ISession session)
        {
            var data = LoadAllPluginConfigureFromDataSource(session);

            lock(alllockobject)
            {
                AddinCache.ClearCfg();                
                foreach (var item in data)
                {
                    AddinCache.AddCfg(item.MstModel.Phid, item);

                }
            }
            return true;           

        }

        /// <summary>
        /// 根据调用信息获取注入信息
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MethodAroundBizModel GetPluginConfigure(string methodName, object[] args, object target, EnumInterceptorType type)
        {
            AddinConfigureEntityKey entityKey = new AddinConfigureEntityKey
                   (target.ToString(), methodName, type);

            //取不到配置信息
            var entitys = AddinConfigure.GetPluginConfigure(entityKey);
            if (entitys.Count == 0) return null;

            //如果只有一个则直接返回
            if (entitys.Count == 1) return entitys[0];

            foreach (var entity in entitys)
            {
                //如果判断
                var service = AddinCache.GetService(entity.MstModel.TargetServiceId);
                if (service == null)
                {
                    LogHelper<AddinServiceModel>.Info("In AddinConfigure GetPluginConfigure method 通过addin_method_m中的servicePHid未能取得对应的service");
                    return null;
                }

                string matchCondition = service.MatchCondition;
                //如果不存在条件则直接返回
                if (string.IsNullOrEmpty(matchCondition)) continue;

                string[] eles = matchCondition.Split(new char[] { ':' });
                if (eles.Length != 2)
                {
                    LogHelper<AddinServiceModel>.Info("In AddinConfigure GetPluginConfigure method 条件定义出错" + matchCondition);
                    continue;
                }
                int index = -1;
                if (!int.TryParse(eles[0].Trim(), out index))
                {
                    LogHelper<AddinServiceModel>.Info("In AddinConfigure GetPluginConfigure method 条件定义出错" + matchCondition);
                    continue;
                }

                if (index < 0 || index > args.Length)
                {

                    LogHelper<AddinServiceModel>.Info("In AddinConfigure GetPluginConfigure method 条件定义出错" + matchCondition);
                    continue;

                }
                if (args[index] == null)
                {
                    LogHelper<AddinServiceModel>.Info(string.Format("In AddinConfigure GetPluginConfigure args{0} is null", index));
                    continue;
                }

                if (!args[index].ToString().Equals(eles[1], StringComparison.OrdinalIgnoreCase))
                {
                    LogHelper<AddinServiceModel>.Info(string.Format("In AddinConfigure GetPluginConfigure args{0} is {1},MatchCondition value {2}", index, args[index].ToString(), eles[1]));
                    continue;
                }
                return entity;
            }

            return null;
        }

        private static IList<MethodAroundBizModel> GetPluginConfigure(AddinConfigureEntityKey entityKey )
        {
            
            if (!_inited) return null;

            var bizModel = AddinCache.GetDeployedCfgList(entityKey.GetKey());

            if (bizModel.Count != 0) return bizModel;

            if(AddinOperator.CurrentUserIsOperator())
            {
                //如果是未发布，但是测试人员可以进行测试
                bizModel = AddinCache.GetCfgList(entityKey.GetKey());                
            }           
            return bizModel;
           
        }

        /// <summary>
        /// 加载所有注册的可以进行功能注入的服务
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private static IList<AddinServiceModel> LoadAllPluginService(ISession session)
        {            
            //所有注册的可以进行功能注入的服务
            var serviceCriteria = session.CreateCriteria<AddinServiceModel>();
            var serviceData = serviceCriteria.List<AddinServiceModel>();
            return serviceData;
        }

        private static IList<MethodAroundBizModel> LoadAllPluginConfigureFromDataSource(ISession session)
        {
            List<MethodAroundBizModel> configures = new List<MethodAroundBizModel>();




            var criteria = session.CreateCriteria<MethodAroundMstModel>();
            var mstData = criteria.List<MethodAroundMstModel>();
            foreach (var item in mstData)
            {
                //从SQL中取数，先模拟
                MethodAroundBizModel entity = new MethodAroundBizModel();
                long phid = item.Phid;

                var sqldatas = session.CreateQuery("from AddinSqlModel s where s.MstPhid=" + phid + " and SqlCatalog="+ (int)EnumCatalog.Interceptor).List<AddinSqlModel>();

                var expdatas = session.CreateQuery("from AddinExpressionModel e where e.MstPhid=" + phid).List<AddinExpressionModel>();

                var expvardatas = session.CreateQuery("from AddinExpressionVarModel v where v.MstPhid=" + phid).List<AddinExpressionVarModel>();

                var assemblydatas = session.CreateQuery("from AddinAssemblyModel a where a.MstPhid=" + phid + " and AssemblyCatalog="+ (int)EnumCatalog.Interceptor).List<AddinAssemblyModel>();
                               
                entity.AssemblyModels = assemblydatas;
                entity.ExpVarModels = expvardatas;
                entity.ExpModels = expdatas;
                entity.SqlModels = sqldatas;
                entity.MstModel = item;

                configures.Add(entity);
            }

            return configures;
        }

    }
}
