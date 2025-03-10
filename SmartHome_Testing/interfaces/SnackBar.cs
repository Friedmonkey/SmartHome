using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome_Testing.interfaces
{
    public class SnackBarTest : ISnackbar
    {
        public IEnumerable<Snackbar> ShownSnackbars { get; }
        public SnackbarConfiguration Configuration { get; }

        public Snackbar? Add(string message, Severity severity = Severity.Normal, Action<SnackbarOptions>? configure = null, string? key = null)
        {
            return null;
        }

        public Snackbar? Add(MarkupString message, Severity severity = Severity.Normal, Action<SnackbarOptions>? configure = null, string? key = null)
        {
            return null;
        }

        public Snackbar? Add(RenderFragment message, Severity severity = Severity.Normal, Action<SnackbarOptions>? configure = null, string? key = null)
        {
            return null;
        }

        public Snackbar? Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(Dictionary<string, object>? componentParameters = null, Severity severity = Severity.Normal, Action<SnackbarOptions>? configure = null, string? key = null) where T : IComponent
        {
            return null;
        }

        public void Clear()
        {

        }

        public void Remove(Snackbar snackbar)
        {

        }

        public void RemoveByKey(string key)
        {

        }

        public void Dispose()
        {

        }

        

        public event Action? OnSnackbarsUpdated;
    }
}
