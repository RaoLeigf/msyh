using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices;
using NHibernate;
using NHibernate.Type;
using Newtonsoft.Json.Linq;

namespace SUP.Common.Base
{
    /// <summary>
    /// 构建Ext树的处理逻辑
    /// 
    /// 使用模板方法设计模式，基类完成迭代算法操作，子类负责加工节点属性
    /// </summary>
    public abstract class ExtJsTreeBuilderBase
    {

        public abstract TreeJSONBase BuildTreeNode(DataRow dr);
            

        public IList<TreeJSONBase> GetExtTreeList(DataTable treedt, string pid, string id, string firstLevelFilter,TreeDataLevelType leveltype, int level = 1000)
        {
            return this.GetExtTreeList(treedt, pid, id, firstLevelFilter, string.Empty, leveltype, level);
        }
        /// <summary>
        /// 构建ExtJS的树结构
        /// </summary>
        /// <param name="menudt">树列表数据</param>
        /// <param name="pid">父节点字段名称</param>
        /// <param name="id">当前节点字段名称</param>
        /// <param name="firstLevelFilter">获取第一层数据的过滤条件</param>
        /// <param name="leveltype">层数类型top,other</param>
        /// <param name="level">首次获取几层</param>
        /// <returns></returns>
        public IList<TreeJSONBase> GetExtTreeList(DataTable treedt, string pid, string id, string firstLevelFilter, string sort, TreeDataLevelType leveltype, int level = 1000)
        {
          
            IList<TreeJSONBase> firstlevellist = new List<TreeJSONBase>();
            if (treedt == null || string.IsNullOrWhiteSpace(pid) || string.IsNullOrWhiteSpace(id))
            {
                return firstlevellist;
            }

                TreeJSONBase tempnode;
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
                    drs = treedt.Select(firstLevelFilter,sort);
                }

                foreach (DataRow row in drs)
                {
                    tempnode = BuildTreeNode(row);
                    if (level > 1)//首次只加载一层，children为null
                    {
                        tempnode.children = new List<TreeJSONBase>();
                    }                    
                    tempnode.myLevel = 1;
                    //tempnode.expanded = true;
                    //tempnode.id = row[id].ToString();//基类控制id属性
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

                    firstlevellist.Add(tempnode);
                }

            }
            return firstlevellist;       
        }
        
        private void LoadSubTree(DataTable treedt, IList<TreeJSONBase> parentList, string pid, string id, int level)
        {
            Queue<TreeJSONBase> queue = new Queue<TreeJSONBase>();//树的广度遍历算法          

            foreach (TreeJSONBase node in parentList)
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
                TreeJSONBase parentNode = queue.Dequeue();

                string tempid = parentNode.id;
                int currentLevel = parentNode.myLevel;//当前层数

                if (currentLevel >= level) continue;// children为null, 将会懒加载

                TreeJSONBase subnode;

                string filter = pid + "='" + tempid + "'";
                if (isNum)
                {
                    filter = pid + "=" + tempid + "";
                }
                DataRow[] drs = treedt.Select(filter);//找当前节点的子节点


                foreach (DataRow row in drs)
                {

                    subnode = this.BuildTreeNode(row);
                    subnode.myLevel = currentLevel + 1;

                    if (!subnode.LeafSeted)//处理叶子标记
                    {
                        string subFilter = pid + "='" + subnode.id + "'";
                        if (isNum)
                        {
                            subFilter = pid + "=" + subnode.id + "";
                        }
                        DataRow[] rows = treedt.Select(subFilter);//找当前节点的子节点
                        if (subnode.myLevel < level)
                        {
                            subnode.children = new List<TreeJSONBase>();
                        }

                        if (rows.Length == 0)
                        {
                            subnode.leaf = true;
                        }
                        else
                        {
                            subnode.leaf = false;
                        }

                    }
                    else
                    {
                        if (!subnode.leaf && subnode.myLevel < level)
                        {
                            subnode.children = new List<TreeJSONBase>();
                        }

                    }

                    if (parentNode.children != null)
                    {
                        parentNode.children.Add(subnode);
                        queue.Enqueue(subnode);//入队登记
                    }

                }
            }
        }
        
        /// <summary>
        /// 懒加载一棵树,对于数据库不是懒加载，客户端是懒加载
        /// </summary>
        /// <param name="treedt">整棵树数据，无奈为了判断叶子节点</param>
        /// <param name="pid">父节点id</param>
        /// <param name="id">id</param>
        /// <param name="nodeid">当前节点id</param>
        /// <returns></returns>
        public IList<TreeJSONBase> LazyLoadTreeList(DataTable treedt, string pid, string id, string nodeid)
        {
            IList<TreeJSONBase> firstlevellist = new List<TreeJSONBase>();
            TreeJSONBase tempnode;

            DataRow[] drs = treedt.Select(pid + "='" + nodeid + "'");

            foreach (DataRow dr in drs)
            {
                tempnode = BuildTreeNode(dr);
                DataRow[] rows = treedt.Select(pid + "='" + tempnode.id + "'");//找当前节点的子节点

                if (rows.Length == 0)
                {
                    tempnode.leaf = true;
                }
                else
                {
                    tempnode.leaf = false;
                }

                firstlevellist.Add(tempnode);
            }
            return firstlevellist;
        }

    }

    public enum TreeDataLevelType
    {
        TopLevel,
        LazyLevel
    }
}
