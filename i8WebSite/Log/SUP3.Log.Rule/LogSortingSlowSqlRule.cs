#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingSlowSqlRule
    * 命名空间：        SUP3.Log.Rule
    * 文 件 名：        LogSortingSlowSqlRule.cs
    * 创建时间：        2017/11/4 
    * 作    者：        洪鹏    
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

using SUP3.Log.Rule.Interface;
using SUP3.Log.Dac.Interface;
using SUP3.Log.Model.Domain;

namespace SUP3.Log.Rule
{
	/// <summary>
	/// LogSortingSlowSql业务逻辑处理类
	/// </summary>
    public partial class LogSortingSlowSqlRule : EntRuleBase<LogSortingSlowSqlModel>, ILogSortingSlowSqlRule
    {
        #region 类变量及属性
		/// <summary>
        /// LogSortingSlowSql数据访问处理对象
        /// </summary>
        ILogSortingSlowSqlDac LogSortingSlowSqlDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentDac as ILogSortingSlowSqlDac;
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
        public override bool DataCheck(IList<LogSortingSlowSqlModel> entities, out string prompt)
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
        public override void DataHandling<Int64>(ref IList<LogSortingSlowSqlModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
 
        }
        #endregion

		#region 实现 ILogSortingSlowSqlRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<LogSortingSlowSqlModel> ExampleMethod<LogSortingSlowSqlModel>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}
