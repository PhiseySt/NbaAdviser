using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nba.Adviser.Abstractions;
using Nba.Adviser.Models;
using Newtonsoft.Json.Linq;

namespace Nba.Adviser.Services
{
	public class NbaClient : INbaClient
	{
		private readonly IHttpClientFactory _clientFactory;

		public NbaClient(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public async Task<IEnumerable<Team>> GetTeamsAsync()
		{
			var currentYear = DateTime.Now.ToString("yyyy");
			var requestUri = $"prod/v1/{currentYear}/teams.json";
			var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
			var client = _clientFactory.CreateClient("NbaClient");
			var response = await client.SendAsync(request);
			IList<Team> teamListObject = new List<Team>();
			if (response.IsSuccessStatusCode)
			{
				var jsonResults = JObject.Parse(await response.Content.ReadAsStringAsync());
				IList<JToken> teamListResult = jsonResults["league"]?["standard"]?.Children().ToList();

				if (teamListResult != null)
					foreach (var token in teamListResult)
					{
						var team = token.ToObject<Team>();
						teamListObject.Add(team);
					}
			}
			return teamListObject;
		}

		public async Task<IEnumerable<Game>> GetTodaysGamesAsync()
		{
				var currentDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
				var requestUri = $"/prod/v2/{currentDate}/scoreboard.json";
				var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
				var client = _clientFactory.CreateClient("NbaClient");
				var response = await client.SendAsync(request);
				IList<Game> todaySchedule = new List<Game>();
				if (response.IsSuccessStatusCode)
				{
					var jsonResults = JObject.Parse(await response.Content.ReadAsStringAsync());
					IList<JToken> teamListResult = jsonResults["games"]?.Children().ToList();
					if (teamListResult != null)
						foreach (var token in teamListResult)
						{
							var game = token.ToObject<Game>();
							todaySchedule.Add(game);
						}
				}
				return todaySchedule;
		}

		#region Tops for period

		public async Task<IEnumerable<Game>> GetTop1Async()
		{
			var day1Before = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
			var requestUri = $"/prod/v2/{day1Before}/scoreboard.json";
			var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
			var client = _clientFactory.CreateClient("NbaClient");
			var response = await client.SendAsync(request);
			IList<Game> todaySchedule = new List<Game>();
			if (response.IsSuccessStatusCode)
			{
				var jsonResults = JObject.Parse(await response.Content.ReadAsStringAsync());
				IList<JToken> teamListResult = jsonResults["games"]?.Children().ToList();
				if (teamListResult != null)
					foreach (var token in teamListResult)
					{
						var game = token.ToObject<Game>();
						todaySchedule.Add(game);
					}
			}
			return todaySchedule;
		}

		public async Task<IEnumerable<Game>> GetTop3Async()
		{
			const byte countDays = 3;
			var days = new string[countDays];
			for (var i = 0; i < countDays; i++)
			{
				days[i] = DateTime.Now.AddDays(-1*(i+1)).ToString("yyyyMMdd");
			}
	        var requestUrls = new string[countDays];
	        for (var i = 0; i < countDays; i++)
	        {
		        requestUrls[i] = $"/prod/v2/{days[i]}/scoreboard.json";
	        }

	        var client = _clientFactory.CreateClient("NbaClient");

	        var downloads = requestUrls.Select(url => client.GetStringAsync(url));
	        var downloadTasks = downloads.ToArray();

	        var dataNba =
		        await Task.WhenAll(downloadTasks);

	        return (from data in dataNba 
		        select JObject.Parse(data) 
		        into jsonResults 
		        select jsonResults["games"]?.Children().ToList() 
		        into teamListResult 
		        where teamListResult != null 
		        from token in teamListResult 
		        select token.ToObject<Game>()).ToList();
		}

		public async Task<IEnumerable<Game>> GetTop5Async()
		{
			const byte countDays = 5;
			var days = new string[countDays];
			for (var i = 0; i < countDays; i++)
			{
				days[i] = DateTime.Now.AddDays(-1 * (i + 1)).ToString("yyyyMMdd");
			}
			var requestUrls = new string[countDays];
			for (var i = 0; i < countDays; i++)
			{
				requestUrls[i] = $"/prod/v2/{days[i]}/scoreboard.json";
			}

			var client = _clientFactory.CreateClient("NbaClient");

			var downloads = requestUrls.Select(url => client.GetStringAsync(url));
			var downloadTasks = downloads.ToArray();

			var dataNba =
				await Task.WhenAll(downloadTasks);

			return (from data in dataNba
				select JObject.Parse(data)
				into jsonResults
				select jsonResults["games"]?.Children().ToList()
				into teamListResult
				where teamListResult != null
				from token in teamListResult
				select token.ToObject<Game>()).ToList();
		}

		public async Task<IEnumerable<Game>> GetTop7Async()
		{
			const byte countDays = 3;
			var days = new string[countDays];
			for (var i = 0; i < countDays; i++)
			{
				days[i] = DateTime.Now.AddDays(-1 * (i + 1)).ToString("yyyyMMdd");
			}
			var requestUrls = new string[countDays];
			for (var i = 0; i < countDays; i++)
			{
				requestUrls[i] = $"/prod/v2/{days[i]}/scoreboard.json";
			}

			var client = _clientFactory.CreateClient("NbaClient");

			var downloads = requestUrls.Select(url => client.GetStringAsync(url));
			var downloadTasks = downloads.ToArray();

			var dataNba =
				await Task.WhenAll(downloadTasks);

			return (from data in dataNba
				select JObject.Parse(data)
				into jsonResults
				select jsonResults["games"]?.Children().ToList()
				into teamListResult
				where teamListResult != null
				from token in teamListResult
				select token.ToObject<Game>()).ToList();
		}

		#endregion
	}
}
