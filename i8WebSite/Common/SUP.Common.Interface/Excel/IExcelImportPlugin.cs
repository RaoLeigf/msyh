using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.Interface.Excel
{
    public interface IExcelImportPlugin
    {
        /// <summary>
        /// Excel导入插件方法
        /// </summary>
        /// <param name="ds">需要导入的数据集</param>
        /// <param name="selected">是否覆盖，0:覆盖，1:不覆盖，2:停止导入</param>
        /// <param name="message">导入结果信息说明</param>
        /// <returns></returns>
        bool ImportBill(DataSet ds, string selected, ref string message);
    }
}
