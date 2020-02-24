using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Frame.DataEntity
{
    [Serializable]
    public class MacroEntity
    {
        /// <summary>
        /// 宏名
        /// </summary>
        private string _name = "";
        /// <summary>
        /// 宏名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 宏描述
        /// </summary>
        private string _description = "";
        /// <summary>
        /// 宏描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 宏当前值
        /// </summary>
        private string _currentValue = "";
        /// <summary>
        /// 宏当前值
        /// </summary>
        public string CurrentValue
        {
            set { _currentValue = value; }
            get { return _currentValue; }
        }
        /// <summary>
        /// 宏实体
        /// </summary>
        /// <param name="name">宏名</param>
        /// <param name="description">宏描述</param>
        /// <param name="currentValue">宏当前值</param>
        public MacroEntity(string name, string description, string currentValue)
        {
            this._name = name;
            this._description = description;
            this._currentValue = currentValue;
        }
    }
}
