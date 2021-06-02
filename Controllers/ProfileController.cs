using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DataBaseConnector;
using EliteBuild.Models.View;
using EliteBuild.Helpers;

namespace EliteBuild.Controllers
{
    public class ProfileController : Controller
    {
        
        /// <summary>
        /// Получить данные о запросах
        /// </summary>
        /// <param name="UserId">ID Пользователя для получения именных запросов</param>
        public void SetRequestData(Int32 UserId = -1)
        {
            var RequestList = new List<RequestView>();
            var RequestStates = new List<RequestStateView>();
            // подключаемся к БД
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                // получаем список состояний заявок
                foreach (var Item in Context.RequestStates.FromEntity(Context.RequestStates.Get(DB)))
                    RequestStates.Add(new RequestStateView(Item.ID.Value, Item.Title.Value));
                // если пользователь авторизован
                if (UserId > -1)
                {
                    // загружаем его заявки
                    foreach (var Item in Context.Requests.FromEntity(Context.Requests.Get(DB, $"{Context.Requests.User.Name} = {UserId}")))
                        RequestList.Add(new RequestView(Item.ID.Value, "", "", Item.State.Value, Item.Calculate.Value, Item.Date.Value));
                }
                else
                {
                    // если не авторизован, то загружаем все заявки всех пользователей (для менеджеров или администраторов)
                    foreach (var Item in Context.Requests.FromEntity(Context.Requests.Get(DB)))
                    {
                        var User = Context.Users.Find(DB, $"{Context.Users.ID.Name} = {Item.User.Value}", out _);
                        RequestList.Add(new RequestView(Item.ID.Value, (User != null) ? User.Name.Value : "", (User != null) ? User.Login.Value : "", Item.State.Value, Item.Calculate.Value, Item.Date.Value));
                    }
                }
            }
            // сохраняем данные для вывода на страницу
            this.ViewBag.Requests = RequestList;
            this.ViewBag.RequestStates = RequestStates;
        }
        public void SetPagesData()
        {
            var PagesList = new List<PagesView>();
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                var Pages = Context.Pages.FromEntity(Context.Pages.Get(DB));
                foreach (var Page in Pages)
                {
                    var tmp = new PagesView(Page.ID.Value, Page.Title.Value, (Page.Submenu.Value == -2));
                    var main = Pages.Find(x => x.ID.Value == Page.Submenu.Value);
                    if (main != null)
                        tmp.MainMenu = main.Title.Value;
                    var block = Context.FooterMenus.Find(DB, $"{Context.FooterMenus.Page.Name} = {Page.ID.Value}", out _);
                    if (block != null)
                        tmp.FooterBlock = block.Block.Value;
                    PagesList.Add(tmp);
                }
            }
            this.ViewBag.PagesData = PagesList;
        }
        /// <summary>
        /// Страница профиля для администратора
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            // получаем данные пользователя
            UserHelper.SetUserInfo(this);
            UserHelper.SetUserData(this);
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this, -2);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            // если авторизованные пользователь администратор, то получаем данные о страницах
            if (Global.GetUserRole(this) == Global.AdminRoleID)
                this.SetPagesData();
            return this.View();
        }
        /// <summary>
        /// Страница профиля для пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult Account()
        {
            // получаем данные пользователя
            UserHelper.SetUserInfo(this);
            UserHelper.SetUserData(this);
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this, -2);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            int id = Global.GetUserID(this);
            // если пользователь авторизован, то получаем список его заявок
            if (id > -1)
                this.SetRequestData(id);
            return this.View();
        }
        /// <summary>
        /// Страница профиля для пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult Manager()
        {
            // получаем данные пользователя
            UserHelper.SetUserInfo(this);
            UserHelper.SetUserData(this);
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this, -2);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            // если авторизованные пользователь менеджер или администратор, то получаем данные о заявках
            if (Global.GetUserRole(this) == Global.AdminRoleID || Global.GetUserRole(this) == Global.ManagerRoleID)
                this.SetRequestData();
            return this.View();
        }
    
    }
}