using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataAccess;
using NG3;
using System.Data;
using SUP.Common.Rule;

namespace SUP.Common.Facade
{
    public class PersonFacade : IPersonFacade
    {
        /// <summary>
        /// 
        /// </summary>
        private PersonDac PersonDac;
        /// <summary>
        /// 
        /// </summary>
        public PersonFacade()
        {
            PersonDac = new PersonDac();
        }

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
        [DBControl]
        public DataTable GetEmpList(string sqlFilter, int pageSize, int PageIndex, ref int totalRecord, string oCode, bool leaf, string relatIndex, bool isRgt = false)
        {
            return PersonDac.GetEmpList(sqlFilter, pageSize, PageIndex, ref totalRecord, oCode, leaf, relatIndex, isRgt);
        }

        /// <summary>
        /// 通过数据库表名，获取DataTable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileds">查询字段集</param>
        /// <param name="sqlWhere">查询条件</param>
        /// <param name="sortFiled">排序字段</param>
        /// <returns>DataTable</returns>
        [DBControl]
        public DataTable GetDT(string tableName, string fileds, string sqlWhere, string sortFiled)
        {
            return PersonDac.GetDT(tableName, fileds, sqlWhere, sortFiled);
        }

        /// <summary>
        /// 加载HR组织关系树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        [DBControl]
        public IList<Base.TreeJSONBase> LoadHrTree(string nodeid)
        {
            return PersonDac.LoadHrTree(nodeid);
        }

        /// <summary>
        /// 获取树节点的记忆状态
        /// </summary>
        /// <param name="TreeType"></param>
        /// <param name="BussType"></param>
        /// <returns></returns>
        [DBControl]
        public DataEntity.TreeMemoryEntity GetTreeMemory(DataEntity.TreeMemoryType TreeType, string BussType = "all")
        {
            return PersonDac.GetTreeMemory(TreeType, BussType);
        }

        /// <summary>
        /// 更新树节点的记忆状态
        /// </summary>
        /// <param name="treeMemoryEntity"></param>
        /// <returns></returns>
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int UpdataTreeMemory(DataEntity.TreeMemoryEntity treeMemoryEntity)
        {
            return PersonDac.UpdataTreeMemory(treeMemoryEntity);
        }

        /// <summary>
        /// 获取人员数据列表
        /// </summary>
        /// <param name="defaultFilter"></param>
        /// <param name="sqlFilter"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="oCode"></param>
        /// <param name="leaf"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        [DBControl]
        public DataTable GetDataTable(string defaultFilter, string sqlFilter, int pageSize, int PageIndex, ref int totalRecord, string oCode, bool leaf, string getType)
        {
            return new PersonRule().GetDataTable(defaultFilter, sqlFilter, pageSize, PageIndex, ref  totalRecord, oCode, leaf, getType);

        }

        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        [DBControl]
        public IList<Base.TreeJSONBase> LoadActorTree(string nodeid)
        {
            return PersonDac.LoadActorTree(nodeid);
        }

        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        [DBControl]
        public IList<Base.TreeJSONBase> LoadUGroupTree(string nodeid)
        {
            return PersonDac.LoadUGroupTree(nodeid);
        }

        /// <summary>
        /// 加载自定义联系人组
        /// </summary>
        /// <param name="LogId"></param>
        /// <returns></returns>
        [DBControl]
        public IList<Base.TreeJSONBase> LoadSelfGroupTree(string LogId)
        {
            return PersonDac.LoadSelfGroupTree(LogId);
        }

        /// <summary>
        /// 加载外部人员分组
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public IList<Base.TreeJSONBase> LoadOuterTree(string Product)
        {
            return PersonDac.LoadOuterTree(Product);
        }
    }
}
