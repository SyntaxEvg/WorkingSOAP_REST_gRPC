using ClinicService.Data;
using ClinicServiceV2;
using Grpc.Core;
using static ClinicServiceV2.ClinicService;

namespace ServicesProtos.Services.Client
{
    public class ClinicServiceClientGrpc : ClinicServiceClient
    {
        private readonly ClinicServiceDbContext _db;

        public ClinicServiceClientGrpc(ClinicServiceDbContext db)
        {
            _db = db;
        }

        public override CreateClientResponse CreateClinet(CreateClientRequestGrpc    request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.CreateClinet(request, headers, deadline, cancellationToken);
        }

        public override CreateClientResponse CreateClinet(CreateClientRequestGrpc request, CallOptions options)
        {
            return base.CreateClinet(request, options);
        }

        public override AsyncUnaryCall<CreateClientResponse> CreateClinetAsync(CreateClientRequestGrpc request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.CreateClinetAsync(request, headers, deadline, cancellationToken);
        }

        public override AsyncUnaryCall<CreateClientResponse> CreateClinetAsync(CreateClientRequestGrpc request, CallOptions options)
        {
            return base.CreateClinetAsync(request, options);
        }

        public override DeleteClientResponse DeleteClient(DeleteClientRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.DeleteClient(request, headers, deadline, cancellationToken);
        }

        public override DeleteClientResponse DeleteClient(DeleteClientRequest request, CallOptions options)
        {
            return base.DeleteClient(request, options);
        }

        public override AsyncUnaryCall<DeleteClientResponse> DeleteClientAsync(DeleteClientRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.DeleteClientAsync(request, headers, deadline, cancellationToken);
        }

        public override AsyncUnaryCall<DeleteClientResponse> DeleteClientAsync(DeleteClientRequest request, CallOptions options)
        {
            return base.DeleteClientAsync(request, options);
        }

        public override ClientResponse GetClientById(GetClientByIdRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.GetClientById(request, headers, deadline, cancellationToken);
        }

        public override ClientResponse GetClientById(GetClientByIdRequest request, CallOptions options)
        {
            return base.GetClientById(request, options);
        }

        public override AsyncUnaryCall<ClientResponse> GetClientByIdAsync(GetClientByIdRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.GetClientByIdAsync(request, headers, deadline, cancellationToken);
        }

        public override AsyncUnaryCall<ClientResponse> GetClientByIdAsync(GetClientByIdRequest request, CallOptions options)
        {
            return base.GetClientByIdAsync(request, options);
        }

        public override GetClientsResponse GetClients(GetClientsRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.GetClients(request, headers, deadline, cancellationToken);
        }

        public override GetClientsResponse GetClients(GetClientsRequest request, CallOptions options)
        {
            return base.GetClients(request, options);
        }

        public override AsyncUnaryCall<GetClientsResponse> GetClientsAsync(GetClientsRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.GetClientsAsync(request, headers, deadline, cancellationToken);
        }

        public override AsyncUnaryCall<GetClientsResponse> GetClientsAsync(GetClientsRequest request, CallOptions options)
        {
            return base.GetClientsAsync(request, options);
        }

        public override UpdateClientResponse UpdateClient(UpdateClientRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.UpdateClient(request, headers, deadline, cancellationToken);
        }

        public override UpdateClientResponse UpdateClient(UpdateClientRequest request, CallOptions options)
        {
            return base.UpdateClient(request, options);
        }

        public override AsyncUnaryCall<UpdateClientResponse> UpdateClientAsync(UpdateClientRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.UpdateClientAsync(request, headers, deadline, cancellationToken);
        }

        public override AsyncUnaryCall<UpdateClientResponse> UpdateClientAsync(UpdateClientRequest request, CallOptions options)
        {
            return base.UpdateClientAsync(request, options);
        }
    }
}
