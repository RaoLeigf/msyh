using NG3.Metadata.Core;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// 显示和数值的成对的数据结构
    /// </summary>
    public class PbPairValueInfo:MetadataGod
    {
        private string _displayValue = string.Empty;

        private string _saveValue = string.Empty;

        public PbPairValueInfo(string displayValue, string saveValue)
        {
            _displayValue = displayValue;
            _saveValue = saveValue;
        }

        /// <summary>
        /// 显示值
        /// </summary>
        public string DisplayValue
        {
            get { return _displayValue; }
            set { _displayValue = value; }
        }

        /// <summary>
        /// 保存值
        /// </summary>
        public string SaveValue
        {
            get { return _saveValue; }
            set { _saveValue = value; }
        }
    }
}