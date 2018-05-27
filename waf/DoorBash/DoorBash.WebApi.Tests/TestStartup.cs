using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DoorBash.Persistence;

namespace DoorBash.WebApi.Tests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DoorBashDbContext>(options =>
                options.UseInMemoryDatabase("TestDB"));


            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DoorBashDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DoorBashDbContext context)
        {
            app.UseAuthentication();
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>(); // automatikus "bejelentkezés"

            app.UseMvc();
            DbInitializer.Initialize(context);
        }
    }
}
