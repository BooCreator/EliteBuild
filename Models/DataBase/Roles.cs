using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Role : Basic<Role>
    {

        public override String Table => "[Roles]";
        public override String[] Fields => new String[] { "[id]", "[title]", "[page]" };
        public override String[] Types => new String[] { "int NOT NULL PRIMARY KEY", "varchar(255)", "varchar(255)" };

        public Role()
        {
            this.ID = new Typle<int>(this.Fields[0]);
            this.Title = new Typle<string>(this.Fields[1]);
            this.Page = new Typle<string>(this.Fields[2]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<String> Title { get; set; }
        public Typle<String> Page { get; set; }

        protected override Role FromObject(object[] line) =>
            new Role() {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Title = new Typle<string>(this.Fields[1], To.String(line[1])),
                Page = new Typle<string>(this.Fields[2], To.String(line[2])),
            };
    }
}