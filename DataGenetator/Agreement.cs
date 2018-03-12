using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Agreement {
        public Agreement(int AgreementId, int FlatId, int PaymentDay, double Rent) {
            this.AgreementId = AgreementId;
            this.FlatFK = FlatId;
            this.PaymentDay = PaymentDay;
            this.Rent = Rent;

            if (Program.isSecendGenerated) createSignDateSecendP();
            else createSignDateFirstP();

            this.ReceptionDate = SignDate + new TimeSpan(Program.rand.Next(1, 20), 0, 0, 0);
            this.period = Program.rand.Next(1, 3);
            this.EndTime = new DateTime(
                SignDate.Year + period,
                SignDate.Month,
                SignDate.Day
            );

            
        }

        public int AgreementId { get; set; }
        public int FlatFK { get; set; }
        public double Rent { get; set; }
        public int PaymentDay { get; set; }
        public DateTime SignDate { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime EndTime { get; set; }

        public void CreatePayment() {
            for (int i = 0; i < period * 12; i++) {
                if (Program.rand.NextDouble() > 0.89) continue;
                bool isOnTime = Program.rand.NextDouble() > 0.82 ? true : false;
                DateTime paymentDate = isOnTime ?
                    new DateTime(
                        SignDate.Year + i / 12,
                        (SignDate.Month + i % 12) % 12 + 1,
                        this.PaymentDay
                        ) :
                    new DateTime(
                        SignDate.Year + i / 12,
                        (SignDate.Month + i % 12) % 12 + 1,
                        Program.rand.Next(1, 28)
                        );
                if (paymentDate > DateTime.Today && !Program.isSecendGenerated)
                    break;
                if (paymentDate > DateTime.Today + new TimeSpan(
                    Program.secendPeriod * 365, 0, 0, 0) && Program.isSecendGenerated)
                    break;
                Payment p = new Payment() {
                    PaymentId = Program.payments.Count + 1,
                    AgreementFK = this.AgreementId,
                    Amount = Program.rand.NextDouble() < 0.95 ?
                        this.Rent : Program.rand.Next(1, (int)this.Rent),
                    Date = paymentDate
                };
                Program.payments.Add(p);
                if (Program.isSecendGenerated) Program.secendPeriodData.Add(p);
            }
        }

        public void CreateDenunciation() {
            if (Program.rand.NextDouble() < 0.95) return;
            DateTime date;
            while (true) {
                date = new DateTime(
                    SignDate.Year + Program.rand.Next(period + 1),
                    (SignDate.Month + Program.rand.Next(12)) % 12 + 1,
                    (SignDate.Day + Program.rand.Next(28)) % 28 + 1
                    );
                if (date > SignDate) {
                    if (date < DateTime.Today && !Program.isSecendGenerated)
                        break;
                    if (date < DateTime.Today + new TimeSpan(Program.secendPeriod * 365, 0, 0, 0) &&
                        Program.isSecendGenerated)
                        break;
                }
            }
            string site = Program.rand.NextDouble() < 0.7 ?
                    "najemca" : "wynajemca";
            Denunciation d = new Denunciation() {
                DenunciationId = Program.denunciations.Count + 1,
                AgreementFK = this.AgreementId,
                Aplicant = site,
                Reason = site == "najemca" ? Program.ClientReasonsList.ElementAt(
                    Program.rand.Next(Program.ClientReasonsList.Length)) :
                    Program.CompanyReasonList.ElementAt(Program.rand.Next(
                        Program.CompanyReasonList.Length)),
                Date = date
            };
            Program.denunciations.Add(d);
            if (Program.isSecendGenerated) Program.secendPeriodData.Add(d);
        }

        public void CreateAgreementSign() {
            int numberOfClients = Program.rand.Next(1, 5);

            AgreementSign a = new AgreementSign() {
                AgreementFK = this.AgreementId,
                PersonFK = employeeCURP
            };
            Program.agreementSings.Add(a);
            if (Program.isSecendGenerated) Program.secendPeriodData.Add(a);
            for (int i = 0; i < numberOfClients; i++) {
                Client client = new Client();
                Program.clients.Add(client);
                if (Program.isSecendGenerated) Program.secendPeriodData.Add(client);
                a = new AgreementSign() {
                    AgreementFK = this.AgreementId,
                    PersonFK = client.CURP
                };
                Program.agreementSings.Add(a);
                if (Program.isSecendGenerated) Program.secendPeriodData.Add(a);
            }
        }

        public override string ToString() {
            return "INSERT INTO Agreement VALUES (" + AgreementId + "," +
                FlatFK + "," + Rent + "," + PaymentDay + ",\'" + 
                SignDate.ToShortDateString() + "\',\'" + 
                ReceptionDate.ToShortDateString() + "\',\'" +
                EndTime.ToShortDateString() + "\')\n";
        }
        private string employeeCURP;

        private int period;

        private void createSignDateFirstP() {
            while (true) {
                DateTime signDate = new DateTime(
                    Program.rand.Next(DateTime.Today.Year - Program.firstPeriod, DateTime.Today.Year),
                    Program.rand.Next(1, 13),
                    Program.rand.Next(1, 29)
                );
                List<Employee> tmp = Program.employees.Where(
                x => x.District == Program.buildings.ElementAt(
                Program.flats.ElementAt(FlatFK - 1).BuildingFK - 1).
                District).ToList<Employee>();
                tmp = tmp.Where(x => x.Position == "konsultant").ToList<Employee>();
                tmp = tmp.Where(x => x.StartTime <= signDate && x.StopTime >= signDate).
                    ToList<Employee>();
                if (tmp.Count != 0 && signDate < DateTime.Today) {
                    this.employeeCURP = tmp.ElementAt(Program.rand.Next(tmp.Count)).CURP;
                    this.SignDate = signDate;
                    break;
                }
            }
        }

        private void createSignDateSecendP() {
            while (true) {
                DateTime signDate = new DateTime(
                    Program.rand.Next(DateTime.Today.Year, DateTime.Today.Year + 
                    Program.secendPeriod),
                    Program.rand.Next(1, 13),
                    Program.rand.Next(1, 29)
                );
                List<Employee> tmp = Program.employees.Where(
                x => x.District == Program.buildings.ElementAt(
                Program.flats.ElementAt(FlatFK - 1).BuildingFK - 1).
                District).ToList<Employee>();
                tmp = tmp.Where(x => x.Position == "konsultant").ToList<Employee>();
                tmp = tmp.Where(x => x.StartTime <= signDate && x.StopTime >= signDate).
                    ToList<Employee>();
                if (tmp.Count != 0 && signDate < DateTime.Today) {
                    this.employeeCURP = tmp.ElementAt(Program.rand.Next(tmp.Count)).CURP;
                    this.SignDate = signDate;
                    break;
                }
            }
        }

        

    }
}
