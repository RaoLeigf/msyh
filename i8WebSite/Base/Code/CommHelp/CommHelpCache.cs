using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NG3;
using System.Xml;
using NG3.Base;

namespace NG3.SUP.Base
{
    public class CommHelpCache : ConfigBase<CommHelpItem, CommHelpCache>
    {        
        protected override string ConfigCacheKey
        {
            get { return "NG2_CommHelp_CACHE"; }
        }

        protected override string ShortPath
        {
            get { return "CommHelp.xml"; }
        }

        protected override bool AllowNull
        {
            get { return false; }
        }

        protected override ConcurrentDictionary<string, CommHelpItem> CreateCacheDict()
        {
            ConcurrentDictionary<string, CommHelpItem> dict = new ConcurrentDictionary<string, CommHelpItem>(StringComparer.OrdinalIgnoreCase);
            XmlDocument doc = new XmlDocument();
            doc.Load(this.FullPath);
            var list = doc.SelectNodes("/CommonHelp/Help");
            foreach (XmlNode node in list)
            {
                string key = node.GetAttrOrDefault("id", null);
                if (key == null)
                {
                    throw new NGException("CommHelpDict.cs:/CommonHelp/Help id is null");
                }
                else if (dict.ContainsKey(key))
                {
                    throw new NGException("CommHelpDict.cs:/CommonHelp/Help id[{0}] is exist".FormatWith(key));
                }
                var v = new CommHelpItem(node);
                dict.TryAdd(key, v);
            }
            return dict;
        }
    }
}