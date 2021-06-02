using System;
using DataBaseConnector;
using DataBaseConnector.Ext;

namespace EliteBuild.Models.DataBase
{

    public class FooterMenu : Basic<FooterMenu>
    {
        public override String Table => "[FooterMenu]";
        public override String[] Fields => new String[] { "[id]", "[page]", "[block]" };
        public override String[] Types => new String[] { "int NOT NULL PRIMARY KEY", "int", "int" };

        public FooterMenu()
        {
            this.ID = new Typle<int>(this.Fields[0]);
            this.Page = new Typle<int>(this.Fields[1]);
            this.Block = new Typle<int>(this.Fields[2]);
        }

        public Typle<Int32> ID { get; set; }
        public Typle<Int32> Page { get; set; }
        public Typle<Int32> Block { get; set; }
        public Boolean Add(DataBaseExtended DB, Int32 Page, Int32 Block)
        {
            DB.Table = this.Table;
            DB.Fields = this.Fields;
            return DB.Insert(new string[] { Page.ToString(), Block.ToString() }) > 0;
        }
        protected override FooterMenu FromObject(object[] line) =>
            new FooterMenu()
            {
                ID = new Typle<int>(this.Fields[0], To.Int(line[0])),
                Page = new Typle<int>(this.Fields[1], To.Int(line[1])),
                Block = new Typle<int>(this.Fields[2], To.Int(line[2])),
            };

    }
}