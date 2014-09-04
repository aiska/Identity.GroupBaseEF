using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    public class GroupStore<TGroup, TKey, TGroupRole> : IQueryableGroupStore<TGroup, TKey>, IGroupStore<TGroup, TKey>, IDisposable
        where TGroup : IdentityGroup<TKey, TGroupRole>, new()
        where TGroupRole : IdentityGroupRole<TKey>, new()
    {
        private bool _disposed;
        private EntityStore<TGroup> _groupStore;
        /// <summary>
        ///     Context for the store
        /// </summary>
        public DbContext Context
        {
            get;
            private set;
        }
        /// <summary>
        ///     If true will call dispose on the DbContext during Dipose
        /// </summary>
        public bool DisposeContext
        {
            get;
            set;
        }
        /// <summary>
        ///     Returns an IQueryable of users
        /// </summary>
        public IQueryable<TGroup> Groups
        {
            get
            {
                return this._groupStore.EntitySet;
            }
        }
        /// <summary>
        ///     Constructor which takes a db context and wires up the stores with default instances using the context
        /// </summary>
        /// <param name="context"></param>
        public GroupStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.Context = context;
            this._groupStore = new EntityStore<TGroup>(context);
        }
        /// <summary>
        ///     Find a group by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Task<TGroup> FindByIdAsync(TKey groupId)
        {
            this.ThrowIfDisposed();
            return this._groupStore.GetByIdAsync(groupId);
        }
        /// <summary>
        ///     Find a group by name
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public Task<TGroup> FindByNameAsync(string groupName)
        {
            this.ThrowIfDisposed();
            return this._groupStore.EntitySet.FirstOrDefaultAsync((TGroup u) => u.Name.ToUpper() == groupName.ToUpper());
        }
        /// <summary>
        ///     Insert an entity
        /// </summary>
        /// <param name="group"></param>
        public virtual async Task CreateAsync(TGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            this._groupStore.Create(group);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
        /// <summary>
        ///     Mark an entity for deletion
        /// </summary>
        /// <param name="group"></param>
        public virtual async Task DeleteAsync(TGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            this._groupStore.Delete(group);
            await this.Context.SaveChangesAsync().ConfigureAwait(false);
        }
        /// <summary>
        ///     Update an entity
        /// </summary>
        /// <param name="group"></param>
        public virtual async Task UpdateAsync(TGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            this._groupStore.Update(group);
            await this.Context.SaveChangesAsync().ConfigureAwait(false);
        }
        /// <summary>
        ///     Dispose the store
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }
        /// <summary>
        ///     If disposing, calls dispose on the Context.  Always nulls out the Context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this.Context != null)
            {
                this.Context.Dispose();
            }
            this._disposed = true;
            this.Context = null;
            this._groupStore = null;
        }
    }

    /// <summary>
    ///     EntityFramework based implementation
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    public class GroupStore<TGroup> :
        GroupStore<TGroup, string, IdentityGroupRole>, IQueryableGroupStore<TGroup>, IQueryableGroupStore<TGroup, string>, IGroupStore<TGroup, string>, IDisposable
        where TGroup : IdentityGroup<string, IdentityGroupRole>, new()
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public GroupStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context"></param>
        public GroupStore(DbContext context)
            : base(context)
        {
        }
    }
}
