using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.Base;
using SUP.Common.DataEntity;

namespace SUP.Common.Facade
{
    public interface IPersonFacade
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="defaultFilter">默认过滤串</param>
        /// <param name="sqlFilter"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="oCode"></param>
        /// <param name="leaf"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        DataTable GetDataTable(string defaultFilter, string sqlFilter, int pageSize, int PageIndex, ref int totalRecord, string oCode, bool leaf, string getType);


        /// <summary>
        /// 获取员工数据列表
        /// </summary>
        /// <param name="sqlFilter"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="oCode"></param>
        /// <param name="leaf"></param>
        /// <param name="relatIndex"></param>
        /// <param name="isRgt"></param>
        /// <returns></returns>
        DataTable GetEmpList(string sqlFilter, int pageSize, int PageIndex, ref int totalRecord, string oCode, bool leaf, string relatIndex, bool isRgt = false);

        /// <summary>
        /// 通过数据库表名，获取DataTable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileds">查询字段集</param>
        /// <param name="sqlWhere">查询条件</param>
        /// <param name="sortFiled">排序字段</param>
        /// <returns>DataTable</returns>
        DataTable GetDT(string tableName, string fileds, string sqlWhere, string sortFiled);

        /// <summary>
        /// 加载HR组织关系树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        IList<TreeJSONBase> LoadHrTree(string nodeid);

        /// <summary>
        /// 加载角色树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        IList<TreeJSONBase> LoadActorTree(string nodeid);

        /// <summary>
        /// 加载用户组
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        IList<Base.TreeJSONBase> LoadUGroupTree(string nodeid);

        /// <summary>
        /// 加载自定义联系人组
        /// </summary>
        /// <param name="LogId"></param>
        /// <returns></returns>
        IList<TreeJSONBase> LoadSelfGroupTree(string LogId);

        /// <summary>
        /// 加载外部人员分组
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        IList<TreeJSONBase> LoadOuterTree(string Product);

        /// <summary>
        /// 获取树节点的记忆状态
        /// </summary>
        /// <param name="TreeType"></param>
        /// <param name="BussType"></param>
        /// <returns></returns>
        TreeMemoryEntity GetTreeMemory(TreeMemoryType TreeType, string BussType = "all");

        /// <summary>
        /// 更新树节点的记忆状态
        /// </summary>
        /// <param name="treeMemoryEntity"></param>
        /// <returns></returns>
        int UpdataTreeMemory(TreeMemoryEntity treeMemoryEntity);
    }
}
