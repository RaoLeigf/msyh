using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Text;

//using NG.UP.DataServices;
using NG3.Data;
using NG3.Data.Service;
//using NG3.Data.DB;

//using Spring.Http;
//using Spring.Util;
//using Spring.Rest;
//using Spring.Rest.Client;
using Newtonsoft.Json;

namespace SUP.NSServer
{
    /// <summary>
    /// 组织模块授权
    /// </summary>
    public class NGModuleRight
    {
        #region 单件构造
        private readonly static NGModuleRight _right = new NGModuleRight();
        /// <summary>
        /// 单件构造
        /// </summary>
        public static NGModuleRight Instance
        {
            get
            {
                return _right;
            }
        }
        #endregion

        /// <summary>
        /// A3,GE产品，是演示版也不允许使用模块
        /// </summary>
        public Hashtable HTNotUseModuleWhereDemo
        {
            //集团管理(GM)	不允许
            //网银平台(EBP)	不允许

            get
            {
                Hashtable ht = new Hashtable();

                ht.Add("17", "17");//集团财务-集团管理
                ht.Add("66", "66");//集团财务-网银平台

                return ht;
            }
        }

        ///// <summary>
        ///// 通过服务过来，主连接需指定
        ///// </summary>
        //private DataTable _ngProductsDt = null;
        //private DataTable NgProductsDt
        //{
        //    get
        //    {
        //        if (_ngProductsDt == null)
        //        {
        //            _ngProductsDt = DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString,"select * from ngproducts");
        //        }
        //        return _ngProductsDt;
        //    }
        //}

        /// <summary>
        /// 构造函数
        /// </summary>
        public NGModuleRight()
		{
        }

