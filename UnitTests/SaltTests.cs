using System;
using System.Collections.Generic;
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
            var salt1 = Salt.Create();
            var salt2 = Salt.Create();

            Assert.NotEqual(salt1, salt2);
        }
    }
}
