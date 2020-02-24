
namespace GData3.Common.Model.Enums
{
    /// <summary>
    /// 单据审批状态，供业务单据使用
    /// </summary>
    public enum EnumBillApproval
    {
        /// <summary>
        /// 0-待送审
        /// </summary>
        PendingSend = 0,

        /// <summary>
        /// 1-审批中
        /// </summary>
        InApproval = 1,

        /// <summary>
        /// 2-未通过
        /// </summary>
        NotPass = 2,

        /// <summary>
        /// 9-审批通过
        /// </summary>
        Approved = 9
    }
}