using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class StudyModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string StudyName { get; set; }
        public string StudyDescription { get; set; }
        public List<PatientModel> Patients { get; set; }
        public List<string> Sessions { get; set; }
    }
}
