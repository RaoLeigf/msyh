using NG3.Data;
using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.DataAccess
{
    /// <summary>
    /// 自定义表单
    /// </summary>
    public class IndividualFormDac
    {

        public int Save(DataTable dt)
        {
            string sql = "select * from fg_individualform";

            int iret = DbHelper.Update(dt, sql);

            return iret;
        }

        /// <summary>
        /// 获取某一业务类型的自定义表单列表
        /// </summary>
        /// <param name="bustype">业务类型</param>
        /// <returns></returns>
        public DataTable GetIndividualFormList(string bustype)
        {
            string sql = "select phid,name,bustype,remark from fg_individualform where bustype={0}";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype ?? string.Empty;

            return DbHelper.GetDataTable(sql, p);

        }

    }
}
