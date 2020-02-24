using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Data.Service;

namespace SUP.Frame.DataAccess
{
    /// <summary>
    /// 多语言帮助类
    /// </summary>
    public class LangInfo
    {
        /// <summary>
        /// 语言（zh-CN）
        /// </summary>
        private static string _lang
        {
            get
            {
                return
                    System.Web.HttpContext.Current.Session["ClientLanguage"] == null
                        ? "zh-CN" : System.Web.HttpContext.Current.Session["ClientLanguage"].ToString();
            }
        }

        /// <summary>
        /// 多语言数据字典
        /// </summary>
        private static Dictionary<Int64, LangModel> _dicLangs = new Dictionary<Int64, LangModel>();

        /// <summary>
        /// 多语言数据字典UI与语言映射
        /// </summary>
        private static Dictionary<string, List<string>> _dicUILabels = new Dictionary<string, List<string>>();

        /// <summary>
        /// Tip别名与语言映射
        /// </summary>
        private static Dictionary<string, Int64> _dictTipAlias = new Dictionary<string, long>();

        /// <summary>
        /// 启用多语言信息
        /// </summary>
        private static DataTable _dtUseLang;

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <param name="langAlias">多语言别名</param>
        /// <returns>Lang,LangSln</returns>
        public static SubLang GetTipLang(string langAlias)
        {
            Int64 phid = 0;

            if (_dictTipAlias.ContainsKey(langAlias))
                phid = _dictTipAlias[langAlias];
            else
            {
                var sphid =
                    DbHelper.GetString(string.Format("select lang_phid from ng3_busi_tip where lang_alias = '{0}'",
                        langAlias));

                phid = Convert.ToInt64(sphid);
                _dictTipAlias.Add(langAlias, phid);
            }

            if (phid == 0)
                return new SubLang()
                {
                    Lang = string.Empty,
                    LangSln = string.Empty
                };

            Console.WriteLine(phid);
            return GetTipLang(phid, langAlias);
        }

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <param name="langId">多语言Id</param>
        /// <param name="langAlias">别名</param>
        /// <returns>Lang,LangSln</returns>
        public static SubLang GetTipLang(Int64 langId, string langAlias = "")
        {
            var lang = new SubLang()
            {
                LangId = langId,
                Lang = string.Empty,
                LangSln = string.Empty,
                LangAlias = langAlias
            };

            if (!_dicLangs.ContainsKey(langId))
            {
                if (_dtUseLang == null) //获取启用语言包
                    _dtUseLang = DbHelper.GetDataTable("select * from ng3_use_lang");

                DataTable dt;
                if (_dtUseLang.Rows.Count == 0) //没有语言包则只读取中文和英文
                {
                    dt =
                        DbHelper.GetDataTable(
                            "select phid,isuse,standard_lang,standard_lang_sln,zh_lang,zh_lang_sln,en_lang,en_lang_sln from ng3_lang where phid = " +
                            langId);
                }
                else
                {
                    //存在配置的语言包则读取中文、英文和其他语言包
                    var fields = _dtUseLang.Rows.Cast<DataRow>().Aggregate(string.Empty,
                        (current, row) => current + string.Format("," + "lang{0},lang{0}_sln", row["phid"]));

                    dt =
                        DbHelper.GetDataTable(string.Format(
                            "select phid,isuse,standard_lang,standard_lang_sln,zh_lang,zh_lang_sln,en_lang,en_lang_sln,{0} from ng3_lang where phid = {1}",
                            fields.Substring(1), langId));
                }

                if (dt.Rows.Count == 1)
                {
                    //存在数据，则将此数据添加静态变量中
                    var model = new LangModel
                    {
                        PhId = Convert.ToInt64(dt.Rows[0]["phid"]),
                        StandardLang = dt.Rows[0]["standard_lang"].ToString(),
                        StandardLangSln = dt.Rows[0]["standard_lang_sln"].ToString(),
                        ZhLang = dt.Rows[0]["zh_lang"].ToString(),
                        ZhLangSln = dt.Rows[0]["zh_lang_sln"].ToString(),
                        EnLang = dt.Rows[0]["en_lang"].ToString(),
                        EnLangSln = dt.Rows[0]["en_lang_sln"].ToString(),
                    };

                    var dic = new Dictionary<string, string>();
                    foreach (DataRow row in _dtUseLang.Rows)
                    {
                        var field = "lang" + row["phid"];
                        var clang = dt.Rows[0][field].ToString();
                        dic.Add(field, string.IsNullOrWhiteSpace(clang) ? model.StandardLang : clang);

                        field += "_sln";
                        clang = dt.Rows[0][field].ToString();
                        dic.Add(field, string.IsNullOrWhiteSpace(clang) ? model.StandardLangSln : clang);
                    }

                    model.DicLangs = dic;

                    _dicLangs.Add(langId, model);

                    if (Convert.ToInt16(dt.Rows[0]["isuse"]) == 0)
                    {
                        DbHelper.Open();
                        DbHelper.ExecuteNonQuery("update ng3_lang set isuse = 1 where phid =" + langId);
                        DbHelper.Close();
                    }
                }
            }

            var langModel = _dicLangs[langId];

            if (langModel == null)
            {
                langModel = new LangModel { PhId = langId };
            }

            if (langModel.UseCount < Int64.MaxValue - 1)
                langModel.UseCount++;
            else
                langModel.UseCount = 0;

            if (_lang == "zh-CN") //中文
            {
                lang.Lang = string.IsNullOrWhiteSpace(langModel.ZhLang) ? langModel.StandardLang : langModel.ZhLang;
                lang.LangSln = string.IsNullOrWhiteSpace(langModel.ZhLangSln)
                    ? langModel.StandardLangSln
                    : langModel.ZhLangSln;
            }
            else if (_lang == "en-US") //英文
            {
                lang.Lang = string.IsNullOrWhiteSpace(langModel.EnLang) ? langModel.StandardLang : langModel.EnLang;
                lang.LangSln = string.IsNullOrWhiteSpace(langModel.EnLangSln)
                    ? langModel.StandardLangSln
                    : langModel.EnLangSln;
            }
            else
            {
                var rows = _dtUseLang.Select(string.Format("lang_key = '{0}'", _lang));

                if (rows.Length > 0)
                {
                    var field = "lang" + rows[0]["phid"];
                    lang.Lang = string.IsNullOrWhiteSpace(langModel.DicLangs[field])
                        ? langModel.StandardLang
                        : langModel.DicLangs[field];
                    ;
                    lang.LangSln = string.IsNullOrWhiteSpace(langModel.DicLangs[field + "_sln"])
                        ? langModel.StandardLangSln
                        : langModel.DicLangs[field + "_sln"];
                    ;
                }
            }

            return lang;
        }

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <param name="busiType">ui元素业务类型</param>
        /// <returns>Lang,LangSln</returns>
        public static Dictionary<string, string> GetLabelLang(string busiType)
        {
            var dicLabel = new Dictionary<string, string>();

            if (!_dicUILabels.ContainsKey(busiType))
            {
                if (_dtUseLang == null) //获取启用语言包
                    _dtUseLang = DbHelper.GetDataTable("select * from ng3_use_lang");

                var fields = _dtUseLang.Rows.Cast<DataRow>().Aggregate(string.Empty,
                    (current, row) => current + string.Format("," + "lang{0}", row["phid"]));

                var dtLang = DbHelper.GetDataTable(
                    string.Format(@"SELECT ng3_ui_lable.ui_identify, lang_alias, ng3_lang.phid, isuse, standard_lang, zh_lang, en_lang {0} 
                                FROM ng3_lang LEFT OUTER JOIN ng3_ui_lable ON ng3_lang.phid = ng3_ui_lable.lang_phid 
                                WHERE ui_identify = '{1}'", fields, busiType));

                if (dtLang == null || dtLang.Rows.Count == 0)
                {
                    _dicUILabels.Add(busiType, new List<string>());
                    return new Dictionary<string, string>();
                }
                var langAlias = new List<string>();
                foreach (DataRow dr in dtLang.Rows)
                {
                    var alias = dr["lang_alias"].ToString();
                    var langId = Convert.ToInt64(dr["phid"]);
                    langAlias.Add(alias + "$" + langId);

                    if (_dicLangs.ContainsKey(langId))
                        continue;

                    //存在数据，则将此数据添加静态变量中
                    var model = new LangModel
                    {
                        PhId = langId,
                        StandardLang = dr["standard_lang"].ToString(),
                        ZhLang = dr["zh_lang"].ToString(),
                        EnLang = dr["en_lang"].ToString(),
                        LangAlias = alias,
                    };

                    var dic = new Dictionary<string, string>();
                    foreach (DataRow row in _dtUseLang.Rows)
                    {
                        var field = "lang" + row["phid"];
                        var clang = dr[field].ToString();
                        dic.Add(field, string.IsNullOrWhiteSpace(clang) ? model.StandardLang : clang);
                    }

                    model.DicLangs = dic;

                    _dicLangs.Add(langId, model);

                    if (Convert.ToInt16(dr["isuse"]) == 0)
                    {
                        DbHelper.Open();
                        DbHelper.ExecuteNonQuery("update ng3_lang set isuse = 1 where phid =" + dr["phid"]);
                        DbHelper.Close();
                    }
                }

                _dicUILabels.Add(busiType, langAlias);
            }

            var lblLangs = _dicUILabels[busiType];

            if (lblLangs == null)
            {
                lblLangs = new List<string>();
            }

            if (_lang == "zh-CN" || _lang == "en-US") //中文或者英文
            {
                foreach (var id in lblLangs)
                {
                    var aliasAndId = id.Split('$');
                    var alias = aliasAndId[0];
                    var langId = Convert.ToInt64(aliasAndId[1]);

                    var item = _dicLangs[langId];
                    if (item == null)
                        continue;

                    if (item.UseCount < Int64.MaxValue - 1)
                        item.UseCount++;
                    else
                        item.UseCount = 1;

                    if (_lang == "zh-CN")
                    {
                        if (!string.IsNullOrWhiteSpace(item.ZhLang))
                            dicLabel.Add(alias, item.ZhLang);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(item.EnLang))
                            dicLabel.Add(alias, item.EnLang);
                    }
                }
            }
            else
            {
                var rows = _dtUseLang.Select(string.Format("lang_key = '{0}'", _lang));

                if (rows.Length > 0)
                {
                    var field = "lang" + rows[0]["phid"];
                    foreach (var id in lblLangs)
                    {
                        var aliasAndId = id.Split('$');
                        var alias = aliasAndId[0];
                        var langId = Convert.ToInt64(aliasAndId[1]);

                        var item = _dicLangs[langId];
                        if (item == null)
                            continue;

                        if (item.UseCount < Int64.MaxValue - 1)
                            item.UseCount++;
                        else
                            item.UseCount = 1;

                        if (!string.IsNullOrWhiteSpace(item.DicLangs[field]))
                            dicLabel.Add(alias, item.DicLangs[field]);
                    }
                }
            }

            return dicLabel;
        }

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <param name="busiType">ui元素业务类型</param>
        /// <returns>Lang,LangSln</returns>
        public static Dictionary<string, string> GetLabelLangWithConn(string language, string conn,string busiType)
        {
           
            var dicLabel = new Dictionary<string, string>();

            if (!_dicUILabels.ContainsKey(busiType))
            {
                if (_dtUseLang == null) //获取启用语言包
                    _dtUseLang = DbHelper.GetDataTable(conn,"select * from ng3_use_lang");

                var fields = _dtUseLang.Rows.Cast<DataRow>().Aggregate(string.Empty,
                    (current, row) => current + string.Format("," + "lang{0}", row["phid"]));

                string test = string.Format(@"SELECT ng3_ui_lable.ui_identify, lang_alias, ng3_lang.phid, isuse, standard_lang, zh_lang, en_lang {0} 
                                FROM ng3_lang LEFT OUTER JOIN ng3_ui_lable ON ng3_lang.phid = ng3_ui_lable.lang_phid 
                                WHERE ui_identify = '{1}'", fields, busiType);

                var dtLang = DbHelper.GetDataTable(conn,
                    string.Format(@"SELECT ng3_ui_lable.ui_identify, lang_alias, ng3_lang.phid, isuse, standard_lang, zh_lang, en_lang {0} 
                                FROM ng3_lang LEFT OUTER JOIN ng3_ui_lable ON ng3_lang.phid = ng3_ui_lable.lang_phid 
                                WHERE ui_identify = '{1}'", fields, busiType));

                if (dtLang == null || dtLang.Rows.Count == 0)
                {
                    _dicUILabels.Add(busiType, new List<string>());
                    return new Dictionary<string, string>();
                }
                var langAlias = new List<string>();
                foreach (DataRow dr in dtLang.Rows)
                {
                    var alias = dr["lang_alias"].ToString();
                    var langId = Convert.ToInt64(dr["phid"]);
                    langAlias.Add(alias + "$" + langId);

                    if (_dicLangs.ContainsKey(langId))
                        continue;

                    //存在数据，则将此数据添加静态变量中
                    var model = new LangModel
                    {
                        PhId = langId,
                        StandardLang = dr["standard_lang"].ToString(),
                        ZhLang = dr["zh_lang"].ToString(),
                        EnLang = dr["en_lang"].ToString(),
                        LangAlias = alias,
                    };

                    var dic = new Dictionary<string, string>();
                    foreach (DataRow row in _dtUseLang.Rows)
                    {
                        var field = "lang" + row["phid"];
                        var clang = dr[field].ToString();
                        dic.Add(field, string.IsNullOrWhiteSpace(clang) ? model.StandardLang : clang);
                    }

                    model.DicLangs = dic;

                    _dicLangs.Add(langId, model);

                    if (Convert.ToInt16(dr["isuse"]) == 0)
                    {
                        DbHelper.Open(conn);
                        DbHelper.ExecuteNonQuery(conn,"update ng3_lang set isuse = 1 where phid =" + dr["phid"]);
                        DbHelper.Close(conn);
                    }
                }

                _dicUILabels.Add(busiType, langAlias);
            }

            var lblLangs = _dicUILabels[busiType];

            if (lblLangs == null)
            {
                lblLangs = new List<string>();
            }

            if (language == "zh-CN" || language == "en-US") //中文或者英文
            {
                foreach (var id in lblLangs)
                {
                    var aliasAndId = id.Split('$');
                    var alias = aliasAndId[0];
                    var langId = Convert.ToInt64(aliasAndId[1]);

                    var item = _dicLangs[langId];
                    if (item == null)
                        continue;

                    if (item.UseCount < Int64.MaxValue - 1)
                        item.UseCount++;
                    else
                        item.UseCount = 1;

                    if (language == "zh-CN")
                    {
                        if (!string.IsNullOrWhiteSpace(item.ZhLang))
                            dicLabel.Add(alias, item.ZhLang);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(item.EnLang))
                            dicLabel.Add(alias, item.EnLang);
                    }
                }
            }
            else
            {
                var rows = _dtUseLang.Select(string.Format("lang_key = '{0}'", language));

                if (rows.Length > 0)
                {
                    var field = "lang" + rows[0]["phid"];
                    foreach (var id in lblLangs)
                    {
                        var aliasAndId = id.Split('$');
                        var alias = aliasAndId[0];
                        var langId = Convert.ToInt64(aliasAndId[1]);

                        var item = _dicLangs[langId];
                        if (item == null)
                            continue;

                        if (item.UseCount < Int64.MaxValue - 1)
                            item.UseCount++;
                        else
                            item.UseCount = 1;

                        if (!string.IsNullOrWhiteSpace(item.DicLangs[field]))
                            dicLabel.Add(alias, item.DicLangs[field]);
                    }
                }
            }

            return dicLabel;
        }

        /// <summary>
        /// 移除业务类型的多语言数据
        /// </summary>
        /// <param name="busiType">业务类型</param>
        public static void RemoveLangByUiBusiType(string busiType)
        {
            if (string.IsNullOrWhiteSpace(busiType)) return;

            if (_dicUILabels.ContainsKey(busiType))
            {
                var list = _dicUILabels[busiType];

                var langIds = list.Select(item => Convert.ToInt64(item.Split('$')[1])).ToList();

                RemoveLang(langIds);
                _dicUILabels.Remove(busiType);
            }
        }

        /// <summary>
        /// 移除提示的多语言数据
        /// </summary>
        /// <param name="langAlias">多语言别名</param>
        public static void RemoveLangByTipAlias(string langAlias)
        {
            var phid =
                DbHelper.GetString(string.Format("select lang_phid from ng3_busi_tip where lang_alias = '{0}'",
                    langAlias));

            if (string.IsNullOrWhiteSpace(phid)) return;

            if (_dicLangs.ContainsKey(Convert.ToInt64(phid)))
                _dicLangs.Remove(Convert.ToInt64(phid));
        }

        /// <summary>
        /// 移除提示的多语言数据
        /// </summary>
        /// <param name="langId">语言Id</param>
        public static void RemoveLang(Int64 langId)
        {
            RemoveLang(new List<Int64> { langId });
        }

        /// <summary>
        /// 移除提示的多语言数据
        /// </summary>
        /// <param name="langIds">语言Id集合</param>
        public static void RemoveLang(List<Int64> langIds)
        {
            if (langIds == null || langIds.Count == 0) return;

            foreach (var id in langIds.Where(id => _dicLangs.ContainsKey(id)))
            {
                _dicLangs.Remove(id);
            }
        }

        /// <summary>
        /// 语言包子类用于传递值
        /// </summary>
        public class SubLang
        {
            /// <summary>
            /// 语言Id
            /// </summary>
            public Int64 LangId { get; set; }

            /// <summary>
            /// 别名
            /// </summary>
            public string LangAlias { get; set; }

            /// <summary>
            /// 相应语言
            /// </summary>
            public string Lang { get; set; }

            /// <summary>
            /// 解决方案
            /// </summary>
            public string LangSln { get; set; }
        }

        /// <summary>
        /// 语言Model
        /// </summary>
        public class LangModel
        {
            /// <summary>
            /// 语言Id
            /// </summary>
            public virtual Int64 PhId { get; set; }

            /// <summary>
            /// 别名
            /// </summary>
            public virtual String LangAlias { get; set; }

            /// <summary>
            /// 标准中文提示
            /// </summary>
            public virtual String StandardLang { get; set; }

            /// <summary>
            /// 标准中文提示
            /// </summary>
            public virtual String StandardLangSln { get; set; }

            /// <summary>
            /// 中文提示
            /// </summary>
            public virtual String ZhLang { get; set; }

            /// <summary>
            /// 中文解决方案
            /// </summary>
            public virtual String ZhLangSln { get; set; }

            /// <summary>
            /// 英文提示
            /// </summary>
            public virtual String EnLang { get; set; }

            /// <summary>
            /// 英文解决方案
            /// </summary>
            public virtual String EnLangSln { get; set; }

            /// <summary>
            /// 其他语言
            /// </summary>
            public virtual Dictionary<string, string> DicLangs { get; set; }

            /// <summary>
            /// 使用次数
            /// </summary>
            public virtual Int64 UseCount { get; set; }
        }
    }
}