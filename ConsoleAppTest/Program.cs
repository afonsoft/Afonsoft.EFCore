using ConsoleAppTest.DataBase;
using ConsoleAppTest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Afonsoft.EFCore;


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

        private static void test()
        {
            TesteDbContext dbSQLite = new TesteDbContext(Afonsoft.EFCore.EnumProvider.SQLite, "Data Source=SQLite.db");
            TesteDbContext dbInMemory = new TesteDbContext(Afonsoft.EFCore.EnumProvider.InMemory);

            dbSQLite.EnsureCreated();
            //dbInMemory.EnsureCreated();
            

            Repository<UserModel> users1 = new Repository<UserModel>(dbSQLite);

            Repository<UserModel> users2 = new Repository<UserModel>(dbInMemory);

            var all2 = users2.Get().ToList();
            var all1= users1.Get().ToList();
            

            if (all1.Any())
            {
                var item = all1.FirstOrDefault(x => x.Id == 1);
            }
            else
            {
                users1.Add(new UserModel() { Id = 1, Nome = "Afonso" });
                users1.Add(new UserModel() { Id = 2, Nome = "Marcelo" });
            }

            if (all2.Any())
            {
                var item = all2.FirstOrDefault(x => x.Id == 1);
            }
            else
            {
                users2.Add(new UserModel() { Id = 1, Nome = "Afonso" });
                users2.Add(new UserModel() { Id = 2, Nome = "Marcelo" });
            }


            var item1 = users1.Get(x => x.Id == 1, x => x.Nome).FirstOrDefault();


            if (item1 != null)
            {
                Console.WriteLine(item1.Nome);
                item1.Nome = "Afonso 1";
                users1.Update(item1);
                var all = users1.Get().ToList();
                if(all!= null)
                {
                    Console.WriteLine(all[0].Nome);
                }
            }
        }
    }
}
