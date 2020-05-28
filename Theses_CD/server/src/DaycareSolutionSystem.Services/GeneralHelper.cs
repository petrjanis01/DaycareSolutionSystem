using System;

namespace DaycareSolutionSystem.Helpers
{
    public static class GeneralHelper
    {
        public static DateTime GetNextDateFromDay(DayOfWeek day)
        {
            var date = DateTime.Today;
            var until = date.AddDays(6);

            while (date <= until)
            {
                if (date.DayOfWeek == day)
                {
                    break;
                }

                date = date.AddDays(1);
            }

            return date;
        }
    }
}
