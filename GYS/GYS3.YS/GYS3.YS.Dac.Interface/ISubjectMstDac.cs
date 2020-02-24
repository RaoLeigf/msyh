#region Summary
/**************************************************************************************
    * 类 名 称：        ISubjectMstDac
    * 命名空间：        GYS3.YS.Dac.Interface
    * 文 件 名：        ISubjectMstDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Dac.Interface
{
    /// <summary>
    /// SubjectMst数据访问层接口
    /// </summary>
    public partial interface ISubjectMstDac : IEntDacBase<SubjectMstModel>
    {
        #region ISubjectMstDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<SubjectMstModel> ExampleMethod<SubjectMstModel>(string param)

        #endregion
    }
}

