using System;
using System.Collections.Generic;

namespace CCAdmin.Models
{
    public partial class CcdispatchQueue
    {
        public int CcdispatchQueueId { get; set; }
        public int StudentDetailId { get; set; }
        public DateTime DateStamp { get; set; }
        public string AddedBy { get; set; }
        public int? Status { get; set; }
        public string SystemRef { get; set; }
    }
}
