﻿#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Rule
    * 类 名 称：			XmReportDtlRule
    * 文 件 名：			XmReportDtlRule.cs
    * 创建时间：			2020/1/17 
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

using GXM3.XM.Rule.Interface;
using GXM3.XM.Dac.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Rule
{
    /// <summary>
    /// XmReportReturn业务逻辑处理类
    /// </summary>
    public partial class XmReportReturnRule : EntRuleBase<XmReportReturnModel>, IXmReportReturnRule
    {
        #region 类变量及属性
        /// <summary>
        /// XmReportReturn数据访问处理对象
        /// </summary>
        IXmReportReturnDac XmReportReturnDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentDac as IXmReportReturnDac;
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
        public override bool DataCheck(IList<XmReportReturnModel> entities, out string prompt)
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
        public override void DataHandling<Int64>(ref IList<XmReportReturnModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.RuleHelper.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
            //赋外键值
            base.RuleHelper.DataHandlingForeignKey(ref entities, "MstPhid", masterId);

        }
        #endregion

        #region 实现 IXmReportReturnRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<XmReportReturnModel> ExampleMethod<XmReportReturnModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}