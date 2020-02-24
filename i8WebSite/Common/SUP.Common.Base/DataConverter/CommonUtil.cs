using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;
using NG3.Base;
using NG3.Data.Service;

namespace SUP.Common.Base
{
    public static class CommonUtil
    {


        //返回条件
        public static string GetWhere(JObject jo, string root)
        {
            string key = jo[root].Attr("key");

            JToken modify = jo[root]["modifiedRow"];
            JToken deleteRow = jo[root]["deletedRow"];
            JToken unchangedRow = jo[root]["unChangedRow"];

            string[] arr = key.Split(',');
            if (arr.Length > 1)//多个主键
            {

                StringBuilder filter = new StringBuilder();

                for (int i = 0; i < arr.Length; i++)
                {
                    List<string> list = new List<string>();

                    if (modify != null)
                    {
                        AddValue(modify, list, i);
                    }
                    if (deleteRow != null)
                    {
                        AddValue(deleteRow, list, i);
                    }
                    if (unchangedRow != null)
                    {
                        AddValue(modify, list, i);
                    }

                    StringBuilder strb = BuildInXpress(list, arr[i]);

                    if (strb.Length > 0)
                    {
                        //filter.Append(arr[i] + " in " + strb.ToString());
                        filter.Append(strb.ToString());
                        if (i < arr.Length - 1)
                        {
                            filter.Append(" and ");
                        }
                    }
                }

                return filter.ToString();
            }
            {//单主键          

                string where = string.Empty;
                List<string> list = new List<string>();

                if (modify != null)
                {
                    AddValue(modify, list);
                }
                if (deleteRow != null)
                {
                    AddValue(deleteRow, list);
                }
                if (unchangedRow != null)
                {
                    AddValue(unchangedRow, list);
                }

                StringBuilder strb = BuildInXpress(list,key);

                if (strb.Length > 0)
                {
                    //where = key + " in " + strb.ToString();
                    where = strb.ToString();
                }

                return where;
            }
        }

        //返回修改行条件
        public static string GetModifyWhere(JObject jo, string root)
        {
            string key = jo[root].Attr("key");

            JToken modify = jo[root]["modifiedRow"];           

            string[] arr = key.Split(',');
            if (arr.Length > 1)//多个主键
            {

                StringBuilder filter = new StringBuilder();

                for (int i = 0; i < arr.Length; i++)
                {
                    List<string> list = new List<string>();

                    if (modify != null)
                    {
                        AddValue(modify, list, i);
                    }                 

                    StringBuilder strb = BuildInXpress(list, arr[i]);

                    if (strb.Length > 0)
                    {
                        //filter.Append(arr[i] + " in " + strb.ToString());
                        filter.Append(strb.ToString());
                        if (i < arr.Length - 1)
                        {
                            filter.Append(" and ");
                        }
                    }
                }

                return filter.ToString();
            }
            {//单主键          

                string where = string.Empty;
                List<string> list = new List<string>();

                if (modify != null)
                {
                    AddValue(modify, list);
                }              

                StringBuilder strb = BuildInXpress(list,key);

                if (strb.Length > 0)
                {
                    //where = key + " in " + strb.ToString();
                    where = strb.ToString();
                }

                return where;
            }
        }

        public static void AddValue(JToken jt, List<string> list)
        {
            if (jt is JArray)
            {
                JArray arr = jt as JArray;
                foreach (JObject item in arr)
                {
                    string value = string.Empty;

                    if (item["row"] != null)
                    {
                        value = item["row"].Attr("key");
                    }
                    else//多行没有row属性
                    {
                        value = item.Attr("key");
                    }

                    list.Add(value);
                }
            }
            else
            {
                if (jt["row"] != null)
                {
                    string value = ((JObject)jt["row"]).Attr("key");
                    list.Add(value);
                }
                else
                {
                    string value = jt["key"].ToString();
                    list.Add(value);
                }

            }
        }

        public static void AddValue(JToken jt, List<string> list, int index)
        {
            if (jt is JArray)
            {
                JArray arr = jt as JArray;
                foreach (JObject item in arr)
                {
                    string value = string.Empty;

                    if (item["row"] != null)
                    {
                        value = item["row"].Attr("key").Split(',')[index];
                    }
                    else//多行没有row属性
                    {
                        value = item.Attr("key").Split(',')[index];
                    }

                    list.Add(value);
                }
            }
            else
            {
                if (jt["row"] != null)
                {
                    string value = ((JObject)jt["row"]).Attr("key").Split(',')[index];
                    list.Add(value);
                }
                else
                {
                    string value = jt["key"].ToString().Split(',')[index];
                    list.Add(value);
                }
            }
        }

        //拼装in条件
        public static StringBuilder BuildInXpress(List<string> list,string field)
        {
            //StringBuilder strb = new StringBuilder();
            //if (list.Count > 0)
            //{
            //    strb.Append("('");
            //    strb.Append(list[0]);
            //    strb.Append("'");
            //    for (int j = 1; j < list.Count; j++)
            //    {
            //        string str = list[j];
            //        strb.Append(",'" + str + "'");
            //    }
            //    strb.Append(")");
            //}
            //return strb;
                       

            //分组
            List<List<string>> groupList = new List<List<string>>();
            for (int i = 0; i < list.Count; i++)
            {
                int groupIndex = i / 500;//oracle in 语句不能超过1000个
                if (i % 500 == 0)
                {
                    groupList.Add(new List<string>());
                }
                List<string> tempList = groupList[groupIndex];
                tempList.Add("'" + list[i] + "'");//兼容varchar和int
            }

            StringBuilder strb = new StringBuilder();
            if (list.Count > 0)
            {

                strb.Append("(");
                int upIndex = groupList.Count - 1;
                for (int i = 0; i < groupList.Count; i++)
                {
                    List<string> ls = groupList[i];

                    string instr = string.Join(",", ls.ToArray());
                    if (i < upIndex)
                    {
                        strb.Append(field + " in (" + instr + ") or ");
                    }
                    else
                    {
                        strb.Append(field + " in (" + instr + ") ");
                    }
                }
                strb.Append(")");
            }
            return strb;
        }

        public static DataTable GetDataTable(string sql)
        {
            try
            {
                //DbHelper.Open();
                return DbHelper.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //DbHelper.Close();
            }
        }

        public static string UTCtimeToDateTime(string utc)
        {
            //"Mon Jan 7 15:07:37 UTC+0800 2013";
            //Mon Apr 08 2013 00:00:00 GMT+0800

            Dictionary<string, int> month = new Dictionary<string, int>();
            month["Jan"] = 1;
            month["Feb"] = 2;
            month["Mar"] = 3;
            month["Apr"] = 4;
            month["May"] = 5;
            month["Jun"] = 6;
            month["Jul"] = 7;
            month["Aug"] = 8;
            month["Sep"] = 9;
            month["Oct"] = 10;
            month["Nov"] = 11;
            month["Dec"] = 12;

            string[] arr = utc.Split(' ');
            StringBuilder date = new StringBuilder();

            if (utc.IndexOf("UTC") > 0)
            {
                date.Append(arr[5]);
                date.Append("-");
                date.Append(month[arr[1]]);
                date.Append("-");
                date.Append(arr[2]);
                date.Append(" ");
                date.Append(arr[3]);
            }
            else
            {
                date.Append(arr[3]);
                date.Append("-");
                date.Append(month[arr[1]]);
                date.Append("-");
                date.Append(arr[2]);
                date.Append(" ");
                date.Append(arr[4]);
            }

            return date.ToString();

        }
    }
}
