#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesDac
    * 命名空间：        GQT3.QT.Dac
    * 文 件 名：        ExamplesDac.cs
    * 创建时间：        2018/8/17 9:54:18 
    * 作    者：        丰立新    
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
using Enterprise3.NHORM.Dac;
using NHibernate.Criterion;

using GQT3.QT.Model;
using GQT3.QT.Dac.Interface;

namespace GQT3.QT.Dac
{
    /// <summary>
    /// Examples数据访问处理类
    /// </summary>
    public partial class ExamplesDac : EntDacBase<ExamplesModel>, IExamplesDac
    {
        #region 实现 IExamplesDac 业务添加的成员

        /// <summary>
        /// 获取所有有效的组织信息
        /// </summary>
        /// <returns>组织集合</returns>
        public IList<ExamplesModel> GetAllEffectiveOrgs()
        {
            var query =
                Session.QueryOver<ExamplesModel>()
                    .Where(t => t.IsCorp == 1)
                ;

            return query.List<ExamplesModel>();
        }


        /// <summary>
        /// 获取指定组织所有有效的部门信息
        /// </summary>
        /// <param name="ocode">组织编码（目前通过编码设置关系）</param>
        /// <returns></returns>
        public IList<ExamplesModel> GetAllEffectiveDepts(string ocode)
        {
            if (ocode == null) throw new ArgumentNullException("ocode");

            var query =
                Session.QueryOver<ExamplesModel>()
                    .Where(t => t.IsCorp == 0)
                    .WhereRestrictionOn(t => t.OCode).IsLike(ocode + ".", MatchMode.Start)
                ;

            return query.List<ExamplesModel>();
        }

        /// <summary>
        /// 获取指定Id集合的部门信息
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns>组织信息</returns>
        public IList<ExamplesModel> GetDeptsByIds(List<Int64> ids)
        {
            return GetOrgsByIds(ids, 0);
        }

        /// <summary>
        /// 获取指定Id集合的组织信息
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns>组织信息</returns>
        public IList<ExamplesModel> GetOrgsByIds(List<Int64> ids)
        {
            return GetOrgsByIds(ids, 1);
        }

        /// <summary>
        /// 获取部门或者组织信息数据
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="isCorp">标志</param>
        /// <returns></returns>
        IList<ExamplesModel> GetOrgsByIds(List<Int64> ids, Int16 isCorp)
        {
            var query =
                Session.QueryOver<ExamplesModel>()
                    .Where(t => t.IsCorp == isCorp)
                    .WhereRestrictionOn(t => t.PhId).IsIn(ids)
                ;

            return query.List<ExamplesModel>();
        }

        #endregion
    }
}

