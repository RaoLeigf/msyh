
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Domain.BusinessModel;
using NG3.Addin.Model.Enums;
using NHibernate;
using Spring.Data.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Extend
{
    public static class ExtendActionFactory
    {

       
        public static IExtendAction GetExtendAction(ISession session,string name)
        {
            ExtendFuncBizModel entity = ExtendConfigure.GetConfigureEntity(name);

            //找不到对应的配置则返回空
            if (entity == null) return null;

            EnumAddinType type = entity.MstModel.FuncType;

            IExtendAction executor = null;
            if(type== EnumAddinType.Url)
            {
                executor = new ExtendUrlAction();                                
            }
            else if(type== EnumAddinType.Sql)
            {
                executor = new ExtendSqlAction();
            }else if(type== EnumAddinType.Assembly)
            {
                executor = new ExtendAssemblyAction();
            }
            //注入参数
            if(executor!=null)
            {
                executor.Session = session;
                executor.ConfigureEntity = entity;
            }

            return executor;

        }
    }
}
