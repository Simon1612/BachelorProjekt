using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DentalResearchApp.Models;
using System.IO;
using DentalResearchApp.Code.Impl;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Csv;

namespace IntegrationTests
{
    public class CsvPrinterTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly TestFixture _fixture;

        public CsvPrinterTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.DropDatabases();
        }

        [Fact]
        public void CsvPrinter_PrintSurveyResults_something()
        {
            var printer = new CsvPrinter();
            string res;

            var str = File.ReadAllText(@".\TestFiles\ProductFeedbackSurvey.json");
            var json = JsonConvert.DeserializeObject<jsonModel>(str); //CsvReader.ReadFromText(str);

            string[] tempArray = new string[json.pages.Count()];
            for (int i = 0; i < json.pages.Count(); i++)
            {
                tempArray[i] = json.pages.ElementAt(i).ToString();
            }

            res = CsvWriter.WriteToText(tempArray, new List<string[]>() { tempArray });

            using (StreamWriter writer = File.CreateText(@"C:\Users\Simon\Desktop\test.csv"))
            {
                CsvWriter.Write(writer, tempArray, new List<string[]>() { tempArray });
            }

            Assert.Equal("lol", res);
        }
    }
    public class jsonModel
    {
        public IDictionary<string, string> pages { get; set; }
    }
}
