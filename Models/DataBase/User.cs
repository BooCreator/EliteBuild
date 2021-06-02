using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class User : Basic<User>
    {
        public override String Table => "[Users]";
        public override String[] Fields => new String[] { "[id]", "[login]", "[password]", "[role]", "[reg_date]", "[login_date]", "[name]" };
        public override String[] Types => new String[] { "int NOT NULL PRIMARY KEY", "varchar(25)", "varchar(40)", "int", "varchar(12)", "varchar(12)", "varchar(255)" };

        public Typle<Int32> ID { get; set; }
        public Typle<String> Login { get; set; }
        public Typle<String> Password { get; set; }
        public Typle<Int32> Role { get; set; }
        public Typle<String> Reg_date { get; set; }
        public Typle<String> Log_date { get; set; }
        public Typle<String> Name { get; set; }

        public User()
        {
            this.ID = new Typle<int>(this.Fields[0], -1);
            this.Login = new Typle<string>(this.Fields[1]);
            this.Password = new Typle<string>(this.Fields[2]);
            this.Role = new Typle<int>(this.Fields[3]);
            this.Reg_date = new Typle<string>(this.Fields[4]);
            this.Log_date = new Typle<string>(this.Fields[5]);
            this.Name = new Typle<string>(this.Fields[6]);
        }

        public Boolean Add(DataBaseExtended DB, String Name, String Login, String Password, Int32 Role)
        {
            DB.Table = this.Table;
            DB.Fields = this.Fields;
            return DB.Insert($"'{Login}', '{Password}', {Role}, '{DateTime.Now.ToString("dd-MM-yyyy")}', '{DateTime.Now.ToString("dd-MM-yyyy")}', '{Name}'") > 0;
        }

        protected override User FromObject(object[] line) =>
            new User() {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Login = new Typle<string>(this.Fields[1], To.String(line[1])),
                Password = new Typle<string>(this.Fields[2], To.String(line[2])),
                Role = new Typle<int>(this.Fields[3], To.Int(line[3])),
                Reg_date = new Typle<string>(this.Fields[4], To.String(line[4])),
                Log_date = new Typle<string>(this.Fields[5], To.String(line[5])),
                Name = new Typle<string>(this.Fields[6], To.String(line[6]))
            };
    }

}