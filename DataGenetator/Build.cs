using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Build {
        public Build() { }
         

        public string SubcontractorFK { get; set; }
        public int BuildingFK { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime PlanEndTime { get; set; }
        public DateTime EndTime { get; set; }

        public override string ToString() {
            return "INSERT INTO Build VALUES (\'" + SubcontractorFK + "\'," +
                BuildingFK + ",\'" + StartTime.ToShortDateString() + "\',\'" +
                PlanEndTime.ToShortDateString() + "\',\'" + 
                EndTime.ToShortDateString() + "\')\n";
        }

        public string ToUpdateString() {
            return "UPDATE Build SET EndDate = \'" + EndTime.ToShortDateString() +
                "\' WHERE FK_Building = " + BuildingFK + ";";
        }
    }
}
