using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.DTO;
using crickinfo_mvc_ef_core.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamRepo _teamRepo;

        public TeamController(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }
        public IActionResult Content(int id,int tournamentId, int? teamId = null, bool showForm = false)
        {
            ViewBag.User = HttpContext.Session.GetString("Username");
            var teams = _teamRepo.GetTeamsByTournametId(id);
            tournamentId = id;
            Team formModel;
            Console.WriteLine("start Content");
            Console.WriteLine("in contecnt TournaentID : " + id);
            Console.WriteLine("in contecnt TeamID : " + teamId);
            if (teamId.HasValue && teamId.Value != 0)
            {
                // Editing
                formModel = _teamRepo.GetTeamById(teamId.Value) ?? new Team();
            }
            else
            {
                // Adding
                formModel = new Team();
            }

            TeamContentViewModel viewModel = new TeamContentViewModel
            {
                Teams = teams,
                TeamForm = formModel,
                TournamentId = id,
                ShowForm = showForm
            };
            Console.WriteLine("IMPPPPPPPPPPPPPPPPPPPp" + viewModel.TournamentId);
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Save(TeamContentViewModel model, IFormFile? LogoFile)
        {
            var team = model.TeamForm;  
            var tournamentId = model.TournamentId;

            foreach (var formdata in ModelState.Values)
            {
                foreach (var error in formdata.Errors)
                {
                    Console.WriteLine("Error"+error.ErrorMessage);
                }
            }
        
            Console.WriteLine("start Save");
            Console.WriteLine(ModelState.IsValid);
            Console.WriteLine(team.Id);
            Console.WriteLine(tournamentId);
            Console.WriteLine(model.TournamentId);
            

            if (ModelState.IsValid)
            {
                if (team.Id == 0)
                {
                    // Add
                    if (LogoFile != null && LogoFile.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            LogoFile.CopyTo(ms);
                            team.Logo = ms.ToArray();
                        }
                    }

                    _teamRepo.Add(team, tournamentId);
                }
                else
                {
                    // Edit
                    var existingTeam = _teamRepo.GetTeamById(team.Id);
                    if (existingTeam == null)
                    {
                        return NotFound();
                    }

                    existingTeam.Name = team.Name;

                    if (LogoFile != null && LogoFile.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            LogoFile.CopyTo(ms);
                            existingTeam.Logo = ms.ToArray();
                        }
                    }

                    _teamRepo.Update(existingTeam);
                }

                return RedirectToAction("Content", "Team" , new { id = tournamentId });
            }

            // Reload if model is invalid
            var teams = _teamRepo.GetTeamsByTournametId(tournamentId);
            model.Teams = teams;
            model.ShowForm = true;

            return View("Content", model);
        }

    }
}
