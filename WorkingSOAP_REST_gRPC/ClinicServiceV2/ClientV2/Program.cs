using ClinicService.Models.Requests;
using ClinicService.ProtoV2;
using ClinicServiceV2;
using Grpc.Core;
using Grpc.Net.Client;
using static ClinicService.ProtoV2.AuthenticateService;
using static ClinicServiceV2.ClinicService;

string Url = "https://localhost:5002";

using var channel = GrpcChannel.ForAddress(Url);
AuthenticateServiceClient authenticateServiceClient = new AuthenticateServiceClient(channel);
var authenticationResponse = authenticateServiceClient.Login(new AuthenticationRequestGrpc
{
    UserName = "best@gmail.com",
    Password = "passwd"
});

if (authenticationResponse.Status != 0)
{
    Console.WriteLine("Authentication error.");
    Console.ReadKey();
    return;
}

Console.WriteLine($"Session token: {authenticationResponse.SessionContext.SessionToken}");

ClinicServiceClient clinicServiceClient = new ClinicServiceClient(GrpcChannel.ForAddress(Url, new GrpcChannelOptions
{
    Credentials = ChannelCredentials.Create(new SslCredentials(), CallCredentials.FromInterceptor((c, m) =>
    {
        m.Add("Authorization",
            $"Bearer {authenticationResponse.SessionContext.SessionToken}");
        return Task.CompletedTask;
    }))
}));

var createClientResponse = clinicServiceClient.CreateClinet(new CreateClientRequestGrpc
{
    Document = "Document 1",
    FirstName = "FirstName 3",
    Patronymic = "Patronymic 2",
    Surname = "Surname 1"
});

if (createClientResponse.ErrCode == 0)
{
    Console.WriteLine($"Client #{createClientResponse.ClientId} created successfully");
}
else
{
    Console.WriteLine($"{createClientResponse.ErrCode};ErrorMessage: {createClientResponse.ErrMessage}");
}

var getClientResponse = clinicServiceClient.GetClients(new GetClientsRequest());

if (createClientResponse.ErrCode == 0)
{
    Console.WriteLine("Clients");
    foreach (var client in getClientResponse.Clients)
    {
        Console.WriteLine($"#{client.ClientId} {client.Document} {client.Surname} {client.FirstName} {client.Patronymic}");
    }
}
else
{
    Console.WriteLine($"Get clients error.\nErrorCode: {getClientResponse.ErrCode}\nErrorMessage: {getClientResponse.ErrMessage}");
}

Console.ReadKey();