using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EliteBuild.Models.View
{
    public class NavView
    {
        public NavView(Int32 Id, String Title, String Page, Boolean Active = false)
        {
            this.Id = Id;
            this.Title = Title;
            this.Page = Page;
            this.Active = Active;
        }
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Page { get; set; }
        public Boolean Active { get; set; }
        public List<NavView> Items { get; set; } = new List<NavView>();
    }
}