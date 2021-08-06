using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pix_API;
namespace Testing
{
    [TestFixture]
    public class IdAssignerTests
    {
        [Test]
        public void Check_normal_assigning()
        {
            var ids = new List<int>() { 3, 4, 5, 6, 7, 2 };
            var IdAssigner = new IdAssigner(ids);
            Assert.AreEqual(8, IdAssigner.NextEmptyId);
        }
    }
}
