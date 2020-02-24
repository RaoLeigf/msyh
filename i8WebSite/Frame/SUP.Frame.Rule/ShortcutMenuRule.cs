using Newtonsoft.Json.Linq;
using NG.Data.DB;
using SUP.Common.Interface;
using SUP.Frame.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Rule
{
    
    public class ShortcutMenuRule: IUserConfig
    {
        private ShortcutMenuDac dac = new ShortcutMenuDac();

        /// <summary>
        /// 方案拷贝
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="fromUserType"></param>
        /// <param name="toUserId"></param>
        /// <param name="toUserType"></param>
        /// <returns></returns>
        public bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            return dac.UserConfigCopy(fromUserId, fromUserType, toUserId, toUserType);
        }

        /// <summary>
        /// 方案删除
        /// </summary>
        /// <param name="userid">目标用户phid</param>
        /// <param name="usertype">目标用户类型</param>
        /// <returns></returns>
        public bool DeleteUserConfig(long userid, int usertype)
        {
            return dac.UserConfigDel(userid, usertype);
        }
    }
}
