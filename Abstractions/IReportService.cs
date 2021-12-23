using System.Threading.Tasks;

namespace Nba.Adviser.Abstractions
{
	public interface IReportService
	{
		Task<string> GetReportBestTodayMatchesAsync();
		Task<string> GetReportTop1Async();
		Task<string> GetReportTop3Async();
		Task<string> GetReportTop5Async();
		Task<string> GetReportTop7Async();
	}
}
