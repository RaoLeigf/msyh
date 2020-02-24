
var WebOffice = new WebOffice2015(); //创建WebOffice对象
var menufile1;
var menufile2;
var menufile3;
var menufile4;
//是否启用iWebOffice对象内的拷贝功能。默认：TRUE
var CopyEnabled = function (mBoolean) {
    WebOffice.WebObject.CopyEnabled = mBoolean;
}

//是否启用iWebOffice对象内的保存功能。默认：TRUE
var SaveEnabled = function (mBoolean) {
    WebOffice.WebObject.SaveEnabled = mBoolean;
}


//是否启用iWebOffice对象内的打印功能。默认：TRUE
var PrintEnabled = function (mBoolean) {
    WebOffice.WebObject.PrintEnabled = mBoolean;
}

//设置控件标题栏上的标题。
//注意：该属性只支持单行文本。
var SetCaption = function (title) {
    WebOffice.WebObject.Caption = title;
}

//设置控件是否启用最大化显示。默认：FALSE
//注意：该属性设置为true，控件最大化后置于父窗口顶层，不能操作父窗口内容。
var FullSize = function (mBoolean) {
    WebOffice.WebObject.FullSize = mBoolean;
}

//是否启用双击最大化功能。默认：TRUE
var DblClickFullSizeEnabled = function (mBoolean) {
    WebOffice.WebObject.DblClickFullSizeEnabled = mBoolean;
}
/*控制2015标题栏*/
var ShowTitleBar = function (mBoolean) {
    var style = WebOffice.WebObject.Style;
    style.ShowTitleBar = mBoolean;
}

/*控制2015菜单栏*/
var ShowMenuBar = function (mBoolean) {
    var style = WebOffice.WebObject.Style;
    style.ShowMenuBar = mBoolean;
}

/*控制Office工具栏*/
var ShowToolBars = function (mBoolean) {
    var style = WebOffice.WebObject.Style;
    style.ShowToolBars = mBoolean;
}

/*控制2015状态栏*/
var ShowStatusBar = function (mBoolean) {
    var style = WebOffice.WebObject.Style;
    style.ShowStatusBar = mBoolean;
}

//打开本地文件返回打开文档的全路径。
var FileFullName = function () {
    return WebOffice.WebObject.FullName;
}

//iWebOffice打开文件无历史痕迹功能。默认：FALSE
//注意：该属性建议在控件加载完成后设置。
var NoRecentFile = function (mBoolean) {
    WebOffice.WebObject.NoRecentFile = mBoolean;
}

//设置word锁定状态。
//第一位可以为0或1，0表示非锁定，1表示锁定；
//第二位可以为0或1，0表示锁定且不能复制，1表示锁定可以复制。
//当第一位为0时，第二位可以不填。
var EditType = function (status) {
    WebOffice.WebObject.EditType = status;
}
//style 0：无标题风格，1：有标题风格
var FullWindowStyle = function (style) {
    WebOffice.WebObject.FullWindowStyle = style;
}

//作用：保存文档
var SaveDocument = function () {
    if (WebOffice.WebSave()) {    //交互OfficeServer的OPTION="SAVEFILE"
        return WebOffice.PhId;
    } else {
        return 0;
    }
}
//显示或隐藏手写签批工具栏
function ShowSignature(mValue) {
    WebOffice.ShowCustomToolbar(mValue);//隐藏手写签批工具栏
}
//设置保护文档
var WebSetProtect = function (mBoolean, password) {
    var docType;
    if (WebOffice.FileType === ".doc" || WebOffice.FileType === ".docx" || WebOffice.FileType === ".wps") { // word
        docType = WebOffice.DocType.WOED;
    }
    if (WebOffice.FileType === ".xls" || WebOffice.FileType === ".xlsx" || WebOffice.FileType === ".et") { // execl
        docType = WebOffice.DocType.EXECL;
    }
    if (password === "") {
        password = "123456";
    }
    if (mBoolean === true) {
        if (docType === WebOffice.DocType.WOED) { //word 保护模式
            if (WebOffice.WebObject.ActiveDocument.ProtectionType === "-1") {
                WebOffice.WebObject.ActiveDocument.Protect(3, false, password);
                return true;
            }
        }
        if (docType === WebOffice.DocType.EXECL) { //word 保护模式，这里只保护表单1的其他的安自己需求编写
            if (!WebOffice.WebObject.ActiveDocument.Application.Sheets(1).ProtectContents) { //判断表单是否是保护的
                WebOffice.WebObject.ActiveDocument.Application.Sheets(1).Protect(password);
            }
            return true;
        }
    } else {
        if (docType === WebOffice.DocType.WOED) { //word 保护模式
            WebOffice.WebObject.ActiveDocument.Unprotect(password);;
            return true;
        }
        if (docType === WebOffice.DocType.EXECL) { //word 保护模式，这里只保护表单1的其他的安自己需求编写
            WebOffice.WebObject.ActiveDocument.Application.Sheets(1).Unprotect(password);
            return true;
        }
    }
    return false;
}

