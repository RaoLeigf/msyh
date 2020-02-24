#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectMstRule
    * 命名空间：        GXM3.XM.Rule
    * 文 件 名：        ProjectMstRule.cs
    * 创建时间：        2018/8/28 
    * 作    者：        李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.NHORM.Rule;

using GXM3.XM.Rule.Interface;
using GXM3.XM.Dac.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Rule
{
	/// <summary>
	/// ProjectMst业务逻辑处理类
	/// </summary>
    public partial class ProjectMstRule : EntRuleBase<ProjectMstModel>, IProjectMstRule
    {
        #region 类变量及属性
		/// <summary>
        /// ProjectMst数据访问处理对象
        /// </summary>
        IProjectMstDac ProjectMstDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentDac as IProjectMstDac;
            }
        }

        #endregion

        #region 数据校验
		/// <summary>
        /// 数据校验(重写方法)
        /// </summary>
        /// <param name="entities">实体集合</param>
		/// <param name="prompt">提示信息</param>
        /// <returns>处理成功返回True</returns>
        public override bool DataCheck(IList<ProjectMstModel> entities, out string prompt)
        {
			prompt = null;
            return true;
        }
        #endregion

        #region 数据处理
		/// <summary>
        /// 数据处理
        /// </summary>
        /// <para>
        /// 此方法必须进行重写,执行顺序在DataCheck方法之前。
        /// </para>
        /// <param name="entities">实体集合</param>
        /// <param name="masterId">主表关键字值</param>
        public override void DataHandling<Int64>(ref IList<ProjectMstModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.RuleHelper.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
 
        }
        #endregion

        #region 实现 IProjectMstRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectMstModel> ExampleMethod<ProjectMstModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 审批时同步项目数据
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSql"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <param name="DEF_BZ1"></param>
        /// <param name="mstCode"></param>
        /// <param name="dtlcodeList"></param>
        /// <param name="DJH"></param>
        /// <param name="DT1List"></param>
        /// <param name="DT2List"></param>
        /// <returns></returns>
        public int ApproveAddData(string userConn, string zbly_dm,List<string> valuesqlList, string mstSql, List<string> dtlSqlList, List<DateTime?> DJRQList, string DEF_BZ1, string mstCode, List<string> dtlcodeList, string DJH, List<DateTime?> DT1List, List<DateTime?> DT2List)
        {
            return ProjectMstDac.ApproveAddData(userConn, zbly_dm,valuesqlList, mstSql, dtlSqlList, DJRQList, DEF_BZ1, mstCode, dtlcodeList, DJH, DT1List, DT2List);
        }

        /// <summary>
        /// 年中调整预执行时同步项目数据
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSql"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <param name="DEF_BZ1"></param>
        /// <param name="mstCode"></param>
        /// <param name="dtlcodeList"></param>
        /// <param name="DJH"></param>
        /// <returns></returns>
        public int ApproveAddData2(string userConn, string zbly_dm, List<string>[] valuesqlList, List<string> mstSql, List<string> dtlSqlList, List<DateTime?>[] DJRQList, string DEF_BZ1, List<string> mstCode, List<string> dtlcodeList, List<string> DJH)
        {
            return ProjectMstDac.ApproveAddData2(userConn, zbly_dm, valuesqlList, mstSql, dtlSqlList, DJRQList, DEF_BZ1, mstCode, dtlcodeList, DJH);
        }
        #endregion
    }
}
