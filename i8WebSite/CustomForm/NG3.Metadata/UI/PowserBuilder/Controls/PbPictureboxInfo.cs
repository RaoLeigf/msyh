using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// 图片框控件
    /// </summary>
    public class PbPictureboxInfo : PbBaseControlInfo
    {
        public PbPictureboxInfo()
        {
            ControlType = PbControlType.Picturebox;
        }

        private string _filePath = string.Empty;
        private bool _isFromAttachment = false;
        private string _fileName = string.Empty;

        /// <summary>
        /// 图片路径(URL地址)
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <summary>
        /// 是否来自附件
        /// </summary>
        public bool IsFromAttachment
        {
            get { return _isFromAttachment; }
            set { _isFromAttachment = value; }
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

    }
}
