using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using NG3.Data;
using SUP.Common.Base;

namespace SUP.Common.DataAccess
{
    public class UsefulControlDac
    {
        public DataTable GetList(string userId, string controlId, string names)
        {
            string sql = @"
SELECT 
	phid,
	userid,
	controlid,
	names,
	lasttimes,
	counts
FROM fg3_useful where userid={0} and controlid={1} and names LIKE '"+ names + "%'";
            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("userid", userId);
            p[1] = new NGDataParameter("controlid", controlId);

            string sortString = "counts desc,lasttimes desc";
            int totalRecord = 0;
            int pageSize = 10;
            int pageIndex = 0;

            string strSql = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortString, p);

            return DbHelper.GetDataTable(strSql, p);

        }

        public DataTable Find(string userId, string controlId, string names)
        {
            string sql = @"
SELECT 
	phid,
    userid,
	controlid,
	names,
	lasttimes,
	counts
FROM fg3_useful where userid={0} and controlid={1} and names={2}
ORDER BY counts DESC,lasttimes Desc
";
            IDataParameter[] p = new NGDataParameter[3];
            p[0] = new NGDataParameter("userid", userId);
            p[1] = new NGDataParameter("controlid", controlId);
            p[2] = new NGDataParameter("names", names);
            return DbHelper.GetDataTable(sql, p);
        }

        public Int64 GetMaxPhid()
        {
            string sql = @"
SELECT max(phid) FROM  fg3_useful";
            Int64 maxLineid = 0;
            object obj = DbHelper.ExecuteScalar(sql);
            if (obj != null && obj != System.DBNull.Value)
            {
                maxLineid = Int64.Parse(obj.ToString());
            }
            return maxLineid++;
        }

        public int Insert(long phid, string userid, string controlid, string names, DateTime lasttimes, int counts)
        {
            string sql = @"
INSERT INTO fg3_useful (phid, userid, controlid, names, lasttimes, counts)
VALUES ({0}, {1}, {2},{3},{4},{5}) ";
            IDataParameter[] p = new NGDataParameter[6];
            p[0] = new NGDataParameter("phid", phid);
            p[1] = new NGDataParameter("userid", userid);
            p[2] = new NGDataParameter("controlid", controlid);
            p[3] = new NGDataParameter("names", names);
            p[4] = new NGDataParameter("lasttimes", lasttimes);
            p[5] = new NGDataParameter("counts", counts);
            return DbHelper.ExecuteNonQuery(sql, p);
        }

        public int Update(long phid, DateTime lasttimes, int counts)
        {
            string sql = @"
UPDATE fg3_useful
SET lasttimes = {0}, counts = {1}
WHERE phid = {2}
";
            IDataParameter[] p = new NGDataParameter[3];
            p[0] = new NGDataParameter("lasttimes", lasttimes);
            p[1] = new NGDataParameter("counts", counts);
            p[2] = new NGDataParameter("phid", phid);
            return DbHelper.ExecuteNonQuery(sql, p);
        }
    }
}
