using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Web.UnitTests
{
    public static class TestUtils
    {
        public static DbContextOptions GetOptions(string databaseName)
        {
            var provider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

            return new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName)
            .UseInternalServiceProvider(provider)
            .Options;
        }
    }
}