        /// <summary>
        /// 判断当前模块是否授权
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="validErrMsg"></param>
        /// <returns></returns>
        public bool HasRight(string moduleid, ref string validErrMsg)
        {
            ////封存组织不需要判断
            //if (i6SessionInfo.IsOrgActive == false) return true;

            return this.HasRight(NG3.AppInfoBase.PubConnectString, NG3.AppInfoBase.UCode, NG3.AppInfoBase.OCode, moduleid, ref validErrMsg);
        }
        /// <summary>
        /// 判断当前模块是否授权
        /// </summary>
        /// <param name="pubconstr"></param>
        /// <param name="ucode"></param>
        /// <param name="ocode"></param>
        /// <param name="moduleid"></param>
        /// <param name="validErrMsg"></param>
        /// <returns></returns>
        public bool HasRight(string pubconstr, string ucode, string ocode, string moduleid, ref string validErrMsg)
        {
            validErrMsg = string.Empty;

            if (this.AllModuleIsOpen()) return true;//通狗

            //******************过滤权限判断******************
            //对部分moduleid做特殊处理
            string strModuleid = this.GetAdjustedModuleId(moduleid);
            //不需要控制的模块，直接返回true;
            if (NGCOM.Instance.HtNoControlModules.ContainsValue(strModuleid)) return true;
            //242-权限中心(信息权限管理),Psoft特殊处理
            if (strModuleid == "242" || strModuleid == "19015" || strModuleid == "19016" || strModuleid == "19022" || strModuleid == "19026" || strModuleid == "19033" || strModuleid == "19038" || strModuleid == "19040") return true;
            //************************************************

            string strFullModuleNo = this.GetFullModuleNoByModuleId(pubconstr, strModuleid);//套件.模块号
            if (this.IsModuleBuy(strFullModuleNo))
            {
                //已购买
                int iRightCount = this.GetModuleRightsCountByFullNo(strFullModuleNo);//获取此模块授权数
                string strModuleno = this.GetModuleNoByModuleId(pubconstr, strModuleid);//根据【ngproducts.moduleid】获取【ngproducts.moduleno】
                string sql = string.Format(@"select * from ngmodulerights where moduleno={0}", DbConvert.ToSqlString(strModuleno));
                DataTable dt = DbHelper.GetDataTable(pubconstr, sql);//获取此模块所有已授权数

                if (dt == null || dt.Rows.Count == 0)
                {
                    validErrMsg = "此模块(" + strModuleid + ")尚未授权";
                }
                else if (dt.Rows.Count > iRightCount)
                {
                    validErrMsg = "此模块已授权超过上限";
                }
                else
                {
                    //授权按组织、账套过滤
                    string sqlfilter = "ucode=" + DbConvert.ToSqlString(ucode) + " and ocode=" + DbConvert.ToSqlString(ocode);
                    DataRow[] drRights = dt.Select(sqlfilter);
                    if (drRights == null || drRights.Length == 0)
                    {
                        validErrMsg = "此模块(" + strModuleid + ")尚未授权";
                    }
                }

                if (validErrMsg != string.Empty)
                {
                    if (NGCOM.Instance.Product.ToUpper() == "I6" || NGCOM.Instance.Product.ToUpper() == "I6P")
                    {
                        return false;
                    }
                    else
                    {
                        //A3,GE产品如果授权为0等情况，再判断是否按演示版使用
                        if (this.HTNotUseModuleWhereDemo.ContainsKey(strModuleid))
                        {
                            validErrMsg = "此模块不允许使用";
                            return false;
                        }
                        else
                        {
                            validErrMsg = "此模块按演示版打开";
                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }

            }
            else
            {
                //未购买
                if (NGCOM.Instance.Product.ToUpper() == "I6" || NGCOM.Instance.Product.ToUpper() == "I6P")
                {
                    validErrMsg = "此模块未购买不允许使用";
                    return false;
                }
                else
                {
                    //未购买-A3,GE演示版判断
                    if (this.HTNotUseModuleWhereDemo.ContainsKey(strModuleid))
                    {
                        validErrMsg = "此模块为演示版且不允许使用";
                        return false;
                    }
                    else
                    {
                        validErrMsg = "此模块按演示版打开";
                        return true;
                    }
                }
            }

        }

        /// <summary>
        /// 通狗判断
        /// </summary>
        /// <returns></returns>
        public bool AllModuleIsOpen()
        {
            if (NGCOM.Instance.HREmployees == 32768)
                return true;//通狗
            else
                return false;//产品狗
        }
        /// <summary>
        /// 判断有没购买
        /// </summary>
        /// <param name="pubconstr"></param>
        /// <param name="moduleid"></param>
        /// <returns></returns>
        private bool IsModuleBuy(string strFullModuleNo)
        {
            //调用NGCOM的属性
            if (NGCOM.Instance.ModuleBuy[strFullModuleNo] != null)
            {
                return (bool)NGCOM.Instance.ModuleBuy[strFullModuleNo];
            }

            return true;
        }
        /// <summary>
        /// 获取组织模块授权数
        /// </summary>
        /// <param name="strFullModuleNo"></param>
        /// <returns></returns>
        public int GetModuleRightsCountByFullNo(string strFullModuleNo)
        {
            int iRet = 0;
            string newmoduleno = NGCOM.Instance.Product + "." + strFullModuleNo;

            try
            {
                //调用NGCOM的属性
                if (NGCOM.Instance.ModuleRightsCount[newmoduleno.ToLower()] != null)
                {
                    iRet = Convert.ToInt32(NGCOM.Instance.ModuleRightsCount[newmoduleno.ToLower()]);
                }
            }
            catch (Exception ex)
            {
            }

            return iRet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetAdjustedModuleId(string moduleid)
        {
            //******特殊处理******
            //账务中心01包括（总账01,现金中心02,客户中心09,供应商中心10）四个权限模块
            if (moduleid == "01" || moduleid == "02" || moduleid == "09" || moduleid == "10")
            {
                moduleid = "01";
            }
            //成本管理： 灵动成本，精细成本取灵动成本授权数
            if (moduleid == "19013" || moduleid == "19016" || moduleid == "19043")
            {
                moduleid = "19013";
            }
            //******

            return moduleid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pubconstr"></param>
        /// <param name="moduleid"></param>
        /// <returns></returns>
        private string GetModuleNoByModuleId(string pubconstr, string moduleid)
        {
            string sql = string.Format(@"select moduleno from ngproducts where moduleid={0} and product={1} ", DbConvert.ToSqlString(moduleid), DbConvert.ToSqlString(NGCOM.Instance.Product));
            DataTable dt = DbHelper.GetDataTable(pubconstr, sql);
            if (dt == null || dt.Rows.Count == 0)
                return string.Empty;
            else
                return Convert.ToString(dt.Rows[0]["moduleno"]);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pubconstr"></param>
        /// <param name="moduleid"></param>
        /// <returns></returns>
        private string GetFullModuleNoByModuleId(string pubconstr, string moduleid)
        {
            //套件+模块号
            string sql = string.Format(@"select * from ngproducts where moduleid={0} and product={1} ", DbConvert.ToSqlString(moduleid), DbConvert.ToSqlString(NGCOM.Instance.Product));
            DataTable dt = DbHelper.GetDataTable(pubconstr, sql);
            if (dt == null || dt.Rows.Count == 0) return string.Empty;
            string suitno = Convert.ToString(dt.Rows[0]["suitno"]);
            string moduleno = Convert.ToString(dt.Rows[0]["moduleno"]);
            string newmoduleno = string.Empty;
            if (moduleno.IndexOf(".") > 0)
            {
                //ngproducts.moduleno字段信息中已包含套件信息
                newmoduleno = moduleno;
            }
            else
            {
                newmoduleno = Convert.ToString(dt.Rows[0]["suitno"]) + "." + moduleno;
            }

            return newmoduleno;

        }
        /// <summary>
        /// 构建不需要控制的模块过滤串
        /// </summary>
        /// <returns></returns>
        public string GetNotRightControlModuleFilterString()
        {
            string strReturn = string.Empty;

            foreach (System.Collections.DictionaryEntry item in NGCOM.Instance.HtNoControlModules)
            {
                strReturn += "'" + item.Value + "',";
            }

            strReturn = "(" + strReturn.Substring(0, strReturn.Length - 1) + ")";

            return strReturn;
        }


        ///// <summary>
        ///// 获取组织模块授权数
        ///// </summary>
        ///// <param name="pubconstr"></param>
        ///// <param name="moduleno"></param>
        ///// <returns></returns>
        //public int GetModuleRightsCount(string pubconstr, string moduleno)
        //{
        //    int iRet = 0;
        //    string newmoduleno = string.Empty;

        //    //***********生成产品+套件+模块号
        //    string sql = string.Format(@"select * from ngproducts where moduleno={0} and product={1} ", DbConvert.ToSqlString(moduleno), DbConvert.ToSqlString(NGCOM.Instance.Product));
        //    DataTable dt = DbHelper.GetDataTable(pubconstr, sql);
        //    if (dt == null || dt.Rows.Count == 0) return 0;
        //    if (moduleno.IndexOf(".") > 0)
        //    {
        //        //ngproducts.moduleno字段信息中已包含套件信息
        //        newmoduleno = Convert.ToString(dt.Rows[0]["product"]) + "." + moduleno;
        //    }
        //    else
        //    {
        //        newmoduleno = Convert.ToString(dt.Rows[0]["product"]) + "." + Convert.ToString(dt.Rows[0]["suitno"]) + "." + moduleno;
        //    }
        //    //********************

        //    try
        //    {
        //        //调用NGCOM的属性
        //        if (NGCOM.Instance.ModuleRightsCount[newmoduleno.ToLower()] != null)
        //        {
        //            iRet = Convert.ToInt32(NGCOM.Instance.ModuleRightsCount[newmoduleno.ToLower()]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return iRet;
        //}


    }

}
