using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder.Events
{
    /// <summary>
    /// PB事件描述
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PbEvent<T>:MetadataGod where T:PbBaseImp,new()
    {
        private IList<T> _pbImp = new List<T>();

        public IList<T> PbImp
        {
            get { return _pbImp; }
            set { _pbImp = value; }
        }
    }
}
