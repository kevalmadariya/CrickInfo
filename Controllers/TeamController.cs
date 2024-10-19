using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.Models;
using Microsoft.AspNetCore.Mvc;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamsRepo _teamRepo;

        public TeamController(ITeamsRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team model)
        {
            int tournament_id = 2;
            model.Tournaments = new List<Tournament>();
            if (ModelState.IsValid)
            {
                Team team = new Team
                {
                    Name = model.Name,
                    Logo = model.Logo,
                };

                _teamRepo.Add(team,tournament_id);
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
