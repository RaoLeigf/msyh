#region Summary
/**************************************************************************************
    * 命名空间：			GJX3.JX.Dac
    * 类 名 称：			ThirdAttachmentDac
    * 文 件 名：			ThirdAttachmentDac.cs
    * 创建时间：			2019/10/9 
    * 作    者：			刘杭    
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

using GJX3.JX.Model.Domain;
using GJX3.JX.Dac.Interface;

namespace GJX3.JX.Dac
{
	/// <summary>
	/// ThirdAttachment数据访问处理类
	/// </summary>
    public partial class ThirdAttachmentDac : EntDacBase<ThirdAttachmentModel>, IThirdAttachmentDac
    {
		#region 实现 IThirdAttachmentDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ThirdAttachmentModel> ExampleMethod<ThirdAttachment>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

