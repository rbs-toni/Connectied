﻿using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Connectied.Application.Repositories;
using Connectied.Domain;
using Connectied.Infrastructure.Persistence;
using Mapster;
using System;
using System.Linq;

namespace Connectied.Infrastructure.Repositories;
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public EfRepository(ConnectiedDbContext dbContext) : base(dbContext)
    {
    }

    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    // This is only done when no Selector is defined, so regular specifications with a selector also still work.
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) => specification.Selector is not null
        ? base.ApplySpecification(specification)
        : ApplySpecification(specification, false).ProjectToType<TResult>();
}
