using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sport.Model
{
    public class ClientActivity
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public int ActivityId { get; set; }
    }
}
