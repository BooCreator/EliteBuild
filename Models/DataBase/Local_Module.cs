using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Local_Module : Basic<Local_Module>
    {
        public override String Table => "[Local_modules]";
        public override String[] Fields => new String[] { "[id]", "[value]" };
        public override String[] Types => new String[] { "int NOT NULL PRIMARY KEY", "varchar(255)" };

        public Local_Module()
        {
            this.ID = new Typle<int>(this.Fields[0]);
            this.Value = new Typle<string>(this.Fields[1]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<String> Value { get; set; }

        protected override Local_Module FromObject(object[] line) =>
            new Local_Module() {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Value = new Typle<string>(this.Fields[1], To.String(line[1])),
            };

    }
}