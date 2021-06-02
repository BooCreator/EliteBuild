using System;
using DataBaseConnector;
using DataBaseConnector.Ext;
using System.Web.Configuration;
using EliteBuild.Models.DataBase;
using System.Collections.Generic;

namespace EliteBuild
{
    public static class Context
    {
        private static String query = "create table #TABLE (#FIELDS);";

        public static User Users = new User();
        public static Role Roles = new Role();
        public static Module Modules = new Module();
        public static Local_Module Local_Modules = new Local_Module();
        public static Page Pages = new Page();
        public static Request Requests = new Request();
        public static RequestState RequestStates = new RequestState();
        public static Models.DataBase.Type Types = new Models.DataBase.Type();
        public static Work_Category Work_Categories = new Work_Category();
        public static Work Works = new Work();
        public static FooterBlock FooterBlocks = new FooterBlock();
        public static FooterMenu FooterMenus = new FooterMenu();
        public static List<String> CheckTables()
        {
            var log = new List<string> { "--Start!" };

            String last_query = "";

            var temp = new DataBase(WebConfigurationManager.AppSettings["DataBase"]);

            try
            {
                temp.Open();

                log.Add(_check(temp, Users, ref last_query));
                log.Add(_check(temp, Roles, ref last_query));
                log.Add(_check(temp, Modules, ref last_query));
                log.Add(_check(temp, Local_Modules, ref last_query));
                log.Add(_check(temp, Requests, ref last_query));
                log.Add(_check(temp, Pages, ref last_query));
                log.Add(_check(temp, Types, ref last_query));
                log.Add(_check(temp, RequestStates, ref last_query));
                log.Add(_check(temp, Work_Categories, ref last_query));
                log.Add(_check(temp, Works, ref last_query));
                log.Add(_check(temp, FooterBlocks, ref last_query));
                log.Add(_check(temp, FooterMenus, ref last_query));

                log.Add(_fill(temp, RequestStates, new Typle<Int32>[] {
                    new Typle<Int32>("'-'", -1),
                    new Typle<Int32>("'Принято'", 0),
                    new Typle<Int32>("'Выполнено'", 1),
                    new Typle<Int32>("'Отменено'", 2) }, ref last_query));
            }
            catch (Exception e)
            {
                log.Add(last_query);
                log.Add(e.ToString());
            }
            finally
            {
                temp.Dispose();
            }

            log.Add("--End!");

            return log;
        }

        private static String _check<T>(DataBase DB, Basic<T> Table, ref String last_query)
        {
            List<String> text = new List<String>();

            if (!DB.TrySelect($"select * from {Table.Table}"))
            {
                for (int i = 0; i < Table.Fields.Length; i++)
                    text.Add(Table.Fields[i] + " " + Table.Types[i]);

                last_query = query.Replace("#TABLE", Table.Table).Replace("#FIELDS", To.String(text.ToArray(), ","));

                DB.NonQuery(last_query);
                return $"-table {Table.Table} created!";
            }
            return $"{Table.Table} -- ok";
        }

        public static String _fill<T>(DataBase DB, Basic<T> Table, Typle<Int32>[] Values, ref String last_query)
        {
            if (DB.Scalar($"select count(*) from {Table.Table}") == 0)
            {
                for (int i = 0; i < Values.Length; i++)
                {
                    last_query = $"insert into {Table.Table}({To.String(Table.Fields, ",")}) values({Values[i].Value}, {Values[i].Name})";
                    DB.NonQuery(last_query);
                }
                return $"{Table.Table} filled!";
            }
            return $"{Table.Table} -- ok fill";
        }
        public static String _fill<T>(DataBase DB, Basic<T> Table, List<Typle<Int32>> Values, ref String last_query)
            => _fill<T>(DB, Table, Values.ToArray(), ref last_query);

    }
}