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
        Activity GetActivity(int activityId);
        bool ActivityExists(int activityId);
        void AddActivity(Activity activity);
        void DeleteActivity(Activity activity);
        bool Save();
    }
}
