using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Identity.GroupBaseEF;
using System.Data.Entity;

namespace UnitTestGroupBase
{
    [TestClass]
    public class TestEntityStore
    {
        [TestMethod]
        public void TestCreate()
        {
            var test = new IdentityDbContext();
            var test2 = test.Roles;
            Assert.IsNotNull(test2);
        }
    }
}
