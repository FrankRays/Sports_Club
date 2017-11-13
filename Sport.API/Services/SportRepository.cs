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

        public IEnumerable<Activity> GetTrainerActivities(string trainerId)
        {
            return _context.Activities.Where(i => i.TrainerId == trainerId).ToList();
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

        /*IEnumerable<Activity> GetClientActivities(string clientId)
        {
            return _context.Activities.Where(i => i.ClientActivities.ClientId == clientId).ToList();
        }*/

        public void AddClientActivity(int activityId, ClientActivity clientActivity)
        {
            var activity = GetActivity(activityId);
            activity.ClientActivities.Add(clientActivity);
        }
    }
}
