using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.UnitTests.BalTests
{
    public static class MockUserManager
    {
        public static Mock<UserManager<IdentityUser>> CreateMockUserManager()
        {
            var users = new List<IdentityUser>
        {
            {
                new IdentityUser { Id = "114419fb-e456-43e9-9cc4-2612f8a8480d", Email = "oof1@gmail.com", UserName = "User1" }
            },
            {
                new IdentityUser { Id = "a6fa9268-7f68-4e94-a5f1-972a1b817550", Email = "ooof2@gmail.com", UserName = "User2" }
            },
        };

            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                null, null, null, null, null, null, null, null);

            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => users.FirstOrDefault(u=>u.Id==userId));

            userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email == email));

            userManagerMock.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(new List<Claim> { new Claim("bio","Bio") });
            userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(new List<string> { "Author" });

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(m => m.AddToRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.AddClaimAsync(It.IsAny<IdentityUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);
            return userManagerMock;
        }
    }
}
