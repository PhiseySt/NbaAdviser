using Nba.Adviser.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nba.Adviser.Services
{
	public class ReportService : IReportService
	{
		private readonly INbaClient _nbaClient;

		public ReportService(INbaClient nbaClient)
		{
			_nbaClient = nbaClient;
		}
		public async Task<string> GetReportBestTodayMatchesAsync()
		{
			var teams = await _nbaClient.GetTeamsAsync();
			var games = await _nbaClient.GetTodaysGamesAsync();

	        var listGames = games.ToList();
			if (listGames.Count==0) return "Игр сегодня не было.";
			var minDifferentScore = listGames.Min(g=> g.ScoreDifferent);
			var maxWinLossDifferent
				= listGames.Where(g => g.ScoreDifferent == minDifferentScore).Max(g=>g.WinLossDifferent);
		    var gameMinDifferentScore
				= listGames.FirstOrDefault(g => g.ScoreDifferent == minDifferentScore && g.WinLossDifferent== maxWinLossDifferent);

			var listTeams = teams.ToList();
			var homeTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.HomeTeam.TeamId)?.FullName;
			var guestTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.GuestTeam.TeamId)?.FullName;

			var resultMessage = new StringBuilder();
			resultMessage.AppendLine($"Сегодня лучший по накалу матч: {homeTeamName} - {guestTeamName}.");

			var bestTeamsValue = listGames.Max(g => g.WinLossDifferent);
			var minScoreDifferent
				= listGames.Where(g => g.WinLossDifferent == bestTeamsValue).Min(g => g.ScoreDifferent);

			var gameMaxWinLossDifferent = listGames.FirstOrDefault(g =>
				g.ScoreDifferent == minScoreDifferent && g.WinLossDifferent == bestTeamsValue);
			var homeTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.HomeTeam.TeamId)?.FullName;
			var guestTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.GuestTeam.TeamId)?.FullName;

			resultMessage.AppendLine($"Сегодня самый высокий рейтинг у команд в паре: {homeTeamName2} - {guestTeamName2}.");

			return resultMessage.ToString();
		}

		public async Task<string> GetReportTop1Async()
		{
			var teams = await _nbaClient.GetTeamsAsync();
			var games = await _nbaClient.GetTop1Async();

			var listGames = games.ToList();
			if (listGames.Count == 0) return "Игр за период не было.";
			var minDifferentScore = listGames.Min(g => g.ScoreDifferent);
			var maxWinLossDifferent
				= listGames.Where(g => g.ScoreDifferent == minDifferentScore).Max(g => g.WinLossDifferent);
			var gameMinDifferentScore
				= listGames.FirstOrDefault(g => g.ScoreDifferent == minDifferentScore && g.WinLossDifferent == maxWinLossDifferent);

			var listTeams = teams.ToList();
			var homeTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.HomeTeam.TeamId)?.FullName;
			var guestTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.GuestTeam.TeamId)?.FullName;

			var resultMessage = new StringBuilder();
			resultMessage.AppendLine($"За период лучший по накалу матч: {homeTeamName} - {guestTeamName}.");

			var bestTeamsValue = listGames.Max(g => g.WinLossDifferent);
			var minScoreDifferent
				= listGames.Where(g => g.WinLossDifferent == bestTeamsValue).Min(g => g.ScoreDifferent);

			var gameMaxWinLossDifferent = listGames.FirstOrDefault(g =>
				g.ScoreDifferent == minScoreDifferent && g.WinLossDifferent == bestTeamsValue);
			var homeTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.HomeTeam.TeamId)?.FullName;
			var guestTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.GuestTeam.TeamId)?.FullName;

			resultMessage.AppendLine($"За период самый высокий рейтинг у команд в паре: {homeTeamName2} - {guestTeamName2}.");

			return resultMessage.ToString();
		}

		public async Task<string> GetReportTop3Async()
		{
			var teams = await _nbaClient.GetTeamsAsync();
			var games = await _nbaClient.GetTop3Async();

			var listGames = games.ToList();
			if (listGames.Count == 0) return "Игр за период не было.";
			var minDifferentScore = listGames.Min(g => g.ScoreDifferent);
			var maxWinLossDifferent
				= listGames.Where(g => g.ScoreDifferent == minDifferentScore).Max(g => g.WinLossDifferent);
			var gameMinDifferentScore
				= listGames.FirstOrDefault(g => g.ScoreDifferent == minDifferentScore && g.WinLossDifferent == maxWinLossDifferent);

			var listTeams = teams.ToList();
			var homeTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.HomeTeam.TeamId)?.FullName;
			var guestTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.GuestTeam.TeamId)?.FullName;

			var resultMessage = new StringBuilder();
			resultMessage.AppendLine($"За период лучший по накалу матч: {homeTeamName} - {guestTeamName}.");

			var bestTeamsValue = listGames.Max(g => g.WinLossDifferent);
			var minScoreDifferent
				= listGames.Where(g => g.WinLossDifferent == bestTeamsValue).Min(g => g.ScoreDifferent);

			var gameMaxWinLossDifferent = listGames.FirstOrDefault(g =>
				g.ScoreDifferent == minScoreDifferent && g.WinLossDifferent == bestTeamsValue);
			var homeTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.HomeTeam.TeamId)?.FullName;
			var guestTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.GuestTeam.TeamId)?.FullName;

			resultMessage.AppendLine($"За период самый высокий рейтинг у команд в паре: {homeTeamName2} - {guestTeamName2}.");

			return resultMessage.ToString();
		}

		public async Task<string> GetReportTop5Async()
		{
			var teams = await _nbaClient.GetTeamsAsync();
			var games = await _nbaClient.GetTop5Async();

			var listGames = games.ToList();
			if (listGames.Count == 0) return "Игр за период не было.";
			var minDifferentScore = listGames.Min(g => g.ScoreDifferent);
			var maxWinLossDifferent
				= listGames.Where(g => g.ScoreDifferent == minDifferentScore).Max(g => g.WinLossDifferent);
			var gameMinDifferentScore
				= listGames.FirstOrDefault(g => g.ScoreDifferent == minDifferentScore && g.WinLossDifferent == maxWinLossDifferent);

			var listTeams = teams.ToList();
			var homeTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.HomeTeam.TeamId)?.FullName;
			var guestTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.GuestTeam.TeamId)?.FullName;

			var resultMessage = new StringBuilder();
			resultMessage.AppendLine($"За период лучший по накалу матч: {homeTeamName} - {guestTeamName}.");

			var bestTeamsValue = listGames.Max(g => g.WinLossDifferent);
			var minScoreDifferent
				= listGames.Where(g => g.WinLossDifferent == bestTeamsValue).Min(g => g.ScoreDifferent);

			var gameMaxWinLossDifferent = listGames.FirstOrDefault(g =>
				g.ScoreDifferent == minScoreDifferent && g.WinLossDifferent == bestTeamsValue);
			var homeTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.HomeTeam.TeamId)?.FullName;
			var guestTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.GuestTeam.TeamId)?.FullName;

			resultMessage.AppendLine($"За период самый высокий рейтинг у команд в паре: {homeTeamName2} - {guestTeamName2}.");

			return resultMessage.ToString();
		}

		public async Task<string> GetReportTop7Async()
		{
			var teams = await _nbaClient.GetTeamsAsync();
			var games = await _nbaClient.GetTop7Async();

			var listGames = games.ToList();
			if (listGames.Count == 0) return "Игр за период не было.";
			var minDifferentScore = listGames.Min(g => g.ScoreDifferent);
			var maxWinLossDifferent
				= listGames.Where(g => g.ScoreDifferent == minDifferentScore).Max(g => g.WinLossDifferent);
			var gameMinDifferentScore
				= listGames.FirstOrDefault(g => g.ScoreDifferent == minDifferentScore && g.WinLossDifferent == maxWinLossDifferent);

			var listTeams = teams.ToList();
			var homeTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.HomeTeam.TeamId)?.FullName;
			var guestTeamName = listTeams.FirstOrDefault(t => t.TeamId == gameMinDifferentScore.GuestTeam.TeamId)?.FullName;

			var resultMessage = new StringBuilder();
			resultMessage.AppendLine($"За период лучший по накалу матч: {homeTeamName} - {guestTeamName}.");

			var bestTeamsValue = listGames.Max(g => g.WinLossDifferent);
			var minScoreDifferent
				= listGames.Where(g => g.WinLossDifferent == bestTeamsValue).Min(g => g.ScoreDifferent);

			var gameMaxWinLossDifferent = listGames.FirstOrDefault(g =>
				g.ScoreDifferent == minScoreDifferent && g.WinLossDifferent == bestTeamsValue);
			var homeTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.HomeTeam.TeamId)?.FullName;
			var guestTeamName2 = listTeams.FirstOrDefault(t => t.TeamId == gameMaxWinLossDifferent.GuestTeam.TeamId)?.FullName;

			resultMessage.AppendLine($"За период самый высокий рейтинг у команд в паре: {homeTeamName2} - {guestTeamName2}.");

			return resultMessage.ToString();
		}
	}
}
