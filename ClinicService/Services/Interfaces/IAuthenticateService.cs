using ClinicService.Proto;
using Grpc.Core;

namespace ClinicService.Services.Interfaces
{
    public interface IAuthenticateService
    {
        ClinicService.Models.Requests.AuthenticationResponse Login(ClinicService.Models.Requests.AuthenticationRequest authenticationRequest);

        public SessionContext GetSessionInfo(string sessionToken);
    }
}
