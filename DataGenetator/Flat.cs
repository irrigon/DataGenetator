using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Flat {
        public Flat(int FlatId, int BuildingId, char EnterGateSymbol, int FlatNumber, short Floor) {
            this.FlatId = FlatId;
            this.BuildingFK = BuildingId;
            this.FlatNumber = FlatNumber;
            this.Size = Program.rand.Next(40, 121);
            this.EnterGateSymbol = EnterGateSymbol;
            this.Floor = Floor;
        }

        public int FlatId { get; set; }
        public int BuildingFK { get; set; }
        public int Size { get; set; }
        public short Floor { get; set; }
        public char EnterGateSymbol { get; set; }
        public int FlatNumber { get; set; }

        public override string ToString() {
            return "INSERT INTO Flat VALUES (" + FlatId + "," + BuildingFK +
                "," + Size + "," + Floor + ",\'" + EnterGateSymbol + "\'," +
                FlatNumber + ")\n";
        }

        public void CreateAgreement() {
            int rent = 0;
            if (this.Size < 60) rent = Program.rand.Next(1800, 3000);
            else if (this.Size < 80) rent = Program.rand.Next(2500, 5000);
            else if (this.Size < 100) rent = Program.rand.Next(4500, 7500);
            else rent = Program.rand.Next(6800, 10000);
            Agreement a = new Agreement(
                Program.agreements.Count + 1,
                this.FlatId,
                Program.rand.Next(1, 29),
                rent + .0
            );
            Program.agreements.Add(a);
            if (Program.isSecendGenerated) Program.secendPeriodData.Add(a);
            a.CreateAgreementSign();
            a.CreatePayment();
            a.CreateDenunciation();
        }

        public void CreateEquipment(int number) {
            DateTime purchaseTime;
            DateTime buildDate = Program.builds.Find(
                x => x.BuildingFK == BuildingFK).EndTime;
            if (Program.isSecendGenerated) {
                while (true) {
                    purchaseTime = Program.rand.NextDouble() < 0.8 ? buildDate ://is bought when the build was ended 
                        new DateTime(
                            Program.rand.Next(DateTime.Today.Year, DateTime.Today.Year + Program.secendPeriod),
                            Program.rand.Next(1, 13),
                            Program.rand.Next(1, 29)
                        );
                    if (purchaseTime < DateTime.Today + new TimeSpan(Program.secendPeriod * 365, 0, 0, 0) &&
                        purchaseTime >= buildDate)
                        break;
                }
            }
            else {
                while (true) {
                    purchaseTime = Program.rand.NextDouble() < 0.8 ? buildDate ://is bought when the build was ended 
                        new DateTime(
                            Program.rand.Next(buildDate.Year, DateTime.Today.Year),
                            Program.rand.Next(1, 13),
                            Program.rand.Next(1, 29)
                        );
                    if (purchaseTime < DateTime.Today && purchaseTime >= buildDate)
                        break;
                }
            }
            for (int i = 0; i < number; i++) {
                Equipment e = new Equipment() {
                    EquipmentId = Program.equipments.Count + 1,
                    FlatFK = this.FlatId,
                    PurchaseTime = purchaseTime
                };
                Program.equipments.Add(e);
                if (Program.isSecendGenerated) Program.secendPeriodData.Add(e);
            }
        }

        
    }
}
