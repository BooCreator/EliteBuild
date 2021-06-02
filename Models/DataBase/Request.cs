using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Request : Basic<Request>
    {

        public override String Table => "[Requests]";
        public override String[] Fields => new String[] { "[id]", "[user]", "[state]", "[calculate]", "[date]" };
        public override String[] Types => new String[] { "int NOT NULL PRIMARY KEY", "int", "int", "varchar(512)", "varchar(12)" };

        public Request()
        {
            this.ID = new Typle<int>(this.Fields[0]);
            this.User = new Typle<int>(this.Fields[1]);
            this.State = new Typle<int>(this.Fields[2]);
            this.Calculate = new Typle<string>(this.Fields[3]);
            this.Date = new Typle<string>(this.Fields[4]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<Int32> User { get; set; }
        public Typle<Int32> State { get; set; }
        public Typle<String> Calculate { get; set; }
        public Typle<String> Date { get; set; }
        public bool Add(DataBaseExtended DB, Int32 User, String Calculate)
        {
            DB.Table = this.Table;
            DB.Fields = this.Fields;
            return DB.Insert(new string[] { User.ToString(), "-1", Calculate, DateTime.Now.ToString("dd-MM-yyyy") }) > 0;
        }
        protected override Request FromObject(object[] line) =>
            new Request()
            {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                User = new Typle<int>(this.Fields[1], To.Int(line[1])),
                State = new Typle<int>(this.Fields[2], To.Int(line[2])),
                Calculate = new Typle<string>(this.Fields[3], To.String(line[3])),
                Date = new Typle<string>(this.Fields[4], To.String(line[4])),
            };
    }
}