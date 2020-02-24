using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NG3.Addin.Model.Domain.BusinessModel;

namespace NG3.Addin.Core.Extend
{
    public abstract class AbstractExtendAction : IExtendAction
    {
        private ExtendFuncBizModel _configureEntity;
        private ISession _session;
        public ExtendFuncBizModel ConfigureEntity
        {
            get
            {
                return _configureEntity;
            }

            set
            {
                _configureEntity = value;
            }
        }

        public ISession Session
        {
            get
            {
                return _session;
            }

            set
            {
                _session = value;
            }
        }

        public abstract string Execute();
        
    }
}
