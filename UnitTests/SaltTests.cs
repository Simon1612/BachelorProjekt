using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentalResearchApp.Code.Impl;
using Xunit;

namespace UnitTests
{
    public class SaltTests
    {
        [Fact]
        public void Salt_Create_UniqueSaltCreated()
        {
            var saltCount = 100;
            var listOfSalt = new List<byte[]>();

            for (int i = 0; i < saltCount; i++)
            {
                listOfSalt.Add(Salt.Create());
            }

            var uniqueSalts = listOfSalt.ToHashSet();

            Assert.Equal(100, uniqueSalts.Count);
        }
    }
}
