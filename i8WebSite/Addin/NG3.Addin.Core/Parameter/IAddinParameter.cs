using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Parameter
{
    public interface IAddinParameter
    {
        //全名称
         string Name { set; get; }
        //.
         string FirstPart { set; get; }       
         string SecondPart { set; get; }
         string ThirdPart { set; get; }

        string[] GetValues();

        string GetValue(int index);

    }
}
