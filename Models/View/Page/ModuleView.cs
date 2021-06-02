using System;

namespace EliteBuild.Models.View
{
    public class ModuleView
    {
        public ModuleView(Int32 Type, String Text)
        {
            this.Type = Type;
            this.Text = Text;
        }
        public Int32 Type { get; set; }
        public String Text { get; set; }
    }
}