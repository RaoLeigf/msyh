using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NG3;
using SUP.Frame.DataAccess;
using SUP.Frame.Facade.Interface;
using SUP.Frame.Rule;
using SUP.Common.Base;

namespace SUP.Frame.Facade
{
    public class ExcelImportFacade : IExcelImportFacade
    {
        ExcelImportDac dac = new ExcelImportDac();
        ExcelImportRule rule = new ExcelImportRule();

        [DBControl]
        public DataTable GetTemplate(string id)
        {
            return dac.GetTemplate(id);
        }

        [DBControl]
        public DataTable GetFormData(string id)
        {
            return dac.GetFormData(id);
        }

        [DBControl]
        public IList<TreeJSONBase> LoadMenu(string nodeid,string id)
        {
            return dac.LoadMenu(nodeid,id);
        }

        [DBControl]
        public MemoryStream ExportTemplate(DataTable dt, string multipleSheet)
        {
            try
            {
                return rule.ExportTemplate(dt, multipleSheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [DBControl]
        public List<string> GetTableName()
        {
            return rule.GetTableName();
        }

        [DBControl]
        public void BindFile(string templateId, string filename, string filepath)
        {
            rule.BindFile(templateId, filename, filepath);
        }

        [DBControl]
        public DataTable GetExcelDataAndColInfo(string tableName, out string jsonStr)
        {
            //Dictionary<string, List<KeyValuePair<string, string>>> fieldDic =
            //    new Dictionary<string, List<KeyValuePair<string, string>>>();
            //Dictionary<string, List<bool>> mustInputDic = new Dictionary<string, List<bool>>();
            //Dictionary<string, List<string>> helpIdDic = new Dictionary<string, List<string>>();
            //Dictionary<string, List<string>> columnTypeDic = new Dictionary<string, List<string>>();

            DataSet ds;
            try
            {
                ds = rule.GetExcelDataAndColInfo(tableName,out jsonStr);
                //jsonStr = rule.GetFieldNames(tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return ds.Tables[tableName];
        }

        [DBControl]
        public bool Save(DataSet ds,string selected,ref string message)
        {
             return rule.Save(ds,ref message,selected);
        }

        public string GetSelectSql(string json, string tname)
        {
            return rule.GetSelectSql(json, tname);
        }
    }
}
