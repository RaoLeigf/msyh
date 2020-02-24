using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.Base;
using NG3.Data.Service;
using NG3.Data;
using SUP.Common.DataEntity;

namespace SUP.Common.DataAccess
{
    public class PersonDac
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonDac()
        {

        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetUserList(string sqlString, int pageSize, int PageIndex, ref int totalRecord)
        {
            string tmpSql = PaginationAdapter.GetPageDataSql(sqlString, pageSize, ref PageIndex, ref totalRecord, "order by cno");
            return DbHelper.GetDataTable(tmpSql);
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
        /// <param name="isRgt">是否需要组织查询权限</param>
        /// <returns></returns>
        public DataTable GetEmpList(string sqlFilter, int pageSize, int PageIndex, ref int totalRecord, string oCode, bool leaf, string relatIndex, bool isRgt = false)
        {
            string isNvl = DbHelper.Vendor == DbVendor.Oracle ? "nvl" : "isnull";
            string sql = "select hr_epm_main.cno,hr_epm_main.cname,'1' ctype," + isNvl + "(hr_epm_station.dept,hr_epm_main.dept) deptno,hr_epm_station.assigntype "
                       + "from hr_epm_main LEFT JOIN hr_epm_station ON hr_epm_station.ccode=hr_epm_main.ccode AND systype='0'";
            string sortField = "order by cno";
            if (!string.IsNullOrEmpty(sqlFilter))
            {
                sql += " where " + sqlFilter;
            }
            if (!string.IsNullOrEmpty(oCode))
            {
                string tmpWhere = string.Empty; ;
                if (leaf) //叶子节点
                {
                    tmpWhere = string.Format("deptno='{0}'", oCode);
                }
                else
                {
                    string relatId = GetHrRelatCode();
                    tmpWhere = string.Format("deptno in (select ocode from fg_orgrelatitem where fg_orgrelatitem.relatid ='{0}' and relatindex like '{1}%')", relatId, relatIndex);
                }
                sql = string.Format("select * from ({0}) tmpTable where {1}", sql, tmpWhere);
            }
            else
            {
                sql = string.Format("select * from ({0}) tmpTable", sql);
            }
            if (isRgt)
            {
                //增加组织查询权限
            }
            sql = PaginationAdapter.GetPageDataSql(sql, pageSize, ref PageIndex, ref totalRecord, sortField);
            return DbHelper.GetDataTable(sql);
        }

        /// <summary>
        /// 通过数据库表名，获取DataTable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileds">查询字段集</param>
        /// <param name="sqlWhere">查询条件</param>
        /// <param name="sortFiled">排序字段</param>
        /// <returns>DataTable</returns>
        public DataTable GetDT(string tableName, string fileds, string sqlWhere, string sortFiled)
        {
            string sql = string.Format("select {0} from {1}", fileds, tableName);
            if (!string.IsNullOrEmpty(sqlWhere))
            {
                sql += " where " + sqlWhere;
            }
            if (!string.IsNullOrEmpty(sortFiled))
            {
                sql += " order by " + sortFiled;
            }
            return DbHelper.GetDataTable(sql);
        }

        /// <summary>
        /// 根据“人力资源组织属性”获取人力资源组织关系代码
        /// </summary>
        /// <returns></returns>
        public string GetHrRelatCode()
        {
            string selSql = "SELECT relatid  FROM fg_orgrelat  WHERE attrcode = '18'";
            object retObj = DbHelper.ExecuteScalar(selSql);
            if (retObj != null)
            {
                return retObj.ToString().Trim();
            }
            return "";
        }

        /// <summary>
        /// 得到HR组织关系树的DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable GetHrOrgRelatTree()
        {
            string relateId = GetHrRelatCode();
            if (string.IsNullOrEmpty(relateId))
            {
                throw new Exception("不能找到HR组织关系,请检查您的组织关系设置!");
            }
            string selSql = "SELECT fg_orgrelatitem.ocode,fg_orgrelatitem.parentorg,fg_orgrelatitem.relatindex,fg_orglist.oname,fg_orglist.bopomofo "
                         + "FROM fg_orgrelatitem LEFT OUTER JOIN fg_orglist ON fg_orgrelatitem.ocode=fg_orglist.ocode "
                         + "WHERE fg_orgrelatitem.relatid ='" + relateId + "' AND fg_orglist.isactive='1' order by relatindex";
            return DbHelper.GetDataTable(selSql);
        }

        /// <summary>
        /// 加载HR组织关系树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> LoadHrTree(string nodeid)
        {
            string filter = "(parentorg is null or parentorg='')";
            DataTable dt = GetHrOrgRelatTree();
            dt.PrimaryKey = new DataColumn[] { dt.Columns["parentorg"], dt.Columns["ocode"] };
            return new MenuTreeBuilder().GetExtTreeList(dt, "parentorg", "ocode", filter, TreeDataLevelType.TopLevel);
        }

        /// <summary>
        /// 加载角色关系树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> LoadActorTree(string nodeid)
        {
            string filter = "(parentorg='')";
            DataTable dt = DbHelper.GetDataTable("SELECT actorid ocode,parentid parentorg,nodeindex relatindex,memo oname,dbo.fun_getPY(memo) bopomofo FROM fg_actor");
            DataRow dr = dt.NewRow();
            dr["ocode"] = "××％#$%(*)";
            dr["oname"] = "所有角色";
            dr["parentorg"] = "";
            dr["relatindex"] = "";
            dr["bopomofo"] = "SYJS";
            dt.Rows.InsertAt(dr, 0);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["parentorg"], dt.Columns["ocode"] };
            return new MenuTreeBuilder().GetExtTreeList(dt, "parentorg", "ocode", filter, TreeDataLevelType.TopLevel);
        }

        /// <summary>
        /// 加载用户组树
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> LoadUGroupTree(string nodeid)
        {
            string filter = "(parentorg='')";
            DataTable dt = DbHelper.GetDataTable("select DISTINCT fg_ugroup.g_code ocode,fg_ugroup.g_name oname,'allugroup' parentorg,'' relatindex, dbo.fun_getPY(fg_ugroup.g_name) bopomofo,fg_ugroup.priority from fg_guser LEFT OUTER JOIN fg_ugroup ON fg_ugroup.g_code=fg_guser.g_code order by fg_ugroup.priority desc,fg_ugroup.g_name asc");
            DataRow dr = dt.NewRow();
            dr["ocode"] = "allugroup";
            dr["oname"] = "所有用户组";
            dr["parentorg"] = "";
            dr["relatindex"] = "";
            dr["bopomofo"] = "SYYHZ";
            dt.Rows.InsertAt(dr, 0);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["parentorg"], dt.Columns["ocode"] };
            return new MenuTreeBuilder().GetExtTreeList(dt, "parentorg", "ocode", filter, TreeDataLevelType.TopLevel);
        }

