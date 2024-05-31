using NetBlog.BAL.Services.UserSummaryServices;
using NetBlog.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.UnitTests.BalTests
{
    public class UserSummaryServiceTests
    {
        [Fact]
        public async Task GetUserSummaryById_ExistingUserId_ReturnsUserSummary()
        {
            var userManagerMock = MockUserManager.CreateMockUserManager();
            var userSummaryService = new UserSummaryService(userManagerMock.Object);
            var existingUserId = "a6fa9268-7f68-4e94-a5f1-972a1b817550";

            var result = await userSummaryService.GetUserSummary(existingUserId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUserSummaryById_NonExistingUserId_ReturnsErrorResponse()
        {
            var userManagerMock = MockUserManager.CreateMockUserManager();
            var userSummaryService = new UserSummaryService(userManagerMock.Object);
            var nonExistingUserId = "asdasdasdasdasdasdadsdsad1234";

            var result = await userSummaryService.GetUserSummary(nonExistingUserId);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserSummaryById_ExistingUserId_ReturnsUpdatedUserSummary()
        {
            var userManagerMock = MockUserManager.CreateMockUserManager();
            var userSummaryService = new UserSummaryService(userManagerMock.Object);
            var existingUserId = "a6fa9268-7f68-4e94-a5f1-972a1b817550";
            var updatedUserSummary = new UpdateUserDTO { Name = "New name", Bio = "new bio"};

            var result = await userSummaryService.UpdateUserSummaryById(updatedUserSummary, existingUserId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateUserSummaryById_NonExistingUserId_ReturnsErrorResponse()
        {
            var userManagerMock = MockUserManager.CreateMockUserManager();
            var userSummaryService = new UserSummaryService(userManagerMock.Object);
            var nonExistingUserId = "asdasdasdasdasd";
            var updatedUserSummary = new UpdateUserDTO { Name = "New name", Bio = "new bio" };

            var result = await userSummaryService.UpdateUserSummaryById(updatedUserSummary, nonExistingUserId);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserSummaryById_InvalidData_ReturnsErrorResponse()
        {
            var userManagerMock = MockUserManager.CreateMockUserManager();
            var userSummaryService = new UserSummaryService(userManagerMock.Object);
            var existingUserId = "a6fa9268-7f68-4e94-a5f1-972a1b817550";
            var invalidUpdatedUserSummary = new UpdateUserDTO();

            var result = await userSummaryService.UpdateUserSummaryById(invalidUpdatedUserSummary, existingUserId);

            Assert.Null(result);
        }
    }
}
