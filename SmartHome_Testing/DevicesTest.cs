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
using Bunit.TestDoubles;
using Moq;
using SmartHome.Database.ApiContext;
using Microsoft.EntityFrameworkCore;
using SmartHome.Database;
using Bogus;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using SmartHome.Database.Auth;
using SmartHome.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using static SmartHome.Common.Api.IDeviceService;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Hosting.Server;
//using static SmartHome.Common.SharedConfig.Urls;
using FastEndpoints;

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
                Name = "Lamp 55",
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

        IDeviceService deviceServiceFunctions;
        Mock<SmartHomeContext> mockDbContext = new Mock<SmartHomeContext>();

        public ApiContext GetDbContext()
        {
            string connectionString = @"server = localhost; database = SmartHome; uid = root";
            var obj = new DbContextOptionsBuilder<SmartHomeContext>().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;

            SmartHomeContext smartHomeContext = new SmartHomeContext(obj);

            DeviceContext deviceContext1 = new DeviceContext(smartHomeContext);
            RoomContext roomContext1 = new RoomContext(smartHomeContext);

            //AuthContext
            IHttpContextAccessor httpContextAccessor = null;
            UserManager<AuthAccount> userManager = null;
            AuthContext authContext1 = new AuthContext(httpContextAccessor, smartHomeContext, userManager);
            //
            ApiContext apiContext = new ApiContext(authContext1, deviceContext1, roomContext1, smartHomeContext);

            return apiContext;

        }


        [Fact]
        public async Task TestApi()
        {
            Device TestDevice1 = new Device
            {
                Id = Guid.Parse("a4aefbef-0d8d-4f84-b5d2-e9a05eac103a"),
                Name = "Lamp 1",
                Type = DeviceType.Lamp,
                RoomId = Guid.Parse("08dd5264-bf3e-4884-821c-e41670578c32"),
                JsonObjectConfig = JsonConvert.SerializeObject(new LampConfig() { Brightness = 100, Color = "#ffffff", Enabled = false })
            };

            List<Device> deviceList = new List<Device>();
            deviceList.Add(TestDevice1);
            var deviceData = deviceList.AsQueryable();

            var mockSet = new Mock<DbSet<Device>>();

            mockSet.Setup(x => x.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Callback((Device model, CancellationToken token) => { })
                .Returns((Device model, CancellationToken token) => ValueTask.FromResult((EntityEntry<Device>)null));
            mockDbContext.Setup(x => x.Set<Device>()).Returns(mockSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));
            var authContext = new Mock<AuthContext>();
            var Service = new SmartHome.Backend.Api.DeviceService(GetDbContext());

            DeviceRequest request = new DeviceRequest(TestDevice1);
            request.smartHome = Guid.Parse("054aba40-97d2-4b85-8269-35206b8141b7");
            var result = await Service.CreateDevice(request);
        }

        [Fact]
        public async Task TestApiDelete()
        {
            Device TestDevice1 = new Device
            {
                Id = Guid.Parse("a4aefbef-0d8d-4f84-b5d2-e9a05eac103a"),
                Name = "Lamp 1",
                Type = DeviceType.Lamp,
                RoomId = Guid.Parse("08dd5264-bf3e-4884-821c-e41670578c32"),
                JsonObjectConfig = JsonConvert.SerializeObject(new LampConfig() { Brightness = 100, Color = "#ffffff", Enabled = false })
            };

            List<Device> deviceList = new List<Device>();
            deviceList.Add(TestDevice1);
            var deviceData = deviceList.AsQueryable();

            var mockSet = new Mock<DbSet<Device>>();

            mockSet.Setup(x => x.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Callback((Device model, CancellationToken token) => { })
                .Returns((Device model, CancellationToken token) => ValueTask.FromResult((EntityEntry<Device>)null));
            mockDbContext.Setup(x => x.Set<Device>()).Returns(mockSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));
            var authContext = new Mock<AuthContext>();
            var Service = new SmartHome.Backend.Api.DeviceService(GetDbContext());

            //Delete device guid
            DeleteDeviceRequest request = new DeleteDeviceRequest(Guid.Parse("08dd5263-f10d-46d6-830a-28ef72ad54c7"));
            var result = await Service.DeleteDevice(request);
        }

        [Fact]
        public async Task TestApiCreateRoom()
        {
            Room TestRoom1 = new Room
            {
                Id = Guid.Parse("9cd030d7-347e-4673-8a78-e180e22162c9"),
                Name = "Woonkamer1"
            };

            List<Room> roomList = new List<Room>();
            roomList.Add(TestRoom1);
            var roomData = roomList.AsQueryable();

            var mockSet = new Mock<DbSet<Room>>();

            mockSet.Setup(x => x.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
                .Callback((Room model, CancellationToken token) => { })
                .Returns((Room model, CancellationToken token) => ValueTask.FromResult((EntityEntry<Room>)null));
            mockDbContext.Setup(x => x.Set<Room>()).Returns(mockSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));
            var authContext = new Mock<AuthContext>();
            var Service = new SmartHome.Backend.Api.RoomService(GetDbContext());

            //Delete device guid
            RoomRequest request = new RoomRequest(TestRoom1);
            request.smartHome = Guid.Parse("054aba40-97d2-4b85-8269-35206b8141b7");
            var result = await Service.CreateRoom(request);
        }
    }
}
