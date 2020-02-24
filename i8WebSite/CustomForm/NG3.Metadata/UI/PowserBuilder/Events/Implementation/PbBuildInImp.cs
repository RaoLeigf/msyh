using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Events.Implementation
{
    public class PbBuildInImp : PbBaseImp
    {
        private string _param = string.Empty;

        public string Param
        {
            get { return _param; }
            set { _param = value; }
        }
    }
}
