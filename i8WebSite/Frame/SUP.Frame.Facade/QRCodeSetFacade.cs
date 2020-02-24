using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NG3.Data.Service;
using SUP.Frame.Rule;
using SUP.Frame.DataAccess;
using NG3;
using SUP.Frame.DataEntity;

namespace SUP.Frame.Facade
{
    public class QRCodeSetFacade : IQRCodeSetFacade
    {
        private QRCodeSetRule rule = new QRCodeSetRule();
        private QRCodeSetDac dac = new QRCodeSetDac();

        [DBControl]
        public DataTable GetList(string query, int pageSize, int pageIndex, ref int totalRecord)
        {
            return rule.GetList(query, pageSize, pageIndex, ref totalRecord);
        }
        [DBControl]
        public DataTable GetMaster(string id)
        {
            return rule.GetMaster(id);
        }
        [DBControl]
        public DataTable GetProduct(string id)
        {
            return rule.GetProduct(id);
        }
        [DBControl]
        public DataTable GetDetailField(string id)
        {
            return rule.GetDetailField(id);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public ResponseResult Save(string masterID, DataTable masterDt, DataTable detailDt, DataTable productDt)
        {
            ResponseResult result = new ResponseResult();
            int iret = dac.Save(masterID, masterDt, detailDt, productDt);

            if (iret > 0)
            {
                result.Status = ResponseStatus.Success;
            }
            else
            {
                result.Status = ResponseStatus.Error;
            }

            return result;
        }
        [DBControl]
        public Int64 GetMaxID(string tableName, out bool isFirstRow)
        {
            return dac.GetMaxID(tableName, out isFirstRow);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int Delete(string masterIdString)
        {
            return dac.Delete(masterIdString);
        }
        [DBControl]
        public DataTable GetQrStyle(string id)
        {
            return dac.GetQrStyle(id);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public ResponseResult SaveQrStyle(DataTable masterDt)
        {
            ResponseResult result = new ResponseResult();
            int iret = dac.SaveQrStyle(masterDt);
            if (iret > 0)
            {
                result.Status = ResponseStatus.Success;
            }
            else
            {
                result.Status = ResponseStatus.Error;
            }

            return result;
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveImgName(string imgname)
        {
            return dac.SaveImgName(imgname);
        }

        [DBControl]
        public string GetImgName()
        {
            return dac.GetImgName();
        }
        [DBControl]
        public DataTable GetGridByBusTree(string busphid)
        {
            return dac.GetGridByBusTree(busphid);
        }
        [DBControl]
        public DataTable getUrlByCode(string code)
        {
            return dac.getUrlByCode(code);
        }
        [DBControl]
        public string getPhidByCode(string code)
        {
            return dac.getPhidByCode(code);
        }
    }
}
