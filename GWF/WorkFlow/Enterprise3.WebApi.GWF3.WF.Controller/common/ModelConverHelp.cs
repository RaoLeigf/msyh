using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GWF3.WF.Controller.Common
{
    /// <summary>
    /// DataTable转List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelConverHelp<T> where T : new()
    {
        public static IList<T> DataTableToModel(DataTable dt)
        {

            IList<T> list = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pro in propertys)
                {
                    tempName = pro.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pro.CanWrite) continue;

                        //object value = dr[tempName];
                        object value = (dr[tempName].GetType() == typeof(DBNull)) ? null : dr[tempName];

                        //获取数据的类型
                        var propertyType = pro.PropertyType;
                        //转换数据的类型
                        object v = ChangeType(value, propertyType);

                        if (value != DBNull.Value)
                            pro.SetValue(t, v);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        private static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }

            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }
}
