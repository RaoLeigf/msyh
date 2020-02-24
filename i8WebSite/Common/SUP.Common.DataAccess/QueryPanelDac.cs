using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data;
using NG3.Data.Service;
using SUP.Common.Base;

namespace SUP.Common.DataAccess
{
    public class QueryPanelDac
    {
        /// <summary>
        /// 保存用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="ClientJsonString"></param>
        /// <returns></returns>
        public int SetQueryPanelData(string PageId, string ClientJsonString)
        {
            string sqlWhere = " pageid=" + DbConvert.ToSqlString(PageId) + " and userid=" + DbConvert.ToSqlString(NG3.AppInfoBase.LoginID) + " and cboo=" + DbConvert.ToSqlString(NG3.AppInfoBase.OCode);
            string query = DataConverterHelper.ConvertQueryString(ClientJsonString);


            //目前是直接删除后创建  这样的话，guid的主从明细表就查不到明细数据了
            //若存在 则更新，否则 删除后创建   
            DataTable tmpDT = DbHelper.GetDataTable("select * from c_sys_search_def_master where " + sqlWhere );
            if (tmpDT.Rows.Count == 0)
            {
                DataRow dr = tmpDT.NewRow();
                dr["id"] = Guid.NewGuid().ToString();
                dr["pageid"] = PageId;
                dr["userid"] = NG3.AppInfoBase.LoginID;
                dr["cboo"] = NG3.AppInfoBase.OCode;
                dr["isdefault"] = 0;
                dr["ismember"] = 0;
                dr["remeberstr"] = ClientJsonString;
                dr["reembersql"] = query;
                tmpDT.Rows.Add(dr);
                return DbHelper.Update(tmpDT, "select * from c_sys_search_def_master");
            }
            else
            {
                tmpDT.Rows[0]["isdefault"] = 0;
                tmpDT.Rows[0]["ismember"] = 0;
                tmpDT.Rows[0]["remeberstr"] = ClientJsonString;
                tmpDT.Rows[0]["reembersql"] = query;
                return DbHelper.Update(tmpDT, "select * from c_sys_search_def_master where " + sqlWhere);
            }
        }

        /// <summary>
        /// 获取用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <returns></returns>
        public DataTable GetQueryPanelData(string PageId)
        {
            string sqlString = " select remeberstr,reembersql from c_sys_search_def_master where pageid=" + DbConvert.ToSqlString(PageId) + " and userid=" + DbConvert.ToSqlString(NG3.AppInfoBase.LoginID) + " and cboo=" + DbConvert.ToSqlString(NG3.AppInfoBase.OCode); ;
            return DbHelper.GetDataTable(sqlString);
        }

        /// <summary>
        /// 获取用户排序数据
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public DataTable GetSortFilterData(string pageid, string ocode, string logid) {
            IDataParameter[] p;
            if (this.ExistsUserDefineInDetail(pageid, ocode, logid))
            {
                //filter = " and c_sys_search_def_detail.isplay='1' ";
                StringBuilder strb = new StringBuilder();
                strb.Append(" SELECT c_sys_search.searchfield,c_sys_search_def_detail.sortmode,c_sys_search_def_detail.sortorder ");
                strb.Append(" FROM c_sys_search ");
                strb.Append(" LEFT JOIN c_sys_search_def_master ON c_sys_search.pageid = c_sys_search_def_master.pageid ");
                strb.Append(" LEFT JOIN c_sys_search_def_detail ON (c_sys_search.ccode = c_sys_search_def_detail.code)  and (c_sys_search_def_master.id = c_sys_search_def_detail.mst_id)");
                strb.Append(" WHERE c_sys_search_def_master.userid = {0} AND c_sys_search_def_master.cboo = {1} ");
                strb.Append(" AND c_sys_search_def_master.pageid = {2} AND c_sys_search_def_detail.sortmode <> 0 ORDER BY sortorder ASC ");
                p = new NGDataParameter[] { new NGDataParameter("userid", logid), new NGDataParameter("cboo", ocode), new NGDataParameter("pageid", pageid) };
                return DbHelper.GetDataTable(strb.ToString(), p);
            }



            string sql = @"SELECT c_sys_search.searchfield,c_sys_search.sortmode,c_sys_search.sortorder
                        FROM c_sys_search                       
                        WHERE pageid = {0} AND c_sys_search.sortmode <> 0  " + " ORDER BY sortorder ASC  ";
            p = new NGDataParameter[] { new NGDataParameter("pageid", pageid) };

            return DbHelper.GetDataTable(sql, p);
        }

