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
using SmartHome.Backend.Api;
//using static SmartHome.Common.Api.DeviceServiceTest;
using SmartHome_Testing.interfaces;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using Newtonsoft.Json;
using SmartHome.Common.Models.Configs;
using AngleSharp.Dom;
using SmartHome.UI.Components.Devices;

namespace SmartHome_Testing
{
    public class DevicesTest : TestContext
    {
        [Fact]
        public async void Title_Zichbaar()
        {
            Room TestRoom1 = new Room
            {
                Id = Guid.Parse("66133695-9398-4e09-b782-7c9702d44206"),
                Name = "Woonkamer"
            };

            Device TestDevice1 = new Device
            {
                Id = Guid.Parse("a4aefbef-0d8d-4f84-b5d2-e9a05eac103a"),
                Name = "Lamp 1",
                Type = DeviceType.Lamp,
                RoomId = Guid.Parse("66133695-9398-4e09-b782-7c9702d44206"),
                Room = TestRoom1,
                JsonObjectConfig = JsonConvert.SerializeObject(new LampConfig() { Brightness = 100, Color = "#ffffff", Enabled = false})
            };

            DeviceServiceTest deviceServiceTest = new DeviceServiceTest();
            deviceServiceTest.TestDevices.Add(TestDevice1);

            RoomServiceTest roomServiceTest = new RoomServiceTest();
            roomServiceTest.TestRooms.Add(TestRoom1);

            IDeviceService deviceService = deviceServiceTest;
            IRoomService roomService = roomServiceTest;
            ISnackbar snackbar = new SnackBarTest();

            Services.AddSingleton<IDeviceService>(deviceService);
            Services.AddSingleton<IRoomService>(roomService);
            Services.AddSingleton<ISnackbar>(snackbar);

            var remderComponent = RenderComponent<Overview>();

          //  var DeviceComponent = remderComponent.FindComponent<DeviceRenderer>();

           // var DeviceComponent = remderComponent.FindComponent<DeviceRenderer>();

           remderComponent.WaitForState(() => remderComponent.Find("h3").TextContent == "Woonkamer", TimeSpan.FromSeconds(1));
           remderComponent.WaitForAssertion(() => remderComponent.Find("h3"));
           
            // remderComponent.Find("h3");

            // var task = await remderComponent.InvokeAsync<Task>(() => remderComponent.MarkupMatches("h3"));

            //Test de blazor component
            remderComponent
                .Find("p")
                .MarkupMatches("<p class=\"mud-typography mud-typography-body1\">You have no devices</p>");

           

        }

        //<h3 class="mud-typography mud-typography-h3 Lamp-Name" style="font-size: 250%">Lamp15</h3>

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
