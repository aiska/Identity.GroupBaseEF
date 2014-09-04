using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace Identity.GroupBaseEF
{
    public class IdentityGroup<TKey, TGroupRole> : IGroup<TKey>
        where TGroupRole : IdentityGroupRole<TKey>
    {
        public IdentityGroup()
        {
            Roles = new List<TGroupRole>();
        }
        public TKey Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TGroupRole> Roles { get; private set; }
    }

    public class IdentityGroup : IdentityGroup<string, IdentityGroupRole>
    {
        public IdentityGroup()
        {
            base.Id = Guid.NewGuid().ToString();
        }
        public IdentityGroup(string groupName)
            : this()
        {
            base.Name = groupName;
        }
    }
}