        /// <summary>
        /// 加载自定义联系人组
        /// </summary>
        /// <param name="LogId"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> LoadSelfGroupTree(string LogId)
        {
            string filter = "(parentorg='')";
            string lj = DbHelper.Vendor == DbVendor.Oracle ? "||" : "+";
            string sql = string.Format(@"select fg_msg_linkmangroup.ccode ocode,cname oname,'allselfgroup' parentorg,'' relatindex,dbo.fun_getPY(cname) bopomofo from fg_msg_linkmangroup where fillemp ='{0}' or (cno in 
                        (select groupid from user_grouprgts where usertype='1' and userid='{0}' 
                        union select groupid from user_grouprgts where usertype='7' AND userid IN 
                        (SELECT actorid from fg_useractor where userid='{0}') 
                        union select groupid from user_grouprgts where usertype='0' AND EXISTS
                        (select logid from secuser where logid='{0}' and deptno IN (select ocode from fg_orgrelatitem 
                        where relatindex LIKE (select DISTINCT relatindex FROM fg_orgrelatitem where ocode=user_grouprgts.userid 
                        AND user_grouprgts.usertype='0' AND fg_orgrelatitem.relatid IN(SELECT fg_orgrelat.relatid from 
                        fg_orgrelat where  fg_orgrelat.attrcode='18')){1}'%'))))", LogId, lj);
            DataTable dt = DbHelper.GetDataTable(sql);
            DataRow dr = dt.NewRow();
            dr["ocode"] = "allselfgroup";
            dr["oname"] = "所有联系人组";
            dr["parentorg"] = "";
            dr["relatindex"] = "";
            dr["bopomofo"] = "SYLXRZ";
            dt.Rows.InsertAt(dr, 0);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["parentorg"], dt.Columns["ocode"] };
            return new MenuTreeBuilder().GetExtTreeList(dt, "parentorg", "ocode", filter, TreeDataLevelType.TopLevel);
        }

        /// <summary>
        /// 加载外部人员分组
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> LoadOuterTree(string Product)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ocode");
            dt.Columns.Add("oname");
            dt.Columns.Add("parentorg");
            dt.Columns.Add("relatindex");
            dt.Columns.Add("bopomofo");
            DataRow dr = dt.NewRow();
            dr["ocode"] = "allubemp";
            dr["oname"] = "联盟体人才";
            dr["parentorg"] = "";
            dr["relatindex"] = "";
            dr["bopomofo"] = "LMTRC";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ocode"] = "alloutemp";
            dr["oname"] = "外部联系人";
            dr["parentorg"] = "";
            dr["relatindex"] = "";
            dr["bopomofo"] = "WBLXR";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ocode"] = "allkehemp";
            dr["oname"] = "客户";
            dr["parentorg"] = "";
            dr["relatindex"] = "";
            dr["bopomofo"] = "KH";
            dt.Rows.Add(dr);

            if (Product.ToLower() == "i6")
            {
                dr = dt.NewRow();
                dr["ocode"] = "alluicemp";
                dr["oname"] = "UIC会员";
                dr["parentorg"] = "";
                dr["relatindex"] = "";
                dr["bopomofo"] = "UICHY";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ocode"] = "allfxsemp";
                dr["oname"] = "分销商";
                dr["parentorg"] = "";
                dr["relatindex"] = "";
                dr["bopomofo"] = "FXS";
                dt.Rows.Add(dr);
            }
            return new MenuTreeBuilder().GetExtTreeList(dt, "parentorg", "ocode", "(parentorg='')", TreeDataLevelType.TopLevel);
        }

        #region 树节点记忆功能处理
        /// <summary>
        /// 获取树节点的记忆状态
        /// </summary>
        /// <param name="TreeType"></param>
        /// <param name="BussType"></param>
        /// <returns></returns>
        public TreeMemoryEntity GetTreeMemory(TreeMemoryType TreeType, string BussType)
        {
            string sql = string.Format("SELECT * from fg_comhelp_treememo where logid='{0}' and ocode='{1}' and comhelptype='{2}' and busstype='{3}' and ismemo='1'",
                NG3.AppInfoBase.LoginID, NG3.AppInfoBase.OCode, TreeType.ToString(), BussType);
            DataTable tmpDT = DbHelper.GetDataTable(sql);
            TreeMemoryEntity treeMemoryEntity = new TreeMemoryEntity(NG3.AppInfoBase.LoginID, NG3.AppInfoBase.OCode, TreeType, BussType);
            if (null == tmpDT || tmpDT.Rows.Count == 0)
            {
                treeMemoryEntity.IsMemo = false;
            }
            else
            {
                treeMemoryEntity.FoucedNodeValue = tmpDT.Rows[0]["FoucedNodeValue"].ToString();
            }
            return treeMemoryEntity;
        }

        /// <summary>
        /// 更新树节点的记忆状态
        /// </summary>
        /// <param name="treeMemoryEntity"></param>
        /// <returns></returns>
        public int UpdataTreeMemory(TreeMemoryEntity treeMemoryEntity)
        {
            string sql = string.Format("SELECT * from fg_comhelp_treememo where logid='{0}' and ocode='{1}' and comhelptype='{2}' and busstype='{3}'",
               treeMemoryEntity.LogId, treeMemoryEntity.OCode, treeMemoryEntity.TreeType.ToString(), treeMemoryEntity.BussType);
            DataTable tmpDT = DbHelper.GetDataTable(sql);
            DataRow dr;
            if (tmpDT.Rows.Count == 0)
            {
                dr = tmpDT.NewRow();
                tmpDT.Rows.Add(dr);
            }
            else
            {
                dr = tmpDT.Rows[0];
            }
            dr["logid"] = treeMemoryEntity.LogId;
            dr["ocode"] = treeMemoryEntity.OCode;
            dr["comhelptype"] = treeMemoryEntity.TreeType.ToString();
            dr["foucednodevalue"] = treeMemoryEntity.FoucedNodeValue;
            dr["ismemo"] = treeMemoryEntity.IsMemo ? 1 : 0;
            dr["busstype"] = treeMemoryEntity.BussType;
            return DbHelper.Update(tmpDT, sql);
        }
        #endregion

        public bool IsOracle()
        {
            return DbHelper.Vendor == DbVendor.Oracle;
        }
    }

    class HrTreeJSONBase : TreeJSONBase
    {
        public virtual string pid { get; set; }
        public virtual string relatindex { get; set; }
        public virtual string bopomofo { get; set; }
    }

    public class MenuTreeBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            HrTreeJSONBase node = new HrTreeJSONBase();
            node.pid = dr["parentorg"].ToString();
            node.expanded = string.IsNullOrEmpty(node.pid);
            node.id = dr["ocode"].ToString();
            node.text = dr["oname"].ToString();
            node.relatindex = dr["relatindex"].ToString();
            node.bopomofo = dr["bopomofo"].ToString();
            return node;
        }
    }
}
