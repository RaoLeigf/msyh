using NG3.Data.Service;
using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.DataAccess
{
    public class BillCodeRuleDac
    {
        /// <summary>
        /// 获取编码规则页面信息
        /// </summary>
        /// <param name="c_btype"></param>
        /// <returns></returns>
        public DataTable GetBillCodeRuleList(string c_btype)
        {
            try
            {
                StringBuilder sbsql = new StringBuilder();
                sbsql.Append("select * from c_pfc_billnorule_m");
                if (c_btype != "-1")
                {
                    sbsql.Append(" where c_btype = '" + c_btype + "' and mark = 1");
                }
                else
                {
                    sbsql.Append(" where mark = 1");
                }
                DataTable dt = new DataTable();
                dt = DbHelper.GetDataTable(sbsql.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取规则设置页面列表信息
        /// </summary>
        /// <param name="c_bcode"></param>
        /// <returns></returns>
        public DataTable GetBillCodeRuleDetailList(string c_bcode)
        {
            try
            {
                string sql = @"SELECT * from c_pfc_billnorule_d where c_bcode = '" + c_bcode + "' and mark = '1' order by sortfield";
                DataTable dt = new DataTable();

                dt = DbHelper.GetDataTable(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="logind"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public DataTable GetInfo(string logind, long orgid)
        {
            try
            {
                string deptno = DbHelper.GetString("select deptno from secuser where logid = '" + logind + "'");
                string ocode = DbHelper.GetString("select ocode from fg_orglist where phid = '" + orgid + "'");
                string codevalue = DbHelper.GetString("select codevalue from fg_orglist where phid = '" + orgid + "'");
                DataTable dt = new DataTable();
                dt.Columns.Add("deptno", Type.GetType("System.String"));
                dt.Columns.Add("ocode", Type.GetType("System.String"));
                dt.Columns.Add("codevalue", Type.GetType("System.String"));
                DataRow dr = dt.NewRow();
                if (string.IsNullOrEmpty(deptno))
                {
                    dr["deptno"] = ocode;
                }
                else
                {
                    dr["deptno"] = deptno;
                }
                dr["ocode"] = ocode;
                dr["codevalue"] = codevalue;
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取帮助信息
        /// </summary>
        /// <param name="busphid"></param>
        /// <returns></returns>
        public DataTable GetBillInfoHelp(string containerid)
        {
            try
            {
                string sql = string.Format(@"SELECT t1.DBFIELD as code,t1.CTL_LABEL as name FROM METADATA_UICTRLINFO t1
LEFT JOIN METADATA_UICONTAINER t2 
ON t1.MASTERID = t2.PHID
WHERE t1.isbillcoderule = 1 AND  t2.CONTAINERID = '{0}'", containerid);
                DataTable fieldsdt = DbHelper.GetDataTable(sql);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取toolkit注册的业务类型
        /// </summary>
        /// <param name="containerid"></param>
        /// <returns></returns>
        public DataTable GetBillType(string busphid)
        {
            try
            {
                string sql = string.Format(@"select c_code,doc_title from c_pfc_register where busphid = '{0}'", busphid);
                DataTable dt = DbHelper.GetDataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetTypeName(string c_code)
        {
            try
            {
                string sql = (@"select doc_title from c_pfc_register where c_code = '" + c_code +"'");
                return DbHelper.GetString(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存编码规则
        /// </summary>
        /// <param name="dt"></param>
        public int Save(DataTable dt, string c_btype, DataTable deldt)
        {
            try
            {

                #region 保存编码规则
                int num = 0;
                int count = 0;
                string sql = "select * from c_pfc_billnorule_m";
                string max_sql = "select max(c_code) from c_pfc_billnorule_m";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        count++;
                        dr["c_code"] = GetMaxId(max_sql,count);
                        dr["mark"] = "1";
                        dr["c_btype"] = c_btype;
                    }
                }
                #endregion

                #region 如果有删除的编码规则删除对应编码规则设置数据
                if(deldt != null || deldt.Rows.Count != 0)
                {
                    foreach (DataRow deldr in deldt.Rows)
                    {
                        DbHelper.ExecuteNonQuery(string.Format(@"delete from c_pfc_billnorule_d where c_bcode = '{0}'", deldr["key"]));
                    }
                }
                #endregion
                #region 删除组织对应方案
                //if (deldt != null || deldt.Rows.Count != 0)
                //{
                //    foreach (DataRow deldr in deldt.Rows)
                //    {
                //        DbHelper.ExecuteNonQuery(string.Format(@"delete from c_pfc_billnorule_org where billrule_m_code = '{0}'", deldr["key"]));
                //    }
                //}
                #endregion
                num = DbHelper.Update(dt, sql);
                return num;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存编码规则明细
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int SaveDetails(DataTable dt, string loginid, long orgid)
        {
            try
            {
                string ocode = DbHelper.GetString("select ocode from fg_orglist where phid = '" + orgid +"'");
                string codevalue = DbHelper.GetString("SELECT codevalue from fg_orglist where phid = '" + orgid + "'");
                string deptno = DbHelper.GetString("select deptno from secuser where logid = '" + loginid +"'");
                string serialnumsql = "select max(codeitemvalue) from c_pfc_billnorule_d where codeitem = 'serialnum'";
                string serialnumstr = DbHelper.GetString(serialnumsql);
                int transfernum = 0;
                if (!string.IsNullOrEmpty(serialnumstr))
                {
                    transfernum = Int32.Parse(serialnumstr);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dt.Rows[i]["phid"].ToString()))
                    {
                        dt.Rows[i]["phid"] = Guid.NewGuid().ToString();
                    }
                    #region codeitemvalue赋值
                    if (dt.Rows[i]["codeitem"].ToString() == "custom")
                    {
                        dt.Rows[i]["codeitemvalue"] = dt.Rows[i]["codeitemcontent"];
                    }
                    else if (dt.Rows[i]["codeitem"].ToString() == "systeminfo")
                    {
                        switch (dt.Rows[i]["codeitemcontent"].ToString())
                        {
                            case "operatorno":
                                if (string.IsNullOrEmpty(loginid))
                                {
                                    dt.Rows[i]["codeitemvalue"] = dt.Rows[i]["alternative"];
                                }
                                else
                                {
                                    dt.Rows[i]["codeitemvalue"] = loginid;
                                }
                                break;
                            case "deptno":
                                if (string.IsNullOrEmpty(deptno))
                                {
                                    dt.Rows[i]["codeitemvalue"] = dt.Rows[i]["alternative"];
                                }
                                else
                                {
                                    dt.Rows[i]["codeitemvalue"] = deptno;
                                }
                                break;
                            case "ocode":
                                if (string.IsNullOrEmpty(ocode))
                                {
                                    dt.Rows[i]["codeitemvalue"] = dt.Rows[i]["alternative"];
                                }
                                else
                                {
                                    dt.Rows[i]["codeitemvalue"] = ocode;
                                }
                                break;
                            case "codevalue":
                                if (string.IsNullOrEmpty(codevalue))
                                {
                                    dt.Rows[i]["codeitemvalue"] = dt.Rows[i]["alternative"];
                                }
                                else
                                {
                                    dt.Rows[i]["codeitemvalue"] = codevalue;
                                }
                                break;
                        }
                    }
                    else if (dt.Rows[i]["codeitem"].ToString() == "billinfo")
                    {
                        dt.Rows[i]["codeitemvalue"] = dt.Rows[i]["codeitemcontent"];
                    }
                    else if (dt.Rows[i]["codeitem"].ToString() == "datetime")
                    {
                        dt.Rows[i]["codeitemvalue"] = DateTime.Now.ToString(dt.Rows[i]["codeitemcontent"].ToString());
                    }
                    else if (dt.Rows[i]["codeitem"].ToString() == "serialnum")
                    {
                        if (string.IsNullOrEmpty(dt.Rows[i]["codeitemvalue"].ToString()))
                        {
                            int count = Int32.Parse(dt.Rows[i]["length"].ToString());
                            transfernum = transfernum + 1;
                            dt.Rows[i]["codeitemvalue"] = (transfernum).ToString().PadLeft(count, '0');
                        }

                    }
                    #endregion
                    dt.Rows[i]["mark"] = "1";
                }
                int num = 0;
                string sql = "select * from c_pfc_billnorule_d";
                num = DbHelper.Update(dt, sql);
                return num;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存对应组织的方案
        /// </summary>
        /// <param name="dt"></param>
        public object SaveRuleDistribution(DataTable dt, DataTable checkdt, string[] strArray, string billRule_m_code)
        {
            try
            {
                DbHelper.Open();
                int count = 0;
                int sign = 0;
                string sb_check_sql = "select org_code from c_pfc_billnorule_org where billRule_m_code = '" + billRule_m_code + "'";
                string sql = "select * from c_pfc_billnorule_org";
                string maxsql = "select max(phid) from c_pfc_billnorule_org";
                StringBuilder sbsql = new StringBuilder();
                sbsql.Append("select oname from fg_orglist");
                DataTable existdt = DbHelper.GetDataTable(sb_check_sql);
                if (strArray.Length > 0)
                {
                    foreach (string arr in strArray)
                    {
                        DataRow checkdr = checkdt.NewRow();
                        checkdr["code"] = arr.Trim('[', ']', '\"');
                        checkdt.Rows.Add(checkdr);
                    }
                    if (existdt.Rows.Count > 0)
                    {
                        for (int i = checkdt.Rows.Count-1; i>= 0; i-- )
                        {
                            foreach (DataRow existdr in existdt.Rows)
                            {
                                if (existdr["org_code"].ToString() == checkdt.Rows[i]["code"].ToString())
                                {
                                    count = 1;
                                    sign = 1;
                                }
                            }
                            if (sign == 0)
                            {
                                checkdt.Rows[i].Delete();
                            }
                        }
                        if (count == 1)
                        {
                            checkdt.AcceptChanges();
                            sbsql.Append(" where phid in ( ");
                            int mark = 0;
                            foreach (DataRow checkdr in checkdt.Rows)
                            {
                                if (mark > 0 && mark < checkdt.Rows.Count)
                                {
                                    sbsql.Append(" ,");
                                }
                                mark++;
                                sbsql.Append("'" + checkdr["code"] + "'");
                            }
                            sbsql.Append(" )");
                            DataTable returndt = DbHelper.GetDataTable(sbsql.ToString());
                            return returndt;
                        }
                        else
                        {
                            for (int j = 0; j < strArray.Length; j++)
                            {
                                DataRow dr = dt.NewRow();
                                dr["phid"] = GetId(maxsql, j);
                                dr["billrule_m_code"] = billRule_m_code;
                                dr["org_code"] = strArray[j].Trim('[', ']', '\"');
                                dt.Rows.Add(dr);
                            }
                            count = DbHelper.Update(dt, sql);
                            return count;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["phid"] = GetId(maxsql, j);
                            dr["billrule_m_code"] = billRule_m_code;
                            dr["org_code"] = strArray[j].Trim('[', ']', '\"');
                            dt.Rows.Add(dr);
                        }
                        count = DbHelper.Update(dt, sql);
                        return count;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close();
            }
        }

        /// <summary>
        /// 获取新主键
        /// </summary>
        /// <param name="max_sql"></param>
        /// <returns></returns>
        private string GetMaxId(string max_sql, int count)
        {
            try
            {
                string max_c_code = DbHelper.GetString(max_sql);
                if (string.IsNullOrEmpty(max_c_code))
                {
                    max_c_code = "0";
                }
                string new_c_code = ((Int32.Parse(max_c_code) + count).ToString()).PadLeft(10, '0');
                return new_c_code;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取递增主键
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private long GetId(string sql, int count)
        {
            try
            {
                string newid = DbHelper.GetString(sql);
                long num = 0;
                if (!string.IsNullOrEmpty(newid))
                {
                    num = Int64.Parse(newid) + count + 1;
                }
                else
                {
                    num = 1;
                }
                return num;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
