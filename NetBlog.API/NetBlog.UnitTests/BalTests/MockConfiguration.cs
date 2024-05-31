using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.UnitTests.BalTests
{
    public static class MockConfiguration
    {
        public static IConfiguration CreateMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "JiPcElN9eTZM7tOgz90Nzhk6zyV7n2uiH8afjnLE7uhlsoeLdB9Ccp8RqBB2tvW"},
            {"Jwt:Issuer", "https://localhost:1234/"},
            {"Jwt:Audience", "https://localhost:1234/"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            return configuration;
        }
    }
}
