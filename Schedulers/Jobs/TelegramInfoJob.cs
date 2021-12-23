using System;
using Nba.Adviser.Abstractions;
using Quartz;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nba.Adviser.Configurations;

namespace Nba.Adviser.Schedulers.Jobs
{
	public class TelegramInfoJob : IJob
	{
		private readonly ITelegramBotClientService _telegramBotClient;
		private readonly IReportService _reportService;
		private readonly TelegramBotConfiguration _configuration;

		public TelegramInfoJob(IOptions<TelegramBotConfiguration> configuration, ITelegramBotClientService telegramBotClient,
			IReportService reportService)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException($"configuration", "is Null");
			}

			_configuration = configuration.Value;
			_telegramBotClient = telegramBotClient;
			_reportService = reportService;
		}
		public async Task Execute(IJobExecutionContext context)
		{
			var message = _reportService.GetReportBestTodayMatchesAsync().Result;
			await _telegramBotClient.SendMessage(_configuration.DefaultChatId, message);
		}

	}
}
