using Core.Entities;

namespace Core.Utils.Security.JWT
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(AppUser user);
    }
}
