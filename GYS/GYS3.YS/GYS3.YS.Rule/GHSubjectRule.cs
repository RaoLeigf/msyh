#region Summary
/**************************************************************************************
    * 类 名 称：        GHSubjectRule
    * 命名空间：        GYS3.YS.Rule
    * 文 件 名：        GHSubjectRule.cs
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
	/// GHSubject业务逻辑处理类
	/// </summary>
    public partial class GHSubjectRule : EntRuleBase<GHSubjectModel>, IGHSubjectRule
    {
        #region 类变量及属性
		/// <summary>
        /// GHSubject数据访问处理对象
        /// </summary>
        IGHSubjectDac GHSubjectDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentDac as IGHSubjectDac;
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
        public override bool DataCheck(IList<GHSubjectModel> entities, out string prompt)
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
        public override void DataHandling<Int64>(ref IList<GHSubjectModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.RuleHelper.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
 
        }
        #endregion

        #region 实现 IGHSubjectRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GHSubjectModel> ExampleMethod<GHSubjectModel>(string param)
        //{
        //    //编写代码
        //}

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
        public int AddData(string userConn, string zbly_dm,List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList)
        {
            return GHSubjectDac.AddData(userConn,zbly_dm,valuesqlList, mstSqlList, dtlSqlList, DJRQList);
        }

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
        public int AddDataSP(string userConn, string zbly_dm, List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList)
        {
            return GHSubjectDac.AddDataSP(userConn,zbly_dm,valuesqlList, mstSqlList, dtlSqlList, DJRQList);
        }

        #endregion
    }
}
