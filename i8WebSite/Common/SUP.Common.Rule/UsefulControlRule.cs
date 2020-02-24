using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataAccess;
using System.Data;

namespace SUP.Common.Rule
{
    public class UsefulControlRule
    {
        UsefulControlDac dac = new UsefulControlDac();

        public DataTable GetList(string userId, string controlId, string names)
        {
            if (string.IsNullOrEmpty(names))
            {
                names = "";
            }
            if (
            names.ToLower().Contains(" exec ")
            || names.ToLower().Contains(" insert ")
            || names.ToLower().Contains(" select ")
            || names.ToLower().Contains(" delete ")
            || names.ToLower().Contains(" update ")
            || names.ToLower().Contains(" chr ")
            || names.ToLower().Contains(" mid ")
            || names.ToLower().Contains(" master ")
            || names.ToLower().Contains(" truncate ")
            || names.ToLower().Contains(" char ")
            || names.ToLower().Contains(" declare ")
            || names.ToLower().Contains(" join ")
            || names.ToLower().Contains(" cmd ")
            || names.ToLower().Contains(" create ")
            || names.ToLower().Contains(" drop ")
            || names.ToLower().Contains(" go ")
            )
            {
                names = "";
            }

            names = LoopReplace(names, "\r\n", "\r\n");
            names = LoopReplace(names, "\n", "\n");
            names = LoopReplace(names, @"\r\n", @"\r\n");
            names = LoopReplace(names, @"\n", @"\n");

            if (names.Length > 100)
            {
                names = names.Substring(0, 100);
            }

            return dac.GetList(userId, controlId, names);
        }

        public int Update(string userId, string controlId, string names)
        {
            if (string.IsNullOrWhiteSpace(names))
            {
                return 0;
            }

            names = LoopReplace(names, "\r\n", "\r\n");
            names = LoopReplace(names, "\n", "\n");
            names = LoopReplace(names, @"\r\n", @"\r\n");
            names = LoopReplace(names, @"\n", @"\n");

            if (names.Length > 100)
            {
                names = names.Substring(0, 100);
            }

            var olddt = dac.Find(userId, controlId, names);
            if (olddt == null || olddt.Rows.Count == 0)
            {
                //新增
                var phid = dac.GetMaxPhid() + 1;
                var lasttimes = DateTime.Now;
                var counts = 0;
                return dac.Insert(phid, userId, controlId, names, lasttimes, counts);
            }
            else
            {
                //修改counts+1
                var phid = long.Parse(olddt.Rows[0]["phid"].ToString());
                var lasttimes = DateTime.Now;
                var counts = int.Parse(olddt.Rows[0]["counts"].ToString()) + 1;
                return dac.Update(phid, lasttimes, counts);
            }

        }

        private string LoopReplace(string input, string starts, string ends)
        {
            input = input.Trim();
            while (input.StartsWith(starts) || input.EndsWith(ends))
            {
                if (input.StartsWith(starts))
                {
                    input = input.Substring(starts.Length - 1);
                }
                if (input.EndsWith(ends))
                {
                    input = input.Substring(0, input.Length - ends.Length);
                }
            }
            return input;
        }

    }
}
