using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Building {

        public Building() {
            BuildingId = Program.buildings.Count + 1;
            string[] address = Program.LocationList[
                Program.rand.Next(Program.LocationList.Length)].Split('\t');
            State = address[0];
            City = address[1];
            District = address[2];
            string buildingAddress = State + "," + City + "," + District;
            while (true) {
                Street = Program.StreetList[Program.rand.Next(
                    Program.StreetList.Length)];
                BuildingNumber = Program.rand.Next(1, 200);
                string tmp = buildingAddress + "," + Street + "," + BuildingNumber;
                if (!Program.usedAddresses.Select(x => x == tmp).Contains(true)) {
                    Program.usedAddresses.Add(tmp);
                    break;
                }
            }
            isEnded = Program.rand.NextDouble() < 0.75 ? true : false;
        }

        public int BuildingId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
        public bool isEnded { get; set; }

        public override string ToString() {
            return "INSERT INTO Building VALUES (" + BuildingId + ",\'" +
                State + "\',\'" + City + "\',\'" + District + "\',\'" +
                Street + "\'," + BuildingNumber + ")\n";
        }

        public void CreateFlats() {
            if (!isEnded) return;
            int EnterGatesNumber = Program.rand.Next(3, 8);
            int FlatsForFloor = Program.rand.Next(2, 6);
            int FloorsNumber = Program.rand.Next(4, 12);

            for (int i = 0; i < EnterGatesNumber; i++) {
                for (short j = 0; j < FloorsNumber; j++) {
                    for (int k = FlatsForFloor - 1; k >= 0; k--) {
                        Flat flat = new Flat(
                            Program.flats.Count + 1, this.BuildingId,
                            (char)(i + 0x41), (j + 1) * FlatsForFloor - k, j);
                        Program.flats.Add(flat);
                        if (Program.isSecendGenerated) Program.secendPeriodData.Add(flat);
                        flat.CreateAgreement();
                        flat.CreateEquipment(Program.rand.Next(4, 12));
                    }
                }  
            }
        }

        public void CreateBuild() {
            List<Subcontractor> tmp = Program.subconteractors.
                Where(x => x.Type == "elektryka").ToList<Subcontractor>();
            string electricId = tmp.ElementAt(Program.rand.Next(tmp.Count)).RFC;
            tmp = Program.subconteractors.Where(x => x.Type == "budowlana").
                ToList<Subcontractor>();
            string builderId = tmp.ElementAt(Program.rand.Next(tmp.Count)).RFC;
            tmp = Program.subconteractors.Where(x => x.Type == "wykończeniowa").
                ToList<Subcontractor>();
            string finisherId = tmp.ElementAt(Program.rand.Next(tmp.Count)).RFC;

            generateBuildStartDate();
            generateBuildPlanEndDate();

            Build b1 = new Build() {
                SubcontractorFK = electricId,
                BuildingFK = BuildingId,
                StartTime = startDate,
                PlanEndTime = planEndDate
            };

            Build b2 = new Build() {
                SubcontractorFK = builderId,
                BuildingFK = BuildingId,
                StartTime = startDate,
                PlanEndTime = planEndDate
            };

            Build b3 = new Build() {
                SubcontractorFK = finisherId,
                BuildingFK = BuildingId,
                StartTime = startDate,
                PlanEndTime = planEndDate
            };

            if (isEnded) {
                generateBuildEndDate();
                b1.EndTime = endDate;
                b2.EndTime = endDate;
                b3.EndTime = endDate;
            }

            Program.builds.Add(b1);
            Program.builds.Add(b2);
            Program.builds.Add(b3);
            if (Program.isSecendGenerated) {
                Program.secendPeriodData.Add(b1);
                Program.secendPeriodData.Add(b2);
                Program.secendPeriodData.Add(b3);
            }
        }

        private DateTime startDate;
        private DateTime planEndDate;
        private DateTime endDate;

        private void generateBuildStartDate() {
            startDate = !Program.isSecendGenerated ?
                new DateTime(
                    Program.rand.Next(DateTime.Today.Year - Program.firstPeriod,
                        DateTime.Today.Year),
                    Program.rand.Next(1, 13),
                    Program.rand.Next(1, 29)
                ) :
                new DateTime(
                    Program.rand.Next(DateTime.Today.Year, DateTime.Today.Year +
                        Program.secendPeriod),
                    Program.rand.Next(1, 13),
                    Program.rand.Next(1, 29)
                );
        }

        private void generateBuildPlanEndDate() {
            planEndDate = new DateTime(
                startDate.Year + Program.rand.Next(2, 4),
                Program.rand.Next(1, 13),
                Program.rand.Next(1, 29)
            );
        }

        private void generateBuildEndDate() {
            int i = 0;
            if (Program.isSecendGenerated){
                while (true) {
                    endDate = Program.rand.NextDouble() > 0.6 ? new DateTime(
                        Program.rand.Next(startDate.Year, DateTime.Today.Year + Program.secendPeriod),
                        Program.rand.Next(1, 13),
                        Program.rand.Next(1, 29)
                        ) : planEndDate;
                    if (planEndDate > DateTime.Today + new TimeSpan(
                        Program.secendPeriod * 365, 0, 0, 0) && i > 15)
                    {
                        isEnded = false;
                        break;
                    }
                    i++;
                    if (endDate > startDate + new TimeSpan(200, 0, 0, 0) &&
                        endDate < DateTime.Today + new TimeSpan(Program.secendPeriod * 365, 0, 0, 0)) //Condition build must last for minimum 200 days
                        break;
                }
            }
            else {
                while (true) {
                    endDate = Program.rand.NextDouble() > 0.6 ? new DateTime(
                        Program.rand.Next(startDate.Year, DateTime.Today.Year),
                        Program.rand.Next(1, 13),
                        Program.rand.Next(1, 29)
                        ) : planEndDate;
                    if (planEndDate > DateTime.Today && i > 15) {
                        isEnded = false;
                        break;
                    }
                    i++;
                    if (endDate > startDate + new TimeSpan(200, 0, 0, 0) && endDate < DateTime.Today) //Condition build must last for minimum 200 days
                        break;
                }
            }
        }
        
    }
}
