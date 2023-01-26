using ClinicService.Proto;
using Grpc.Core;

namespace ClinicService.Services.Interfaces
{
    public interface IAuthenticateService
    {
        ClinicService.Models.Requests.AuthenticationResponse Login(ClinicService.Models.Requests.AuthenticationRequest authenticationRequest);

        public Proto.SessionContext GetSessionInfo(string sessionToken);
    }
}
