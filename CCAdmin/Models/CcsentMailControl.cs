using System;
using System.Collections.Generic;

namespace CCAdmin.Models
{
    public partial class CcsentMailControl
    {
        public int CcsentMailId { get; set; }
        public int StudentDetailId { get; set; }
        public string Receipiant { get; set; }
        public DateTime? DateSent { get; set; }
        public int Attempts { get; set; }
    }
}
