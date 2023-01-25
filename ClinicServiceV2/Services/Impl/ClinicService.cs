using ClinicService.Data.DbContexts;
using ClinicService.Data.Models;
using ClinicServiceNamespace;
using Grpc.Core;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicService.Services.Impl
{
    public class ClinicService : ClinicServiceBase
    {
        private readonly ClinicServiceDbContext _dbContext;
        public ClinicService(ClinicServiceDbContext dbContext) => _dbContext = dbContext;

        public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
        {
            try
            {
                var client = new Client
                {
                    Document = request.Document,
                    Surname = request.Surname,
                    FirstName = request.FirstName,
                    Patronymic = request.Patronymic,
                };
                _dbContext.Clients.Add(client);
                _dbContext.SaveChanges();

                var response = new CreateClientResponse
                {
                    ClientId = client.Id,
                    ErrCode = 0,
                    ErrMesage = ""
                };

                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                var response = new CreateClientResponse
                {
                    ClientId = 0,
                    ErrCode = 1001,
                    ErrMesage = "Server sleeps, don't disturb"
                };

                return Task.FromResult(response);
            }
        }

        public override Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
        {
            try
            {
                var response = new GetClientsResponse();
                var clients = _dbContext.Clients.Select(c => new ClientResponse
                {
                    ClientId = c.Id,
                    Document = c.Document,
                    FirstName = c.FirstName,
                    Patronymic = c.Patronymic,
                    Surname = c.Surname
                }).ToList();
                response.Clients.AddRange(clients);
                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                var response = new GetClientsResponse
                {
                    ErrCode = 1002,
                    ErrMessage = "Server sleeps, don't disturb"
                };

                return Task.FromResult(response);
            }
        }
    }
}
