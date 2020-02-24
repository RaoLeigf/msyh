using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    public  class DataStoreParam
    {
        /// <summary>
        /// 页号
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 排序参数模型
        /// </summary>
        public StoreSortParamModel[] Sorters { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
    }

    /// <summary>
    /// 排序参数模型
    /// </summary>
    public class StoreSortParamModel
    {
        /// <summary>
        /// property
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// direction
        /// </summary>
        public string direction { get; set; }
    }
}
