using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    /// <summary>
    /// 帮助类型
    /// </summary>
    public enum TreeMemoryType
    {
        EmpSingleHelp,
        EmpMultiHelp,
        OpSingleHelp,
        OpMultiHelp,
        HrTree,
        ActorTree,
        UGroupTree,
        SelfGroupTree,
        OnLineTree,
        OuterTree,
        Other
    }

    [Serializable]
    public class TreeMemoryEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Logid"></param>
        /// <param name="OCode"></param>
        /// <param name="treeMemoryType"></param>
        public TreeMemoryEntity(string Logid, string OCode, TreeMemoryType treeMemoryType, string BussType)
        {
            this.LogId = Logid;
            this.OCode = OCode;
            this.TreeType = treeMemoryType;
            this.BussType = BussType;
            this.FoucedNodeValue = "";
            this.IsMemo = true;
        }

        public TreeMemoryEntity(string Logid, string OCode, string type, string BussType)
        {
            this.LogId = Logid;
            this.OCode = OCode;
            this.Type = type;
            this.BussType = BussType;
            this.FoucedNodeValue = "";
            this.IsMemo = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeMemoryType"></param>
        public TreeMemoryEntity(TreeMemoryType treeMemoryType) : this(string.Empty, string.Empty, treeMemoryType, string.Empty) { }

        /// <summary>
        /// 当前操作员
        /// </summary>
        public string LogId { get; set; }

        /// <summary>
        /// 当前组织
        /// </summary>
        public string OCode { get; set; }

        /// <summary>
        /// 树类型
        /// </summary>
        public TreeMemoryType TreeType { get; set; }

        /// <summary>
        /// 树类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BussType { get; set; }

        /// <summary>
        /// 记忆树节点
        /// </summary>
        public string FoucedNodeValue { get; set; }

        /// <summary>
        /// 是否记忆
        /// </summary>
        public bool IsMemo { get; set; }
    }
}
