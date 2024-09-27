using System;
using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;

namespace Core.Specifications;
public class Specification<T>(Expression<Func<T,bool>>? criteria) : ISpecification<T> where T : BaseEntity
{
    public Specification() : this(null) {}
    public Expression<Func<T, bool>>? Criteria => criteria;

    public Expression<Func<T, object>>? Orderby {get; private set;}

    public Expression<Func<T, object>>? OrderbyDescending {get; private set;}

    public bool IsDistinct {get; private set;}

    public bool IsPagingEnabled {get; private set;}

    public int Take {get; private set;}

    public int Skip {get; private set;}
    public string Search {get; private set;} = "";

    public List<Expression<Func<T, object>>> Includes {get;} = [];

    public List<string> IncludeStrings {get;} = [];

    protected void AddInclude(Expression<Func<T,object>> includeExpression) 
    {
        Includes.Add(includeExpression);
    }
    protected void AddInclude(string includeString) 
    {
        IncludeStrings.Add(includeString);
    }

    protected void AddOrerBy(Expression<Func<T,object>> orderByEx) {
        Orderby = orderByEx;
    }
    protected void AddOrerByDescending(Expression<Func<T,object>> orderByDescEx) {
        OrderbyDescending = orderByDescEx;
    }
    protected void AddIsDistinct() {
        IsDistinct = true;
    }
    protected void ApplyPaging(int take, int skip) {
        Take = take;
        Skip = skip;
        IsPagingEnabled = true;
    }
}
public class Specification<T, TReturn>(Expression<Func<T, bool>>? criteria) : Specification<T>(criteria), ISpecification<T, TReturn> where T : BaseEntity
{
    protected Specification() : this(null) {}
    public Expression<Func<T, TReturn>>? Select {get; private set;}
    protected void AddSelect(Expression<Func<T,TReturn>> selectEx) {
        Select = selectEx;
    }
}
