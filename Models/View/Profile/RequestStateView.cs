using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EliteBuild.Models.View
{
    public class RequestStateView
    {
        public RequestStateView(Int32 Id, String Title)
        {
            this.Id = Id;
            this.Title = Title;
        }
        public Int32 Id { get; set; }
        public String Title { get; set; }
    }
}