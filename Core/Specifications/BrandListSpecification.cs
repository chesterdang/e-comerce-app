using System;
using Core.Entities;

namespace Core.Specifications;

public class BrandListSpecification : Specification<Product,string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);
        AddIsDistinct();
    }
}
