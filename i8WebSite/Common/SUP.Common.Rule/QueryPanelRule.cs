using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataAccess;
using System.Data;
using SUP.Common.DataEntity.Individual;
using System.Web.Caching;
using SUP.Common.Base;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SUP.Common.Rule
{
    public class QueryPanelRule
    {
        private const string CACHEDID = "_ng3queryPanel";
        private const string MAINKEY = "_ng3queryPanelTimeStampkey";

        /// <summary>
        /// 
        /// </summary>
        private QueryPanelDac queryPanelDac;
        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryPanelRule()
        {
            queryPanelDac = new QueryPanelDac();
        }

        /// <summary>
        /// 保存用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="ClientJsonString"></param>
        /// <returns></returns>
        public int SetQueryPanelData(string PageId, string ClientJsonString)
        {
            return queryPanelDac.SetQueryPanelData(PageId, ClientJsonString);
        }

        /// <summary>
        /// 获取用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <returns></returns>
        public DataTable GetQueryPanelData(string PageId)
        {
            return queryPanelDac.GetQueryPanelData(PageId);
        }

        /// <summary>
        /// 获取用户排序数据
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public string GetSortFilterData(string pageid, string ocode, string logid)
        {
            DataTable dt = queryPanelDac.GetSortFilterData(pageid, ocode, logid);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                string sortFilter = " order by ";

                foreach (DataRow dr in dt.Rows)
                {

                    if (Convert.ToInt32(dr["sortmode"]) == 1)
                    {
                        sortFilter += dr["searchfield"].ToString() + " asc, ";
                    }
                    else
                    {
                        sortFilter += dr["searchfield"].ToString() + " desc, ";
                    }

                }
                return sortFilter.ToString().Substring(0, sortFilter.LastIndexOf(","));

                //return sortFilter;
            }


        }

        public string GetCheckData(string pageid, string ocode, string logid)
        {
            string ischeck = queryPanelDac.GetCheckData(pageid, ocode, logid);
            if (String.IsNullOrEmpty(ischeck))
            {
                return "1";
            }
            else
            {
                return ischeck;
            }
            //return ischeck;
        }

        public int SaveCheckData(string pageid, string ocode, string logid, string ischeck)
        {
            return queryPanelDac.SaveCheckData(pageid, ocode, logid, ischeck);
        }

        public QueryPanelInfoEntity GetQueryPanel(string pageId, string ocode, string logid)
        {
            IList<ExtControlInfoBase> list = this.GetIndividualQueryPanel(pageId, ocode, logid);

            DataTable tmpDT = this.GetQueryPanelData(pageId);
            var langInfo = this.GetLang();//获取多语言
            string sortfilter = this.GetSortFilterData(pageId, ocode, logid);

            string ischeck = this.GetCheckData(pageId, ocode, logid);

            Dictionary<string, object> rememStr = null;
            if (tmpDT.Rows.Count > 0)
            {
                //rememStr = tmpDT.Rows[0]["remeberstr"].ToString();
                rememStr = JsonConvert.DeserializeObject<Dictionary<string, object>>(tmpDT.Rows[0]["remeberstr"].ToString());//json先转成对象
            }

            QueryPanelInfoEntity en = new QueryPanelInfoEntity();
            en.list = list;
            en.rememberstr = rememStr;
            en.langInfo = langInfo;
            en.sortfilter = sortfilter;
            en.ischeck = ischeck;
            return en;         
        }


        public IList<ExtControlInfoBase> GetIndividualQueryPanel(string pageId, string ocode, string logid)
        {


            #region 获取控件
            DataTable dt = queryPanelDac.GetIndividualQueryPanelInfo(pageId, ocode, logid);
            IList<ExtControlInfoBase> list = new List<ExtControlInfoBase>();
            if (dt.Rows.Count == 0)
            {
                return list;//无内嵌查询
            }

            #region 缓存处理

            string cachedKey = new StringBuilder().Append(CACHEDID).Append("_")
                                           .Append(ocode).Append("_").Append(logid).Append("_").Append(pageId).ToString();
            string cachedTimeKey = cachedKey + "time";//时间戳缓存键

            string localTimeStamp = HttpRuntime.Cache.Get(cachedTimeKey) as String;
            string cachedSrvTimeStamp = NG.Cache.Client.CacheClient.Instance.GetData(MAINKEY, cachedTimeKey) as String;
            if (localTimeStamp == cachedSrvTimeStamp)//本地时间戳与缓存服务器的时间戳比对
            {
                IList<ExtControlInfoBase> cachedList = HttpRuntime.Cache.Get(cachedKey) as IList<ExtControlInfoBase>;
                if (cachedList != null)
                {
                    return cachedList;
                }
            }

            #endregion

            //获取内嵌查询多语言键值对
            Dictionary<string, string> langDic = SUP.Common.DataAccess.LangInfo.GetLabelLang(pageId);

            string tables = GetTableNames(dt);
            DataTable PropertyDt = queryPanelDac.GetPropertyDt(tables);

            RichHelpDac richHelpDac = new RichHelpDac();

            foreach (DataRow dr in dt.Rows)
            {
                ExtControlInfoBase col = new ExtControlInfoBase();
                            

                string searchetable = dr["searchtable"].ToString();
                string searchfield = dr["searchfield"].ToString();
                string fieldname = dr["searchfield"].ToString();
                string langkey = dr["langkey"].ToString(); ;//全部
                fieldname = GetPropertyName(PropertyDt, searchetable, searchfield, fieldname);

                //处理itemId         
                string itemId = fieldname;

                fieldname = DealDataType(dr, fieldname);
                fieldname = DealOperation(dr, fieldname);

                fieldname += "*" + dr["isaddtowhere"].ToString();//是否直接参与查询


                string label = dr["fname_chn"].ToString();//标签名
                if (langDic.ContainsKey(langkey) && !string.IsNullOrEmpty(langDic[langkey]))
                {
                    label = langDic[langkey];//取多语言
                }
                string xtype = string.Empty;
                string displayfield = "";
                string valuefield = "";
                string namefield = "";
                string codefield = "";
                string usercodefield = string.Empty;//用户编码
                string usercodePro = string.Empty;//用户编码对应的属性名
                switch (dr["controltype"].ToString())
                {
                    case "TextBox":
                        xtype = "ngText";
                        break;
                    case "CompositeTextBox":
                        xtype = "ngRichHelp";
                        NGRichHelp richhelp = new NGRichHelp();
                        richhelp.helpid = dr["controlflag"].ToString();
                        richhelp.xtype = xtype;
                        richhelp.name = fieldname;
                        richhelp.itemId = itemId;
                        richhelp.fieldLabel = label;
                        richhelp.matchFieldWidth = false;
                        //if (dr["matchfieldwidth"] != null && dr["matchfieldwidth"] != DBNull.Value)
                        //{
                        //    if (dr["matchfieldwidth"].ToString() == "0")
                        //    {
                        //        richhelp.matchFieldWidth = false;
                        //    }
                        //}
                        //else
                        //{
                        //    richhelp.matchFieldWidth = false;//为空也设置为false
                        //}
                        richhelp.showAutoHeader = false;
                        if (!string.IsNullOrEmpty(richhelp.helpid))
                        {
                            SUP.Common.DataEntity.CommonHelpEntity helpEntity = richHelpDac.GetCommonHelpItem(richhelp.helpid);
                        
                            namefield = helpEntity.NameField;
                            codefield = helpEntity.CodeField;
                            codefield = this.DeleteTableName(codefield);
                            namefield = this.DeleteTableName(namefield);                                    
                            usercodefield = helpEntity.UserCodeField;                          
                            string tablename = helpEntity.TableName;                        
                            displayfield = helpEntity.NameProperty;
                            valuefield = helpEntity.CodeProperty;
                            usercodefield = this.DeleteTableName(usercodefield);
                            valuefield = this.DeleteTableName(valuefield);
                            displayfield = this.DeleteTableName(displayfield);                           
                            if (string.IsNullOrWhiteSpace(usercodefield))
                            {
                                usercodefield = codefield;
                            }
                            else
                            {                                
                                usercodePro = helpEntity.UserCodeProperty;
                            }
                            if (string.IsNullOrEmpty(displayfield) && string.IsNullOrEmpty(valuefield))
                            {

                                richhelp.displayField = namefield;
                                richhelp.valueField = codefield;
                                richhelp.usercodeField = usercodefield;
                                richhelp.ORMMode = false;//richhelp这个必须为false
                            }
                            else
                            {
                                if (tablename.IndexOf(",") > 0 || tablename.IndexOf("=") > 0)//表名有多表关联,属性无法正确获取，不走orm
                                {
                                    richhelp.ORMMode = false;//richhelp这个必须为false
                                }
                                richhelp.displayField = displayfield;
                                richhelp.valueField = valuefield;
                                richhelp.usercodeField = usercodePro;
                            }
                        }
                        richhelp.listFields = richhelp.usercodeField + "," + richhelp.displayField + "," + richhelp.valueField;
                        richhelp.value = dr["defaultdata"].ToString();
                        richhelp.listHeadTexts = "代码,名称";
                        richhelp.clientSqlFilter = dr["sqlfilter"].ToString();//sql过滤条件
                        list.Add(richhelp);
                        continue;
                    case "NGCustomFormHelp":
                        xtype = "ngCustomFormHelp";
                        NGCustomFormHelp customhelp = new NGCustomFormHelp();//自定义表单的帮助
                        customhelp.helpid = dr["controlflag"].ToString();
                        customhelp.xtype = xtype;
                        customhelp.name = fieldname;
                        customhelp.itemId = itemId;
                        customhelp.fieldLabel = label;
                        customhelp.showAutoHeader = false;
                        if (!string.IsNullOrEmpty(customhelp.helpid))
                        {
                            SUP.CustomForm.DataAccess.HelpDac customdac = new CustomForm.DataAccess.HelpDac();
                            SUP.CustomForm.DataEntity.HelpEntity helpEntity = customdac.GetCustomFormHelpItem(customhelp.helpid);
                            namefield = helpEntity.NameField;
                            codefield = helpEntity.CodeField;
                            if (codefield.IndexOf(".") > 0)
                            {
                                codefield = codefield.Split('.')[1];//
                            }
                            if (namefield.IndexOf(".") > 0)
                            {
                                namefield = namefield.Split('.')[1];
                            }
                            usercodefield = helpEntity.NoField;

                            customhelp.displayField = namefield;
                            customhelp.valueField = codefield;
                            customhelp.usercodeField = usercodefield;
                            customhelp.ORMMode = false;

                        }
                        customhelp.listFields = customhelp.usercodeField + "," + customhelp.displayField + "," + customhelp.valueField;
                        customhelp.value = dr["defaultdata"].ToString();
                        customhelp.listHeadTexts = "代码,名称";
                        customhelp.clientSqlFilter = dr["sqlfilter"].ToString();//sql过滤条件
                        list.Add(customhelp);
                        continue;
                    case "DateBox":
                        xtype = "ngDate";
                        break;
                    case "DateTimeBox":
                        xtype = "ngDateTime";
                        break;
                    case "DropDownList":
                        xtype = "ngComboBox";
                        NGComboBox combo = new NGComboBox();
                        combo.helpid = dr["controlflag"].ToString();
                        if (dr["matchfieldwidth"] != null && dr["matchfieldwidth"] != DBNull.Value)
                        {
                            if (dr["matchfieldwidth"].ToString() == "0")
                            {
                                combo.matchFieldWidth = false;
                            }
                        }
                        if (!string.IsNullOrEmpty(combo.helpid))
                        {
                            SUP.Common.DataEntity.CommonHelpEntity helpEntity = richHelpDac.GetCommonHelpItem(combo.helpid);
                            combo.queryMode = "remote";                         
                            namefield = helpEntity.NameField;
                            codefield = helpEntity.CodeField;
                            usercodefield = helpEntity.UserCodeField;
                            codefield = this.DeleteTableName(codefield);
                            namefield = this.DeleteTableName(namefield);
                            string tablename = helpEntity.TableName;
                            displayfield = helpEntity.NameProperty;
                            valuefield = helpEntity.CodeProperty;
                            if (string.IsNullOrWhiteSpace(usercodefield))
                            {
                                usercodefield = codefield;
                            }
                            else
                            {
                                usercodePro = helpEntity.UserCodeProperty;
                            }
                            if (string.IsNullOrEmpty(displayfield) && string.IsNullOrEmpty(valuefield))
                            {
                                combo.displayField = namefield;
                                combo.valueField = codefield;
                                combo.usercodeField = usercodefield;
                                combo.ORMMode = false;//richhelp这个必须为false
                            }
                            else
                            {
                                if (tablename.IndexOf(",") > 0 || tablename.IndexOf("=") > 0)//表名有多表关联,属性无法正确获取，不走orm
                                {
                                    combo.ORMMode = false;//richhelp这个必须为false
                                }
                                combo.displayField = displayfield;
                                combo.valueField = valuefield;
                                combo.usercodeField = usercodePro;
                            }
                            if (namefield == codefield)//不相等出两列
                            {
                                combo.listFields = combo.displayField;
                                combo.listHeadTexts = "名称";
                            }
                            else
                            {
                                combo.listFields = combo.usercodeField + "," + combo.displayField + "," + combo.valueField;
                                combo.listHeadTexts = "编码,名称";
                            }

                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(dr["datasource"].ToString()))
                            {
                                throw new Exception(string.Format("内嵌查询字段【{0}】类型为下拉，但未配置数据源！", itemId));
                            }
                            combo.data = ColumnInfoBuilder.TranslateData(dr["datasource"].ToString(), '|', ':');
                            combo.queryMode = "local";
                        }
                        combo.xtype = xtype;
                        combo.name = fieldname;
                        combo.itemId = itemId;
                        combo.fieldLabel = label;
                        combo.value = dr["defaultdata"].ToString();
                        combo.clientSqlFilter = dr["sqlfilter"].ToString();//sql过滤条件
                        list.Add(combo);
                        continue;
                    case "CheckBoxList":
                        xtype = "ngCheckbox";
                        break;
                    case "AutoCompleteWithCommonHelpControl":
                        xtype = "ngCommonHelp";
                        namefield = dr["namefield"].ToString();
                        codefield = dr["codefield"].ToString();
                        NGCommonHelp commonHelp = new NGCommonHelp();
                        commonHelp.helpid = dr["controlflag"].ToString();
                        displayfield = DataConverterHelper.FieldToProperty(dr["tablename"].ToString(), namefield);
                        valuefield = DataConverterHelper.FieldToProperty(dr["tablename"].ToString(), codefield);
                        if (string.IsNullOrEmpty(codefield) && string.IsNullOrEmpty(namefield))//从xml取数
                        {
                            DataEntity.CommonHelpEntity item = new CommonHelpDac().GetHelpItem(commonHelp.helpid);
                            commonHelp.displayField = item.NameField;
                            commonHelp.valueField = item.CodeField;

                        }
                        else if (string.IsNullOrEmpty(dr["modelname"].ToString()) && string.IsNullOrEmpty(displayfield) && string.IsNullOrEmpty(valuefield))
                        {
                            commonHelp.displayField = namefield;
                            commonHelp.valueField = codefield;
                        }
                        else
                        {
                            commonHelp.displayField = dr["modelname"].ToString() + '.' + displayfield;
                            commonHelp.valueField = dr["modelname"].ToString() + '.' + valuefield;
                        }
                        commonHelp.xtype = xtype;
                        commonHelp.fieldLabel = label;
                        commonHelp.name = fieldname;
                        list.Add(commonHelp);
                        continue;
                    case "ngOrgHelp":
                        xtype = "ngOrgHelp";//组织组件
                        break;
                    case "ngProjectHelp":
                        xtype = "ngProjectHelp";//项目组件
                        break;
                    case "ngCustomFileHelp":
                        xtype = "ngCustomFileHelp";//客户
                        break;
                    case "ngSupplyFileHelp":
                        xtype = "ngSupplyFileHelp";//供应商
                        break;
                    case "ngEnterpriseHelp":
                        xtype = "ngEnterpriseHelp";//往来单位
                        break;
                    case "ngEmpHelp":
                        xtype = "ngEmpHelp";//员工
                        break;
                    case "WbsHelpField":
                        xtype = "WbsHelpField";//wbs
                        break;
                    case "ItemDataHelpField":
                        xtype = "ItemDataHelpField";//物料帮助
                        break;
                    case "CntInfoHelpField":
                        xtype = "CntInfoHelpField";//合同帮助
                        break;
                    //增加numberfield控件
                    case "numberfield":
                        xtype = "numberfield";//数字控件
                        break;
                    default:
                        xtype = "ngText";
                        break;
                }

                col = IndividualInfoFactory.GetControlInfo(xtype, fieldname, label, string.Empty, 100, 2);//内嵌查询最大长度100,2位小数
                col.value = dr["defaultdata"].ToString();
                list.Add(col);
            }

            #region 缓存处理            
            //缓存起来
            HttpRuntime.Cache.Remove(cachedKey);//先remove
            HttpRuntime.Cache.Add(cachedKey,
                                  list,
                                  null,
                                  DateTime.Now.AddDays(1),
                                  Cache.NoSlidingExpiration,
                                  CacheItemPriority.NotRemovable,
                                  null);

            string time = DateTime.Now.ToString("yyyyMMddhhmmss");
            //时间戳本地缓存
            HttpRuntime.Cache.Remove(cachedTimeKey);//先remove
            HttpRuntime.Cache.Add(cachedTimeKey,
                                  time,
                                  null,
                                  DateTime.Now.AddDays(1),
                                  Cache.NoSlidingExpiration,
                                  CacheItemPriority.NotRemovable,
                                  null);

            NG.Cache.Client.CacheClient.Instance.Add(MAINKEY, cachedTimeKey, time);//外部缓存存放时间戳
            #endregion

            return list;
            #endregion
        }

        private static string GetPropertyName(DataTable PropertyDt, string searchetable, string searchfield, string fieldname)
        {
            DataRow[] drs = PropertyDt.Select(string.Format("c_bname='{0}' and c_name='{1}'", searchetable, searchfield));
            if (drs.Length > 0)
            {
                if (drs[0]["propertyname"] != null && drs[0]["propertyname"] != DBNull.Value)
                {
                    fieldname = drs[0]["propertyname"].ToString();
                }
            }

            return fieldname;
        }

        private static string GetTableNames(DataTable dt)
        {
            List<string> tableList = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string tname = row["searchtable"].ToString();
                tname = "'" + tname + "'";
                if (!tableList.Contains(tname))
                {
                    tableList.Add(tname);
                }
            }
            string tables = string.Join(",", tableList.ToArray());
            return tables;
        }

        private static string DealOperation(DataRow dr, string fieldname)
        {
            //key  拼操作符
            switch (dr["combflg"].ToString())
            {
                case "!=":
                    fieldname += "*NotEq";
                    break;
                case "notin":
                    fieldname += "*NotIn";
                    break;
                case "=":
                    fieldname += "*eq";
                    break;
                case ">":
                    fieldname += "*gt";
                    break;
                case ">=":
                    fieldname += "*ge";
                    break;
                case "<":
                    fieldname += "*lt";
                    break;
                case "<=":
                    fieldname += "*le";
                    break;
                case "%*%":
                    fieldname += "*like";
                    break;
                case "*%":
                    fieldname += "*LLike";
                    break;
                case "%*":
                    fieldname += "*RLike";
                    break;
                default:
                    fieldname += "*" + dr["combflg"].ToString();
                    break;
            }

            return fieldname;
        }

        private static string DealDataType(DataRow dr, string fieldname)
        {
            //拼类型 
            switch (dr["fieldtype"].ToString())
            {
                case "Date":
                    fieldname += "*date";
                    break;
                case "Byte":
                    fieldname += "*byte";
                    break;
                case "Int16":
                    fieldname += "*int16";
                    break;
                case "Int":
                    fieldname += "*int32";//pb那边用了Int表示Int32，兼容
                    break;
                case "Int64":
                    fieldname += "*int64";
                    break;
                case "Number":
                    fieldname += "*number";
                    break;
                case "Enum":
                    fieldname += "*enum";
                    break;
                default:
                    fieldname += "*str";
                    break;
            }

            return fieldname;
        }

        /// <summary>
        /// 获取默认信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public DataTable GetIndividualQueryPanelInfo(string pageid, string ocode, string logid)
        {
            return queryPanelDac.GetIndividualQueryPanelInfo(pageid, ocode, logid, true);
        }

        public int SaveQueryInfo(DataTable dtchange, DataTable dt, string pageid, string ocode, string logid)
        {
            int iret = queryPanelDac.SaveQueryInfo(dtchange, dt, pageid, ocode, logid);

            string cachedKey = new StringBuilder().Append(CACHEDID).Append("_")
                                          .Append(ocode).Append("_").Append(logid).Append("_").Append(pageid).ToString();
            string cachedTimeKey = cachedKey + "time";//时间戳缓存键

            string time = DateTime.Now.ToString("yyyyMMddhhmmss");
            NG.Cache.Client.CacheClient.Instance.Add(MAINKEY, cachedTimeKey, time);//更新外部缓存存放时间戳
            return iret;

        }

        //内嵌查询新增页面需要调用UpdateQueryCache()用来清除全部的缓存
        public void UpdateQueryCache()
        {
            NG.Cache.Client.CacheClient.Instance.Remove(MAINKEY);//清除全部的缓存
        }

        private string DeleteTableName(string field)
        {
            if (field.IndexOf(".") > 0)
            {
                return field.Split('.')[1];
            }
            else {
                return field;
            }
        }

        public Dictionary<string, string> GetLang()
        {
            Dictionary<string, string> ToolbarlangDic = SUP.Common.DataAccess.LangInfo.GetLabelLang("NGQueryPanel");
            return ToolbarlangDic;
        }

        public int RestoreDefault(string pageid, string ocode, string logid)
        {
            this.UpdateQueryCache(); //清理缓存
            return queryPanelDac.RestoreDefault(pageid, ocode, logid);
        }

    }

    /// <summary>
    /// 内嵌查询内容
    /// </summary>
    public class QueryPanelInfoEntity
    {
        
        public IList<ExtControlInfoBase> list { get; set; }

        public Dictionary<string, object> rememberstr { get; set; }

        public Dictionary<string, string> langInfo { get; set; }

        public string sortfilter { get; set; }

        public string ischeck { get; set; }

    }

}
