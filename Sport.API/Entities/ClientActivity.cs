using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Entities
{
    public class ClientActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ClientId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }
        public int ActivityId { get; set; }
    }
}
