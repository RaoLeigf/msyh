using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// /
    /// </summary>
    public class QtNaviGationdtlModel
    {
        /// <summary>
        /// area1
        /// </summary>
        public List<QtNaviGationModel> area1 { get; set; }
        /// <summary>
        /// /area2
        /// </summary>
        public List<QtNaviGationModel> area2 { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public long Operator { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class QtDemo
    {
        /// <summary>
        /// 
        /// </summary>
        public System.Int64 PhId
        {
            get;
            set;
        }
        /// <summary>
        /// 按钮code
        /// </summary>
        public System.String Buttoncode
        {
            get;
            set;
        }

        /// <summary>
        /// 排序值
        /// </summary>
        public System.Int32 Sortvalue
        {
            get;
            set;
        }
        /// <summary>
		/// 是否隐藏
		/// </summary>
        public System.Int32 Invisible
        {
            get;
            set;
        }

        /// <summary>
        /// 操作员
        /// </summary>
        public System.Int64 Operator
        {
            get;
            set;
        }

        /// <summary>
		/// Name
		/// </summary>
        public System.String Name
        {
            get;
            set;
        }
        /// <summary>
		/// Menu
		/// </summary>
        public System.String Menu
        {
            get;
            set;
        }
        /// <summary>
		/// Srcs
		/// </summary>
        public System.String Srcs
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int NgRecordVer { get; set; }

    }
}
