using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Service.Contract
{
    public interface ITokenService
    {
        string CreateAccessToken(JwtOptions jwtOptions, string id);
    }
}
