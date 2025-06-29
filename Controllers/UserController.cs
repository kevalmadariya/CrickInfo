using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.DTO;
using crickinfo_mvc_ef_core.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;
        public UserController(IUserRepo userRepo) { 
             _userRepo = userRepo;
        }

        //public ViewResult Index()
        //{
        //    var model = _userRepo.GetUsers().FirstOrDefault();
        //    return View(model);
        //}

        public IActionResult Details(int id)
        {
            User user = _userRepo.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User Not found! Id: {id}");
            }
            return View(user);
        }


        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User model)
        {
            Console.WriteLine("user creation start");
            Console.WriteLine(model);
            Console.WriteLine(ModelState.IsValid);

            //usefull in debug
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            try
            {
                if (ModelState.IsValid)
                {

                    //User user = new User(model);
                    _userRepo.Add(model);

                    HttpContext.Session.SetInt32("UserId", model.Id);
                    HttpContext.Session.SetString("Username", model.Name);
                    Console.WriteLine("user creation end");

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while adding user:" + e);
            }

            return View();
        }


        [HttpGet]
        public ViewResult Login()
        {
            Console.WriteLine("login page");
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                User u = _userRepo.GetUserByEmailId(model.Email.ToString());
                if (u != null)
                {
                    if (u.Password == model.Password)
                    {
                        HttpContext.Session.SetInt32("UserId", u.Id);
                        HttpContext.Session.SetString("Username", u.Name);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            return View();
        }

    }
}
