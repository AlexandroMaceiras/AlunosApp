using AlunosApp.intefaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AlunosApp.Classes
{
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(config.Plataforma, System.IO.Path.Combine(config.DiretorioDB, "Alunos.db3"));
            connection.CreateTable<User>();
 }



        public void Inserir<T>(T model)
        {
            connection.Insert(model);
        }


        public void Update<T>(T model)
        {
            connection.Update(model);
        }


        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }


        public T Find<T>(int id) where T : class
        {
            return connection.Table<T>().FirstOrDefault(Model => Model.GetHashCode() == id);
        }


        public T First<T>() where T : class
        {
            return connection.Table<T>().FirstOrDefault();
        }

        public List<T> GetList<T>() where T : class
        {
            return connection.Table<T>().ToList();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
