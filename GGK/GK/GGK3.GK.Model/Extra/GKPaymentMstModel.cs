#region Summary
/**************************************************************************************
    * 类 名 称：        GKPaymentMstModel
    * 命名空间：        GGK3.GK.Model.Domain
    * 文 件 名：        GKPaymentMstModel.cs
    * 创建时间：        2019/5/23 
    * 作    者：        李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;
using Newtonsoft.Json;

namespace GGK3.GK.Model.Domain
{
    /// <summary>
    /// GKPaymentMst实体定义类
    /// </summary>
    public partial class GKPaymentMstModel : EntityBase<GKPaymentMstModel>
    {
        #region Json序列化要忽略的列增加 JsonIgnore 属性
        /// <summary>
        /// 外键
        /// </summary>
        [DataMember, JsonIgnore]
        override public IList<PropertyColumnMapperInfo> ForeignKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public IList<PropertyColumnMapperInfo> BusinessPrimaryKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public object _OldIdValue_ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public Dictionary<string, object> ExtendObjects { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public Dictionary<string, byte[]> PropertyBytes { get; set; }


        /// <summary>
        /// 关联业务单名称
        /// </summary>
        [DataMember]
        public virtual System.String RefbillName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否待我审批
        /// </summary>
        [DataMember]
        public virtual bool IsApprovalNow
        {
            get;
            set;
        }

        /// <summary>
        /// 审批流程主键
        /// </summary>
        [DataMember]
        public virtual Int64 ProcPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位主键
        /// </summary>
        [DataMember]
        public virtual Int64 PostPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 是否待我审批
        /// </summary>
        [DataMember]
        public virtual Int64 OperaPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 审批记录phid
        /// </summary>
        [DataMember]
        public virtual Int64 AppvalPhid
        {
            get;
            set;
        }

        

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public virtual string fdepname
        {
            get;
            set;
        }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember]
        public virtual string FOrgname
        {
            get;
            set;
        }

        #endregion

    }

}