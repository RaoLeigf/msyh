using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataAccess;
using System.Data;
using NG3;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using SUP.Common.DataEntity.Individual;

namespace SUP.Common.Facade
{
    public class IndividualPropertyFacade : IIndividualPropertyFacade
    {
        private IndividualPropertyDac dac;
        private SUP.Common.Rule.IndividualPropertyRule rule;

        public IndividualPropertyFacade()
        {
            dac = new IndividualPropertyDac();
            rule = new Rule.IndividualPropertyRule();
        }


        /// <summary>
        /// 获取表注册列表
        /// </summary>
        /// <param name="clientJson"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        [DBControl]
        public DataTable GetTableRegList(string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            return dac.GetTableRegList(clientJson, pageSize, PageIndex, ref totalRecord);
        }

        /// <summary>
        /// 获得表注册信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DBControl]
        public DataTable GetTableRegInfo(string tablename, Int64 busPhid)
        {
            return dac.GetTableRegInfo(tablename,busPhid);
        }

         /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="tableregcode"></param>
        /// <returns></returns>
        [DBControl]
        public DataTable GetColumnsInfo(string tablename)
        {
            return dac.GetColumnsInfo(tablename);
        }

         /// <summary>
        /// 保存字段注册信息
        /// </summary>
        /// <param name="columnregdt"></param>
        /// <returns></returns>
        [DBControl(ControlOption=DbControlOption.BeginTransaction)]
        public ResponseResult Save(DataTable columnregdt)
        {            
            ResponseResult result = new ResponseResult();

            int iret = new SUP.Common.Rule.IndividualPropertyRule().Save(columnregdt);

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
        public IList<TreeJSONBase> GetIndividualFieldTree(string bustype)
        {
            return new SUP.Common.Rule.IndividualPropertyRule().GetIndividualFieldTree(bustype);
        }

       
        [DBControl]
        public DataTable GetColumnInfo(string tname)
        {
            return dac.GetColumnInfo(tname);
        }

        [DBControl]
        public DataTable GetBusTypeList(string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            return dac.GetBusTypeList(clientJson, pageSize, PageIndex, ref totalRecord);
        }

        [DBControl]
        public DataTable GetPropertyUIInfo(string tablename, string bustype)
        {
            return dac.GetPropertyUIInfo(tablename,bustype);
        }

         /// <summary>
        /// 保存自定义字段ui信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [DBControl]
        public ResponseResult SavePropertyUIInfo(DataTable dt)
        {
            ResponseResult result = new ResponseResult();

            int iret = rule.SavePropertyUIInfo(dt);

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
        public IList<TreeJSONBase> LoadBusTree(string nodeid,string tablename)
        {
            return dac.LoadBusTree(nodeid,tablename);
        }

        [DBControl]
        public DataTable GetBusTables(string busID)
        {
            return dac.GetBusTables(busID);
        }

        [DBControl]
        public  ResponseResult GetInUseFiedlUIInfo(string fieldUIId)
        {
            ResponseResult result = new ResponseResult();

           List<string> ls= dac.GetInUseFiedlUIInfo(fieldUIId);

            if (ls.Count > 0)
            {
                result.Status = ResponseStatus.Success;
                result.Data = string.Join(",", ls.ToArray());
            }
            else
            {
                result.Status = ResponseStatus.Error;
            }
            return result;
        }
    }
}
