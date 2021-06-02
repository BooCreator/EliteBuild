using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DataBaseConnector;
using DataBaseConnector.Ext;
using EliteBuild.Models.View;
using Newtonsoft.Json;
using EliteBuild.Helpers;


namespace EliteBuild.Controllers
{
    public class PageController : Controller
    {
        /// <summary>
        /// Последняя добавленная страница
        /// </summary>
        /// <returns></returns>
        public ActionResult LastPage()
        {
            int Id = -1;
            // подключаемся к БД
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                // получаем ИД последней страницы
                Id = DB.Scalar($"select MAX({Context.Pages.ID.Name}) from {Context.Pages.Table}");
            }
            // перегнаправляем пользователя
            return this.RedirectToAction($"/Index/{Id}");
        }
        /// <summary>
        /// Любая страница с информацией
        /// </summary>
        /// <param name="Id">ИД страницы</param>
        /// <returns></returns>
        public ActionResult Index(Int32 Id = 0)
        {
            // получаем данные пользователя
            UserHelper.SetUserInfo(this);
            UserHelper.SetUserData(this);
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this, Id);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            this.ViewBag.PageId = Id;
            // название страницы
            string PageTitle = "";
            // модули страницы
            var Modules = new List<ModuleView>();
            // типы модулей
            var ModuleTypes = new List<ModuleTypeView>();
            // подключаемся к БД
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                // получаем данные страницы
                var Page = Context.Pages.FromEntity(Context.Pages.Get(DB, $"{Context.Pages.ID.Name} = {Id}"));
                // если странциа найдена
                if (Page.Count > 0)
                {
                    // получаем название страницы
                    PageTitle = Page[0].Title.Value;
                    // получаем модули страницы из БД
                    foreach (var Item in Context.Modules.FromEntity(Context.Modules.Get(DB, $"{Context.Modules.Page.Name} = {Id}")))
                        Modules.Add(new ModuleView(Item.Type.Value, Item.Value.Value.Replace("&code_prime;", "'")));
                }
                // загружаем типы модулей
                foreach (var Item in Context.Types.FromEntity(Context.Types.Get(DB)))
                    ModuleTypes.Add(new ModuleTypeView(Item.ID.Value, Item.Title.Value, Item.Class.Value));
            }

            // сохраняем информацию для вывода на страницу
            this.ViewBag.PageTitle = PageTitle;
            this.ViewBag.Modules = Modules;
            this.ViewBag.ModuleTypes = ModuleTypes;

            return this.View();
        }
        /// <summary>
        /// Страница аккаунта
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
            // получаем ИД пользователя
            int id = Global.GetUserID(this);
            // получаем роль пользователя
            int role = Global.GetUserRole(this);
            // если пользователь авторизован
            if (id > -1)
            {
                // подключаемся к БД
                using (var DB = new DataBaseExtended(Global.ConnectionString))
                {
                    // получаем адрес страницы профиля
                    var Role = Context.Roles.Find(DB, $"{Context.Roles.ID.Name} = {role}", out _);
                    // перенаправляем пользователя на страницу профиля
                    return this.RedirectToAction($"{Role.Page.Value}", "Profile");
                }
            }
            return this.RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Страница редактора
        /// </summary>
        /// <param name="Id">ИД редактируемой страницы</param>
        /// <returns></returns>
        public ActionResult Editor(Int32 Id = -1)
        {
            // получаем данные пользователя
            UserHelper.SetUserInfo(this);
            UserHelper.SetUserData(this);
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this, -3);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            this.ViewBag.PageId = Id;
            // название страницы
            string PageTitle = "Новая страница";
            // модули страницы
            var Modules = new List<ModuleView>();
            // типы модулей страницы
            var ModuleTypes = new List<ModuleTypeView>();
            // ИД родительского пункта меню
            var PageSubmenu = -1;
            // подключаемся к БД
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                // получаем данные страницы
                var Page = Context.Pages.FromEntity(Context.Pages.Get(DB, $"{Context.Pages.ID.Name} = {Id}"));
                // если страница найдена
                if (Page.Count > 0)
                {
                    // получаем ИД родительского меню
                    PageSubmenu = Page[0].Submenu.Value;
                    // получаем название страницы
                    PageTitle = Page[0].Title.Value;
                    // получаем модули страницы
                    foreach(var Item in Context.Modules.FromEntity(Context.Modules.Get(DB, $"{Context.Modules.Page.Name} = {Id}")))
                        Modules.Add(new ModuleView(Item.Type.Value, Item.Value.Value.Replace("&code_prime;", "'")));
                }
                // получаем типы модулей страницы
                foreach (var Item in Context.Types.FromEntity(Context.Types.Get(DB)))
                    ModuleTypes.Add(new ModuleTypeView(Item.ID.Value, Item.Title.Value));
            }

            // сохраняем информацию для вывода настраницу
            this.ViewBag.PageSubmenu = PageSubmenu;
            this.ViewBag.PageTitle = PageTitle;
            this.ViewBag.Modules = Modules;
            this.ViewBag.ModuleTypes = ModuleTypes;

            return this.View();
        }
        /// <summary>
        /// Странциа калькулятора
        /// </summary>
        /// <param name="param">Калькуляция</param>
        /// <returns></returns>
        public ActionResult Calculator(String param = "")
        {
            // получаем данные пользователя
            UserHelper.SetUserInfo(this);
            UserHelper.SetUserData(this);
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this, -4);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            // работы для выбора
            var Works = new List<WorkView>();
            // калькуляция
            var Calculate = new CalculateView();
            // подключаемся к БД
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                // получаем категорию работ (Укладка ламината, укладка плитки и т.д.)
                foreach(var Category in Context.Work_Categories.FromEntity(Context.Work_Categories.Get(DB)))
                {
                    var Work_category = new WorkView(Category.ID.Value, Category.Title.Value);
                    // получаем перечень работ с ценами
                    foreach(var Work in Context.Works.FromEntity(Context.Works.Get(DB, $"{Context.Works.Category.Name} = {Category.ID.Value}")))
                    {
                        Work_category.Items.Add(new WorkItemView(Work.ID.Value, Work.Title.Value, Work.Price.Value));
                    }
                    Works.Add(Work_category);
                }
            }
            // удаляем из калюкуляции один разделитель
            param = param.Replace("[", "");
            // разбиваем калькуляцию на элементы
            var Items = From.String(param, "]");
            // если количество элементов > 1
            if (Items.Count > 1)
            {
                // получаем значение категории
                Calculate.Category = Int32.Parse(Items[0]);
                // получаем значение работы
                Calculate.Work = Int32.Parse(Items[1]);
                // получаем общую сумму
                Calculate.Price = Single.Parse(Items[2]);
                // получаем параметры комнат
                for(int i = 3; i < Items.Count; i++)
                {
                    var tmp = From.String(Items[i], ",");
                    Calculate.Items.Add(new RoomView(Int32.Parse(tmp[0]), Int32.Parse(tmp[1])));
                }
            }

            // сохраняем информацию для вывода на страницу
            this.ViewBag.Calculate = Calculate;
            this.ViewBag.Works = Works;

            return this.View();
        }
        /// <summary>
        /// Функция удаления страницы
        /// </summary>
        /// <param name="Id">ИД страницы</param>
        /// <returns></returns>
        [HttpPost]
        public String Remove(Int32 Id = -1)
        {
            // если пользователь является администратором и имеется ИД страницы
            if(Global.GetUserRole(this) == Global.AdminRoleID && Id > -1)
            {
                // подключаемся к БД
                using (var DB = new DataBaseExtended(Global.ConnectionString))
                {
                    // удаляем все модули страницы
                    Context.Modules.Remove(DB, $"{Context.Modules.Page.Name} = {Id}"); ;
                    // удаляем страницу
                    return Context.Pages.Remove(DB, $"{Context.Pages.ID.Name} = {Id}") 
                        ? Error.Accept 
                        : Error.Unknown;
                }
            } else
                return "У вас нет прав для удаления этой страницы!";
        }
        /// <summary>
        /// Функция добавления страницы
        /// </summary>
        /// <param name="Json">Данные в формате Json строки</param>
        /// <returns></returns>
        [HttpPost]
        public String Add(String Json)
        {
            // преобразуем Json в модель
            PageView model = JsonConvert.DeserializeObject<PageView>(Json);
            // если пользователь является администратором
            if (Global.GetUserRole(this) == Global.AdminRoleID)
            {
                // подключаемся к БД
                using (var DB = new DataBaseExtended(Global.ConnectionString))
                {
                    // если страница редактируется
                    if (Context.Pages.Get(DB, $"{Context.Pages.ID.Name} = {model.Id}").Lines.Count > 0)
                    {
                        // изменить данные страницы
                        if (Context.Pages.Update(DB, new string[] { Context.Pages.Title.Name, Context.Pages.Submenu.Name }, new string[] { $"'{model.Title}'", model.Submenu.ToString() }, $"{Context.Pages.ID.Name} = {model.Id}"))
                        {
                            // удали модули страницы
                            Context.Modules.Remove(DB, $"{Context.Modules.Page.Name} = {model.Id}");
                            // добавить новые модули в БД
                            foreach (var Item in model.Items)
                                Context.Modules.Add(DB, Item.Type, $"'{Item.Text.Replace("&code_lt;", "<").Replace("&code_gt;", ">").Replace("'", "&code_prime;")}'", model.Id);
                            return Error.Accept;
                        }
                        return Error.Unknown;
                    }
                    // если страница добавляется
                    else
                    {
                        // добавить новую страницу в БД
                        if (Context.Pages.Add(DB, $"'{model.Title}'", model.Submenu))
                        {
                            // получить ИД новой страницы
                            int Id = DB.Scalar($"select MAX({Context.Pages.ID.Name}) from {Context.Pages.Table}");
                            // удалить все модули, которые могли принадлежать странице с таким же ИД
                            Context.Modules.Remove(DB, $"{Context.Modules.Page.Name} = {Id}");
                            // добавляем новые модулди в БД
                            foreach (var Item in model.Items)
                                Context.Modules.Add(DB, Item.Type, $"'{Item.Text.Replace("&code_lt;", "<").Replace("&code_gt;", ">").Replace("'", "&code_prime;")}'", Id);
                            return Error.Accept;
                        }
                        return Error.Unknown;
                    }
                }
            }
            else
                return Error.NotAccess;
        }
        /// <summary>
        /// Страница инициализации базы данных
        /// </summary>
        /// <returns></returns>
        public ActionResult DataBase()
        {
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            // проверяем базу данных
            this.ViewBag.Items = Context.CheckTables();
            return this.View();
        }
        /// <summary>
        /// Функция изменения блока подвала для меню
        /// </summary>
        /// <param name="Id">ИД пункта меню</param>
        /// <param name="State">ИД блока</param>
        /// <returns></returns>
        [HttpPost]
        public String Update(Int32 Id, Int32 Block)
        {
            // проверяем, является ли пользователь администратором изи менеджером
            if (Global.GetUserRole(this) == Global.AdminRoleID)
            {
                // подключаемся к БД
                using (var DB = new DataBaseExtended(Global.ConnectionString))
                {
                    // если блок не выбран
                    if (Block < 0)
                    {
                        // удаляем страницу из меню подвала
                        Context.FooterMenus.Remove(DB, $"{Context.FooterMenus.Page.Name} = {Id}");
                        return Error.Accept;
                    }
                    else
                    {
                        // добавляем страницу в меню подвала
                        if (Context.FooterMenus.Add(DB, Id, Block))
                            return Error.Accept;
                    }
                }
                return Error.Unknown;
            }
            return Error.NotAccess;
        }
    }
}