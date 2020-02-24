using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using NHibernate;
using NHibernate.Type;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace SUP.Common.Base
{
    /// <summary>
    /// 构建Ext树的处理逻辑
    /// 
    /// 使用模板方法设计模式，基类完成迭代算法操作，子类负责加工节点属性
    /// </summary>
    public abstract class ExtJsTreeBuilderBase<T> where T : class
    {
        /// <summary>
        /// 构建树节点,ColumnName为属性名
        /// </summary>
        /// <param name="row">具体数据行</param>
        /// <returns>树的Json数据</returns>
        public abstract TreeJSONBase BuildTreeNode(DataRow row);

        public readonly string NodeIdPropertyName;
        public readonly string NoteTextPropertyName;
        public readonly string SelectPropertyName;
        public readonly string ExParamPropertyName;
        public readonly bool AllowChecked;
        public readonly Dictionary<string, object> Dic;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ExtJsTreeBuilderBase()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeIdPropertyName">用于NodeId的属性名称</param>
        /// <param name="noteTextPropertyName">用于NodeText的属性名称,可以多个用|分隔</param>
        /// <param name="selectPropertyName">用于Select的属性名称</param>
        /// <param name="exParamPropertyName">扩展参数对应的属性值</param>
        /// <param name="dic">Node的所有参数(可选项expanded、cls、leaf、exparams、textformatstring)</param>
        protected ExtJsTreeBuilderBase(string nodeIdPropertyName, string noteTextPropertyName, string selectPropertyName, string exParamPropertyName, Dictionary<string, object> dic)
        {
            NodeIdPropertyName = nodeIdPropertyName;
            NoteTextPropertyName = noteTextPropertyName;
            SelectPropertyName = selectPropertyName;
            ExParamPropertyName = exParamPropertyName;
            AllowChecked = !string.IsNullOrWhiteSpace(selectPropertyName);
            Dic = dic;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeIdPropertyName">用于NodeId的属性名称</param>
        /// <param name="noteTextPropertyName">用于NodeText的属性名称,可以多个用|分隔</param>
        /// <param name="selectPropertyName">用于Select的属性名称</param>
        /// <param name="dic">Node的所有参数(可选项expanded、cls、leaf、exparams、textformatstring)</param>
        protected ExtJsTreeBuilderBase(string nodeIdPropertyName, string noteTextPropertyName, string selectPropertyName, Dictionary<string, object> dic)
            : this(nodeIdPropertyName, noteTextPropertyName, selectPropertyName, "", dic)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeIdPropertyName">用于NodeId的属性名称</param>
        /// <param name="noteTextPropertyName">用于NodeText的属性名称,可以多个用|分隔</param>
        /// <param name="selectPropertyName">用于Select的属性名称</param>
        protected ExtJsTreeBuilderBase(string nodeIdPropertyName, string noteTextPropertyName, string selectPropertyName)
            : this(nodeIdPropertyName, noteTextPropertyName, selectPropertyName, null)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeIdPropertyName">用于NodeId的属性名称</param>
        /// <param name="noteTextPropertyName">用于NodeText的属性名称,可以多个用|分隔</param>
        protected ExtJsTreeBuilderBase(string nodeIdPropertyName, string noteTextPropertyName)
            : this(nodeIdPropertyName, noteTextPropertyName, "", null)
        {

        }

        public IList<TreeJSONBase> GetExtTreeList(IList<T> list, string pid, string id, string firstLevelFilter,
            TreeDataLevelType leveltype, int level = 1000)
        {
            return GetExtTreeList(list, pid, id, firstLevelFilter, leveltype, level, "");
        }

        /// <summary>
        /// 构建ExtJS的树结构
        /// </summary>
        /// <param name="list">树列表数据</param>
        /// <param name="pid">父节点属性名称</param>
        /// <param name="id">当前节点属性名称</param>
        /// <param name="firstLevelFilter">获取第一层数据的过滤条件</param>
        /// <param name="leveltype">层数类型top,other</param>
        /// <param name="level">首次获取几层</param>
        /// <param name="sort">排序字段（list转为）</param>
        /// <returns></returns>
        public IList<TreeJSONBase> GetExtTreeList(IList<T> list, string pid, string id, string firstLevelFilter,
            TreeDataLevelType leveltype, int level = 1000, string sort = "")
        {
            IList<TreeJSONBase> firstlevellist = new List<TreeJSONBase>();

            TreeJSONBase tempnode;

            var tp = typeof (T);
            var pi = tp.GetProperty(pid);
            if (pi == null)
                throw new Exception(Resources.PidPropertyNameNotFind);

            var dt = EntitiesToDataTable(list);

            if (dt == null)
                return firstlevellist;

            //获得第一层的数据            
            if (leveltype == TreeDataLevelType.TopLevel) //(!string.IsNullOrEmpty(firstLevelFilter))
            {
                var drs = dt.Select(firstLevelFilter, sort);

                foreach (var item in drs)
                {
                    tempnode = BuildTreeNode(item);
                    if (level > 1) //首次只加载一层，children为null
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
                    LoadSubTree(dt, firstlevellist, pid, id, level, pi.PropertyType, sort);
                }
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    tempnode = BuildTreeNode(row);
                    firstlevellist.Add(tempnode);
                }
            }

            return firstlevellist;
        }


        private void LoadSubTree(DataTable dt, IList<TreeJSONBase> parentList, string pid, string id, int level, Type pidType, string sort)
        {
            var queue = new Queue<TreeJSONBase>();//树的广度遍历算法          

            foreach (var node in parentList)
            {
                queue.Enqueue(node);//第一层入队               
            }

            while (queue.Count > 0)
            {
                var parentNode = queue.Dequeue();

                var tempid = parentNode.id;
                var currentLevel = parentNode.myLevel;//当前层数

                if (currentLevel >= level) continue;

                TreeJSONBase subnode;
                string filter = string.Format("{0}={1}", pid, tempid);
                //找当前节点的子节点
                if (pidType == typeof(string))
                    filter = string.Format("{0}='{1}'", pid, tempid);

                var drs = dt.Select(filter, sort);//找当前节点的子节点

                //parentNode.leaf = drs.Length == 0;

                foreach (var dr in drs)
                {
                    subnode = this.BuildTreeNode(dr);
                    subnode.myLevel = currentLevel + 1;

                    if (!subnode.LeafSeted)//处理叶子标记
                    {
                        filter = string.Format("{0}={1}", pid, subnode.id);
                        //找当前节点的子节点
                        if (pidType == typeof(string))
                            filter = string.Format("{0}='{1}'", pid, subnode.id);

                        var rows = dt.Select(filter, sort);

                        if (subnode.myLevel < level)
                        {
                            subnode.children = new List<TreeJSONBase>();
                        }
                        subnode.leaf = rows.Length == 0;
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
        /// <param name="list">整棵树数据，无奈为了判断叶子节点</param>
        /// <param name="pid">父节点id</param>
        /// <param name="id">id</param>
        /// <param name="nodeid">当前节点id</param>
        /// <returns></returns>
        public IList<TreeJSONBase> LazyLoadTreeList(List<T> list, string pid, string id, string nodeid, string sort="")
        {
            IList<TreeJSONBase> firstlevellist = new List<TreeJSONBase>();

            var tp = typeof(T);
            var pi = tp.GetProperty(pid);
            if (pi == null)
                throw new Exception(Resources.PidPropertyNameNotFind);
            Type pidType = pi.PropertyType;

            pi = tp.GetProperty(id);
            if (pi == null)
                throw new Exception(Resources.PidPropertyNameNotFind);
            Type idType = pi.PropertyType;

            var dt = EntitiesToDataTable(list);

            if (dt == null)
                return firstlevellist;

            string filter = string.Format("{0}={1}", pid, nodeid);
            //找当前节点的子节点
            if (pidType == typeof(string))
                filter = string.Format("{0}='{1}'", pid, nodeid);

            DataRow[] drs = dt.Select(filter, sort);

            foreach (DataRow dr in drs)
            {
                var tempnode = BuildTreeNode(dr);

                var idVal = dr[id].ToString();

                filter = string.Format("{0}={1}", pid, idVal);
                //找当前节点的子节点
                if (pidType == typeof(string))
                    filter = string.Format("{0}='{1}'", pid, idVal);

                var drsTemp = dt.Select(filter, sort);

                var rowCount = drsTemp.Length;//找当前节点的子节点

                tempnode.leaf = rowCount == 0;

                firstlevellist.Add(tempnode);
            }
            return firstlevellist;
        }

        /// <summary>
        /// 懒加载一棵树,对于数据库不是懒加载，客户端是懒加载
        /// </summary>
        /// <param name="list">整棵树数据，无奈为了判断叶子节点</param>
        /// <param name="pid">父节点id</param>
        /// <param name="id">id</param>
        /// <param name="nodeid">当前节点id</param>
        /// <returns></returns>
        public IList<TreeJSONBaseCheck> LazyLoadTreeCheckList(List<T> list, string pid, string id, string nodeid)
        {
            var treeList = LazyLoadTreeCheckList(list, pid, id, nodeid);

            return treeList.Cast<TreeJSONBaseCheck>().ToList();
        }

        ///// <summary>
        ///// 构建树节点,ColumnName为属性名
        ///// </summary>
        ///// <param name="row">具体数据行</param>
        ///// <returns>树的Json数据</returns>
        //private TreeJSONBase MyBuildTreeNode(DataRow row)
        //{
        //    var node = BuildTreeNode(row);

        //    //业务代码自己处理
        //    //if (string.IsNullOrWhiteSpace(node.PhId))            
        //    //{
        //    //    node.PhId = node.id;
        //    //}

        //    return node;
        //}

        /// <summary>
        /// 构建ExtJS的树结构
        /// </summary>
        /// <param name="list">树列表数据</param>
        /// <param name="pid">父节点字段名称</param>
        /// <param name="id">当前节点字段名称</param>
        /// <param name="firstLevelFilter">获取第一层数据的过滤条件</param>
        /// <param name="leveltype">层数类型top,other</param>
        /// <param name="level">首次获取几层</param>
        /// <returns></returns>
        public IList<TreeJSONBaseCheck> GetExtTreeCheckList(IList<T> list, string pid, string id, string firstLevelFilter, TreeDataLevelType leveltype, int level = 1000)
        {
            var treeList = GetExtTreeList(list, pid, id, firstLevelFilter, leveltype, level);

            return treeList.Cast<TreeJSONBaseCheck>().ToList();
        }

        /// <summary>
        /// 将实体集合转换成DataTable
        /// </summary>
        /// <param name="list">实体集合</param>
        /// <returns>对应集合的DataTable</returns>
        DataTable EntitiesToDataTable(IList<T> list)
        {
            var dt = new DataTable();

            if (list == null)
                return null;

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    dt.Columns.Add(pi.Name, pi.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    dt.Columns.Add(pi.Name, pi.PropertyType);
                }
            }

            foreach (var entity in list)
            {
                var row = dt.NewRow();

                foreach (PropertyInfo pi in typeof(T).GetProperties())
                {
                    //if (pi.PropertyType.FullName.IndexOf("DateTime") > 0)
                    //{
                    //    continue;
                    //}

                    var val = pi.GetValue(entity, null);
                    row[pi.Name] = val ?? DBNull.Value;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

    }
}
