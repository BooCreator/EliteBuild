using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Module : Basic<Module>
    {
        public override string Table => "[Modules]";

        public override string[] Fields => new string[] { "[id]", "[type]", "[value]", "[page]" };

        public override string[] Types => new string[] { "int NOT NULL PRIMARY KEY", "int", "varchar(8000)", "int" };

        public Module()
        {
            this.ID = new Typle<int>(this.Fields[0], -1);
            this.Type = new Typle<int>(this.Fields[1]);
            this.Value = new Typle<string>(this.Fields[2]);
            this.Page = new Typle<int>(this.Fields[3]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<Int32> Type { get; set; }
        public Typle<String> Value { get; set; }
        public Typle<Int32> Page { get; set; }

        public Boolean Add(DataBaseExtended DB, Int32 Type, String Value, Int32 Page)
        {
            DB.Table = this.Table;
            DB.Fields = this.Fields;
            return DB.Insert(new string[] { Type.ToString(), Value, Page.ToString() }) > 0;
        }

        protected override Module FromObject(object[] line)
            => new Module() {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Type = new Typle<int>(this.Fields[1], To.Int(line[1])),
                Value = new Typle<string>(this.Fields[2], To.String(line[2])),
                Page = new Typle<int>(this.Fields[3], To.Int(line[3])),
            };
    }
}