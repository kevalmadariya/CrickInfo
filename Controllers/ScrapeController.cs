using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Api;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class ScrapeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CrickDbContext _context;
        private int tournament_id = 3;

        public ScrapeController(IHttpClientFactory httpClientFactory, CrickDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        /// <summary>
        /// ✅ Manual Trigger via URL: GET /Scrape/FetchAndUpdateData
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FetchAndUpdateData()
        {
            Console.WriteLine("Manual fetch started.");
            await FetchAndUpdateDataInternal();
            return Content("Data fetched and updated successfully!");
        }

        /// <summary>
        /// ✅ Manual Trigger with URL parameter: GET /Scrape/FetchAndUpdateDataByUrl?url=your-url
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FetchAndUpdateDataByUrl(string url)
        {
            Console.WriteLine("Fetch started ..................");

            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL parameter is required");
            }

            Console.WriteLine($"Manual fetch started for URL: {url}");
            await FetchAndUpdateDataInternal(url);
            return Content($"Data fetched and updated successfully from URL: {url}");
        }

        /// <summary>
        /// ✅ Shared Logic - Default URL
        /// </summary>
        private async Task FetchAndUpdateDataInternal()
        {
            string defaultApiUrl = "http://127.0.0.1:8000/scrape-data";
            await FetchAndUpdateDataInternal(defaultApiUrl);
        }

        /// <summary>
        /// ✅ Shared Logic - Custom URL
        /// </summary>
        private async Task FetchAndUpdateDataInternal(string apiUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiData = JsonSerializer.Deserialize<ApiResponse>(jsonString, options);

                    if (apiData?.Points_Table != null && apiData.Points_Table.Any())
                    {
                        foreach (var point in apiData.Points_Table)
                        {
                            var existingTeam = _context.Teams.FirstOrDefault(t => t.Name == point.TeamName);
                            if (existingTeam == null)
                            {
                                var newTeam = new Team { Name = point.TeamName, Logo = null };
                                _context.Teams.Add(newTeam);
                                await _context.SaveChangesAsync();
                                var t = _context.Teams.FirstOrDefault(t => t.Name == point.TeamName);
                                _context.TeamsTournaments.Add(new TeamTournament
                                {
                                    TeamId = t.Id,
                                    TournamentId = tournament_id,
                                    Team = newTeam
                                });

                                existingTeam = newTeam;
                            }

                            var existingPoint = _context.Pointstables.FirstOrDefault(p => p.Team.Name == point.TeamName && p.TournamentId == tournament_id);

                            if (existingPoint != null)
                            {
                                existingPoint.Wins = point.Won;
                                existingPoint.Loss = point.Lost;
                                existingPoint.Draw = point.Tied;
                                existingPoint.NRR = point.NR;
                                existingPoint.Points = point.Points;
                            }
                            else
                            {
                                _context.Pointstables.Add(new Pointstable
                                {
                                    TournamentId = tournament_id,
                                    Team = existingTeam,
                                    Wins = point.Won,
                                    Loss = point.Lost,
                                    Draw = point.Tied,
                                    NRR = point.NR,
                                    Points = point.Points,
                                });
                            }
                        }
                    }
                    Console.WriteLine("schdule adding start");
                    if (apiData?.Schedule != null && apiData.Schedule.Any())
                    {
                        foreach (var match in apiData.Schedule)
                        {
                            var existingMatch = _context.Matches.FirstOrDefault(m => m.MatchNo == match.MatchNo && m.TournamentId == tournament_id);

                            Team teamA = _context.Teams.FirstOrDefault(m => m.Name == match.Team1);
                            Team teamB = _context.Teams.FirstOrDefault(m => m.Name == match.Team2);

                            if (teamA != null && teamB != null)
                            {
                                Console.WriteLine("teamA and teamB both exist");
                                if (existingMatch != null)
                                {
                                    existingMatch.TeamA = teamA;
                                    existingMatch.TeamB = teamB;
                                    existingMatch.MatchDate = match.Date;
                                    existingMatch.Result = match.Result;
                                }
                                else
                                {
                                    _context.Matches.Add(new Models.Match
                                    {
                                        TournamentId = tournament_id,
                                        MatchNo = match.MatchNo,
                                        TeamA = teamA,
                                        TeamB = teamB,
                                        MatchDate = match.Date,
                                        Result = match.Result
                                    });
                                }
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Data updated successfully from URL: {apiUrl}");
                }
                else
                {
                    Console.WriteLine($"API call failed with status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from {apiUrl}: {ex.Message}");
            }
        }
    }
}