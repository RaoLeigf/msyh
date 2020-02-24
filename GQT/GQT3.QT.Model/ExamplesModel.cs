#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesModel
    * 命名空间：        GQT3.QT.Model
    * 文 件 名：        ExamplesModel.cs
    * 创建时间：        2015/9/21 
    * 作    者：        丰立新    
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

namespace GQT3.QT.Model
{
    /// <summary>
    /// Examples实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class ExamplesModel : EntityBase<ExamplesModel>
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

        private System.String _ocode = string.Empty;
        /// <summary>
        /// 组织编号
        /// </summary>
        [DataMember]
        public virtual System.String OCode
        {
            get
            {
                return _ocode;
            }
            set
            {
                _ocode = value;
            }
        }

        private System.String _oname = string.Empty;
        /// <summary>
        /// 组织名称
        /// </summary>
        [DataMember]
        public virtual System.String OName
        {
            get
            {
                return _oname;
            }
            set
            {
                _oname = value;
            }
        }

        /// <summary>
        /// 是否独立法人：1,0
        /// </summary>
        [DataMember]
        public virtual System.Int16 IsCorp
        {
            get;
            set;
        }

    }

}