using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Dac
{
    public class DacSupport
    {
      
        public object Execute(IDacCallback callback)
        {
            try
            {
                
                DbHelper.Open();
                object result =  callback.DoInCallback();
                return result;

            }
            catch (Exception)
            {
                throw ;
            }
            finally
            {
                DbHelper.Close();
            }
        }


        public DataTable QueryForDataTable(string sql, IDataParameter[] paras)
        {
            IDacCallback callback = new ParametersDacCallback(sql, paras);
            return Execute(callback) as DataTable;
        }

        public DataTable QueryForDataTable(string sql)
        {
            IDacCallback callback = new SqlDacCallback(sql);
            return Execute(callback) as DataTable;
        }

        public decimal QueryForDecimal(string sql)
        {
            IDacCallback callback = new SqlDacScalarCallback(sql);
            object obj = Execute(callback);
            if (obj == null) return 0; //返回0还是空，自己处理
            return Convert.ToDecimal(obj);
        }
    }

    #region 实现类
    internal class ParametersDacCallback : IDacCallback
    {
        private string sql;
        private IDataParameter[] paras;

        public ParametersDacCallback(string sql, IDataParameter[] paras)
        {
            this.sql = sql;
            this.paras = paras; //参数
        }

        public object DoInCallback()
        {
            return DbHelper.GetDataTable(sql, paras);
        }
    }


    internal class SqlDacCallback : IDacCallback
    {
        private string sql;

        public SqlDacCallback(string sql)
        {
            this.sql = sql;
        }

        public object DoInCallback()
        {
            return DbHelper.GetDataTable(sql);
        }
    }


    internal class SqlDacScalarCallback : IDacCallback
    {
        private string sql;

        public SqlDacScalarCallback(string sql)
        {
            this.sql = sql;
        }

        public object DoInCallback()
        {
            return DbHelper.ExecuteScalar(sql);
        }
    }


    #endregion
}
