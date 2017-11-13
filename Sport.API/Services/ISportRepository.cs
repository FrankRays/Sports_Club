using Sport.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Services
{
    public interface ISportRepository
    {
        IEnumerable<Activity> GetActivities();
        IEnumerable<Activity> GetTrainerActivities(string trainerId);
        Activity GetActivity(int activityId);
        bool ActivityExists(int activityId);
        void AddActivity(Activity activity);
        void DeleteActivity(Activity activity);
        bool Save();
        //IEnumerable<Activity> GetClientActivities(string clientId);
        void AddClientActivity(int activityId, ClientActivity clientActivity);
    }
}
