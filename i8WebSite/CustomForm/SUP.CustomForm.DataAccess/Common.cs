using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using NG3.Data;
using NG3.Data.Service;

namespace SUP.CustomForm.DataAccess
{
    public class Common
    {
        //任务分解用到的明细表的分组小计信息
        public static DataSet GetSubtotalDS(string eform, string phidwork)
        {
            DataSet ds = new DataSet();

            try
            {
                //根据流程id获取模板id
                string phidtemplate = DbHelper.GetString("select phidtemplate from work_breakdown where phid = " + phidwork);

                if (!string.IsNullOrEmpty(phidtemplate))
                {
                    //根据模板id获取明细表合计信息
                    DataTable dtDetail = DbHelper.GetDataTable("select * from work_templategroup where templatephid = " + phidtemplate);

                    //所有明细表汇总小计信息
                    DataTable[] dtSubtotal = new DataTable[dtDetail.Rows.Count];

                    for (int i = 0; i < dtDetail.Rows.Count; i++)
                    {
                        string tableName = dtDetail.Rows[i]["tablename"].ToString();
                        string groupphid = dtDetail.Rows[i]["uicontainerphid"].ToString();

                        dtSubtotal[i] = DbHelper.GetDataTable("select dbfield,isgroup,issum,func from work_templateprope where templatephid = " + phidtemplate + " and groupphid = " + groupphid);
                        dtSubtotal[i].TableName = tableName;
                        ds.Tables.Add(dtSubtotal[i]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("GetSubtotalDS: " + e.Message);
            }

            return ds;
        }

        public static DataTable GetHelpInfo(string helpid)
        {
            string sql = string.Format("select * from p_form_base where phid='{0}'", helpid);
            DataTable dt = DbHelper.GetDataTable(sql); 

            if (dt.Rows.Count > 0)
            {
                //弹出式的帮助
                if (dt.Rows[0]["fromsql"].Equals("1"))
                {
                    return dt;
                }
                else
                { //下拉式帮助
                    sql = string.Format("select * from p_form_base where billtype='{0}' order by phid asc", dt.Rows[0]["code"]);
                    dt = DbHelper.GetDataTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    return null;
                }
            }
            return null;
        }

        public static DataTable GetComboInfo(string helpid)
        {
            string sql = string.Format("select phid, base_code, base_name from p_form_base where billtype='{0}' order by phid asc", helpid);
            DataTable dt = DbHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            return null;
        }

        /// <summary>
        /// 获取存储过程的参数名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetProcedureParams(string name)
        {
            string sql = string.Empty;
            if (DbHelper.Vendor == DbVendor.Oracle)
            {
                sql =
                    string.Format(
                        @"select ARGUMENT_NAME as name from user_arguments where object_name='{0}' ORDER BY SEQUENCE ", name);
            }
            else
            {
                sql =
                    string.Format(@"select name from sys.parameters where object_id = object_id('{0}') ORDER BY parameter_id 
", name);
            }

            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }


        public static DataTable GetDataTable(string connectionString, string commandText, params SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.Parameters.AddRange(parms);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }
    }
}
