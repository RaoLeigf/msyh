using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GXM3.XM.Model.Request
{
    /// <summary>
    /// 预算支出导入中间model
    /// </summary>
    public class ProjectMstUpload
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNum { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string FProjCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string FProjName { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        public long FProPrice { get; set; }

        /// <summary>
        /// 业务条线编码
        /// </summary>
        public string FBusinessCode { get; set; }

        /// <summary>
        /// 业务条线
        /// </summary>
        public string FBusinessName { get; set; }

        /// <summary>
        /// 费用归属编码
        /// </summary>
        public string FBudgetDeptCode { get; set; }

        /// <summary>
        /// 费用归属
        /// </summary>
        public string FBudgetDept { get; set; }

        /// <summary>
        /// 费用说明
        /// </summary>
        public string FName { get; set; }

        /// <summary>
        /// 费用金额
        /// </summary>
        public long FAmount { get; set; }

        /// <summary>
        /// 科目编码
        /// </summary>
        public string FBudgetAccountsCode { get; set; }


        /// <summary>
        /// 科目名称
        /// </summary>
        public string FBudgetAccounts { get; set; }

        /// <summary>
        /// 支出分项编码
        /// </summary>
        public string FSubitemCode { get; set; }

        /// <summary>
        /// 支出分项名称
        /// </summary>
        public string FSubitemName { get; set; }

        /// <summary>
        /// 是否申请补助
        /// </summary>
        public string FIsApply { get; set;}

        /// <summary>
        /// 是否集中采购
        /// </summary>
        public string FIsPurchase { get; set; }

        /// <summary>
        /// 是否必须签报列支
        /// </summary>
        public string FIsReport { get; set; }

        /// <summary>
        /// 是否集体决议
        /// </summary>
        public string FIsResolution { get; set; }

        /// <summary>
        /// 是否个人额度分摊
        /// </summary>
        public string FIsShare { get; set; }
        
    }
}
