using DAL.Entities;

namespace DAL.IRepositories;

public interface IProducerRepository
{
    Task AddProducer(Provider? producer);
    
    Task<Provider?> GetProducer(Guid id);
    
    Task<Provider?> GetProducer(string name);
    
    Task DeleteProducer(Provider? producer);
    
    Task UpdateProducer(Provider? producer);
    Task<Provider?> GetProducerByAdmin(Guid requestorId);
}