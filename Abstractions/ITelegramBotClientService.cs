using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nba.Adviser.Abstractions
{
	/// <summary>
	/// Представляет логику взаимодействия с ботом.
	/// </summary>
	public interface ITelegramBotClientService
	{
		Task SendMessage(string chatId, string text);
		Task StartReceiving(Action<Message> botOnMessageReceived, CancellationToken stoppingCts);
		Task StopReceiving();
		TelegramBotClient GetClient();
		Task UnsubscribeBotClient();
	}
}
