using NetBlog.Application.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.IntegrationTests.Utilities
{
    public static class JWTGenerator
    {
            public static async Task<string> GetExistingAuthorJwt(HttpClient client)
            {
                var loginDto = new LoginUserDTO
                {
                    Email = "frookt4555@gmail.com",
                    Password = "SuperSecurePaswwordqwerty@"
                };
                var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/Auth/login", content);

                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(responseString);

                return loginResponse.Token;
            }
            public static async Task<string> GetExistingReaderJwt(HttpClient client)
            {
                var loginDto = new LoginUserDTO
                {
                    Email = "frookt4444@gmail.com",
                    Password = "SuperSecurePaswwordqwerty@"
                };
                var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/Auth/login", content);

                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(responseString);

                return loginResponse.Token;
        }
       
    }
}
