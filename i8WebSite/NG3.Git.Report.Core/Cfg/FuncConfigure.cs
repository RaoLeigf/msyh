using NG3.Report.Func.Core.Dac;
using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Supcan;
using NG3.Report.Func.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Cfg
{
    public static class FuncConfigure
    {

        /// <summary>
        /// 加载系统的公式信息
        /// </summary>
        /// <param name="catalogs">int类型以逗号分隔的字符串</param>
        public static void LoadFuncs(string catalogs)
        {
            if (string.IsNullOrEmpty(catalogs)) return;

            string[] catalog = catalogs.Split(new char[] { ',' });
            StringBuilder sb = new StringBuilder();
            foreach (var item in catalog)
            {
                string value = FuncCache.GetCatalog(item);
                if (string.IsNullOrEmpty(value))
                {
                    sb.Append(item);
                    sb.Append(",");
                }                    
            }
            if (sb.Length > 0)
            { 
                sb.Remove(sb.Length - 1, 1); //去掉最后一个逗号
                //缓存目录
                catalog = sb.ToString().Split(new char[] { ',' });
                Array.ForEach(catalog, s => { FuncCache.AddCatalog(s); });

                var funcInfos = LoadFuncInfos(sb.ToString());
                foreach (var key in funcInfos.Keys)
                {
                    FuncCache.AddFuncInfoMetaDataByCatalog(key,funcInfos[key]);
                    funcInfos[key].ToList().ForEach(p => { FuncCache.AddFuncInfoMetaData(p.Name, p); });
                }
                
                var funcInfoRefs = LoadFuncInfoRefs(sb.ToString());
                funcInfoRefs.ToList().ForEach(p => { FuncCache.AddFuncRefInfo(p.FuncName, p); });

                var droplists = LoadDropDownLists(sb.ToString()); 
                foreach (var key in droplists.Keys)
                {
                    FuncCache.AddDropDownList(key, droplists[key]);
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogs">模块过滤列表</param>
        /// <returns></returns>
        private static IDictionary<string,IList<FuncInfo>> LoadFuncInfos(string catalogs)
        {
            ConfigureDac dac = new ConfigureDac();
            var dic = dac.LoadFuncInfos(catalogs);
            var param = dac.LoadFuncParameter(catalogs);

            foreach (var key in dic.Keys)
            {
                var funcs = dic[key];
                funcs.Select(f =>
                {
                    f.Paras = param.Where(funcp => { return funcp.FuncName.Equals(f.Name, StringComparison.OrdinalIgnoreCase); }).ToList<FuncParameter>();
                    return f;
                }).ToList<FuncInfo>();
            }




            //dic.Select(p =>
            //{
            //    p.Value.Select(f =>
            //    {
            //        f.Paras = param.Where(funcp => { return funcp.FuncName.Equals(f.Name, StringComparison.OrdinalIgnoreCase); }).ToList<FuncParameter>();
            //        return f;
            //    }).ToList<FuncInfo>();
            //    return p;

            //});

            return dic;
        }

        /// <summary>
        /// 加载函数的反射信息
        /// </summary>
        /// <returns></returns>
        private static IList<FuncRefInfo> LoadFuncInfoRefs(string catalogs)
        {
            ConfigureDac dac = new ConfigureDac();
            var list = dac.LoadFuncInfoRefs(catalogs);
            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogs">模块过滤</param>
        /// <returns></returns>
        private static IDictionary<string, IList<DropDownList>> LoadDropDownLists(string catalogs)
        {
            ConfigureDac dac = new ConfigureDac();
            var dic = dac.LoadDropDownLists(catalogs);
            return dic;
        }

        



        //得到函数的插件配置信息
        public static FuncRefInfo GetFuncRefInfo(FuncInfo func)
        {
            if (func == null) return null;
            return FuncCache.GetFuncRefInfo(func.Name);
        }
        
        
         
    }
}
