using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using crickinfo_mvc_ef_core.Models;

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
		public ViewResult Create()
		{
			return View();
		}

        //[HttpPost]
        //      public IActionResult Create(Tournament model)
        //      {
        //          ViewBag.Message += "invalid 1";
        //          ViewBag.outer += "outer";
        //          if (ModelState.IsValid)
        //          {
        //              ViewBag.Message += "Tournament 1 added successfully";
        //              ViewBag.inner += "inner";
        //              ViewBag.ModelStateValid = true;

        //              //var uId = HttpContext.Session.GetInt32("UserId");
        //              var uId = 1;
        //              Console.WriteLine("Create method called"); // This will log to the server-side console.
        //              ViewBag.Message += "Tournament 2 added successfully";

        //              Tournament tournament = new Tournament
        //              {
        //                  Name = model.Name,
        //                  Description = model.Description,
        //                  DateOfTournament = model.DateOfTournament,
        //                  Status = model.Status,
        //                  UserId = (int)uId,
        //                  User = new User()
        //              };


        //              _tournamentRepo.Add(tournament,(int)uId);
        //              return RedirectToAction("details", new { id = tournament.Id });

        //          }
        //          return View();
        //      }
        [HttpPost]
        public IActionResult Create(Tournament model)
        {
            ViewBag.outer = "outer";  // Just for testing purpose
            var uId = 1;  // Sample UserId, you can replace it with session logic if needed

            model.UserId = 1;
            model.User = new User((int)uId);
            model.Teams = new List<Team>();
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Tournament added successfully!";
                ViewBag.ModelStateValid = true;


                // Create a new Tournament object
                Tournament tournament = new Tournament
                {
                    Name = model.Name,
                    Description = model.Description,
                    DateOfTournament = model.DateOfTournament,
                    Status = model.Status,
                    UserId = uId,
                    User = new User((int)uId)  // Assuming you have a related user entity
                };

                // Add the tournament to the repository
                _tournamentRepo.Add(tournament, uId);

                // Instead of redirecting, stay on the same page and display the success message
                // If you still want to redirect after success, uncomment the below line
                 return RedirectToAction("Details", new { id = tournament.Id });

                // Return the same view and preserve form data
                //return View(model);
            }

            // If model is invalid, show validation errors and return the view with the same data
            var errorMessages = ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage)
          .ToList();

            // Join the error messages into a single string for display
            ViewBag.ErrorMessages = string.Join("<br/>", errorMessages);
            //ViewBag.ModelStateValid = false;
            //ViewBag.Message = $"There are {ModelState.ErrorCount} errors in the form. Please correct them.";

            ViewBag.ModelStateValid = false;
            //ModelState.AddModelError
            ViewBag.Message = "There are errors in the form. Please correct them."+ ModelState.ErrorCount.ToString()+" Errror" ;
           
            return View();
        }

        public ViewResult Details(int id)
        {
            Tournament model = _tournamentRepo.GetTournamentById(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                return View("Tournament Not Found", id);
            }
            return View(model);
        }
    }
}
