#region Summary
/**************************************************************************************
    * 类 名 称：        GKPaymentMstModel
    * 命名空间：        GGK3.GK.Model.Domain
    * 文 件 名：        GKPaymentMstModel.cs
    * 创建时间：        2019/5/23 
    * 作    者：        李明    
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
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;
using Newtonsoft.Json;

namespace GGK3.GK.Model.Domain
{
    /// <summary>
    /// 资金拨付支付单模型
    /// </summary>
    public class GKPayment4ZjbfModel 
    {
        //主表
        public GKPaymentMstModel Mst { get; set; }

        //明细表
        public List<GKPaymentDtl4ZjbfModel> Dtls { set; get; }

    }

}