using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Application.Abstractions.Data;
using Domain.Followers;
using Application.Abstractions.UserNotifications;
using Infrastructure.UserNotifications;
using Infrastructure.Interceptors;
using Quartz;
using Infrastructure.BackgroundJobs;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("NpgsqlDatabase")
                ?? throw new Exception("Connection string 'NpgsqlDatabase' was not found.");

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, options) =>
            {
                var convertDomainEventsToOutboxMessagesInterceptor =
                    sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

                options.UseNpgsql(connectionString, o => o.MigrationsAssembly("Infrastructure"))
                    .AddInterceptors(convertDomainEventsToOutboxMessagesInterceptor!);
            });

            //services.AddScoped<IFollowerService, FollowerService>();

            services.AddTransient<IUserNotificationService, UserNotificationService>();

            services.AddQuartz(options =>
            {
                JobKey jobKey = new(nameof(ProcessOutboxMessagesJob));

                options
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(
                        trigger =>
                            trigger.ForJob(jobKey)
                                .WithSimpleSchedule(
                                    schedule =>
                                        schedule.WithIntervalInSeconds(10)
                                            .RepeatForever()));
            });

            services.AddQuartzHostedService();

            return services;
        }
    }
}