//显示痕迹和隐藏痕迹
var VBAShowRevisions = function (mBoolean) {
    var docType;
    if (WebOffice.FileType === ".doc" || WebOffice.FileType === ".docx" || WebOffice.FileType === ".wps") { // word
        docType = WebOffice.DocType.WOED;
    }
    if (WebOffice.FileType === ".xls" || WebOffice.FileType === ".xlsx" || WebOffice.FileType === ".et") { // execl
        docType = WebOffice.DocType.EXECL;
    }
    if (docType === WebOffice.DocType.WOED && WebOffice.WebObject.ActiveDocument.ProtectionType === "-1") {
        WebOffice.WebObject.ActiveDocument.TrackRevisions = mBoolean; //显示标记和隐藏标记
        WebOffice.WebObject.ActiveDocument.ShowRevisions = mBoolean; //显示痕迹或隐藏
        return true;
    }
    return false;
}

//调用模板进行套红fileName为phid,fileType为doc
function WebUseTemplate(fileName, fileType) {
    WebOffice.RecordID = fileName;            //RecordID:本文档记录编号
    WebOffice.FileName = fileName + fileType;
    WebOffice.FileType = fileType;            //FileType:文档类型  .doc  .xls  
    var currFilePath;
    if (WebOffice.WebAcceptAllRevisions()) {//清除正文痕迹的目的是为了避免痕迹状态下出现内容异常问题。
        currFilePath = WebOffice.getFilePath(); //获取2015特殊路径
        //alert(currFilePath);
        WebOffice.WebSaveLocalFile(currFilePath + WebOffice.iWebOfficeTempName);//将当前文档保存下来
        if (this.WebInsertFile()) {//下载服务器指定的文件 word 
            //WebOffice.OpenLocalFile(currFilePath + fileName);//打开该文件 +doc
            if (!WebOffice.VBAInsertFile("Content", currFilePath + WebOffice.iWebOfficeTempName)) {//插入文档
                alert("插入文档失败,模板设置Content书签");
                return;
            }
            alert("模板套红成功");
            return;
        }
        alert("下载文档失败");
        return;
    }
    alert("清除正文痕迹失败，套红中止!");
}


//根据文档名称插入文档
WebInsertFile = function () {
    var httpclient = WebOffice.obj.Http; //设置http对象
    httpclient.Clear();
    WebOffice.WebSetMsgByName("FILENAME", WebOffice.FileName); //加载FileName
    WebOffice.WebSetMsgByName("FILETYPE", WebOffice.FileType); //加载FileType
    WebOffice.WebSetMsgByName("RECORDID", WebOffice.RecordID); //加载RecordID
    WebOffice.WebSetMsgByName("OPTION", "LOADFILE"); //发送请求LOADFILE
    WebOffice.WebSetMsgByName("OpType", "LoadTemplate");
    httpclient.AddForm("FormData", WebOffice.WebSendMessage()); //这里是自定义json 传输格式。
    WebOffice.WebClearMessage(); //清除所有WebSetMsgByName参数
    if (WebOffice.LOADFILE(httpclient)) { //Http下载服务器文件   
        WebOffice.Status = "打开文档成功"; //Status：状态信息 
        return true;
    }
    WebOffice.Status = "打开文档失败"; //Status：状态信息 
    return false;
}

