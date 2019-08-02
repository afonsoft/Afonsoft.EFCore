using ConsoleAppTest.DataBase;
using ConsoleAppTest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Afonsoft.EFCore;
using System.Collections.Generic;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            test();
            Console.WriteLine("OK");
            Console.ReadKey();
        }

        private async static void test()
        {
            //Afonsoft.EFCore.EnumProvider.SQLite, "Data Source=SQLite.db"
            AppDbContext dbSQLite = new AppDbContext(c => { c.Provider = EnumProvider.SQLite; c.ConnectionString = "Data Source=SQLite.db"; });
            AppDbContext dbInMemory = new AppDbContext(c => c.Provider = EnumProvider.InMemory);

            dbSQLite.EnsureCreated();
            dbInMemory.EnsureCreated();


            Repository<UserModel> usersSql = new Repository<UserModel>(dbSQLite);
            Repository<UserModel> usersMemory = new Repository<UserModel>(dbInMemory);

            var all1 = usersSql.Get().ToList();

            IList<UserModel> usrs = new List<UserModel>();
            for (int i = 1; i <= 5000; i++)
            {
                usrs.Add(new UserModel() { Id = i, Nome = $"Afonso {i}" });
            }

            await usersMemory.AddRangeAsync(usrs);

            if (!all1.Any())
            {
                await usersSql.AddRangeAsync(usrs);

            }


            var page = usersSql.GetPagination(x => x.Nome.Contains("Afon"), x => x.Nome, 2, 100).ToList();

            if (page.Any())
            {

            }

        }
    }
}