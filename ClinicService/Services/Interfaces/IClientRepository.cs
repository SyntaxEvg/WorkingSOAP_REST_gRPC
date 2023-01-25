using ClinicService.Data;
using ClinicService.Data.Models;

namespace ClinicService.Services.Interfaces
{
    public interface IClientRepository : IRepository<Client, int>
    {
        object? Add(Client client);
    }
}
