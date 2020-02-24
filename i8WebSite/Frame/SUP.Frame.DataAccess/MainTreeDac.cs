using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using SUP.Common.Base;
using Enterprise3.Common.ExceptionHandling;

namespace SUP.Frame.DataAccess
{
    public class MainTreeDac
    {
        public MainTreeDac() {
 
        }

         /// <summary>
        /// 取菜单数据
        /// </summary>
        /// <param name="product"></param>
        /// <param name="suite"></param>
        /// <returns></returns>
        public DataTable LoadMenuData(string product, string suite, string nodeid)
        {

            StringBuilder strbuilder = new StringBuilder();

            DataTable menudt = new DataTable();

            strbuilder.Append(" select fg3_menu.code,fg3_menu.id,fg3_menu.pid,name,url,managername,rightname,functionname,");
            strbuilder.Append(" fg3_menu.suite,apptype,functionnode_flag,fg3_menu.moduleno,rightkey,ebankflg,norightcontrol,fg3_menu.seq,adminvisiable,'0' expanded,fg3_menu.busphid,");
            strbuilder.Append(" metadata_bustree.langkey from fg3_menu ");
            strbuilder.Append(" LEFT JOIN metadata_bustree on fg3_menu.busphid = metadata_bustree.phid ");
            //strbuilder.Append(string.Format(" where product='{0}'",product.ToUpper()));
            strbuilder.Append(string.Format(" where fg3_menu.suite='{0}'", suite));           
            strbuilder.Append(" and menusign='1' ");
            strbuilder.Append(" and (subflg='0' or subflg='' or subflg is null) ");
            if (nodeid != "root")
            {
                strbuilder.Append(" and fg3_menu.pid='" + nodeid + "'");//lazyload
            }
            strbuilder.Append(" order by seq ");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());

            //供应商门户过滤
            //menudt = DbHelper.GetDataTable("select * from fg_menu where suite = 'EC' and (url like'%PSU%') order by seq");

            return menudt;

        }

