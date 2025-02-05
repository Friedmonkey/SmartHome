
namespace SmartHome.Shared.Commands.User;

public class DeleteCommand(Guid Id)
    : IDeleteCommand
{
    public Guid Id { get; set; } = Id;
}

public class DeleteCommandValidator
    : DeleteCommandValidator<DeleteCommand>
{ }