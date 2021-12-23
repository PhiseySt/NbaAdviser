using Newtonsoft.Json;
using System;

namespace Nba.Adviser.Models
{
	public class Game
    {
        public int GameID { get; set; }
        [JsonProperty("hTeam")]
        public Team HomeTeam { get; set; }
        [JsonProperty("vTeam")]
        public Team GuestTeam { get; set; }
        public string StartTimeEastern { get; set; }
        [JsonProperty("clock")]
        public string Clock { get; set; }
        public DateTimeOffset? StartTimeUTC { get; set; }

		public int ScoreDifferent
		{
			get
			{
				if (HomeTeam?.Score == null || GuestTeam?.Score == null)
					return int.MaxValue;
				return Math.Abs((int) (HomeTeam.Score - GuestTeam.Score));
			}
		}

		public int WinLossDifferent
		{
			get
			{
				if (HomeTeam?.Wins == null || GuestTeam?.Wins == null || HomeTeam.Losses == null || GuestTeam.Losses == null)
					return int.MinValue;
				return HomeTeam.Wins + GuestTeam.Wins - HomeTeam.Losses - GuestTeam.Losses;
			}
		}
	}
}
