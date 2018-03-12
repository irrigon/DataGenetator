using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SI_Projekt;

namespace DataGenetator {
    class Program {
        static void Main(string[] args) {

            firstPeriod = Int32.Parse(args[0]);
            secendPeriod = Int32.Parse(args[1]);
            
            loadData();

            //First period generated data
            createSubcontractors(100);
            createEmployees(500);
            createBuildings(40);

            //First period files
            createSqlFile("insert.sql");
            createClientsCsv("clients.csv");
            createEmloyeeCsv("employees.csv");

            isSecendGenerated = true;

            //Secend period generated data
            createSubcontractors(15);
            createEmployees(60);
            createBuildings(6);
            updateBuilds();
            updateClients();
            updateEmployees();

            //Secend period files
            createUpdatedSqlFile("update.sql");
            createClientsCsv("clients_2.csv");
            createEmloyeeCsv("employees_2.csv");
            
        }

        public static int firstPeriod;
        public static int secendPeriod;
        public static bool isSecendGenerated = false;

        public static string[] NamesList;
        public static string[] SurnamesList;
        public static string[] ClientReasonsList;
        public static string[] CompanyReasonList;
        public static string[] LocationList;
        public static string[] StreetList;
        public static string[] DistrictList;
        public static Random rand = new Random();
        public static Nodev2 root = new Nodev2(' ');

        public static List<Employee> employees = new List<Employee>();
        public static List<Client> clients = new List<Client>();
        public static List<Building> buildings = new List<Building>();
        public static List<Flat> flats = new List<Flat>();
        public static List<Agreement> agreements = new List<Agreement>();
        public static List<Subcontractor> subconteractors = new List<Subcontractor>();
        public static List<Payment> payments = new List<Payment>();
        public static List<Denunciation> denunciations = new List<Denunciation>();
        public static List<Build> builds = new List<Build>();
        public static List<AgreementSign> agreementSings = new List<AgreementSign>();
        public static List<Equipment> equipments = new List<Equipment>();
        public static List<string> usedAddresses = new List<string>();

        public static List<object> secendPeriodData = new List<object>();
        public static List<Build> updatedBuilds = new List<Build>();
        public static List<Person> updatedPerson = new List<Person>();

        private static void loadData() {
            root.createGraph(2);
            try {
                root.teach(2, "WordList.txt");
                NamesList = File.ReadAllLines(Path.GetFullPath("CSV_Database_of_First_Names.csv"));
                SurnamesList = File.ReadAllLines(Path.GetFullPath("CSV_Database_of_Last_Names.csv"));
                ClientReasonsList = File.ReadAllLines(Path.GetFullPath("ClientReasons.txt"));
                CompanyReasonList = File.ReadAllLines(Path.GetFullPath("CompanyReasons.txt"));
                LocationList = File.ReadAllLines(Path.GetFullPath("Location.txt"));
                StreetList = File.ReadAllLines(Path.GetFullPath("StreetsList.txt"));
                DistrictList = File.ReadAllLines(Path.GetFullPath("DistrictsList.txt"));
            }
            catch (IOException e) {
                System.Console.Error.WriteLine("Error while reading files");
            }
        }

        private static void createSubcontractors(int number) {
            for (int i = 0; i < number; i++) {
                Subcontractor s = new Subcontractor();
                Program.subconteractors.Add(s);
                if (isSecendGenerated) secendPeriodData.Add(s);
            }
        }

        private static void createEmployees(int number) {
            for (int i = 0; i < number; i++) {
                Employee e = new Employee();
                employees.Add(e);
                if (isSecendGenerated) secendPeriodData.Add(e);
            }
        }

        private static void createBuildings(int number) {
            for (int i = 0; i < number; i++) {
                Building b = new Building();
                buildings.Add(b);
                if (isSecendGenerated) secendPeriodData.Add(b);
                b.CreateBuild();
                b.CreateFlats();
            }
        }

        private static void createSqlFile(string fileName) {
            using (StreamWriter writer = new StreamWriter(Path.GetFullPath(fileName))) {
                employees.ForEach(x => writer.Write(x));
                clients.ForEach(x => writer.Write(x));
                buildings.ForEach(x => writer.Write(x));
                flats.ForEach(x => writer.Write(x));
                agreements.ForEach(x => writer.Write(x));
                subconteractors.ForEach(x => writer.Write(x));
                payments.ForEach(x => writer.Write(x));
                denunciations.ForEach(x => writer.Write(x));
                builds.ForEach(x => writer.Write(x));
                agreementSings.ForEach(x => writer.Write(x));
                equipments.ForEach(x => writer.Write(x));

            }
        }

        private static void createClientsCsv(string fileName) {
            using (StreamWriter writer = new StreamWriter(Path.GetFullPath(fileName))) {
                clients.ForEach(x => writer.WriteLine(x.CURP + "," + x.Name + "," +
                    x.SecendName + "," + x.Surname + "," + x.IdCardNumber + "," +
                    x.Email + "," + x.PhoneNumber));
            }
        }

