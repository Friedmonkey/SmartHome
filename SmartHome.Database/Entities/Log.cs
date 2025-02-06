using SmartHome.Common.Models.Enums;
namespace SmartHome.Database.Entities;
public class Log
{
    public LogType LogType { get; set; }
    public string? Action { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid SmartUserId { get; set; }

    public SmartHome? SmartHome { get; set; }
}