//控制自定义菜单项，目前支持四个，后续可根据需求添加
var AddCustomMenu = function (index1, index2, index3, index4) {
    var custommenu = WebOffice.obj.CustomMenu;
    if (index1 === "1") {

        menufile1 = custommenu.CreatePopupMenu();
        custommenu.AppendMenu(menufile1, 30, false, "显示痕迹");
        custommenu.AppendMenu(menufile1, 31, false, "隐藏痕迹");
        custommenu.AppendMenu(menufile1, 32, false, "获取痕迹");
        custommenu.Add(menufile1, "痕迹");
    }
    if (index2 === "1") {
        menufile2 = custommenu.CreatePopupMenu();
        custommenu.AppendMenu(menufile2, 40, false, "手写签名");
        custommenu.AppendMenu(menufile2, 41, false, "文字签名");
        custommenu.AppendMenu(menufile2, 42, false, "图形签名");
        custommenu.Add(menufile2, "签名");
    }
    if (index3 === "1") {
        menufile3 = custommenu.CreatePopupMenu();
        custommenu.AppendMenu(menufile3, 50, false, "添加模板标签");
        custommenu.AppendMenu(menufile3, 51, false, "定义模板标签");
        custommenu.AppendMenu(menufile3, 0, false, "-");
        custommenu.AppendMenu(menufile3, 52, false, "表单模板");
        custommenu.AppendMenu(menufile3, 53, false, "VBA套红定稿");
        custommenu.AppendMenu(menufile3, 54, false, "模板定稿");
        custommenu.AppendMenu(menufile3, 55, false, "会议纪要模板");
        custommenu.Add(menufile3, "模板");
    }
    if (index4 === "1") {
        menufile4 = custommenu.CreatePopupMenu();
        custommenu.AppendMenu(menufile4, 61, false, "切换到Excel");
        custommenu.Add(menufile4, "切换");
    }
    //通知系统更新菜单
    custommenu.Update();
}


//删除菜单栏子选项
var DeleteMenuItem = function (menufile, id) {
    var custommenu = WebOffice.obj.CustomMenu;
    custommenu.RemoveMenu(menufile, id, false);
}
//删除菜单栏
var DeleteMenu = function (index) {
    var custommenu = WebOffice.obj.CustomMenu;
    custommenu.Delete(index);//该方法会销毁菜单及其所有子菜单
}
//插入图片
varInsertImageByBookMark = function () {
    WebOffice.obj.ActiveDocument.Application.Selection.GoTo(-1, 0, 0, "Manager");
    WebOffice.obj.ActiveDocument.Application.Selection.InlineShapes.AddPicture(WebOffice.FilePath + WebOffice.ImageName);
    WebOffice.obj.ActiveDocument.InlineShapes.Item(1).ConvertToShape(); //转为浮动型
    WebOffice.obj.ActiveDocument.Shapes.Item(1).WrapFormat.Type = 5; //0:四周型  1：紧密型  2：穿越型环绕 3：浮于文字上方 4：上下型环绕 5：衬于文字下方  6：浮于文字上方
    return true;
}
var ChangeWordToExcel = function () {
    var sFileType = WebOffice.FileType;
    if (sFileType === ".doc" || sFileType === ".wps") {
        if (sFileType === ".doc") {
            WebOffice.FileType = ".xls";
            WebOffice.FileName = WebOffice.RecordID + WebOffice.FileType;
            WebOffice.WebOpen();
        } else if (sFileType === ".wps") {
            WebOffice.FileType = ".et";
            WebOffice.WebOpen();
            WebOffice.FileName = WebOffice.RecordID + WebOffice.FileType;
        }
        WebOffice.UpdateMenu("word");
    } else if (sFileType === ".xls" || sFileType === ".et") {
        if (sFileType === ".xls") {
            WebOffice.FileType = ".doc";
            WebOffice.FileName = WebOffice.RecordID + WebOffice.FileType;
            WebOffice.WebOpen();
        } else if (sFileType === ".et") {
            WebOffice.FileType = ".wps";
            WebOffice.FileName = WebOffice.RecordID + WebOffice.FileType;
            WebOffice.WebOpen();
        }
        WebOffice.UpdateMenu("excel");
    }
}

