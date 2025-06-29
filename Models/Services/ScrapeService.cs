using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Api;
using System.Text.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace crickinfo_mvc_ef_core.Services
{
    public interface IScrapeService
    {
        Task FetchAndUpdateDataAsync();
        Task FetchAndUpdateDataAsync(string apiUrl);
    }

    public class ScrapeService : IScrapeService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ScrapeService> _logger;
        private readonly int tournament_id = 3;

        public ScrapeService(
            IHttpClientFactory httpClientFactory,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ScrapeService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task FetchAndUpdateDataAsync()
        {
            string defaultApiUrl = "http://127.0.0.1:8000/scrape-data";
            await FetchAndUpdateDataAsync(defaultApiUrl);
        }

        public async Task FetchAndUpdateDataAsync(string apiUrl)
        {
            _logger.LogInformation("Starting data fetch from URL: {ApiUrl}", apiUrl);

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CrickDbContext>();

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiData = JsonSerializer.Deserialize<ApiResponse>(jsonString, options);

                    if (apiData?.Points_Table != null && apiData.Points_Table.Any())
                    {
                        await ProcessPointsTableData(context, apiData.Points_Table);
                    }

                    if (apiData?.Schedule != null && apiData.Schedule.Any())
                    {
                        await ProcessScheduleData(context, apiData.Schedule);
                    }

                    await context.SaveChangesAsync();
                    _logger.LogInformation("Data updated successfully from URL: {ApiUrl}", apiUrl);
                }
                else
                {
                    _logger.LogError("API call failed with status: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from {ApiUrl}: {Message}", apiUrl, ex.Message);
                throw; // Re-throw for Hangfire to handle retry logic
            }
        }

        private async Task ProcessPointsTableData(CrickDbContext context, List<PointTableEntry> pointsTable)
        {
            foreach (var point in pointsTable)
            {
                var existingTeam = context.Teams.FirstOrDefault(t => t.Name == point.TeamName);
                if (existingTeam == null)
                {
                    var newTeam = new Team { Name = point.TeamName, Logo = null };
                    context.Teams.Add(newTeam);
                    await context.SaveChangesAsync();

                    context.TeamsTournaments.Add(new TeamTournament
                    {
                        TeamId = newTeam.Id,
                        TournamentId = tournament_id,
                        Team = newTeam
                    });

                    existingTeam = newTeam;
                }

                var existingPoint = context.Pointstables.FirstOrDefault(p =>
                    p.Team.Name == point.TeamName && p.TournamentId == tournament_id);

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
                    context.Pointstables.Add(new Pointstable
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

        private async Task ProcessScheduleData(CrickDbContext context, List<ScheduleEntry> schedule)
        {
            foreach (var match in schedule)
            {
                var existingMatch = context.Matches.FirstOrDefault(m =>
                    m.MatchNo == match.MatchNo && m.TournamentId == tournament_id);

                Team teamA = context.Teams.FirstOrDefault(m => m.Name == match.Team1);
                Team teamB = context.Teams.FirstOrDefault(m => m.Name == match.Team2);

                if (existingMatch != null)
                {
                    existingMatch.TeamA = teamA;
                    existingMatch.TeamB = teamB;
                    existingMatch.MatchDate = match.Date;
                    existingMatch.Result = match.Result;
                }
                else
                {
                    context.Matches.Add(new Models.Match
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
}