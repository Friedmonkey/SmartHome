namespace SmartHome.Shared.Commands.User;

public class CreateCommand(string Name)
{
    public string Name { get; set; } = Name;
}

public class CreateCommandValidator
    : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);
    }
}

public class CreateResponse(
    Guid Id)
{
    public Guid Id { get; } = Id;
}