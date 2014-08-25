using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahgora.Api.Model
{
    public class Timesheet
    {
        public string date { get; set; }
        public string workdayType { get; set; }
        public List<object> punchTime { get; set; }
        public Reasons reasons { get; set; }
        public string result { get; set; }
    }
}
