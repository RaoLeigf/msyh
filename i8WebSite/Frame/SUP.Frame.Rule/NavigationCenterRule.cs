using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Common.Rule;
using SUP.Frame.DataEntity;
using SUP.Common.Interface;

namespace SUP.Frame.Rule
{
    public class NavigationCenterRule
    {
        private NavigationCenterDac dac = null;
        public NavigationCenterRule()
        {
            dac = new NavigationCenterDac();
        }
        public IList<TreeJSONBase> LoadTree(string type)
        {
            string sql = string.Empty;
            string filter = string.Empty;
            DataTable dt = this.GetMainTreeData(type);
            filter = "(mypid='root')";
            return new NavigationBuilder().GetExtTreeList(dt, "mypid", "myid", filter, TreeDataLevelType.TopLevel);//加载所有
         
        }


        public DataTable GetMainTreeData(string type)
        {
            DataTable menudt = dac.LoadTree().Copy();
            if(type != "edit")
            {
                RemoveNode(menudt);
            }            
            return menudt;
        }
        private void RemoveNode(DataTable menudt)
        {            
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                string id = dr["myid"].ToString();

                if (dr["isleaf"].ToString() != "1")//非功能节点
                {
                    DataRow[] tempdr = menudt.Select("mypid='" + id + "'");
                    if (tempdr.Length == 0)
                    {
                        dr.Delete();
                    }
                }
            }
            menudt.AcceptChanges();
        }

        public int SaveTree(DataTable NavigationTable)
        {
            int count = 0;
            foreach (DataRow dr in NavigationTable.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    count++;
                }
            }

            List<long> phid = null;
            if (count > 0)
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_process_tree", "phid", count);
            }
            return dac.SaveTree(NavigationTable, phid);
        }

        public string LoadChart(string phid)
        {
            return dac.LoadChart(phid);
        }

        public string SaveChart(string svgConfig, string phid)
        {
            return dac.SaveChart(svgConfig, phid);
        }

        public DataTable FindProcessByWiki(IList<long> phids)
        {
            return dac.FindProcessByWiki(phids);
        }
    }

    class NavigationJSONBase : TreeJSONBase
    {
        public virtual string myid { get; set; }
        public virtual string mypid { get; set; }
        public virtual string name { get; set; }
        public virtual long phid { get; set; }
        public virtual string svgconfig { get; set; }
        public virtual int isleaf { get; set; }
    }

    public class NavigationBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            NavigationJSONBase node = new NavigationJSONBase();
            node.myid = dr["myid"].ToString();
            node.id = dr["myid"].ToString();
            node.text = dr["name"].ToString();
            node.phid = Int64.Parse(dr["phid"].ToString());
            node.mypid = dr["mypid"].ToString();
            node.name = dr["name"].ToString();
            node.leaf = dr["isleaf"].ToString()== "1"?true:false;
            int isleaf = 0;
            int.TryParse(dr["isleaf"].ToString(), out isleaf);
            node.isleaf = isleaf;
            node.svgconfig = dr["svgconfig"].ToString();            
            return node;
        }
    }
}
