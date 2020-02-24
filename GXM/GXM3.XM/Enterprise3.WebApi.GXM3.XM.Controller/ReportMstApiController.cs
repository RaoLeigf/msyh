using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GXM3.XM.Model.Common;
using Enterprise3.WebApi.GXM3.XM.Model.Request;
using GData3.Common.Utils.Filters;
using GXM3.XM.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Enterprise3.WebApi.GXM3.XM.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class ReportMstApiController: ApiBase
    {
        IXmReportMstService XmReportMstService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportMstApiController()
        {
            XmReportMstService = base.GetObject<IXmReportMstService>("GXM3.XM.Service.XmReportMst");

        }

        /// <summary>
        /// 点击生成项目草案获取该单据对应的签报单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHProjectReport([FromUri]BaseListModel param)
        {
            if (param.FPhid == 0)
            {
                return DCHelper.ErrorMessage("单据主键不能为空！");
            }
            try
            {
                var result = this.XmReportMstService.GetMSYHProjectReport(param.FPhid);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = result
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}