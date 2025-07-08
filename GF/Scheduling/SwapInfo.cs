using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Scheduling
{
    public class SwapInfo
    {
        public string FromFile { get; set; }   // "May24.gfs"
        public string ToFile { get; set; }   // "June24.gfs"
        public string Day { get; set; }   // "3, Fri."
        public string FromEmployee { get; set; }   // "Alice"
        public string ToEmployee { get; set; }   // "Bob"
        public string FromShift { get; set; }   // "9-15"
        public string ToShift { get; set; }   // "15-21" або null
        public DateTime When { get; set; }   // DateTime.Now    }
    }
}
