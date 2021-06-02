using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EliteBuild.Models.View
{
    public class ProfileView
    {
        public ProfileView(Int32 Id, String Name, String Phone)
        {
            this.Id = Id;
            this.Name = Name;
            this.Phone = Phone;
        }
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
    }
}