using BLL.DTO;
using DAL.Entities;

namespace BLL.IService;

public interface IProviderService
{
    Task<Provider?> GetProvider(Guid id);
    Task<Provider?> GetProvider(string name);
    Task RegisterProvider(RegisterProviderDTO model);
    Task UpdateProvider(UpdateProviderDTO model,Guid providerId);
    Task DeleteProvider(DeleteProviderDTO model, Guid providerId);
}