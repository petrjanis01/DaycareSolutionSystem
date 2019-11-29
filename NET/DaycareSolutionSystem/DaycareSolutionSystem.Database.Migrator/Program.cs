using System;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DaycareSolutionSystem.Database.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var ans = "";
            while (ans.ToUpper().Equals("Y") == false && ans.ToUpper().Equals("N") == false)
            {
                Console.WriteLine("Operation will delete whole database (if exists) and create/migrate new database structure and populate it with demo data." +
                                  "Do you want to proceed? (Y/N)");

                ans = Console.ReadLine();
            }


            if (ans.ToUpper().Equals("N"))
            {
                return;
            }

            var dc = new DssDataContext();

            // delete db structure
            dc.Database.EnsureDeleted();

            // create/migrate db structure
            dc.Database.Migrate();

            new DemoDataInitializer(dc).CreateDemoData();
        }
    }
}
