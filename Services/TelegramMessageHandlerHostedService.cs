using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nba.Adviser.Abstractions;
using Nba.Adviser.Configurations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nba.Adviser.Services
{
	/// <summary>
	/// Осуществляет управление состоянием бота и отправляет сообщения на обработку.
	/// </summary>
	public class TelegramMessageHandlerHostedService : IHostedService, IDisposable
	{
		private readonly ILogger _logger;
		private readonly CancellationTokenSource _stoppingCts;
		private readonly ITelegramBotClientService _telegramBotClient;
		private readonly IReportService _reportService;
		private bool _disposed;
		public TelegramMessageHandlerHostedService(ILogger logger, ITelegramBotClientService telegramBotClient,
			 IReportService reportService)
		{
			_disposed = false;
			_logger = logger;
			_stoppingCts = new CancellationTokenSource();
			_telegramBotClient = telegramBotClient;
			_reportService = reportService;
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Запуск StartAsync");

				_telegramBotClient.StartReceiving(BotOnMessageReceived, cancellationToken);

				_logger.LogInformation("TelegramMessageHandlerHostedService запущен.");
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при запуске StartAsync");
				throw;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Запуск StopAsync");

			_telegramBotClient.StopReceiving();
			_stoppingCts.Cancel();

			_logger.LogInformation("Окончание StopAsync");

			return Task.CompletedTask;
		}

		/// <summary>
		/// Подписка на обработку сообщений.
		/// </summary>
		/// <param name="message"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Design",
			"CA1031:Не перехватывать исключения общих типов",
			Justification = "Обработка ошибок является глобальной.")]
		private async void BotOnMessageReceived(Message message)
		{

			if (message?.Type == MessageType.Text || message?.Type == MessageType.Document || message?.Type == MessageType.Photo
				|| message?.Type == MessageType.Contact)
			{
				try
				{
					if (message.Entities != null)
					{
						var botCommand = message.Entities.FirstOrDefault(x => x.Type == MessageEntityType.BotCommand);
						{
							if (botCommand != null)
							{
								if (message.Text != null)
								{
									var command = message.Text.Substring(botCommand.Offset, botCommand.Length);
									if (command == "/gettop1")
									{
										var messageTop1 = _reportService.GetReportTop1Async().Result;
										await _telegramBotClient.SendMessage(message.Chat.Id.ToString(), messageTop1);
										return;
									}
									if (command == "/gettop3")
									{
										var messageTop3 = _reportService.GetReportTop3Async().Result;
										await _telegramBotClient.SendMessage(message.Chat.Id.ToString(), messageTop3);
										return;
									}
									if (command == "/gettop5")
									{
										var messageTop5 = _reportService.GetReportTop5Async().Result;
										await _telegramBotClient.SendMessage(message.Chat.Id.ToString(), messageTop5);
										return;
									}

									if (command == "/gettop7")
									{
										var messageTop7 = _reportService.GetReportTop7Async().Result;
										await _telegramBotClient.SendMessage(message.Chat.Id.ToString(), messageTop7);
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка в процедуре BotOnMessageReceived");
				}
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_stoppingCts.Dispose();
				_disposed = true;
			}
		}
	}
}
