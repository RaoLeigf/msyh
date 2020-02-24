using NG3.Data.Service;
using System;
using System.Data;

namespace SUP.Frame.DataAccess
{
    public class LoginPicManagerDac
    {

        public DataTable GetLoginPictureTree()
        {
            return DbHelper.GetDataTable("select * from fg3_loginpicture");
        }

        public DataTable GetLoginPicture(string phid)
        {
            return DbHelper.GetDataTable("select * from fg3_loginpicture where phid = " + phid);
        }

        public void AddNode(long phid, string id, string name, string src, string attachid)
        {
            if (string.IsNullOrEmpty(id))
            {
                DbHelper.ExecuteNonQuery("insert into fg3_loginpicture(phid,id,pid,name,src,type,sysflg,userid) values ("
                    + phid + "," + phid + ",'','" + name + "','',0,0," + NG3.AppInfoBase.UserID + ")");
            }
            else
            {
                DataTable dt = DbHelper.GetDataTable("select * from fg3_loginpicture where id = " + id);
                if (dt.Rows[0]["type"].ToString() == "0")
                {
                    if (string.IsNullOrEmpty(src))
                    {
                        DbHelper.ExecuteNonQuery("insert into fg3_loginpicture(phid,id,pid,name,type,sysflg,userid) values ("
                            + phid + "," + phid + "," + id + ",'" + name + "',0,0," + NG3.AppInfoBase.UserID + ")");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(attachid))
                        {
                            DbHelper.ExecuteNonQuery("insert into fg3_loginpicture(phid,id,pid,name,src,type,sysflg,userid) values ("
                                + phid + "," + phid + "," + id + ",'" + name + "','" + src + "',1,0," + NG3.AppInfoBase.UserID + ")");
                        }
                        else
                        {
                            DbHelper.ExecuteNonQuery("insert into fg3_loginpicture(phid,id,pid,name,src,type,sysflg,userid,attachid) values ("
                                + phid + "," + phid + "," + id + ",'" + name + "','" + src + "',1,0," + NG3.AppInfoBase.UserID + "," + attachid + ")");
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(src))
                    {
                        DbHelper.ExecuteNonQuery("insert into fg3_loginpicture(phid,id,pid,name,type,sysflg,userid) values ("
                            + phid + "," + phid + "," + dt.Rows[0]["pid"] + ",'" + name + "',0,0," + NG3.AppInfoBase.UserID + ")");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(attachid))
                        {
                            DbHelper.ExecuteNonQuery("insert into fg3_loginpicture(phid,id,pid,name,src,type,sysflg,userid) values ("
                                + phid + "," + phid + "," + dt.Rows[0]["pid"] + ",'" + name + "','" + src + "',1,0," + NG3.AppInfoBase.UserID + ")");
                        }
                        else
                        {
                            DbHelper.ExecuteNonQuery("insert into fg3_loginpicture(phid,id,pid,name,src,type,sysflg,userid,attachid) values ("
                                + phid + "," + phid + "," + dt.Rows[0]["pid"] + ",'" + name + "','" + src + "',1,0," + NG3.AppInfoBase.UserID + "," + attachid + ")");
                        }
                    }
                }
            }
        }

        public void DelNode(string phid)
        {
            DbHelper.ExecuteNonQuery("delete from fg3_loginpicture where phid = " + phid);
        }

        public DataTable GetLoginPicSet()
        {
            long userid = GetUserId();
            DataTable dt = DbHelper.GetDataTable("select * from fg3_loginpictureset where phid = " + userid);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return DbHelper.GetDataTable("select * from fg3_loginpictureset where phid = 0");
            }
        }

        public string ChangeShowType(string showtype, string showpic)
        {
            LoginDac loginDac = new LoginDac();

            long userid = GetUserId();
            InitLoginPicSet(userid);

            if (showtype == "0")
            {
                return DbHelper.GetString("select showpic from fg3_loginpictureset where phid = " + userid);
            }
            else
            {
                return DbHelper.GetString("select showpic2 from fg3_loginpictureset where phid = " + userid);
            }
        }

        public void InitLoginPicSet(long userid)
        {
            DataTable dt = DbHelper.GetDataTable("select * from fg3_loginpictureset where phid = " + userid);
            if (dt == null || dt.Rows.Count == 0)
            {
                DbHelper.ExecuteNonQuery("insert into fg3_loginpictureset(phid,showtype,showlogo,allowuser) values (" + userid + ",0,1,0)");
            }
        }

        public long GetUserId()
        {
            long userid = NG3.AppInfoBase.UserID;
            if (NG3.AppInfoBase.UserType.ToUpper() == "SYSTEM")
            {
                userid = 0;
            }
            return userid;
        }

        public void SaveLoginPicSet(string showtype, string showlogo, string allowuser, string showpic)
        {
            LoginDac loginDac = new LoginDac();

            long userid = GetUserId();
            InitLoginPicSet(userid);

            if (showtype == "0")
            {
                DbHelper.ExecuteNonQuery("update fg3_loginpictureset set showpic = '" + showpic + "', showtype = " + showtype + ", showlogo = " 
                    + showlogo + ", allowuser = " + allowuser + " where phid = " + userid);               
            }
            else
            {
                DbHelper.ExecuteNonQuery("update fg3_loginpictureset set showpic2 = '" + showpic + "', showtype = " + showtype + ", showlogo = "
                   + showlogo + ", allowuser = " + allowuser + " where phid = " + userid);
            }

            loginDac.SetCookieValue();
        }

    }
}
