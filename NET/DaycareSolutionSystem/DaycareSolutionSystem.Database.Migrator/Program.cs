using System;
using DaycareSolutionSystem.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DaycareSolutionSystem.Database.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dc = new DssDataContext())
            {
                dc.Database.Migrate();
            }
        }
    }
}
