#region Summary
/**************************************************************************************
    * 类 名 称：        IGHSubjectDac
    * 命名空间：        GYS3.YS.Dac.Interface
    * 文 件 名：        IGHSubjectDac.cs
    * 创建时间：        2018/11/26 
    * 作    者：        董泉伟    
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Dac.Interface
{
	/// <summary>
	/// GHSubject数据访问层接口
	/// </summary>
    public partial interface IGHSubjectDac : IEntDacBase<GHSubjectModel>
    {
        #region IGHSubjectDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GHSubjectModel> ExampleMethod<GHSubjectModel>(string param)

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSqlList"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <returns></returns>
        int AddData(string userConn, string zbly_dm,List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList);

        /// <summary>
        /// 基本支出审批同步数据到老G6H数据库
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSqlList"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <returns></returns>
        int AddDataSP(string userConn, string zbly_dm, List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList);

        #endregion
    }
}

