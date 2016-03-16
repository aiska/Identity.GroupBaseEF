using Identity.GroupBaseEF;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace UnitTestGroupBase
{
    [TestClass]
    public class TestGroupStore
    {
        [TestMethod]
        public void TestGroupStores()
        {
            GroupStore<IdentityGroup> store = new GroupStore<IdentityGroup>();
            var gm = new GroupManager<IdentityGroup>(store);
            var newGroup = new IdentityGroup("testgroup");
            gm.Create(newGroup);

            var result = CreateAsync().Result;
            Assert.AreEqual(true, result);
        }

        public async Task<bool> CreateAsync()
        {
            GroupStore<IdentityGroup> store = new GroupStore<IdentityGroup>();
            var gm = new GroupManager<IdentityGroup>(store);
            var newGroup = new IdentityGroup("testgroup");
            await gm.CreateAsync(newGroup);
            return true;
        }
        [TestMethod]
        public void TestGroupStores2()
        {
            var ctx = new IdentityDbContext();
            //var store = new GroupStore<IdentityGroup<string, IdentityGroupRole<string>>, string, IdentityGroupRole<string>>(ctx);
            //IdentityGroup<string, IdentityGroupRole<string>>, string, IdentityUser, IdentityGroupRole<string>>(ctx);
            var gr = new IdentityGroupRole<string>();
            //var group = new IdentityGroup<string, IdentityGroupRole<string>>();
            //var gm = new GroupManager<IdentityGroup<string, IdentityGroupRole<string>>, string>(store);
            var newGroup = new IdentityGroup("testgroup2");
            //gm.Create(group);

            var result = CreateAsync().Result;
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestUser()
        {
            //var ctx = new IdentityDbContext();
            var store = new UserStore<IdentityUser>();
            var usr = new IdentityUser("usertest2");
            var um = new UserManager<IdentityUser>(store);
            var gr = new IdentityGroup("group3");
            var gstore = new GroupStore<IdentityGroup>();
            var gm = new GroupManager<IdentityGroup>(gstore);
            if (!gm.Create(gr).Succeeded)
            {
                gm.FindByName("group3");
            }
            if (!um.Create(usr).Succeeded)
            {
                usr = um.FindByName("usertest2");
            }
            um.SetToGroup(usr.Id, "group3");
            //um.Delete(usr);
            //gr.Name = "groupchange";
            //gm.Update(gr);
            //gm.Delete(gr);
        }


        [TestMethod]
        public void TestUserdelete()
        {
            //var ctx = new IdentityDbContext();
            var store = new UserStore<IdentityUser>();
            var um = new UserManager<IdentityUser>(store);
            var usr = um.FindByName("usertest2");
            if (usr != null)
            {
                um.Delete(usr);
            }
        }
        [TestMethod]
        public void Testgroupdelete()
        {
            var gstore = new GroupStore<IdentityGroup>();
            var gm = new GroupManager<IdentityGroup>(gstore);
            var gr = gm.FindByName("group3");
            if (gr != null)
            {
                gm.Delete(gr);
            }
        }
        [TestMethod]
        public void TestFindGroup()
        {
            var gstore = new GroupStore<IdentityGroup>();
            var gm = new GroupManager<IdentityGroup>(gstore);
            var gr = gm.FindByName("group3");
            if (gr == null)
            {
                gr = new IdentityGroup("group3");
                gm.Create(gr);
            }
            var res = gm.FindById(gr.Id);
        }
        [TestMethod]
        public void TestGroupExist()
        {
            var gstore = new GroupStore<IdentityGroup>();
            var gm = new GroupManager<IdentityGroup>(gstore);
            gm.GroupExists("group3");
        }
        [TestMethod]
        public void TestUpdateGroup()
        {
            var gstore = new GroupStore<IdentityGroup>();
            var gm = new GroupManager<IdentityGroup>(gstore);
            var gr = gm.FindByName("group3");
            if (gr != null)
            {
                gr.Name = "test";
                gm.Update(gr);
            }
        }
        [TestMethod]
        public void TestUpdateGroup2()
        {
            var ctx = new IdentityDbContext<IdentityUser>();
            var gstore = new GroupStore<IdentityGroup>(ctx);
            var gm = new GroupManager<IdentityGroup>(gstore);
            var gr = gm.FindByName("group3");
            if (gr != null)
            {
                gr.Name = "group3";
                gm.Update(gr);
            }
        }
    }
}
