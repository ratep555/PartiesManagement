using System.Threading.Tasks;
using Core.Dtos;
using Google.Apis.Auth;

namespace Core.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth);
    }
}