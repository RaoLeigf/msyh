﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本: 14.0.0.0
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，则所做更改将丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace SUP.CustomForm.ClientGen
{
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class PageEditTemplate : PageEditTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("@{\r\n    Layout = \"~/Views/Shared/_EFormLayout.cshtml\";\r\n}\r\n<style type=\"text/css\"" +
                    ">\r\n   \r\n     .row-parentLevel .x-grid-cell .x-grid-cell-inner {\r\n\t\t");
            
            #line 8 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SumRowStyle));
            
            #line default
            #line hidden
            this.Write("\r\n     }\r\n\r\n     .row-leafLevel .x-grid-cell .x-grid-cell-inner {\r\n\t\t");
            
            #line 12 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(NoSumRowStyle));
            
            #line default
            #line hidden
            this.Write("\r\n\t }\r\n\r\n\t .row-parentLevel .x-grid-cell {\r\n\t    ");
            
            #line 16 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SumRowStyle));
            
            #line default
            #line hidden
            this.Write("\r\n      }\r\n\r\n\t .row-leafLevel .x-grid-cell {\r\n\t    ");
            
            #line 20 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(NoSumRowStyle));
            
            #line default
            #line hidden
            this.Write(@"
      }
</style>

@section Script
{  
	<script type=""text/javascript"">
		var IsSso = '@ViewBag.IsSso';  //单点登录
        var otype = '@ViewBag.OType';
        var busid = '@ViewBag.ID';
		var billno = '@ViewBag.BillNo';
		var CurDate = '@ViewBag.CurDate';
		var CurTime = '@ViewBag.CurTime';
		var TreeOrgId = '@ViewBag.TreeOrgId';
		var TreeProjId = '@ViewBag.TreeProjId';
		var EditAttach = '@ViewBag.EditAttach';  //单据审核后也允许修改附件
		var info = Ext.htmlDecode('@ViewBag.WorkFlowInfo');
		var WorkFlowInfo = Ext.isEmpty(info) ? {} : Ext.decode(info);  //工作流对象
		var langInfo = Ext.htmlDecode('@ViewBag.NG3Lang');
		var Lang = (Ext.isEmpty(langInfo) || langInfo=='null') ? {} : Ext.decode(langInfo);  //多语言
		var IsApplyCheck = '@ViewBag.IsApplyCheck';  //打开自申请去审

		var BusType = '@ViewBag.BusType';            //2: 来自任务分解的原汇总信息
		var IDs = '@ViewBag.IDs';                    //任务分解下级单据id集合
		var PhidWork = '@ViewBag.PhidWork';          //任务分解流程id
		var PhidWorkNode = '@ViewBag.PhidWorkNode';  //任务分解节点id
		var PhidTemplate = '@ViewBag.PhidTemplate';  //任务分解模板id
	</script>

<script src=""~/NG3Resource/js/eformJs/");
            
            #line 49 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write("Edit.js?_v=");
            
            #line 49 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Common.GetJsVersion()));
            
            #line default
            #line hidden
            this.Write("\" type=\"text/javascript\" charset=\"gb2312\"></script>\r\n<script src=\"~/NG3Resource/j" +
                    "s/eformJs/");
            
            #line 50 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write("Ext.js?_v=");
            
            #line 50 "E:\NG3_Business\BusinessBaseClass\SUP\CustomForm\SUP.CustomForm.ClientGen\PageEditTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Common.GetJsVersion()));
            
            #line default
            #line hidden
            this.Write("\" type=\"text/javascript\" charset=\"gb2312\"></script>\r\n\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public class PageEditTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
