using ClinicService.Data;
using ClinicServiceV2;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClinicServiceV2.ClinicService;

namespace ServicesProtos.Services.Server
{

    public class ClinicServiceGrpc : ClinicServiceBase
    {
        private readonly ClinicServiceDbContext _db;

        public ClinicServiceGrpc(ClinicServiceDbContext db)
        {
            _db = db;
        }

        public override async Task<CreateClientResponse> CreateClinet(CreateClientRequestGrpc request, ServerCallContext context)
        {
            try
            {
                var client = new ClinicService.Data.Client
                {
                    Document = request.Document,
                    Surname = request.Surname,
                    FirstName = request.FirstName,
                    Patronymic = request.Patronymic
                };

                _db.Clients.Add(client);
                await _db.SaveChangesAsync();
                var response = new CreateClientResponse
                {
                    ClientId = client.Id,
                    ErrCode = 0,
                    ErrMessage = ""
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new CreateClientResponse
                {
                    ErrCode = 500,
                    ErrMessage = "Err service"
                };
                return response;
            }
        }

        public override async Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request, ServerCallContext context)
        {
            try
            {
                var temp = await _db.Clients.Where(x => x.Id == request.ClientId).FirstOrDefaultAsync();
                if (temp is { })
                {
                    temp.Document = request.Document;
                    temp.Surname = request.Surname;
                    temp.FirstName = request.FirstName;
                    temp.Patronymic = request.Patronymic;
                    _db.Clients.Update(temp);
                    await _db.SaveChangesAsync();
                }
                var response = new UpdateClientResponse
                {
                    ErrCode = 0,//error null
                    ErrMessage = ""
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new UpdateClientResponse
                {
                    ErrCode = 500,
                    ErrMessage = ex.Message
                };
                return response;
            }
        }

        public override async Task<DeleteClientResponse> DeleteClient(DeleteClientRequest request, ServerCallContext context)
        {
            try
            {
                var temp = await _db.Clients.Where(x => x.Id == request.ClientId).FirstOrDefaultAsync();
                if (temp is { })
                {
                    _db.Clients.Remove(temp);
                    await _db.SaveChangesAsync();
                }
                var response = new DeleteClientResponse
                {
                    ErrCode = 0,
                    ErrMessage = ""
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new DeleteClientResponse
                {
                    ErrCode = 500,
                    ErrMessage = ex.Message
                };
                return response;
            }
        }

        public override async Task<ClientResponse> GetClientById(GetClientByIdRequest request, ServerCallContext context)
        {
            try
            {
                var temp = await _db.Clients.Where(x => x.Id == request.ClientId).FirstOrDefaultAsync();
                if (temp is { })
                {
                }
                var response = new ClientResponse
                {
                    // TODO: ClientId = ???
                    Document = temp?.Document ?? "The value of 'temp?.Document' should not be null",
                    Surname = temp?.Surname ?? "The value of 'temp?.Surname' should not be null",
                    FirstName = temp?.FirstName ?? "The value of 'temp?.FirstName' should not be null",
                    Patronymic = temp?.Patronymic ?? "The value of 'temp?.Patronymic' should not be null"
                };
                return response;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public override async Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
        {
            try
            {
                var clients = await _db.Clients.Select(client => new ClientResponse
                {
                    ClientId = client.Id,
                    Document = client.Document,
                    FirstName = client.FirstName,
                    Patronymic = client.Patronymic,
                    Surname = client.Surname
                }).ToListAsync();

                var response = new GetClientsResponse();
                response.Clients.AddRange(clients);
                return response;
            }
            catch (Exception ex)
            {
                var response = new GetClientsResponse
                {
                    ErrCode = 501,
                    ErrMessage = "Err service"
                };
                return response;
            }
        }

    }
}
