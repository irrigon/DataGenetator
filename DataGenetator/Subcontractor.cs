using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Subcontractor {
        public Subcontractor() {
            this.RFC = generateRFC();
            this.Name = Program.root.generateNewWord(2);
            int type = Program.rand.Next(0, 3);
            if (type == 0) this.Type = "elektryka";
            else if (type == 1) this.Type = "budowlana";
            else this.Type = "wykończeniowa";
        }

        public string RFC { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public override string ToString() {
            return "INSERT INTO Subcontractor VALUES (\'" +
                RFC + "\',\'" + Name + "\',\'" + Type + "\')\n";
        }

        private string generateRFC() {
            while (true) {
                string result;
                int sum = 0;
                while (true) {
                    result = Program.rand.Next(100000000, 1000000000).ToString();

                    sum = 0;
                    short[] scales = new short[] { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
                    for (int i = 0; i < 9; i++) {
                        sum += scales[i] * (result[i] - 0x30);
                    }
                    if (sum % 11 != 10)
                        break;
                }
                result += sum % 11;

                if(!Program.subconteractors.Select(x => x.RFC == result).Contains(true))
                    return result;
            }
        }
        
    }
}
