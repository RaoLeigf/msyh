using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ToHtmlModel
    {
        /// <summary>
        /// 转换纯文本内容为 HTML 内容
        /// </summary>
        /// <param name="Text">纯文本内容</param>
        /// <returns>转换后的 HTML 内容</returns>
        public static string ToHTML(string Text)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace("<p>" + Text + "</p>", "\r\n", "</p><p>"), "\r", "</p><p>"), "\n", "<br />"), "\t", "    "), "  ", "  ");
        }
        /// <summary>
        /// 转换 Html 内容为纯文本内容
        /// </summary>
        /// <param name="HTML">HTML 内容</param>
        /// <returns>转换后的纯文本内容</returns>
        public static string ToText(string HTML)
        {
            string input = HTML;
            return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(input, @"(?m)<script[^>]*>(\w|\W)*?</script[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(?m)<style[^>]*>(\w|\W)*?</style[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(?m)<select[^>]*>(\w|\W)*?</select[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(?m)<a[^>]*>(\w|\W)*?</a[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), "(<[^>]+?>)| ", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(\s)+", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }
    }
}
