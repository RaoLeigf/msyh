using GYS3.YS.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GYS3.YS.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class AllYsIncomeRequestModel: BaseListModel
    {
        /// <summary>
        /// 用款计划主表对象
        /// </summary>
        [DataMember]
        public virtual YsIncomeMstModel YsIncomeMst
        {
            get;
            set;
        }

        /// <summary>
        /// 用款计划主表对象
        /// </summary>
        [DataMember]
        public virtual IList<YsIncomeDtlModel> YsIncomeDtls
        {
            get;
            set;
        }
    }
}