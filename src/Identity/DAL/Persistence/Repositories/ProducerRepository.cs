using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence.Repositories;

public class ProducerRepository : IProducerRepository
{
    
    private readonly ApplicationDbContext _context;
    
    public ProducerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async  Task AddProducer(Producer? producer)
    {
        await _context.Producers.AddAsync(producer);
    }

    public async Task<Producer?> GetProducer(Guid id)
    {
        return await _context.Producers.FirstOrDefaultAsync(u=>u.Id == id);
    }

    public async Task<Producer?> GetProducer(string name)
    {
        return await _context.Producers.FirstOrDefaultAsync(u=>u.Name == name); 
    }

    public Task DeleteProducer(Producer? producer)
    {
        _context.Producers.Remove(producer);
        
        return Task.CompletedTask;
    }

    public Task UpdateProducer(Producer? producer)
    {
        _context.Producers.Update(producer);
        
        return Task.CompletedTask;
    }

    public async Task<Producer?> GetProducerByAdmin(Guid requestorId)
    {
        return await _context.Producers.FirstOrDefaultAsync(p => p.Staff.Select(U=>U.Id).Contains(requestorId)); 
    }
}