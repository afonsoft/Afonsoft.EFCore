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
            TesteDbContext dbSQLite = new TesteDbContext(Afonsoft.EFCore.EnumProvider.SQLite, "Data Source=SQLite.db");
            TesteDbContext dbInMemory = new TesteDbContext(Afonsoft.EFCore.EnumProvider.InMemory);

            dbSQLite.EnsureCreated();
            //dbInMemory.EnsureCreated();


            Repository<UserModel> users = new Repository<UserModel>(dbSQLite);
            //Repository<UserModel> users = new Repository<UserModel>(dbInMemory);

            var all1 = users.Get().ToList();


            if (!all1.Any())
            {
                IList<UserModel> usrs = new List<UserModel>();
                for (int i = 1; i <= 5000; i++)
                {
                    usrs.Add(new UserModel() { Id = i, Nome = $"Afonso {i}" });
                }
                await users.AddRangeAsync(usrs);
            }
        
            
            var page= users.GetPagination(x => x.Nome.Contains("Afon"), x => x.Nome, 2, 100).ToList();

            if (page.Any())
            {
                
            }

        }
    }
}
