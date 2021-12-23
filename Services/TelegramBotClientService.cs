using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nba.Adviser.Abstractions;
using Nba.Adviser.Configurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nba.Adviser.Services
{
	public class TelegramBotClientService : ITelegramBotClientService
	{
		private static TelegramBotClient _botClient;
		private readonly TelegramBotConfiguration _configuration;
		private readonly ILogger _logger;

		private Action<Message> _botOnMessageReceived;

		public TelegramBotClientService(IOptions<TelegramBotConfiguration> configuration,
			ILogger logger
			)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException($"configuration", "is Null");
			}

			_configuration = configuration.Value;
			_logger = logger;
		}

		public TelegramBotClient GetClient()
		{
			try
			{
				_logger.LogInformation("Начало получения TelegramBotClient.");

				var tgBot = _botClient ??= new TelegramBotClient(_configuration.Key);

				_logger.LogInformation("Завершение получения TelegramBotClient.");

				return tgBot;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения TelegramBotClient.");
				throw;
			}
		}

		public async Task SendMessage(string chatId, string text)
		{
			try
			{
				_logger.LogInformation("Начало отправки сообщения в  Telegram.");

				var client = GetClient();
				await client.SendTextMessageAsync(chatId, text).ConfigureAwait(false);

				_logger.LogInformation("Завершение отправки сообщения в  Telegram.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка отправки сообщения в Telegram.");
				throw;
			}
		}

		public Task StartReceiving(Action<Message> botOnMessageReceived, CancellationToken stoppingCts)
		{
			try
			{
				_logger.LogInformation("Начало запуска клиента.");

				var hdTgBot = GetClient();
				_botOnMessageReceived = botOnMessageReceived;
				RegisterCommands();
				using var cts = new CancellationTokenSource();

				ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
				hdTgBot.StartReceiving(HandleUpdateAsync,
					HandleErrorAsync,
					receiverOptions,
					stoppingCts);
				_logger.LogInformation("Завершение запуска клиента Telegram.");

				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка запуска клиента HdTelegram.");
				throw;
			}
		}

		private void RegisterCommands()
		{
			var hdTgBot = GetClient();
			var commands = new List<BotCommand>();
			var commandTop1 = new BotCommand { Description = "Получить top матч за последний день", Command = "/gettop1" };
			commands.Add(commandTop1);
			var commandTop3 = new BotCommand { Description = "Получить top матч за три последних дня", Command = "/gettop3" };
			commands.Add(commandTop3);
			var commandTop5 = new BotCommand { Description = "Получить top матч за пять последних дней", Command = "/gettop5" };
			commands.Add(commandTop5);
			var commandTop7 = new BotCommand { Description = "Получить top матч за семь последних дней", Command = "/gettop7" };
			commands.Add(commandTop7);

			hdTgBot.SetMyCommandsAsync(commands).GetAwaiter().GetResult();

		}

		public Task StopReceiving()
		{
			try
			{
				_logger.LogInformation("Начало остановки клиента Telegram.");

				var hdTgBot = GetClient();
				var token = CancelBotReceiving();
				ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
				hdTgBot.StartReceiving(HandleUpdateAsync,
					HandleErrorAsync,
					receiverOptions,
					token);

				_logger.LogInformation("Завершение остановки клиента Telegram.");

				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка остановки клиента Telegram.");
				throw;
			}

			CancellationToken CancelBotReceiving()
			{
				CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
				CancellationToken token = cancelTokenSource.Token;
				cancelTokenSource.Cancel();
				return token;
			}
		}

		public Task UnsubscribeBotClient()
		{
			try
			{
				_logger.LogInformation("Начало отписки бота от прослушивания.");

				_botOnMessageReceived = null;

				_logger.LogInformation("Завершение отписки бота от прослушивания.");

				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка отписки бота от прослушивания.");
				throw;
			}
		}

		public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
			CancellationToken cancellationToken)
		{
			var errorMessage = exception switch
			{
				ApiRequestException apiRequestException =>
					$"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};

			_logger.Log(LogLevel.Error, errorMessage);
			return Task.CompletedTask;
		}

		public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
			CancellationToken cancellationToken)
		{
			if (update != null)
			{

				var handler = update.Type switch
				{
					UpdateType.Message => BotOnMessageReceived(update.Message!),
					_ => UnknownHandlerAsync(update)
				};

				try
				{
					await handler.ConfigureAwait(false);
				}
				catch (Exception exception)
				{
					await HandleErrorAsync(botClient, exception, cancellationToken).ConfigureAwait(false);
				}
			}
		}

		private Task UnknownHandlerAsync(Update update)
		{
			var logInfo = JsonConvert.SerializeObject(update);
			_logger.Log(LogLevel.Error, $"Ошибка UnknownHandlerAsync: {logInfo}");
			return Task.CompletedTask;
		}

		private async Task BotOnMessageReceived(Message message)
		{
			if (message?.Type == MessageType.Text)
			{
				try
				{
					_logger.LogInformation("Старт процедуры BotOnMessageReceived");

					_botOnMessageReceived?.Invoke(message);
					await Task.CompletedTask.ConfigureAwait(false);

					_logger.LogInformation("Завершение процедуры BotOnMessageReceived");
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка процедуры BotOnMessageReceived");
				}
			}
		}
	}
}
