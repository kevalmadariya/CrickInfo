using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.AspNetCore.Mvc;
namespace crickinfo_mvc_ef_core.Controllers
{
    public class PointsTableController : Controller
    {
        private readonly IPointsTableRepo _pointtablerepo;

        public PointsTableController(IPointsTableRepo pointtablerepo)
        {
            _pointtablerepo = pointtablerepo;
        }

        [HttpGet]
        public IActionResult Create(PointsTable model)
        {
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {

            return View();
        }
    }
}
