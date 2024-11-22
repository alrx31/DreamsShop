using DAL.Entities;

namespace DAL.IRepositories;

public interface IProducerRepository
{
    Task AddProducer(Producer? producer);
    
    Task<Producer?> GetProducer(Guid id);
    
    Task<Producer?> GetProducer(string name);
    
    Task DeleteProducer(Producer? producer);
    
    Task UpdateProducer(Producer? producer);
    Task<Producer?> GetProducerByAdmin(Guid requestorId);
}