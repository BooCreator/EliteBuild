using System;
using System.Collections.Generic;

namespace EliteBuild.Models.View
{
    public class WorkView
    {
        public WorkView(Int32 Id, String Title)
        {
            this.Id = Id;
            this.Title = Title;
        }
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public List<WorkItemView> Items { get; set; } = new List<WorkItemView>();
    }

    public class WorkItemView
    {
        public WorkItemView(Int32 Id, String Title, Single Price)
        {
            this.Id = Id;
            this.Title = Title;
            this.Price = Price;
        }
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public Single Price { get; set; }
    }
}