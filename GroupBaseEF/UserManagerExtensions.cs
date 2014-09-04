using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    public static class UserGroupManagerExtensions
    {
        public static void SetToGroup<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string groupName)
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
        {
            AsyncHelper.RunSync(() => SetToGroupAsync(manager, userId, groupName));
        }
        public static async Task SetToGroupAsync<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string groupName)
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            TUser tUser = manager.FindById(userId);
            if (tUser == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, IdentityResources.UserIdNotFound, new object[]
		        {
			        userId
		        }));
            }
            IUserGroupStore<TUser, TKey> Store = (IUserGroupStore<TUser, TKey>)manager.GetType().GetProperty("Store", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(manager);
            await Store.SetGroupAsync(tUser, groupName);
        }
    }
}
