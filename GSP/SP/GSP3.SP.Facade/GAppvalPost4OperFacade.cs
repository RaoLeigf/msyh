#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade
    * 类 名 称：			GAppvalPost4OperFacade
    * 文 件 名：			GAppvalPost4OperFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GSP3.SP.Facade.Interface;
using GSP3.SP.Rule.Interface;
using GSP3.SP.Model.Domain;
using Enterprise3.WebApi.GSP3.SP.Model.Common;

namespace GSP3.SP.Facade
{
	/// <summary>
	/// GAppvalPost4Oper业务组装处理类
	/// </summary>
    public partial class GAppvalPost4OperFacade : EntFacadeBase<GAppvalPost4OperModel>, IGAppvalPost4OperFacade
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalPost4Oper业务逻辑处理对象
        /// </summary>
		IGAppvalPost4OperRule GAppvalPost4OperRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGAppvalPost4OperRule;
            }
        }
        #endregion



        #region 实现 IGAppvalPost4OperFacade 业务添加的成员

        /// <summary>
        /// 根据岗位id查找所有的操作员
        /// </summary>
        /// <param name="postId">岗位id</param>
        /// <returns></returns>
        public List<GAppvalPost4OperModel> GetOperatorsByPostID(long postId) {

            if (postId == 0) {
                return new List<GAppvalPost4OperModel>();
            }

            SqlDao sqlDao = new SqlDao();
            List<GAppvalPost4OperModel> operModels = sqlDao.GetOperatorsByPostID(postId);

            return operModels;
        }

        #endregion
    }
}

