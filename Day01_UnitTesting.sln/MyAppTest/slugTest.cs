using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Day01_UnitTesting.sln.MyAPP.MyApp;

namespace Day01_UnitTesting.sln.MyAppTest
{
    public class slugTest
    {
        private readonly SlugGenerator slugGenerator;

        public slugTest()
        {
            slugGenerator = new SlugGenerator();
        }
        [Theory]
        [InlineData("Merhaba Dünya", "merhaba-dunya")]
        [InlineData("C# Geliştirici!", "c-gelistirici")]
        [InlineData("  Bu    bir   test  ", "bu-bir-test")]
        [InlineData("çığ öş üğ", "cig-os-ug")]
        [InlineData("   ", "")]
        public async Task Generate(string input, string expected)
        {
            var result = await slugGenerator.Generate(input);
            Assert.Equal(expected, result);
        }
    }
}
