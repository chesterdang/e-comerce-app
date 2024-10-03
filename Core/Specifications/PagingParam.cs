using System;
using System.Security.Cryptography.X509Certificates;

namespace Core.Specifications;

public class PagingParam
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set ; } = 1;
    private int pageSize = 6;
    public int PageSize {
        get => pageSize;
        set => pageSize = (value < MaxPageSize) ? value : MaxPageSize;
    }
}
