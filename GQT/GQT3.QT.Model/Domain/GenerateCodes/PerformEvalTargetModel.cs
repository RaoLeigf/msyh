#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTargetModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        PerformEvalTargetModel.cs
    * 创建时间：        2018/10/16 
    * 作    者：        刘杭    
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

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// PerformEvalTarget实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class PerformEvalTargetModel : EntityBase<PerformEvalTargetModel>
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
        /// 指标代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetCode
        {
            get;
            set;
        }

        /// <summary>
        /// 指标名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetName
        {
            get;
            set;
        }

        /// <summary>
        /// 指标内容
        /// </summary>
        [DataMember]
        public virtual System.String FTargetContent
        {
            get;
            set;
        }

        /// <summary>
        /// 指标值
        /// </summary>
        [DataMember]
        public virtual System.String FTargetValue
        {
            get;
            set;
        }

        /// <summary>
        /// 指标权重
        /// </summary>
        [DataMember]
        public virtual System.String FTargetWeight
        {
            get;
            set;
        }

        /// <summary>
        /// 指标描述
        /// </summary>
        [DataMember]
        public virtual System.String FTargetDescribe
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类别代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetClassCode
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否用户增加
        /// </summary>
        [DataMember]
        public virtual System.Int32 FIfCustom
        {
            get;
            set;
        }

        /// <summary>
        /// 组织id
        /// </summary>
        [DataMember]
        public virtual System.Int64 Orgid
        {
            get;
            set;
        }

        /// <summary>
        /// 组织代码
        /// </summary>
        [DataMember]
        public virtual System.String Orgcode
        {
            get;
            set;
        }


        /// <summary>
        /// 指标类别名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        public virtual System.Byte Isactive
        {
            get;
            set;
        }
        #region//多层指标类别
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypePerantCode
        {
            get;
            set;
        }
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode1
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName1
        {
            get;
            set;
        }
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode2
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName2
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode3
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName3
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode4
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName4
        {
            get;
            set;
        }
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode5
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName5
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型的层级
        /// </summary>
        [DataMember]
        public virtual System.Int32 TypeCount
        {
            get;
            set;
        }
        #endregion
    }

}