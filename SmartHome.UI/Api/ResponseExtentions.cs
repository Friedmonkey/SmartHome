using MudBlazor;
using SmartHome.Common;
using SmartHome.Common.Models;

namespace SmartHome.UI.Api;
public static class ResponseExtentions
{
    public static void Show<T>(this Response<T>? response, ISnackbar snackbar, string? successMessage = null, Severity severity = Severity.Info) where T : Response<T>
    {
        if (response.EnsureSuccess(snackbar))
        { 
            if (successMessage is not null)
                snackbar.Add(successMessage, severity);
        }
    }
    public static bool EnsureSuccess<T>(this Response<T>? response, ISnackbar snackbar) where T : Response<T>
    {   //Returns if the response was success only logs errors if any
        return CheckSuccess(response, snackbar, onError: (error) => 
        {
            snackbar.Add(error, Severity.Error, opt => opt.RequireInteraction = true);
        });
    }
    public static bool CheckSuccess<T>(this Response<T>? response, ISnackbar snackbar, Action<string>? onError = null) where T : Response<T>
    {
        if (response?._RequestSuccess is null)
        { 
            snackbar.Add("Response was empty!", Severity.Error, opt => opt.RequireInteraction = true);
            return false;
        }

        if (response?._RequestSuccess == true)
            return true;

        if (string.IsNullOrEmpty(response?._RequestMessage))
            snackbar.Add("Response message was empty!", Severity.Error, opt => opt.RequireInteraction = true);
        else
            onError?.Invoke(response!._RequestMessage);

        return false;
    }
    public static void EnforceSuccess<T>(this Response<T>? response) where T : Response<T>
    {
        if (response?._RequestSuccess is null)
            throw new ApiError("Response was empty!");

        if (response?._RequestSuccess == true)
            return;

        if (string.IsNullOrEmpty(response?._RequestMessage))
            throw new ApiError("Response message was empty!");
        else
            throw new ApiError(response._RequestMessage);
    }

    public static bool WasSuccess<T>(this Response<T>? response) where T : Response<T>
    {   //handles null too
        return response?._RequestSuccess == true;
    }
}
