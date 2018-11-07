using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class CsvPrinter
    {
        //public string PrintToCsv(string jsonToPrint)
        //{

        //    StringWriter csvString = new StringWriter();

        //    using (var csv = new CsvWriter(csvString))
        //    {
        //        csv.Configuration.Delimiter = ",";

        //        using (var dt = JsonStringToTable(jsonToPrint))
        //        {
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                csv.WriteField(column.ColumnName);
        //            }
        //            csv.NextRecord();

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                for (var i = 0; i < dt.Columns.Count; i++)
        //                {
        //                    csv.WriteField(row[i]);
        //                }
        //                csv.NextRecord();
        //            }
        //        }
        //    }
        //    return csvString.ToString();
        //}

        public DataTable JsonStringToTable(string jsonContent)
        {
            var dtJObject = JsonConvert.DeserializeObject<JObject>(jsonContent);
            var dt = dtJObject.ToObject<DataTable>();
            return dt;
        }
    }
}
