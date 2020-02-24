using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    public class CodeToNameInfoEntity
    {

        public CodeToNameInfoEntity()
        {
        }


        /// <summary>
        /// 代码列属性
        /// </summary>
        public string CodeProperty
        {
            get;
            set;
        }


        /// <summary>
        /// 待修改列属性
        /// </summary>
        public string ModifyProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 待修改列属性
        /// </summary>
        public string HelpID
        {
            get;
            set;
        }

        /// <summary>
        /// sql过滤条件
        /// </summary>
        public string Filter
        {
            get;
            set;
        }

    }
}
