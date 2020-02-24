using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Interface;

namespace SUP.Right.Rules
{
    /// <summary>
    /// 菜单权限管理
    /// </summary>
    public class EntryRight : FunctionRight
    {
        /// <summary>
        /// 判断是否有权限:窗口的入口权限
        /// </summary>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <param name="rightname"></param>
        /// <returns></returns>
        public override bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return base.services.IsHaveRight(ocode, logid, rightname, funcname);
        }
    }

    /// <summary>
    /// 按钮权限管理
    /// </summary>
    public class ButtonRight : FunctionRight
    {
        /// <summary>
        /// 判断按钮是否有权限
        /// </summary>
        /// <param name="ocode">组织号</param>
        /// <param name="logid">操作员</param>
        /// <param name="rightname">权限名</param>
        /// <param name="buttonname">按钮名</param>
        /// <returns></returns>
        public override bool Validate(string ocode, string logid, string rightname, string buttonname, params object[] objs)
        {
            return base.services.IsButtonHaveRight(ocode, logid, rightname, buttonname);
        }
    }

    /// <summary>
    /// 特殊按钮权限管理
    /// 适合于一个菜单在两个业务点被注册的情况下
    /// </summary>
    public class ButtonRightByMenu : FunctionRight
    {
        /// <summary>
        /// 判断按钮是否有权限
        /// </summary>
        /// <param name="ocode">组织号</param>
        /// <param name="logid">操作员</param>
        /// <param name="rightname">权限名</param>
        /// <param name="buttonname">按钮名</param>
        /// <param name="objs">功能名</param>
        /// <returns></returns>
        public override bool Validate(string ocode, string logid, string rightname, string buttonname, params object[] objs)
        {
            //如果没有传递功能参数进来直接返回false
            if (objs == null) return false;
            return base.services.IsButtonHaveRight(ocode, logid, rightname, buttonname, objs[0].ToString());
        }
    }

    /// <summary>
    /// 功能权限管理
    /// </summary>
    public class FunctionRight : IRight
    {
        protected Services services = new Services();

        public bool Add(IRightEntity entity)
        {
            return services.RightAdd(entity);
        }

        public bool Remove(IRightEntity entity)
        {
            return services.RightDelete(entity);
        }

        public bool Update(IRightEntity entity)
        {
            return services.RightUpdate(entity);
        } 

        public virtual bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return false;
        }

        public CType GetInfo<CType>(string ocode, string logid, params object[] objs)
        {
            CType type = default(CType);
            //type = (CType)DataSource;
            return type;
        }
    }
}
