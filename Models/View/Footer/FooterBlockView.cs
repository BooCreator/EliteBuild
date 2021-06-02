using System;
using System.Collections.Generic;

namespace EliteBuild.Models.View
{
    public class FooterBlockView
    {
        public FooterBlockView(Int32 Id, String Title)
        {
            this.Id = Id;
            this.Title = Title;
        }
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public List<NavView> Items { get; set; } = new List<NavView>();
    }
}