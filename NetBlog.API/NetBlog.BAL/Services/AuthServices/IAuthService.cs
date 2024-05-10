using Microsoft.AspNetCore.Identity;
using NetBlog.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.Services.AuthServices
{
    public interface IAuthService
    {
        public Task<IdentityResult> Register(RegisterUserDTO dto);
        public Task<LoginResponseDTO> Login(LoginUserDTO dto);
    }
}
