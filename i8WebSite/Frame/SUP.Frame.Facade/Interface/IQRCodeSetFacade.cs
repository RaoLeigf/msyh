using System.Data;
using SUP.Frame.DataEntity;
namespace SUP.Frame.Facade
{
    public interface IQRCodeSetFacade
    {
        DataTable GetList(string query, int pageSize, int pageIndex, ref int totalRecord);

        DataTable GetMaster(string id);
        DataTable GetProduct(string id);
        DataTable GetDetailField(string id);

        ResponseResult Save(string masterID, DataTable masterDt, DataTable detailDt, DataTable productDt);

        long GetMaxID(string tableName, out bool isFirstRow);

        int Delete(string masterIdString);
        DataTable GetQrStyle(string id);
        ResponseResult SaveQrStyle(DataTable masterDt);
        int SaveImgName(string imgname);
        string GetImgName();
        DataTable GetGridByBusTree(string busphid);
        DataTable getUrlByCode(string code);
        string getPhidByCode(string code);
    }
}
