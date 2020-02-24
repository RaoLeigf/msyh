using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.Base;
using NG3.Data.Service;
using NG3.Data;

namespace SUP.Common.DataAccess
{
    public class IndividualPropertyDac
    {

        public IndividualPropertyDac() { 
        }

        /// <summary>
        /// 获取表注册列表
        /// </summary>
        /// <param name="clientJson"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetTableRegList(string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            //string sql = "select * from fg_table_reg";
            //string sql = " select fg_table_reg.*, fg_individialinfo_reg.busname from fg_table_reg,fg_individialinfo_reg "
            //              + " where fg_table_reg.individualreg_code = fg_individialinfo_reg.code ";

            string sql = " select c_code,c_name,chn from fg_table ";

            string sortField = " c_code asc ";
            if (!string.IsNullOrEmpty(clientJson))
            {
                string query = string.Empty;
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientJson, string.Empty, ref query);

                if (!string.IsNullOrEmpty(query))
                {
                    if (sql.IndexOf("where") > 0)
                    {
                        sql += " and " + query;
                    }
                    else
                    {
                        sql += " where " + query;
                    }
                }

                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref PageIndex, ref totalRecord, sortField, p);
                return DbHelper.GetDataTable(sqlstr, p);
            }
            else
            {
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref PageIndex, ref totalRecord, sortField);
                return DbHelper.GetDataTable(sqlstr);
            }
        }

        /// <summary>
        /// 获得表注册信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetTableRegInfo(string tablename, Int64 busPhid)
        {
            //string sql = "select * from fg_table_reg where code={0}";
            //string sql = "select * from fg_table where c_name={0}";
            string sql = "select * from  metadata_bustable where tname={0} and busphid={1}";

            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("tname", DbType.AnsiString);
            p[0].Value = tablename;
            p[1] = new NGDataParameter("busphid", DbType.UInt64);
            p[1].Value = busPhid;


            return DbHelper.GetDataTable(sql, p);
            
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="tableregcode"></param>
        /// <returns></returns>
        public DataTable GetColumnsInfo(string tablename)
        {
            //string sql = "select * from fg_column_reg where tablereg_code={0}";

            string sql = "select * from fg_columns where c_kind='user' and c_bname={0} order by c_name";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("c_bname", DbType.AnsiString);
            p[0].Value = tablename;

            DataTable dt = DbHelper.GetDataTable(sql, p);        
            return dt;           
        }

        /// <summary>
        /// 保存字段注册信息
        /// </summary>
        /// <param name="columnregdt"></param>
        /// <returns></returns>
        public int Save(DataTable columnregdt)
        {
            foreach (DataRow dr in columnregdt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr["c_code"] = Guid.NewGuid().ToString();
                    dr["c_kind"] = "user";
                }
            }

            string sql = "select * from fg_columns";

            int iret = DbHelper.Update(columnregdt, sql);

            return iret;
        }

        /// <summary>
        /// 根据业务类型获取表注册信息
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public DataTable GetTableRegByBusType(string bustype)
        {
            //string sql = "select code,tname,uitype from fg_table_reg where bustype={0}";
            string sql = "select distinct fg_columns.c_bname from fg_col_uireg,fg_columns "
                           + " where fg_col_uireg.columnreg_code = fg_columns.c_code "
                           + " and bustype={0} ";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype;

            return DbHelper.GetDataTable(sql, p);
        }

        /// <summary>
        /// 通过业务类型获取字段信息
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public DataTable GetColumnsByBusType(string bustype)
        {
            //string sql = " select fg_column_reg.* from fg_table_reg,fg_column_reg "
            //               + " where fg_table_reg.code = fg_column_reg.tablereg_code "
            //               + " and fg_table_reg.bustype='" + bustype + "'";

            StringBuilder strb = new StringBuilder();
            strb.Append("select fg_columns.c_bname,fg_columns.c_name,fg_columns.c_fullname,fg_columns.c_type,fg_columns.collen,fg_columns.declen,");
            strb.Append(" fg_col_uireg.uixtype,fg_col_uireg.container_uitype,fg_col_uireg.helpid,fg_col_uireg.querymode,fg_col_uireg.combodata");
            strb.Append(" from fg_col_uireg,fg_columns ");
            strb.Append(" where fg_col_uireg.columnreg_code = fg_columns.c_code ");
            strb.Append(" and bustype={0} ");

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype;


            return DbHelper.GetDataTable(strb.ToString(),p);
        }

        /// <summary>
        /// 创建字段
        /// </summary>
        public void CreateColumn(DataTable columnregdt)
        {
            StringBuilder strb = new StringBuilder();

            foreach (DataRow dr in columnregdt.Rows)
            {
                strb.Clear();

                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    string coltype = dr["c_type"].ToString();
                    string colname = dr["c_name"].ToString();
                    string tname = dr["c_bname"].ToString();


                    strb.Append("alter table ");
                    strb.Append(tname);
                    strb.Append(" add ");
                    strb.Append(colname);
                    strb.Append(" " + this.GetColomnType(coltype));

                    if (coltype == "02" || coltype == "06")//varchar, numeric
                    {
                        string length = dr["collen"].ToString();

                        strb.Append("(" + length);
                        if (coltype == "06")
                        {
                            string declen = dr["declen"].ToString();
                            strb.Append("," + declen);
                        }
                        strb.Append(")");
                    }

                    DbHelper.ExecuteNonQuery(strb.ToString());
                }
                
            }
        }

        //修改字段
        public void ModifyColumn(DataTable columnregdt)
        {
                        
            foreach (DataRow dr in columnregdt.Rows)
            {
                string sql = string.Empty;

                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Modified)
                {
                    string coltype = dr["c_type"].ToString();
                    string colname = dr["c_name"].ToString();
                    string tname = dr["c_bname"].ToString();
                    string length = dr["collen"].ToString();
                    string declen = dr["declen"].ToString();
 
                    if (DbHelper.Vendor == DbVendor.Oracle)
                    {
                        if (coltype == "02")
                        {
                            sql = " alter table " + tname + " modify " + colname + " VARCHAR2(" + length + ")  ";
                        }
                        else if (coltype == "06")
                        {
                            sql += " alter table " + tname + " modify " + colname + " NUMBER(" + length + "," + declen + ")  ";
                        }
                    }
                    else
                    {
                        if (coltype == "02")
                        {
                            sql = " alter table " + tname + " alter column " + colname + " varchar(" + length + ")  ";
                        }
                        else if (coltype == "06")
                        {
                            sql += " alter table " + tname + " alter column " + colname + " numeric(" + length + "," + declen + ")  ";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(sql))
                    {
                        DbHelper.ExecuteNonQuery(sql);
                    }                    
                  
                }
            }

        }

        //删除字段
        public void DeleteColumn(DataTable columnregdt)
        {
            foreach (DataRow dr in columnregdt.Rows)
            {
                string sql = string.Empty;
                if (dr.RowState == DataRowState.Deleted)
                {
                    string colname = dr["c_name"].ToString();
                    string tname = dr["c_bname"].ToString();

                    if (DbHelper.Vendor == DbVendor.Oracle)
                    {
                        sql = "alter table " + tname + " drop column " + colname;
                    }
                    else
                    {
                        sql = "alter table " + tname + " drop " + colname;
                    }

                    DbHelper.ExecuteNonQuery(sql);

                }            

            }
        }

        //获取列信息
        private string GetColomnType(string type)
        {
            string coltype = string.Empty;
            if (DbHelper.Vendor == DbVendor.SQLServer)
            {
                switch (type)
                {
                    case "02": coltype = "varchar";
                        break;
                    case "03": coltype = "integer";
                        break;
                    case "04": coltype = "smallint";
                        break;
                    case "05": coltype = "tinyint";
                        break;
                    case "06": coltype = "numeric";
                        break;
                    case "07": coltype = "datetime";
                        break;
                    case "09": coltype = "text";
                        break;
                    case "10": coltype = "bigint";
                        break;
                    default: coltype = "varchar";
                        break;
                } 
            }

            if (DbHelper.Vendor == DbVendor.Oracle)
            {
                switch (type)
                {
                    case "02": coltype = "VARCHAR2";
                        break;
                    case "03": coltype = "NUMBER(10,0)";
                        break;
                    case "04": coltype = "NUMBER(5,0)";
                        break;
                    case "05": coltype = "NUMBER(1,0)";
                        break;
                    case "06": coltype = "NUMBER";
                        break;
                    case "07": coltype = "DATE";
                        break;
                    case "10": coltype = "NUMBER(17,0)";
                        break;
                    default: coltype = "VARCHAR2";
                        break;
                } 
            }

            return coltype;

        }

        public DataTable GetColumnInfo(string tname)
        {
            string sql = "select fieldname,fieldchn,c_type from fg_columns where tname = {0} and listshow_flg='1'";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("tname", DbType.AnsiString);
            p[0].Value = tname;

            return DbHelper.GetDataTable(sql, p);
        }

        //获取自定义字段组装sql语句, 以逗号分隔
        public string GetIndividualFields(string tname)
        {
            string sql = "select c_name from fg_columns where c_bname = {0} ";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("c_bname", DbType.AnsiString);
            p[0].Value = tname;

            DataTable dt = DbHelper.GetDataTable(sql, p);

            if (dt.Rows.Count == 0) return string.Empty;

            StringBuilder strb = new StringBuilder();

            int count = dt.Rows.Count - 1;
            for (int i = 0; i < count; i++)
            {
                strb.Append(dt.Rows[i]["c_name"].ToString());
                strb.Append(",");

            }

            strb.Append(dt.Rows[count]["c_name"].ToString());

            return strb.ToString();
        }

        /// <summary>
        /// 获取业务类型列表
        /// </summary>
        /// <param name="clientJson">客户端查询条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="PageIndex">页号</param>
        /// <param name="totalRecord">总记录数</param>
        /// <returns></returns>
        public DataTable GetBusTypeList(string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            string sql = "select c_code,sys_module,user_code,doc_title from c_pfc_register ";

            string sortField = " c_code asc ";
            if (!string.IsNullOrEmpty(clientJson))
            {
                string query = string.Empty;
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientJson, string.Empty, ref query);

                if (!string.IsNullOrEmpty(query))
                {
                    if (sql.IndexOf("where") > 0)
                    {
                        sql += " and " + query;
                    }
                    else
                    {
                        sql += " where " + query;
                    }
                }

                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref PageIndex, ref totalRecord, sortField, p);
                return DbHelper.GetDataTable(sqlstr, p);
            }
            else
            {
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref PageIndex, ref totalRecord, sortField);
                return DbHelper.GetDataTable(sqlstr);
            }
        }

        /// <summary>
        /// 获取字段ui设置列表
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public DataTable GetPropertyUIInfo(string tablename, string bustype)
        {
            string sql = "select fg_columns.c_bname,fg_columns.c_name,fg_columns.c_fullname,fg_columns.c_type,fg_col_uireg.*,'' as helpid_name  from fg_col_uireg,fg_columns"
                          + " where fg_col_uireg.columnreg_code = fg_columns.c_code "
                          + " and c_bname={0} and fg_col_uireg.bustype={1} order by fg_columns.c_bname ";

            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("tablename", DbType.AnsiString);
            p[0].Value = tablename;
            p[1] = new NGDataParameter("bustype", DbType.AnsiString);
            p[1].Value = bustype;

            DataTable dt = DbHelper.GetDataTable(sql, p);

            //代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            helpdac.CodeToName("helpid", "helpid_name","helpid",dt);

            return dt;

        }

        /// <summary>
        /// 保存自定义字段ui信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int SavePropertyUIInfo(DataTable dt)
        {
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr.RowState == DataRowState.Deleted) continue;

            //    if (dr.RowState == DataRowState.Added)
            //    {
            //        //dr["code"] = Guid.NewGuid().ToString();                    
            //    }
            //}

            return DbHelper.Update(dt, "select * from fg_col_uireg");
        }

        /// <summary>
        /// 判断字段UI信息是否被引用
        /// </summary>
        /// <param name="fieldUIId"></param>
        /// <returns></returns>
        public List<string> GetInUseFiedlUIInfo(string fieldUIId)
        {
            string sql = string.Format("SELECT name FROM fg_individualinfo where individualinfo LIKE'%{0}%'",fieldUIId);
            DataTable dt = DbHelper.GetDataTable(sql);
            List<string> uiInfoName = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                uiInfoName.Add(dr["name"].ToString());
            }

            return uiInfoName;
        }


        /// <summary>
        /// 获取业务类型树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> LoadBusTree(string nodeid,string tablename)
        {
            string sql = string.Empty;
            string filter = string.Empty;
            StringBuilder sb = new StringBuilder();

            if ("root" == nodeid)//首次加载
            {
                if (tablename == "")
                {
                    sb.Append("select phid,id,pid,code,busname,nodetype from tk_metadata_bustree order by phid");
                }
                else {
                    sb.Append("select distinct tk_metadata_bustree.phid, tk_metadata_bustree.id, tk_metadata_bustree.pid, tk_metadata_bustree.code, tk_metadata_bustree.busname, nodetype from tk_metadata_bustree ");
                    sb.Append(string.Format(" inner join {0} on {0}.busphid = tk_metadata_bustree.phid ", tablename));
                    sb.Append(" UNION select tk_metadata_bustree.phid,id,pid,code,tk_metadata_bustree.busname,nodetype ");
                    sb.Append(" from tk_metadata_bustree where nodetype = '0' or nodetype = '1' order by  phid");
                }
                filter = "(pid is null or pid='')";

                DataTable dt = DbHelper.GetDataTable(sb.ToString());

                return new BusTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);
            }
            else
            {
                sql = string.Format("select phid,id,pid,code,busname,nodetype from tk_metadata_bustree order by phid ", nodeid);
                DataTable dt = DbHelper.GetDataTable(sql);

                return new BusTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.LazyLevel);
            }

        }




        /// <summary>
        /// 获取业务类型对应的表
        /// </summary>
        /// <param name="busID">业务类型phid</param>
        /// <returns></returns>
        public DataTable GetBusTables(string busID)
        {
            string sql = string.Format("select tname,chn from  metadata_bustable where busphid='{0}'", busID);

            return DbHelper.GetDataTable(sql);
        }

        public DataTable GetHelpInfo(string helpid)
        {
            string sql = string.Format("select codefield,namefield from fg_helpinfo_master where helpid='{0}'",helpid);
            return DbHelper.GetDataTable(sql);
        }

        

    }


    class BusTreeJSONBase : TreeJSONBase
    {
        public virtual string phid { get; set; }
        public virtual string bustype { get; set; }
    }
    public class BusTreeBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            BusTreeJSONBase node = new BusTreeJSONBase();

            node.id = dr["id"].ToString();
            node.text = dr["busname"].ToString();           
            node.phid = dr["phid"].ToString();
            node.bustype = dr["code"].ToString();

            node.leaf = (dr["nodetype"].ToString() == "2") ? true : false;

            return node;
        }
    }
}
