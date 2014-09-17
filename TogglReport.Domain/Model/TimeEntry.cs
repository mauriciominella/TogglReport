using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.Domain.Model
{
    public class TimeEntry
    {
        public string description { get; set; }
        public DateTime start { get; set; }
        public double duration { get; set; }
        public bool duronly { get; set; }
        public string created_with { get; set; }
        public object ui_modified_at { get; set; }
        public object dirty_at { get; set; }
        public int id { get; set; }
        public string guid { get; set; }
        public int wid { get; set; }
        public bool billable { get; set; }
        public string at { get; set; }
        public int server_id { get; set; }
        public List<string> tags { get; set; }
        public DateTime stop { get; set; }

        public String durationInHours
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(this.duration);
                return span.ToString();
            }
        }

        public double percent { get; set; }
        public double hoursSuggested { get; set; }
        public double hoursSuggestedRounded { get; set; }

        public bool isTimesheet { get; set; }
    }
}
