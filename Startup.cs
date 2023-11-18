using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoomRental.Data;
using RoomRental.ErrorDescribers;
using RoomRental.Models;
using RoomRental.Services;

namespace RoomRental
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RoomRentalsContext>(options => options.UseSqlServer(connection), ServiceLifetime.Scoped);

            //Добавление классов авторизации
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;    // уникальный email
                opts.Password.RequiredLength = 6;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
            })
                .AddErrorDescriber<RussianIdentityErrorDescriber>()
                .AddEntityFrameworkStores<RoomRentalsContext>()
                .AddDefaultTokenProviders();

            // внедрение зависимости CachedService
            services.AddScoped<OrganizationService>();
            services.AddScoped<BuildingService>();
            services.AddScoped<RoomService>();
            services.AddScoped<RoomImageService>();
            services.AddScoped<RentalService>();
            services.AddScoped<InvoiceService>();
            services.AddScoped<PeopleService>();

            // добавление кэширования
            services.AddMemoryCache();

            //Добавление сессий
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".RoomRental.Session";
                //options.IdleTimeout = System.TimeSpan.FromSeconds(2*10+240);
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();

            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc();

            //Отключение конечных точек
            services.AddControllersWithViews(mvcOptions =>
            {
                mvcOptions.EnableEndpointRouting = false;
            });
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();    // аутентификация
            app.UseAuthorization();     // авторизация

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}
