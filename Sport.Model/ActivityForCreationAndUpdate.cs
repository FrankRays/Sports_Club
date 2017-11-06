using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sport.Model
{
    public class ActivityForCreationAndUpdate
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime Beginning { get; set; }

        [Required]
        public DateTime Ending { get; set; }
    }
}
