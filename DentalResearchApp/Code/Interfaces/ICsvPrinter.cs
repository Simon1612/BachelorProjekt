using System.IO;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ICsvPrinter
    {
        void PrintToCsv(Stream jsonToPrint, string surveyName);
    }
}
