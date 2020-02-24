using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SUP.Common.DataEntity
{
    public class CommonHelpSettingEntity
    {
        public DataTable MasterDt { get; set; }

        public DataTable SystemFieldDt { get; set; }

        public DataTable UserFieldDt { get; set; }

        public DataTable QueryProperty { get; set; }

        public DataTable RichQuery { get; set; }

        public DataTable DetailDt { get; set; }

    }
}
