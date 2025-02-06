namespace SmartHome.Common.Commands.Base;

public interface IDuplicateCommand
{
    Guid Id { get; }
}

public interface IUpdateCommand
{
    Guid Id { get; }
}

public interface IReadCommand
{
    Guid Id { get; }
}

public interface IDeleteCommand
{
    Guid Id { get; }
}
