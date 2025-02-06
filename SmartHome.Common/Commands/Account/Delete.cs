using SmartHome.Common.Commands.Base;
using SmartHome.Common.Commands.Base.Validators;

namespace SmartHome.Common.Commands.Account;

public class DeleteCommand(Guid Id)
    : IDeleteCommand
{
    public Guid Id { get; set; } = Id;
}

public class DeleteCommandValidator
    : DeleteCommandValidator<DeleteCommand>
{ }