using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Supcan;
using NG3.Report.Func.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core
{
    public static class FuncManager
    {


        /// <summary>
        /// 开始计算任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool StartCalcTask(FuncContext context)
        {
            string taskId = FuncCalcTask.GetTaskId();
            return FuncCache.AddContext(taskId,context);            
        }

        /// <summary>
        /// 结束计算任务，清理
        /// </summary>
        /// <returns></returns>
        public static bool StopCalcTask()
        {
            string taskId = FuncCalcTask.GetTaskId();
            //清理缓存
            return FuncCache.Clear(taskId);
        }
        /// <summary>
        /// 取多个函数的值
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetFuncsValue(string xml)
        {
            //生成本次报表运算的GUID
            IList<FuncInfo> funcs = XmlHelper.GetFuncs(xml);

            IList<FuncCalcResult> results = new List<FuncCalcResult>();


            funcs.ToList().ForEach(p => {
               var result =  FuncFactory.GetFuncValue(p);                
                if(result.Status != EnumFuncActionStatus.Failure)
                {
                    //设置返回值的数据类型
                    result.ResultDataType = FuncCache.GetFuncInfoMetaData(p.Name).ResultType;
                }                
                results.Add(result);
            });            
            //返回结果集
            return XmlHelper.GetFuncResult(results);

        }

        /// <summary>
        /// 单一函数获取结果集
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static FuncCalcResult GetFuncValue(FuncInfo func)
        {
            return FuncFactory.GetFuncValue(func);
        }

        /// <summary>
        /// 函数结果分解
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static IList<FuncResolveResult> GetFuncsReslove(string xml)
        {
            //生成本次报表运算的GUID
            IList<FuncInfo> funcs = XmlHelper.GetFuncs(xml);

            IList<FuncResolveResult> results = new List<FuncResolveResult>();

            funcs.ToList().ForEach(p => {
                var result =  FuncFactory.GetFuncReslove(p);
                results.Add(result);
            });

            return results;
            
        }

        /// <summary>
        /// 函数追踪
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static IList<FuncTrackResult> GetFuncsTrack(string xml)
        {
            //生成本次报表运算的GUID
            IList<FuncInfo> funcs = XmlHelper.GetFuncs(xml);

            IList<FuncTrackResult> results = new List<FuncTrackResult>();

            funcs.ToList().ForEach(p => {
                var result = FuncFactory.GetFuncTrack(p);
                results.Add(result);
            });
            return results;

            
        }

        /// <summary>
        /// 获取系统支持的函数,
        /// supcan报表控件要求的XML说明
        /// </summary>
        /// <returns></returns>
        public static string GetSupportFuncs(string catalogs)
        {
            return XmlHelper.SupcanFuncsXmlDescriptor(catalogs);
        }


        
        
    }
}
