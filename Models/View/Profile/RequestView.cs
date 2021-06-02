using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EliteBuild.Models.View
{
    public class RequestView
    {
        public RequestView(Int32 Id, String Name, String Phone, Int32 State, String Calculate, String Date)
        {
            this.Id = Id;
            this.Name = Name;
            this.Phone = Phone;
            this.State = State;
            this.Calculate = Calculate;
            this.Date = Date;
        }
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public Int32 State { get; set; }
        public String Calculate { get; set; }
        public String Date { get; set; }
    }
}