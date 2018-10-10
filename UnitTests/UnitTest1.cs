using NUnit.Framework;

namespace UnitTests
{
    [TestFixture(Category = "Unit Tests")]
    public class UnitTestClass1
    {
        [Test]
        public void UnitTest1()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
