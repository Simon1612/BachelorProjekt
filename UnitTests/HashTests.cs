using DentalResearchApp.Code.Impl;
using Xunit;

namespace UnitTests
{
    public class HashTests
    {
        [Fact]
        public void Hash_CreateTwiceWithSamePassword_ReturnsIdenticalHash()
        {
            var salt = Salt.Create();

            var hash1 = Hash.Create("password", salt);
            var hash2 = Hash.Create("password", salt);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void Hash_CreateTwiceWithDifferentPassword_ReturnsNonIdenticalHash()
        {
            var salt = Salt.Create();

            var hash1 = Hash.Create("Password", salt);
            var hash2 = Hash.Create("password", salt);

            Assert.NotEqual(hash1, hash2);
        }
    }
}