using NG3.Addin.Core.Extend;
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
    public static class ExtendConfigure
    {

        private static object lockobject = new object();      
        private static bool _inited = false;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitConfigure(ISession session)
        {
            if (_inited) return;
            lock (lockobject)
            {
                if (_inited) return;
                IList<ExtendFuncBizModel> configures = LoadExtendConfigureFromDataSource(session);
                configures.ToList().ForEach(p => AddinCache.AddExtendFunc(p.MstModel.FuncName, p));               
                _inited = true; //设置初始化完成标志
            }
        }

        /// <summary>
        /// 刷新配置
        /// </summary>
        public static void Refresh(ISession session)
        {
            lock(lockobject)
            {
                IList<ExtendFuncBizModel> configures = LoadExtendConfigureFromDataSource(session);
                AddinCache.ClearExtendFunc();
                configures.ToList().ForEach(p => AddinCache.AddExtendFunc(p.MstModel.FuncName, p));
            }
        }



        //根据扩展功能的名称获取配置类
        public static ExtendFuncBizModel GetConfigureEntity(string name)
        {
            if (!_inited) return null;
            if (string.IsNullOrEmpty(name)) return null;

            return AddinCache.GetExtendFuncConfigure(name);

        }


        /// <summary>
        /// 从数据库取出
        /// </summary>
        /// <returns></returns>
        private static List<ExtendFuncBizModel> LoadExtendConfigureFromDataSource(ISession session)
        {
            List<ExtendFuncBizModel> configures = new List<ExtendFuncBizModel>();

   
            var criteria = session.CreateCriteria<ExtendFunctionMstModel>();
            var mstData = criteria.List<ExtendFunctionMstModel>();
            foreach (var item in mstData)
            {
                //从SQL中取数，先模拟
                ExtendFuncBizModel entity = new ExtendFuncBizModel();
                long phid = item.Phid;

                var sqldatas = session.CreateQuery("from AddinSqlModel s where s.MstPhid=" + phid +" and SqlCatalog=" + (int)EnumCatalog.Extend).List<AddinSqlModel>();

                var assemblydatas = session.CreateQuery("from AddinAssemblyModel a where a.MstPhid=" + phid + " and AssemblyCatalog=" + (int)EnumCatalog.Extend).List<AddinAssemblyModel>();

                entity.AssemblyModels = assemblydatas;
                entity.SqlModels = sqldatas;
                entity.MstModel = item;

                configures.Add(entity);
            }
            //直接从数据库获取

            return configures;
        }





    }
}
