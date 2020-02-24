using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core
{
    public class AddinException:Exception
    {
        public AddinException(string message):base(message)
        {

        }
    }
}
