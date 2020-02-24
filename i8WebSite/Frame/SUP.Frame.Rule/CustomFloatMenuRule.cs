using System;
using System.Collections.Generic;
using SUP.Frame.DataAccess;
using SUP.Frame.DataEntity;
using System.Data;
using SUP.Common.Rule;
using SUP.Common.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace SUP.Frame.Rule
{
    public class CustomFloatMenuRule
    {
        private CustomFloatMenuDac dac = null;

        public CustomFloatMenuRule()
        {
            dac = new CustomFloatMenuDac();
        }

        public string GetFullMenuNameByCode(string code)
        {
            try
            {
                DataTable dt = null;
                string name = string.Empty;

                DataTable menuDt = dac.GetMenuDtByCode(code);

                string id = menuDt.Rows[0]["id"].ToString();
                while (true)
                {
                    dt = dac.GetMenuPidName(id, "");
                    if (name == "")
                    {
                        name += dt.Rows[0]["name"].ToString();
                    }
                    else
                    {
                        name += "-" + dt.Rows[0]["name"].ToString();
                    }

                    if (dt.Rows[0]["pid"].ToString() == "") break;
                    else
                    {
                        id = dt.Rows[0]["pid"].ToString();
                    }
                }

                string[] strs = name.Split('-');
                name = "";
                for (int i = strs.Length - 1; i > -1; i--)
                {
                    name += "-" + strs[i];
                }

                return name;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetFloatMenuTree(string code)
        {
            DataTable dt = dac.GetFloatMenuTree(code);
            Dictionary<string, string> langDic = SUP.Common.DataAccess.LangInfo.GetLabelLang("bustree");
            dt.Columns.Add("leaf",typeof(bool));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["leaf"] = true;
                if (dt.Rows[i]["langkey"] != null && dt.Rows[i]["langkey"] != DBNull.Value)
                {
                    string langKey = dt.Rows[i]["langkey"].ToString();
                    if (langDic.ContainsKey(langKey) && !string.IsNullOrWhiteSpace(langDic[langKey]))
                    {
                        string oriText = dt.Rows[i]["text"].ToString();
                        string customName = string.Empty;
                        if (oriText.IndexOf("[") > -1)
                        {
                            customName = oriText.Substring(oriText.IndexOf("["), oriText.IndexOf("]") - oriText.IndexOf("[") + 1);
                        }
                        dt.Rows[i]["text"] = langDic[langKey] + customName;
                    }
                }
            }
            return dt;
        }

        public bool SaveFloatMenu(string code, List<FloatMenuEntity> floatMenuList)
        {
            try
            {
                DataTable dt = dac.GetFloatMenuTree(code);
                string code_in;
                bool exist = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    code_in = dt.Rows[i]["code"].ToString();
                    for (int j = 0; j < floatMenuList.Count; j++)
                    {
                        if (code_in == floatMenuList[j].code)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        dac.DelFloatMenu(code, code_in);
                    }
                    exist = false;
                }

                for (int i = 0; i < floatMenuList.Count; i++)
                {
                    string result = dac.GetFloatMenuCount(code, floatMenuList[i].code);

                    if (result != "0")
                    {
                        dac.UpdateFloatMenu(code, floatMenuList[i].code, floatMenuList[i].name, floatMenuList[i].param, floatMenuList[i].url);
                    }
                    else
                    {
                        long phid = SUP.Common.Rule.CommonUtil.GetPhId("fg3_floatmenu");                      
                        dac.InsertFloatMenu(phid, code, floatMenuList[i].code, floatMenuList[i].name, floatMenuList[i].param, floatMenuList[i].url);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetFloatMenuByCode(string code)
        {
            DataTable floatMenuDt = dac.GetFloatMenuTree(code);
            Dictionary<string, string> langDic = SUP.Common.DataAccess.LangInfo.GetLabelLang("bustree");
            if (floatMenuDt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.TableName = "FloatMenu";

                dt.Columns.Add(new DataColumn("Code", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Url", Type.GetType("System.String")));

                DataRow dr;
                for (int i = 0; i < floatMenuDt.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["Code"] = floatMenuDt.Rows[i]["code"];

                    if (floatMenuDt.Rows[i]["url"] != null && !string.IsNullOrEmpty(floatMenuDt.Rows[i]["url"].ToString()))
                    {
                        dr["Url"] = GetReportUrl(floatMenuDt.Rows[i]["code"].ToString(), floatMenuDt.Rows[i]["url"].ToString(), floatMenuDt.Rows[i]["param"].ToString());
                    }
                    else
                    {                       
                        if (floatMenuDt.Rows[i]["langkey"] != null && floatMenuDt.Rows[i]["langkey"] != DBNull.Value)
                        {
                            string langKey = floatMenuDt.Rows[i]["langkey"].ToString();
                            if (langDic.ContainsKey(langKey) && !string.IsNullOrWhiteSpace(langDic[langKey]))
                            {
                                string oriText = floatMenuDt.Rows[i]["text"].ToString();
                                string customName = string.Empty;
                                if (oriText.IndexOf("[") > -1)
                                {
                                    customName = oriText.Substring(oriText.IndexOf("["), oriText.IndexOf("]") - oriText.IndexOf("[") + 1);
                                }
                                floatMenuDt.Rows[i]["text"] = langDic[langKey] + customName;
                            }
                        }

                        dr["Url"] = dac.GetFloatMenuUrl(dr["Code"].ToString(), floatMenuDt.Rows[i]["param"].ToString());
                    }

                    if (floatMenuDt.Rows[i]["text"].ToString().IndexOf("[") > -1)
                    {
                        string text = floatMenuDt.Rows[i]["text"].ToString();
                        string name = string.Empty;
                        name = text.Substring(text.IndexOf("[") + 1, text.IndexOf("]") - text.IndexOf("[") - 1);
                        dr["Name"] = name;
                    }
                    else
                    {
                        dr["Name"] = floatMenuDt.Rows[i]["text"];
                    }

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            else
            {
                return new DataTable();
            }
        }

        public string GetBusNameByCode(string code)
        {
            DataTable dt = dac.GetBusNameByCode(code);
            Dictionary<string, string> langDic = SUP.Common.DataAccess.LangInfo.GetLabelLang("bustree");

            if (dt.Rows[0]["langkey"] != null && dt.Rows[0]["langkey"] != DBNull.Value)
            {
                string langKey = dt.Rows[0]["langkey"].ToString();
                if (langDic.ContainsKey(langKey) && !string.IsNullOrWhiteSpace(langDic[langKey]))
                {
                    dt.Rows[0]["busname"] = langDic[langKey];
                }
            }

            return dt.Rows[0]["busname"].ToString();
        }

        public string GetReportUrl(string rep_id, string url, string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return url;
            }

            string reportUrl = url + "&replink=true&datavaluetype=cell&replinkdata=";
           
            StringBuilder replinkdata = new StringBuilder();
            replinkdata.Append("{ LinkRep_tarSheets: \"");
            JObject jo = (JObject)JsonConvert.DeserializeObject(param);
            string sheetid = jo["sheetid"].ToString();
            string tarSheets = sheetid;
            string ds_no = jo["ds_no"].ToString();
            List<string> paraList = new List<string>();
            string paraValue = "";
            foreach (var jp in jo)
            {
                if (jp.Key != "sheetid" && jp.Key != "ds_no")
                {
                    string value = jp.Value.ToString();
                    paraList.Add(value.Substring(0, value.IndexOf("||")));
                    if (paraValue != "")
                    {
                        paraValue += @"\\|";
                        tarSheets += (@"\\|" + sheetid);
                    }
                    paraValue += value.Substring(value.IndexOf("||") + 2);
                }
            }

            replinkdata.Append(tarSheets);
            replinkdata.Append("\", LinkRep_tarCells:\"");
            replinkdata.Append(dac.GetReportCells(rep_id, ds_no, paraList));
            replinkdata.Append("\",LinkRep_srcParas:\"");
            replinkdata.Append(paraValue);
            replinkdata.Append("\"}");

            return reportUrl + replinkdata.ToString();
        }

        public string LoadReportList()
        {
            DataTable dt = dac.LoadSysReportList().Copy();
            IList<TreeJSONBase> sysList = LoadReportTreeList(dt, "1");

            dt = dac.LoadCusReportList().Copy();
            IList<TreeJSONBase> cusList = LoadReportTreeList(dt, "0");

            return "{\"sysReport\": " + JsonConvert.SerializeObject(sysList) + ", \"cusReport\": " + JsonConvert.SerializeObject(cusList) + "}";
        }

        public string LoadSearchReportList(string search)
        {
            DataTable dt = dac.LoadSearchSysReportList(search).Copy();
            IList<TreeJSONBase> sysList = LoadReportTreeList(dt, "1");

            dt = dac.LoadSearchCusReportList(search).Copy();
            IList<TreeJSONBase> cusList = LoadReportTreeList(dt, "0");

            return "{\"sysReport\": " + JsonConvert.SerializeObject(sysList) + ", \"cusReport\": " + JsonConvert.SerializeObject(cusList) + "}";
        }

        IList<TreeJSONBase> LoadReportTreeList(DataTable dt,string rep_src)
        {
            string filter = "(pid=0)";
            RemoveNode(dt);
            dt.AcceptChanges();
            dt.Columns.Add("rep_src", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["rep_src"] = rep_src;
            }
            IList<TreeJSONBase> treeList = new ReportListBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);//加载两层
            foreach (ReportListJSONBase node in treeList)
            {
                DealId(node);
            }
            return treeList;
        }

        //清除没有子节点的非叶子节点
        private void RemoveNode(DataTable menudt)
        {
            //Dictionary<string, string> langDic = SUP.Frame.DataAccess.LangInfo.GetLabelLang("SystemMenu");

            //处理掉父节点的seq小于子节点seq的那些行
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                string id = dr["phid"].ToString();

                if (dr["isleaf"].ToString() == "0")//非功能节点
                {
                    DataRow[] tempdr = menudt.Select("pid='" + id + "'");
                    if (tempdr.Length == 0)
                    {
                        dr.Delete();
                    }
                }
            }

            //清除剩余的节点，父节点seq大于子节点seq
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                if (dr.RowState == DataRowState.Deleted) continue;

                string id = dr["phid"].ToString();
                if (dr["isleaf"].ToString() == "0")//非功能节点
                {
                    bool hit = false;
                    IsValidPath(id, menudt, ref hit);
                    if (!hit)
                    {
                        dr.Delete();
                    }
                }
            }
        }

        //检测是否是有效路径，从当前节点出发能到达叶子节点
        //递归处理，容易引起性能问题，最好是toolkit导出的时候菜单的顺序排好，
        //保证父节点的seq小于子节点的seq，就不需要递归检测
        private void IsValidPath(string id, DataTable menudt, ref bool hit)
        {
            if (hit)
            {
                return;
            }
            DataRow[] tempdr = menudt.Select("pid='" + id + "'");
            if (tempdr.Length > 0)
            {
                foreach (DataRow dr in tempdr)
                {
                    string flg = dr["isleaf"].ToString();
                    if (flg == "1")
                    {
                        hit = true;
                        return;
                    }

                    if (!hit)
                    {
                        string tempid = dr["phid"].ToString();
                        IsValidPath(tempid, menudt, ref hit);
                    }
                    else
                    {
                        break;//已经检测到有效，跳出循环
                    }
                }
            }
        }

        /// <summary>
        /// 避免id重复，重新处理树节点的id
        /// </summary>
        /// <param name="dt"></param>
        private void DealId(ReportListJSONBase node)
        {
            if (node.leaf)
            {
                node.id += "1" + node.src;
            }
            else
            {
                node.id += "0" + node.src;
            }

            if(node.children != null && node.children.Count > 0)
            {
                foreach (ReportListJSONBase childNode in node.children)
                {
                    DealId(childNode);
                }
            }
        }

        class ReportListJSONBase : TreeJSONBase
        {
            public virtual string url { get; set; }
            public virtual string bustype { get; set; }
            public virtual string phid { get; set; }
            public virtual string src { get; set; }
        }

        public class ReportListBuilder : ExtJsTreeBuilderBase
        {
            public override TreeJSONBase BuildTreeNode(DataRow dr)
            {
                ReportListJSONBase node = new ReportListJSONBase();
                node.text = dr["rep_name"].ToString();
                node.phid = dr["phid"].ToString();
                node.leaf = (dr["isleaf"].ToString() == "1");
                node.bustype = dr["rep_code"].ToString();
                node.id = dr["phid"].ToString();
                node.src = dr["rep_src"].ToString();

                if (dr["isleaf"].ToString() == "1")
                {
                    node.url = "RW/DesignFrame/ReportView?rep_src=" + node.src  + "&rep_id=" + node.phid + "&rep_code=" + node.bustype;
                }

                return node;
            }           
        }
        
    }
}
