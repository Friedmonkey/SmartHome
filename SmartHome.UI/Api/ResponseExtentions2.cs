using MudBlazor;
using SmartHome.Common.Models;

namespace SmartHome.UI.Api;
public static class ResponseExtentions2
{
    public static void Show<T>(this Response<T>? response, ISnackbar snackbar, string successMessage = null) where T : Response<T>
    {
        if (response.EnsureSuccess(snackbar))
        { 
            snackbar.Add(successMessage, Severity.Info);
        }
    }
    public static bool EnsureSuccess<T>(this Response<T>? response, ISnackbar snackbar) where T : Response<T>
    {
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
    public static bool EnforceSuccess<T>(this Response<T>? response) where T : Response<T>
    {
        if (response?._RequestSuccess is null)
            throw new Exception("Response was empty!");

        if (response?._RequestSuccess == true)
            return true;

        if (string.IsNullOrEmpty(response?._RequestMessage))
            throw new Exception("Response message was empty!");
        else
            throw new Exception(response._RequestMessage);
    }
}
