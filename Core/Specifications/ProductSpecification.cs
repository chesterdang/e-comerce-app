using System;
using System.Linq.Expressions;
using Core.Entities;


namespace Core.Specifications;

public class ProductSpecification : Specification<Product>
{
    public ProductSpecification(ProductSpecParams specParams) : base(x =>
        (string.IsNullOrEmpty(specParams.Search) || x.Name.Contains(specParams.Search)) &&
        (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) &&
        (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type)))
    {
        ApplyPaging(specParams.PageSize, specParams.PageSize*(specParams.PageIndex-1));
        
        switch (specParams.Sort)
        {
            case "priceAsc": 
                {
                    AddOrerBy(x => x.Price);
                    break;
                }
            case "priceDesc":
                {
                    AddOrerByDescending(x => x.Price);
                    break;
                }
            default:
                {
                    AddOrerBy(x => x.Name);
                    break;
                }
        }
    }
}
