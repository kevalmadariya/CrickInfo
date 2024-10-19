using Microsoft.AspNetCore.Mvc;
using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interface;
namespace crickinfo_mvc_ef_core.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserRepo _userRepo;

		public UserController(IUserRepo userRepo)
		{
			_userRepo = userRepo;
		}

		public ViewResult Index()
		{
			var model = _userRepo.GetUsers().FirstOrDefault();
			return View(model);
		}

		[HttpGet]
		public ViewResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(User model)
		{
            ViewBag.Message += "Tournament 1 added successfully";

            if (ModelState.IsValid)
			{
				User user = new User
				{
					Name = model.Name,
					Email = model.Email,
					Password = model.Password,
					IsAdmin = false
				};
				_userRepo.Add(user);
                
				HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Name);
                
				return RedirectToAction("details", new { id = user.Id });
			}
			return View();
		}

		public ViewResult Details(int id)
		{
			User model = _userRepo.GetUserById(id);
			if (model==null)
			{
				Response.StatusCode = 404;
				return View("User Not Found",id);
			}
			return View();
		}


	}
}
