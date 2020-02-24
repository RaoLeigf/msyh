using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Extend
{
    public class ExtendUrlAction : AbstractExtendAction
    {
        public override string Execute()
        {
            try
            {
                
                return ConfigureEntity.MstModel.Url;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
