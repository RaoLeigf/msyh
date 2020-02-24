using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using NG3.Data.Service;
using NG3.Exceptions;
using NG3.MemCached.Client;
using NG3.Cache.Interface;
using System.Web;
using System.IO;
using NG3.Cache.Client;

namespace SUP.Right.Rules
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class Services
    {

        private const string PRODUCTFILE = "PRODUCTFILE"; 

        /// <summary>
        /// 功能权限新增
        /// </summary>
        /// <param name="entity">功能权限涉及的三张表需要填写的信息</param>
        /// <returns></returns>
        public bool RightAdd(NG3.Interface.IRightEntity entity)
        {
            string sqlfg_rights = @"
            insert into fg_rights(rightid,moduleno,rightname,alertflg,secflg,a3flg,i6flg,geflg,w3flg,parentright)
            values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
            sqlfg_rights = string.Format(sqlfg_rights, entity.rightid, entity.moduleno, entity.rightname, entity.alertflg, entity.secflg, entity.a3flg, entity.i6flg, entity.geflg, entity.parentright);

            string sqlfg_rgtfunc = @"
            insert into fg_rgtfunc(rightid,moduleno,pagename,buttonname,funcname,rightarr)
            values ('{0}','{1}','{2}','{3}','{4}','{5}')";
            sqlfg_rgtfunc = string.Format(sqlfg_rgtfunc, entity.rightid, entity.moduleno, entity.pagename, entity.buttonname, entity.funcname, entity.rightarr);

            string sqlfg_menu = @"
            insert into fg_menu(code,id,pid,name,product,suite,seq,moduleno,rightid)
            values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
            sqlfg_menu = string.Format(sqlfg_menu, Guid.NewGuid().ToString(), entity.id, entity.pid, entity.rightname, entity.product, entity.suite, entity.seq, entity.moduleno, entity.rightid);

            try
            {
                NG3.Data.Service.DbHelper.BeginTran();
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_rights) == 0) {
                    throw new Exception("insert right data into fg_rights failed");
                }
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_rgtfunc) == 0)
                {
                    throw new Exception("insert right data into fg_rgtfunc failed");
                }
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_menu) == 0)
                {
                    throw new Exception("insert right data into fg_menu failed");
                }
                return true;
            }
            catch (Exception ex)
            {
                //NG3.Log.Log4Net.ILogger logger = (new NG3.Log.Log4Net.Log4NetLoggerFactory()).CreateLogger(typeof(NoLoginException), NG3.Log.Log4Net.LogType.authorization);
                //logger.Info(ex.Message, ex);
                NG3.Data.Service.DbHelper.RollbackTran();
                return false;
            }
            finally
            {
                NG3.Data.Service.DbHelper.CommitTran();
            }
        }

        /// <summary>
        /// 功能权限更新
        /// </summary>
        /// <param name="entity">功能权限涉及的三张表需要填写的信息</param>
        /// <returns></returns>
        public bool RightUpdate(NG3.Interface.IRightEntity entity)
        {
            string sqlfg_rights = @"
            insert into fg_rights(rightid,moduleno,rightname,alertflg,secflg,a3flg,i6flg,geflg,w3flg,parentright)
            values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
            sqlfg_rights = string.Format(sqlfg_rights, entity.rightid, entity.moduleno, entity.rightname, entity.alertflg, entity.secflg, entity.a3flg, entity.i6flg, entity.geflg, entity.parentright);

            string sqlfg_rgtfunc = @"
            insert into fg_rgtfunc(rightid,moduleno,pagename,buttonname,funcname,rightarr)
            values ('{0}','{1}','{2}','{3}','{4}','{5}')";
            sqlfg_rgtfunc = string.Format(sqlfg_rgtfunc, entity.rightid, entity.moduleno, entity.pagename, entity.buttonname, entity.funcname, entity.rightarr);

            string sqlfg_menu = @"
            insert into fg_menu(code,id,pid,name,product,suite,seq,moduleno,rightid)
            values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
            sqlfg_menu = string.Format(sqlfg_menu, Guid.NewGuid().ToString(), entity.id, entity.pid, entity.rightname, entity.product, entity.suite, entity.seq, entity.moduleno, entity.rightid);

            try
            {
                NG3.Data.Service.DbHelper.BeginTran();
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_rights) < 0)
                {
                    throw new Exception("insert right data into fg_rights failed");
                }
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_rgtfunc) < 0)
                {
                    throw new Exception("insert right data into fg_rgtfunc failed");
                }
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_menu) < 0)
                {
                    throw new Exception("insert right data into fg_menu failed");
                }
                return true;
            }
            catch (Exception ex)
            {
                //NG3.Log.Log4Net.ILogger logger = (new NG3.Log.Log4Net.Log4NetLoggerFactory()).CreateLogger(typeof(NoLoginException), NG3.Log.Log4Net.LogType.authorization);
                //logger.Info(ex.Message, ex);
                NG3.Data.Service.DbHelper.RollbackTran();
                return false;
            }
            finally
            {
                NG3.Data.Service.DbHelper.CommitTran();
            }
        }
        /// <summary>
        /// 功能权限删除
        /// </summary>
        /// <param name="entity">功能权限涉及的三张表需要填写的信息</param>
        /// <returns></returns>
        public bool RightDelete(NG3.Interface.IRightEntity entity)
        {
            string sqlfg_rights = @"
            delete from fg_rights where rightid = '{0}' and moduleno = '{1}'";
            sqlfg_rights = string.Format(sqlfg_rights, entity.rightid, entity.moduleno);

            string sqlfg_rgtfunc = @"
            delete from  fg_rgtfunc where rightid = '{0}' and moduleno = '{1}'";
            sqlfg_rgtfunc = string.Format(sqlfg_rgtfunc, entity.rightid, entity.moduleno);

            string sqlfg_menu = @"
            delete from fg_menu where rightid = '{8}' and moduleno = '{7}'";
            sqlfg_menu = string.Format(sqlfg_menu,entity.moduleno, entity.rightid);

            try
            {
                NG3.Data.Service.DbHelper.BeginTran();
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_rights) < 0)
                {
                    throw new Exception("insert right data into fg_rights failed");
                }
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_rgtfunc) < 0)
                {
                    throw new Exception("insert right data into fg_rgtfunc failed");
                }
                if (NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlfg_menu) < 0)
                {
                    throw new Exception("insert right data into fg_menu failed");
                }
                return true;
            }
            catch (Exception ex)
            {
                //NG3.Log.Log4Net.ILogger logger = (new NG3.Log.Log4Net.Log4NetLoggerFactory()).CreateLogger(typeof(NoLoginException), NG3.Log.Log4Net.LogType.authorization);
                //logger.Info(ex.Message, ex);
                NG3.Data.Service.DbHelper.RollbackTran();
                return false;
            }
            finally
            {
                NG3.Data.Service.DbHelper.CommitTran();
            }
        }


        /// <summary>
        /// 判断是否有权限:窗口的入口权限
        /// </summary>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <param name="rightname"></param>
        /// <returns></returns>
        public bool IsHaveRight(string ocode, string logid, string rightname, string funcname)
        {
            bool haveright = false;

            //用户的页面权限
            DataTable dtUserPageRight = this.GetAllPageRight(ocode, logid);
            //所有页面权限
            DataTable dtAllPageRight = this.GetAllPageRight();

            DataRow[] dr = null;

            if (string.IsNullOrEmpty(funcname))
            {
                dr = dtAllPageRight.Select("pagename='" + rightname + "' and (funcname='' or funcname is null)");
            }
            else
            {
                dr = dtAllPageRight.Select("pagename='" + rightname + "' and funcname='" + funcname + "'");
            }

            if (dr.Length == 0) return true;//不需要控制权限

            string module = dr[0]["moduleno"].ToString();//模块号
            string rightid = dr[0]["rightid"].ToString();//权限号

            //取权限号,计算偏移量
            int index = Convert.ToInt32(rightid);

            int offset = (index - 1) / 1000;//取整,修正那些权限号超过1000的
            int realoffset = index - (offset * 1000);//真正的下标值

            //取权限串(secuserrgt.Righttr字段)的值
            DataRow[] userdr = dtUserPageRight.Select("moduleno='" + module + "' and right_offset='" + offset + "'");

            if (userdr.Length > 0)
            {
                for (int i = 0; i < userdr.Length; i++)
                {
                    StringBuilder Righttr = new StringBuilder();
                    Righttr.Append(userdr[i]["Righttr"].ToString());
                    Righttr.Append(userdr[i]["Righttr1"].ToString());
                    Righttr.Append(userdr[i]["Righttr2"].ToString());
                    Righttr.Append(userdr[i]["Righttr3"].ToString());

                    //计算权限串下标为index的值
                    char[] c = Righttr.ToString().ToCharArray();

                    if (realoffset <= c.Length)
                    {
                        if (c[realoffset - 1] == '1')
                        {
                            haveright = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                haveright = false;
            }

            return haveright;
        }

        /// <summary>
        /// 判断按钮是否有权限
        /// </summary>
        /// <param name="ocode">组织号</param>
        /// <param name="logid">操作员</param>
        /// <param name="rightname">权限名</param>
        /// <param name="buttonname">按钮名</param>
        /// <param name="funcname">功能名</param>
        /// <returns></returns>
        public bool IsButtonHaveRight(string ocode, string logid, string rightname, string buttonname)
        {
            string sql = string.Empty;
            sql = string.Format("select rightid,moduleno from fg_rgtfunc where pagename = '{0}' and buttonname ='{1}'", rightname, buttonname);

            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                bool have = this.UserHasRight(ocode, logid, dr["moduleno"].ToString(), dr["rightid"].ToString(), false);
                return have;
            }
            else
            {
                return true;//取不到权限，表示不控制
            }
        }

        public bool IsButtonHaveRight(string ocode, string logid, string rightname, string buttonname, string funcname)
        {
            string sql = string.Empty;
            sql = string.Format("select rightid,moduleno from fg_rgtfunc where pagename = '{0}' and buttonname ='{1}' and funcname = '{2}'", rightname, buttonname, funcname);

            //DataTable dt = this.Sqlca.GetDataTable(sql);//changed by jin 2012-05-21
            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                bool have = this.UserHasRight(ocode, logid, dr["moduleno"].ToString(), dr["rightid"].ToString(), false);
                return have;
            }
            else
            {
                return true;//取不到权限，表示不控制
            }
        }

        /// <summary>
        /// 用户是否有权限
        /// </summary>
        /// <param name="organizeId">组织代码</param>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleNo">模块号</param>
        /// <param name="rightId">权限ID</param>
        /// <param name="needFilterActor">是否需要过滤角色－需要硬件身份验证</param>
        /// <returns></returns>
        public bool UserHasRight(string organizeId, string userId, string moduleNo, string rightId, bool needFilterActor)
        {

            string UCode = "";
            if (null == System.Web.HttpContext.Current.Session["UCode"])
                return false;
            UCode = System.Web.HttpContext.Current.Session["UCode"].ToString();
            System.Diagnostics.Debug.WriteLine("Aorgcode=" + organizeId + " aUserID=" + userId + " AModuleNo=" + moduleNo + " ARightID=" + rightId);

            return HasRight(UCode, organizeId, userId, moduleNo, rightId, needFilterActor);
        }


        /// <param name="UCode">帐套号</param>
        /// <param name="organizeId">组织号</param>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleNo">模块号</param>
        /// <param name="rightId">权限</param>
        /// <param name="needFilterActor">是否需要过滤角色－需要硬件身份验证</param>
        /// <returns></returns>
        public bool HasRight(string UCode, string organizeId, string userId, string moduleNo, string rightId, bool needFilterActor)
        {


            if (string.IsNullOrEmpty(organizeId))
                return false;
            if (string.IsNullOrEmpty(userId))
                return false;
            if (string.IsNullOrEmpty(moduleNo))
                return false;
            if (string.IsNullOrEmpty(rightId))
                return false;
            string sName = GetServerName();
            string keyNoRight = sName + "-" + UCode + "-" + organizeId + "@No@" + userId;
            string keyYesRight = sName + "-" + UCode + "-" + organizeId + "@Yes@" + userId;
            string sSql = "";
            try
            {
                //removed by jin 2012-05-21 没发现这个变量定义了之后有被调用
                //i6.Biz.ProductInfo productInfo = new ProductInfo();
                DataTable dtNoRights = null;
                //收回权限
                sSql = "select * from uv_userright_cancel  where userid='" + userId + "'";
                //removed by jin 2013-05-06 尚未集成MemCache
                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                dtNoRights = client.Get<DataTable>(keyNoRight);
                if (dtNoRights == null)
                {
                dtNoRights = NG3.Data.Service.DbHelper.GetDataTable(sSql);
                client.Set(keyNoRight, dtNoRights, 300);
                }
                //end removed

                //取权限号,计算偏移量
                int index = Convert.ToInt32(rightId);
                int offset = (index - 1) / 1000;//取整,修正那些权限号超过1000的
                int realoffset = index - (offset * 1000);//真正的下标值
                string where = " moduleno='" + moduleNo + "' and right_offset='" + offset + "'";//收回权限
                DataRow[] dr = dtNoRights.Select(where);
                if (dr != null && dr.Length > 0)
                {
                    return false;
                }

                DataTable dtYesRights = null;
                //removed by jin 2013-05-06 尚未集成MemCache
                dtYesRights = client.Get<DataTable>(keyYesRight);
                if (dtYesRights == null)
                {
                    //所有授予的权限
                    sSql = "select * from uv_userright_all where ocode='" + organizeId + "'"
                        + "  and userid='" + userId + "'";

                    // {梁越平：判断是否过滤掉受限制的角色}
                    if (needFilterActor)
                    {
                        sSql = getFilterActorSqlString(organizeId, userId);//这个方法又得改,先不管
                    }

                    dtYesRights = NG3.Data.Service.DbHelper.GetDataTable(sSql);

                    client.Set(keyYesRight, dtYesRights, 300);
                }
                //end removed

                dr = dtYesRights.Select(where);
                if (dr != null && dr.Length > 0)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        //取secuserrgt.rightstr字段的值
                        string rightstr = dr[i]["rightstr"].ToString() + dr[i]["rightstr1"].ToString() + dr[i]["rightstr2"].ToString() + dr[i]["rightstr3"].ToString();

                        //计算权限串下标为index的值
                        char[] c = rightstr.ToCharArray();

                        if (c[realoffset - 1] == '1')
                        {
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //removed by jin 2012-05-21 暂时移除
                //this.LastError = ex.Message.ToString();
                return false;
            }
            return false;
        }

        /// <summary>
        /// 用户是否有权限
        /// </summary> 
        /// <param name="UCode">帐套号</param>
        /// <param name="organizeId">组织号</param>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleNo">模块号</param>
        /// <param name="rightId">权限</param>
        /// <param name="needFilterActor">是否需要过滤角色－需要硬件身份验证</param>
        /// <returns></returns>
        public bool HasRightEx(string UCode, string organizeId, string userId, string moduleNo, string rightId, bool needFilterActor, CacheClient client, DataTable dtNoRights, DataTable dtYesRights, string keyNoRight, string keyYesRight)
        {
            if (string.IsNullOrEmpty(organizeId))
                return false;
            if (string.IsNullOrEmpty(userId))
                return false;
            if (string.IsNullOrEmpty(moduleNo))
                return false;
            if (string.IsNullOrEmpty(rightId))
                return false;
            //string sName = GetServerName();
            //string keyNoRight = sName + "-" + UCode + "-" + organizeId + "@No@" + userId;
            //string keyYesRight = sName + "-" + UCode + "-" + organizeId + "@Yes@" + userId;
            string sSql = "";
            try
            {
                //收回权限
                sSql = "select * from uv_userright_cancel  where userid='" + userId + "'";

                #region MyRegion               

                //把权限缓存放到NGCacheServer中
                if (client.IsUseWebGarden)
                {
                    if (null == dtNoRights)
                    {
                        dtNoRights = DbHelper.GetDataTable(sSql);
                        client.Add(keyNoRight, dtNoRights, 5);
                    }
                }
                else
                {
                    //dtNoRights = System.Web.HttpRuntime.Cache[keyNoRight] as DataTable;
                    if (dtNoRights == null)
                    {

                        dtNoRights = DbHelper.GetDataTable(sSql);

                        System.Web.HttpRuntime.Cache.Add(keyNoRight, dtNoRights, null, DateTime.Now.AddHours(5), System.TimeSpan.Zero, System.Web.Caching.CacheItemPriority.NotRemovable, null);

                    }
                }

                #endregion
                               

                //取权限号,计算偏移量
                int index = Convert.ToInt32(rightId);
                int offset = (index - 1) / 1000;//取整,修正那些权限号超过1000的
                int realoffset = index - (offset * 1000);//真正的下标值
                string where = String.Format(" moduleno='{0}' and right_offset='{1}'", moduleNo, offset);//收回权限
                DataRow[] dr = dtNoRights.Select(where);
                if (dr != null && dr.Length > 0)
                {
                    return false;
                }

                //DataTable dtYesRights = null;

                #region cancel

                if (client.IsUseWebGarden)
                {
                    if (null == dtYesRights)
                    {
                        //所有授予的权限
                        sSql = "select * from uv_userright_all where ocode='" + organizeId + "'"
                            + "  and userid='" + userId + "'";

                        // {梁越平：判断是否过滤掉受限制的角色}
                        if (needFilterActor)
                        {
                            sSql = getFilterActorSqlString(organizeId, userId);//这个方法又得改,先不管
                        }

                        dtYesRights = DbHelper.GetDataTable(sSql);

                        //SlidingTime expireTime = new SlidingTime(TimeSpan.FromHours(1));
                        client.Add(keyYesRight, dtYesRights, 5);
                    }
                }
                else
                {
                    //dtYesRights = System.Web.HttpRuntime.Cache[keyYesRight] as DataTable;

                    if (dtYesRights == null)
                    {
                        //所有授予的权限
                        sSql = "select * from uv_userright_all where ocode='" + organizeId + "'"
                            + "  and userid='" + userId + "'";

                        // {梁越平：判断是否过滤掉受限制的角色}
                        if (needFilterActor)
                        {
                            sSql = getFilterActorSqlString(organizeId, userId);//这个方法又得改,先不管
                        }

                        dtYesRights = DbHelper.GetDataTable(sSql);

                        System.Web.HttpRuntime.Cache.Add(keyYesRight, dtYesRights, null, DateTime.Now.AddHours(5), System.TimeSpan.Zero, System.Web.Caching.CacheItemPriority.NotRemovable, null);

                    }


                }

                #endregion
                                
                dr = dtYesRights.Select(where);
                if (dr != null && dr.Length > 0)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        //取secuserrgt.rightstr字段的值
                        string rightstr = dr[i]["rightstr"].ToString() + dr[i]["rightstr1"].ToString() + dr[i]["rightstr2"].ToString() + dr[i]["rightstr3"].ToString();

                        //计算权限串下标为index的值
                        char[] c = rightstr.ToCharArray();

                        if (c[realoffset - 1] == '1')
                        {
                            return true;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string getFilterActorSqlString(string organizeId, string userId)
        { 
            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();

            sbSql.Append("select fg_useractor.ocode as ocode,fg_useractor.userid as userid,fg_actorrgts.moduleno as moduleno,fg_actorrgts.rightid as rightid ");
            sbSql.Append(" from fg_useractor,fg_actorrgts,fg_actor ");
            sbSql.Append("where fg_useractor.ocode=fg_actorrgts.ocode ");
            sbSql.Append("and fg_useractor.ocode=fg_actor.ocode ");
            sbSql.Append("and fg_useractor.actorid=fg_actor.actorid ");
            sbSql.Append("and fg_useractor.actorid=fg_actorrgts.actorid ");
            sbSql.Append("and (fg_actor.actor_opt=0 or fg_actor.actor_opt is null) and (fg_actor.usbval = '0' or fg_actor.usbval is null) ");
            sbSql.Append("and fg_useractor.ocode='" + organizeId + "' and fg_useractor.userid='" + userId + "' ");
            sbSql.Append("union ");
            sbSql.Append("select fg_groupuser.ocode as ocode,fg_groupuser.userid as userid,fg_actorrgts.moduleno as moduleno,fg_actorrgts.rightid  as rightid ");
            sbSql.Append("from fg_groupuser,fg_groupactor,fg_actorrgts,fg_actor ");
            sbSql.Append("where fg_groupuser.ocode =fg_actor.ocode ");
            sbSql.Append("and fg_groupuser.ocode =fg_groupactor.ocode ");
            sbSql.Append("and fg_groupuser.ocode =fg_actorrgts.ocode ");
            sbSql.Append("and fg_groupactor.groupid=fg_groupuser.groupid ");
            sbSql.Append("and fg_groupactor.actorid=fg_actor.actorid ");
            sbSql.Append("and fg_groupactor.actorid=fg_actorrgts.actorid ");
            sbSql.Append("and (fg_actor.actor_opt=0 or fg_actor.actor_opt is null) and (fg_actor.usbval = '0' or fg_actor.usbval is null) ");
            sbSql.Append("and fg_groupuser.ocode='" + organizeId + "' and fg_groupuser.userid='" + userId + "' ");
            sbSql.Append("union ");
            sbSql.Append("select fg_groupuser.ocode as ocode,fg_groupuser.userid as userid,fg_grouprgts.moduleno as moduleno,fg_grouprgts.rightid  as rightid ");
            sbSql.Append("from fg_groupuser,fg_grouprgts ");
            sbSql.Append("where fg_groupuser.ocode=fg_grouprgts.ocode ");
            sbSql.Append("and fg_groupuser.groupid=fg_grouprgts.groupid ");
            sbSql.Append("and fg_groupuser.ocode='" + organizeId + "' and fg_groupuser.userid='" + userId + "' ");
            sbSql.Append("union ");
            sbSql.Append("select fg_userrgts.ocode as ocode,fg_userrgts.userid as userid,fg_userrgts.moduleno as moduleno,fg_userrgts.rightid  as rightid ");
            sbSql.Append("from fg_userrgts ");
            sbSql.Append("where fg_userrgts.ocode='" + organizeId + "' and fg_userrgts.userid='" + userId + "' ");

            return sbSql.ToString();
           
        }

        /// <summary>
        /// Database name
        /// </summary>
        /// <returns></returns>
        private string GetServerName()
        {
            int iPos1, iPos2;
            //string tempValue = this.Sqlca.ConnectionString;// System.Web.HttpContext.Current.Session["UserConnectStr"].ToString();
            //changed by jin 2012-05-21
            string tempValue = ConnectionInfoService.GetSessionConnectString();
            if (NG3.Data.Service.DbHelper.Vendor == NG3.Data.DbVendor.SQLServer)
            {
                iPos1 = tempValue.IndexOf("Server", 0, tempValue.Length);
                if (iPos1 >= 0)
                {
                    iPos2 = tempValue.IndexOf(";", iPos1);
                    if (iPos2 > 0)
                    {
                        tempValue = tempValue.Substring(iPos1 + 7, iPos2 - iPos1 - 7);
                    }
                    else
                    {
                        tempValue = "error";
                    }
                }
                else
                {
                    tempValue = "error";
                }
            }
            else
            {
                iPos1 = tempValue.IndexOf("Data Source", 0, tempValue.Length);
                if (iPos1 >= 0)
                {
                    iPos2 = tempValue.IndexOf(";", iPos1);
                    if (iPos2 > 0)
                    {
                        tempValue = tempValue.Substring(iPos1 + 12, iPos2 - iPos1 - 12);
                    }
                    else
                    {
                        tempValue = "error";
                    }
                }
                else
                {
                    tempValue = "error";
                }
            }


            return tempValue;
        }


        /// <summary>
        /// 获取界面上没用权限的按钮
        /// </summary>
        /// <param name="UCode">账套</param>
        /// <param name="organizeId">组织</param>
        /// <param name="userId">账户</param>
        /// <param name="userType"></param>
        /// <param name="pageName">界面</param>
        /// <param name="FuncName">功能按钮</param>
        /// <returns></returns>
        public string GetNonFormRightsItems(string UCode, Int64 orgID, Int64 userID, string userType, string pageName, string FuncName, string connStr)
        {
            //Hashtable ht = GetFormRights(UCode, organizeId, userId, userType, pageName, FuncName);
            // i6.Biz.DMC.CPCM_Interface cpcm = new i6.Biz.DMC.CPCM_Interface(connStr);
            Enterprise3.Rights.AnalyticEngine.FuncRightControl cpcm = new Enterprise3.Rights.AnalyticEngine.FuncRightControl();
            Hashtable ht = cpcm.GetFormRights(UCode, orgID, userID, userType, pageName, FuncName);
            string ret = "";
            foreach (DictionaryEntry de in ht)
            {
                if (de.Value != null && de.Value.ToString().ToLower() == "false")
                {
                    ret = ret + de.Key.ToString()  + ",";
                }
            }
            if (ret.Length > 1) ret = ret.TrimEnd(',');
            return ret;
        }

        /// <summary>
        /// 获取界面上的按钮
        /// </summary>
        /// <param name="UCode">账套</param>
        /// <param name="organizeId">组织</param>
        /// <param name="userId">账户</param>
        /// <param name="userType"></param>
        /// <param name="pageName">界面</param>
        /// <param name="FuncName">功能按钮</param>
        /// <returns></returns>
        public Hashtable GetFormRights(string UCode, string organizeId, string userId, string userType, string pageName, string FuncName)
        {

            bool needFilterActor;
            bool isUICSystem;
            Hashtable ht;
            DataTable dt;
            SetFunc(pageName, out needFilterActor, out isUICSystem, out ht, out dt);
            if (dt == null)
                return ht;

            string moduleNo, funcName;
            if (String.Compare(userType, "SYSTEM", true) == 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    moduleNo = dr["moduleno"].ToString();
                    if (moduleNo == "241" || moduleNo == "242" || moduleNo == "245" || moduleNo == "231" || moduleNo == "118" || isUICSystem) //modify by lmq 加了数据快报
                    {
                        //{梁越平：如果是UIC系统则开发维护权限}
                        ht[dr["buttonname"].ToString()] = true;
                    }
                    else
                    {
                        ht[dr["buttonname"].ToString()] = false;
                    }
                }
            }
            else
            {
                string sName = GetServerName();

                string keyNoRight = sName + "-" + UCode + "-" + organizeId + "@No@" + userId;
                string keyYesRight = sName + "-" + UCode + "-" + organizeId + "@Yes@" + userId;

                CacheClient client = CacheClientFactory.GetCacheClient(CacheType.CommonDataCache);

                DataTable dtNoRights = null;
                DataTable dtYesRights = null;

                if (client.IsUseWebGarden)
                {
                    dtNoRights = CacheClient.Instance.GetData(keyNoRight) as DataTable;
                    dtYesRights = CacheClient.Instance.GetData(keyYesRight) as DataTable;
                }
                else
                {
                    dtNoRights = System.Web.HttpRuntime.Cache[keyNoRight] as DataTable;
                    dtYesRights = System.Web.HttpRuntime.Cache[keyYesRight] as DataTable;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    funcName = dr["funcname"].ToString();
                    if ((funcName.Length > 0 && funcName != FuncName) || (string.IsNullOrEmpty(funcName) && !string.IsNullOrEmpty(FuncName)))
                        continue;//modify by lmq 2007-7-10
                    //ht[dr["buttonname"].ToString()] = this.HasRight(UCode, organizeId, userId, dr["moduleno"].ToString(), dr["rightid"].ToString(), needFilterActor);
                    ht[dr["buttonname"].ToString()] = this.HasRightEx(UCode, organizeId, userId, dr["moduleno"].ToString(), dr["rightid"].ToString(), needFilterActor, client, dtNoRights, dtYesRights, keyNoRight, keyYesRight);
                }
            }

            return ht;

        }

        private void SetFunc(string pageName, out bool needFilterActor, out bool isUICSystem, out Hashtable ht, out DataTable dt)
        {
            string USBValString;
            needFilterActor = false;
            isUICSystem = false;

            ht = new Hashtable();

            string sSql = "select * from fg_rgtfunc where pagename=" + "'" + pageName + "'";

            // {梁越平于2005-10-21日加入}
            try
            {
                // {注意：USBValString由两节购成，节间用“，”分隔，前节标识帐套是否启用USB验证}
                // {后节用于标识本用户是否通过USB身份验证，两节的值均为true或false}
                USBValString = System.Web.HttpContext.Current.Session["FILTER_ACTOR"].ToString();
            }
            catch
            {
                USBValString = "";
            }
            if (USBValString == null)
                USBValString = "";
            if (USBValString == "1")
                needFilterActor = true;
            else
                needFilterActor = false;

            // {梁越平于2005-12-27日加入,取得服务的产品代码}
            string productName;
            try
            {
                productName = System.Web.HttpContext.Current.Session["CurProduct"].ToString();
            }
            catch
            {
                productName = "I6";
            }
            if (String.Compare(productName, "UIC", true) == 0)
            {
                isUICSystem = true;
            }

            dt = NG3.Data.Service.DbHelper.GetDataTable(sSql);
        }

        /// <summary>
        /// 根据组织号和用户id获取页面权限
        /// 根据产品过滤
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="userId"></param>
        /// <returns>DataTable</returns>
        public DataTable GetAllPageRight(string organizeId, string userId)
        {
            //取用户拥有的全部权限：包括该用户本身和他所在用户组、所属角色所拥有的权限
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select * from uv_userright_all where ");
            strsql.Append(" ocode= '" + organizeId + "'");
            strsql.Append(" and userid='" + userId + "'");

            //DataTable allrightdt = Sqlca.GetDataTable(strsql.ToString());//changed by jin 2012-05-21
            DataTable allrightdt = NG3.Data.Service.DbHelper.GetDataTable(strsql.ToString());
            if (allrightdt.Rows.Count == 0)
            {
                return allrightdt;
            }


            //取被撤销的权限数据,这个业务ms有点搞,所谓的"反权限",一旦角色的权限被取消了,就屏蔽掉
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from uv_userright_cancel where ");
            //sql.Append(" ocode= '" + organizeId + "'");//角色无组织，不需要传
            sql.Append(" userid='" + userId + "'");

            //DataTable canceldt = this.Sqlca.GetDataTable(sql.ToString());//changed by jin 2012-05-21
            DataTable canceldt = NG3.Data.Service.DbHelper.GetDataTable(sql.ToString());
            //DataTable moduledt = GetModule(); //模块信息 Delete by xnb

            DataRow[] drs = allrightdt.Select();
            int count = drs.Length;
            for (int i = 0; i < count; i++)
            {
                //处理收回权限
                DataRow[] canceldr = canceldt.Select("moduleno ='" + drs[i]["moduleno"].ToString() + "'");
                if (canceldr.Length > 0)
                {
                    string Righttr = drs[i]["Righttr"].ToString();
                    string cancelRighttr = canceldr[0]["Righttr"].ToString();

                    if (cancelRighttr.IndexOf("1") >= 0)
                    {
                        drs[i]["Righttr"] = ShieldRight(Righttr, cancelRighttr);
                    }

                    string Righttr1 = drs[i]["Righttr1"].ToString();
                    string cancelRighttr1 = canceldr[0]["Righttr1"].ToString();
                    if (cancelRighttr1.IndexOf("1") >= 0)
                    {
                        drs[i]["Righttr1"] = ShieldRight(Righttr1, cancelRighttr1);
                    }

                    string Righttr2 = drs[i]["Righttr2"].ToString();
                    string cancelRighttr2 = canceldr[0]["Righttr2"].ToString();
                    if (cancelRighttr2.IndexOf("1") >= 0)
                    {
                        drs[i]["Righttr2"] = ShieldRight(Righttr2, cancelRighttr2);
                    }

                    string Righttr3 = drs[i]["Righttr3"].ToString();
                    string cancelRighttr3 = canceldr[0]["Righttr3"].ToString();
                    if (cancelRighttr3.IndexOf("1") >= 0)
                    {
                        drs[i]["Righttr3"] = ShieldRight(Righttr3, cancelRighttr3);
                    }
                }
            }

            allrightdt.AcceptChanges();
            return allrightdt;
        }

        /// <summary>
        /// 获取所有的页面权限
        /// 根据产品过滤
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllPageRight()
        {
            string sSql = "select pagename,rightid, moduleno,lineid,funcname from fg_rgtfunc where pagename = buttonname";
            //string sSql = "select pagename,rightid, moduleno,funcname from fg_rgtfunc where pagename = buttonname and moduleno='142' and pagename='ReportFrameSet'";
            // DataTable dt = this.Sqlca.GetDataTable(sSql); //changed by jin 2012-05-21
            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(sSql);
            if (dt.Rows.Count == 0)
                return dt;
            // DataTable dtm = GetModule();  Delete by xnb
            DataRow[] drs = dt.Select();
            int count = drs.Length;
            for (int i = 0; i < count; i++)
            {
                //modified by wangjf, 2009.3.19 for W300033500
                if (drs[i]["funcname"] == DBNull.Value || drs[i]["funcname"] == null)
                {
                    drs[i]["funcname"] = String.Empty;
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        //屏蔽权限
        private string ShieldRight(string Righttr, string cancelRighttr)
        {
            StringBuilder strbuild = new StringBuilder();
            for (int i = 0; i < Righttr.Length; i++)
            {
                if (cancelRighttr[i] == '1')//收回权限
                {
                    strbuild.Append("0");
                }
                else
                {
                    strbuild.Append(Righttr[i]);
                }
            }
            return strbuild.ToString();
        }
        

        /// <summary>
        /// 读取套件权限
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetSuitRight(string organizeId, string userId)
        {
            string suitlist = this.GetSuitList();
            StringBuilder sql = new StringBuilder();

            sql.Append("select moduleno,rightstr from uv_userright_cancel ");
            sql.Append(" where userid='" + userId + "'");
            sql.Append(" and moduleno in (");
            sql.Append(suitlist);
            sql.Append(" )");


            DataTable canceldt = DbHelper.GetDataTable(sql.ToString());//收回权限


            StringBuilder strsql = new StringBuilder();
            strsql.Append("select moduleno,rightstr from uv_userright_all ");
            strsql.Append(" where ocode= '" + organizeId + "' and userid='" + userId + "'");
            strsql.Append(" and moduleno in (");
            strsql.Append(suitlist);
            strsql.Append(" )");

            DataTable allrightdt = DbHelper.GetDataTable(strsql.ToString());//拥有权限

            DataRow[] drs = allrightdt.Select();
            for (int i = 0; i < drs.Length; i++)
            {
                DataRow[] canceldr = canceldt.Select("moduleno ='" + drs[i]["moduleno"].ToString() + "'");
                if (canceldr.Length > 0)
                {
                    if ((canceldr[0]["rightstr"].ToString().Substring(0, 1) == "1"))//被收回
                    {
                        drs[i].Delete();//取消权限
                    }
                }
            }

            allrightdt.AcceptChanges();
            return allrightdt;

        }

        /// <summary>
        /// 取得套件列表串
        /// </summary>
        /// <returns></returns>
        private string GetSuitList()
        {
            DataSet ds = ReadProductInfo();

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < ds.Tables["SuitInfo"].Rows.Count; i++)
            {
                str.Append("'");
                str.Append(ds.Tables["SuitInfo"].Rows[i]["Code"].ToString().ToUpper());
                str.Append("'");
                if (i < ds.Tables["SuitInfo"].Rows.Count - 1)
                {
                    str.Append(",");
                }
            }

            return str.ToString();
        }

        public  DataSet ReadProductInfo()
        {
            string rootpath = HttpContext.Current.Request.PhysicalApplicationPath;

            string file = Path.Combine(rootpath, "product.xml"); //i6Culture.GetPrdXmlFilePath(rootpath);

            DataSet ds = HttpRuntime.Cache[PRODUCTFILE] as DataSet;
            if (ds == null)
            {
                ds = new DataSet();
                try
                {
                    ds.ReadXml(file);

                    //缓存到进程Cache
                    HttpRuntime.Cache.Insert(PRODUCTFILE, ds, null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(12, 0, 0), System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return ds;
        }
        
    }

    /*
    *  FieldRight 的摘要说明。信息控制的接口，供其它模块调用。
    *   简单的sql语句可直接调用第一个函数，第一函数需要解析，比第二函数费时
    *  复杂的如涉及左右连接，关联表比较多的sql语句最好调用第二个函数
    * */
    public class FieldServices { 
        private string loginid = string.Empty;
        private string ocode = string.Empty;
        public FieldServices(string loginid, string ocode) {
            this.loginid = loginid;
            this.ocode = ocode;
        }

        /// <summary>
        /// 信息控制条件包装器
        /// </summary>
        /// <param name="sql">信息访问sql语句</param>
        /// <returns>包装控制条件后的信息访问语句</returns>
        #region public string LimitBuilder(string rawsql)
        public string LimitBuilder(string rawsql)
        {
            string s_return = rawsql;
            string sql = "";

            //解析sql语句（考虑用union连接起来的sql语句）
            ArrayList sqls = this.GetLimitSqls(rawsql);
            if (sqls == null || sqls.Count <= 0) return rawsql;

            int sqlnum = sqls.Count;

            for (int j = 0; j < sqlnum; j++)   //逐一处理sql语句
            {
                sql = sqls[j].ToString();

                //解析对象
                ArrayList Objs = this.GetLimitObjs(sql);
                if (Objs == null || Objs.Count <= 0)
                {
                    return sql;
                }

                int num = Objs.Count;
                string curcond = "1=1";
                for (int i = 0; i < num; i++)
                {
                    string obj = Objs[i].ToString();

                    //判断对象是否控制对象
                    if (this.UnderControl(obj) == false) continue;

                    //根据当前操作员取对象对应的限制域
                    string limitstr = "";
                    if (this.GetManLimitRegion(obj, ref limitstr) == false) continue;
                    if (limitstr == null || limitstr.Trim().Length == 0) continue;
                    curcond += " and " + limitstr;
                }

                if (curcond.Length == 3)
                {
                    return s_return;  //若无附加条件，立即返回
                }

                //将限制域附加到sql语句上
                int iWhereIndex = 0, iOrderIndex = 0, iGroupIndex = 0;
                string noand = curcond.Substring(8, curcond.Length - 8);
                string hasand = curcond.Substring(3, curcond.Length - 3);

                iWhereIndex = sql.ToLower().IndexOf("where");
                iGroupIndex = sql.ToLowerInvariant().IndexOf("group by");
                iOrderIndex = sql.ToLower().IndexOf("order by");

                if (iWhereIndex <= 0 && iOrderIndex <= 0 && iGroupIndex <= 0)
                    sql += " where " + noand;
                else
                {
                    if (iWhereIndex > 0)
                    {
                        if (iGroupIndex > 0)
                            sql = sql.Substring(0, iGroupIndex) + hasand + " " + sql.Substring(iGroupIndex, sql.Length - iGroupIndex);
                        else
                        {
                            if (iOrderIndex > 0)
                            {
                                sql = sql.Substring(0, iOrderIndex) + hasand + " " + sql.Substring(iOrderIndex, sql.Length - iOrderIndex);
                            }
                            else
                                sql = sql + hasand;
                        }
                    }
                    else
                    {
                        if (iGroupIndex > 0)
                            sql = sql.Substring(0, iGroupIndex) + " where " + noand + sql.Substring(iGroupIndex, sql.Length - iGroupIndex);
                        else
                        {
                            if (iOrderIndex > 0)
                            {
                                sql = sql.Substring(0, iOrderIndex) + " where " + noand + sql.Substring(iOrderIndex, sql.Length - iOrderIndex);
                            }
                        }
                    }
                }

                if (j == 0)
                    s_return = sql;
                else
                    s_return += " union " + sql;
            }
            return s_return;
        }

        /// <summary>
        /// 解析sql语句中的控制对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        #region private ArrayList GetLimitObjs(string Sql)
        private ArrayList GetLimitObjs(string Sql)
        {
            int iFromIndex = 0, iWhereIndex = 0, iGroupIndex = 0, iOrderIndex = 0, iHaving = 0;
            string substr = "";

            if (Sql == null || Sql.Trim().Length == 0) return null;

            Sql = Sql.Replace('\t', ' ');  //用空格符替换tab键
            Sql = Sql.Replace('\n', ' ');  //用空格符替换回车键

            iFromIndex = Sql.ToLower().IndexOf(" from ");
            iWhereIndex = Sql.ToLower().IndexOf(" where ");
            iGroupIndex = Sql.ToLower().IndexOf(" group ");
            iHaving = Sql.ToLower().IndexOf(" having ");
            iOrderIndex = Sql.ToLower().IndexOf(" order ");

            //取得表名序列
            if (iWhereIndex == -1 && iGroupIndex == -1 && iOrderIndex == -1)
                substr = Sql.Substring(iFromIndex + 5, Sql.Length - iFromIndex - 5);
            else
            {
                if (iWhereIndex > 0)
                    substr = Sql.Substring(iFromIndex + 5, iWhereIndex - iFromIndex - 5);
                else
                {
                    if (iGroupIndex > 0)
                        substr = Sql.Substring(iFromIndex + 5, iGroupIndex - iFromIndex - 5);
                    else
                    {
                        if (iHaving > 0)
                            substr = Sql.Substring(iFromIndex + 5, iHaving - iFromIndex - 5);
                        else
                        {
                            if (iOrderIndex > 0)
                                substr = Sql.Substring(iFromIndex + 5, iOrderIndex - iFromIndex - 5);
                            else
                                substr = Sql.Substring(iFromIndex + 5);
                        }
                    }
                }
            }

            //分解表名放于数组中
            ArrayList limitObjs = new ArrayList();
            substr = substr.Trim();
            int count = substr.Trim().Length;
            int i = substr.IndexOf(",");//解析表名的位置
            string tablename = string.Empty;//解析出的表名
            while (i > 0)
            {
                tablename = substr.Substring(0, i);
                tablename = tablename.Trim();
                int pos = tablename.IndexOf(" ", 0);
                if (pos > 0) tablename = tablename.Substring(0, pos);
                limitObjs.Add(tablename);
                substr = substr.Substring(i + 1, substr.Length - i - 1);
                i = substr.IndexOf(",");
            }
            //OracleClient和SqlClient的左连接写法不一样导致下面解析表名的时候要分段
            //OracleClient只要做","解析
            //SqlClient先做","解析，在做"left","right","join","on"等左右连接解析（SqlClient要做以下特别处理）
            //by zhoujb 2007-9-29
            //由于OracleClient在9i以后支持left","right","join","on"等左右连接，所以也要解析
            string leftOrRightFlg = "left";//左右连接标志
            i = substr.ToLower().IndexOf(" left ");//先找"left"，找到情况表明存在左连接
            if (i < 0)
            {
                i = substr.ToLower().IndexOf(" right ");//在找"right"，找到情况表明存在右连接
                leftOrRightFlg = "right";
            }
            if (i > 0)//存在左右连接的情况
            {
                int j = 0;//找"on"的位置
                while (i > 0)
                {
                    int pos = 0;
                    if (leftOrRightFlg == "left" || leftOrRightFlg == "right")//获取左右连接主表
                    {
                        tablename = substr.Substring(0, i);
                        tablename = tablename.Trim();
                        pos = tablename.IndexOf(" ", 0);
                        if (pos > 0) tablename = tablename.Substring(0, pos);
                        limitObjs.Add(tablename);
                        leftOrRightFlg = string.Empty;
                    }
                    i = substr.ToLower().IndexOf(" join ");
                    j = substr.ToLower().IndexOf(" on ");
                    tablename = substr.Substring(i + 6, j - i - 6);
                    tablename = tablename.Trim();
                    pos = tablename.IndexOf(" ", 0);
                    if (pos > 0) tablename = tablename.Substring(0, pos);
                    limitObjs.Add(tablename);
                    substr = substr.Substring(j + 4, substr.Length - j - 4);
                    i = substr.ToLower().IndexOf(" join ");
                }
            }

            substr = substr.Trim();
            limitObjs.Add(substr);
            return limitObjs;
        }
        #endregion

        /// <summary>
        /// 解析sql语句（用union连接起来的复合sql语句）中的独立sql语句
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        #region private ArrayList GetLimitSqls(string Sql)
        private ArrayList GetLimitSqls(string Sql)
        {
            ArrayList limitObjs = new ArrayList();
            string substr = Sql;

            if (substr == null || substr.Trim().Length == 0) return null;
            if (substr.ToUpper().IndexOf(" UNION ") <= 0)  //无union连接时直接返回原sql语句
            {
                limitObjs.Add(Sql);
                return limitObjs;
            }

            substr = substr.Replace('\t', ' ');  //用空格符替换tab键
            substr = substr.Replace('\n', ' ');  //用空格符替换回车键

            //分解独立sql语句放于数组中
            substr = substr.Trim();
            int i = System.Math.Max(substr.IndexOf(" UNION "), substr.IndexOf(" union "));
            while (i > 0)
            {
                string sqlname = substr.Substring(0, i);
                sqlname = sqlname.Trim();
                limitObjs.Add(sqlname);
                substr = substr.Substring(i + 6, substr.Length - i - 6);
                substr = substr.Trim();
                i = System.Math.Max(substr.IndexOf(" UNION "), substr.IndexOf(" union "));
            }
            substr = substr.Trim();
            limitObjs.Add(substr);
            return limitObjs;
        }
        #endregion

        /// <summary>
        /// 判断对象是否加入信息控制
        /// </summary>
        /// <param name="LimitObj"></param>
        /// <returns></returns>
        #region public bool UnderControl(string LimitObj)
        public bool UnderControl(string LimitObj)
        {
            if (LimitObj == null || LimitObj.Trim().Length == 0)
            {
                return false;
            }

            string sql = "select fg_limitmodul.needflg  from  fg_limitmodul,fg_limitobj " +
                       " where fg_limitobj.moduleno = fg_limitmodul.moduleno and " +
                       " fg_limitobj.limitflg=1 and fg_limitobj.tbname=" +LimitObj;
            object num = DbHelper.ExecuteScalar(sql);
            if (num == null || num.ToString().Trim().Length == 0)
                return false;
            else
            {
                int need = 0;
                try
                {
                    need = int.Parse(num.ToString());
                    if (need == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        #endregion
        /// <summary>
        /// 获取人员－对象已定义的限制域
        /// </summary>
        /// <param name="LimitObj"></param>
        /// <param name="sRegion"></param>
        /// <returns></returns>
        #region private bool GetManLimitRegion(string LimitObj,ref string sRegion)
        private bool GetManLimitRegion(string LimitObj, ref string sRegion)
        {
            if (LimitObj == null || LimitObj.Trim().Length == 0)
            {
                //this.LastError = "参数错误";
                return false;
            }

            sRegion = "1=1";
            string sql = "select fg_regdefmst.limitid from fg_regdefmst,fg_disinflmt " +
                       " where fg_disinflmt.limitid = fg_regdefmst.limitid and fg_regdefmst.limittype=1 " +
                       " and fg_disinflmt.distype='USER' " +
                       " and fg_disinflmt.mainid =" + loginid +
                       " and fg_regdefmst.ocode=" + ocode +
                       " and fg_regdefmst.tbname=" + LimitObj;

            sql += " union ";

            sql += "select fg_regdefmst.limitid from fg_regdefmst,fg_disinflmt " +
                " where fg_disinflmt.limitid = fg_regdefmst.limitid and fg_regdefmst.limittype=1 " +
                " and fg_disinflmt.distype='GROUP' " +
                " and fg_regdefmst.ocode=" + ocode +
                " and fg_regdefmst.tbname=" + LimitObj +
                " and fg_disinflmt.mainid in (select groupid from fg_groupuser where userid=" + loginid + " and ocode=" + ocode + ")";

            sql += " union ";

            sql += "select fg_regdefmst.limitid from fg_regdefmst,fg_disinflmt " +
                " where fg_disinflmt.limitid = fg_regdefmst.limitid and fg_regdefmst.limittype=1 " +
                " and fg_regdefmst.ocode=" + ocode +
                " and fg_regdefmst.tbname=" + LimitObj +
                " and fg_disinflmt.distype='ACTOR' " +
                " and fg_disinflmt.mainid in(select actorid from fg_useractor where userid=" + this.loginid + " and ocode=" +ocode + " union select fg_groupactor.actorid from fg_groupuser,fg_groupactor " +
                " where fg_groupactor.groupid = fg_groupuser.groupid and " +
                " fg_groupuser.userid =" + this.loginid + " and fg_groupactor.ocode=" +ocode + ")";


            DataTable dt = DbHelper.GetDataTable(sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                sRegion = "";
                return false;
            }
            else
            {
                //合并限制条件为一字符串（以and连接）
                int rows = dt.Rows.Count;
                for (int row = 0; row < rows; row++)
                {
                    sql = "select condvalue from fg_regdefmst where limitid=" + dt.Rows[row]["limitid"].ToString();
                    string cond = DbHelper.GetString(sql);
                    if (cond == null || cond.Trim().Length <= 0) continue;
                    cond = cond.Replace("[CURRENT_USER]", this.loginid);       //限制为本人的条件转化为当前用户
                    sRegion += " or " + cond;
                }
                if (sRegion.Length > 3)  //存在条件且条件不为空
                    sRegion = " ( " + sRegion.Substring(7, sRegion.Length - 7) + " ) ";//几个条件连接后添加（）
                else      //存在条件且条件为空
                    sRegion = "";
                return true;
            }
        }

        public string GetObjectLimiteCondition(string LimitObj, string sWhere)
        {
            string limitCondition = "";
            if (!GetManLimitRegion(LimitObj, ref limitCondition)) return sWhere;

            if (limitCondition.Length == 0) return sWhere;

            if (sWhere.Length == 0) return limitCondition;

            string strTemp = sWhere.ToUpper();
            int iPot = strTemp.IndexOf("ORDER");
            if (iPot > -1)
            {
                strTemp = strTemp.Substring(0, iPot) + " AND " + limitCondition + strTemp.Substring(iPot);
                return strTemp;
            }
            else
                return sWhere + " and " + limitCondition;

        }
        #endregion

        #endregion


    }


    public enum RightType
    {
        /// <summary>
        /// 功能权限
        /// </summary>
        Function = 1,

        /// <summary>
        /// 入口权限
        /// </summary>
        EntryRight = 11,

        /// <summary>
        /// 按钮权限
        /// </summary>
        ButtonRight = 12,

        /// <summary>
        /// 一个菜单在两个业务点被注册的按钮权限
        /// </summary>
        ButtonRightByMenu = 13,

        /// <summary>
        /// 域权限
        /// </summary>
        Field = 2,

        /// <summary>
        /// 信息权限/动作权限
        /// </summary>
        Info = 3,

    }
}
