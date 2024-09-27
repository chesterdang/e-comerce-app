using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepositoty<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{

    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public bool IsExist(int id)
    {
        return  context.Set<T>().Any(x => x.Id == id);
    }

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T?> GetWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TReturn>> ListAsync<TReturn>(ISpecification<T, TReturn> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<TReturn?> GetWithSpec<TReturn>(ISpecification<T, TReturn> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }
    private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>(),spec);
    }
    private IQueryable<TReturn> ApplySpecification<TReturn>(ISpecification<T,TReturn> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T,TReturn>(context.Set<T>(),spec);
    }

    

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        var query = await SpecificationEvaluator<T>.GetCriteriaQuery(context.Set<T>(),spec).ToListAsync();
        return query.Count;
    }
}
