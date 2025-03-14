﻿using Newtonsoft.Json;

namespace SmartHome.Common.Models;

public record Response<T>(bool _RequestSuccess = true, string _RequestMessage = "") where T : Response<T>
{
    public static T Failed(string error)
    {
        // Find the constructor with the most parameters
        var ctor = typeof(T).GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
        if (ctor == null) throw new InvalidOperationException($"No constructor found for {typeof(T)}");

        // Create default values for each parameter
        var parameters = ctor.GetParameters()
            .Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null)
            .ToArray();

        // Invoke constructor with default values and set Success = false
        var instance = (T)ctor.Invoke(parameters);
        return instance with { _RequestSuccess = false, _RequestMessage = error };
    }
    public static T FailedJson(object obj)
    { 
        string json = JsonConvert.SerializeObject(obj);
        return Failed(json);
    }
    public static T Error(Exception ex)
    {
        if (ex is ApiError apiError && (apiError.IsFatal == false))
            return Failed(apiError.Error);
#if DEBUG
        return FailedJson(ex);
#else
        return Failed(ex.Message);
#endif
    }
}