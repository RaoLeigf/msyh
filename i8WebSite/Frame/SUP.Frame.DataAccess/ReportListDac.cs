using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;


namespace SUP.Frame.DataAccess
{
    public class ReportListDac
    {
        public DataTable LoadReportList(string userid, long orgid,string page="")

        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            strbuilder.Append("select rw.*,'"+page+"' page, '' treeid,'' treepid from( ");
            strbuilder.Append("SELECT phid,pid,'' rep_code,catalog_name rep_name,0 isleaf  FROM rw_data_catalog  ");
            strbuilder.Append("union ");
            strbuilder.Append("select rw_report_main.phid,rw_report_main.catalog_id pid,rw_report_main.rep_code,rw_report_main.rep_name, 1 isleaf from ( select rep_id,max(rightstr) rt  from rw_report_rights ");
            strbuilder.Append("where u_sort ='01' and rightstr >='1' and userid = '" + userid + "' ");
            strbuilder.Append("group by rep_id ");
            strbuilder.Append("union all ");
            strbuilder.Append("select rep_id,max(rightstr) rt from rw_report_rights where u_sort='04'  and rightstr >='1' and  ");
            strbuilder.Append("userid in(select roleid from fg3_userroleorg where userid= '" + userid + "' and orgid =" + orgid + ") ");
            strbuilder.Append("group by rep_id ");
            strbuilder.Append("union all ");
            strbuilder.Append("SELECT rep_id,t_classright.rt FROM rw_report_relclass JOIN( ");
            strbuilder.Append("select keyid,max(rightstr) rt  from rw_common_rights ");
            strbuilder.Append("where u_sort ='01' and userid = '" + userid + "' and rightstr >= '1' and keytype='RW.REPCLASSTYPE'  group by keyid ");
            strbuilder.Append("union all ");
            strbuilder.Append("select keyid,max(rightstr) rt from rw_common_rights ");
            strbuilder.Append("where u_sort='04' and userid in(select roleid from fg3_userroleorg where userid = '" + userid + "' and orgid =" + orgid + ") ");
            strbuilder.Append("and rightstr >= '1' and keytype= 'RW.REPCLASSTYPE' group by keyid) t_classright on  rw_report_relclass.class_id=t_classright.keyid ");
            strbuilder.Append("union all ");
            strbuilder.Append("SELECT phid rep_id,'3' ret FROM rw_report_main WHERE ocode IN(select orgid from fg3_userroleorg where userid='" + userid + "' and roleid IN(select phid  from fg3_role WHERE roleno='rw_admin')) ");
            strbuilder.Append(")t_repright join rw_report_main on t_repright.rep_id=rw_report_main.phid and rw_report_main.rep_status=1) rw ORDER BY isleaf,phid ");         


            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            return menudt;

        }
    }
}
