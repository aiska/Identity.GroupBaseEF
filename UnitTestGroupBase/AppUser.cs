using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNet.Identity;
using Identity.GroupBaseEF;

namespace UnitTestGroupBase
{
    [TestClass]
    public class AppUser : IdentityUser<long, AppLogin, AppClaim, AppGroup, AppGroupRole>
    {
    }
    public class AppRole : IdentityRole<long, AppGroupRole>
    {
    }
    public class AppGroup : IdentityGroup<long, AppGroupRole>
    {
    }
    public class AppLogin : IdentityUserLogin<long>
    {
    }
    public class AppClaim : IdentityUserClaim<long>
    {
    }
    public class AppGroupRole : IdentityGroupRole<long>
    {
    }


    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, long, AppLogin, AppClaim, AppGroup, AppGroupRole>
    {
        public ApplicationDbContext()
            : base("IdentityLong")
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    [TestClass]
    //public class AppUserStore : UserStore<AppUser>
    public class AppUserStore : UserStore<AppUser, AppRole, long, AppLogin, AppClaim, AppGroup, AppGroupRole>
    {
        public AppUserStore()
            : base(new ApplicationDbContext())
        {

        }
    }

    [TestClass]
    //public class AppUserStore : UserStore<AppUser>
    public class AppRoleStore : RoleStore<AppRole, long, AppGroupRole>
    {
        public AppRoleStore()
            : base(new ApplicationDbContext())
        {

        }
    }

    [TestClass]
    public class TestAppClass
    {
        [TestMethod]
        public void TestMethod1()
        {
            AppUser user = new AppUser();
            AppUserStore store = new AppUserStore();
            UserManager<AppUser, long> manager = new UserManager<AppUser, long>(store);
            user.UserName = "aiska";
            manager.Create(user);

            AppRoleStore rolestore = new AppRoleStore();
            RoleManager<AppRole, long> rolemanager = new RoleManager<AppRole, long>(rolestore);
            var role = rolemanager.FindByName("roletest");
            var role2 = rolemanager.FindByName("roleupdate1");

            if (role == null)
            {
                role = new AppRole() { Name = "roletest" };
                rolemanager.Create(role);
            }
            if (role2 != null)
            {
                role2.Name = "roleupdate2";
                rolemanager.Update(role2);
            }
            else
            {
                role = new AppRole() { Name = "roleupdate1" };
                rolemanager.Create(role);
            }
            var role3 = rolemanager.FindByName("roleupdate3");
            if (role3 == null)
            {
                role3 = new AppRole() { Name = "roleupdate3" };
                rolemanager.Create(role3);
            }
            else
            {
                rolemanager.Delete(role3);
            }
        }
    }
}
