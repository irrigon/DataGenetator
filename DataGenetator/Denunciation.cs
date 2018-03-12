using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Denunciation {
        public Denunciation() { }

        public int DenunciationId { get; set; }
        public int AgreementFK { get; set; }
        public DateTime Date { get; set; }
        public string Aplicant { get; set; }
        public string Reason { get; set; }

        public override string ToString() {
            return "INSERT INTO Denunciation VALUES (" + DenunciationId + "," +
                AgreementFK + ",\'" + Date.ToShortDateString() + "\',\'" +
                Aplicant + "\',\'" + Reason + "\')\n";
        }
    }
}
