
using NG3.Data;
using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Supcan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Dac
{
    public class ConfigureDac:DacSupport
    {
        public IDictionary<string, IList<FuncInfo>> LoadFuncInfos(string catalogs)
        {
            string sql = @"SELECT r.code,r.funcname,r.USAGE,r.detail,r.example,r.catalogid,
                           c.catalogname,r.resulttype FROM rpt_func r, rpt_funccatalog c
                           WHERE r.catalogid = c.phid and catalogid in ({0}) ORDER BY r.catalogid";
            
            sql = string.Format(sql, catalogs);

            var datas = this.QueryForDataTable(sql);
            if (datas == null) throw new FuncException("无法从数据库加载公式配置信息");

            IDictionary<string, IList<FuncInfo>> funcinfos = new Dictionary<string, IList<FuncInfo>>();

            List <FuncInfo> funcInfoList = new List<FuncInfo>();
            string catalogid = string.Empty;
            foreach (DataRow dr in datas.Rows)
            {                
                if (string.IsNullOrEmpty(catalogid))
                {
                    catalogid = dr["catalogid"].TryGetString();
                }
                else
                {
                    if(catalogid!= dr["catalogid"].TryGetString())
                    {
                        //表示不同的目录处理,则直接加入到dictionary
                        funcinfos.Add(catalogid, funcInfoList);

                        funcInfoList = new List<FuncInfo>();
                        catalogid = dr["catalogid"].TryGetString();
                    }
                }

                FuncInfo func = new FuncInfo();
                func.Module = dr["catalogname"].TryGetString();
                func.Name = dr["funcname"].TryGetString();
                func.Usage = dr["usage"].TryGetString();
                func.Detail = dr["detail"].TryGetString();
                func.Example = dr["example"].TryGetString();
                func.ResultType = (EnumFuncDataType)dr["resulttype"].TryGetInt();
                funcInfoList.Add(func);
            }
            if(!string.IsNullOrEmpty(catalogid))
            {
                funcinfos.Add(catalogid, funcInfoList);
            }           
            return funcinfos;
        }


        public  IList<FuncRefInfo> LoadFuncInfoRefs(string catalogs)
        {
            string sql = "SELECT f.funcname,r.assemblyname,r.classname FROM rpt_func_ref r,rpt_func f WHERE r.funcphid = f.phid AND f.catalogid IN ({0}) order by f.catalogid";
            sql = string.Format(sql, catalogs);

            var datas = this.QueryForDataTable(sql);
            if (datas == null) throw new FuncException("无法从数据库加载公式反射信息");

            IList<FuncRefInfo> funcRefInfoList = new List<FuncRefInfo>();

            foreach (DataRow dr in datas.Rows)
            {
                FuncRefInfo refInfo = new FuncRefInfo();
                refInfo.FuncName = dr["funcname"].TryGetString();
                refInfo.AssemblyName = dr["assemblyname"].TryGetString();
                refInfo.ClassName = dr["classname"].TryGetString();

                funcRefInfoList.Add(refInfo);

            }
            return funcRefInfoList;
        }

        public IList<FuncParameter> LoadFuncParameter(string catalogs)
        {
            string sql = @"SELECT f.funcname,p.pindex,p.pname,p.displayname,p.datatype,p.droplistId,p.edittype,d.code 
                   FROM rpt_func_param p,rpt_func f,rpt_droplist d WHERE p.funcphid = f.phid 
                    AND d.phid = p.droplistid AND f.catalogid IN ({0}) ORDER BY f.catalogid,f.funcname,p.pindex ";

            sql = string.Format(sql, catalogs);
            var datas = this.QueryForDataTable(sql);
            if (datas == null) throw new FuncException("无法从数据库加载公式参数信息");

            IList<FuncParameter> funcParamList = new List<FuncParameter>();

            foreach (DataRow dr in datas.Rows)
            {
                FuncParameter paramInfo = new FuncParameter();

                paramInfo.FuncName = dr["funcname"].TryGetString();
                paramInfo.Name = dr["pname"].TryGetString();
                paramInfo.Index = dr["pindex"].TryGetInt();
                paramInfo.DisplayProperty = dr["displayname"].TryGetString();
                paramInfo.ParamType = dr["datatype"].TryGetString();
                paramInfo.DroplistCode = dr["code"].TryGetString(); //下拉名称
                paramInfo.EditType = dr["edittype"].TryGetString(); //下拉类型

                funcParamList.Add(paramInfo);

            }
            return funcParamList;
        }

        public IDictionary<string, IList<DropDownList>> LoadDropDownLists(string catalogs)
        {
            string sql = @"SELECT distinct d.code,d.displaycol,d.datacol,d.treelist,d.dataurl,d.params,f.catalogid 
                           FROM rpt_func_param p,rpt_func f, rpt_droplist d 
                           WHERE p.funcphid = f.phid AND p.droplistId = d.phid AND f.catalogid IN ({0}) ORDER BY f.catalogid";

            sql = string.Format(sql, catalogs);
            var datas = this.QueryForDataTable(sql);

            if (datas == null) throw new FuncException("无法从数据库加载公式下拉列表信息");

            IDictionary<string, IList<DropDownList>> droplistInfos = new Dictionary<string, IList<DropDownList>>();

            List<DropDownList> dropList = new List<DropDownList>();
            string catalogid = string.Empty;
            foreach (DataRow dr in datas.Rows)
            {
                if (string.IsNullOrEmpty(catalogid))
                {
                    catalogid = dr["catalogid"].TryGetString();
                }
                else
                {
                    if (catalogid != dr["catalogid"].TryGetString())
                    {
                        //表示不同的目录处理,则直接加入到dictionary
                        droplistInfos.Add(catalogid, dropList);

                        catalogid = dr["catalogid"].TryGetString();
                        dropList = new List<DropDownList>();
                    }
                }

                DropDownList drop = new DropDownList();

                drop.Id = dr["code"].TryGetString(); //id
                drop.DisplayCol = dr["displaycol"].TryGetString();
                drop.DataCol = dr["datacol"].TryGetString();
                drop.TreelistUrl = dr["treelist"].TryGetString();
                drop.DataUrl = dr["dataurl"].TryGetString();
                drop.Paras = dr["params"].TryGetString();

                dropList.Add(drop);
            }

            if (!string.IsNullOrEmpty(catalogid))
            {
                droplistInfos.Add(catalogid, dropList);
            }
            return droplistInfos;

            
        }
    }
    
}
