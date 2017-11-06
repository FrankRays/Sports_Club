using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Entities
{
    public class SportContext : DbContext
    {
        public SportContext(DbContextOptions<SportContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Entities.Activity> Activities { get; set; }
    }
}
