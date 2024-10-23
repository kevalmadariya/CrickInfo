using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.CreateModels;
using Microsoft.AspNetCore.Mvc;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class MatchesController : Controller
    {

        private readonly IMatchesRepo _matchRepo;
        private readonly ITeamsRepo _teamRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITournamentRepo _tournamentRepo;
            private readonly ILogger<MatchesController> _logger;


        public MatchesController(IMatchesRepo matchRepo, IUnitOfWork unitOfWork,ITeamsRepo teamRepo,ITournamentRepo tournamentRepo, ILogger<MatchesController> logger)
        {
            _matchRepo = matchRepo;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _teamRepo = teamRepo ?? throw new ArgumentException(nameof(teamRepo));
            _tournamentRepo = tournamentRepo ?? throw new ArgumentException(nameof(tournamentRepo));    
            _logger = logger;
        }

        //[HttpGet]
        //public ViewResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(Matches model)
        //{
        //    ViewData["name"] = "Cricket Match Creation";
        //    ViewBag.PageTitle = "Create New Match";
        //    return View(model);
        //}
        [HttpGet]
        public IActionResult Create()
        {
            int tournament_id = 1;

            //var teams = _unitOfWork.Team.GetAllTeams().ToList(); // Fetch all teams from the database
            var teams = _unitOfWork.Team.GetTeamsByTournamentId(tournament_id).ToList(); // Fetch all teams from the database

            var model = new CreateMatchModel
            {
                teamslist = teams // Assign teams to the model
            };

            ViewData["name"] = "Cricket Match Creation";
            ViewBag.PageTitle = "Create New Match";

            return View(model); // Pass the model (with teamslist) to the view
        }

        [HttpPost]
        //public IActionResult Create(Matches model, int[] selectedTeamIds)
        //{
        //    var teams = _unitOfWork.Team.GetAllTeams().ToList(); // Explicit ToList() here as well
        //    ViewData["name"] = "Cricket Match Creation";
        //    ViewBag.PageTitle = "Create New Match";
        //    //ViewBag.Teams = teams;

        //    _logger.LogInformation($"Processing match creation with {teams.Count} available teams");


        //    if (teams != null)
        //    {
        //        ViewData["name"] = "GT";
        //    }


        //    if (teams == null || !teams.Any())
        //    {
        //        Console.WriteLine($"Fetched teams from the database.");

        //        ViewBag.Teams = new List<Team>(); // Initialize an empty list to avoid null reference in the view
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Fetched {teams.Count} teams from the database.");
        //        foreach (var team in teams)
        //        {
        //            Console.WriteLine($"Team: {team.Name}");
        //        }
        //        ViewBag.Teams = teams;
        //    }


        //    if (ModelState.IsValid)
        //    {
        //        if (selectedTeamIds.Length != 2)
        //        {
        //            ModelState.AddModelError(string.Empty, "You must select exactly two teams.");
        //            return View(model);
        //        }

        //        // Get selected teams
        //        var teamA = _teamRepo.GetTeamById(selectedTeamIds[0]);
        //        var teamB = _teamRepo.GetTeamById(selectedTeamIds[1]);

        //        Matches match = new Matches
        //        {
        //            TeamAId = teamA.TeamId,
        //            TeamBId = teamB.TeamId,
        //            TeamA = teamA,
        //            TeamB = teamB,
        //            MatchDate = model.MatchDate,
        //            Result = model.Result
        //        };
        //        int tournamentId = 1;
        //        _matchRepo.Add(match, tournamentId);
        //        //_unitOfWork.Save();
        //        return View(); // Redirect to match list after creation
        //    }

        //    var errorMessages = ModelState.Values
        //        .SelectMany(v => v.Errors)
        //        .Select(e => e.ErrorMessage)
        //        .ToList();

        //    ViewBag.ErrorMessages = string.Join("<br/>", errorMessages);
        //    ViewBag.ModelStateValid = false;
        //    ViewBag.Message = "There are errors in the form. Please correct them.";

        //    //var teams = _unitOfWork.Team.GetAllTeams(); // Reload teams in case of error
        //    //ViewBag.Teams = teams;
        //    model.Result = "pendding";
        //    model.teamslist = teams;
        //    return View(model);
        //}
        [HttpPost]
        public IActionResult Create(CreateMatchModel model, int[] selectedTeamIds)
        {
            var teams = _unitOfWork.Team.GetAllTeams().ToList();
            ViewData["name"] = "Cricket Match Creation";
            ViewBag.PageTitle = "Create New Match";

            _logger.LogInformation($"Processing match creation with {teams.Count} available teams");

            // Assign teamslist to the model
            model.teamslist = teams;


            // Validate the ModelState
            if (ModelState.IsValid)
            {
                // Validate that exactly two teams are selected
                if (selectedTeamIds.Length != 2)
                {
                    ModelState.AddModelError(string.Empty, "You must select exactly two teams.");
                    return View(model);
                }

                // Fetch the selected teams
                var teamA = _teamRepo.GetTeamById(selectedTeamIds[0]);
                var teamB = _teamRepo.GetTeamById(selectedTeamIds[1]);
                int tournament_id = 1;
                Matches matches = new Matches
                {
                    TeamAId = selectedTeamIds[0],
                    TeamBId = selectedTeamIds[1],
                    TeamA = teamA,
                    TeamB = teamB,
                    Result = model.Result,
                    MatchDate = model.MatchDate,
                    Tournament = _tournamentRepo.GetTournamentById(tournament_id),
                    TournamentId = tournament_id
                };


                // Add match to the database
                _matchRepo.Add(matches,tournament_id);

                return RedirectToAction("Index"); // Redirect after successful creation
            }

            // If ModelState is not valid, capture errors and display them
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            ViewBag.ErrorMessages = string.Join("<br/>", errorMessages);
            ViewBag.ModelStateValid = false;
            ViewBag.Message = "There are errors in the form. Please correct them.";

            // Return the view with the model (and reloaded team list)
            model.teamslist = teams;
            return View(model);
        }

    }
}
