using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.DataAccess;
using NG3;
using SUP.Common.DataEntity;

namespace SUP.Common.Facade
{
    public class CommonhelpSettingFacade : ICommonhelpSettingFacade
    {

        private CommonhelpSettingDac dac = new CommonhelpSettingDac();

        [DBControl]
        public DataTable GetList(string clientJsonQuery, int pageSize, int pageIndex, ref int totalRecord)
        {
            return dac.GetList(clientJsonQuery,pageSize,pageIndex,ref totalRecord);
        }

        [DBControl]
        public ResponseResult GetMaster(string id)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                result.Data =  dac.GetMaster(id);
            }
            catch (Exception)
            {                
                throw;
            }
            return result;                    
        }

        [DBControl]
        public DataTable GetSystemField(string masterId)
        {
            return dac.GetSystemField(masterId);

            //ResponseResult result = new ResponseResult();
            //try
            //{
            //    result.data = dac.GetSystemField(masterId);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //return result;      
        }

        [DBControl]
        public DataTable GetUserField(string masterId)
        {
            return dac.GetUserField(masterId);

            //ResponseResult result = new ResponseResult();
            //try
            //{
            //    result.data = dac.GetUserField(masterId);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //return result;    
        }

        [DBControl]
        public DataTable GetDetailTableField(string masterId)
        {
            return dac.GetDetailTableField(masterId);
        }

        [DBControl]
        public DataTable GetQueryProperty(string masterId)
        {
            return dac.GetQueryProperty(masterId);
        }

        [DBControl]
        public DataTable GetRichQuery(string masterId)
        {
            return dac.GetRichQuery(masterId);
        }

        [DBControl(ControlOption=DbControlOption.BeginTransaction)]
        public ResponseResult Save(string masterID, CommonHelpSettingEntity entity)
        {
            ResponseResult result = new ResponseResult();
            int iret = dac.Save(masterID, entity);

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

    }

    public interface ICommonhelpSettingFacade
    {
        DataTable GetList(string clientJsonQuery, int pageSize, int pageIndex, ref int totalRecord);

        ResponseResult GetMaster(string id);

        DataTable GetSystemField(string masterId);

        DataTable GetUserField(string masterId);

        DataTable GetDetailTableField(string masterId);
        DataTable GetQueryProperty(string masterId);

        DataTable GetRichQuery(string masterId);

        ResponseResult Save(string masterID, CommonHelpSettingEntity entity);
    }
}
