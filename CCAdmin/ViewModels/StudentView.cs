using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCAdmin.ViewModels
{
    public class StudentView
    {
        public int StudentDetailId { get; set; }
        public string Name { get; set; }
        public string RefNo { get; set; }
        public bool Success { get; set; }
        public bool ShowCCButton { get; set; }
        public bool ShowHarrogateLetterButton { get; set; }
        public bool ShowGeneralLetterButton { get; internal set; }
    }
}
