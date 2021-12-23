using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nba.Adviser.Abstractions;
using Nba.Adviser.Services;
using System;
using Microsoft.Extensions.Logging;
using Nba.Adviser.Configurations;
using Nba.Adviser.Schedulers;
using Nba.Adviser.Schedulers.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Nba.Adviser
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
			var serviceProvider = services.BuildServiceProvider();
			var logger = serviceProvider.GetService<ILogger<TelegramBotClientService>>();
			services.AddSingleton(typeof(ILogger), logger ?? throw new InvalidOperationException());
			services.AddHostedService<TelegramMessageHandlerHostedService>();

			services.AddSingleton<INbaClient, NbaClient>();
			services.AddSingleton<ITelegramBotClientService, TelegramBotClientService>();
			services.AddSingleton<IReportService, ReportService>();

			services.Configure<TelegramBotConfiguration>(
				Configuration.GetSection("Telegram"));

			services.AddHttpClient("NbaClient", c =>
			{
				c.BaseAddress = new Uri("https://data.nba.net/");
			});
			services.AddHostedService<QuartzHostedService>();
			services.AddSingleton<IJobFactory, SingletonJobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			services.AddSingleton<TelegramInfoJob>();
			services.AddSingleton(new JobSchedule(
				jobType: typeof(TelegramInfoJob),
				cronExpression: Configuration["Quartz:TelegramAlertExpression"])); // периодичность запуска job
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

		}
	}
}
