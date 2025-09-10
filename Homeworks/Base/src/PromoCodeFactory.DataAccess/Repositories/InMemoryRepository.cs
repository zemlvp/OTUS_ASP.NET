using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories;

public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
{
    protected List<T> Data { get; private set; }

    public InMemoryRepository(List<T> data)
    {
        Data = data;
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult(Data.AsEnumerable());
    }

    public Task<T> GetByIdAsync(Guid id)
    {
        return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
    }

    public Task<Guid> AddAsync(T entity)
    {
        entity.Id = Guid.NewGuid();
        Data.Add(entity);

        return Task.FromResult(entity.Id);
    }

    public Task UpdateAsync(Guid id, T entity)
    {        
        Data[GetDataIndexById(id)] = entity;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {        
        Data.RemoveAt(GetDataIndexById(id));

        return Task.CompletedTask;
    }

    private int GetDataIndexById(Guid id) => Data.FindIndex(x => x.Id == id);
}