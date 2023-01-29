using ClinicClientToleranse;

public class ClinicClient : IClinicClient
{
    private readonly ILogger<ClinicClient> logger;

    public ClinicClientToleranse.ClinicClientToleranse _ClientToleranse { get; }

    public ClinicClient(ILogger<ClinicClient> logger,HttpClient httpClient)
    {
        this.logger = logger;
        _ClientToleranse = new ClinicClientToleranse.ClinicClientToleranse("http://localhost:5063", httpClient);
    }


    public Task<GetClientsResponse> ClientsAsync(GetClientsRequest body)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateClientResponse> CreateClinetAsync(CreateClientRequestGrpc body, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<DeleteClientResponse> DeleteClientAsync(DeleteClientRequest body, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ClientResponse> GetClientByIdAsync(int? clientId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UpdateClientResponse> UpdateClientAsync(UpdateClientRequest body, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}