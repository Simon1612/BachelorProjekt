﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class StudyListModel
    {
        public int StudyId { get; set; }
        public string StudyName { get; set; }
    }
}
