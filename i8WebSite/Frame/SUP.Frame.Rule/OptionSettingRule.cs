//using NG3.Data.Service;
using SUP.Common.Base;
using SUP.Frame.DataAccess;
using SUP.Frame.DataEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SUP.Frame.Rule
{

    public class OptionSettingRule
    {
        private OptionSettingDac dac = null;

        public OptionSettingRule()
        {
            dac = new OptionSettingDac();
        }

        public IList<TreeJSONBase> LoadOptionTree(string moduleid)
        {
            string sql = string.Empty;
            string filter = string.Empty;

            DataTable dt = this.GetMainTreeData();
            if (moduleid == null || moduleid == string.Empty)
                filter = " parent_phid is null or parent_phid=0 ";
            else
                filter = " phid=" + Convert.ToInt64(moduleid) + "";
            return new OptionSettingBuilder().GetExtTreeList(dt, "parent_phid", "phid", filter, TreeDataLevelType.TopLevel);//加载两层
        }

        public List<OptionSettingEntity> GetOptionDetail(string option_group)
        {
            DataTable dt = dac.GetOptionDetail(option_group);
            List<OptionSettingEntity> optionList = new List<OptionSettingEntity>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    OptionSettingEntity optionModel = new OptionSettingEntity();
                    optionModel.option_group = dt.Rows[i]["option_group"].ToString();
                    optionModel.option_code = dt.Rows[i]["option_code"].ToString();
                    optionModel.option_name = dt.Rows[i]["option_name"].ToString();
                    optionModel.isunify = dt.Rows[i]["isunify"].ToString();
                    optionModel.option_type = dt.Rows[i]["option_type"].ToString();
                    optionModel.option_value = dt.Rows[i]["option_value"].ToString();
                    optionModel.range = dt.Rows[i]["range"].ToString();
                    optionList.Add(optionModel);
                }
            }
            return optionList;
        }

        public OptionSettingEntity GetOptionValue(string option_group, string option_code, string key)
        {
            DataTable dt = dac.GetOptionDetail(option_group, option_code);

            OptionSettingEntity optionModel = new OptionSettingEntity();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["isunify"].ToString() == "1")
                {
                    optionModel.option_group = dt.Rows[0]["option_group"].ToString();
                    optionModel.option_code = dt.Rows[0]["option_code"].ToString();
                    optionModel.option_name = dt.Rows[0]["option_name"].ToString();
                    optionModel.isunify = dt.Rows[0]["isunify"].ToString();
                    optionModel.option_type = dt.Rows[0]["option_type"].ToString();
                    optionModel.option_value = dt.Rows[0]["option_value"].ToString();
                    optionModel.range = dt.Rows[0]["range"].ToString();
                }
            }
            return optionModel;
        }

        public OptionSettingEntity GetValueByCode(string option_code)
        {
            DataTable dt = dac.GetValueByCode(option_code);

            OptionSettingEntity optionModel = new OptionSettingEntity();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["isunify"].ToString() == "1")
                {
                    optionModel.option_group = dt.Rows[0]["option_group"].ToString();
                    optionModel.option_code = dt.Rows[0]["option_code"].ToString();
                    optionModel.option_name = dt.Rows[0]["option_name"].ToString();
                    optionModel.isunify = dt.Rows[0]["isunify"].ToString();
                    optionModel.option_type = dt.Rows[0]["option_type"].ToString();
                    optionModel.option_value = dt.Rows[0]["option_value"].ToString();
                    optionModel.range = dt.Rows[0]["range"].ToString();
                }
            }
            return optionModel;
        }

        public string GetOptionDetail(string option_group, string option_code)
        {
            List<object> detailList = new List<object>();
            Dictionary<string, string> detailDic = new Dictionary<string, string>();
            DataTable dt = dac.GetOptionDetail(option_group, option_code);

            DataTable dtValue = new DataTable();
            Dictionary<string, string> argDic = new Dictionary<string, string>();
            //List<object> argList = new List<object>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    detailDic.Add("option_group", dt.Rows[i]["option_group"].ToString());
                    detailDic.Add("option_code", dt.Rows[i]["option_code"].ToString());
                    detailDic.Add("option_name", dt.Rows[i]["option_name"].ToString());
                    detailDic.Add("isunify", dt.Rows[i]["isunify"].ToString());
                    detailDic.Add("option_type", dt.Rows[i]["option_type"].ToString());
                    detailDic.Add("option_value", dt.Rows[i]["option_value"].ToString());
                    if (dt.Rows[i]["isunify"].ToString() == "0")
                    {
                        dtValue = dac.GetArgValue(dt.Rows[i]["phid"].ToString());
                        if (dtValue.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtValue.Rows.Count; j++)
                            {
                                argDic.Add(dtValue.Rows[j]["id"].ToString(), dtValue.Rows[j]["argument"].ToString());
                            }
                        }
                    }
                }
                detailList.Add(detailDic);
                detailList.Add(new { argJson = argDic });
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(detailList);
            return json;
        }

        public Dictionary<string, string> GetArgumentDic(string option_group, string option_code, string[] keys)
        {
            Dictionary<string, string> argDic = new Dictionary<string, string>();
            DataTable dt = dac.GetOptionDetail(option_group, option_code);
            if (dt.Rows.Count > 0)
            {
                DataTable dtValue = dac.GetDicValue(dt.Rows[0]["phid"].ToString(), keys);
                if (dtValue.Rows.Count > 0)
                {
                    for (int j = 0; j < dtValue.Rows.Count; j++)
                    {
                        argDic.Add(dtValue.Rows[j]["id"].ToString(), dtValue.Rows[j]["argument"].ToString());
                    }
                }
            }
            return argDic;
        }

        public string GetSingleArgument(string option_group, string option_code, string key)
        {
            string argValue = string.Empty;
            DataTable dt = dac.GetOptionDetail(option_group, option_code);
            if (dt.Rows.Count > 0)
            {
                DataTable dtValue = dac.GetSingleValue(dt.Rows[0]["phid"].ToString(), key);
                if (dtValue.Rows.Count > 0)
                {
                    argValue = dtValue.Rows[0]["argument"].ToString();
                }
            }
            return argValue;
        }

        public string GetInitSettingValue(string option_group, string option_code)
        {
            DataTable dt = new DataTable();
            string optionValue = string.Empty;
            var obj = HttpRuntime.Cache.Get("option_value");
            if (obj != null)
                dt = (DataTable)obj;
            if (dt != null && dt.Rows.Count > 0)
            {
                dt = dt.Select("option_group='" + option_group + "' and option_code='" + option_code + "'").CopyToDataTable();
            }
            else
            {
                dt = dac.GetOptionDetail(option_group, option_code);
            }

            if (dt.Rows.Count > 0)
            {
                optionValue = dt.Rows[0]["option_value"] != DBNull.Value ? dt.Rows[0]["option_value"].ToString() : "";
            }
            return optionValue;
        }


        public string GetValueByKey(string option_group, string option_code, string key)
        {
            DataTable dt = new DataTable();
            string optionValue = string.Empty;
            var obj = HttpRuntime.Cache.Get("option_value");
            if (obj != null)
                dt = (DataTable)obj;
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("option_group='" + option_group + "' and option_code='" + option_code + "'");
                //防止没有这条数据报错
                if (drs.Length > 0)
                {
                    dt = dt.Select("option_group='" + option_group + "' and option_code='" + option_code + "'").CopyToDataTable();
                }
                else {
                    dt = dac.GetOptionDetail(option_group, option_code);
                }
            }
            else
            {
                dt = dac.GetOptionDetail(option_group, option_code);
            }
            //dt = dac.GetOptionDetail(option_group, option_code);
            DataTable dtValue = new DataTable();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["isunify"].ToString() == "0" && !string.IsNullOrEmpty(key))
                {
                    dtValue = dac.GetArgValue(dt.Rows[0]["phid"].ToString());
                    if (dtValue != null && dtValue.Rows.Count > 0)
                    {
                        DataRow dr = dtValue.Select("id='" + key + "'").FirstOrDefault();
                        if (dr != null && dr["argument"] != DBNull.Value)
                        {
                            optionValue = dr["argument"].ToString();
                        }
                        else
                        {
                            optionValue = dt.Rows[0]["option_value"].ToString();
                        }
                    }

                }
                else
                {
                    optionValue = dt.Rows[0]["option_value"].ToString();
                }
            }
            return optionValue;
        }

        public string GetValueByKey(string conn,string option_group, string option_code, string key)
        {
            DataTable dt = new DataTable();
            string optionValue = string.Empty;
            var obj = HttpRuntime.Cache.Get("option_value");
            if (obj != null)
                dt = (DataTable)obj;
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("option_group='" + option_group + "' and option_code='" + option_code + "'");
                //防止没有这条数据报错
                if (drs.Length > 0)
                {
                    dt = dt.Select("option_group='" + option_group + "' and option_code='" + option_code + "'").CopyToDataTable();
                }
                else
                {
                    dt = dac.GetOptionDetail(conn,option_group, option_code);
                }
            }
            else
            {
                dt = dac.GetOptionDetail(conn,option_group, option_code);
            }
            //dt = dac.GetOptionDetail(option_group, option_code);
            DataTable dtValue = new DataTable();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["isunify"].ToString() == "0" && !string.IsNullOrEmpty(key))
                {
                    dtValue = dac.GetArgValue(conn,dt.Rows[0]["phid"].ToString());
                    if (dtValue != null && dtValue.Rows.Count > 0)
                    {
                        DataRow dr = dtValue.Select("id='" + key + "'").FirstOrDefault();
                        if (dr != null && dr["argument"] != DBNull.Value)
                        {
                            optionValue = dr["argument"].ToString();
                        }
                        else
                        {
                            optionValue = dt.Rows[0]["option_value"].ToString();
                        }
                    }

                }
                else
                {
                    optionValue = dt.Rows[0]["option_value"].ToString();
                }
            }
            return optionValue;
        }

        public Dictionary<string, string> GetDicValueByKey(string option_group, string key)
        {
            Dictionary<string, string> dicValue = new Dictionary<string, string>();
            DataTable dt = dac.GetOptionDetail(option_group);
            DataTable dtValue = new DataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["isunify"].ToString() == "0")
                    {
                        dtValue = dac.GetArgValue(dt.Rows[0]["phid"].ToString());
                        DataRow dr = dtValue.Select("id='" + key + "'").FirstOrDefault();
                        if (dr != null && dr["argument"] != DBNull.Value)
                        {
                            dicValue.Add(dt.Rows[i]["option_code"].ToString(), dr["argument"].ToString());
                        }
                        else
                        {
                            dicValue.Add(dt.Rows[i]["option_code"].ToString(), dt.Rows[i]["option_value"].ToString());
                        }
                    }
                    else
                    {
                        dicValue.Add(dt.Rows[i]["option_code"].ToString(), dt.Rows[i]["option_value"].ToString());
                    }
                }
            }
            return dicValue;
        }

        public Dictionary<string, string> GetValueByGroup(string option_group)
        {
            Dictionary<string, string> dicValue = new Dictionary<string, string>();

            DataTable dt = dac.GetOptionDetail(option_group);
            DataTable dtValue = new DataTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["isunify"].ToString() == "0")
                    {
                        dtValue = dac.GetArgValue(dt.Rows[0]["phid"].ToString());
                        DataRow[] dr = dtValue.Select("id is not null");
                        if (dr != null && dr.Length > 0)
                        {
                            for (int j = 0; j < dr.Length; j++)
                            {
                                Dictionary<string, string> argValue = new Dictionary<string, string>();
                                if (!argValue.ContainsKey(dr[j]["id"].ToString()))
                                {
                                    argValue.Add(dr[j]["id"].ToString(), dr[j]["argument"] != DBNull.Value ? dr[j]["argument"].ToString() : dt.Rows[i]["option_value"].ToString());
                                }
                                dicValue.Add(dt.Rows[i]["option_code"].ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(argValue));
                            }
                            
                        }
                        else
                        {
                            dicValue.Add(dt.Rows[i]["option_code"].ToString(), dt.Rows[i]["option_value"].ToString());
                        }
                    }
                    else
                    {
                        dicValue.Add(dt.Rows[i]["option_code"].ToString(), dt.Rows[i]["option_value"].ToString());
                    }
                }
            }
            return dicValue;
        }

        public DataTable GetMainTreeData()
        {
            DataTable menudt = dac.LoadTreeData().Copy();

            return menudt;
        }

        class OptionSettingTreeJSONBase : TreeJSONBase
        {
            public virtual string url { get; set; }
            public virtual string pid { get; set; }
            public virtual string name { get; set; }
            //public virtual string originalcode { get; set; }
            public virtual long phid { get; set; }
        }

        public class OptionSettingBuilder : ExtJsTreeBuilderBase
        {
            public override TreeJSONBase BuildTreeNode(DataRow dr)
            {
                //string root = System.Web.HttpContext.Current.Request.ApplicationPath; //+ "/";

                //if (root != "/")//
                //{
                //    if (dr["url"].ToString().StartsWith("/") == false)//有些url以"/"起头，不需要加了
                //    {
                //        root += "/";
                //    }
                //}

                OptionSettingTreeJSONBase node = new OptionSettingTreeJSONBase();

                //string functionname = dr["functionname"].ToString();

                node.id = dr["phid"].ToString();
                node.text = dr["name"].ToString();
                node.phid = Int64.Parse(dr["phid"].ToString());
                node.pid = dr["parent_phid"].ToString();
                node.name = dr["name"].ToString();
                node.leaf = dr["optiontype"].ToString() == "2";
                node.url = dr["url"].ToString();
                return node;
            }
        }


        public IList<TreeJSONBase> LoadOrgTree(string detailPhid)
        {
            string sql = string.Empty;
            string filter = " parent_orgid is null or parent_orgid=0";

            DataTable dt = dac.LoadOrgTree(detailPhid);
            return new OrgTreeBuilder().GetExtTreeList(dt, "parentorg", "ocode", filter, TreeDataLevelType.TopLevel);//加载两层
        }

        //组织树
        class OrgTreeJSONBase : TreeJSONBase
        {
            public virtual string argument { get; set; }
            public virtual string option_value_name { get; set; }
            public virtual string name { get; set; }
            //public virtual string originalcode { get; set; }
            public virtual string phid { get; set; }

            public virtual string orgid { get; set; }
        }


        public class OrgTreeBuilder : ExtJsTreeBuilderBase
        {
            public override TreeJSONBase BuildTreeNode(DataRow dr)
            {
                //string root = System.Web.HttpContext.Current.Request.ApplicationPath; //+ "/";

                //if (root != "/")//
                //{
                //    if (dr["url"].ToString().StartsWith("/") == false)//有些url以"/"起头，不需要加了
                //    {
                //        root += "/";
                //    }
                //}

                OrgTreeJSONBase node = new OrgTreeJSONBase();

                //string functionname = dr["functionname"].ToString();

                node.id = dr["ocode"].ToString();
                node.orgid = dr["org_id"].ToString();
                node.option_value_name = dr["name"].ToString();
                node.argument = dr["argument"].ToString();
                node.name = dr["name"].ToString();
                node.text = "(" + dr["ocode"].ToString() + ")" + dr["oname"].ToString();
                node.phid = dr["phid"] != null ? dr["phid"].ToString() : "";
                //node.pid = dr["parent_phid"].ToString();
                node.name = "(" + dr["ocode"].ToString() + ")" + dr["oname"].ToString();
                //node.leaf = dr["optiontype"].ToString() == "2";
                //node.url = dr["url"].ToString();
                return node;
            }
        }
    }
}
