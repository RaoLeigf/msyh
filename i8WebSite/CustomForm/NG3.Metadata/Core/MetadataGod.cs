using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.Core
{
    public abstract class MetadataGod
    {
        public string Id { get; set; }

        public MetadataGod()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
