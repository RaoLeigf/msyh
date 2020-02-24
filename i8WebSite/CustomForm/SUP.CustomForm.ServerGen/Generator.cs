using System;
using NG3.Metadata.UI.PowserBuilder;
using NG3.Metadata.UI.PowserBuilder.Controls;
using SUP.CustomForm.ServerGen.AppTemplate;

namespace SUP.CustomForm.ServerGen
{
    public class Generator
    {
        /// <summary>
        /// 生成dll程序集;
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyPath">生成程序集路径;</param>
        /// <returns></returns>
        public static bool Generate(PbBillInfo billInfo, string assemblyPath)
        {
            return Generate(billInfo, assemblyPath, false, string.Empty, string.Empty);
        }

        /// <summary>
        /// 生成带cs文件的dll程序集;
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyPath">生成程序集路径;</param>
        /// <param name="generatedCsFilePath">.cs文件生成路径;</param>
        /// <returns></returns>
        public static bool Generate(PbBillInfo billInfo, string assemblyPath, string generatedCsFilePath)
        {
            return Generate(billInfo, assemblyPath, true, generatedCsFilePath, string.Empty);
        }

        /// <summary>
        /// 生成PC表单服务端dll;
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyPath"></param>
        /// <param name="isGenerateCsFile"></param>
        /// <param name="csFilePath"></param>
        /// <returns></returns>
        public static bool Generate(PbBillInfo billInfo, string assemblyPath, bool isGenerateCsFile, string csFilePath, string ucode)
        {
            try
            {
                //参数设置
                var templateInfo = new TemplateInfo
                {
                    NameSpacePrefix = "SUP.CustomForm",
                    ClassName = ucode + billInfo.Name,                         //NG0001pform0000000008
                    PForm = billInfo.Name,                                     //pform0000000008
                    EForm = billInfo.Name.Replace("pform", "EFORM"),           //EFORM0000000008
                    QForm = billInfo.Name.Replace("pform", "w_eform_p_list"),  //w_eform_p_list0000600008
                    TableMaster = billInfo.HeadInfo.TableName,
                    Title = billInfo.Description,
                    SqlList = billInfo.PbList.Sql,
                    SqlMaster = billInfo.HeadInfo.Sql
                };

                foreach (PbGridInfo grid in billInfo.PbGrids)
                {
                    var info = new TemplateDetailInfo();
                    info.Name = grid.Name;
                    info.Sql = grid.Sql;
                    info.TableName = grid.TableName;
                    info.Subtotal = grid.Subtotal;
                    info.Groupfield = grid.Groupfield;
                    templateInfo.DetailInfoList.Add(info);

                    //grids中生成代码转名称相关代码
                    foreach (var helpControl in grid.PbBaseTextInfos)
                    {
                        //物资不需要代码转名称，单独处理
                        if (helpControl.ColumnInfo.ColumnName == "res_code")
                        {
                            continue;
                        }

                        if (helpControl.ControlType.ToString() == "DataHelpEdit")
                        {
                            var helpEdit = (PbDataHelpEditInfo)helpControl;

                            var codeToNameInfo = new CodeToNameInfo
                            {
                                TableName = grid.TableName,
                                CodeName = helpEdit.ColumnInfo.ColumnName,
                                HelpId = helpEdit.DataHelpId,
                                MultiSelect = helpEdit.MultiSelect
                            };

                            templateInfo.CodeToNameGrid.Add(codeToNameInfo);
                        }
                    }
                }

                //List中生成代码转名称相关代码
                foreach (var helpControl in billInfo.PbList.PbBaseTextInfos)  //mod by ljy 2016.12.08 billInfo.HeadInfo.PbColumns 改成 billInfo.PbList.PbBaseTextInfos
                {
                    if (helpControl.ControlType.ToString() == "DataHelpEdit")
                    {
                        var helpEdit = (PbDataHelpEditInfo)helpControl;
                        var codeToNameInfo = new CodeToNameInfo
                        {
                            TableName = billInfo.PbList.TableName,
                            CodeName = helpEdit.ColumnInfo.ColumnName,
                            HelpId = helpEdit.DataHelpId,
                            MultiSelect = helpEdit.MultiSelect
                        };

                        templateInfo.CodeToNameList.Add(codeToNameInfo);
                    }
                }

                //图片控件
                foreach (PbPictureboxInfo picturebox in billInfo.HeadInfo.PbPictureboxInfos)
                {
                    templateInfo.BitmapNameList.Add(picturebox.Name);
                }

                if (billInfo.AsrGridInfo.Visible || billInfo.AsrGridInfo.PbBaseTextInfos.Count > 0)
                {
                    templateInfo.HasAsrGrid = "1";
                }

                var controllerTemplate = new ControllerTemplate(templateInfo);

                if (isGenerateCsFile)  //生成代码文件
                {
                    string path = csFilePath + templateInfo.ClassName;
                    controllerTemplate.WriteEx(ref path);
                    NG3.Compile.Compiler.CompileFromFile(path, assemblyPath, true, templateInfo.NameSpacePrefix + "." + templateInfo.ClassName + ".Controller");
                }
                else  //不生成代码文件
                {
                    string[] codeArray = new string[] { controllerTemplate.WriteEx() };
                    NG3.Compile.Compiler.CompileFromCode(codeArray, assemblyPath, templateInfo.NameSpacePrefix + "." + templateInfo.ClassName + ".Controller");
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }

        /// <summary>
        /// 生成app表单服务端dll;
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyPath"></param>
        /// <param name="isGenerateCsFile"></param>
        /// <param name="csFilePath"></param>
        /// <returns></returns>
        public static bool GenerateApp(PbBillInfo billInfo, string assemblyPath, bool isGenerateCsFile, string csFilePath, string ucode)
        {

            try
            {
                //参数设置
                var templateInfo = new TemplateInfo
                {
                    NameSpacePrefix = "SUP.CustomForm",
                    ClassName = ucode + billInfo.Name.Replace("pform", "aform"),  //NG0001pform0000000008
                    PForm = billInfo.Name.Replace("pform", "aform"),           //pform0000000008
                    EForm = billInfo.Name.Replace("pform", "EFORM"),           //EFORM0000000008
                    QForm = billInfo.Name.Replace("pform", "w_eform_p_list"),  //w_eform_p_list0000600008
                    TableMaster = billInfo.HeadInfo.TableName,
                    Title = billInfo.Description,
                    SqlList = billInfo.PbList.Sql,
                    SqlMaster = billInfo.HeadInfo.Sql
                };

                //List中生成代码转名称相关代码
                foreach (var helpControl in billInfo.PbList.PbBaseTextInfos)  //mod by ljy 2016.12.08 billInfo.HeadInfo.PbColumns 改成 billInfo.PbList.PbBaseTextInfos
                {
                    if (helpControl.ControlType.ToString() == "DataHelpEdit")
                    {
                        var helpEdit = (PbDataHelpEditInfo)helpControl;
                        var codeToNameInfo = new CodeToNameInfo
                        {
                            TableName = billInfo.PbList.TableName,
                            CodeName = helpEdit.ColumnInfo.ColumnName,
                            HelpId = helpEdit.DataHelpId,
                            MultiSelect = helpEdit.MultiSelect
                        };

                        templateInfo.CodeToNameList.Add(codeToNameInfo);
                    }
                }

                var controllerTemplate = new AppControllerTemplate(templateInfo);

                if (isGenerateCsFile)  //生成代码文件
                {
                    string path = csFilePath + templateInfo.ClassName;
                    controllerTemplate.WriteEx(ref path);
                    NG3.Compile.Compiler.CompileFromFile(path, assemblyPath, true, templateInfo.NameSpacePrefix + "." + templateInfo.ClassName + ".Controller");
                }
                else  //不生成代码文件
                {
                    string[] codeArray = new string[] { controllerTemplate.WriteEx() };
                    NG3.Compile.Compiler.CompileFromCode(codeArray, assemblyPath, templateInfo.NameSpacePrefix + "." + templateInfo.ClassName + ".Controller");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }
    }
}
