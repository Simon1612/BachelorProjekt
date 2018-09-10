using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DentalResearchApp
{
    public class SessionStorage
    {
        private ISession session;

        public SessionStorage(ISession session)
        {
            this.session = session;
        }

        public T GetFromSession<T>(string storageId, T defaultValue)
        {
            if (string.IsNullOrEmpty(session.GetString(storageId)))
            {
                session.SetString(storageId, JsonConvert.SerializeObject(defaultValue));
            }
            var value = session.GetString(storageId);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public Dictionary<string, string> GetSurveys()
        {
            Dictionary<string, string> surveys = new Dictionary<string, string>();

            surveys["Product feedback survey"] = File.ReadAllText(@"Views\Survey\Json\ProductFeedbackSurvey.json");
            surveys["Customer and his/her partner income survey"] = File.ReadAllText(@"Views\Survey\Json\IncomeSurvey.json"); 

            return GetFromSession<Dictionary<string, string>>("SurveyStorage", surveys);
        }

        public Dictionary<string, List<string>> GetResults()
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
            results["Product feedback survey"] = new List<string>(
                new string[] {
                    @"{""Quality"":{""affordable"":""5"",""better then others"":""5"",""does what it claims"":""5"",""easy to use"":""5""},""satisfaction"":5,""recommend friends"":5,""suggestions"":""I am happy!"",""price to competitors"":""Not sure"",""price"":""low"",""pricelimit"":{""mostamount"":""100"",""leastamount"":""100""}}",
                    @"{""Quality"":{""affordable"":""3"",""does what it claims"":""2"",""better then others"":""2"",""easy to use"":""3""},""satisfaction"":3,""suggestions"":""better support"",""price to competitors"":""Not sure"",""price"":""high"",""pricelimit"":{""mostamount"":""60"",""leastamount"":""10""}}"
                }
            );
            results["Customer and his/her partner income survey"] = new List<string>(
                new string[] {
                    @"{""member_arrray_employer"":[{}],""partner_arrray_employer"":[{}],""maritalstatus_c"":""Married"",""member_receives_income_from_employment"":""0"",""partner_receives_income_from_employment"":""0""}",
                    @"{""member_arrray_employer"":[{}],""partner_arrray_employer"":[{}],""maritalstatus_c"":""Single"",""member_receives_income_from_employment"":""1"",""member_type_of_employment"":[""Self employment""],""member_seasonal_intermittent_or_contract_work"":""0""}"
                }
            );
            return GetFromSession<Dictionary<string, List<string>>>("ResultsStorage", results);
        }

        public string GetSurvey(string surveyId)
        {
            return GetSurveys()[surveyId];
        }

        public void StoreSurvey(string surveyId, string jsonString)
        {
            var storage = GetSurveys();
            storage[surveyId] = jsonString;
            session.SetString("SurveyStorage", JsonConvert.SerializeObject(storage));
        }

        public void ChangeName(string id, string name)
        {
            var storage = GetSurveys();
            storage[name] = storage[id];
            storage.Remove(id);
            session.SetString("SurveyStorage", JsonConvert.SerializeObject(storage));
        }

        public void DeleteSurvey(string surveyId)
        {
            var storage = GetSurveys();
            storage.Remove(surveyId);
            session.SetString("SurveyStorage", JsonConvert.SerializeObject(storage));
        }

        public void PostResults(string postId, string resultJson)
        {
            var storage = GetResults();
            if (!storage.ContainsKey(postId))
            {
                storage[postId] = new List<string>();
            }
            storage[postId].Add(resultJson);
            session.SetString("ResultsStorage", JsonConvert.SerializeObject(storage));
        }

        public List<string> GetResults(string postId)
        {
            var storage = GetResults();
            return storage.ContainsKey(postId) ? storage[postId] : new List<string>();
        }
    }
}
