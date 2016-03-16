using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Identity.GroupBaseEF;

namespace UnitTestGroupBase
{
    [TestClass]
    public class UnitTestGroupBaseEF
    {
        [TestMethod]
        public void TestIdentityGroup()
        {
            // Arange
            var group = new IdentityGroup();
            var group2 = new IdentityGroup("testGroup");
            //var group3 = new IdentityGroup<string, IdentityGroupRole<string>>();
            //group3.Id = "3";

            Assert.IsNotNull(group);
            Assert.IsNotNull(group2);
            //Assert.IsNotNull(group3);
        }

        [TestMethod]
        public void TestIdentityRole()
        {
            // Arange
            IdentityRole role = new IdentityRole();

            Assert.IsNotNull(role);
        }

        [TestMethod]
        public void TestIdentityRole2()
        {
            // Arange
            IdentityRole role = new IdentityRole("testRole");

            Assert.IsNotNull(role);
        }
        [TestMethod]
        public void TestIdentityRole3()
        {
            // Arange
            var role = new IdentityRole<int, IdentityGroupRole<int>>();
            role.Id = 2;
            role.Name = "test Role";
            var test = role.Groups;

            Assert.IsNotNull(role);
        }
    }
}
