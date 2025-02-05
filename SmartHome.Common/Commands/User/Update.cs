namespace SmartHome.Shared.Commands.User;

public class UpdateCommand(Guid Id, string Name)
    : IUpdateCommand
{
    public Guid Id { get; set; } = Id;
    public string Name { get; set; } = Name;
}

public class UpdateCommandValidator
    : UpdateCommandValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);
    }
}

public class UpdateResponse(
)
{
}