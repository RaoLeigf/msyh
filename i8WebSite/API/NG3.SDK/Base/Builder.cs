using System;
using System.Collections.Generic;
using System.Text;

namespace NG3.SDK
{
    public class Builder
    {

        public static Builder CreateInstance(string appKey, string secret, string tokenidentity)
        {
            Builder builder = new Builder();
            builder.Create(appKey, secret, tokenidentity);
            return builder;
        }

        private Builder() {
            securityEntity = new SecurityEntity();
        }

        private void Create(string appKey, string secret, string tokenidentity)
        {
            this.securityEntity.AppKey = appKey;
            this.securityEntity.TokenIdentity = tokenidentity;
            this.securityEntity.Secret = secret;
            //this.securityEntity.UserID = userid;
        }

        internal SecurityEntity SecurityEntity {
            get { return securityEntity; }
        }

        SecurityEntity securityEntity=null;
    }

    /// <summary>
    /// 安全验证实体
    /// </summary>
    internal class SecurityEntity
    {
        /// <summary>
        /// 是否参与安全验证
        /// </summary>
        public bool Join
        {
            get { return join; }
            set { join = value; }
        }
        private bool join = true;

        /// <summary>
        /// 请求的应用程序
        /// </summary>
        public string AppKey
        {
            get { return appKey; }
            set { appKey = value; }
        }
        private string appKey = string.Empty;

        /// <summary>
        /// 令牌标识
        /// </summary>
        public string TokenIdentity
        {
            get { return tokenidentity; }
            set { tokenidentity = value; }
        }
        private string tokenidentity = string.Empty;

        /// <summary>
        /// 传递的参数的格式
        /// </summary>
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        private string format = string.Empty;

        /// <summary>
        /// 请求提交方式
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        private string method = string.Empty;

        /// <summary>
        /// 请求时间
        /// </summary>
        public string TimeStamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        private string timestamp = string.Empty;

        /// <summary>
        /// SDK版本
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
        private string version = string.Empty;  
        
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string Secret
        {
            get { return secret; }
            set { secret = value; }
        }
        private string secret = string.Empty;  
        
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return productname; }
            set { productname = value; }
        }
        private string productname = string.Empty;  

        /// <summary>
        /// 当前账套
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        private string account = string.Empty;  
        
        /// <summary>
        /// 当前登录的UserID
        /// </summary>
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
        private string userid = string.Empty; 
        
        /// <summary>
        /// 当前登录的组织
        /// </summary>
        public string Org
        {
            get { return org; }
            set { org = value; }
        }
        private string org = string.Empty; 
    }
}
