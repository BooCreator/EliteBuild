using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DataBaseConnector;
using EliteBuild.Models.View;
using EliteBuild.Helpers;

namespace EliteBuild.Helpers
{
    public static class UserHelper
    {
        /// <summary>
        /// Установить параметры UserID и указатель что пользователь является Администратором для страницы
        /// </summary>
        public static void SetUserInfo(Controller Controller)
        {
            Controller.ViewBag.UserID = Global.GetUserID(Controller);
            Controller.ViewBag.isAdmin = (Global.GetUserRole(Controller) == Global.AdminRoleID);
        }
        /// <summary>
        /// Получить данные пользователя
        /// </summary>
        public static void SetUserData(Controller Controller)
        {
            // получаем ID активного пользователя
            int id = Global.GetUserID(Controller);
            ProfileView UserData = null;
            // если пользователь авторизован
            if (id > -1)
            {
                // подключаемся к БД
                using (var DB = new DataBaseExtended(Global.ConnectionString))
                {
                    // получаем данные пользователя из БД
                    var User = Context.Users.Find(DB, $"{Context.Users.ID.Name} = {id}", out _);
                    // если пользователь найден
                    if (User != null)
                        UserData = new ProfileView(User.ID.Value, User.Name.Value, User.Login.Value);
                }
            }
            // сохраняем данные дял вывода на страницу
            Controller.ViewBag.UserData = UserData;
        }
    }
}