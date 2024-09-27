using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T,bool>>? Criteria { get; }
    Expression<Func<T,object>>? Orderby { get; }
    Expression<Func<T,object>>? OrderbyDescending { get;}
    List<Expression<Func<T,object>>> Includes {get;}
    List<string> IncludeStrings {get;}
    bool IsDistinct { get; }
    bool IsPagingEnabled { get; }
    int Take {get;}
    int Skip {get;}
    string Search {get;}
}
public interface ISpecification<T,TReturn> : ISpecification<T>
{
    Expression<Func<T,TReturn>>? Select { get; }
} 
