using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Client : Person {

        public Client() : base("najemca") {
            this.Email = Program.root.generateNewWord(2) + "@gmail.com";
            this.IdCardNumber = "";
            for (int i = 0; i < 3; i++)
                this.IdCardNumber += (char)(Program.rand.Next(0, 26) + 0x41);
            this.IdCardNumber += Program.rand.Next(100000, 1000000);
            this.PhoneNumber = Program.rand.Next(100000000, 1000000000).ToString();
        }

        public string IdCardNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

       
    }
}
