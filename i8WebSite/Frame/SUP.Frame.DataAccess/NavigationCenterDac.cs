using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using NG3;
using SUP.Frame.DataAccess;

namespace SUP.Frame.DataAccess
{
    public class NavigationCenterDac
    {
        public DataTable LoadTree()
        {
            long userid = AppInfoBase.UserID;
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();
            strbuilder.Append("select * from ");
            strbuilder.Append("fg3_process_tree");
            strbuilder.Append(" order by seq");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());
          
            return menudt;

        }

        public int SaveTree(DataTable masterdt, List<long> phid)
        {
            long userid = AppInfoBase.UserID;
            //string sql = string.Format("delete from fg3_process_tree");
            //int iret = DbHelper.ExecuteNonQuery(sql);

            if (masterdt.Rows.Count == 0)//删除用户我的功能树所有节点
            {
                return 1;
            }
            int i = 0;
            //处理主表的主键
            foreach (DataRow dr in masterdt.Rows)
            {

                if (dr.RowState == DataRowState.Deleted) continue;
                if(dr.RowState == DataRowState.Modified) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    //Guid.NewGuid().ToString();//主表的主键
                    //dr["phid"] = ++masterid;
                    dr["phid"] = phid[i++];
                    dr["userid"] = userid;
                }
            }
            int m = DbHelper.Update(masterdt, "select * from fg3_process_tree");
            return m;
        }

        public string LoadChart(string phid)
        {
            long userid = AppInfoBase.UserID;
            long phidInt;
            long.TryParse(phid, out phidInt);
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("select svgconfig from fg3_process_tree where phid =" + phidInt);           
            string svgConfig = DbHelper.GetString(strbuilder.ToString());           
            return svgConfig;
        }
        public string SaveChart(string svgConfig, string phid)
        {
            long userid = AppInfoBase.UserID;
            long phidInt;
            long.TryParse(phid, out phidInt);            
            string sql = String.Format(@"update fg3_process_tree set svgconfig = '{0}' where phid ={1}", svgConfig, phidInt);
              
            try
            {
                DbHelper.ExecuteScalar(sql);
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
                throw new Exception(ex.Message);
            }
        }

        public DataTable FindProcessByWiki(IList<long> phids)
        {
            StringBuilder strbuilder = new StringBuilder();
            if (phids.Count > 0)
            {
                strbuilder.Append(String.Format("select * from fg3_process_tree where phid = {0} ",phids[0]));
            }
            else
            {
                return null;
            }
            for(int i = 1; i < phids.Count; i++)
            {
                strbuilder.Append(String.Format("or phid = {0} ", phids[i]));
            }
            DataTable dt = DbHelper.GetDataTable(strbuilder.ToString());
            return dt;
        }
    }
}
