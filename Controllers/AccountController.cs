using System;
using System.Web.Security;
using System.Web.Mvc;
using DataBaseConnector;
using System.Web.Configuration;
using EliteBuild.Models.DataBase;
using System.Text;
using EliteBuild.Helpers;

namespace EliteBuild.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Функция генерации SHA1-хэша для строки.
        /// </summary>
        /// <param name="str">Входящая строка</param>
        /// <returns></returns>
        public static String SHA1(String str)
            => Encoding.UTF8.GetString(getHash(Encoding.UTF8.GetBytes(str))).Replace("'", "\"");
        /// <summary>
        /// Функция генерации SHA1-хэша для массива байт
        /// </summary>
        /// <param name="bytes">Массив символов строки</param>
        /// <returns></returns>
        private static byte[] getHash(byte[] bytes)
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                byte[] hash = sha.ComputeHash(sha.ComputeHash(bytes));
                return hash;
            }
        }
        /// <summary>
        /// Открыть страницу логина
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            // инициализируем главное меню
            this.ViewBag.MainMenu = MenuHelper.GenerateNav(this, -2);
            // инициализируем меню в подвале сайта
            MenuHelper.SetFooterMenu(this);
            return this.View(new Account.LoginModel());
        }
        /// <summary>
        /// Функция авторизации
        /// </summary>
        /// <param name="model">Модель с данными пользователя</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account.LoginModel model)
        {
            // Если модель заполнена корректно
            if (this.ModelState.IsValid)
            {
                // продключаемся к БД
                using (var DB = new DataBaseExtended(Global.ConnectionString))
                {
                    // находим пользователя с подходящим логином
                    User user = Context.Users.Find(DB, $"{Context.Users.Login.Name} like '{model.Phone}'", out bool _);
                    // если пользователь найден и у него не пустой пароль
                    if (user != null && user.Password.Value.Length > 0)
                    {
                        // проверяем, совпадает ли введённый пароль с паролем из БД
                        user = Context.Users.Find(DB,
                        $"{Context.Users.Login.Name} like '{model.Phone}' and {Context.Users.Password.Name} like '{SHA1(model.Password)}'",
                        out bool _);
                        if (user != null)
                        {
                            // если совпадает, то авторизуем пользователя
                            Global.LoginedUser.Add(new UserData(user.ID.Value, user.Login.Value, user.Role.Value));
                            FormsAuthentication.SetAuthCookie(model.Phone, true);
                            return this.RedirectToAction("Index", "Page");
                        } else
                            this.ModelState.AddModelError("", "Пользователя с таким логином и паролем нет!");
                    }
                    else
                    {
                        // проверяем, пользователь был не найден или у него пустой пароль
                        bool res = (user == null)
                            // если был не найден, то создаём нового пользователя
                            ? Context.Users.Add(DB, model.Name, model.Phone, SHA1(model.Password).ToString(), 0)
                            // если был пустой пароль, то добавляем вместо него нормальный
                            : Context.Users.Update(DB, $"{Context.Users.Password.Name}", $"'{SHA1(model.Password)}'", $"{Context.Users.ID.Name} = {user.ID.Value}");
                        // если предыдущая операция добавления/обновления прошла успешно
                        if (res)
                        {
                            // получаем ID пользователя
                            int newID = (user == null)
                                ? DB.Scalar($"select max({Context.Users.ID.Name}) from {Context.Users.Table}")
                                : user.ID.Value;
                            // авторизуем пользователя
                            Global.LoginedUser.Add(new UserData(newID, model.Phone, 1));
                            FormsAuthentication.SetAuthCookie(model.Phone, true);
                            return this.RedirectToAction("Index", "Page");
                        }
                        else
                            this.ModelState.AddModelError("", "Произошла ошибка регистрации!");
                    }
                }
            }

            return this.View(model);
        }
        /// <summary>
        /// Функция выхода из системы
        /// </summary>
        /// <returns></returns>
        public ActionResult Logoff()
        {
            Global.LoginedUser.Remove(Global.LoginedUser.Find(x => x.UserID == Global.GetUserID(this)));
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Page");
        }

    }
}