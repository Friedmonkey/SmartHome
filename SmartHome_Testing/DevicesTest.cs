using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using SmartHome.Common.Api;
using SmartHome.UI.Pages.SmartPages;
using SmartHome.Common.Api;
using SmartHome.UI.Api;
using static SmartHome.Common.Api.IDeviceService;
using SmartHome_Testing.interfaces;

namespace SmartHome_Testing
{
    public class DevicesTest : TestContext
    {
        [Fact]
        public void Title_Zichbaar()
        {            
            IDeviceService deviceServie = new DeviceServiceTest();
            Services.AddSingleton<IDeviceService>(deviceServie);

            var remderComponent = RenderComponent<Overview>();

            //Test de blazor component
            remderComponent
                .Find("p")
                .MarkupMatches("<p class=\"mud-typography mud-typography-body1\">You have no devices</p>");
        }

        //public static int Data { get; set; }

        //[Theory]
        //[InlineData(1)]
        //public void Counter(int Value1)
        //{
        //    //Haal de geladen home component op uit blazor
        //    var remderComponent = RenderComponent<Counter>();

        //    remderComponent.SetParametersAndRender(param => param.Add<int>(p => p.currentCount, 5));

        //    int ExpectedValue = 5;

        //    //Test de blazor component
        //    remderComponent
        //        .Find("p")
        //        .MarkupMatches($"<p role=\"status\">Current count: {ExpectedValue}</p>");

        //}
    }
}
