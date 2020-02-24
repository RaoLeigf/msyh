using GQT3.QT.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GQT3.QT.Model.Response.common
{
    /// <summary>
    /// 
    /// </summary>
   public class CodingResultModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CorrespondenceSettingsModel> list { get; set; }

        public string orgid { get; set; }
    }
}
