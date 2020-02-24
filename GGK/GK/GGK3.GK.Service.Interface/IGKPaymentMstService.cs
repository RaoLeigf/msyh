#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Service.Interface
    * 类 名 称：			IGKPaymentMstService
    * 文 件 名：			IGKPaymentMstService.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using GGK3.GK.Model.Domain;
using GGK3.GK.Model.Enums;
using GQT3.QT.Model.Domain;

namespace GGK3.GK.Service.Interface
{
    /// <summary>
    /// GKPaymentMst服务组装层接口
    /// </summary>
    public partial interface IGKPaymentMstService : IEntServiceBase<GKPaymentMstModel>
    {
        #region IGKPaymentMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="gKPaymentMstEntity"></param>
        /// <param name="gKPaymentDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveGKPaymentMst(GKPaymentMstModel gKPaymentMstEntity, List<GKPaymentDtlModel> gKPaymentDtlEntities);

        /// <summary>
        /// 通过外键值获取GKPaymentDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<GKPaymentDtlModel> FindGKPaymentDtlByForeignKey<TValType>(TValType id);

        #endregion

        /// <summary>
        /// 保存付款单
        /// </summary>
        /// <param name="paymentEntity"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveGKPayment(GKPaymentModel paymentEntity);

        /// <summary>
        /// 获取资金拨付支付单信息
        /// </summary>
        /// <param name="phid">支付单主键</param>
        /// <returns></returns>
        GKPayment4ZjbfModel GetPayment4Zjbf(Int64 phid);

        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf(int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts);

        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf2(BillRequestModel billRequest, int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts);

        /// <summary>
        /// 单笔提交支付,如果返回null且errorMsg不为空，则表示出错了
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="errorMsg">错误消息提示信息</param>
        /// <param name="submitterID"></param>
        /// <returns></returns>
        GKPaymentModel SubmitPayment(Int64 phid, out string errorMsg, Int64 submitterID);

        /// <summary>
        /// 刷新单笔支付单支付状态,如果返回null且errorMsg不为空，则表示出错了
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="errorMsg">错误消息提示信息</param>
        /// <returns></returns>
        GKPaymentModel RefreshPaymentState(Int64 phid, out string errorMsg);


        /// <summary>
        /// 更新单据支付状态
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="payState"></param>
        /// <param name="submitterID"></param>
        /// <returns></returns>
        SavedResult<long> UpdatePaymentState(Int64 phid, byte payState, Int64 submitterID);

        /// <summary>
        /// 根据单据号集合作废单据
        /// </summary>
        /// <param name="phids">单据集合</param>
        /// <returns></returns>
        SavedResult<long> PostCancetGkPaymentList(List<long> phids);

        /// <summary>
        /// 导出网银模板
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        string ExportTemplate(List<GKPaymentDtlModel> list, int Type);


        /// <summary>
        /// 通过项目属性获取项目属性名称
        /// </summary>
        /// <param name="fProjStatus">项目属性</param>
        /// <returns></returns>
        string GetProjStatusName(int fProjStatus);

        /// <summary>
        /// 根据数组集合获取当个实体集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetPaymentArea(long[] ids,int type);

        /// <summary>
        /// 获取支付失败的和未支付的信息
        /// </summary>
        /// <returns></returns>
        List<GKPaymentMstModel> GetPaymentFailure(Dictionary<string, object> dicWhere);
    }
}