//添加区域保护
function WebAreaProtect() {
    var mText = window.prompt("请输入书签名称", "KingGrid", "");
    if (mText != null) WebOffice.WebAreaProtect(mText);
}
//取消区域保护
function WebAreaUnprotect() {
    var mText = window.prompt("请输入书签名称", "KingGrid", "");
    if (mText != null) WebOffice.WebAreaUnprotect(mText);
}
//作用：VBA套红示例
function WebInsertVBA() {
    //画线
    try {
        var object = WebOffice.WebObject.ActiveDocument;
        var myl = object.Shapes.AddLine(100, 60, 305, 60);
        var myl1 = object.Shapes.AddLine(326, 60, 520, 60);
        var myRange = WebOffice.WebObject.ActiveDocument.Range(0, 0);
        myRange.Select();
        var mtext = "★";
        WebOffice.WebObject.ActiveDocument.Application.Selection.Range.InsertAfter(mtext + "\n");
        var myRange = WebOffice.WebObject.ActiveDocument.Paragraphs(1).Range;
        myRange.ParagraphFormat.LineSpacingRule = 1.5;
        myRange.font.ColorIndex = 6;
        myRange.ParagraphFormat.Alignment = 1;
        myRange = WebOffice.WebObject.ActiveDocument.Range(0, 0);
        myRange.Select();
        mtext = "新中大发[２０１５]１５４号";
        WebOffice.WebObject.ActiveDocument.Application.Selection.Range.InsertAfter(mtext + "\n");
        myRange = WebOffice.WebObject.ActiveDocument.Paragraphs(1).Range;
        myRange.ParagraphFormat.LineSpacingRule = 1.5;
        myRange.ParagraphFormat.Alignment = 1;
        myRange.font.ColorIndex = 1;
        mtext = "新中大政务文件";
        WebOffice.WebObject.ActiveDocument.Application.Selection.Range.InsertAfter(mtext + "\n");
        myRange = WebOffice.WebObject.ActiveDocument.Paragraphs(1).Range;
        myRange.ParagraphFormat.LineSpacingRule = 1.5;
        myRange.Font.ColorIndex = 6;
        myRange.Font.Name = "仿宋_GB2312";
        myRange.font.Bold = true;
        myRange.Font.Size = 50;
        myRange.ParagraphFormat.Alignment = 1;
        WebOffice.WebObject.ActiveDocument.PageSetup.LeftMargin = 70;
        WebOffice.WebObject.ActiveDocument.PageSetup.RightMargin = 70;
        WebOffice.WebObject.ActiveDocument.PageSetup.TopMargin = 70;
        WebOffice.WebObject.ActiveDocument.PageSetup.BottomMargin = 70;
    } catch (e) {
        alert(e);
    }
}

//作用：获取痕迹
function WebGetRevisions() {
    var i;
    var Text = "";
    for (i = 1; i <= WebOffice.WebObject.Revisions.Count; i++) {
        try {
            Text = Text + WebOffice.WebObject.Revisions.Item(i).Author;
            if (WebOffice.WebObject.Revisions.Item(i).Type == "1") {
                Text = Text + Inter_Insert + WebOffice.WebObject.Revisions.Item(i).Range.Text + "\r\n";
            }
            else {
                Text = Text + Inter_Del + WebOffice.WebObject.Revisions.Item(i).Range.Text + "\r\n";
            }
        }
        catch (e) { ; }
    }
    if (Text != "") {
        alert(Inter_MarkContent + "\r\n" + Text);
    }
    else {
        alert(Inter_NomarkContent);
    }
}