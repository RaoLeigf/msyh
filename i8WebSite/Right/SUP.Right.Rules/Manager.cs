using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Interface;
using SUP.Right.Rules;

namespace SUP.Right.Rules
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// 权限类型工厂
        /// </summary>
        /// <param name="type">权限类型</param>
        /// <returns></returns>
        public IRight GetInstance(RightType type)
        {
            IRight right;

            switch (type)
            {
                case RightType.Function:
                    right = new FunctionRight();
                    break;
                case RightType.EntryRight:
                    right = new EntryRight();
                    break;
                case RightType.ButtonRight:
                    right = new ButtonRight();
                    break;
                case RightType.ButtonRightByMenu:
                    right = new ButtonRightByMenu();
                    break;
                case RightType.Field:
                    right = new FieldRight();
                    break;
                default:
                    right = null;
                    break;
            }
            return right;

        }

        /// <summary>
        /// 入口权限
        /// </summary>
        public IRight EntryRight
        {
            get
            {
                return GetInstance(RightType.EntryRight);
            }
        }

        /// <summary>
        /// 按钮权限
        /// </summary>
        public IRight ButtonRight
        {
            get
            {
                return GetInstance(RightType.ButtonRight);
            }
        }

        /// <summary>
        /// 一个菜单在两个业务点被注册的按钮权限
        /// </summary>
        public IRight ButtonRightByMenu
        {
            get
            {
                return GetInstance(RightType.ButtonRightByMenu);
            }
        }

        /// <summary>
        /// 域权限
        /// </summary>
        public IRight FieldRight {
            get {
                return GetInstance(RightType.Field);
            }
        }

        /// <summary>
        /// 信息权限/动作权限
        /// </summary>
        public IRight InfoRight {
            get {
                return GetInstance(RightType.Info);
            }
        }
    }

}
