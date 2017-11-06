using Sport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Client.ViewModels
{
    public class SportIndexViewModel
    {
        public IEnumerable<Activity> Activities { get; private set; }
            = new List<Activity>();

        public SportIndexViewModel(List<Activity> activities)
        {
            Activities = activities;
        }
    }
}
