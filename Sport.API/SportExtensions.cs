using Sport.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API
{
    public static class SportExtensions
    {
        public static void EnsureSeedDataForContext(this SportContext context)
        {
            if (context.Activities.Any())
            {
                return;
            }

            var activities = new List<Activity>()
            {
                new Activity()
                {
                    Name = "Yoga",
                    Beginning = new DateTime(2017, 12, 3, 15, 0, 0),
                    Ending = new DateTime(2017, 12, 3, 16, 0, 0)
                }
            };

            context.Activities.AddRange(activities);
            context.SaveChanges();
        }
    }
}
