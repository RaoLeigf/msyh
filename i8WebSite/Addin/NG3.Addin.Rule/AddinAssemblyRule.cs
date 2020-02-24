#region Summary
/**************************************************************************************
    * 类 名 称：        AddinAssemblyRule
    * 命名空间：        NG3.Addin.Rule
    * 文 件 名：        AddinAssemblyRule.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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

using NG3.Addin.Rule.Interface;
using NG3.Addin.Dac.Interface;
using NG3.Addin.Model.Domain;

namespace NG3.Addin.Rule
{
	/// <summary>
	/// AddinAssembly业务逻辑处理类
	/// </summary>
    public partial class AddinAssemblyRule : EntRuleBase<AddinAssemblyModel>, IAddinAssemblyRule
    {
        #region 类变量及属性
		/// <summary>
        /// AddinAssembly数据访问处理对象
        /// </summary>
        IAddinAssemblyDac AddinAssemblyDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentDac as IAddinAssemblyDac;
            }
        }

        /// <summary>
        /// 业务单据编码请求类型（原先系统的BillType值，若有用户编码，这里请填写业务类型，不要为空）
        /// </summary>
        protected override string BillNoIdentity
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
        public override bool DataCheck(IList<AddinAssemblyModel> entities, out string prompt)
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
        public override void DataHandling<Int64>(ref IList<AddinAssemblyModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
 
        }
        #endregion

		#region 实现 IAddinAssemblyRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<AddinAssemblyModel> ExampleMethod<AddinAssemblyModel>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}
