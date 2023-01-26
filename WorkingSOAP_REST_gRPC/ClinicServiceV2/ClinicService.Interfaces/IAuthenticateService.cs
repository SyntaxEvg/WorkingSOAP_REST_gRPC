﻿using ClinicService.Models;
using ClinicService.Models.Requests;

namespace ClinicService.Services
{
    public interface IAuthenticateService
    {
        AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

        public SessionContext GetSessionInfo(string sessionToken);
    }
}
