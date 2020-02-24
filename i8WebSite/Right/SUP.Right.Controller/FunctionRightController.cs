using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Interface;
using SUP.Right.Rules;
using SUP.Right.Facade;
using NG3.Aop.Transaction;
using NG3.Web.Controller;

namespace SUP.Right.Controller
{
    /// <summary>
    /// 按钮权限管理
    /// </summary>
    public class ButtonRightController:AFController
    {
        
        private ButtonRightFacad rightFacad;

        public ButtonRightController() {
            rightFacad = AopObjectProxy.GetObject<ButtonRightFacad>(new ButtonRightFacad());
        }

        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return rightFacad.Validate(ocode, logid, rightname, funcname, objs);
        }

        //public void Validate()
        //{
        //    bool d = Validate("0001", NG3.AppInfoBase.LoginID, "EBMTemplateManager", null);
        //    Ajax.WriteRaw(d.ToString());
        //}
    }

    /// <summary>
    /// 菜单权限管理
    /// </summary>
    public class EntryRightController : NG3.Web.Controller.AFController
    {
        private EntryRightFacad rightFacad;

        public EntryRightController()
        {
            rightFacad = AopObjectProxy.GetObject<EntryRightFacad>(new EntryRightFacad());
        }

        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return rightFacad.Validate(ocode, logid, rightname, funcname, objs);
        }
    }

    /// <summary>
    /// 特殊按钮权限管理
    /// 适合于一个菜单在两个业务点被注册的情况下
    /// </summary>
    public class ButtonRightByMenuController : NG3.Web.Controller.AFController
    {
        private ButtonRightByMenuFacad rightFacad;

        public ButtonRightByMenuController()
        {
            rightFacad = AopObjectProxy.GetObject<ButtonRightByMenuFacad>(new ButtonRightByMenuFacad());
        }

        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return rightFacad.Validate(ocode, logid, rightname, funcname, objs);
        }
    }

    /// <summary>
    /// 功能权限管理
    /// </summary>
    public class FunctionRightController : NG3.Web.Controller.AFController
    {
        protected Manager manager = new Manager();

        private FunctionRightFacad rightFacad;

        public FunctionRightController()
        {
            rightFacad = AopObjectProxy.GetObject<FunctionRightFacad>(new FunctionRightFacad());
        }
        /// <summary>
        /// 功能权限新增
        /// </summary>
        /// <param name="entity">功能权限涉及的三张表需要填写的信息</param>
        /// <returns></returns>
        public bool Add(IRightEntity entity)
        {
            return rightFacad.Add(entity);
        }

        /// <summary>
        /// 功能权限删除
        /// </summary>
        /// <param name="entity">功能权限涉及的三张表需要填写的信息</param>
        /// <returns></returns>
        public bool Remove(IRightEntity entity)
        {
            return rightFacad.Remove(entity);
        }

        /// <summary>
        /// 功能权限更新
        /// </summary>
        /// <param name="entity">功能权限涉及的三张表需要填写的信息</param>
        /// <returns></returns>
        public bool Update(IRightEntity entity)
        {
            return rightFacad.Update(entity);
        }
    }
}
