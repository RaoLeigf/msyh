using Enterprise3.Common.Base.Criterion;
using Enterprise3.WebApi.ApiControllerBase;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GXM3.XM.Model.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class ProjectThresholdApiController : ApiBase
    {
        IProjectThresholdService ProjectThresholdService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectThresholdApiController()
        {
            ProjectThresholdService = base.GetObject<IProjectThresholdService>("GQT3.QT.Service.ProjectThreshold");
        }

        /// <summary>
        /// 根据组织和项目类型取阈值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetProjectThresholdByOrgAndZCLB([FromUri]ProjectMstModel parameters)
        {
            if (string.IsNullOrEmpty(parameters.FDeclarationUnit))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            if (string.IsNullOrEmpty(parameters.FExpenseCategory))
            {
                return DCHelper.ErrorMessage("项目类型为空！");
            }
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Orgcode", parameters.FDeclarationUnit));
            IList<ProjectThresholdModel> projectThresholds = ProjectThresholdService.Find(dicWhere).Data;
            if (projectThresholds.Count > 0)
            {
                foreach (var projectThreshold in projectThresholds)
                {
                    IList<String> useCodeList = (projectThreshold.ProjTypeId ?? "").Split(',').ToList<String>();
                    if (useCodeList.Contains(parameters.FExpenseCategory))
                    {
                        return JsonConvert.SerializeObject(projectThreshold);
                    }
                }
            }
            return JsonConvert.SerializeObject(new ProjectThresholdModel());
        }
    }
}
