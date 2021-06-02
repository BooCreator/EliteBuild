using System;
using System.Web.Mvc;
using DataBaseConnector;
using EliteBuild.Models.DataBase;

namespace EliteBuild.Controllers
{
    public class RequestController : Controller
    {
        /// <summary>
        /// Функция добавления заявки
        /// </summary>
        /// <param name="Name">Имя пользователя</param>
        /// <param name="Phone">Телефон пользователя</param>
        /// <param name="Calculate">Калькуляция</param>
        /// <returns></returns>
        [HttpPost]
        public String Add(String Name, String Phone, String Calculate)
        {
            // подключаемся к БД
            using (var DB = new DataBaseExtended(Global.ConnectionString))
            {
                // получаем данные пользователя из БД
                User user = Context.Users.Find(DB, $"{Context.Users.Login.Name} like '{Phone}'", out bool _);
                // если пользователь не найден
                if(user == null)
                {
                    // добавляем запись о пользователе в БД без пароля
                    if (Context.Users.Add(DB, Name, Phone, "", 0))
                    {
                        // получаем ИД добавленного пользователя
                        int ID = DB.Scalar($"select MAX({Context.Users.ID.Name}) from {Context.Users.Table}");
                        // добавляем заявку в БД
                        if(Context.Requests.Add(DB, ID, $"'{Calculate}'"))
                            return Error.Accept;
                    }
                    else
                        return "Произошла ошибка регистрации!";
                } else
                {
                    // если пользователь был найден в БД, то только заявку добавляем в БД
                    if (Context.Requests.Add(DB, user.ID.Value, $"'{Calculate}'"))
                        return Error.Accept;
                }
            }
            return Error.Unknown;
        }
        /// <summary>
        /// Функция изменения состояния заявки
        /// </summary>
        /// <param name="Id">ИД заявки</param>
        /// <param name="State">ИД состояния</param>
        /// <returns></returns>
        [HttpPost]
        public String Update(Int32 Id, Int32 State)
        {
            // проверяем, является ли пользователь администратором изи менеджером
            if (Global.GetUserRole(this) == Global.AdminRoleID || Global.GetUserRole(this) == Global.ManagerRoleID)
            {
                // подключаемся к БД
                using (var DB = new DataBaseExtended(Global.ConnectionString))
                {
                    // Изменяем состояние заявки в БД
                    if (Context.Requests.Update(DB, $"{Context.Requests.State.Name}", $"{State}", $"{Context.Requests.ID.Name} = {Id}"))
                        return Error.Accept;
                }
                return Error.Unknown;
            }
            return Error.NotAccess;
        }
    }
}