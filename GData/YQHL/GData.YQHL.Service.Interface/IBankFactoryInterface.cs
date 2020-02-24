using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Service.Interface
{
    public interface IBankFactoryInterface
    {
        /// <summary>
        /// 创建银行服务
        /// </summary>
        /// <returns></returns>
        IBankServiceInterface CreateBankService();
    }
}
