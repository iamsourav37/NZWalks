using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repository.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser identityUser, List<string> roles);
    }
}
