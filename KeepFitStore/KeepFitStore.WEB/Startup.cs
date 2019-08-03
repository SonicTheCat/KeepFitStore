namespace KeepFitStore.WEB
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Identity.UI.Services;

    using KeepFitStore.Services;
    using KeepFitStore.Services.MessageSender;
    using KeepFitStore.Data;
    using KeepFitStore.WEB.MappingConfiguration;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Middlewares;
    using KeepFitStore.Helpers;
    using KeepFitStore.Domain;

    using Microsoft.AspNetCore.Authentication.Cookies;

    using AutoMapper;
    using KeepFitStore.WEB.Rules;
    using Microsoft.AspNetCore.Rewrite;
    using Stripe;
    using KeepFitStore.Services.PhotoKeeper;
    using KeepFitStore.WEB.Filters;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<KeepFitDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<KeepFitUser, IdentityRole>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequiredUniqueChars = 0;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireLowercase = false;

                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;

                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = new TimeSpan(1, 0, 0); 
            })
              .AddEntityFrameworkStores<KeepFitDbContext>()
              .AddDefaultUI(UIFramework.Bootstrap4)
              .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            //Facebook Authentication 
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            //Services
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IProteinsService, ProteinsService>();
            services.AddTransient<ICreatinesService, CreatinesService>();
            services.AddTransient<IVitaminsService, VitaminsService>();
            services.AddTransient<IAminosService, AminosService>();
            services.AddTransient<IBasketService, BasketSerivce>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IUsersService, UsersSerivce>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<IJobPositionService, JobPositionService>();
            services.AddTransient<IJobApplicantService, JobApplicantService>();

            //Email Sender Services
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new KeepFitProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //Cloudinary - storing photos
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddSingleton<IMyCloudinary, MyCloudinary>();

            //Stripe
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe")); 

            services.AddSession();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
              //  options.Filters.Add(new KeepFitExceptionFilter(true));

                //Set default message for [Required] attribute
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    (_) => "The field is required"); 
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //stripe
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseCreateRolesMiddleware();
            app.UseCreatePowerUserMiddleware();

            //var rewriteOptions = new RewriteOptions().Add(new AdminRewriteRule());
            //app.UseRewriter(rewriteOptions);

            app.UseSession(); 

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "productRoute",
                //    template: "Administrator/{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}