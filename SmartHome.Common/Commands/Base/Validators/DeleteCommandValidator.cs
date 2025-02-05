namespace SmartHome.Shared.Commands.Base.Validators;

public abstract class DeleteCommandValidator<T>
    : AbstractValidator<T>
    where T : IDeleteCommand
{
    protected DeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}