        public DataTable Query(string product, string condition, string usertype, string treeFilter)
        {

            StringBuilder strbuilder = new StringBuilder();
            //string lang = Enterprise3.Common.ExceptionHandling.LangInfo.GetLangFieldName();
            string lang = Enterprise3.Rights.AnalyticEngine.LangInfo.GetLangFieldName();
            DataTable menudt = new DataTable();
            if (treeFilter != null && treeFilter != "")
            {
                strbuilder.Append("select code,id,pid,name,url,managername,rightname,functionname,suite,apptype,functionnode_flag,moduleno,rightkey,ebankflg,norightcontrol,seq,adminvisiable,'0' expanded,fg3_menu.busphid from(");
                strbuilder.Append(treeFilter);
                strbuilder.Append(" ) a");
                strbuilder.Append(" where name like '%" + condition + "%' ");
                //strbuilder.Append(" and product= ");
                //strbuilder.Append(" '" + product.ToUpper() + "'");
                strbuilder.Append(" and menutype='1' ");
                strbuilder.Append(" order by seq ");
            }
            else
            {
                //strbuilder.Append(String.Format(@"SELECT fg3_menu.code,fg3_menu.id,fg3_menu.pid,name,url,managername,rightname,functionname,fg3_menu.suite,apptype,functionnode_flag,fg3_menu.moduleno,rightkey,ebankflg,norightcontrol,fg3_menu.seq,adminvisiable,'0' expanded,metadata_bustree.langkey FROM fg3_menu 
                //                    JOIN metadata_bustree on fg3_menu.busphid = metadata_bustree.phid
                //                     JOIN ng3_ui_lable on ng3_ui_lable.lang_alias = metadata_bustree.langkey
                //                     JOIN ng3_lang on ng3_lang.phid = ng3_ui_lable.lang_phid
                //                    where ng3_ui_lable.ui_identity='bustree'
                //                    and {0} LIKE '%{1}%' or (standard_lang LIKE '%{1}%' AND ({0} is null OR {0}='' )) ", lang, condition));
                ////strbuilder.Append(" and fg3_menu.product= ");
                ////strbuilder.Append(" '" + product.ToUpper() + "'");
                //strbuilder.Append(" and menusign='1'");
                //strbuilder.Append(" and fg3_menu.menutype='1' ");
                //strbuilder.Append(" order by fg3_menu.seq ");

                strbuilder.Append(String.Format(
                @"select * from                                  
                    (SELECT fg3_menu.code,fg3_menu.id,fg3_menu.pid,name,url,managername,rightname,functionname,fg3_menu.suite,apptype,functionnode_flag,fg3_menu.moduleno,rightkey,ebankflg,norightcontrol,fg3_menu.seq,
                    adminvisiable,'0' expanded,fg3_menu.busphid,metadata_bustree.langkey FROM fg3_menu 
                    JOIN metadata_bustree on fg3_menu.busphid = metadata_bustree.phid
                    JOIN ng3_ui_lable on ng3_ui_lable.lang_alias = metadata_bustree.langkey
                    JOIN ng3_lang on ng3_lang.phid = ng3_ui_lable.lang_phid
                    where ng3_ui_lable.ui_identity='bustree'
                    and ({0} LIKE '%{1}%' or (standard_lang LIKE '%{1}%' AND ({0} is null OR {0}='' )))and fg3_menu.menusign='1' and fg3_menu.menutype='1'
                union
                    SELECT fg3_menu.code,fg3_menu.id,fg3_menu.pid,name,url,managername,rightname,functionname,fg3_menu.suite,apptype,functionnode_flag,fg3_menu.moduleno,rightkey,ebankflg,norightcontrol,fg3_menu.seq,
                    adminvisiable,'0' expanded,fg3_menu.busphid,null FROM fg3_menu 
                    where fg3_menu.name like '%{1}%'and fg3_menu.menusign='1' and fg3_menu.menutype='1' and fg3_menu.code not in(
                        select fg3_menu.code from fg3_menu,metadata_bustree
                        where fg3_menu.busphid = metadata_bustree.phid )  
                 )t order by seq ", lang, condition));
            }


            menudt = DbHelper.GetDataTable(strbuilder.ToString());

            //供应商门户过滤
            //menudt = DbHelper.GetDataTable("select * from fg_menu where suite = 'EC' and (url like'%PSU%') order by seq");

            return menudt;

        }
        public DataTable LoadTable(string product, string usertype)
        {

            StringBuilder strbuilder = new StringBuilder();

            DataTable menudt = new DataTable();

            //strbuilder.Append("select code,id,pid,name,url,managername,rightname,functionname,suite,apptype,functionnode_flag,moduleno,rightkey,ebankflg,norightcontrol,seq,adminvisiable,'0' expanded");
            //strbuilder.Append(" from fg3_menu where product= ");
            //strbuilder.Append(" '" + product.ToUpper() + "'");
            //if (String.Compare(usertype, UserType.System, true) == 0)
            //{
            //    strbuilder.Append(" and suite='DMC'");
            //}
            //strbuilder.Append(" and menusign='1' ");
            //strbuilder.Append(" and menutype='1' ");
            ////strbuilder.Append(" and name like '%" + condition + "%' ");
            //strbuilder.Append(" and (subflg='0' or subflg='' or subflg is null) ");
            ////if (nodeid != "root")
            ////{
            ////    strbuilder.Append(" and pid='" + nodeid + "'");//lazyload
            ////}
            //strbuilder.Append(" order by seq ");

            strbuilder.Append(" select fg3_menu.code,fg3_menu.id,fg3_menu.pid,name,url,managername,rightname,functionname,");
            strbuilder.Append(" fg3_menu.suite,apptype,functionnode_flag,fg3_menu.moduleno,rightkey,ebankflg,norightcontrol,fg3_menu.seq,adminvisiable,'0' expanded,fg3_menu.busphid, ");
            strbuilder.Append(" metadata_bustree.langkey from fg3_menu ");
            strbuilder.Append(" LEFT JOIN metadata_bustree on fg3_menu.busphid = metadata_bustree.phid ");
            //strbuilder.Append(string.Format(" where product='{0}'", product.ToUpper()));
            strbuilder.Append(" where menusign='1' ");
            if (String.Compare(usertype, UserType.System, true) == 0)
            {
                strbuilder.Append(" and fg3_menu.suite='DMC'");
            }
            strbuilder.Append(" and menutype='1' ");
            strbuilder.Append(" and (subflg='0' or subflg='' or subflg is null) ");           
            strbuilder.Append(" order by seq ");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());

            //供应商门户过滤
            //menudt = DbHelper.GetDataTable("select * from fg_menu where suite = 'EC' and (url like'%PSU%') order by seq");

            return menudt;

        }
        public void SetTimeStamp(string strkey, string strvalue)
        {
            try
            {
                DbHelper.Open();
                string sql = "select * from fg_timestamp where strkey='" + strkey + "'";
                DataTable dt = DbHelper.GetDataTable(sql);

                if (dt.Rows.Count > 0)//存在
                {
                    DataRow dr = dt.Rows[0];
                    dr["strvalue"] = strvalue;
                    DbHelper.Update(dt, "select * from fg_timestamp");
                }
                else//新增
                {
                    DataRow dr = dt.NewRow();
                    dr["strkey"] = strkey;
                    dr["strvalue"] = strvalue;
                    dt.Rows.Add(dr);

                    DbHelper.Update(dt, "select * from fg_timestamp");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                DbHelper.Close();
            }
        }
        //取过滤后的表
        public DataTable FilterTable(string treeFilter)
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();
            strbuilder.Append("select code,id,pid,name,url,managername,rightname,functionname,suite,apptype,functionnode_flag,moduleno,rightkey,ebankflg,norightcontrol,seq,adminvisiable,'0' expanded from(");
            strbuilder.Append(treeFilter);
            strbuilder.Append(" ) a");
            strbuilder.Append(" order by seq ");
            menudt = DbHelper.GetDataTable(strbuilder.ToString());

            return menudt;
        }
        //取按套件查询过滤后的表
        public DataTable FilterTable(string product, string suite, string nodeid, string treeFilter)
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();
            strbuilder.Append("select code,id,pid,name,url,managername,rightname,functionname,suite,apptype,functionnode_flag,moduleno,rightkey,ebankflg,norightcontrol,seq,adminvisiable,'0' expanded from(");
            strbuilder.Append(treeFilter);
            strbuilder.Append(" ) a");
            //strbuilder.Append(" where product= ");
            //strbuilder.Append(" '" + product.ToUpper() + "'");
            strbuilder.Append(" where suite=");
            strbuilder.Append("'" + suite + "'");
            strbuilder.Append(" and menusign='1' ");
            strbuilder.Append(" and menutype=1 ");
            strbuilder.Append(" and (subflg='0' or subflg='' or subflg is null) ");
            if (nodeid != "root")
            {
                strbuilder.Append(" and pid='" + nodeid + "'");//lazyload
            }
            strbuilder.Append(" order by seq ");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());

            return menudt;
        }

        //根据busphid取fg3-menu数据的接口
        public DataTable GetMenuByBusphid(long busphid)
        {
            DataTable menudt = null;
            string sql = string.Format("select * from fg3_menu where busphid = {0}", busphid);
            menudt = DbHelper.GetDataTable(sql);
            return menudt;
        }
    }
}
