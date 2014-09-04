using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    public class IdentityConfiguration
    {
        public string Schema { get; set; }
        public string UserTableName { get; set; }
        public string RoleTableName { get; set; }
        public string GroupTableName { get; set; }
        public string GroupRoleTableName { get; set; }
        public string ClaimTableName { get; set; }
        public string LoginTableName { get; set; }

        public IdentityConfiguration(string schema = "dbo")
        {
            this.Schema = schema;
            this.UserTableName = "AspNetUsers";
            this.GroupRoleTableName = "AspNetGroupRoles";
            this.LoginTableName = "AspNetUserLogins";
            this.ClaimTableName = "AspNetUserClaims";
            this.RoleTableName = "AspNetRoles";
            this.GroupTableName = "AspNetGroups";
        }
    }
}
