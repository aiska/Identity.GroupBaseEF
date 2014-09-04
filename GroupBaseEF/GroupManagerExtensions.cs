using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    /// <summary>
    ///     Extension methods for GroupManager
    /// </summary>
    public static class GroupManagerExtensions
    {
        /// <summary>
        ///     Find a group by id
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static TGroup FindById<TGroup, TKey>(this GroupManager<TGroup, TKey> manager, TKey groupId)
            where TGroup : class, IGroup<TKey>
            where TKey : IEquatable<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync<TGroup>(() => manager.FindByIdAsync(groupId));
        }
        /// <summary>
        ///     Find a group by name
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static TGroup FindByName<TGroup, TKey>(this GroupManager<TGroup, TKey> manager, string groupName)
            where TGroup : class, IGroup<TKey>
            where TKey : IEquatable<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync<TGroup>(() => manager.FindByNameAsync(groupName));
        }
        /// <summary>
        ///     Create a group
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static IdentityResult Create<TGroup, TKey>(this GroupManager<TGroup, TKey> manager, TGroup group)
            where TGroup : class, IGroup<TKey>
            where TKey : IEquatable<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync<IdentityResult>(() => manager.CreateAsync(group));
        }
        /// <summary>
        ///     Update an existing group
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static IdentityResult Update<TGroup, TKey>(this GroupManager<TGroup, TKey> manager, TGroup group)
            where TGroup : class, IGroup<TKey>
            where TKey : IEquatable<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync<IdentityResult>(() => manager.UpdateAsync(group));
        }
        /// <summary>
        ///     Delete a group
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static IdentityResult Delete<TGroup, TKey>(this GroupManager<TGroup, TKey> manager, TGroup group)
            where TGroup : class, IGroup<TKey>
            where TKey : IEquatable<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync<IdentityResult>(() => manager.DeleteAsync(group));
        }
        /// <summary>
        ///     Returns true if the group exists
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static bool GroupExists<TGroup, TKey>(this GroupManager<TGroup, TKey> manager, string groupName)
            where TGroup : class, IGroup<TKey>
            where TKey : IEquatable<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync<bool>(() => manager.GroupExistsAsync(groupName));
        }
    }
}
