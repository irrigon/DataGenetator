using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Employee : Person {

        public Employee() : base("wynajemca") {
            double position = Program.rand.NextDouble();
            if (position < 0.05) this.Position = "kierownik oddziału";
            else if (position < 0.2) this.Position = "prawnik";
            else if (position < 0.8) this.Position = "konsultant";
            else this.Position = "inżynier budowlaniec";
            this.District = Program.DistrictList[Program.rand.Next(
                Program.DistrictList.Length)];
            if (Program.isSecendGenerated) createDateSecendP();
            else createDateFirstP();
        }
        
        public string District { get; set; }
        public string Position { get; set; }
        public bool isEnded { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }

        private void createDateFirstP() {
            this.StartTime = new DateTime(
                Program.rand.Next(DateTime.Today.Year - Program.firstPeriod,
                    DateTime.Today.Year),
                Program.rand.Next(1, 13),
                Program.rand.Next(1, 28)
                );
            if (Program.rand.NextDouble() > 0.82) {
                while (true) {
                    DateTime tmp = new DateTime(
                            Program.rand.Next(StartTime.Year, DateTime.Today.Year),
                            Program.rand.Next(1, 13),
                            Program.rand.Next(1, 29)
                        );
                    if (tmp > StartTime) {
                        this.isEnded = true;
                        this.StopTime = tmp;
                        break;
                    }
                }
            }
            else
                this.StopTime = DateTime.MaxValue;
        }

        private void createDateSecendP() {
            this.StartTime = new DateTime(
                Program.rand.Next(DateTime.Today.Year,
                    DateTime.Today.Year + Program.secendPeriod),
                Program.rand.Next(1, 13),
                Program.rand.Next(1, 28)
                );
            if (Program.rand.NextDouble() > 0.82) {
                while (true) {
                    DateTime tmp = new DateTime(
                            Program.rand.Next(StartTime.Year, DateTime.Today.Year
                            + Program.secendPeriod),
                            Program.rand.Next(1, 13),
                            Program.rand.Next(1, 29)
                        );
                    if (tmp > StartTime) {
                        this.isEnded = true;
                        this.StopTime = tmp;
                        break;
                    }
                }
            }
            else
                this.StopTime = DateTime.MaxValue;
        }
    }
}
