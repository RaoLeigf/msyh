using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using i6.Common.Util;
using System.IO;
using System.Web;
using System.Data;

namespace NG3.Attachment.Base
{
    /// <summary>
    /// 附件上传服务
    /// </summary>
    public class NG3UploadFileService
    {
        private static UploadFileServiceProxy.UploadFileService _UploadFileService
        {
            get
            {
                string siteUrl = string.Empty;
                if (!string.IsNullOrEmpty(i6AppInfoEntity.ServerAddress))
                {
                    siteUrl = "http://" + i6AppInfoEntity.ServerAddress + "/" + i6AppInfoEntity.Product + i6AppInfoEntity.Series + "FileSrv/UploadFileService.asmx";
                }
                else
                {
                    //siteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath);
                    //siteUrl = siteUrl.Replace(HttpContext.Current.Request.ServerVariables["SERVER_NAME"], "127.0.0.1") + "FileSrv/UploadFileService.asmx";

                    siteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath);

                    try
                    {
                        siteUrl = siteUrl.Replace(HttpContext.Current.Request.ServerVariables["SERVER_NAME"], "127.0.0.1") + "FileSrv/UploadFileService.asmx";
                    }
                    catch (Exception ex)
                    {
                        siteUrl = "http://127.0.0.1:" + HttpContext.Current.Request.Url.Port + HttpContext.Current.Request.ApplicationPath + "FileSrv/UploadFileService.asmx";
                    }
                }

                //输出地址
                System.Diagnostics.Trace.WriteLine("[attach][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "][ServerUrl]" + siteUrl);

                //绑定到WebService
                UploadFileServiceProxy.UploadFileService service = new UploadFileServiceProxy.UploadFileService();
                service.Timeout = 300000;
                service.Url = siteUrl;

                return service;
            }
        }

