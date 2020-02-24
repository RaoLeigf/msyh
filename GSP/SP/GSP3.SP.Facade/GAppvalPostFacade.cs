#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade
    * 类 名 称：			GAppvalPostFacade
    * 文 件 名：			GAppvalPostFacade.cs
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
	/// GAppvalPost业务组装处理类
	/// </summary>
    public partial class GAppvalPostFacade : EntFacadeBase<GAppvalPostModel>, IGAppvalPostFacade
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalPost业务逻辑处理对象
        /// </summary>
		IGAppvalPostRule GAppvalPostRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGAppvalPostRule;
            }
        }
		/// <summary>
        /// GAppvalPost4Oper业务逻辑处理对象
        /// </summary>
		IGAppvalPost4OperRule GAppvalPost4OperRule { get; set; }
		#endregion
        
		#region 实现 IGAppvalPostFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gAppvalPostEntity"></param>
		/// <param name="gAppvalPost4OperEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveGAppvalPost(GAppvalPostModel gAppvalPostEntity, List<GAppvalPost4OperModel> gAppvalPost4OperEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(gAppvalPostEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (gAppvalPost4OperEntities.Count > 0)
				{
					GAppvalPost4OperRule.Save(gAppvalPost4OperEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        /// <summary>
        /// 根据审批流程的id查找审批岗位
        /// </summary>
        /// <param name="phid">审批岗位id</param>
        /// <returns></returns>
        public List<GAppvalPostModel> FindAppvalPostByProcID(long phid) {

            SqlDao sqlDao = new SqlDao();

            List<GAppvalPostModel> postModels = sqlDao.FindAppvalPostByProcID(phid);

            if (postModels == null)
                return new List<GAppvalPostModel>();

            return postModels;
        }

        #endregion
    }
}

