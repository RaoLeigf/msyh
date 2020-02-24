
namespace SUP.Common.Interface
{
    /// <summary>
    /// 桌面方案分配接口
    /// </summary>
    public interface IUserConfig
    {

        /// <summary>
        /// 方案拷贝
        /// </summary>
        /// <param name="fromUserId">原用户phid</param>
        /// <param name="fromUserType">原用户类型</param>
        /// <param name="toUserId">目标用户phid</param>
        /// <param name="toUserType">目标用户类型</param>
        /// <returns></returns>
        bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType);

        /// <summary>
        /// 方案删除
        /// </summary>
        /// <param name="userid">目标用户phid</param>
        /// <param name="usertype">目标用户类型</param>
        /// <returns></returns>
        bool DeleteUserConfig(long userid, int usertype);
        
    }

}
