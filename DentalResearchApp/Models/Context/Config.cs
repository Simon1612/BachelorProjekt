namespace DentalResearchApp.Models.Context
{

    public interface IConfig
    {
        string MongoConnectionString { get; }
    }

    public class TestConfig : IConfig
    {
        public string MongoConnectionString { get; } = "asdf";
    }

    public class ProdConfig : IConfig
    {
        public string MongoConnectionString { get; } = "mongodb+srv://test:test@2018e21-surveydb-wtdmw.mongodb.net/test?retryWrites=true";
    }
}
