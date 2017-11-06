using Sport.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Services
{
    public class SportRepository: ISportRepository
    {
        SportContext _context;

        public SportRepository(SportContext context)
        {
            _context = context;
        }

        public bool ActivityExists(int activityId)
        {
            return _context.Activities.Any(c => c.Id == activityId);
        }

        public Activity GetActivity(int activityId)
        {
            return _context.Activities.FirstOrDefault(i => i.Id == activityId);
        }

        public IEnumerable<Activity> GetActivities()
        {
            return _context.Activities
                .OrderBy(i => i.Name).ToList();
        }

        public void AddActivity(Activity activity)
        {
            _context.Activities.Add(activity);
        }

        public void DeleteActivity(Activity activity)
        {
            _context.Activities.Remove(activity);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
