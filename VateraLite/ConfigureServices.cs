using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using VateraLite.Application.Interfaces;
using VateraLite.Application.Mappings;
using VateraLite.Application.Services;
using VateraLite.Infrastructure.Identity.Models;
using VateraLite.Infrastructure.Persistence;
using VateraLite.Infrastructure.Services;
using VateraLite.Infrastructure.Settings;

namespace VateraLite
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(configuration.GetConnectionString("VateraDatabase")));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.SignIn.RequireConfirmedAccount = false;
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Identity/Account/Login";
                opt.AccessDeniedPath = "/Identity/Account/AccesDenied";
                opt.Cookie.HttpOnly = true;
            });

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<AppDbContextInitialiser>();

            services.AddRazorPages();
            services.AddSignalR();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStockService, StockService>();
            services.AddSingleton<IConcurrentOrderQueue, ConcurrentOrderQueue>();
            services.AddHostedService<ProductProductionService>();
            services.AddHostedService<OrderFinalizationService>();

            return services;
        }
    }
}
