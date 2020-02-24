using System;
using System.Data;
using System.Text;
using System.IO;
using System.Web;

using NG3;
using NG3.Data.Service;
using NG3.Metadata.UI.PowserBuilder;
using SUP.CustomForm.DataEntity;
using SUP.CustomForm.Rule;

namespace SUP.CustomForm.Builder
{
    public class Build
    {
        /// <summary>
        /// 生成web自定义表单;
        /// </summary>
        /// <param name="buildPara"></param>
        /// <returns></returns>
        public string BuildCustomForm(BuildParameter buildPara)
        {
            string winType = "Edit";
            string ucode = string.Empty;

            if (string.IsNullOrEmpty(buildPara.Id))
            {
                throw new Exception("Id不能为空！");
            }

            var fileName = BuildFile(buildPara.Id, ref ucode); //得到ini文件内容和帐套号
            var extJsStr = BuildExtJs(buildPara.Id);

            //p_form_m表中ucode可能不对，比如帐套是其他地方还原过来的，这里取默认帐套号
            if (!string.IsNullOrEmpty(ucode))
            {
                ucode = AppInfoBase.DbName;  //如NG0002
            }

            var type = buildPara.Type;  //web：网页自定义表单，app：移动自定义表单
            if (string.IsNullOrEmpty(type))
            {
                type = "web";
            }

            buildPara.AssemblyPath = AppDomain.CurrentDomain.BaseDirectory + "bin\\";
            buildPara.CsFilePath = AppDomain.CurrentDomain.BaseDirectory + "CustomFormTemp\\";

            //获取设计器生成ini文件内容
            PbBillInfo billInfo = new PbBillInfo();
            billInfo = CommonParser.GetBillBase(fileName);

            // 生成前端代码;
            var clientGen = new ClientGen.Generator();
            if (type == "app")
            {
                if (!ClientGen.Generator.GenerateApp(billInfo, ref winType, ucode)) throw new Exception("前端代码生成失败！");
            }
            else
            {
                if (!ClientGen.Generator.Generate(billInfo, ref winType, extJsStr, ucode)) throw new Exception("前端代码生成失败！");
            }


            // 生成服务端代码;
            bool isGenerateCsFile = true;  //是否生成cs文件
            if (string.IsNullOrEmpty(buildPara.CsFilePath))
            {
                isGenerateCsFile = false;
            }

            if (type == "app")
            {
                if (!ServerGen.Generator.GenerateApp(billInfo, buildPara.AssemblyPath, isGenerateCsFile, buildPara.CsFilePath, ucode)) throw new Exception("服务端代码生成失败！");
            }
            else
            {
                if (!ServerGen.Generator.Generate(billInfo, buildPara.AssemblyPath, isGenerateCsFile, buildPara.CsFilePath, ucode)) throw new Exception("服务端代码生成失败！");
            }


            if (!ServerGen.Generator.Generate(billInfo, buildPara.AssemblyPath, isGenerateCsFile, buildPara.CsFilePath, ucode)) throw new Exception("服务端代码生成失败！");



            //begin 增加一个菜单        
            string configPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\NG3Config\\MainNavigation.xml";
            string menuName = ucode + (type == "app" ? "aform" : "pform") + buildPara.Id + winType;
            string menuText = menuName + " - " + billInfo.Description;
            string menuTabTitle = billInfo.Description;

            DataSet ds = new DataSet();
            ds.ReadXml(configPath);

            DataRow[] drs = ds.Tables["TreeNode"].Select("Text='" + menuText + "'");

            if (drs == null || drs.Length < 1)
            {
                DataRow dr = ds.Tables["TreeNode"].NewRow();
                dr["NavigateUrl"] = "/Custom/SUP/" + menuName;
                dr["Text"] = menuText;
                dr["TabTitle"] = menuTabTitle;
                dr["UserType"] = "system,developer,translator";
                dr["TreeNodeGroup_Id"] = "0";
                ds.Tables["TreeNode"].Rows.Add(dr);
                ds.WriteXml(configPath);
            }
            //end 增加一个菜单


            //// 返回URL链接;
            //var ip = string.IsNullOrEmpty(buildPara.Port) ? buildPara.Host : string.Format("{0}:{1}", buildPara.Host, buildPara.Port);
            //return string.Format(@"http://{0}/SUP/pform{1}List", ip, buildPara.Id);

            //返回生成业务点;
            return string.Format(@"自定义表单pform{0}生成成功", buildPara.Id);
        }

