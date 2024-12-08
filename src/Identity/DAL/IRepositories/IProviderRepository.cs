using DAL.Entities;

namespace DAL.IRepositories;

public interface IProviderRepository
{
    Task AddProducer(Provider? producer);
    
    Task<Provider?> GetProvider(Guid id);
    
    Task<Provider?> GetProvider(string name);
    
    Task DeleteProducer(Provider? producer);
    
    Task UpdateProducer(Provider? producer);
    Task<Provider?> GetProducerByAdmin(Guid requestorId);
}