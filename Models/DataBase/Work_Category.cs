using System;

using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Work_Category : Basic<Work_Category>
    {

        public override String Table => "[Work_categories]";
        public override String[] Fields => new String[] { "[id]", "[title]" };
        public override String[] Types => new String[] { "int NOT NULL PRIMARY KEY", "varchar(255)" };

        public Work_Category()
        {
            this.ID = new Typle<int>(this.Fields[0]);
            this.Title = new Typle<string>(this.Fields[1]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<String> Title { get; set; }

        protected override Work_Category FromObject(object[] line) =>
            new Work_Category()
            {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Title = new Typle<string>(this.Fields[1], To.String(line[1])),
            };
    }
}