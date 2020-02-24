using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity
{
    public class BuildParameter
    {
        /// <summary>
        /// 表单的Id号;
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 生成表单的类型;
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 服务端程序生成路径;
        /// </summary>
        public string AssemblyPath { get; set; }
        /// <summary>
        /// 服务端代码文件生成路径;
        /// </summary>
        public string CsFilePath { get; set; }
        /// <summary>
        /// 前段代码文件生成路径;
        /// </summary>
        public string ViewPath { get; set; }
        /// <summary>
        /// url链接的主机地址;
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// url链接的端口号;
        /// </summary>
        public string Port { get; set; }
    }
}
