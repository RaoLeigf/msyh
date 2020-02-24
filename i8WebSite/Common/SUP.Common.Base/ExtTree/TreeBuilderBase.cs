using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.Base
{

    public abstract class TreeBuilderBase
    {
        /// <summary>
        /// 动态构建树节点，返回JObject
        /// 树节点的属性根据DataRow的列变化而变化，更加通用,
        /// 主要在自定义表单获取树形结构数据，填入treegrid的场景下用到
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public abstract JObject BuildTreeNode(DataRow dr);

        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="treedt">树形数据</param>
        /// <param name="pid">父节点ID字段名</param>
        /// <param name="id">点ID字段名</param>
        /// <param name="firstLevelFilter">一级节点过滤条件</param>
        /// <param name="sort">排序字段</param>
        /// <param name="leveltype">加载类型：首次加载，懒载</param>
        /// <param name="level">加载层数</param>
        /// <returns></returns>
        public JArray GetTreeList(DataTable treedt, string pid, string id, string firstLevelFilter, string sort, TreeDataLevelType leveltype, int level = 1000)
        {

            JArray firstlevellist = new JArray();
            if (treedt == null || string.IsNullOrWhiteSpace(pid) || string.IsNullOrWhiteSpace(id))
            {
                return firstlevellist;
            }

            JObject tempnode;
            //获得第一层的数据            
            if (leveltype == TreeDataLevelType.TopLevel)//(!string.IsNullOrEmpty(firstLevelFilter))
            {
                DataRow[] drs = null;
                if (string.IsNullOrWhiteSpace(sort))
                {
                    drs = treedt.Select(firstLevelFilter);
                }
                else
                {
                    drs = treedt.Select(firstLevelFilter, sort);
                }

                foreach (DataRow row in drs)
                {
                    tempnode = BuildTreeNode(row);
                    if (level > 1)//首次只加载一层，children为null
                    {
                        //tempnode.children = new List<TreeJSONBase>();
                        tempnode.Add("children", new JArray());
                    }
                    //tempnode.myLevel = 1;
                    tempnode.Add("myLevel", 1);
                    firstlevellist.Add(tempnode);
                }

                if (firstlevellist.Count > 0)
                {
                    LoadSubTree(treedt, firstlevellist, pid, id, level);
                }
            }
            else
            {
                foreach (DataRow dr in treedt.Rows)
                {
                    tempnode = BuildTreeNode(dr);

                    #region 处理叶子节点标记
                    //树上下级节点的数据类型
                    bool isNum = false;
                    if (treedt.Columns[pid].DataType == typeof(Int64) || treedt.Columns[pid].DataType == typeof(Int64))
                    {
                        isNum = true;
                    }
                    string subFilter = pid + "='" + tempnode["id"].ToString() + "'";
                    if (isNum)
                    {
                        subFilter = pid + "=" + Convert.ToInt32(tempnode["id"].ToString()) + "";
                    }
                    DataRow[] rows = treedt.Select(subFilter);//找当前节点的子节点,无子节点，当前节点就是叶子节点
                    if (rows.Length == 0)
                    {
                        //subnode.leaf = true;
                        tempnode.Add("leaf", true);
                    }
                    else
                    {
                        //subnode.leaf = false;
                        tempnode.Add("leaf", false);
                    }
                    #endregion


                    firstlevellist.Add(tempnode);
                }

            }
            return firstlevellist;
        }

        /// <summary>
        /// 构建第二层以下
        /// </summary>
        /// <param name="treedt">树形数据</param>
        /// <param name="parentList">第一级节点列表</param>
        /// <param name="pid">父节点ID字段名</param>
        /// <param name="id">点ID字段名</param>
        /// <param name="level">加载层数</param>
        private void LoadSubTree(DataTable treedt, JArray parentList, string pid, string id, int level)
        {
            Queue<JObject> queue = new Queue<JObject>();//树的广度遍历算法          

            foreach (JObject node in parentList)
            {
                queue.Enqueue(node);//第一层入队               
            }

            //树上下级节点的数据类型
            bool isNum = false;
            if (treedt.Columns[pid].DataType == typeof(Int64) || treedt.Columns[pid].DataType == typeof(Int64))
            {
                isNum = true;
            }

            while (queue.Count > 0)
            {
                JObject parentNode = queue.Dequeue();

                string tempid = parentNode["id"].ToString();//parentNode.id;
                int currentLevel = Convert.ToInt32(parentNode["myLevel"].ToString());//parentNode.myLevel;//当前层数

                if (currentLevel >= level) continue;// children为null, 将会懒加载

                JObject subnode;

                string filter = pid + "='" + tempid + "'";
                if (isNum)
                {
                    filter = pid + "=" + tempid + "";
                }
                DataRow[] drs = treedt.Select(filter);//找当前节点的子节点


                foreach (DataRow row in drs)
                {

                    subnode = this.BuildTreeNode(row);
                    //subnode.myLevel = currentLevel + 1;
                    subnode.Add("myLevel", currentLevel + 1);

                    #region 处理叶子节点标记

                    string subFilter = pid + "='" + subnode["id"].ToString() + "'";
                    if (isNum)
                    {
                        subFilter = pid + "=" + Convert.ToInt64(subnode["id"].ToString()) + "";
                    }
                    DataRow[] rows = treedt.Select(subFilter);//找当前节点的子节点

                    //if (subnode.myLevel < level)
                    if (Convert.ToInt32(subnode["myLevel"].ToString()) < level)
                    {
                        //subnode.children = new List<TreeJSONBase>();
                        subnode.Add("children", new JArray());
                    }

                    if (rows.Length == 0)
                    {
                        //subnode.leaf = true;
                        subnode.Add("leaf", true);
                    }
                    else
                    {
                        //subnode.leaf = false;
                        subnode.Add("leaf", false);
                    }

                    #endregion


                    if (parentNode["children"] != null)
                    {
                        //parentNode.children.Add(subnode);
                        JArray ja = parentNode["children"] as JArray;
                        ja.Add(subnode);                        
                        queue.Enqueue(subnode);//入队登记
                    }

                }
            }
        }
    }
}
