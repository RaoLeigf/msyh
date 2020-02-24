using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    sealed class DwRes
    {
        public static readonly string Crlf = "@ng";
        public static readonly string ListDatawindowName = "listdw";
        public static readonly string TableSectionStart = "table(";

        public static readonly string TableSection = "table";
        public static readonly string VisibleSection = "visible";
        public static readonly string XPosSection = "x";
        public static readonly string YPosSection = "y";


        public static readonly string DbColumnSectionStart = "column=(";
        public static readonly string TextSectionStart = "text(";
        public static readonly string ColumnSectionStart = "column(";
        public static readonly string ButtonSectionStart = "button(";
        public static readonly string GroupBoxSectionStart = "groupbox(";
        public static readonly string ValuesSectionStart = "values=";
        public static readonly string BitmapSectionStart = "bitmap(";

        public static readonly string HeadDwAuthName = "dw_1.";
        public static readonly string BodyDwAuthName = "dw_2.";

        public static readonly string HeadDwName = "dw1";
        public static readonly string BodyDwName = "dw2";

        public static readonly string SyntaxSection = "syntax";
        public static readonly string SyntaxLenSection = "syntaxlen";

        public static readonly string WidthSection = "width";
        public static readonly string HeightSection = "height";
        public static readonly string WindowSection = "window";
        public static readonly string Abslayout = "abslayout";
        public static readonly string Otid = "otid";
        public static readonly string Ref = "ref";
        public static readonly string TextArea = "textarea";
        public static readonly string ColSpan = "colspan";
        public static readonly string Cols = "cols";
        public static readonly string Collapse = "group";

        public static readonly string SqlSection = "retrieve";

        public static readonly string EditToolBarSection = "EditButtonBar";

        public static readonly string BarLeftSection = "BarLeft";
        public static readonly string BarRightSection = "BarRight";

        public static readonly string DddwSourceSection = "dddw_source";
        public static readonly string MulSelectSection = "mul_select";

        public static readonly string WindowOpenSection = "[window.open]";
        public static readonly string WindowDeleteCheck = "[window.deletechk]";
        public static readonly string WindowDelete = "[window.delete]";
        public static readonly string WindowSave = "[window.save]";
        public static readonly string WindowCheck = "[window.check]";
        public static readonly string WindowUncheck = "[window.uncheck]";
        public static readonly string WindowBeforeSave = "[save_rule]";

        public static readonly string ClickEventSection = "[click_event]";
        public static readonly string DoubleClickEventSection = "[double_event]";

        public static readonly string TabSection = "dw_";

        public static readonly string GridCollapse = "collapse";
        public static readonly string Title = "title";

        public static readonly string AsrGrid = "asrgrid";
        public static readonly string Position = "position";
        public static readonly string WfGrid = "wfgrid";

    }
}
