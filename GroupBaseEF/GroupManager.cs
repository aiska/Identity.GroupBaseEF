using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    /// <summary>
    ///     Exposes group related api which will automatically save changes to the GroupStore
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class GroupManager<TGroup, TKey> : IDisposable
        where TGroup : class, IGroup<TKey>
        where TKey : IEquatable<TKey>
    {
        private bool _disposed;
        private IIdentityValidator<TGroup> _groupValidator;
        /// <summary>
        ///     Persistence abstraction that the Manager operates against
        /// </summary>
        protected IGroupStore<TGroup, TKey> Store
        {
            get;
            private set;
        }
        /// <summary>
        ///     Used to validate groups before persisting changes
        /// </summary>
        public IIdentityValidator<TGroup> GroupValidator
        {
            get
            {
                return this._groupValidator;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._groupValidator = value;
            }
        }
        /// <summary>
        ///     Returns an IQueryable of groups if the store is an IQueryableGroupStore
        /// </summary>
        public virtual IQueryable<TGroup> Groups
        {
            get
            {
                IQueryableGroupStore<TGroup, TKey> queryableGroupStore = this.Store as IQueryableGroupStore<TGroup, TKey>;
                if (queryableGroupStore == null)
                {
                    throw new NotSupportedException(IdentityResources.StoreNotIQueryableGroupStore);
                }
                return queryableGroupStore.Groups;
            }
        }
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="store">The IGroupStore is responsible for commiting changes via the UpdateAsync/CreateAsync methods</param>
        public GroupManager(IGroupStore<TGroup, TKey> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            this.Store = store;
            this.GroupValidator = new GroupValidator<TGroup, TKey>(this);
        }
        /// <summary>
        ///     Dispose this object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        ///     Create a group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> CreateAsync(TGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            IdentityResult identityResult = await this.GroupValidator.ValidateAsync(group);
            IdentityResult result;
            if (!identityResult.Succeeded)
            {
                result = identityResult;
            }
            else
            {
                await this.Store.CreateAsync(group);
                result = IdentityResult.Success;
            }
            return result;
        }
        /// <summary>
        ///     Update an existing group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> UpdateAsync(TGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            IdentityResult identityResult = await this.GroupValidator.ValidateAsync(group).ConfigureAwait(false);
            IdentityResult result;
            if (!identityResult.Succeeded)
            {
                result = identityResult;
            }
            else
            {
                await this.Store.UpdateAsync(group).ConfigureAwait(false);
                result = IdentityResult.Success;
            }
            return result;
        }
        /// <summary>
        ///     Delete a group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> DeleteAsync(TGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            TGroup gr = await this.Store.FindByIdAsync(group.Id);
            if (gr == null)
            {
                return IdentityResult.Failed("Group Not Found");
            }
            await this.Store.DeleteAsync(gr).ConfigureAwait(false);
            return IdentityResult.Success;
        }
        /// <summary>
        ///     Returns true if the group exists
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public virtual async Task<bool> GroupExistsAsync(string groupName)
        {
            this.ThrowIfDisposed();
            if (groupName == null)
            {
                throw new ArgumentNullException("groupName");
            }
            return await this.FindByNameAsync(groupName).ConfigureAwait(false) != null;
        }
        /// <summary>
        ///     Find a group by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public virtual async Task<TGroup> FindByIdAsync(TKey groupId)
        {
            this.ThrowIfDisposed();
            return await this.Store.FindByIdAsync(groupId).ConfigureAwait(false);
        }
        /// <summary>
        ///     Find a group by name
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public virtual async Task<TGroup> FindByNameAsync(string groupName)
        {
            this.ThrowIfDisposed();
            if (groupName == null)
            {
                throw new ArgumentNullException("groupName");
            }
            return await this.Store.FindByNameAsync(groupName).ConfigureAwait(false);
        }
        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }
        /// <summary>
        ///     When disposing, actually dipose the store
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this._disposed)
            {
                this.Store.Dispose();
            }
            this._disposed = true;
        }
    }
    /// <summary>
    ///     Exposes group related api which will automatically save changes to the GroupStore
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    public class GroupManager<TGroup> : GroupManager<TGroup, string> where TGroup : class, IGroup<string>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="store"></param>
        public GroupManager(IGroupStore<TGroup, string> store)
            : base(store)
        {
        }
    }
}
