using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture(Category = "Integration Tests")]
    public class IntegrationTests
    {
        [Test]
        public void IntegrationTest1()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
