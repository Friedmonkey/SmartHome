using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using SmartHome.Database.Auth;
using Xunit;
using static SmartHome.Common.Api.ISmartHomeService;
using static SmartHome.Common.SharedConfig.Urls;

namespace SmartHome.Api.Test.AccountService
{
    public class SmartHomeServiceTests
    {
        private readonly Mock<ApiContext> _mockApiContext;
        private readonly Mock<DbContext> _mockDbContext;
        private readonly Mock<DbSet<SmartHomeModel>> _mockSmartHomes;
        private readonly Mock<DbSet<SmartUserModel>> _mockSmartUsers;
        private readonly SmartHomeService _service;

        public SmartHomeServiceTests()
        {
            _mockApiContext = new Mock<ApiContext>();
            _mockDbContext = new Mock<DbContext>();

            _mockSmartHomes = new Mock<DbSet<SmartHomeModel>>();
            _mockSmartUsers = new Mock<DbSet<SmartUserModel>>();

            _mockApiContext.Setup(c => c.DbContext).Returns((Database.SmartHomeContext)_mockDbContext.Object);
            _mockDbContext.Setup(d => d.Set<SmartHomeModel>()).Returns(_mockSmartHomes.Object);
            _mockDbContext.Setup(d => d.Set<SmartUserModel>()).Returns(_mockSmartUsers.Object);

            _service = new SmartHomeService(_mockApiContext.Object);
        }

        [Fact]
        public async Task CreateSmartHome_ShouldReturnValidGuid()
        {
            // Arrange
            var request = new CreateSmartHomeRequest("Home 1", "MyWiFi", "Pass123");
            var homeId = Guid.NewGuid();

            var home = new SmartHomeModel
            {
                Id = homeId,
                Name = request.name,
                SSID = request.wifiname,
                SSPassword = request.password
            };

            _mockSmartHomes.Setup(m => m.AddAsync(It.IsAny<SmartHomeModel>(), default))
                .ReturnsAsync((SmartHomeModel model, CancellationToken _) =>
                    new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<SmartHomeModel>(
                        Mock.Of<Microsoft.EntityFrameworkCore.ChangeTracking.Internal.InternalEntityEntry>())
                );

            _mockApiContext.Setup(a => a.GetLoggedInId()).Returns(Guid.NewGuid());

            _mockSmartUsers.Setup(m => m.AddAsync(It.IsAny<SmartUserModel>(), default))
                .ReturnsAsync((SmartUserModel model, CancellationToken _) =>
                    new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<SmartUserModel>(
                        Mock.Of<Microsoft.EntityFrameworkCore.ChangeTracking.Internal.InternalEntityEntry>())
                );

            _mockDbContext.Setup(d => d.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _service.CreateSmartHome(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task InviteToSmartHome_ShouldReturnSuccess()
        {
            // Arrange
            var smartHomeId = Guid.NewGuid();
            var request = new InviteRequest(smartHomeId, "user@example.com");
            var account = new AuthAccount { Id = Guid.NewGuid(), Email = request.email };

            _mockApiContext.Setup(a => a.EnforceIsSmartHomeAdmin(smartHomeId)).Returns(Task.CompletedTask);
            _mockApiContext.Setup(a => a.GetAccountByEmail(request.email)).ReturnsAsync(account);

            _mockSmartUsers.Setup(m => m.AddAsync(It.IsAny<SmartUserModel>(), default))
                .ReturnsAsync((SmartUserModel model, CancellationToken _) =>
                    new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<SmartUserModel>(
                        Mock.Of<Microsoft.EntityFrameworkCore.ChangeTracking.Internal.InternalEntityEntry>())
                );

            _mockDbContext.Setup(d => d.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _service.InviteToSmartHome(request);

            // Assert
            Assert.True(result._RequestSuccess);
        }

        [Fact]
        public async Task AcceptSmartHomeInvite_ShouldSucceed()
        {
            // Arrange
            var smartHomeId = Guid.NewGuid();
            var request = new SmartHomeRequest() { smartHome = smartHomeId };

            var smartUser = new SmartUserModel
            {
                AccountId = Guid.NewGuid(),
                SmartHomeId = smartHomeId,
                Role = UserRole.InvitationPending
            };

            _mockApiContext.Setup(a => a.GetLoggedInSmartUser(smartHomeId)).ReturnsAsync(smartUser);
            _mockDbContext.Setup(d => d.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _service.AcceptSmartHomeInvite(request);

            // Assert
            Assert.True(result._RequestSuccess);
        }

        [Fact]
        public async Task AcceptSmartHomeInvite_ShouldFailIfNoPendingInvite()
        {
            // Arrange
            var smartHomeId = Guid.NewGuid();
            var request = new SmartHomeRequest() { smartHome = smartHomeId };

            var smartUser = new SmartUserModel
            {
                AccountId = Guid.NewGuid(),
                SmartHomeId = smartHomeId,
                Role = UserRole.User // Not InvitationPending
            };

            _mockApiContext.Setup(a => a.GetLoggedInSmartUser(smartHomeId)).ReturnsAsync(smartUser);

            // Act
            var result = await _service.AcceptSmartHomeInvite(request);

            // Assert
            Assert.False(result._RequestSuccess);
            Assert.Equal("You dont have an invitation.", result._RequestMessage);
        }

        [Fact]
        public async Task GetJoinedSmartHomes_ShouldReturnHomes()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var smartHomes = new List<SmartHomeModel>
            {
                new SmartHomeModel { Id = Guid.NewGuid(), Name = "Home 1" },
                new SmartHomeModel { Id = Guid.NewGuid(), Name = "Home 2" }
            };

            var smartUsers = new List<SmartUserModel>
            {
                new SmartUserModel { AccountId = userId, SmartHomeId = smartHomes[0].Id, Role = UserRole.User },
                new SmartUserModel { AccountId = userId, SmartHomeId = smartHomes[1].Id, Role = UserRole.Admin }
            }.AsQueryable();

            _mockApiContext.Setup(a => a.GetLoggedInId()).Returns(userId);
            _mockSmartUsers.As<IQueryable<SmartUserModel>>().Setup(m => m.Provider).Returns(smartUsers.Provider);
            _mockSmartUsers.As<IQueryable<SmartUserModel>>().Setup(m => m.Expression).Returns(smartUsers.Expression);
            _mockSmartUsers.As<IQueryable<SmartUserModel>>().Setup(m => m.ElementType).Returns(smartUsers.ElementType);
            _mockSmartUsers.As<IQueryable<SmartUserModel>>().Setup(m => m.GetEnumerator()).Returns(smartUsers.GetEnumerator());

            _mockSmartHomes.Setup(m => m.ToListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(smartHomes);

            // Act
            var result = await _service.GetJoinedSmartHomes(new EmptyRequest());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.homes.Count);
        }
    }
}
