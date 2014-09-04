using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    /// <summary>
    ///     Mimimal set of data needed to persist group information
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IGroup<out TKey>
    {
        // Summary:
        //     Id of the group
        TKey Id { get; }
        //
        // Summary:
        //     Name of the group
        string Name { get; set; }
    }
    public interface IGroup : IGroup<string>
    {
    }
}
