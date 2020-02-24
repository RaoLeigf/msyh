using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using SUP.Common.Base;

namespace SUP.Frame.DataAccess
{
    public class QRCodeSetDac
    {
        /// <summary>
        /// 取二维码规则列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetList(string query, int pageSize, int pageIndex, ref int totalRecord)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT phid,code,content FROM fg3_qrcode_rule");
            DataTable dt;
            string sortString = "code asc";
            if (!string.IsNullOrEmpty(query))
            {
                string q = string.Empty;
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(query, string.Empty, ref q);
                if (!string.IsNullOrEmpty(q))
                {
                    sql.Append(" where " + q);
                }                
                string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), pageSize, ref pageIndex, ref totalRecord, sortString, p);
                dt = DbHelper.GetDataTable(strSql,p);
            }
            else
            {              
                string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), pageSize, ref pageIndex, ref totalRecord, sortString, null);
                dt = DbHelper.GetDataTable(strSql);
            }
            
            return dt;
        }

        public DataTable GetMaster(string id)
        {
            //string sql = String.Format("select * from fg3_qrcode_rule where phid={0}", id);
            string sql = String.Format(@"SELECT * FROM  fg3_qrcode_rule 
                            WHERE phid = {0}",
                            id);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetProduct(string id)
        {
            //string sql = String.Format("select * from fg3_qrcode_rule where phid={0}", id);
            string sql = String.Format(@"SELECT * FROM  fg_busiproduct 
                            WHERE fg_busiproduct.busid = 'fg3_qrcode_rule' AND fg_busiproduct.busikey = {0}",
                            id);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetDetailField(string id)
        {
            string sql = String.Format("select * from fg3_qrcode_rule_detail where originalphid={0}", id);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }

        public int Save(string masterID,DataTable masterDt, DataTable detailDt, DataTable productDt)
        {
            if (masterDt.Rows[0].RowState == DataRowState.Added)
            {
                string sqlExist = string.Format("select * from fg3_qrcode_rule where code='{0}'", masterDt.Rows[0]["code"].ToString());
                DataTable dtExist = DbHelper.GetDataTable(sqlExist);
                if (dtExist != null && dtExist.Rows.Count > 0)
                {
                    throw new Exception("已经存在此业务码，不能新增!");
                }
            }

            if (masterDt.Rows[0].RowState == DataRowState.Modified)
            {
                string sqlExist = string.Format("select * from fg3_qrcode_rule where code='{0}' and phid <> {1}", masterDt.Rows[0]["code"].ToString(), masterDt.Rows[0]["phid"]);
                DataTable dtExist = DbHelper.GetDataTable(sqlExist);
                if (dtExist != null && dtExist.Rows.Count > 0)
                {
                    throw new Exception("已经存在此业务码，不能修改!");
                }
            }


            int iret = 0;
            iret = DbHelper.Update(masterDt, "select * from fg3_qrcode_rule");
            iret += DbHelper.Update(productDt, "select * from fg_busiproduct");
            iret += DbHelper.Update(detailDt, "select * from fg3_qrcode_rule_detail");

            return iret;
        }

        public Int64 GetMaxID(string tableName,out bool isFirstRow)
        {
            string sql = "select max(phid) from " + tableName;
            object obj = DbHelper.ExecuteScalar(sql);

            Int64 iret = 0;
            isFirstRow = true;
            if (obj != null && obj != DBNull.Value)
            {
                Int64.TryParse(obj.ToString(), out iret);
                isFirstRow = false;
            }
            return iret;
        }

        public int Delete(string masterIdString)
        {
            int masterId = -1;
            int.TryParse(masterIdString, out masterId);
            int result = -1;
            string sqlstr = string.Format("DELETE FROM fg3_qrcode_rule WHERE phid={0}", masterId);
            result = DbHelper.ExecuteNonQuery(sqlstr);
            sqlstr = string.Format("DELETE FROM fg3_qrcode_rule_detail WHERE originalphid={0}", masterId);
            result += DbHelper.ExecuteNonQuery(sqlstr);
            if (result > 0)
            {
                sqlstr = string.Format("DELETE FROM fg_busiproduct WHERE busid='fg3_qrcode_rule' AND busikey={0}", masterId);
                result += DbHelper.ExecuteNonQuery(sqlstr);
            }
            return result;
        }

        
        public DataTable GetQrStyle(string id)
        {
            string sql = String.Format(@"SELECT * FROM fg3_qrcode_style 
                            WHERE phid = {0}",
                            id);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }
        public int SaveQrStyle(DataTable masterDt)
        {
            int iret = DbHelper.Update(masterDt, "select * from fg3_qrcode_style");
            return iret;
        }

        public int SaveImgName(string imgname)
        {
            int iret = DbHelper.ExecuteNonQuery("update fg3_qrcode_style set imgname = '"+ imgname + "'");
            return iret;
        }

        public string GetImgName()
        {
            //string sql = String.Format(@"SELECT top 1 imgname FROM fg3_qrcode_style");
            string sql = String.Format(@"SELECT imgname FROM fg3_qrcode_style");
            string s = DbHelper.GetString(sql);
            return s;
        }

        public DataTable GetGridByBusTree(string busphid)
        {
            string sql = 
            String.Format(@"SELECT metadata_uictrlinfo.phid, metadata_bustree.busname as bustype,metadata_uicontainer.tablename ,metadata_uictrlinfo.ctl_label as name
                            FROM metadata_uictrlinfo
                            LEFT JOIN metadata_uicontainer
                            ON metadata_uictrlinfo.masterid=metadata_uicontainer.phid
                            LEFT JOIN metadata_bustree
                            on  metadata_bustree.phid = metadata_uicontainer.busphid 
                            where metadata_bustree.phid = {0}", busphid);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }

        public string getPhidByCode(string code)
        {
            string sql = String.Format(@"SELECT phid FROM fg3_qrcode_rule where code = {0}", code);
            string s = DbHelper.GetString(sql);
            return s;
        }

        public DataTable getUrlByCode(string code)
        {
            string sql = String.Format(@"SELECT controllerurl,viewurl FROM fg3_qrcode_rule 
                            WHERE code = {0}",
                            code);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }
    }
}
