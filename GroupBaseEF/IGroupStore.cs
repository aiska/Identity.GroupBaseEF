using System;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    /// <summary>
    ///     Interface that exposes basic group management
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IGroupStore<TGroup, in TKey> : IDisposable where TGroup : IGroup<TKey>
    {
        /// <summary>
        ///     Create a new group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task CreateAsync(TGroup group);
        /// <summary>
        ///     Update a group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task UpdateAsync(TGroup group);
        /// <summary>
        ///     Delete a group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task DeleteAsync(TGroup group);
        /// <summary>
        ///     Find a group by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<TGroup> FindByIdAsync(TKey groupId);
        /// <summary>
        ///     Find a group by name
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<TGroup> FindByNameAsync(string groupName);
    }
}
