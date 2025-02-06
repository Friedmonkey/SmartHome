namespace SmartHome.Common.Commands.Base.Validators;

public abstract class UpdateCommandValidator<T>
    : AbstractValidator<T>
    where T : IUpdateCommand
{
    protected UpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
    }
}