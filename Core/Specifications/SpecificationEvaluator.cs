using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Core.Specifications;
public static class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec) {
        if (spec.Criteria != null) {
            query = query.Where(spec.Criteria);
        }
        if (spec.Orderby != null) {
            query = query.OrderBy(spec.Orderby);
        }
        if (spec.OrderbyDescending != null) {
            query = query.OrderByDescending(spec.OrderbyDescending);
        }
        if (spec.IsDistinct) {
            query = query.Distinct();
        }
        if (spec.IsPagingEnabled) {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
        return query;
    }

    public static IQueryable<TReturn> GetQuery<TSpec,TReturn>(IQueryable<T> query, ISpecification<T,TReturn> spec) {
        if (spec.Criteria != null) {
            query = query.Where(spec.Criteria);
        }
        if (spec.Orderby != null) {
            query = query.OrderBy(spec.Orderby);
        }
        if (spec.OrderbyDescending != null) {
            query = query.OrderByDescending(spec.OrderbyDescending);
        }
        if (spec.IsDistinct) {
            query = query.Distinct();
        }
        var querySelect = query as IQueryable<TReturn>;
        if (spec.Select != null) {
            querySelect = query.Select(spec.Select);
        }
        if (spec.IsPagingEnabled) {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }
        return querySelect ?? query.Cast<TReturn>();
    }
}
