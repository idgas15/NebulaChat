using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NebulaChat.Core.Models;
using NebulaChat.Core.Models.Interfaces;
using NebulaChat.Data;
using NebulaChat.Services;
using NebulaChat.WebApi.Hubs;
using NebulaChat.WebApi.Scheduler;
using NebulaChat.WebApi.Services;

namespace NebulaChat.WebApi.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            // Sheduled Task to purge old messages
            services.AddSingleton<IHostedService, PurgeOldMessages>();
            // Add all dependency injection here.
            services.AddScoped<IMessage, Message>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<INotifyService, NotifyService>();
            services.AddScoped<ChatHub>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
