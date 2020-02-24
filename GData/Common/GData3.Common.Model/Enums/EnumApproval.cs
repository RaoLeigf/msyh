namespace GData3.Common.Model.Enums
{
    /// <summary>
    /// 审批状态，供审批单据使用
    /// </summary>
    public enum EnumApproval
    {
        /// <summary>
        /// 0-待送审
        /// </summary>
        PendingSend = 0,

        /// <summary>
        /// 1-待审批
        /// </summary>
        PendingApproval = 1,

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