        /// <summary>
        /// 获取内嵌查询面板的勾选框信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public string GetCheckData(string pageid, string ocode, string logid)
        {
            string sql = String.Empty;
            if (this.ExistsUserDefineInDetail(pageid, ocode, logid))
            {
                sql = String.Format("select distinct ischeck from c_sys_search_def_detail where mst_id in (select id from c_sys_search_def_master where pageid='{0}' and userid='{1}' and cboo='{2}')", pageid, logid, ocode);
                return DbHelper.GetString(sql);
            }

            sql = String.Format("select distinct ischeck from c_sys_search where pageid='{0}'", pageid);

            return DbHelper.GetString(sql);
        }

        /// <summary>
        /// 保存内嵌查询面板的勾选框信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public int SaveCheckData(string pageid, string ocode, string logid,string ischeck)
        {
            string sql = String.Empty;
            if (this.ExistsUserDefineInDetail(pageid, ocode, logid))
            {
                sql = String.Format("update c_sys_search_def_detail set ischeck='{3}' where mst_id in (select id from c_sys_search_def_master where pageid='{0}' and userid='{1}' and cboo='{2}')", pageid, logid, ocode,ischeck);
                return DbHelper.ExecuteNonQuery(sql);
            }

            sql = String.Format("update c_sys_search set ischeck='{0}' where pageid='{1}'", ischeck,pageid);

            return DbHelper.ExecuteNonQuery(sql);
        }

        #region 自定义查询


        //-----------自定义查询-----------------

        /// <summary>
        /// 获取自定义查询界面信息
        /// </summary>
        /// <param name="pageid">帮助id</param>
        /// <param name="ocode">组织号</param>
        /// <param name="logid">操作员</param>
        /// <returns></returns>
        public DataTable GetIndividualQueryPanelInfo(string pageid, string ocode, string logid, bool showAll = false)
        {
            string filter = " and c_sys_search.isplay='1' ";
            string filter_user = " and c_sys_search_def_detail.isplay='1' ";
            if (showAll)
            {
                filter = string.Empty;//不过滤
            }

            if (showAll)
            {
                filter_user = string.Empty;//不过滤
            }
            //string filter_user = " and c_sys_search_def_detail.isplay='1' ";

            IDataParameter[] p;
            if (this.ExistsUserDefineInDetail(pageid, ocode, logid))
            {
                //filter = " and c_sys_search_def_detail.isplay='1' ";
                StringBuilder strb = new StringBuilder();
                strb.Append(" SELECT c_sys_search.combflg,c_sys_search_def_detail.displayindex,c_sys_search.fieldtype,");
                strb.Append("c_sys_search.datasource,c_sys_search.controlflag,c_sys_search.controltype,c_sys_search.ismulti");
                strb.Append(" ,c_sys_search.sqlfilter,c_sys_search.maxlength,c_sys_search_def_detail.isplay,c_sys_search.matchfieldwidth,c_sys_search.isaddtowhere " +
                            ",c_sys_search.searchfield ,c_sys_search.searchtable, c_sys_search.fname_chn,c_sys_search.langkey,c_sys_search.ccode,c_sys_search.pageid, ");
                strb.Append(" c_sys_search_def_detail.defaultdata,c_sys_search_def_detail.sortmode,c_sys_search_def_detail.sortorder, '用户定义' AS definetype ");
                strb.Append(" FROM c_sys_search ");
                strb.Append(" LEFT JOIN c_sys_search_def_master ON c_sys_search.pageid = c_sys_search_def_master.pageid ");
                strb.Append(" LEFT JOIN c_sys_search_def_detail ON (c_sys_search.ccode = c_sys_search_def_detail.code)  and (c_sys_search_def_master.id = c_sys_search_def_detail.mst_id)");
                strb.Append(" WHERE c_sys_search_def_master.userid = {0} AND c_sys_search_def_master.cboo = {1} ");
                strb.Append(filter_user);
                strb.Append(" AND c_sys_search_def_master.pageid = {2}  ORDER BY displayindex ASC ");
                p = new NGDataParameter[] { new NGDataParameter("userid", logid), new NGDataParameter("cboo", ocode), new NGDataParameter("pageid", pageid) };
                return DbHelper.GetDataTable(strb.ToString(), p);
            }



             string sql = @"SELECT playindex AS displayindex,'系统定义' AS definetype, 
                        c_sys_search.defaultdata,
                        c_sys_search.datasource,c_sys_search.controlflag,c_sys_search.controltype,c_sys_search.ismulti,
                        c_sys_search.sqlfilter, c_sys_search.maxlength,c_sys_search.isplay,c_sys_search.matchfieldwidth,c_sys_search.isaddtowhere,
                        c_sys_search.searchtable,c_sys_search.searchfield,c_sys_search.fname_chn ,c_sys_search.langkey, c_sys_search.ccode,c_sys_search.pageid,
                        c_sys_search.combflg,c_sys_search.fieldtype,c_sys_search.sortmode,c_sys_search.sortorder
                        FROM c_sys_search                       
                        WHERE pageid = {0} " + filter + " ORDER BY displayindex ASC  ";
            p = new NGDataParameter[] { new NGDataParameter("pageid", pageid) };

            return DbHelper.GetDataTable(sql, p);
        }

