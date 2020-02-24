using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using SUP.Common.Base;
using NG3.Data;
using SUP.Common.DataEntity;
using System.Web;


namespace SUP.Common.DataAccess
{
    public class CommonhelpSettingDac
    {

        public DataTable GetList(string clientJsonQuery, int pageSize, int pageIndex, ref int totalRecord)
        {
            string sql = "select * from fg_helpinfo_master";

            string sortField = " helpid asc ";
            DataTable dt;            

            if (!string.IsNullOrEmpty(clientJsonQuery))
            {
                string query = string.Empty;
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientJsonQuery, string.Empty, ref query);

                if (!string.IsNullOrEmpty(query))
                {
                    sql += " where " + query;
                }

                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField, p);
                dt = DbHelper.GetDataTable(sqlstr, p);
            }
            else
            {
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField);
                dt = DbHelper.GetDataTable(sqlstr);
            }
            return dt;
        }
        
        public DataTable GetMaster(string id)
        {
            string sql = "select * from fg_helpinfo_master where code={0}";
            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("code", id);

            DataTable dt = DbHelper.GetDataTable(sql,p);            
            return dt;
        }

        public DataTable GetSystemField(string masterId)
        {
            string sql = "select * from fg_helpinfo_sys where masterid={0}";
            
            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("masterid", masterId);

            return DbHelper.GetDataTable(sql, p);
        }

        public DataTable GetUserField(string masterId)
        {
            string sql = "select * from fg_helpinfo_user where masterid={0}";
            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("masterid", masterId);

            return DbHelper.GetDataTable(sql, p);
        }

        /// <summary>
        /// 获取明细表的字段信息
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public DataTable GetDetailTableField(string masterId)
        {
            string sql = "select * from fg_helpinfo_detailtable where masterid={0}";
            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("masterid", masterId);

            return DbHelper.GetDataTable(sql, p);
        }

        public DataTable GetQueryProperty(string masterId)
        {
            string sql = "select * from fg_helpinfo_queryprop where masterid={0}";
            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("masterid", masterId);

            DataTable dt = DbHelper.GetDataTable(sql, p);
            return dt;
        }

        public DataTable GetRichQuery(string masterId)
        {
            string sql = "select * from fg_helpinfo_richquery where masterid={0}";
            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("masterid", masterId);

            return DbHelper.GetDataTable(sql, p);
        }

        public int Save(string masterID,CommonHelpSettingEntity entity)
        {
            //处理查询条件的单引号
            //entity.MasterDt.Rows[0]["sqlfilter"] = HttpUtility.UrlEncode(entity.MasterDt.Rows[0]["sqlfilter"].ToString());

            foreach (DataRow dr in entity.UserFieldDt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr["code"] = Guid.NewGuid().ToString();
                    dr["masterid"] = masterID;
                }
            }

            foreach (DataRow dr in entity.DetailDt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr["code"] = Guid.NewGuid().ToString();
                    dr["masterid"] = masterID;
                }
            }

            foreach (DataRow dr in entity.QueryProperty.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr["code"] = Guid.NewGuid().ToString();
                    dr["masterid"] = masterID;                  
                }

                if (!string.IsNullOrWhiteSpace(dr["tree_sql"].ToString()))
                {
                    //dr["tree_sql"] = HttpUtility.UrlEncode(dr["tree_sql"].ToString());
                }
            }
            foreach (DataRow dr in entity.RichQuery.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr["code"] = Guid.NewGuid().ToString();
                    dr["masterid"] = masterID;
                }
            }

            int iret = 0;
            iret = DbHelper.Update(entity.MasterDt, "select * from fg_helpinfo_master");
            iret += DbHelper.Update(entity.SystemFieldDt, "select * from fg_helpinfo_sys");
            iret += DbHelper.Update(entity.UserFieldDt, "select * from fg_helpinfo_user");
            iret += DbHelper.Update(entity.DetailDt, "select * from fg_helpinfo_detailtable");
            iret += DbHelper.Update(entity.QueryProperty, "select * from fg_helpinfo_queryprop");
            iret += DbHelper.Update(entity.RichQuery, "select * from fg_helpinfo_richquery");

            return iret;

        }
    }
}
