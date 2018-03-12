using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Person {
        public Person() {
            this.isMale = Program.rand.Next(0, 1) == 0 ? true : false;
            this.Site = Program.rand.NextDouble() < 0.8 ? "najemca" : "wynajemca";
            this.CURP = generateCurp();
            this.Name = Program.NamesList[Program.rand.Next(Program.NamesList.Length)];
            this.SecendName = Program.rand.NextDouble() > 0.7 ?
                Program.NamesList[Program.rand.Next(Program.NamesList.Length)] : "";
            this.Surname = Program.SurnamesList[
                Program.rand.Next(Program.SurnamesList.Length)];
        }

        public Person(string Site) : this(){
            this.Site = Site;
        }

        public string CURP { get; set; }
        public string Name { get; set; }
        public string SecendName { get; set; }
        public string Surname { get; set; }
        public string Site { get; set; }
        public bool isMale { get; set; }

        public override string ToString() {
            return "INSERT INTO Person VALUES (\'" + CURP + "\',\'" + Name +
                "\',\'" + SecendName + "\',\'" + Surname + "\',\'" + Site + "\')\n";
        }

        public string ToUpdateString() {
            return "UPDATE Person SET Surname = \'" + Surname + "\' WHERE CURP = \'" +
                CURP + "\';"; 
        }

        public string generateCurp() {
            while (true) { 
                int monthOfBirth = Program.rand.Next(1, 13);
                int yearOfBirth = this.Site == "najemca" ?
                    Program.rand.Next(1901, 1998) : Program.rand.Next(1960, 1998);
                int dayOfBirth = 1;
                switch (monthOfBirth) {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        dayOfBirth = Program.rand.Next(1, 32);
                        break;
                    case 2:
                        dayOfBirth = (yearOfBirth % 4 == 0 && yearOfBirth % 100 != 0) ||
                            yearOfBirth % 400 == 0 ?
                            Program.rand.Next(1, 30) : Program.rand.Next(1, 29);
                        break;
                    default:
                        dayOfBirth = Program.rand.Next(1, 31);
                        break;
                }
                string result = "";

                result += yearOfBirth < 2000 ? yearOfBirth % 100 : yearOfBirth % 100 + 20;
                result += yearOfBirth % 100 < 10 ? "0" : "";
                result += monthOfBirth < 10 ? "0" + monthOfBirth.ToString() : monthOfBirth.ToString();
                result += dayOfBirth < 10 ? "0" + dayOfBirth.ToString() : dayOfBirth.ToString();
                result += Program.rand.Next(100, 1000);
                int tmp = Program.rand.Next(0, 5);
                result += isMale ? tmp * 2 + 1 : tmp * 2;

                int sum = 0;
                for (int i = 0; i < 10; i++) {
                    if (i % 4 == 0) sum += 9 * (result[i] - 0x30);
                    else if (i % 4 == 1) sum += 7 * (result[i] - 0x30);
                    else if (i % 4 == 2) sum += 3 * (result[i] - 0x30);
                    else sum += result[i] - 0x30;
                }
                result += sum % 10;

                if (!Program.employees.Select(x => x.CURP == result).Contains(true) &&
                    !Program.clients.Select(x => x.CURP == result).Contains(true))
                    return result;
            }
            
        }
        

    }
}
