using System;
using System.Collections.Generic;

namespace CCAdmin.Models
{
    public partial class ApplicantCoursesThisYearAndNext
    {
        public int StudentDetailId { get; set; }
        public int StudentId { get; set; }
        public string AcademicYearId { get; set; }
        public string RefNo { get; set; }
        public string FirstForename { get; set; }
        public string Surname { get; set; }
        public int OfferingId { get; set; }
        public string Code { get; set; }
        public int? OfferId { get; set; }
        public DateTime? OfferDate { get; set; }
        public int? DecisionId { get; set; }
        public DateTime? DecisionDate { get; set; }
        public int? CollegeDecisionId { get; set; }
        public int OfferingTypeId { get; set; }
        public byte Choice { get; set; }
    }
}
