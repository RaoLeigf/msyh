#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectMstDac
    * 命名空间：        GXM3.XM.Dac.Interface
    * 文 件 名：        IProjectMstDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Dac.Interface
{
	/// <summary>
	/// ProjectMst数据访问层接口
	/// </summary>
    public partial interface IProjectMstDac : IEntDacBase<ProjectMstModel>
    {
        #region IProjectMstDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectMstModel> ExampleMethod<ProjectMstModel>(string param)

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
        int ApproveAddData(string userConn, string zbly_dm, List<string> valuesqlList, string mstSql, List<string> dtlSqlList, List<DateTime?> DJRQList, string DEF_BZ1, string mstCode, List<string> dtlcodeList, string DJH, List<DateTime?> DT1List, List<DateTime?> DT2List);


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
        int ApproveAddData2(string userConn, string zbly_dm, List<string>[] valuesqlList, List<string> mstSql, List<string> dtlSqlList, List<DateTime?>[] DJRQList, string DEF_BZ1, List<string> mstCode, List<string> dtlcodeList, List<string> DJH);

        #endregion
    }
}

