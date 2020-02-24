
using NG.KeepConn;
using NG3.Data.Service;
using SUP.Common.Base;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GData3.Common.Utils
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public static class DESHelper
    {
        const string AES_IV = "1234567890000000";//16位 
        const string AES_KEY = "12345678900000001234567890000000"; //32位

        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="input">明文字符串</param>  
        /// <param name="key">密钥（32位）</param>  
        /// <returns>字符串</returns>  
        public static string EncryptByAES(string input, string key)
        {
            if (key.Length >= 32)
            {
                key = key.Substring(0, 32);
            }
            else {
                key = AES_KEY;
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        byte[] bytes = msEncrypt.ToArray();
                        return ByteArrayToHexString(bytes);
                    }
                }
            }
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="input">密文字节数组</param>  
        /// <param name="key">密钥（32位）</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static string DecryptByAES(string input, string key)
        {
            if (key.Length >= 32)
            {
                key = key.Substring(0, 32);
            }
            else
            {
                key = AES_KEY;
            }

            byte[] inputBytes = HexStringToByteArray(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的16进制字符串转换为byte数组
        /// </summary>
        /// <param name="s">16进制字符串(如：“7F 2C 4A”或“7F2C4A”都可以)</param>
        /// <returns>16进制字符串对应的byte数组</returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 将一个byte数组转换成一个格式化的16进制字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>格式化的16进制字符串</returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                //16进制数字
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                //16进制数字之间以空格隔开
                //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToUpper();
        }


        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strSource">需要加密的明文</param>
        /// <returns>返回32位加密结果</returns>
        public static string Get_MD5(string strSource, string sEncode="")
        {
            //new 
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取密文字节数组
            if (string.IsNullOrWhiteSpace(sEncode))
            {
                sEncode = "utf-8";
            }
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.GetEncoding(sEncode).GetBytes(strSource));

            //转换成字符串，并取9到25位 
            //string strResult = BitConverter.ToString(bytResult, 4, 8);  
            //转换成字符串，32位

            string strResult = BitConverter.ToString(bytResult);

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉 
            strResult = strResult.Replace("-", "");

            return strResult.ToLower();
        }

        /// <summary>
        /// 获取临时的组织加密信息
        /// </summary>
        /// <param name="orgcode">组织编码</param>
        /// <param name="dog_code">默认狗号</param>
        /// <returns></returns>
        public static string GetEncodeEmpowerInfo(string orgcode,string dog_code) {

            //I6WebAppInfo i6AppInfo = (I6WebAppInfo)HttpContext.Current.Session["NGWebAppInfo"];

            string userConn = NG3.Data.Service.ConnectionInfoService.GetSessionConnectString();

            var dbname = GetDatabaseByConStr(userConn);

            //加密狗信息的获取
            var sn = NGCOM.Instance.SN;//GetSN();
            //临时用户加密
            var s_sn = sn;
            var s_zth = dbname.Substring(2, dbname.Length - 2); //账套号-去掉NG
            var s_orgcode = orgcode;   //组织代码
            var s_dt = DateTime.Now.AddDays(15).ToString("yyyyMMdd"); //到期日期

            if (string.IsNullOrEmpty(sn)) {
                s_sn = dog_code;
            }

            var empowerInfo = s_sn + '-' + s_zth + '-' + s_orgcode + '-' + s_dt;
            
            var lzw = new NG3.LzwZip();
            //加密
            var encinfo = lzw.Level2EncodeToBase64(empowerInfo);

            return encinfo;
        }

        /// <summary>
        /// 解析数据库连接串
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public static string GetDatabaseByConStr(string conStr)
        {
            if (string.IsNullOrEmpty(conStr))
            {
                conStr = ConnectionInfoService.GetSessionConnectString();
            }
            string dataBase = dataBase = NG.NGKeyValueUtility.GetValue(conStr, "Database", "Initial Catalog");
            if (string.IsNullOrEmpty(dataBase) || dataBase.Length < 1)
            {
                dataBase = NG.NGKeyValueUtility.GetValue(conStr, "User ID");
            }
            return dataBase;
        }
        #endregion

    }
}
