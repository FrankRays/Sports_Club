using System;
using System.Collections.Generic;
using System.Text;

namespace Sport.Model
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }
    }
}
