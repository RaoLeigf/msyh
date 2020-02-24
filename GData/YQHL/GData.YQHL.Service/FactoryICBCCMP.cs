using GData.YQHL.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Service
{
    /// <summary>
    /// 工行推广版工厂类
    /// </summary>
    public class FactoryICBCCMP : IBankFactoryInterface
    {
        public IBankServiceInterface CreateBankService()
        {
            return new ICBCCMPService();
        }
    }
}
