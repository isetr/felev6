using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;
using DoorBash.WebApi;

namespace DoorBash.WebApi.Tests
{
    public class ServerClientFixture : IDisposable
    {
        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public DoorBashDbContext Context { get; private set; }

        public ServerClientFixture()
        {
            // Arrange
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());

            Context = Server.Host.Services.GetRequiredService<DoorBashDbContext>();
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}
