#region Summary
/**************************************************************************************
    * 类 名 称：        KingGridTagModel
    * 命名空间：        WM3.Archive.Model.Domain
    * 文 件 名：        KingGridTagModel.cs
    * 创建时间：        2018/6/26 
    * 作    者：        徐政    
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

namespace WM3.Archive.Model.Domain
{
    /// <summary>
    /// KingGridTag实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class KingGridTagModel : EntityBase<KingGridTagModel>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 PhId
        {
            get;
            set;
        }

        /// <summary>
        /// 标签名
        /// </summary>
        [DataMember]
        public virtual System.String Code
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public virtual System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public virtual System.String Ptype
        {
            get;
            set;
        }

    }

}