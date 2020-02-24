using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using SUP.Common.DataEntity.Individual;

namespace SUP.Common.Facade
{
    public interface IIndividualPropertyFacade
    {
        DataTable GetTableRegList(string clientJson, int pageSize, int PageIndex, ref int totalRecord);

        DataTable GetTableRegInfo(string tablename,Int64 busPhid);

        DataTable GetColumnsInfo(string tablename);

        ResponseResult Save(DataTable columnregdt);

        IList<TreeJSONBase> GetIndividualFieldTree(string bustype);

      

        DataTable GetColumnInfo(string tname);

        DataTable GetBusTypeList(string clientJson, int pageSize, int PageIndex, ref int totalRecord);

        DataTable GetPropertyUIInfo(string tablename, string bustype);

        ResponseResult SavePropertyUIInfo(DataTable dt);

        IList<TreeJSONBase> LoadBusTree(string nodeid, string tablename);

        DataTable GetBusTables(string busID);

        ResponseResult GetInUseFiedlUIInfo(string fieldUIId);
    }
}
