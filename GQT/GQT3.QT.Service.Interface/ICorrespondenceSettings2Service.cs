#region Summary
/**************************************************************************************
    * 类 名 称：        ICorrespondenceSettings2Service
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        ICorrespondenceSettings2Service.cs
    * 创建时间：        2018/9/6 
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
using SUP.Common.DataEntity;

using GQT3.QT.Model.Domain;
using GQT3.QT.Model;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// CorrespondenceSettings2服务组装层接口
	/// </summary>
    public partial interface ICorrespondenceSettings2Service : IEntServiceBase<CorrespondenceSettings2Model>
    {
        #region ICorrespondenceSettings2Service 业务添加的成员

        /// <summary>
        /// 是否为申报单位的设置
        /// </summary>
        /// <returns>返回Json串</returns>
        CommonResult<CorrespondenceSettings2Model> UpdateIfSBOrg(List<OrganizeModel> models, List<CorrespondenceSettings2Model> DeleteData, List<OrganizeModel> InsertData);

        /// <summary>
        /// 支出类别关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        CommonResult<CorrespondenceSettings2Model> UpdateZCLB(string OrgCode, string OrgPhId, List<CorrespondenceSettings2Model> mydelete, List<ExpenseCategoryModel> myinsert);

        /// <summary>
        /// 判断是否是末级组织
        /// </summary>
        /// <param name="ParentOrg"></param>
        /// <returns></returns>
        PagedResult<OrgRelatitem2Model> LoadWithPageIsend(string ParentOrg);

        /// <summary>
        /// 保存项目类型对应项目预算明细显示格式设置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CommonResult<CorrespondenceSettings2Model> UpdateYSDtlGS(string data);

        /// <summary>
        /// 根据按钮主键取对应关系列表基础数据详细(得到的PhId为对应关系的主键)（没有对应关系的数据）(按钮权限)
        /// </summary>
        /// <returns>返回Json串</returns>
        string GetOrgListNoDYGXdtl(string Dwdm);

        /// <summary>
        /// 页面功能控制
        /// </summary>
        /// <param name="Setcode"></param>
        /// <param name="SetPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        CommonResult UpdateControlSet(string Setcode, string SetPhId, List<CorrespondenceSettings2Model> mydelete, List<OrganizeModel> myinsert);

        /// <summary>
        /// 根据操作员取申报单位
        /// </summary>
        /// <param name="USERID"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetSBUnit(long USERID);

        /// <summary>
        /// 根据项目代码获取对应归口部门
        /// </summary>
        /// <param name="Dwdm"></param>
        /// <returns></returns>
        List<OrganizeModel> GetBMListDYGXdtl(string Dwdm);

        /// <summary>
        /// 根据项目代码获取对应归口部门（没有对应关系的）
        /// </summary>
        /// <param name="Dwdm"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetBMListNoDYGXdtl(string Dwdm, string OrgId);

        /// <summary>
        /// 归口项目对应部门设置关系的改变
        /// </summary>
        /// <returns></returns>
        SavedResult<Int64> UpdateGKXM(List<long> mydelete, List<CorrespondenceSettings2Model> myinsert);

        /// <summary>
        /// 归口项目对应部门设置关系的改变
        /// </summary>
        /// <returns></returns>
        SavedResult<Int64> SaveJXJLset(List<CorrespondenceSettings2Model> updateinfo, List<string> deleteinfo);
        #endregion
    }
}
