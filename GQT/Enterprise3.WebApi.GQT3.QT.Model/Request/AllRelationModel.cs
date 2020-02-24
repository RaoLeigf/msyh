using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GQT3.QT.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class AllRelationModel: BaseListModel
    {
        /// <summary>
        /// 关系类型（1-单位对应预算科目，2-单位对应资金来源，3-单位对应支出类别，4-单位对应支出渠道）
        /// </summary>
        [DataMember]
        public string RelationType { get; set; }

        /// <summary>
        /// 基础数据类型类型(PayMethod-支付方式；ProjectLevel-项目级别；ProjectProper-项目属性；TimeLimit-续存期限；PayMethodTwo-支付方式2)
        /// </summary>
        [DataMember]
        public string DicType { get; set; }

        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public string TargetTypeCode { get; set; }

        /// <summary>
        /// 采购相关数据的类型（1-Catalog采购目录,2-Procedures采购程序,3-Type采购类型）
        /// </summary>
        [DataMember]
        public string ProcurType { get; set; }

        /// <summary>
        /// 绩效相关数据的类型 （1-Type评价类型，2-评价指标类别）
        /// </summary>
        [DataMember]
        public string PerformType { get; set; }
    }
}