using NG3.Data;
using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonUtils
    {
        /// <summary>
        /// 两个实体之间相同属性的映射
        /// </summary>
        /// <typeparam name="R">R代表目标实体</typeparam>
        /// <typeparam name="T">T代表数据源实体</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static R Mapping<R, T>(T model)
        {
            R result = Activator.CreateInstance<R>();
            foreach (PropertyInfo info in typeof(R).GetProperties())
            {
                if (info.Name != "ListNotEvaluateProerty")
                {
                    PropertyInfo pro = typeof(T).GetProperty(info.Name);
                    if (pro != null)
                        info.SetValue(result, pro.GetValue(model));
                }
            }
            return result;
        }
        /// <summary>
        /// 复制对象 
        /// </summary>
        /// <typeparam name="TIn">传入对象</typeparam>
        /// <typeparam name="TOut">输出对象</typeparam>
        public static TOut TransReflection<TIn, TOut>(TIn tIn)
        {
            if (tIn == null) {
                throw new Exception("输入对象不能为空!");
            }

            TOut tOut = Activator.CreateInstance<TOut>();
            var tInType = tIn.GetType();
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                if (itemOut.Name != "ListNotEvaluateProerty")
                {
                    var itemIn = tInType.GetProperty(itemOut.Name); ;
                    if (itemIn != null)
                    {
                        itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                    }
                }
            }
            return tOut;
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string GetFileSize(long size)
        {
            var num = 1024.00; //byte

            if (size < num)
                return size + "B";
            if (size < Math.Pow(num, 2))
                return (size / num).ToString("f2") + "K"; //kb
            if (size < Math.Pow(num, 3))
                return (size / Math.Pow(num, 2)).ToString("f2") + "M"; //M
            if (size < Math.Pow(num, 4))
                return (size / Math.Pow(num, 3)).ToString("f2") + "G"; //G

            return (size / Math.Pow(num, 4)).ToString("f2") + "T"; //T
        }

        public static bool IsOracleDB()
        {
            string callStr = ConnectionInfoService.GetCallContextConnectString();
            if (string.IsNullOrEmpty(callStr) || callStr == DbHelper.ConnectString)
            {
                DbHelper.Open();
            }
            else
            {
                DbHelper.Open(callStr);
            }

            var res = DbHelper.Vendor == DbVendor.Oracle || DbHelper.Vendor == DbVendor.Oracle11
                          || DbHelper.Vendor == DbVendor.Oracle10 || DbHelper.Vendor == DbVendor.Oracle9;
           return res;
        }
    }
}
