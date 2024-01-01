
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VbApi.Business.Cqrs;
using VbApi.Business.Mapper;

namespace VbApi
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("MsSqlConnection");
            services.AddDbContext<VbDbContext>(options => options.UseSqlServer(connection));

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add AutoMapper configuration
            services.AddAutoMapper(typeof(MappingProfile)); // Add your mapping profile class
            
            //mediator
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).GetTypeInfo().Assembly));


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(x => { x.MapControllers(); });
        }
    }
}