using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DataBaseConnector;
using EliteBuild.Models.View;

namespace EliteBuild.Helpers
{
    public static class MenuHelper
    {
        /// <summary>
        /// Функция генерации главного меню
        /// </summary>
        /// <param name="Controller">Контроллер приложения</param>
        /// <param name="Id">ID активной страницы</param>
        /// <returns></returns>
        public static List<NavView> GenerateNav(Controller Controller, Int32 Id = -1)
        {
            Controller.ViewBag.ActivePage = Id;
            var Items = new List<NavView>();
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                var pages = Context.Pages.FromEntity(Context.Pages.Get(DB, $"{Context.Pages.Submenu.Name} > -2"));
                foreach (Models.DataBase.Page item in pages)
                {
                    if (item.Submenu.Value < 0)
                        Items.Add(new NavView(item.ID.Value, item.Title.Value, $"/Page/Index/{item.ID.Value}", (item.ID.Value == Id)));
                }
                foreach (Models.DataBase.Page item in pages)
                {
                    if (item.Submenu.Value > -1)
                    {
                        var menu = Items.Find(x => x.Id == item.Submenu.Value);
                        if (menu != null)
                            menu.Items.Add(new NavView(item.ID.Value, item.Title.Value, $"/Page/Index/{item.ID.Value}", (item.ID.Value == Id)));
                    }
                }
            }
            return Items;
        }
        /// <summary>
        /// Функция генерации меню в подвале сайта
        /// </summary>
        /// <param name="Controller"></param>
        public static void SetFooterMenu(Controller Controller)
        {
            var Items = new List<FooterBlockView>();
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                foreach(var Item in Context.FooterBlocks.FromEntity(Context.FooterBlocks.Get(DB)))
                {
                    var Block = new FooterBlockView(Item.ID.Value, Item.Title.Value);
                    foreach(var Menu in Context.FooterMenus.FromEntity(Context.FooterMenus.Get(DB, $"{Context.FooterMenus.Block.Name} = {Block.Id}")))
                    {
                        var menu = Context.Pages.Find(DB, $"{Context.Pages.ID.Name} = {Menu.Page.Value}", out _);
                        if(menu != null)
                            Block.Items.Add(new NavView(menu.ID.Value, menu.Title.Value, $"/Page/Index/{menu.ID.Value}"));
                    }
                    Items.Add(Block);
                }
            }
            Controller.ViewBag.FooterMenu = Items;
        }
    }
}