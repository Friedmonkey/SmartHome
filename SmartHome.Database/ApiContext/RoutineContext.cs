using Microsoft.EntityFrameworkCore;
using SmartHome.Common;

namespace SmartHome.Database.ApiContext;

public class RoutineContext
{
    private readonly SmartHomeContext _dbContext;

    public RoutineContext(SmartHomeContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task EnforceRoutineInSmartHome(Guid smartHomeId, Guid routineId)
    {
        bool isInSmartHome = await _dbContext.Routines
            .AnyAsync(r => r.Id == routineId && r.SmartHomeId == smartHomeId);

        if (!isInSmartHome)
            throw new ApiError("The routine does not exist within the given smarthome!");
    }
    public async Task EnforceRoutineNameUnique(Guid smartHomeId, string? routineName)
    {
        if (string.IsNullOrEmpty(routineName))
            throw new ApiError("Routine name cannot be empty!");

        bool alreadyExists = await _dbContext.Routines
            .Where(r => r.SmartHomeId == smartHomeId)
            .AnyAsync(d => d.Name == routineName);

        if (alreadyExists)
            throw new ApiError("There is already a routine with the same name!!");
    }
}
