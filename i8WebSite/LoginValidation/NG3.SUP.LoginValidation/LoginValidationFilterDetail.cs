using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.SUP.LoginValidation
{
    /// <summary>
    /// 每一个验证过滤器详细信息
    /// </summary>
    [Serializable]
    public sealed class LoginValidationFilterDetail
    {
        /// <summary>
        /// 过滤器唯一编号
        /// </summary>
        private string _id = string.Empty;

        private string _name = string.Empty;

        private string _description = string.Empty;

        /// <summary>
        /// 过滤器顺序号
        /// </summary>
        private int _seq = -1;

        private string _fullType = string.Empty;

        private bool _isNeedUi = false;


        public LoginValidationFilterDetail(string id, string name, string description, int seq, string fullType, bool isNeedUi)
        {
            _id = id;
            _isNeedUi = isNeedUi;
            _fullType = fullType;
            _seq = seq;
            _description = description;
            _name = name;
        }

        /// <summary>
        /// 过滤器是否需要展示UI
        /// </summary>
        public bool IsNeedUi
        {
            get { return _isNeedUi; }
        }

        /// <summary>
        /// 过滤器程序集类型(反射容器使用)
        /// </summary>
        public string FullType
        {
            get { return _fullType; }
        }

        /// <summary>
        /// 过滤器排序号(序号相同的多线程调用)
        /// </summary>
        public int Seq
        {
            get { return _seq; }
        }

        /// <summary>
        /// 过滤器描述
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// 过滤器名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 过滤器唯一编号
        /// </summary>
        public string Id
        {
            get { return _id; }
        }
    }
}
