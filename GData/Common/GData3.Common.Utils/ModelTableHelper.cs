using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    public static class ModelTableHelper
    {
        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item);
                table.Rows.Add(row);
            }
            return table;
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;
            if (rows != null)
            {
                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }
            return list;
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
                return null;

            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
                rows.Add(row);

            return ConvertTo<T>(rows);
        }

        //Convert DataRow into T Object
        public static T CreateItem<T>(DataRow row)
        {
            string columnName;
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    columnName = column.ColumnName.ToLower();
                    //Get property with same columnName
                    PropertyInfo prop = obj.GetType().GetProperty(columnName);
                    object value;
                    try
                    {
                        //Get value for the column
                        value = (row[columnName].GetType() == typeof(DBNull)) ? null : row[columnName];
                        //Set property value
                        if (prop.CanWrite)    //判断其是否可写
                            prop.SetValue(obj, value, null);
                    }
                    catch (ArgumentException are)
                    {
                        //通过反射调用parse方法，理论上可以规避所有数据库与Model对象字段属性不一致问题;
                        value = (row[columnName].GetType() == typeof(DBNull)) ? null : row[columnName];
                        MethodInfo mf = Type.GetType(prop.PropertyType.FullName).GetMethod("Parse", new Type[] { typeof(string) });
                        int ntv = (int)mf.Invoke(null, new object[] { value.ToString() });
                        if (prop.CanWrite)    //判断其是否可写
                            prop.SetValue(obj, ntv, null);
                        continue;
                        //Catch whatever here
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return obj;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, prop.PropertyType);

            return table;
        }
        /// <summary>
        /// 获取 DataTable 列名
        /// </summary>
        /// <param name="dt">对象</param>
        /// <returns></returns>
        public static string[] GetColumnsByDataTable(DataTable dt)
        {
            string[] strColumns = null;

            if (dt != null && dt.Columns.Count > 0)
            {
                int columnNum = 0;
                columnNum = dt.Columns.Count;
                strColumns = new string[columnNum];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strColumns[i] = dt.Columns[i].ColumnName;
                }
            }

            return strColumns;

        }



    }

    public class ModelHelper<T> where T : new()  // 此处一定要加上new()
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
