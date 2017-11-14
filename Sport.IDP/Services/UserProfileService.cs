using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Sport.IDP.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sport.IDP.Service
{
    public class UserProfileService : IProfileService
    {
        private readonly IUserRepository _UserRepository;

        public UserProfileService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = _UserRepository.GetUserClaimsBySubjectId(subjectId);

            context.IssuedClaims = claimsForUser.Select
                (c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = _UserRepository.IsUserActive(subjectId);

            return Task.FromResult(0);
        }
    }
}
