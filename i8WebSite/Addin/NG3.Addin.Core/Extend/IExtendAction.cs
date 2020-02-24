using NG3.Addin.Model.Domain.BusinessModel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Extend
{
    public interface IExtendAction
    {
        ISession Session { set; get; }
        ExtendFuncBizModel ConfigureEntity { set; get; }
        string Execute();
    }
}
