using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EliteBuild.Models.View
{
    public class PageView
    {
        public Int32 Id { get; set; }
        public string Title { get; set; }
        public Int32 Submenu { get; set; }
        public List<ModuleView> Items { get; set; }

    }
}