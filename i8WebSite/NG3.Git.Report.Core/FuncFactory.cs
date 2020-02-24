using NG3.Report.Func.Core.Cfg;
using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Interface;
using NG3.Report.Func.Core.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core
{
    public static class FuncFactory
    {


        //报表格式有可能会产生变化
        internal static FuncCalcResult GetFuncValue(FuncInfo func)
        {
            if (func == null) throw new FuncException("要进行计算的函数为空");

            FuncCalcResult calcResult = null;

            //本次计算KEY
            string taskId = FuncCalcTask.GetTaskId();
            if (string.IsNullOrEmpty(taskId)) throw new FuncException("函数计算任务的taskId不能为空");

            var funcInfoMateData = FuncCache.GetFuncInfoMetaData(func.Name);
            if(funcInfoMateData ==null)
            {
                //计算结果
                calcResult = new FuncCalcResult { Status = EnumFuncActionStatus.Failure };
                calcResult.Fault = FaultBuilder.Fault("错误", func.Name + "没有注册函数信息"); 
                return calcResult;
            }


            //没有缓存则计算
            var refInfo = FuncConfigure.GetFuncRefInfo(func);
            if (refInfo == null)
            {
                //计算结果
                calcResult = new FuncCalcResult { Status = EnumFuncActionStatus.Failure };
                calcResult.Fault = FaultBuilder.Fault("错误", func.Name + "没有注册函数实现类信息");               
                return calcResult;
            }

            try
            {
                //反射调用
                var assembly = Assembly.Load(refInfo.AssemblyName);
                var clazz = assembly.GetType(refInfo.ClassName);

                //使用新封装的接口，所有的参数都是通过request取
                var calcObject = Activator.CreateInstance(clazz) as IFunction;
                if (calcObject == null)
                {
                    //计算结果
                    calcResult = new FuncCalcResult { Status = EnumFuncActionStatus.Failure };

                    calcResult.Fault = FaultBuilder.Fault("错误", func.Name + "没有实现接口IFuncCalc");                    
                    return calcResult;
                }

                if (calcObject != null)
                {
                    calcObject.Func = func;
                    var funcInfo = calcObject.PreHandle(); //返回预处理结果,主要是参数的处理

                    var cache = FuncCache.GetFunc(taskId, funcInfo);
                    if (cache != null)
                    {
                        //已经有值,表示已经缓存，则直接取数
                        return cache.CalcResult;

                    }
                    else
                    {
                        var funcCalc = calcObject as IFuncCalc;
                        var result = funcCalc.GetValue();

                        //同时加入到缓存中,对象是否要clone
                        funcInfo.CalcResult = result;
                        FuncCache.AddFunc(taskId, funcInfo);
                        return result;

                    }

                }

            }
            catch (Exception ex)
            {

                //计算结果
                calcResult = new FuncCalcResult { Status = EnumFuncActionStatus.Failure };
                calcResult.Fault = FaultBuilder.Fault("错误", func.Name + "反射调用异常");                
            }

            return calcResult;

        }


        /// <summary>
        ///函数分解 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        internal static FuncResolveResult GetFuncReslove(FuncInfo func)
        {
            if (func == null) return null;

            //函数分解不进行缓存
            var refInfo = FuncConfigure.GetFuncRefInfo(func);
            if (refInfo == null)
            {
                //计算结果
                FuncResolveResult resolveResult = new FuncResolveResult();
                resolveResult.Fault = FaultBuilder.Fault("错误", func.Name + "没有注册实现类信息");                
                return resolveResult;
            }

            try
            {
                //反射调用
                var assembly = Assembly.Load(refInfo.AssemblyName);
                var clazz = assembly.GetType(refInfo.ClassName);

                //使用新封装的接口，所有的参数都是通过request取
                var resolveObject = Activator.CreateInstance(clazz) as IFuncResolve;
                if (resolveObject == null)
                {
                    //计算结果
                    FuncResolveResult resolveResult = new FuncResolveResult();
                    resolveResult.Fault = FaultBuilder.Fault("错误", func.Name + "没有实现接口IFuncResolve");                   
                    return resolveResult;
                }
                else
                {
                    var result = resolveObject.Resolve();
                    return result;
                }
            }
            catch (Exception ex)
            {

                FuncResolveResult resolveResult = new FuncResolveResult();
                resolveResult.Fault = FaultBuilder.Fault("错误", func.Name + "反射调用异常"+ex.Message);                
                return resolveResult;
            }
        }

        //函数追踪
        internal static FuncTrackResult GetFuncTrack(FuncInfo func)
        {
            if (func == null) return null;

            //函数分解不进行缓存
            var refInfo = FuncConfigure.GetFuncRefInfo(func);
            if (refInfo == null)
            {
                //计算结果
                FuncTrackResult trackResult = new FuncTrackResult();
                trackResult.Fault = FaultBuilder.Fault("错误", func.Name + "没有注册实现类信息");                
                return trackResult;
            }

            try
            {
                //反射调用
                var assembly = Assembly.Load(refInfo.AssemblyName);
                var clazz = assembly.GetType(refInfo.ClassName);

                //使用新封装的接口，所有的参数都是通过request取
                var trackObject = Activator.CreateInstance(clazz) as IFuncTrack;
                if (trackObject == null)
                {
                    //计算结果
                    FuncTrackResult trackResult = new FuncTrackResult();
                    trackResult.Fault = FaultBuilder.Fault("错误", func.Name + "没有实现接口IFuncTrack");                  
                    return trackResult;
                }
                else
                {
                    var result = trackObject.Track();
                    return result;
                }
            }
            catch (Exception ex)
            {

               
                FuncTrackResult trackResult = new FuncTrackResult();
                trackResult.Fault = FaultBuilder.Fault("错误", func.Name + "反射调用异常"+ex.Message);                
                return trackResult;
            }
        }
            
    }
}