        private static NG3UploadFileSvcProxy.NG3UploadFileSvc _NG3UploadFileSvc
        {
            get
            {
                string siteUrl = string.Empty;
                if (!string.IsNullOrEmpty(i6AppInfoEntity.ServerAddress))
                {
                    siteUrl = "http://" + i6AppInfoEntity.ServerAddress + "/" + i6AppInfoEntity.Product + i6AppInfoEntity.Series + "FileSrv/NG3UploadFileSvc.asmx";
                }
                else
                {
                    //siteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath);
                    //siteUrl = siteUrl.Replace(HttpContext.Current.Request.ServerVariables["SERVER_NAME"], "127.0.0.1") + "FileSrv/UploadFileService.asmx";

                    siteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath);

                    try
                    {
                        siteUrl = siteUrl.Replace(HttpContext.Current.Request.ServerVariables["SERVER_NAME"], "127.0.0.1") + "FileSrv/NG3UploadFileSvc.asmx";
                    }
                    catch (Exception ex)
                    {
                        siteUrl = "http://127.0.0.1:" + HttpContext.Current.Request.Url.Port + HttpContext.Current.Request.ApplicationPath + "FileSrv/NG3UploadFileSvc.asmx";
                    }
                }

                //输出地址
                System.Diagnostics.Trace.WriteLine("[attach][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "][ServerUrl]" + siteUrl);

                //绑定到WebService
                NG3UploadFileSvcProxy.NG3UploadFileSvc service = new NG3UploadFileSvcProxy.NG3UploadFileSvc();
                service.Timeout = 300000;
                service.Url = siteUrl;

                return service;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="asr_guid">客户端guid</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_table">业务表表名</param>
        /// <param name="asr_attach_table">附件表名称</param>
        /// <param name="asr_filename">文件名(空值表示删除所有文件)</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns>删除是否成功(1：成功，0：不成功)  </returns>
        public static string Del(string asr_guid, string asr_code, string asr_table, string asr_attach_table, string asr_filename, string asr_dbconn)
        {
            return _UploadFileService.Del(asr_guid, asr_code, asr_table, asr_attach_table, asr_filename, asr_dbconn);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="asr_guid">客户端guid</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_table">业务表表名</param>
        /// <param name="asr_attach_table">附件表名称</param>
        /// <param name="asr_filename">文件名(空值表示删除所有文件)</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns>删除是否成功(1：成功，0：不成功)  </returns>
        public static string DelEx(string asr_guid, string asr_code, string asr_table, string asr_attach_table, string asr_filename, string asr_dbconn)
        {
            return _UploadFileService.DelEx(asr_guid, asr_code, asr_table, asr_attach_table, asr_filename, asr_dbconn);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="asr_guid">客户端guid</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_table">业务表表名</param>
        /// <param name="asr_attach_table">附件表名称</param>
        /// <param name="asr_filename">文件名(空值表示删除所有文件)</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns>删除是否成功(1：成功，0：不成功)  </returns>
        public static string DelAsync(string asr_guid, string asr_code, string asr_table, string asr_attach_table, string asr_filename, string asr_dbconn)
        {
            _UploadFileService.DelAsync(asr_guid, asr_code, asr_table, asr_attach_table, asr_filename, asr_dbconn);
            return "1";
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="asr_guid">客户端guid</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_table">业务表表名</param>
        /// <param name="asr_attach_table">附件表名称</param>
        /// <param name="asr_filename">文件名（空值表示取第一个文件）</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns>文件数据(二进制字符串格式) </returns>
        public static byte[] Get(string asr_guid, string asr_code, string asr_table, string asr_attach_table, string asr_filename, string asr_dbconn)
        {
            return _UploadFileService.Get(asr_guid, asr_code, asr_table, asr_attach_table, asr_filename, asr_dbconn);
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="saveparms">参数</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns>保存是否成功(1：成功，0：不成功) </returns>
        public static string BatchSave(string saveparms, string asr_dbconn)
        {
            return _UploadFileService.BatchSave(saveparms, asr_dbconn);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="asr_guid">客户端guid</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_mode">同步/异步(同步：0， 异步：1， 默认同步)</param>
        /// <returns>保存是否成功(1：成功，0：不成功) </returns>
        public static string Save(string asr_guid, string asr_code, string asr_mode)
        {
            return _UploadFileService.Save(asr_guid, asr_code, asr_mode);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="asr_guid">客户端guid</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_table">业务表表名</param>
        /// <param name="asr_attach_table">附件表名称</param>
        /// <param name="asr_dbconn">连接串</param>
        /// <param name="asr_params">附件参数</param>
        /// <param name="asr_data">附件数据(二进制格式字符串)</param>
        /// <returns>保存是否成功(1：成功，0：不成功) </returns>
        public static string SaveData(string asr_guid, string asr_code, string asr_table, string asr_attach_table, string asr_dbconn, string asr_params, byte[] asr_data)
        {
            return _UploadFileService.SaveData(asr_guid, asr_code, asr_table, asr_attach_table, asr_dbconn, asr_params, asr_data);
        }

        /// <summary>
        /// 取消保存
        /// </summary>
        /// <param name="asr_guid">客户端guid</param>
        /// <returns>取消保存是否成功(1：成功，0：不成功) </returns>
        public static string ClearCache(string asr_guid)
        {
            return _UploadFileService.ClearCache(asr_guid);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns></returns>
        public static string ClearCacheWeb(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn)
        {
            return _UploadFileService.ClearCacheWeb(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn);
        }

        /// <summary>
        /// 批量初始化
        /// </summary>
        /// <param name="batchparms">参数</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns>保存是否成功(1：成功，0：不成功) </returns>
        public static string BatchInit(string batchparms, string asr_dbconn)
        {
            return _UploadFileService.BatchInit(batchparms, asr_dbconn);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="asr_session_guid">会话GUID</param>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns></returns>
        public static string Init(string asr_session_guid, string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn)
        {
            return _UploadFileService.Init(asr_session_guid, asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="asr_session_guid">会话GUID</param>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns></returns>
        public static string InitEx(string asr_session_guid, string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn)
        {
            return _UploadFileService.InitEx(asr_session_guid, asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="asr_session_guid">会话GUID</param>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns></returns>
        public static string InitMutil(string asr_session_guid, string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string asr_where)
        {
            return _UploadFileService.InitMutil(asr_session_guid, asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, asr_where);
        }

        /// <summary>
        /// 初始化数据库链接
        /// </summary>
        /// <param name="asr_dbconn"></param>
        /// <returns></returns>
        public static string InitConn(string asr_dbconn)
        {
            return _UploadFileService.InitConn(asr_dbconn);
        }

        /// <summary>
        /// 初始化数据库链接
        /// </summary>
        /// <param name="asr_session_guid"></param>
        /// <param name="asr_dbconn"></param>
        /// <returns></returns>
        public static string InitConn(string asr_session_guid, string asr_dbconn)
        {
            return _UploadFileService.InitConnEx(asr_session_guid, asr_dbconn);
        }

        /// <summary>
        /// 单据复制
        /// </summary>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_guid_copy">被复制的guid</param>
        /// <returns></returns>
        public static string Copy(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string asr_guid_copy)
        {
            return _UploadFileService.Copy(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, asr_guid_copy);
        }

        /// <summary>
        /// 异步单据复制
        /// </summary>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_guid_copy">被复制的guid</param>
        /// <returns></returns>
        public static string AyncCopy(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string asr_guid_copy)
        {
            return _UploadFileService.AyncCopy(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, asr_guid_copy);
        }

        /// <summary>
        /// 单据复制
        /// </summary>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_guid_copy">被复制的guid</param>
        /// <returns></returns>
        public static string CopyEx(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string asr_guid_copy, string asr_fid)
        {
            return _UploadFileService.CopyEx(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, asr_guid_copy, asr_fid);
        }

        /// <summary>
        /// 单据复制(归档)
        /// </summary>
        /// <param name="asr_session_guid">会话GUID</param>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_guid_copy">被复制的guid</param>
        /// <param name="asr_fid">被复制的附件id</param>
        /// <returns></returns>
        public static string CopyForArchive(string asr_session_guid, string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string asr_guid_copy)
        {
            return _UploadFileService.CopyForArchive(asr_session_guid, asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, asr_guid_copy);
        }

        /// <summary>
        /// 单据复制
        /// </summary>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_session_guid_copy">被复制的guid</param>
        /// <returns></returns>
        public static string CopyAttach(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string asr_session_guid_copy)
        {
            return _UploadFileService.CopyAttach(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, asr_session_guid_copy);
        }

        /// <summary>
        /// 单据复制
        /// </summary>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人姓名</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_session_guid_copy">被复制的guid</param>
        /// <returns></returns>
        public static string CopyAttachFromBills(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string asr_session_guids_copy)
        {
            return _UploadFileService.CopyAttachFromBills(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, asr_session_guids_copy);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="keyvalue">键值</param>
        /// <returns></returns>
        public static string SaveConfig(string keyvalue)
        {
            return _UploadFileService.SaveConfig(keyvalue);
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns></returns>
        public static DataTable ReadConfig()
        {
            return _UploadFileService.ReadConfig();
        }

        /// <summary>
        /// 读取附件数量
        /// </summary>
        /// <param name="asr_guid">附件GUID</param>
        /// <returns></returns>
        public static int GetAttachCount(string asr_guid)
        {
            return _UploadFileService.GetAttachCount(asr_guid);
        }

        /// <summary>
        /// 读取附件数量
        /// </summary>
        /// <param name="asr_guid">附件GUID</param>
        /// <returns></returns>
        public static int GetAttachCountEx(string asr_guid)
        {
            return _UploadFileService.GetAttachCountEx(asr_guid);
        }

        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="asr_guid">附件GUID</param>
        /// <returns></returns>
        public static DataTable GetAttachInfo(string asr_guid)
        {
            return _UploadFileService.GetAttachInfo(asr_guid);
        }

        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_fid">附件文件GUID</param>
        /// <returns></returns>
        public static DataTable GetAttachInfoByFid(string asr_dbconn, string asr_fid)
        {
            return _UploadFileService.GetAttachInfoByFid(asr_dbconn, asr_fid);
        }

        /// <summary>
        /// 数据升级
        /// </summary>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_attach_table">附件表</param>
        /// <returns></returns>
        public static string DataUpgrade(string asr_dbconn, string asr_attach_table)
        {
            return _UploadFileService.DataUpgrade(asr_dbconn, asr_attach_table);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="asr_session_guid">会话GUID</param>
        /// <param name="asr_fid">业务表主键</param>
        /// <returns></returns>
        public static byte[] GetOffice(string asr_guid, string asr_fid)
        {
            return _UploadFileService.GetOffice(asr_guid, asr_fid);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="asr_session_guid">会话GUID</param>
        /// <param name="asr_fid">业务表主键</param>
        /// <returns></returns>
        public static string UpdateOffice(string asr_guid, string asr_fid, byte[] buffer)
        {
            return _UploadFileService.UpdateOffice(asr_guid, asr_fid, buffer);
        }

        /// <summary>
        /// 获取会话GUID
        /// </summary>
        /// <param name="asr_attach_table"></param>
        /// <param name="asr_table"></param>
        /// <param name="asr_code"></param>
        /// <param name="asr_fill"></param>
        /// <param name="asr_fillname"></param>
        /// <param name="asr_dbconn"></param>
        /// <returns></returns>
        public static string GetSessionGuid(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn)
        {
            return _UploadFileService.GetSessionGuid(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn);
        }

        /// <summary>
        /// 设置公共连接串
        /// </summary>
        /// <param name="dbconn"></param>
        /// <returns></returns>
        public static string SetPubConn(string dbconn)
        {
            return _UploadFileService.SetPubConn(dbconn);
        }
        public static string PicInvoke(string dllName, string className, string actionName, object[] args)
        {
            return _UploadFileService.PicInvoke(dllName, className, actionName, args);
        }

        public static object PicInvokeEx(string dllName, string className, string actionName, object[] args)
        {
            return _UploadFileService.PicInvokeEx(dllName, className, actionName, args);
        }

        /// <summary>
        /// App上传附件
        /// </summary>
        /// <param name="asr_guid">会话GUID</param>   
        /// <param name="asr_name">附件名称</param>   
        /// <param name="asr_data">附件数据(二进制格式字符串)</param>
        /// <returns></returns>
        public static string UploadForApp(string asr_guid, string asr_name, byte[] asr_data)
        {
            return _UploadFileService.UploadForApp(asr_guid, asr_name, asr_data);
        }

        /// <summary>
        /// 设置app扫码上传附件记录状态
        /// </summary>
        /// <param name="asr_guid">会话GUID</param> 
        /// <param name="updatetype">0:点击取消按钮,1：点击确定按钮</param>
        /// <returns></returns>
        public static string SetAppUploadfilesStatus(string asr_guid, string updatetype)
        {
            return _UploadFileService.SetAppUploadfilesStatus(asr_guid, updatetype);
        }

        /// 获取app上传附件信息
        /// </summary>
        /// <param name="asr_session_guid">fids</param> 
        /// <param name="status">状态标识</param>
        /// <returns></returns>
        public static string GetAppUploadfiles(string asr_session_guid, string status)
        {
            return _UploadFileService.GetAppUploadfiles(asr_session_guid, status);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="asr_session_guid">会话GUID</param>
        /// <param name="asr_code">业务表主键</param>
        /// <param name="asr_table">业务表表名</param>
        /// <param name="asr_attach_table">附件表名称</param>        
        /// <param name="asr_name">文件名称</param>
        /// <param name="asr_fill">上传人id</param>
        /// <param name="asr_fillname">上传人name</param>wo
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="asr_data">附件数据(二进制格式字符串)</param>
        /// <returns></returns>
        public static string Upload(string asr_guid, string asr_code, string asr_table, string asr_attach_table, string asr_name, string asr_fill, string asr_fillname, string asr_dbconn, byte[] asr_data)
        {
            return _UploadFileService.Upload(asr_guid, asr_code, asr_table, asr_attach_table, asr_name, asr_fill, asr_fillname, asr_dbconn, asr_data);
        }

        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="asr_attach_table">附件表名</param>
        /// <param name="asr_table">业务表名</param>
        /// <param name="asr_code">业务单据编码</param>
        /// <param name="asr_name">附件名称</param>
        /// <param name="asr_fill">录入人</param>
        /// <param name="asr_fillname">录入人名称</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns></returns>
        public static string UpdateFile(string asr_attach_table, string asr_table, string asr_code, string asr_name, string asr_fill, string asr_fillname, string asr_dbconn, byte[] asr_data)
        {
            return _UploadFileService.UpdateFile(asr_attach_table, asr_table, asr_code, asr_name, asr_fill, asr_fillname, asr_dbconn, asr_data);

        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="asr_code">业务单据code</param>
        /// <param name="asr_fid">附件id</param>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <returns></returns>
        public static byte[] GetFileByAsrFid(string asr_code, string asr_fid, string asr_dbconn)
        {
            return _UploadFileService.GetFileByAsrFid(asr_code, asr_fid, asr_dbconn);

        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="asr_guid">附件会话guid</param>
        /// <param name="asr_name">附件名称</param>
        /// <returns></returns>
        public static string DelForInvoiceRecog(string asr_guid, string asr_name)
        {
            return _UploadFileService.DelForInvoiceRecog(asr_guid, asr_name);
        }


        #region 15.0 附件增删改查供丰立新调用
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="asr_dbconn">数据库连接串</param>
        /// <param name="blob_data">附件数据(二进制格式字符串)</param>
        /// <returns></returns>
        public static string NG3Upload(string asr_dbconn, string blob_data)
        {
            return _NG3UploadFileSvc.NG3Upload(asr_dbconn, blob_data);
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="asr_dbconn"></param>
        /// <param name="phid"></param>
        /// <returns></returns>
        public static string NG3Del(string dbconn, long phid)
        {
            return _NG3UploadFileSvc.NG3Del(dbconn, phid);
        }

        /// <summary>
        /// 修改附件
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="phid"></param>
        /// <param name="blob_data"></param>
        /// <returns></returns>
        public static string NG3Modify(string dbconn, long phid, string blob_data)
        {
            return _NG3UploadFileSvc.NG3Modify(dbconn, phid, blob_data);
        }

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="phid"></param>
        /// <returns></returns>
        public static string NG3Get(string dbconn, long phid)
        {
            return _NG3UploadFileSvc.NG3Get(dbconn, phid);
        }

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="phid"></param>
        /// <returns></returns>
        public static byte[] NG3GetEx(string dbconn, long phid)
        {
            return _NG3UploadFileSvc.NG3GetEx(dbconn, phid);
        }
        #endregion

        /// <summary>
        /// 附件拷贝
        /// </summary>
        /// <param name="asr_attach_table"></param>
        /// <param name="asr_table"></param>
        /// <param name="asr_code"></param>
        /// <param name="asr_fill"></param>
        /// <param name="asr_fillname"></param>
        /// <param name="asr_dbconn"></param>
        /// <param name="oriphid"></param>
        /// <param name="ori_asr_table"></param>
        /// <returns></returns>
        public static string NG3Copy(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, long oriphid, string ori_asr_table)
        {
            return _NG3UploadFileSvc.NG3Copy(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, oriphid, ori_asr_table);
        }

        /// <summary>
        /// 多单据附件拷贝
        /// </summary>
        /// <param name="asr_attach_table"></param>
        /// <param name="asr_table"></param>
        /// <param name="asr_code"></param>
        /// <param name="asr_fill"></param>
        /// <param name="asr_fillname"></param>
        /// <param name="asr_dbconn"></param>
        /// <param name="oriphid"></param>
        /// <param name="ori_asr_table"></param>
        /// <returns></returns>
        public static string NG3MutilCopy(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string oriphids, string ori_asr_table)
        {
            return _NG3UploadFileSvc.NG3MutilCopy(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, oriphids, ori_asr_table);
        }

        /// <summary>
        /// 根据业务单据phid删除附件
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="asr_code"></param>
        /// <param name="asr_attach_table"></param>
        /// <param name="asr_table"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static string NG3DelByAsrCode(string dbconn, string asr_code, string asr_attach_table, string asr_table)
        {
            return _NG3UploadFileSvc.NG3DelByAsrCode(dbconn, asr_code, asr_attach_table, asr_table);
        }

        /// <summary>
        /// 下载图片附件到服务端Attach目录供NG3页面展示
        /// </summary>
        /// <param name="dbconn">数据库连接串</param>
        /// <param name="downinfo">下载单据信息串</param>
        /// <returns></returns>
        public static string NG3ImageLoad2AttachFolder(string dbconn, string downinfo)
        {
            return _NG3UploadFileSvc.NG3ImageLoad2AttachFolder(dbconn, downinfo);
        }

        /// <summary>
        /// 异步批量拷贝, 目的单据的附件仅来源于一张原始单据，支持批量拷贝多张目的单据
        /// </summary>
        /// <param name="copyjosn"></param>
        /// <param name="asr_fill"></param>
        /// <param name="asr_fillname"></param>
        /// <param name="asr_dbconn"></param>
        /// <returns></returns>
        public static string AyncNG3MutilCopyEx(string copyjosn, string asr_fill, string asr_fillname, string asr_dbconn)
        {
            return _NG3UploadFileSvc.AyncNG3MutilCopyEx(copyjosn, asr_fill, asr_fillname, asr_dbconn);
        }

        /// <summary>
        /// 批量拷贝, 目的单据的附件仅来源于一张原始单据，支持批量拷贝多张目的单据
        /// </summary>
        /// <param name="copyjosn"></param>
        /// <param name="asr_fill"></param>
        /// <param name="asr_fillname"></param>
        /// <param name="asr_dbconn"></param>
        /// <returns></returns>
        public static string NG3MutilCopyEx(string copyjosn, string asr_fill, string asr_fillname, string asr_dbconn)
        {
            return _NG3UploadFileSvc.NG3MutilCopyEx(copyjosn, asr_fill, asr_fillname, asr_dbconn);
        }

        /// <summary>
        /// 附件拷贝
        /// </summary>
        /// <param name="asr_attach_table"></param>
        /// <param name="asr_table"></param>
        /// <param name="asr_code"></param>
        /// <param name="asr_fill"></param>
        /// <param name="asr_fillname"></param>
        /// <param name="asr_dbconn"></param>
        /// <param name="attachids">attachment_record表phid，可传多个，用“，”隔开</param>
        /// <returns></returns>
        public string NG3CopyByAttachIds(string asr_attach_table, string asr_table, string asr_code, string asr_fill, string asr_fillname, string asr_dbconn, string attachids)
        {
            return _NG3UploadFileSvc.NG3CopyByAttachIds(asr_attach_table, asr_table, asr_code, asr_fill, asr_fillname, asr_dbconn, attachids);
        }
    }
}
