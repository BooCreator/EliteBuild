using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EliteBuild.Models.View
{
    public class ModuleTypeView
    {
        public ModuleTypeView(Int32 Id, String Title, String Class = "")
        {
            this.Id = Id;
            this.Title = Title;
            this.Class = Class;
        }
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Class { get; set; }
    }
}