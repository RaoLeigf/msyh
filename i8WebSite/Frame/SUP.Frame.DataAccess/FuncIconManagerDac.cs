using System;
using System.Data;
using System.IO;
using NG3.Data.Service;

namespace SUP.Frame.DataAccess
{
    public class FuncIconManagerDac
    {

        public DataTable GetChildMenuDtByCode(string code)
        {
            DataTable dt = new DataTable();
            dt.TableName = "FuncIconGrid";

            dt.Columns.Add(new DataColumn("busphid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("funciconid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("funciconname", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("funcicondisplayname", Type.GetType("System.String")));

            DataTable menuDt = DbHelper.GetDataTable("select name,id,suite,funciconname,busphid from fg3_menu where code='" + code + "'");
            string busphid = Convert.ToString(menuDt.Rows[0]["busphid"]);
            DataTable funciconDt = new DataTable();
            if (!string.IsNullOrEmpty(busphid))
            {
                funciconDt = DbHelper.GetDataTable("select id,name from fg3_menufuncicon where busphid='" + busphid + "'");
            }

            string id = menuDt.Rows[0]["id"].ToString();
            string name = menuDt.Rows[0]["name"].ToString();
            string funciconid = funciconDt.Rows.Count > 0 ? funciconDt.Rows[0]["id"].ToString() : "";
            string funciconname = menuDt.Rows[0]["funciconname"].ToString();
            string funcicondisplayname = funciconDt.Rows.Count > 0 ? funciconDt.Rows[0]["name"].ToString() : "";
            string suite = menuDt.Rows[0]["suite"].ToString();

            menuDt = DbHelper.GetDataTable("select name,id,funciconname,busphid from fg3_menu where pid = '" + id 
                + "' and menusign = '1' and suite = '" + suite + "' and (subflg='0' or subflg='' or subflg is null) order by seq");

            if (menuDt.Rows.Count > 0)
            {
                for (int i = 0; i < menuDt.Rows.Count; i++)
                {
                    GetChildMenuDt(menuDt.Rows[i], name, suite, ref dt);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(busphid) && busphid != "0")
                {
                    DataRow dr = dt.NewRow();
                    dr["busphid"] = busphid;
                    dr["name"] = name;
                    dr["funciconid"] = funciconid;
                    dr["funciconname"] = funciconname;
                    dr["funcicondisplayname"] = funcicondisplayname;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private void GetChildMenuDt(DataRow dr, string pre, string suite, ref DataTable dt)
        {
            string busphid = Convert.ToString(dr["busphid"]);
            DataTable funciconDt = new DataTable();
            if (!string.IsNullOrEmpty(busphid))
            {
                funciconDt = DbHelper.GetDataTable("select id,name from fg3_menufuncicon where busphid='" + busphid + "'");
            }

            string id = dr["id"].ToString();
            string name = dr["name"].ToString();
            string funciconid = funciconDt.Rows.Count > 0 ? funciconDt.Rows[0]["id"].ToString() : "";
            string funciconname = dr["funciconname"].ToString();
            string funcicondisplayname = funciconDt.Rows.Count > 0 ? funciconDt.Rows[0]["name"].ToString() : "";

            DataTable menuDt = DbHelper.GetDataTable("select name,id,funciconname,busphid from fg3_menu where pid = '" + id 
               + "' and menusign = '1' and suite = '" + suite + "' and (subflg='0' or subflg='' or subflg is null) order by seq");

            if (menuDt.Rows.Count > 0)
            {
                for (int i = 0; i < menuDt.Rows.Count; i++)
                {
                    GetChildMenuDt(menuDt.Rows[i], pre + "\\" + name, suite, ref dt);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(busphid) && busphid != "0")
                {
                    DataRow newdr = dt.NewRow();
                    newdr["name"] = pre + "\\" + name;
                    newdr["busphid"] = busphid;
                    newdr["funciconid"] = funciconid;
                    newdr["funciconname"] = funciconname;
                    newdr["funcicondisplayname"] = funcicondisplayname;
                    dt.Rows.Add(newdr);
                }
            }
        }

        public bool AddFuncIconTag(long phid, string name)
        {            
            name = name.Trim();
            DataTable dt = DbHelper.GetDataTable("select * from fg3_funcicontag where name = '" + name + "'");

            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                DbHelper.ExecuteNonQuery("insert into fg3_funcicontag(phid,name,deletable) values ('" + phid + "','" + name + "','0')");
                return true;
            }
        }

        public bool DelFuncIconTag(string name)
        {
            string deletable = DbHelper.ExecuteScalar("select deletable from fg3_funcicontag where name = '" + name + "'").ToString();
            if (deletable == "1")
            {
                return false;
            }
            else
            {
                DbHelper.ExecuteNonQuery("delete from fg3_funcicontag where name = '" + name + "'");
                return true;
            }
        }

        public DataTable GetFuncIconTagGrid(string search, ref int totalRecord)
        {
            DataTable dt = new DataTable();
            if (search == "")
            {
                dt = DbHelper.GetDataTable("select phid,name from fg3_funcicontag");
            }
            else
            {
                dt = DbHelper.GetDataTable("select phid,name from fg3_funcicontag where name like '%" + search + "%'");
            }
            DataRow row = dt.NewRow();
            row["phid"] = "0";
            row["name"] = "全部";
            dt.Rows.InsertAt(row, 0);
            return dt;
        }

        public DataTable GetFuncIconDtByPhid(string id)
        {
            return DbHelper.GetDataTable("select * from fg3_funcicon where phid = " + id);
        }

        public bool CheckDelFuncIcon(string id)
        {
            string count = DbHelper.GetString("select count(*) from fg3_menu where funciconid = '" + id + "'");
            if (int.Parse(count) > 0) return false;
            else return true;
        }

        public bool AddFuncIcon(long phid, string name, string tag, string attachid)
        {
            string[] tags = tag.Split(';');
            for (int i = 0; i < tags.Length - 1; i++)
            {
                DbHelper.ExecuteNonQuery("update fg3_funcicontag set deletable = '1' where name ='" + tags[i] + "'");
            }

            if (string.IsNullOrEmpty(attachid))
            {
                DbHelper.ExecuteNonQuery("insert into fg3_funcicon(phid,tag,name,icontype) values (" + phid + ",'" + tag + "','" + name + "','1')");
            }
            else
            {
                DbHelper.ExecuteNonQuery("insert into fg3_funcicon(phid,tag,name,icontype,attachid) values (" + phid + ",'" + tag + "','" + name + "','1'," + attachid + ")");
            }

            return true;
        }

        public bool DelFuncIcon(string id, string tag)
        {
            DbHelper.ExecuteNonQuery("delete from fg3_funcicon where phid = '" + id + "'");

            DataTable dt;
            string[] tags = tag.Split(';');
            for (int i = 0; i < tags.Length - 1; i++)
            {
                dt = DbHelper.GetDataTable("select * from fg3_funcicon where tag like '%" + tags[i] + ";%'");
                if (dt.Rows.Count == 0)
                {
                    DbHelper.ExecuteNonQuery("update fg3_funcicontag set deletable = '0' where name ='" + tags[i] + "'");
                }
            }

            return true;
        }

        public bool ReplaceFuncIcon(string id, string name, string tag)
        {
            DataTable dt = DbHelper.GetDataTable("select tag, name from fg3_funcicon where phid = '" + id + "'");
            string oldtag = dt.Rows[0]["tag"].ToString();
            string oldname = dt.Rows[0]["name"].ToString();

            string path = AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\CustomIcons";
            if (File.Exists(path + "\\" + oldname))
            {
                File.Delete(path + "\\" + oldname);
            }

            DbHelper.ExecuteNonQuery("update fg3_funcicon set name = '" + name + "',tag = '" + tag + "' where phid = '" + id + "'");

            string[] tags = oldtag.Split(';');
            for (int i = 0; i < tags.Length - 1; i++)
            {
                dt = DbHelper.GetDataTable("select * from fg3_funcicon where tag like '%" + tags[i] + ";%'");
                if (dt.Rows.Count == 0)
                {
                    DbHelper.ExecuteNonQuery("update fg3_funcicontag set deletable = '0' where name ='" + tags[i] + "'");
                }
            }

            return true;
        }

        public DataTable GetIconDt(string icontype)
        {
            return DbHelper.GetDataTable("select * from fg3_funcicon where icontype = '" + icontype + "'");
        }

        public string GetFuncIconNameType(string id, ref string icontype)
        {
            DataTable dt = DbHelper.GetDataTable("select name,icontype from fg3_funcicon where phid = '" + id + "'");
            icontype = dt.Rows[0]["icontype"].ToString();
            return dt.Rows[0]["name"].ToString();
        }
        
        public string GetMenuFuncIconCount(string busphid)
        {
            return DbHelper.GetString("select count(*) from fg3_menufuncicon where busphid = " + busphid);
        }

        public void UpdateMenuFuncIcon(string busphid, string id, string name)
        {
            DbHelper.ExecuteScalar("update fg3_menufuncicon set id = " + id + ", name = '" + name + "' where busphid = " + busphid);
        }

        public void InsertMenuFuncIcon(long phid, string busphid, string id, string name)
        {
            DbHelper.ExecuteNonQuery("insert into fg3_menufuncicon(phid,busphid,id,name) values ("
                + phid + "," + busphid + "," + id + ",'" + name + "')");
        }

    }
}
