#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Rule
    * 类 名 称：			YsAccountRule
    * 文 件 名：			YsAccountRule.cs
    * 创建时间：			2019/9/23 
    * 作    者：			王冠冠    
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

using GYS3.YS.Rule.Interface;
using GYS3.YS.Dac.Interface;
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Rule
{
	/// <summary>
	/// YsAccount业务逻辑处理类
	/// </summary>
    public partial class YsAccountRule : EntRuleBase<YsAccountModel>, IYsAccountRule
    {
        #region 类变量及属性
		/// <summary>
        /// YsAccount数据访问处理对象
        /// </summary>
        IYsAccountDac YsAccountDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentDac as IYsAccountDac;
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
        public override bool DataCheck(IList<YsAccountModel> entities, out string prompt)
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
        public override void DataHandling<Int64>(ref IList<YsAccountModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.RuleHelper.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
			//赋外键值
			//base.RuleHelper.DataHandlingForeignKey(ref entities, "PHIDMST", masterId);
 
        }
        #endregion

		#region 实现 IYsAccountRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<YsAccountModel> ExampleMethod<YsAccountModel>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}
