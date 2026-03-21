

﻿using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;

using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // ASP.NET Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Application Services & Repositories
            services.AddScoped<Application.Interfaces.Repositories.IRoomPostRepository, Infrastructure.Repositories.RoomPostRepository>();
            services.AddScoped<Application.Interfaces.Services.IRoomPostService, Application.Services.RoomPostService>();
            services.AddScoped<Application.Interfaces.Services.ICloudinaryService, Infrastructure.Services.CloudinaryService>();

            services.AddScoped<Application.Interfaces.Repositories.IFavoriteRoomRepository, Infrastructure.Repositories.FavoriteRoomRepository>();
            services.AddScoped<Application.Interfaces.Services.IFavoriteRoomService, Application.Services.FavoriteRoomService>();
            // External OAuth Providers
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:Google:ClientId"]!;
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
                })
                .AddFacebook(options =>
                {
                    options.AppId = configuration["Authentication:Facebook:AppId"]!;
                    options.AppSecret = configuration["Authentication:Facebook:AppSecret"]!;
                });

            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewService, ReviewService>();


            return services;
        }
    }
}
