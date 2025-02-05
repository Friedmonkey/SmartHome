using FastEndpoints;
using FluentValidation;
using SmartHome.Shared.Models.Enums;

namespace SmartHome.Backend.FastEndpoints.Base;

public abstract class EndpointBase<TRequest, TResponse, TMapper, TValidator>
    : Endpoint<TRequest, TResponse, TMapper>
    where TRequest : notnull
    where TResponse : notnull
    where TMapper : class, IMapper
    where TValidator : AbstractValidator<TRequest>
{
    public override void Configure()
    {
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
        Validator<TValidator>();
        ConfigureEndpoint();
    }
    protected abstract UserRole[] RoleNames { get; }
    protected abstract void ConfigureEndpoint();
}
public abstract class EndpointBase<TRequest, TResponse, TValidator>
    : Endpoint<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
    where TValidator : AbstractValidator<TRequest>
{
    public override void Configure()
    {
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
        Validator<TValidator>();
        ConfigureEndpoint();
    }
    protected abstract UserRole[] RoleNames { get; }
    protected abstract void ConfigureEndpoint();
}