        /// <summary>
        /// 根据id生成ini文件; 
        /// </summary>
        private static string BuildFile(string fileId, ref string ucode)
        {
            var s = string.Empty;

            // 根据传入的id值取出ini文件的内容;
            DbHelper.Open();
            var dt = new DataTable();
            try
            {
                dt = DbHelper.GetDataTable(string.Format("select * from p_form_m where code='{0}'", fileId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }
            if (dt.Rows.Count <= 0) throw new Exception("id号无对应数据;");

            ucode = dt.Rows[0]["ucode"].ToString();

            var bformBytes = (Byte[]) dt.Rows[0]["bform"];

            /**********************************************
             * "LZW15"开头的流数据是经过压缩的,需解压后再转码;
             * 非"LZW15"开头的数据直接转码即可;
             **********************************************/
            var headBytes = new byte[5];
            Array.Copy(bformBytes, 0, headBytes, 0, 5);
            var headStr = Encoding.GetEncoding("gb2312").GetString(headBytes);
            if (headStr == "LZW15")
            {
                var sourceBytes = new byte[bformBytes.Length - 5];
                Array.Copy(bformBytes, 5, sourceBytes, 0, bformBytes.Length - 5);
                var deCompressByte = new LzwZip(Encoding.GetEncoding("gb2312")).Decompress(sourceBytes);
                s = Encoding.Default.GetString(deCompressByte);
            }
            else
            {
                s = Encoding.Default.GetString(bformBytes);
            }

            var path = AppDomain.CurrentDomain.BaseDirectory + "pform\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // 在ini文件中加入pform信息'id'和'表描述';
            var pformInfo = new StringBuilder();
            pformInfo.Append("[pformInfo]\r\n");
            pformInfo.Append("fileid=" + fileId + "\r\n");
            pformInfo.Append("description=" + dt.Rows[0]["title"].ToString() + "\r\n");

            // 将bform数据写入ini文件,路径为主程序下pform文件夹中;
            var filename = path + fileId + ".ini";

            using (var sw = new StreamWriter(filename, false, Encoding.GetEncoding("gb2312")))
            {
                sw.Write(pformInfo.ToString() + s);
            }

            return filename;
        }

        /// <summary>
        /// 根据id生成扩展js文件字符串;
        /// </summary>
        private static string BuildExtJs(string fileId)
        {
            var s = string.Empty;

            DbHelper.Open();
            var dt = new DataTable();
            try
            {
                dt = DbHelper.GetDataTable(string.Format("select jsplugin from p_form_m where code='{0}'", fileId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }

            //id号无对应扩展js内容
            if (dt.Rows.Count <= 0 || string.IsNullOrEmpty(dt.Rows[0]["jsplugin"].ToString()) )
            {
                return string.Empty;
            }

            var bformBytes = (Byte[])dt.Rows[0]["jsplugin"];

            //id号对应扩展js内容为空
            if (bformBytes.Length < 1)
            {
                return string.Empty;
            }

            /**********************************************
             * "LZW15"开头的流数据是经过压缩的,需解压后再转码;
             * 非"LZW15"开头的数据直接转码即可;
             **********************************************/
            var headBytes = new byte[5];
            Array.Copy(bformBytes, 0, headBytes, 0, 5);
            var headStr = Encoding.GetEncoding("gb2312").GetString(headBytes);
            if (headStr == "LZW15")
            {
                var sourceBytes = new byte[bformBytes.Length - 5];
                Array.Copy(bformBytes, 5, sourceBytes, 0, bformBytes.Length - 5);
                var deCompressByte = new LzwZip(Encoding.GetEncoding("gb2312")).Decompress(sourceBytes);
                s = Encoding.Default.GetString(deCompressByte);
            }
            else
            {
                s = Encoding.Default.GetString(bformBytes);
            }

            return s;
        }
    }
}
