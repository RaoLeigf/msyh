using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.DataAccess;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

namespace SUP.Common.Rule
{
    public class PrintRules
    {
        #region 获取数据
        /// <summary>
        /// 返回套打列表数据
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="clientJson"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetLFormList(string PubConnectString, string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            return new PrintDac(PubConnectString).GetLFormList(clientJson, pageSize, PageIndex, ref totalRecord); ;
        }

        /// <summary>
        /// 返回套打帮助列表数据
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public DataTable GetFmtTemplateFromDb(string PubConnectString, string typeFile)
        {
            return new PrintDac(PubConnectString).GetFmtTemplateFromDb(typeFile);
        }

        /// <summary>
        /// 通过打印ID获取套打模板的路径
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public string GetTemplateById(string PubConnectString, string printId, string tmpDir)
        {
            DataTable dTb = new PrintDac(PubConnectString).GetTemplateById(printId);
            if (dTb.Rows.Count > 0)
            {
                byte[] bTemp = (byte[])dTb.Rows[0]["bfile"];
                string sStr = Convert.ToBase64String(bTemp, 0, bTemp.Length);
                sStr = sStr.Replace("*", "/");
                bTemp = Convert.FromBase64String(sStr);
                string fileName = dTb.Rows[0]["filename"].ToString();
                string sPath = Path.Combine(tmpDir, fileName);
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                FileStream fsw = new FileStream(sPath, FileMode.Create, FileAccess.Write);
                fsw.Write(bTemp, 0, bTemp.Length);
                fsw.Close();
                return fileName;
            }
            return "";
        }

        /// <summary>
        /// 通过打印ID获取套打模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public DataTable GetModuleByID(string PubConnectString, string printId, string tmpDir)
        {
            DataTable dTb = new PrintDac(PubConnectString).GetTemplateById(printId);
            if (dTb.Rows.Count > 0)
            {
                byte[] bTemp = (byte[])dTb.Rows[0]["bfile"];
                string sStr = Convert.ToBase64String(bTemp, 0, bTemp.Length);
                sStr = sStr.Replace("*", "/");
                bTemp = Convert.FromBase64String(sStr);
                string fileName = dTb.Rows[0]["filename"].ToString();
                string sPath = Path.Combine(tmpDir, fileName);
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                FileStream fsw = new FileStream(sPath, FileMode.Create, FileAccess.Write);
                fsw.Write(bTemp, 0, bTemp.Length);
                fsw.Close();
                dTb.Columns.Remove("bfile");
                return dTb;
            }
            return null;
        }
        #endregion

        #region 保存用户自定义模板
        /// <summary>
        /// 保存用户自定义模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <param name="typeFile"></param>
        /// <param name="mTitle"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public string SaveUserDefTemplate(string PubConnectString, ref string printId, string typeFile, string mTitle, string xmlDoc, string tmpDir)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlDoc);

            string fName = Guid.NewGuid().ToString() + ".xml"; //临时文件
            string XmlFileName = Path.Combine(tmpDir, fName);
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
            doc.Save(XmlFileName);
            FileStream fStream = File.OpenRead(XmlFileName);
            byte[] bTemp = new byte[fStream.Length];
            fStream.Read(bTemp, 0, bTemp.Length);
            fStream.Flush();
            fStream.Close();

            string moduleno = "";
            string newFileName = new PrintDac(PubConnectString).SaveUserDefTemplate(ref printId, typeFile, mTitle, bTemp, ref moduleno);
            if (!string.IsNullOrEmpty(newFileName))
            {
                fName = newFileName;
                string newXmlFileName = Path.Combine(tmpDir, moduleno, newFileName);
                if (File.Exists(newXmlFileName))
                {
                    File.Delete(newXmlFileName);
                }
                else if (!Directory.Exists(Path.Combine(tmpDir, moduleno)))
                {
                    Directory.CreateDirectory(Path.Combine(tmpDir, moduleno));
                }
                File.Move(XmlFileName, newXmlFileName);
            }
            return moduleno + "/" + fName;
        }
        #endregion

