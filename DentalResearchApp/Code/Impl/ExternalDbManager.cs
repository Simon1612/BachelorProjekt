using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MySql.Data.MySqlClient;

namespace DentalResearchApp.Code.Impl
{
    public class ExternalDbManager : IExternalDbManager
    {
         
        public List<int> GetParticipantIds(int studyId)
        {
            using (var db = GetMySqlConnection())
            {
                var query =
                    $"SELECT id_participant as ParticipantId FROM bachelordb.studyparticipant WHERE id_study = {studyId}  ORDER BY id_participant asc";


                var result = db.Query<int>(query).ToList();

                return result;
            }
        }

        public List<string> GetParticipantEmails(int studyId)
        {
            using (var db = GetMySqlConnection())
            {
                var query =
                    "SELECT email FROM bachelordb.studyparticipant " +
                    "INNER JOIN bachelordb.participant " +
                    "ON bachelordb.studyparticipant.id_studyParticipant = bachelordb.participant.id_participant " +
                    $"WHERE id_study = {studyId}";


                var result = db.Query<string>(query).ToList();

                return result;
            }
        }

        public List<ParticipantInfo> GetParticipantInfo(int studyId)
        {
            using (var db = GetMySqlConnection())
            {
                var query =
                    "SELECT bachelordb.participant.email AS Email, bachelordb.participant.id_participant as ParticipantId FROM bachelordb.participant " +
                    "INNER JOIN bachelordb.studyparticipant " +
                    "ON bachelordb.studyparticipant.id_participant = bachelordb.participant.id_participant " +
                    $"WHERE id_study = {studyId}";

                var result = db.Query<ParticipantInfo>(query).ToList();

                return result;
            }
        }



        public Study GetStudy(int studyId)
        {
            using (var db = GetMySqlConnection())
            {
                var query = $"SELECT id_study AS StudyId, Description, name as StudyName, DateCreated FROM bachelordb.study WHERE id_study = {studyId}";

                var result = db.Query<Study>(query).FirstOrDefault();

                return result;
            }
        }

        public List<StudyListModel> GetAllStudyListModels()
        {
            using (var db = GetMySqlConnection())
            {
                var query = "SELECT Name AS StudyName, id_study AS StudyId FROM bachelordb.study";

                var result = db.Query<StudyListModel>(query).ToList();

                return result;
            }
        }

        private MySqlConnection GetMySqlConnection(bool open = true, bool convertZeroDatetime = false, bool allowZeroDatetime = false)
        {
            string cs = @"Server=35.228.116.222;Database=bachelordb;user=Read;pwd=Read1234;";

            var csb = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(cs)
            {
                AllowZeroDateTime = allowZeroDatetime,
                ConvertZeroDateTime = convertZeroDatetime
            };
            var conn = new MySql.Data.MySqlClient.MySqlConnection(csb.ConnectionString);
            if (open) conn.Open();
            return conn;
        }
    }
}
