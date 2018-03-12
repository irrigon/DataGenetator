using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Payment {
        public int PaymentId { get; set; }
        public int AgreementFK { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }

        public override string ToString() {
            return "INSERT INTO Payment VALUES (" + PaymentId + "," +
                AgreementFK + ",\'" + Date.ToShortDateString() + "\'," + 
                Amount + ")\n";
        }

    }
}
