using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Identity.GroupBaseEF
{
    public class IdentityGroupRole<TKey>
    {
        public virtual TKey RoleId { get; set; }
        public virtual TKey GroupId { get; set; }
    }
    public class IdentityGroupRole : IdentityGroupRole<string>
    { }
}
