#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesRule
    * 命名空间：        GYS3.YS.Rule
    * 文 件 名：        ExamplesRule.cs
    * 创建时间：        2018/8/17 9:53:12 
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
using SUP.Common.Base;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.NHORM.Rule;
using Enterprise3.Common.Base.Interface;

using GYS3.YS.Model;
using GYS3.YS.Dac.Interface;
using GYS3.YS.Rule.Interface;

namespace GYS3.YS.Rule
{
    /// <summary>
    /// Examples业务逻辑处理类
    /// </summary>
    public partial class ExamplesRule : EntRuleBase<ExamplesModel>, IExamplesRule
    {
        #region 类变量及属性
        /// <summary>
        /// 
        /// </summary>
        IExamplesDac ExamplesDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new ArgumentNullException(Resources.InitializeObject);

                return CurrentDac as IExamplesDac;
            }
        }

        /// <summary>
        /// 业务单据编码请求类型（原先系统的BillType值，若有用户编码，这里请填写业务类型，不要为空）
        /// </summary>
        public override string BillNoReqType
        {
            get { return string.Empty; }
        }
        #endregion

        #region 数据校验
        /// <summary>
        /// 数据校验(重写方法)
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="prompt">提示信息</param>
        /// <returns>处理成功返回True</returns>
        public override bool DataCheck(List<ExamplesModel> entities, out string prompt)
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
        /// <param name="dicMasterId">主表关键字值</param>
        public override void DataHandling(ref List<ExamplesModel> entities, Dictionary<string, object> dicMasterId = null)
        {
            //主键Id的属性名称[新增时赋主键值用，若是单主键，则无需传递idPropertyName];编码列的属性名称[新增时赋用户编码用，一般情况无需传递noPropertyName；若有用户编码，一定要给BillNoReqType属性赋业务类型]
            base.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
        }
        #endregion

        #region 实现 IExamplesRule 业务添加的成员

        /// <summary>
        /// 获取所有有效的组织信息
        /// </summary>
        /// <returns>组织集合</returns>
        public IList<ExamplesModel> GetAllEffectiveOrgs()
        {
            return ExamplesDac.GetAllEffectiveOrgs();
        }

        /// <summary>
        /// 获取指定组织所有有效的部门信息
        /// </summary>
        /// <param name="ocode">组织编码（目前通过编码设置关系）</param>
        /// <returns></returns>
        public IList<ExamplesModel> GetAllEffectiveDepts(string ocode)
        {
            return ExamplesDac.GetAllEffectiveDepts(ocode);
        }

        /// <summary>
        /// 获取指定Id集合的组织信息
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns>组织信息</returns>
        public IList<ExamplesModel> GetDeptsByIds(List<Int64> ids)
        {
            return ExamplesDac.GetDeptsByIds(ids);
        }

        /// <summary>
        /// 获取指定Id集合的组织信息
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns>组织信息</returns>
        public IList<ExamplesModel> GetOrgsByIds(List<Int64> ids)
        {
            return ExamplesDac.GetOrgsByIds(ids);
        }

        /// <summary>
        /// 获取指定Id集合的组织信息(并添加空部门信息PhId=0,OCode="none",OName="")
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="oname">空部门名称(默认空值)</param>
        /// <returns>组织信息</returns>
        public IList<ExamplesModel> GetDeptsAddEmptyByIds(List<Int64> ids, string oname = "")
        {
            var list = GetDeptsByIds(ids);

            list.Add(new ExamplesModel
            {
                PhId = 0,
                OCode = "none",
                OName = oname,
                IsCorp = 0
            });

            return list;
        }

        /// <summary>
        /// 获取指定Id集合的组织信息(并添加空组织信息PhId=0,OCode="none",OName="")
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="oname">空部门名称(默认空值)</param>
        /// <returns>组织信息</returns>
        public IList<ExamplesModel> GetOrgsAddEmptyByIds(List<Int64> ids, string oname = "")
        {
            var list = GetOrgsByIds(ids);

            list.Add(new ExamplesModel
            {
                PhId = 0,
                OCode = "none",
                OName = oname,
                IsCorp = 0
            });

            return list;
        }

        /// <summary>
        /// 构建组织树数据
        /// </summary>
        /// <param name="list">组织集合</param>
        /// <returns></returns>
        public IList<TreeJSONBase> GetOrgTreeData(IList<ExamplesModel> list)
        {
            return new ExtJsTreeBuilder<ExamplesModel>("PhId", "OName", "Selected").GetExtTreeList(list, "OCode", "OName", "PhId > 0", TreeDataLevelType.TopLevel, 3);
        }
        #endregion
    }
}
