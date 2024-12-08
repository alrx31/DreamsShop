using BLL.DTO;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;

namespace BLL.Services;

public class ProviderService(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher
    ) : IProviderService
{
    public Task<Provider?> GetProvider(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Provider?> GetProvider(string name)
    {
        throw new NotImplementedException();
    }

    public Task RegisterProvider(RegisterProviderDTO model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProvider(UpdateProviderDTO model, Guid providerId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProvider(DeleteProviderDTO model, Guid providerId)
    {
        throw new NotImplementedException();
    }
}