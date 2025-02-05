namespace SmartHome.Database.Entities
{
    public interface _IAuditable
    {
        public Guid Id { get; }
    }

    public interface IAuditable
    {
        DateTimeOffset CreatedOn { get; }
        DateTimeOffset ModifiedOn { get; }
    }

    public interface IDeletable
    {
        DateTimeOffset? DeletedOn { get; }
    }
}
