using Microsoft.EntityFrameworkCore;
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
            return _context.Activities.Include(c => c.ClientActivities)
                    .Where(c => c.Id == activityId).FirstOrDefault();
        }

        public IEnumerable<Activity> GetActivities()
        {
            return _context.Activities.Include(c => c.ClientActivities).OrderBy(i => i.Name).ToList();
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

        public IEnumerable<Activity> GetClientActivities(string clientId)
        {
            return _context.Activities.Where(i => i.ClientActivities.Any(x => x.ClientId == clientId)).Include(c => c.ClientActivities).ToList();
        }

        public void AddClientActivity(int activityId, ClientActivity clientActivity)
        {
            var activity = GetActivity(activityId);
            activity.ClientActivities.Add(clientActivity);
        }

        public void DeleteClientActivity(ClientActivity clientActivity)
        {
            _context.ClientActivities.Remove(clientActivity);
        }

        public ClientActivity GetClientActivity(int clientActivityId)
        {
            return _context.ClientActivities.Where(c => c.Id == clientActivityId).FirstOrDefault();
        }

        public bool ClientActivityExists(string clientId, int activityId)
        {
            return _context.ClientActivities.Any(c => c.ActivityId == activityId && c.ClientId == clientId);
        }
    }
}
