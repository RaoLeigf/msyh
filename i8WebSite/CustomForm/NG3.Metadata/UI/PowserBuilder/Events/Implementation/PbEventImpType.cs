using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Events
{
    /// <summary>
    /// PB的事件的实现类型
    /// </summary>
    public enum PbEventImpType
    {
        /// <summary>
        /// 帮助窗口
        /// </summary>
        Help = 0,
        /// <summary>
        /// 原始单据
        /// </summary>
        SysBill=1,
        /// <summary>
        /// 系统报表
        /// </summary>
        SysReport=2,
        /// <summary>
        /// 自定义报表
        /// </summary>
        CustomReport = 3,
        /// <summary>
        /// 打开网址
        /// </summary>
        ExternalWebPage=4,
        /// <summary>
        /// 打开附件
        /// </summary>
        Attachment=5,
        /// <summary>
        /// 触发当前功能
        /// </summary>
        CurrentBillFunction=6,
        /// <summary>
        /// 报表仓库报表
        /// </summary>
        WarehouseReport=7,

        Print,
        Exit,
        Close,
        Expression,
        Refresh,
        Add,
        AddRow,
        Edit,
        Delete,
        DeleteRow,
        Import,
        View,
        Copy,
        QueryFold,
        Save,
        SaveWithWorkflow,
        Insert,
        Verify,     //审核
        UnVerify,   //去审核
        Check,      //送审
        History,    //送审追踪
        ApplyCheck, //申请去审
        Subbill,    //下达
        Ok,         //提交
        Query,      //汇总
        Deal,       //原汇总信息
        Other
    }
}
