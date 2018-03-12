using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class AgreementSign {
        public string PersonFK { get; set; }
        public int AgreementFK { get; set; }

        public override string ToString() {
            return "INSERT INTO AgreementSign VALUES (" + AgreementFK + 
                ",\'" + PersonFK + "\')\n";
        }
    }
}