        #region 套打管理
        /// <summary>
        /// 导入模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="sysPath"></param>
        /// <param name="allMoudleType"></param>
        /// <param name="mType"></param>
        /// <param name="errTxt"></param>
        /// <returns></returns>
        public int ImportModule(string PubConnectString, string sysPath, string allMoudleType, string mType, ref string errTxt)
        {
            string[] mArr = allMoudleType.Split(',');
            string tmpPath = "", fileName = "";
            int numCount = 0;
            PrintDac prt = new PrintDac(PubConnectString);
            foreach (string s in mArr)
            {
                if (s.Length == 0) { continue; }
                tmpPath = Path.Combine(sysPath, s);
                DirectoryInfo df = new DirectoryInfo(tmpPath);
                if (df.Exists)
                {
                    FileInfo[] fis = df.GetFiles("*.xml");
                    foreach (FileInfo f in fis)
                    {
                        fileName = f.Name;
                        if (!prt.CheckFtmTemplate(fileName, mType)) { continue; }
                        FileStream fStream = File.OpenRead(f.FullName);
                        byte[] bTemp = new byte[fStream.Length];
                        fStream.Read(bTemp, 0, bTemp.Length);
                        fStream.Flush();
                        fStream.Close();
                        if (mType == "sys")
                        {
                            numCount += prt.ImportSysTemplate(fileName, bTemp);
                        }
                        else
                        {
                            numCount += prt.ImportUserTemplate(fileName, bTemp);
                        }
                    }
                }
            }
            return numCount;
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <returns></returns>
        public int DeleteModule(string PubConnectString, string printId)
        {
            return new PrintDac(PubConnectString).DeleteModule(printId);
        }

        /// <summary>
        /// 系统、用户模板更新(新增、修改)
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="tmpPath"></param>
        /// <param name="oType"></param>
        /// <param name="printId"></param>
        /// <param name="moduleNo"></param>
        /// <param name="typeFile"></param>
        /// <param name="billName"></param>
        /// <param name="fileName"></param>
        /// <param name="remarks"></param>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public string UpdateLformTemplate(string PubConnectString, string tmpPath, string oType, string printId, string moduleNo, string typeFile, string billName, string fileName, string remarks, string xmlDoc)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlDoc);

            string fName = Guid.NewGuid().ToString() + ".xml"; //临时文件
            string XmlFileName = Path.Combine(tmpPath, fName);
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
            }
            doc.Save(XmlFileName);
            FileStream fStream = File.OpenRead(XmlFileName);
            byte[] bTemp = new byte[fStream.Length];
            fStream.Read(bTemp, 0, bTemp.Length);
            fStream.Flush();
            fStream.Close();

            string oldXmlFileName = "";
            if (oType == "add")
            {
                printId = new PrintDac(PubConnectString).AddLformTemplate(moduleNo, typeFile, billName, ref fileName, remarks, bTemp);
            }
            else if (oType == "edit")
            {
                printId = new PrintDac(PubConnectString).EditLformTemplate(printId, billName, ref fileName, ref oldXmlFileName, remarks, bTemp);
            }
            else if (oType == "update")
            {
                printId = new PrintDac(PubConnectString).UpdateSysTemplate(printId, billName, ref fileName, ref oldXmlFileName, remarks, bTemp);
            }

            #region 更新物理用户模板文件
            if (oType == "add" || oType == "edit")
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    tmpPath = Path.Combine(tmpPath, moduleNo);
                    string newXmlFileName = Path.Combine(tmpPath, fileName);
                    oldXmlFileName = Path.Combine(tmpPath, oldXmlFileName);
                    if (!Directory.Exists(tmpPath))
                    {
                        Directory.CreateDirectory(tmpPath);
                    }
                    if (File.Exists(newXmlFileName))
                    {
                        File.Delete(newXmlFileName);
                    }
                    if (File.Exists(oldXmlFileName))
                    {
                        File.Delete(oldXmlFileName);
                    }
                    File.Move(XmlFileName, newXmlFileName);
                }
            }
            #endregion
            return printId;
        }
        #endregion

        #region 打印设置
        /// <summary>
        /// 保存打印设置信息
        /// </summary>
        /// <param name="UserConnectString"></param>
        /// <param name="ctype"></param>
        /// <param name="PrintPage"></param>
        public void SetPrintSetup(string UserConnectString, string ctype, string PrintPage)
        {
            new PrintDac(UserConnectString).SetPrintSetup(ctype, PrintPage);
        }

        /// <summary>
        /// 获取打印设置信息
        /// </summary>
        /// <param name="UserConnectString"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public string GetPrintSetup(string UserConnectString, string ctype)
        {
            return new PrintDac(UserConnectString).GetPrintSetup(ctype);
        }
        #endregion
    }
}
