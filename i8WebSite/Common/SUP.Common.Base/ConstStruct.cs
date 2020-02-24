using System;
using System.Collections.Generic;
using System.Text;

namespace SUP.Common.Base
{
    
    #region �û�����

    public struct UserType
    {
        public static readonly string OrgUser = "OrgUser";//��ͨ�û�

        public static readonly string System = "SYSTEM";//ϵͳ����Ա
    }


    /// <summary>
    /// ���ݿ�����
    /// </summary>
    public struct DBType
    {
        public static readonly string OracleClient = "oracleclient";//��ͨ�û�

        public static readonly string SqlClient = "sqlclient";//ϵͳ����Ա
    }


    /// <summary>
    /// ��Ʒ
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
    /// ���ݿ�����
    /// </summary>
    public struct PbConnectType
    {
        public static readonly string HTTP = "HTTP";

        public static readonly string TCP = "TCP";
    }


    /// <summary>
    /// �ͻ���������������
    /// </summary>
    public struct NetWorkType
    {
        public static readonly string InterNet = "InterNet";

        public static readonly string IntraNet = "IntraNet";
    }

    /// <summary>
    /// Ӧ�ó�������
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
