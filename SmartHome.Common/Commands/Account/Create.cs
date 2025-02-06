namespace SmartHome.Common.Commands.Account;

public class CreateCommand(string Name, string Email, string password, string SecurityStamp)
{
    public string Name { get; set; } = Name;
    public string Email { get; set; } = Email;
    public string Password { get; set; } = password;
    public string SecurityStamp { get; set; } = SecurityStamp;

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

        RuleFor(x => x.Email)
           .NotNull()
           .NotEmpty()
           .MaximumLength(100);

        RuleFor(x => x.Password)
           .NotNull()
           .NotEmpty();

        RuleFor(x => x.SecurityStamp)
            .NotEmpty()
            .NotNull();
    }
}

public class CreateResponse(
    Guid Id)
{
    public Guid Id { get; } = Id;
}