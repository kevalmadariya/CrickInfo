using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ITournamentRepo _tournamentRepo;

        public TournamentController(ITournamentRepo tournamentRepo)
        {
            _tournamentRepo = tournamentRepo;
        }

        [HttpGet]
        public ViewResult Content()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
            var tournaments = _tournamentRepo.GetTournaments();
            return View(tournaments);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View("TournamentForm",new Tournament());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var tournament = _tournamentRepo.GetTournamentById(id);
            return View("TournamentForm", tournament);
        }
        [HttpPost]
        public IActionResult Save(Tournament model)
        {

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // User not logged in or session expired
                return RedirectToAction("Login", "User");
            }

            Console.WriteLine("Tournammet start creating");
            Console.WriteLine(ModelState.IsValid);
            foreach (var modelStatu in ModelState.Values)
            {
                foreach (var error in modelStatu.Errors)
                {
                    Console.WriteLine("Error:" + error.ErrorMessage);
                }
            }

            Console.WriteLine(userId);
            if (ModelState.IsValid)
            {
                model.UserId = userId.Value;
                var tournament = new Tournament();

                if (model.Id == 0)
                {
                    tournament = _tournamentRepo.Add(model, userId);
                }
                else
                {
                    tournament = _tournamentRepo.Update(model);
                }
                return RedirectToAction("Content", "Team", new { id = tournament.Id });
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Console.WriteLine("Delete start");
            _tournamentRepo.Delete(id);
            return RedirectToAction("Index"); // Redirect back to the list or the same page
        }
    }
}
