using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Type : Basic<Type>
    {
        public override string Table => "[Types]";

        public override string[] Fields => new string[] { "[id]", "[title]", "[class]" };

        public override string[] Types => new string[] { "int NOT NULL PRIMARY KEY", "varchar(255)", "varchar(25)" };

        public Type()
        {
            this.ID = new Typle<int>(this.Fields[0], -1);
            this.Title = new Typle<string>(this.Fields[1]);
            this.Class = new Typle<string>(this.Fields[2]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<String> Title { get; set; }
        public Typle<String> Class { get; set; }

        protected override Type FromObject(object[] line)
            => new Type() {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Title = new Typle<string>(this.Fields[1], To.String(line[1])),
                Class = new Typle<string>(this.Fields[2], To.String(line[2])),
            };
    }
}