using Nba.Adviser.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nba.Adviser.Abstractions
{
	public interface INbaClient
	{
		Task<IEnumerable<Team>> GetTeamsAsync();
		Task<IEnumerable<Game>> GetTodaysGamesAsync();
		Task<IEnumerable<Game>> GetTop1Async();
		Task<IEnumerable<Game>> GetTop3Async();
		Task<IEnumerable<Game>> GetTop5Async();
		Task<IEnumerable<Game>> GetTop7Async();
	}
}
