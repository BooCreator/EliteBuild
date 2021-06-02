using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{
    public class Page : Basic<Page>
    {
        public override String Table => "[Pages]";
        public override String[] Fields => new String[] { "[id]", "[title]", "[submenu]" };
        public override String[] Types => new String[] { "int NOT NULL PRIMARY KEY", "varchar(255)", "int" };

        public Page()
        {
            this.ID = new Typle<int>(this.Fields[0]);
            this.Title = new Typle<string>(this.Fields[1]);
            this.Submenu = new Typle<int>(this.Fields[2]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<String> Title { get; set; }
        public Typle<Int32> Submenu { get; set; }
        public Boolean Add(DataBaseExtended DB, String Title, Int32 Submenu = -1)
        {
            DB.Table = this.Table;
            DB.Fields = this.Fields;
            return DB.Insert(new string[] { Title, Submenu.ToString() }) > 0;
        }
        protected override Page FromObject(Object[] line)
        {
            return new Page() {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Title = new Typle<string>(this.Fields[1], To.String(line[1])),
                Submenu = new Typle<int>(this.Fields[2], To.Int(line[2])),
            };
        }

    }
}