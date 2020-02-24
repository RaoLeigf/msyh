#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesApiController
    * 命名空间：        Enterprise3.WebApi.GYS3.YS.Controller
    * 文 件 名：        ExamplesApiController.cs
    * 创建时间：        2018/8/17 9:53:13 
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
using Enterprise3.WebApi.ApiControllerBase;

using GYS3.YS.Service.Interface;
using GYS3.YS.Model;

namespace Enterprise3.WebApi.GYS3.YS.Controller
{
    /// <summary>
    /// 示例服务
    /// </summary>
    [MethodExceptionFilter]
    public class ExamplesApiController : ApiBase
    {
        /// <summary>
        /// 业务外观层对象
        /// </summary>
        private IExamplesService ExamplesService { get; set; }

        /// <summary>
        /// 获取指定组织所有有效的部门信息
        /// </summary>
        /// <param name="ocode">组织编码（目前通过编码设置关系）</param>
        /// <returns></returns>
        public IList<ExamplesModel> GetAllEffectiveDepts(string ocode)
        {
            return ExamplesService.GetAllEffectiveDepts(ocode);
        }
    }
}
