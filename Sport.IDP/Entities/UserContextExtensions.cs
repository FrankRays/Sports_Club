using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.IDP.Entities
{
    public static class UserContextExtensions
    {
        public static void EnsureSeedDataForContext(this UserContext context)
        {
            // Add 2 demo users if there aren't any users yet
            if (context.Users.Any())
            {
                return;
            }

            // init users
            var users = new List<User>()
            {
                new User()
                {
                    SubjectId = "1",
                    Username = "Jonas Jonaitis",
                    Password = "password",
                    IsActive = true,
                    Claims = {
                         new UserClaim("role", "Trainer"),
                         new UserClaim("given_name", "Frank"),
                         new UserClaim("family_name", "Underwood")
                    }
                },
                new User()
                {
                    SubjectId = "2",
                    Username = "123456",
                    Password = "password",
                    IsActive = true,
                    Claims = {
                         new UserClaim("role", "Client"),
                         new UserClaim("given_name", "Petras"),
                         new UserClaim("family_name", "Petraitis")                   
                }
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
