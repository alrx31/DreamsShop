using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;

namespace BLL.Services;

public class ProviderService(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher
    ) : IProviderService
{
    public async Task<Provider?> GetProvider(Guid id)
    {
        var provider = await unitOfWork.ProviderRepository.GetProvider(id);

        if (provider is null)
        {
            throw new NotFoundException("Provider not found.");
        }

        return provider;
    }

    public async Task<Provider?> GetProvider(string name)
    {
        var provider = await unitOfWork.ProviderRepository.GetProvider(name);

        if (provider is null)
        {
            throw new NotFoundException("Provider not found.");
        }

        return provider;
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