using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGK3.GK.UnitTest
{
    public class SaveResponse
    {
        public string[] KeyCodes { get; set; }
        public int SaveRows { get; set; }
        public string AttachMsg { get; set; }
        public string Msg { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }
    }
}
