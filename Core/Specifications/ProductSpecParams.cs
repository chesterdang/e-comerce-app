using System;
using System.Dynamic;

namespace Core.Specifications;

public class ProductSpecParams : PagingParam
{
    private List<string> brands =[];
    public List<string> Brands  
    {
        get { return brands; }
        set { brands = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList(); 
            }
    }

    private List<string> types =[];
    public List<string> Types  
    {
        get { return types; }
        set { types = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList(); 
            }
    }

    public string? Sort {get; set;} = "";
    private string? search;
    public string Search  
    {
        get =>  search ?? "";
        set =>  search = value.ToLower(); 
    }
}
