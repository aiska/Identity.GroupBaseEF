using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    public interface IUserGroupStore<TUser, TKey> : IUserStore<TUser, TKey>, IDisposable 
        where TUser : class, IUser<TKey>
    {
        /// <summary>
        ///     Get User Group
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetGroupAsync(TUser user);

        /// <summary>
        ///     Set user group
        /// </summary>
        /// <param name="user"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task SetGroupAsync(TUser user, string groupName);
        Task<bool> IsInGroupAsync(TUser user, string groupName);
    }
}
