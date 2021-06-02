using System;
namespace EliteBuild.Models.View
{
    public class PagesView
    {
        public PagesView(Int32 Id, String Title, Boolean Hidden = false, String MainMenu = "", Int32 FooterMenu = -1)
        {
            this.Id = Id;
            this.Title = Title;
            this.Hidden = Hidden;
            this.MainMenu = MainMenu;
            this.FooterBlock = FooterMenu;
        }
        public Int32 Id { get; set; }
        public Boolean Hidden { get; set; }
        public String Title { get; set; }
        public String MainMenu { get; set; }
        public Int32 FooterBlock { get; set; }
    }
}