        /// <summary>
        /// 是否存在用户定义的查询条件
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>       

        //内嵌查询判断是去系统自定义表中找还是用户自定义找
        //说明：存在以下几种情况：
        //1.master没有数据，那么detail肯定没有数据
        //2.master有数据，detail表没有数据
        //3.master有数据，detail表有数据
        //-------内嵌查询判断语句开始--------------


        //情况master有数据，detail表有数据
        private bool ExistsUserDefineInMasterandDetail(string pageid, string ocode, string logid)
        {
            string sqlindetail = string.Empty;
            string sqlinmaster = string.Empty;
            //注释的语句只适用于sql server，oracle不兼容
            //sql = @"select (
            //        (select COUNT(*) from c_sys_search_def_detail where mst_id IN  
            //    (SELECT id FROM c_sys_search_def_master WHERE pageid={0} and userid={1} and cboo = {2}))
            //    +
            //    (SELECT COUNT(*) FROM c_sys_search_def_master WHERE pageid={0} and userid={1} and cboo = {2})
            //    )";
            sqlindetail = @"select COUNT(*) from c_sys_search_def_detail where mst_id IN 
                                (SELECT id FROM c_sys_search_def_master WHERE pageid={0} and userid={1} and cboo = {2})";

            sqlinmaster = @"SELECT COUNT(*) FROM c_sys_search_def_master WHERE pageid={0} and userid={1} and cboo = {2}";

            IDataParameter[] p = new NGDataParameter[3];
            p[0] = new NGDataParameter("helpid", pageid);
            p[1] = new NGDataParameter("userid", logid);
            p[2] = new NGDataParameter("cboo", ocode);

            string sqld = DbHelper.GetString(sqlindetail, p);
            string sqlm = DbHelper.GetString(sqlinmaster, p);
            int all = int.Parse(sqld) + int.Parse(sqlm);

            return (all != 0);
        }

        private bool ExistsUserDefineInDetail(string pageid, string ocode, string logid)
        {
            string sql = string.Empty;
            sql = @"select COUNT(*) from c_sys_search_def_detail where mst_id IN  
                (SELECT id FROM c_sys_search_def_master WHERE pageid={0} and userid={1} and cboo = {2})";

            IDataParameter[] p = new NGDataParameter[3];
            p[0] = new NGDataParameter("helpid", pageid);
            p[1] = new NGDataParameter("userid", logid);
            p[2] = new NGDataParameter("cboo", ocode);

            return (DbHelper.GetString(sql, p) != "0");
        }

        //-------内嵌查询判断语句END--------------

