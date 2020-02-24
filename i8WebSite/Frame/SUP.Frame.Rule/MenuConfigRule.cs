using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using System.Xml;
using System.IO;
using System.Web.Caching;
using System.Web;
using SUP.Common.Interface;
using NG3;
using NG3.Data.Service;

namespace SUP.Frame.Rule 
{
    public class MenuConfigRule : IUserConfig
    {
        private const string UITHEME = "_ng3UITheme";

        private MenuConfigDac dac = null;   

        public MenuConfigRule()
        {
            dac = new MenuConfigDac();
        }

        public string Load(long userid)
        {
            return dac.Load(userid);
        }

        public string LoadEnFuncTreeRight()
        {
            return dac.LoadEnFuncTreeRight();
        }

        public bool Save(long userid, string masterdt)
        {
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            long phid = 0;
            if (obj == "0")
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual", "phid");
            }
            return dac.Save(masterdt, userid, phid);
        }

        public DataSet ConvertXMLToDataSet(string xmlFile)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.Load(xmlFile);

                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmld.InnerXml);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public bool SaveDockControl(int isDockControl, long userid)
        {
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            long phid = 0;
            if (obj == "0")
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual", "phid");
            }
            return dac.SaveDockControl(isDockControl, userid, phid);
        }

        public string GetDockControl(long userid)
        {
            return dac.GetDockControl(userid);
        }

        public bool SaveUITheme(int theme, long userid)
        {
            System.Web.HttpContext.Current.Session[UITHEME] = theme;//放session里面
            string cachedKey = UITHEME + userid;
            if (HttpRuntime.Cache.Get(cachedKey) != null)
            {
                HttpRuntime.Cache.Remove(cachedKey);
            }           
            //缓存起来
            HttpRuntime.Cache.Add(cachedKey,
                                      theme,
                                      null,
                                      DateTime.Now.AddDays(1),
                                      Cache.NoSlidingExpiration,
                                      CacheItemPriority.NotRemovable,
                                      null);
            //return true;
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            long phid = 0;
            if (obj == "0")
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual", "phid");
            }
            return dac.SaveUITheme(theme, userid, phid);
        }

        public string GetUITheme(long userid)
        {
            string cachedKey = UITHEME + userid;
            string theme = string.Empty;
            if (System.Web.HttpContext.Current.Session[UITHEME] != null)
            {
                theme = System.Web.HttpContext.Current.Session[UITHEME].ToString();
            }
            else {                

                if (System.Web.HttpRuntime.Cache.Get(cachedKey) != null)
                {
                    theme = System.Web.HttpRuntime.Cache.Get(cachedKey).ToString();
                }
                else {
                    theme = dac.GetUITheme(userid);
                    //缓存起来
                    HttpRuntime.Cache.Add(cachedKey,
                                              theme,
                                              null,
                                              DateTime.Now.AddDays(1),
                                              Cache.NoSlidingExpiration,
                                              CacheItemPriority.NotRemovable,
                                              null);
                }                
            }

            string str = "gray";//灰色皮肤
            switch (theme)
            {
                case "1":
                    str = "blue";//浅蓝色
                    break;           
                default:
                    str = "gray";
                    break;
            }
            return str;
        }

        public bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            List<long> phid = null;
            string sql = String.Format(@"select count(*) from fg3_mainframe_individual where userid ={0} and usertype = {1}", fromUserId, fromUserType);
            string obj = DbHelper.GetString(sql).ToString();
            int count = 0;
            int.TryParse(obj, out count);
            if (count == 0)
            {
                return true;
            }
            phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_mainframe_individual", "phid", count);
            return dac.UserConfigCopy(fromUserId, fromUserType, toUserId, toUserType, phid);
        }

        public bool DeleteUserConfig(long userid, int usertype)
        {
            return dac.UserConfigDel(userid, usertype);
        }
    }
}
