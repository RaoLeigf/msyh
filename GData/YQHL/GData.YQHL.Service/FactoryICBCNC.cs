using GData.YQHL.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Service
{
    public class FactoryICBCNC : IBankFactoryInterface
    {
        public IBankServiceInterface CreateBankService()
        {
            return new ICBCNCService();
        }
    }
}
