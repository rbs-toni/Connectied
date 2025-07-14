using System;
using Ardalis.Specification;
using Connectied.Application.Common.Paging;

namespace Connectied.Application.Common.Specifications;
public class EntitiesByBaseFilterSpec<TEntity, TResponse> : Specification<TEntity, TResponse>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter)
    {
        Query.SearchBy(filter);
    }
}
public class EntitiesByBaseFilterSpec<TEntity> : Specification<TEntity>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter)
    {
        Query.SearchBy(filter);
    }
}
