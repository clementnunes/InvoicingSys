using Microsoft.OpenApi.Models;
using DotNetEnv;
using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoicingSys.CoreApi
{
    public class Startup
    {
        public IWebHostEnvironment CurrentEnvironment { get; set; }
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
            Env.Load();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<AddressService>();
            services.AddTransient<CustomerService>();
            services.AddTransient<OrderLineService>();
            services.AddTransient<OrderService>();
            services.AddTransient<ProductService>();
            services.AddTransient<BankDetailService>();
            services.AddTransient<InvoiceService>();
            
            var connectionString = $"Host={Env.GetString("POSTGRES_HOST")};" +
                                   $"Port={Env.GetString("POSTGRES_PORT")};" +
                                   $"Database={Env.GetString("POSTGRES_DATABASE")};" +
                                   $"Username={Env.GetString("POSTGRES_USER")};" +
                                   $"Password={Env.GetString("POSTGRES_PASSWORD")};";

            if (CurrentEnvironment.EnvironmentName == "Testing")
            {
                connectionString = $"Host=host.docker.internal;" +
                                       $"Port=5548;" +
                                       $"Database=invoicing_sys_test;" +
                                       $"Username=test;" +
                                       $"Password=eKf4xC52c.!2dI;";
            }
            
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            
            /*services.AddCors(options =>
            {
                options.AddPolicy(name: "InvoicingSysCORSPolicy",
                    policy  =>
                    {
                        policy.WithOrigins(Environment.GetEnvironmentVariable("CLIENT_URL"))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });*/
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "InvoicingSys", Version = "v1"});
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InvoicingSys"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            
            //app.UseCors("InvoicingSysCORSPolicy");
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}