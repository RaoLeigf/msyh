using System;
using System.Collections.Generic;
using System.Text;

namespace SUP.Common.Base
{
    
    #region 用户类型

    public struct UserType
    {
        public static readonly string OrgUser = "OrgUser";//普通用户

        public static readonly string System = "SYSTEM";//系统管理员
    }


    /// <summary>
    /// 数据库类型
    /// </summary>
    public struct DBType
    {
        public static readonly string OracleClient = "oracleclient";//普通用户

        public static readonly string SqlClient = "sqlclient";//系统管理员
    }


    /// <summary>
    /// 产品
    /// </summary>
    public struct Product
    {
        public static readonly string I6 = "I6";

        public static readonly string I6P = "I6P";

        public static readonly string GE = "GE";

        public static readonly string A3 = "A3";

        public static readonly string I6S = "I6S";

        public static readonly string GEP = "GEP";
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public struct PbConnectType
    {
        public static readonly string HTTP = "HTTP";

        public static readonly string TCP = "TCP";
    }


    /// <summary>
    /// 客户端所属网络类型
    /// </summary>
    public struct NetWorkType
    {
        public static readonly string InterNet = "InterNet";

        public static readonly string IntraNet = "IntraNet";
    }

    /// <summary>
    /// 应用程序类型
    /// </summary>
    public struct AppType
    {
        public static readonly string WinForm = "winform";

        public static readonly string WebForm = "webform";

        public static readonly string WebMvc = "mvc";

        public static readonly string PB = "pb";
    }
    
    #endregion
    
}
