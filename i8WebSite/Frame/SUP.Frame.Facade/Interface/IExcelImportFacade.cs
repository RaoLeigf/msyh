using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUP.Common.Base;

namespace SUP.Frame.Facade.Interface
{
    public interface IExcelImportFacade
    {
        IList<TreeJSONBase> LoadMenu(string nodeid,string id); 
        DataTable GetTemplate(string id);
        DataTable GetFormData(string id);
        MemoryStream ExportTemplate(DataTable dt, string multipleSheet);
        List<string> GetTableName();
        void BindFile(string templateId, string filename, string filepath);
        DataTable GetExcelDataAndColInfo(string tableName, out string jsonStr);
        bool Save(DataSet ds, string selected, ref string message);

        string GetSelectSql(string json, string tname);
    }
}
