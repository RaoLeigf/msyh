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
    /// GKPaymentDtlModel实体定义类
    /// </summary>
    public partial class GKPaymentDtlModel : EntityBase<GKPaymentDtlModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GKPaymentDtlModel()
        {
            List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

            PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
            info.PropertyName = "MstPhid";
            info.ColumnName = "mst_phid";
            info.PropertyType = EnumPropertyType.Long;
            info.IsPrimary = false;
            list.Add(info);

            ForeignKeys = list;//设置外键字段属性
        }

        /// <summary>
        /// 重新支付关联单号数组,多个用逗号分隔
        /// </summary>
        [DataMember]
        public virtual System.String FNewCodes
        {
            get;
            set;
        }

        /// <summary>
        /// 重新支付关联单号主表数组,多个用逗号分隔
        /// </summary>
        [DataMember]
        public virtual System.String FNewCodesMstPhid
        {
            get;
            set;
        }

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

        #endregion

    }

}