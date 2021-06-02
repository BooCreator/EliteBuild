using System;

using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Work : Basic<Work>
    {
        public override string Table => "[Works]";

        public override string[] Fields => new string[] { "[id]", "[title]", "[category]", "[price]" };

        public override string[] Types => new string[] { "int NOT NULL PRIMARY KEY", "varchar(255)", "int", "float" };

        public Work()
        {
            this.ID = new Typle<int>(this.Fields[0], -1);
            this.Title = new Typle<string>(this.Fields[1]);
            this.Category = new Typle<int>(this.Fields[2]);
            this.Price = new Typle<float>(this.Fields[3]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<String> Title { get; set; }
        public Typle<Int32> Category { get; set; }
        public Typle<Single> Price { get; set; }

        public Boolean Add(DataBaseExtended DB, Int32 Type, String Value, Int32 Page)
        {
            DB.Table = this.Table;
            DB.Fields = this.Fields;
            return DB.Insert(new string[] { Type.ToString(), Value, Page.ToString() }) > 0;
        }

        protected override Work FromObject(object[] line)
            => new Work()
            {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Title = new Typle<string>(this.Fields[1], To.String(line[1])),
                Category = new Typle<int>(this.Fields[2], To.Int(line[2])),
                Price = new Typle<float>(this.Fields[3], (float)To.Float(line[3])),
            };
    }
}