        /// <summary>
        /// 保存查询条件设置信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public int SaveQueryInfo(DataTable dtchange, DataTable dt, string pageid, string ocode, string logid)
        {
            if (dt.Rows.Count == 0) return 1;//空的表，不处理，也算保存成功

            DataTable detaildt;
            StringBuilder strb = new StringBuilder();
            int count = dt.Rows.Count;
            int index = count - 1;
            for (int i = 0; i < count; i++)
            {
                string val = dt.Rows[i]["ccode"].ToString();
                if (i == index)
                {
                    strb.Append("'" + val + "'");
                }
                else
                {
                    strb.Append("'" + val + "',");
                }
            }

            int iret = 0;
            if (this.ExistsUserDefineInMasterandDetail(pageid, ocode, logid))//先去明细表找
            {
                string sqlGetMasterId = string.Format(@"select id from c_sys_search_def_master where pageid='{0}' and userid= '{1}' and cboo = '{2}'", pageid, logid, ocode);
                string mst_id = DbHelper.GetString(sqlGetMasterId);
                StringBuilder exists = new StringBuilder();
                //select* from c_sys_search_def_detail where mst_id = 'a6d8701e-8526-4ffc-a5e3-a456896aca24' and code in ('00000000000000005123','00000000000000006439','00000000000000005125','00000000000000005126')

                string sqlstr1 = string.Format("select* from c_sys_search_def_detail where mst_id ='{0}' and code in ( {1} )", mst_id, strb.ToString());


                //detaildt = DbHelper.GetDataTable("select * from c_sys_search_def_detail where mst_id ='" + mst_id + "'  and "  +  " code in (' + strb.ToString() + ')");
                detaildt = DbHelper.GetDataTable(sqlstr1);


                //DataTable dtplayindex = DbHelper.GetDataTable(sql);
                //当master表中有数据，而detail表中没有数据的时候，就要去master表中寻找id，这个id对应detail表中的mst_id.  
                //string sqlGetMasterId = string.Format(@"select id from c_sys_search_def_master where pageid='{0}' and userid= '{1}' and cboo = '{2}'", pageid,logid,ocode);
                DataTable masterdtid = DbHelper.GetDataTable(sqlGetMasterId);

                string mst_id_orginal = masterdtid.Rows[0]["id"].ToString();


                foreach (DataRow dr in detaildt.Rows)
                {
                    string codeStr = dr["code"].ToString();
                    exists.Append("'" + codeStr + "'").Append(",");
                    DataRow[] drs = dt.Select(" ccode='" + codeStr + "'");
                    if (drs.Length > 0)
                    {
                        dr["combflg"] = drs[0]["combflg"].ToString();
                        dr["defaultdata"] = drs[0]["defaultdata"].ToString();
                        dr["displayindex"] = drs[0]["displayindex"].ToString();
                        dr["isplay"] = drs[0]["isplay"].ToString();
                        dr["sortmode"] = drs[0]["sortmode"];
                        dr["sortorder"] = drs[0]["sortorder"];
                    }
                }

                //已经保存过c_sys_search_def_detail又再在c_sys_search新增字段
                if (dt.Rows.Count != detaildt.Rows.Count)
                { 
                    DataRow[] rows;
                    //如果是第一次进来，只有master表里有数据，detail表里没有数据，就要将c_sys_search表中所有的数据全部给detail表。
                    if (detaildt.Rows.Count == 0)
                    {
                        rows = dt.Select("1=1");
                        
                    }
                    else
                    {
                        string inStr = exists.ToString().TrimEnd(',');
                        rows = dt.Select("ccode not in (" + inStr + ")");
                    }


                    //string sqlStr = "select * from c_sys_search_def_detail where 1>1";
                    //DataTable detailTempDt = DbHelper.GetDataTable(sqlStr);
                    foreach (DataRow row in rows)
                    {
                        DataRow detailRow = detaildt.NewRow();
                        detailRow.BeginEdit();
                        detailRow["id"] = Guid.NewGuid().ToString();
                        //detailRow["mst_id"] = dt.Rows[0]["mst_id"];//用已经存在的数据的mst_id
                        detailRow["mst_id"] = mst_id_orginal; //直接去获取到,因为第一次进来detail表中没有数据,就没有原来的
                        detailRow["code"] = row["ccode"].ToString();
                        detailRow["combflg"] = row["combflg"].ToString();
                        detailRow["defaultdata"] = row["defaultdata"].ToString();
                        detailRow["displayindex"] = row["displayindex"].ToString();
                        detailRow["isplay"] = row["isplay"].ToString();
                        detailRow["sortmode"] = row["sortmode"];
                        detailRow["sortorder"] = row["sortorder"];
                        detailRow.EndEdit();
                        detaildt.Rows.Add(detailRow);
                    }
                }

                iret = DbHelper.Update(detaildt, "select * from c_sys_search_def_detail");
            }//if
            else
            {
                string sql = "select * from c_sys_search_def_master where 1>1";
                DataTable masterdt = DbHelper.GetDataTable(sql);
                DataRow masterdr = masterdt.NewRow();
                string code = Guid.NewGuid().ToString();
                masterdr.BeginEdit();
                masterdr["id"] = code;
                masterdr["pageid"] = pageid;
                masterdr["userid"] = logid;
                masterdr["cboo"] = ocode;
                masterdr.EndEdit();
                masterdt.Rows.Add(masterdr);
                iret = DbHelper.Update(masterdt, "select * from c_sys_search_def_master");

                sql = "select * from c_sys_search_def_detail where 1>1";
                detaildt = DbHelper.GetDataTable(sql);
                foreach (DataRow row in dt.Rows)
                {
                    DataRow detailRow = detaildt.NewRow();
                    detailRow.BeginEdit();
                    detailRow["id"] = Guid.NewGuid().ToString();
                    detailRow["mst_id"] = code;
                    detailRow["code"] = row["ccode"].ToString();
                    detailRow["combflg"] = row["combflg"].ToString();
                    detailRow["defaultdata"] = row["defaultdata"].ToString();
                    detailRow["displayindex"] = row["displayindex"].ToString();
                    detailRow["isplay"] = row["isplay"].ToString();
                    detailRow["sortmode"] = row["sortmode"];
                    detailRow["sortorder"] = row["sortorder"];
                    detailRow.EndEdit();
                    detaildt.Rows.Add(detailRow);
                }

                iret += DbHelper.Update(detaildt, "select * from c_sys_search_def_detail");
            }

            //内嵌查询注册信息可以修改，系统定义表c_sys_search也得更新
            //DataTable sysDt = DbHelper.GetDataTable("select * from c_sys_search where ccode in (" + strb.ToString() + ")");
            //foreach (DataRow row in dt.Rows)
            //{
            //    string codeStr = row["ccode"].ToString();
            //    DataRow[] dr = sysDt.Select(string.Format("ccode={0}", codeStr));
            //    if (dr.Length > 0)
            //    {
            //        dr[0]["fname_chn"] = row["fname_chn"];
            //        dr[0]["isplay"] = row["isplay"];
            //        dr[0]["user_mod_flg"] = 1;
            //    }
            //}

            //新增-->内嵌查询修改行
            StringBuilder strbchange = new StringBuilder();
            int countchange = dtchange.Rows.Count;
            int indexchange = countchange - 1;
            for (int i = 0; i < countchange; i++)
            {
                string val = dtchange.Rows[i]["ccode"].ToString();
                if (i == indexchange)
                {
                    strbchange.Append("'" + val + "'");
                }
                else
                {
                    strbchange.Append("'" + val + "',");
                }
            }
            if (strbchange.ToString().Length > 0)
            {
                //string sysDtchangesql = string.Format(@"select * from c_sys_search where ccode in (" + strbchange.ToString() + ") and pageid='{0}'", pageid);
                string sysDtchangesql = string.Format(@"select * from c_sys_search where ccode in ({0}) and pageid='{1}'", strbchange.ToString(),pageid);
                DataTable sysDtchange = DbHelper.GetDataTable(sysDtchangesql);

                //DataTable sysDtchange = DbHelper.GetDataTable("select * from c_sys_search where ccode in (" + strbchange.ToString() + ") and pageid=");
                foreach (DataRow row in dtchange.Rows)
                {
                    string codeStr = row["ccode"].ToString();
                    DataRow[] dr = sysDtchange.Select(string.Format("ccode='{0}'", codeStr));
                    if (dr.Length > 0)
                    {
                        //dr[0]["fname_chn"] = row["fname_chn"];
                        //dr[0]["isplay"] = row["isplay"];
                        dr[0]["user_mod_flg"] = 1;
                    }
                }


                iret += DbHelper.Update(sysDtchange, "select * from c_sys_search");
            }


            return iret;
        }



        public DataTable GetPropertyDt(string tables)
        {
            //string sql = string.Format("SELECT DISTINCT searchtable FROM c_sys_search WHERE pageid='{0}'",pageId);

            if (string.IsNullOrEmpty(tables))
            {
                return null;
            }

            string sql = string.Format(@"SELECT c_bname, c_name, propertyname  FROM 
                                                    fg_columns WHERE c_bname IN({0})", tables);

            return DbHelper.GetDataTable(sql);

        }

        public int RestoreDefault(string pageid, string ocode, string logid)
        {

            //int del = 0;
            string sqlDelDetail = String.Format("delete from c_sys_search_def_detail where mst_id in (select id from c_sys_search_def_master where pageid='{0}' and cboo='{1}' and userid='{2}')", pageid, ocode, logid);

            int delDetail = DbHelper.ExecuteNonQuery(sqlDelDetail);

            string sqlDelMaster = String.Format("delete from c_sys_search_def_master where pageid='{0}' and cboo='{1}' and userid='{2}'", pageid, ocode, logid);

            int delMaster = DbHelper.ExecuteNonQuery(sqlDelMaster);
            return delDetail + delMaster;

        }

        #endregion

    }
}
