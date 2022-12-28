using Console.Jobs.MessageReader;
using Console.Services;
using Domain.Configs;
using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

using var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(
        (_, services) => services
            .AddSingleton<MessageConsumer>()
            .AddSingleton<ISchedulerFactory, StdSchedulerFactory>()
            .AddHostedService<MessageReaderScheduledService>()
            .AddTransient<IMessageRepository, MessageRepository>())
    .Build();

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

IConfiguration config = configurationBuilder.Build();

var dbSection = config.GetSection("DbConfig");
DbConfig.Server = dbSection.GetValue(typeof(string), "Server").ToString();
DbConfig.Database = dbSection.GetValue(typeof(string), "Database").ToString();
DbConfig.UserId = dbSection.GetValue(typeof(string), "UserId").ToString();
DbConfig.Password = dbSection.GetValue(typeof(string), "Password").ToString();

var rabbitMqSection = config.GetSection("RabbitMqConfig");
RabbitMqConfig.HostName = rabbitMqSection.GetValue(typeof(string), "HostName").ToString();
RabbitMqConfig.UserName = rabbitMqSection.GetValue(typeof(string), "UserName").ToString();
RabbitMqConfig.Password = rabbitMqSection.GetValue(typeof(string), "Password").ToString();
RabbitMqConfig.VirtualHost = rabbitMqSection.GetValue(typeof(string), "VirtualHost").ToString();

MessageConsumer messageConsumer = host.Services.GetRequiredService<MessageConsumer>();

host.Run();