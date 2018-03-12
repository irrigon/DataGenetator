using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenetator {
    class Equipment {

        public Equipment() {
            int type = Program.rand.Next(3);
            if (type == 0) this.Type = "elektronika";
            else if (type == 1) this.Type = "wyposażenie kuchni";
            else this.Type = "meble";
            this.Name = Program.root.generateNewWord(2);
            this.Price = Program.rand.Next(400, 10000);
        } 
         
        public int EquipmentId { get; set; }
        public int FlatFK { get; set; }
        public DateTime PurchaseTime { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public override string ToString() {
            return "INSERT INTO Equipment VALUES (" +
                EquipmentId + "," + FlatFK + ",\'" + Type + "\',\'" +
                Name + "\',\'" + PurchaseTime.ToShortDateString() + "\'," +
                Price + ")\n";
                 
        }
    }
}
