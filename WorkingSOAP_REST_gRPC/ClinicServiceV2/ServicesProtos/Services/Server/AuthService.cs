using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.ProtoV2;
using ClinicService.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using static ClinicService.ProtoV2.AuthenticateService;

namespace ServicesProtos.Services.Server
{
    [Authorize]
    public class AuthServiceGrpc : AuthenticateServiceBase
    {
        IAuthenticateService _authenticateService;
        public AuthServiceGrpc(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [AllowAnonymous]
        public override Task<AuthenticationResponseGrpc> Login(AuthenticationRequestGrpc request, ServerCallContext context)
        {
            AuthenticationResponse response = _authenticateService.Login(new AuthenticationRequest
            {
                Login = request.UserName,
                Password = request.Password
            });

            if (response.Status == AuthenticationStatus.Success)
            {
                context.ResponseTrailers.Add("X-Session-Token", response.SessionContext.SessionToken);
            }

            return Task.FromResult(new AuthenticationResponseGrpc
            {
                Status = (int)response.Status,
                SessionContext = new SessionContextGrpc
                {
                    SessionId = response.SessionContext.SessionId,
                    SessionToken = response.SessionContext.SessionToken,
                    Account = new AccountDtoGrpc
                    {
                        AccountId = response.SessionContext.Account.AccountId,
                        EMail = response.SessionContext.Account.EMail,
                        FirstName = response.SessionContext.Account.FirstName,
                        LastName = response.SessionContext.Account.LastName,
                        SecondName = response.SessionContext.Account.SecondName,
                        Locked = response.SessionContext.Account.Locked
                    }
                }
            });
        }

        public override Task<GetSessionResponse> GetSession(GetSessionRequest request, ServerCallContext context)
        {
            var authorizationHeader = context.RequestHeaders.FirstOrDefault(header => header.Key == "Authorization");
            if (AuthenticationHeaderValue.TryParse(authorizationHeader.Value, out var headerValue))
            {
                var scheme = headerValue.Scheme; // "Bearer"
                var sessionToken = headerValue.Parameter; // Token
                if (string.IsNullOrEmpty(sessionToken))
                {
                    return Task.FromResult(new GetSessionResponse());
                }

                SessionContext sessionContext = _authenticateService.GetSessionInfo(sessionToken);
                if (sessionContext == null)
                {
                    return Task.FromResult(new GetSessionResponse());
                }

                return Task.FromResult(new GetSessionResponse
                {
                    SessionContext = new SessionContextGrpc
                    {
                        SessionId = sessionContext.SessionId,
                        SessionToken = sessionContext.SessionToken,
                        Account = new AccountDtoGrpc
                        {
                            AccountId = sessionContext.Account.AccountId,
                            EMail = sessionContext.Account.EMail,
                            FirstName = sessionContext.Account.FirstName,
                            LastName = sessionContext.Account.LastName,
                            SecondName = sessionContext.Account.SecondName,
                            Locked = sessionContext.Account.Locked
                        }
                    }
                });
            }

            return Task.FromResult(new GetSessionResponse());
        }
    }
}
