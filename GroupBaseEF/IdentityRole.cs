using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace Identity.GroupBaseEF
{
    public class IdentityRole<TKey, TGroupRole> : IRole<TKey> where TGroupRole : IdentityGroupRole<TKey>
    {
        public virtual ICollection<TGroupRole> Groups { get; private set; }
        public TKey Id { get; set; }
        public string Name { get; set; }
        public IdentityRole()
        {
            this.Groups = new List<TGroupRole>();
        }
    }
    public class IdentityRole : IdentityRole<string, IdentityGroupRole>
    {
        public IdentityRole()
        {
            base.Id = Guid.NewGuid().ToString();
        }
        public IdentityRole(string roleName)
            : this()
        {
            base.Name = roleName;
        }
    }
}
