using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamsRepo _teamRepo;
        private readonly IUnitOfWork _unitOfWork;
        public TeamController(ITeamsRepo teamRepo,IUnitOfWork unitOfWork)
        {
            _teamRepo = teamRepo;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IActionResult Index()
        {
            var teams = _unitOfWork.Team.GetAllTeams();
            return View(teams);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team model)
        {
            int tournament_id = 1;
            model.TeamTournaments = new List<TeamTournament>();
            if (ModelState.IsValid)
            {
                Team team = new Team
                {
                    Name = model.Name,
                    Logo = model.Logo,
                };
                
                //_teamRepo.Add(team,tournament_id);
                _unitOfWork.Team.Add(team, tournament_id);
                _unitOfWork.Save();
                return View(model);
            }

            var errorMessages = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            ViewBag.ErrorMessages = string.Join("<br/>", errorMessages);
            ViewBag.ModelStateValid = false;
            ViewBag.Message = "There are errors in the form. Please correct them." + ModelState.ErrorCount.ToString() + " Errror";
            return View();
        }


    }
}
