using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Identity.GroupBaseEF
{
    /// <summary>
    ///     Interface that exposes an IQueryable groups
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IQueryableGroupStore<TGroup, in TKey> : IGroupStore<TGroup, TKey>, IDisposable where TGroup : IGroup<TKey>
    {
        /// <summary>
        ///     IQueryable Groups
        /// </summary>
        IQueryable<TGroup> Groups { get; }
    }

    /// <summary>
    ///     Interface that exposes an IQueryable groups
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    public interface IQueryableGroupStore<TGroup> : IQueryableGroupStore<TGroup, string>, IGroupStore<TGroup, string>, IDisposable where TGroup : IGroup<string>
    {
    }
}
