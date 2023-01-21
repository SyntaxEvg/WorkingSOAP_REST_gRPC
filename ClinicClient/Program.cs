using ClinicServiceNamespace;
using Grpc.Net.Client;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            ClinicServiceClient clinicServiceClient = new(channel);

            var createClientResponse = clinicServiceClient.CreateClient(new CreateClientRequest
            {
                Document = "Doc",
                FirstName = "FirstName",
                Patronymic = "Patronymic",
                Surname = "Surname"
            });

            if (createClientResponse.ErrCode == 0)
            {
                Console.WriteLine($"Success creation id --> {createClientResponse.ClientId}");
            }
            else
            {
                Console.WriteLine($"Error --> {createClientResponse.ErrCode} ### {createClientResponse.ErrMesage}");
            }

            var getClientsResponse = clinicServiceClient.GetClients(new GetClientsRequest());
            if (createClientResponse.ErrCode == 0)
            {
                Console.WriteLine("Clients");
                Console.WriteLine(new string('=', 15));
                Console.WriteLine(string.Join('\n', getClientsResponse.Clients.Select(c => $"{c.ClientId} - {c.Surname}")));
                Console.WriteLine(new string('=', 15));
            }
            else
            {
                Console.WriteLine($"Error --> {getClientsResponse.ErrCode} ### {getClientsResponse.ErrMessage}");
            }
            Console.ReadKey(true);
        }
    }
}