        private static void createEmloyeeCsv(string fileName) {
            using (StreamWriter writer = new StreamWriter(Path.GetFullPath(fileName))) {
                foreach (Employee e in employees) {
                    string result = e.CURP + "," + e.Name + "," +
                    e.SecendName + "," + e.Surname + "," + e.District + "," +
                    e.Position + "," + e.StartTime.ToShortDateString() + ",";
                    result += e.isEnded ? e.StopTime.ToShortDateString() + ",": ",";
                    writer.WriteLine(result);
                }
            }
        }

        private static void updateBuilds() {
            DateTime endTime = DateTime.Today;
            bool isEnded = false;
            foreach (Build build in builds) {
                int i = 0;
                if (builds.IndexOf(build) % 3 == 0) { 
                    while (true) {
                        endTime = new DateTime(
                            rand.Next(DateTime.Today.Year, DateTime.Today.Year + secendPeriod),
                            rand.Next(1, 13),
                            rand.Next(1, 29)
                            );
                        if (endTime > build.StartTime + new TimeSpan(200, 0, 0, 0) &&
                            endTime < DateTime.Today + new TimeSpan(secendPeriod * 365, 0, 0, 0)) {
                            isEnded = rand.NextDouble() < 0.75 ? true : false;
                            break;
                        }
                        i++;
                        if (i == 15) {
                            isEnded = false;
                            break;
                        } 

                    }
                }
                if (!buildings.Find(x => x.BuildingId == build.BuildingFK).isEnded) {
                    if (isEnded) {
                        build.EndTime = endTime;
                        updatedBuilds.Add(build);
                    }
                }
            }
        }

        private static void updateEmployees() {
            //List<Employee> update = new List<Employee>();
            for (int i = 0; i < employees.Count; i++) {
                //change position
                Employee employee = employees.ElementAt(i);
                DateTime changeDate = new DateTime(
                    rand.Next(DateTime.Today.Year, DateTime.Today.Year + secendPeriod),
                    rand.Next(1, 13),
                    rand.Next(1, 29)
                    );
                if (employees.ElementAt(i).Position != "kierownik oddziału" && rand.NextDouble() < 0.06) {
                    employee.StartTime = changeDate;
                    employee.Position = "kierownik oddziału";
                    employees.ElementAt(i).StopTime = changeDate;
                    employees.Add(employee);
                }
                else if (employees.ElementAt(i).Position == "kierownik oddziału" && rand.NextDouble() < 0.1) {
                    int pos = rand.Next(3);
                    if (pos == 0) employee.Position = "konsultant";
                    else if (pos == 1) employee.Position = "prawnik";
                    else employee.Position = "inżynier budowlaniec";
                    employee.StartTime = changeDate;
                    employees.ElementAt(i).StopTime = changeDate;
                    employees.Add(employee);
                }
                else if (employees.ElementAt(i).Position == "konsultant" && rand.NextDouble() < 0.08) {
                    employee.Position = rand.Next(2) == 1 ? "prawnik" :
                        "inżynier budowlaniec";
                    employee.StartTime = changeDate;
                    employees.ElementAt(i).StopTime = changeDate;
                    employees.Add(employee);
                }
                //end work
                else if (rand.NextDouble() < 0.2) 
                    employees.ElementAt(i).StopTime = changeDate;
                

                //change surname
                if (rand.NextDouble() < 0.1) {
                    employees.ElementAt(i).Surname = SurnamesList[rand.Next(SurnamesList.Length)];
                    updatedPerson.Add(employees.ElementAt(i));
                }
                //change district
                employee = employees.ElementAt(i);
                changeDate = new DateTime(
                    rand.Next(DateTime.Today.Year, DateTime.Today.Year + secendPeriod),
                    rand.Next(1, 13),
                    rand.Next(1, 29)
                    );
                if (rand.NextDouble() < 0.13) {
                    while (true) {
                        string district = DistrictList[rand.Next(DistrictList.Length)];
                        if (employees.ElementAt(i).District != district) {
                            employee.District = district;
                            employee.StartTime = changeDate;
                            employees.ElementAt(i).StopTime = changeDate;
                            employees.Add(employee);
                            break;
                        }
                    }
                }
            }
        }

        private static void updateClients() {
            for (int i = 0; i < clients.Count; i++) {
                //change surname
                if (rand.NextDouble() < 0.1) {
                    clients.ElementAt(i).Surname = SurnamesList[rand.Next(SurnamesList.Length)];
                    updatedPerson.Add(clients.ElementAt(i));
                }
                //change e-mail
                if (rand.NextDouble() < 0.13)
                    clients.ElementAt(i).Email = root.generateNewWord(2) + "@gmail.com";
                //change phone number
                if (rand.NextDouble() < 0.19)
                    clients.ElementAt(i).PhoneNumber = rand.Next(100000000, 1000000000).ToString();
            }
        }

        private static void createUpdatedSqlFile(string fileName) {
            using (StreamWriter writer = new StreamWriter(Path.GetFullPath(fileName))) {
                updatedBuilds.ForEach(x => writer.WriteLine(x.ToUpdateString()));
                updatedPerson.ForEach(x => writer.WriteLine(x.ToUpdateString()));
                secendPeriodData.ForEach(x => writer.Write(x));
            }
        }
    }
}
