using Enterprise3.NHORM.Controller;
using NG3.Addin.Core.Extend;
using NG3.Web.Mvc;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NG3.Addin.Controller
{
    /// <summary>
    /// 新增加功能controller
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ExtendFuncController : AFCommonController
    {


        private ExtendExecutor executor;

        /// <summary>
        /// 扩展的controller
        /// </summary>
        public ExtendFuncController()
        {
            //执行器
            executor = base.GetObject<ExtendExecutor>("NG3.Addin.Core.Extend.ExtendExecutor");
        }
        /// <summary>
        /// 指向页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Ui(string id)
        {
            string view = string.Empty;
            view = executor.Executor(id);        
            //支持多语言
            return View(view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendName"></param>
        /// <returns></returns>
        public string Action(string id)
        {
            string value = string.Empty;
            value = executor.Executor(id);          
            return value;

        }
    }
}
