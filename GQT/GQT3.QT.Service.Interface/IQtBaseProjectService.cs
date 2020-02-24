#region Summary
/**************************************************************************************
    * 类 名 称：        IQtBaseProjectService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IQtBaseProjectService.cs
    * 创建时间：        2018/11/23 
    * 作    者：        刘杭    
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;

namespace GQT3.QT.Service.Interface
{
    /// <summary>
    /// QtBaseProject服务组装层接口
    /// </summary>
    public partial interface IQtBaseProjectService : IEntServiceBase<QtBaseProjectModel>
    {
        #region IQtBaseProjectService 业务添加的成员

        /// <summary>
        /// 获取最大项目库编码
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string CreateOrGetMaxProjCode(string year);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="qtBaseProjectModel"></param>
        /// <param name="subjectMstModel"></param>
        /// <returns></returns>
        SavedResult<Int64> Save2(QtBaseProjectModel qtBaseProjectModel, SubjectMstModel subjectMstModel);

        /// <summary>
        /// 判断是否是末级
        /// </summary>
        /// <param name="kmdm"></param>
        /// <returns></returns>
        Boolean JudgeIfEnd(string kmdm);

        /// <summary>
        /// 修改项目名称和填报部门
        /// </summary>
        /// <param name="qtBaseProjectModel"></param>
        /// <returns></returns>
        SavedResult<Int64> Update2(QtBaseProjectModel qtBaseProjectModel);

        /// <summary>
        /// 判断是否有明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Boolean JudgeHaveDtl(long id);


        /// <summary>
        /// 有明细时判断是否处于审批流程中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeletedResult DeleteIfDtl(long id);

        /// <summary>
        /// 没有明细时判断SubjectMst表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeletedResult Delete2(long id);

        /// <summary>
        /// 查找该单位下全部的预算科目
        /// </summary>
        /// <param name="FDwdm"></param>
        /// <param name="FKmlb"></param>
        /// <param name="FType"></param>
        /// <param name="FYear"></param>
        /// <returns></returns>
        PagedResult<QtBaseProjectModel> FindSubjectData(string FDwdm, string FKmlb, string FType, string FYear);

        #endregion
    }
}
