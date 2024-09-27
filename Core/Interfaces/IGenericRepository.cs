using System;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    bool IsExist(int id);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<T?> GetWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<TReturn>> ListAsync<TReturn>(ISpecification<T,TReturn> spec);
    Task<TReturn?> GetWithSpec<TReturn>(ISpecification<T,TReturn> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    
}
