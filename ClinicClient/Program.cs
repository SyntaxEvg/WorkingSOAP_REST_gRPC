using ClinicService.Proto;
using ClinicServiceNamespace;
using Grpc.Core;
using Grpc.Net.Client;
using static ClinicService.Proto.AuthenticateService;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");

            AuthenticateServiceClient authenticateServiceClient = new AuthenticateServiceClient(channel);
            var authenticationResponse = authenticateServiceClient.Login(new AuthenticationRequest
            {
                UserName = "test@ya.ru",
                Password = "error"
            });

            if (authenticationResponse.Status != 0)
            {
                Console.WriteLine("Authentication error.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"Session token: {authenticationResponse.SessionContext.SessionToken}");


            ClinicServiceClient clinicServiceClient = new(GrpcChannel.ForAddress("https://localhost:5002", 
                                                      new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), CallCredentials.FromInterceptor((c, m) =>
                {
                    m.Add("Authorization",
                        $"Bearer {authenticationResponse.SessionContext.SessionToken}");
                    return Task.CompletedTask;
                }))
            }));

            var createClientResponse = clinicServiceClient.CreateClinet(new CreateClientRequest
            {
                Document = "Document",
                FirstName = "FirstName",
                Patronymic = "Patronymic",
                Surname = "Surname"
            });

            if (createClientResponse.ErrCode == 0)
            {
                Console.WriteLine($"Client #{createClientResponse.ClientId} created successfully.");
            }
            else
            {
                Console.WriteLine($"Create client error.\nErrorCode: {createClientResponse.ErrCode}\nErrorMessage: {createClientResponse.ErrMessage}");
            }

            var getClientResponse = clinicServiceClient.GetClients(new GetClientsRequest());

            if (createClientResponse.ErrCode == 0)
            {
                Console.WriteLine("Clients");
                Console.WriteLine("=======\n");

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
        }
    }
}