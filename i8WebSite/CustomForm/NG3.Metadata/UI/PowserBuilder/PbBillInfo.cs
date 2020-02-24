using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;
using NG3.Metadata.UI.PowserBuilder.Controls;
using NG3.Metadata.UI.PowserBuilder.Events;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder
{
    /// <summary>
    /// PB单据信息
    /// </summary>
    public class PbBillInfo:MetadataGod
    {
        private string _description = string.Empty;
        private int _height = -1;
        private int _width = -1;
        private string _text = string.Empty;
        private string _name = string.Empty;
        private string _code = string.Empty;
        private string _istask = string.Empty;
        private string _reltable = string.Empty;
        private string _blobdocName = string.Empty;  //金格控件所在tab页标题
        private string _hasBlobdoc = "0";  //是否有金格控件, 0没有，1有且不在tab内，2在tab内
        private string _hasEppocx = "0";   //是否有进度控件
        private string _hasReport = "0";   //是否有报表
        private string _sumrowstyle = string.Empty;//汇总行样式
        private string _nosumrowstyle = string.Empty;//非汇总行样式
        private int _bodyCmpCount = 0;  //表体游离panel数量，不包括tab中的组件

        private PbGridInfo _pbList = new PbGridInfo();
        private PbHeadInfo _headInfo = new PbHeadInfo();
        private IList<PbGridInfo> _pbGrids = new List<PbGridInfo>();
        private IList<PbTabInfo> _pbTabInfos = new List<PbTabInfo>();
        private PbToolbarInfo _listToolbarInfo = new PbToolbarInfo();
        private PbToolbarInfo _detailToolbarInfo = new PbToolbarInfo();
        private PbOfficeInfo _officeInfo = new PbOfficeInfo();
        private PbScheduleInfo _scheduleInfo = new PbScheduleInfo();
        private PbGridInfo _asrGridInfo = new PbGridInfo();
        private PbGridInfo _wfGridInfo = new PbGridInfo();

        private PbEvent<PbExpressionImp> _editAddInitEvent = new PbEvent<PbExpressionImp>();
        private PbEvent<PbExpressionImp> _billDelCheckEvent = new PbEvent<PbExpressionImp>();
        private PbEvent<PbExpressionImp> _billDelUpdateEvent = new PbEvent<PbExpressionImp>();
        private PbEvent<PbExpressionImp> _billSaveUpdateEvent = new PbEvent<PbExpressionImp>();
        private PbEvent<PbExpressionImp> _billApprovalUpdateEvent = new PbEvent<PbExpressionImp>();
        private PbEvent<PbExpressionImp> _billUnApprovalUpdateEvent = new PbEvent<PbExpressionImp>();
        private PbEvent<PbExpressionImp> _billBeforeSaveEvent = new PbEvent<PbExpressionImp>();

        public int BodyCmpCount
        {
            get { return _bodyCmpCount; }
            set { _bodyCmpCount = value; }
        }

        public string BlobdocName
        {
            get { return _blobdocName; }
            set { _blobdocName = value; }
        }

        public string HasBlobdoc
        {
            get { return _hasBlobdoc; }
            set { _hasBlobdoc = value; }
        }

        public string HasEppocx
        {
            get { return _hasEppocx; }
            set { _hasEppocx = value; }
        }

        public string HasReport
        {
            get { return _hasReport; }
            set { _hasReport = value; }
        }

        public string SumRowStyle
        {
            get { return _sumrowstyle; }
            set { _sumrowstyle = value; }
        }

        public string NoSumRowStyle
        {
            get { return _nosumrowstyle; }
            set { _nosumrowstyle = value; }
        }

        /// <summary>
        /// 窗口描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 窗口高度
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// PB表头信息
        /// </summary>
        public PbHeadInfo HeadInfo
        {
            get { return _headInfo; }
            set { _headInfo = value; }
        }

        /// <summary>
        /// PB表体信息(多表体)
        /// </summary>
        public IList<PbGridInfo> PbGrids
        {
            get { return _pbGrids; }
            set { _pbGrids = value; }
        }

        /// <summary>
        /// 编辑窗口新增状态的事件
        /// </summary>
        public PbEvent<PbExpressionImp> EditAddInitEvent
        {
            get { return _editAddInitEvent; }
            set { _editAddInitEvent = value; }
        }

        /// <summary>
        /// 编辑窗口删除前的检测事件(和删除事件同一事务)
        /// </summary>
        public PbEvent<PbExpressionImp> BillDelCheckEvent
        {
            get { return _billDelCheckEvent; }
            set { _billDelCheckEvent = value; }
        }

        /// <summary>
        /// 列表窗口删除后的更新事件
        /// </summary>
        public PbEvent<PbExpressionImp> BillDelUpdateEvent
        {
            get { return _billDelUpdateEvent; }
            set { _billDelUpdateEvent = value; }
        }

        /// <summary>
        /// 编辑窗口保存事件
        /// </summary>
        public PbEvent<PbExpressionImp> BillSaveUpdateEvent
        {
            get { return _billSaveUpdateEvent; }
            set { _billSaveUpdateEvent = value; }
        }

        /// <summary>
        /// 核准单据事件
        /// </summary>
        public PbEvent<PbExpressionImp> BillApprovalUpdateEvent
        {
            get { return _billApprovalUpdateEvent; }
            set { _billApprovalUpdateEvent = value; }
        }

        /// <summary>
        /// 取消核准单据事件
        /// </summary>
        public PbEvent<PbExpressionImp> BillUnApprovalUpdateEvent
        {
            get { return _billUnApprovalUpdateEvent; }
            set { _billUnApprovalUpdateEvent = value; }
        }

        /// <summary>
        /// 保存前检测事件
        /// </summary>
        public PbEvent<PbExpressionImp> BillBeforeSaveEvent
        {
            get { return _billBeforeSaveEvent; }
            set { _billBeforeSaveEvent = value; }
        }

        /// <summary>
        /// PB列表界面信息
        /// </summary>
        public PbGridInfo PbList
        {
            get { return _pbList; }
            set { _pbList = value; }
        }

        /// <summary>
        /// 对应的Tab信息(Tab页有多个)
        /// </summary>
        public IList<PbTabInfo> PbTabInfos
        {
            get { return _pbTabInfos; }
            set { _pbTabInfos = value; }
        }

        /// <summary>
        /// 列表工具栏信息
        /// </summary>
        public PbToolbarInfo ListToolbarInfo
        {
            get { return _listToolbarInfo; }
            set { _listToolbarInfo = value; }
        }

        /// <summary>
        /// 明细工具栏信息
        /// </summary>
        public PbToolbarInfo DetailToolbarInfo
        {
            get { return _detailToolbarInfo; }
            set { _detailToolbarInfo = value; }
        }

        /// <summary>
        /// 金格控件信息
        /// </summary>
        public PbOfficeInfo OfficeInfo
        {
            get { return _officeInfo; }
            set { _officeInfo = value; }
        }

        /// <summary>
        /// 进度控件信息
        /// </summary>
        public PbScheduleInfo ScheduleInfo
        {
            get { return _scheduleInfo; }
            set { _scheduleInfo = value; }
        }

        /// <summary>
        /// 单据名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 自定义表单流水号
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 是否任务分解表单
        /// </summary>
        public string IsTask
        {
            get { return _istask; }
            set { _istask = value; }
        }

        /// <summary>
        /// 明细表关联关系
        /// </summary>
        public string Reltable
        {
            get { return _reltable; }
            set { _reltable = value; }
        }

        /// <summary>
        /// 附件单据体
        /// </summary>
        public PbGridInfo AsrGridInfo
        {
            get { return _asrGridInfo; }
            set { _asrGridInfo = value; }
        }

        /// <summary>
        /// 审批单据体
        /// </summary>
        public PbGridInfo WfGridInfo
        {
            get { return _wfGridInfo; }
            set { _wfGridInfo = value; }
        }


    }
}
