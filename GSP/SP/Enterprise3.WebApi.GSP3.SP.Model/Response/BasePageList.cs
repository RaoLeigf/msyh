using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GSP3.SP.Model.Response
{
    public class BasePageList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; }
    }
}
