using ClinicClientToleranse;
using System.Net.Http;
using System.Runtime;

public interface IClinicClient
{
    ClinicClientToleranse.ClinicClientToleranse _ClientToleranse { get; }
    System.Threading.Tasks.Task<CreateClientResponse> CreateClinetAsync(CreateClientRequestGrpc body, System.Threading.CancellationToken cancellationToken);
    System.Threading.Tasks.Task<UpdateClientResponse> UpdateClientAsync(UpdateClientRequest body, System.Threading.CancellationToken cancellationToken);
    System.Threading.Tasks.Task<DeleteClientResponse> DeleteClientAsync(DeleteClientRequest body, System.Threading.CancellationToken cancellationToken);
    System.Threading.Tasks.Task<ClientResponse> GetClientByIdAsync(int? clientId, System.Threading.CancellationToken cancellationToken);
    System.Threading.Tasks.Task<GetClientsResponse> ClientsAsync(GetClientsRequest body);

}