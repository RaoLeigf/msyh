using System;
using System.Data;
using System.Text;
using NG3.Data.Service;
using System.Collections.Generic;

namespace SUP.Frame.DataAccess
{
    public class CustomFloatMenuDac
    {

        public DataTable LoadMenuData(string product, string suite, string language)
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            switch (language.ToUpper())
            {
                case "EN-US":
                    strbuilder.Append("select code,id,pid,name_en name,url,suite");
                    break;
                default:
                    strbuilder.Append("select code,id,pid,name,url,suite");
                    break;
            }

            strbuilder.Append(" from fg3_menu where product= ");
            strbuilder.Append(" '" + product.ToUpper() + "'");
            strbuilder.Append(" and suite=");
            strbuilder.Append("'" + suite + "'");
            strbuilder.Append(" and menusign='1' ");
            strbuilder.Append(" and (subflg='0' or subflg='' or subflg is null) ");

            if (language.ToUpper() == "EN-US")
            {
                strbuilder.Append(" and name_en is not null ");
            }
            strbuilder.Append(" order by seq");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());

            return menudt;
        }

        public DataTable GetMenuPidName(string id, string language)
        {
            if (language == string.Empty)
            {
                language = "ZH-CN";
            }

            string sql = "";

            if (language.ToUpper() == "EN-US")
            {
                sql = "select pid,name_en name from fg3_menu where id = '" + id + "'";
            }
            else
            {
                sql = "select pid,name from fg3_menu where id = '" + id + "'";
            }

            return DbHelper.GetDataTable(sql);
        }

        public DataTable GetFloatMenuTree(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select fg3_floatmenu.code_in code,fg3_floatmenu.name text,fg3_floatmenu.param,");
            sb.Append(" fg3_floatmenu.sysflg,fg3_floatmenu.url,metadata_bustree.langkey from fg3_floatmenu ");
            sb.Append(" LEFT JOIN metadata_bustree on fg3_floatmenu.code_in = metadata_bustree.code ");
            sb.Append(" where fg3_floatmenu.code = '" + code + "'");
            return DbHelper.GetDataTable(sb.ToString());
        }

        public DataTable GetFloatMenuIn(string code)
        {
            return DbHelper.GetDataTable("select * from fg_floatmenu_manager_in where buscode = '" + code + "'");
        }

        public DataTable GetFloatMenuOut(string code)
        {
            return DbHelper.GetDataTable("select * from fg_floatmenu_manager_out where buscode = '" + code + "'");
        }

        public void DelFloatMenu(string code, string code_in)
        {
            DbHelper.ExecuteScalar("delete from fg3_floatmenu where code = '" + code + "' and code_in = '" + code_in + "'");
        }

        public string GetFloatMenuCount(string code, string code_in)
        {
            return DbHelper.GetString("select count(*) from fg3_floatmenu where code = '" + code + "' and code_in = '" + code_in + "'");
        }

        public void UpdateFloatMenu(string code, string code_in, string name, string param, string url)
        {
            DbHelper.ExecuteScalar("update fg3_floatmenu set name = '" + name + "', param = '" + param + "', url = '" + url
                + "', user_mod_flg = 1 where code = '" + code + "' and code_in = '" + code_in + "'");
        }

        public void InsertFloatMenu(long phid, string code, string code_in, string name, string param, string url)
        {
            DbHelper.ExecuteNonQuery("insert into fg3_floatmenu(phid,code,code_in,name,param,url,user_mod_flg,sysflg) values ('"
                + phid + "','" + code + "','" + code_in + "','" + name + "','" + param +"','" + url + "',1,0)");
        }

        public string GetFloatMenuUrl(string code, string param)
        {
            string url = DbHelper.GetString("select listurl from metadata_bustree where code = '" + code + "'");

            if (!string.IsNullOrEmpty(param))
            {
                param = param.Replace("{", "").Replace("}", "");
                if (url.IndexOf("?") > -1)
                {
                    url += "&floatmenu={" + param + "}";
                }
                else
                {
                    url += "?floatmenu={" + param + "}";
                }
            }

            return url;
        }

        public string GetReportCells(string rep_id, string ds_no, List<string> paraList)
        {
            string cells = "";
            for (int i = 0; i < paraList.Count; i++)
            {
                if (cells != "")
                {
                    cells += @"\\|";
                }
                cells += DbHelper.GetString("select pcell from uv_report_para where rep_id = " + rep_id + " and ds_no = '" + ds_no + "' and dsc_para = '" + paraList[i] + "'");
            }
            return cells;
        }

        public DataTable GetMenuDtByCode(string code)
        {
            return DbHelper.GetDataTable("select id,name,moduleno,rightkey,url,managername,rightname,suite,apptype from fg3_menu where code = '" + code + "'");
        }

        public DataTable GetBusNameByCode(string code)
        {
            return DbHelper.GetDataTable("select busname,langkey from metadata_bustree where code = '" + code + "'");
        }

        public DataTable LoadSysReportList()
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            strbuilder.Append("select phid,pid,'' rep_code,catalog_name rep_name,0 isleaf  from rw_data_catalog_template ");
            strbuilder.Append("union ");
            strbuilder.Append("select rw_report_template.phid,rw_report_template.catalog_id pid,rw_report_template.rep_code,rw_report_template.rep_name, 1 isleaf from rw_report_template");
            
            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            return menudt;
        }

        public DataTable LoadSearchSysReportList(string search)
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            strbuilder.Append("select phid,pid,'' rep_code,catalog_name rep_name,0 isleaf  from rw_data_catalog_template ");
            strbuilder.Append("union ");
            strbuilder.Append("select rw_report_template.phid,rw_report_template.catalog_id pid,rw_report_template.rep_code,rw_report_template.rep_name, 1 isleaf from rw_report_template ");
            strbuilder.Append("where rw_report_template.rep_name like '%" + search + "%'");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            return menudt;
        }

        public DataTable LoadCusReportList()
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            strbuilder.Append("select phid,pid,'' rep_code,catalog_name rep_name,0 isleaf  from rw_data_catalog ");
            strbuilder.Append("union ");
            strbuilder.Append("select rw_report_main.phid,rw_report_main.catalog_id pid,rw_report_main.rep_code,rw_report_main.rep_name, 1 isleaf from rw_report_main");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            return menudt;
        }

        public DataTable LoadSearchCusReportList(string search)
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            strbuilder.Append("select phid,pid,'' rep_code,catalog_name rep_name,0 isleaf  from rw_data_catalog ");
            strbuilder.Append("union ");
            strbuilder.Append("select rw_report_main.phid,rw_report_main.catalog_id pid,rw_report_main.rep_code,rw_report_main.rep_name, 1 isleaf from rw_report_main ");
            strbuilder.Append("where rw_report_main.rep_name like '%" + search + "%'");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            return menudt;
        }

        public DataTable GetSheet(string phid)
        {
            return DbHelper.GetDataTable("select distinct sheetid, sheetname from uv_report_ds where rep_id = " + phid + " and sheetname is not null");
        }

        public DataTable GetDsc(string phid, string sheetid)
        {
            return DbHelper.GetDataTable("select ds_no,dsc_name from uv_report_ds where rep_id = " + phid + " and sheetid = " + sheetid + " and dsc_name is not null");
        }

        public DataTable GetPara(string phid, string sheetid, string ds_no)
        {
            return DbHelper.GetDataTable("select dsc_para,displayname from uv_report_para where rep_id = " + phid + " and sheetid = " + sheetid + " and ds_no = '" + ds_no + "'");
        }

    }
}
