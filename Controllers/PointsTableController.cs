using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interfaces;
using crickinfo_mvc_ef_core.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class PointsTableController : Controller
    {
        private readonly IPointsTableRepo _pointsTableRepo;
        private readonly ITeamRepo _teamRepo;
        private readonly IMatchRepo _matchRepo;
        public PointsTableController(IPointsTableRepo pointsTableRepo, ITeamRepo teamRepo, IMatchRepo matchRepo)
        {
            _pointsTableRepo = pointsTableRepo;
            _teamRepo = teamRepo;             
            _matchRepo = matchRepo;
        }

        public IActionResult Content(int id,int tournamentId, bool showForm = false)
        {
            tournamentId = id;

            Console.WriteLine("Match start Contentjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj         ("+tournamentId+"    )jjjjjjjjjjjjj");

            tournamentId = id;
            IEnumerable<Pointstable> pointsTable = _pointsTableRepo.GetPointstablesByTournamentId2(id);

            int c = 0;
            foreach(Pointstable p in pointsTable)
            {
                c++;
                Console.WriteLine("jdfadfj" + p.Points);
            }

            Console.WriteLine("PPOinsdasjkdh tabe " + pointsTable + "///////////////////////////////////////////////////   "+c+"     ///////////////");
            IEnumerable<Team> teams = _teamRepo.GetTeamsByTournametId(tournamentId);
            IEnumerable<Match> matchs = _matchRepo.GetMatchByTournamentId(tournamentId);


            Console.WriteLine("Match start Content jkasdfjaksdfkdsnckmdasknc");
            //Console.WriteLine("Match start Content pointtable" + pointsTable["Points"]);

                Console.WriteLine("ppppppppppppppppppppppppppppppppp");

                Console.WriteLine("pppppppppppppppppppppppppppppppppdjsahdjhashddddddddddddddddddddddddddddddddddddddddddd");

            foreach(Match m in matchs)
            {

                if (m.Result != "Pending")
                {
                    Pointstable existingPointsTableA = _pointsTableRepo.GetPointTableByTeamIdAndTournamentId(m.TeamAId, tournamentId);
                    Pointstable existingPointsTableB = _pointsTableRepo.GetPointTableByTeamIdAndTournamentId(m.TeamBId, tournamentId);

                    existingPointsTableA.Wins = 0;
                    existingPointsTableA.Points = 0;
                    existingPointsTableB.Loss = 0;
                    existingPointsTableB.Wins = 0;
                    existingPointsTableB.Points = 0;
                    existingPointsTableA.Loss = 0;


                    _pointsTableRepo.Update(existingPointsTableA);
                    _pointsTableRepo.Update(existingPointsTableB);
                }
            }

            foreach (Match m in matchs)
            {
                Console.WriteLine("Match" + m.Result);
                //get matchs over here
                //check entry with these team alredy exist
                var entryForTeamAinPointtable = _pointsTableRepo.GetPointTableByTeamIdAndTournamentId(m.TeamAId, tournamentId);
                var entryForTeamBinPointtable = _pointsTableRepo.GetPointTableByTeamIdAndTournamentId(m.TeamBId, tournamentId);

                //if not then add _pointtable entry 

                if (entryForTeamAinPointtable == null)
                {
                    _pointsTableRepo.Add(new Pointstable
                    {
                        TournamentId = tournamentId,
                        TeamId = m.TeamAId
                    }, tournamentId);
                }
                if (entryForTeamBinPointtable == null)
                {
                    _pointsTableRepo.Add(new Pointstable
                    {
                        TournamentId = tournamentId,
                        TeamId = m.TeamBId
                    }, tournamentId);
                }

                if (m.Result != "Pending")
                {
                    Pointstable existingPointsTableA = _pointsTableRepo.GetPointTableByTeamIdAndTournamentId(m.TeamAId, tournamentId);
                    Pointstable existingPointsTableB = _pointsTableRepo.GetPointTableByTeamIdAndTournamentId(m.TeamBId, tournamentId);


                    if (m.Result == m.TeamA.Name)
                    {
                        existingPointsTableA.Wins += 1;
                        existingPointsTableA.Points += 2;
                        existingPointsTableB.Loss += 1;
                    }
                    else
                    {
                        existingPointsTableB.Wins += 1;
                        existingPointsTableB.Points += 2;
                        existingPointsTableA.Loss += 1;
                    }
                    _pointsTableRepo.Update(existingPointsTableA);
                    _pointsTableRepo.Update(existingPointsTableB);
                }
            
                    //update later..
            }
            
            var model = new PointsTableContentViewModel
            {
                TournamentId = tournamentId,
                Teams = teams,
                PointsTableEntries = pointsTable,
                ShowForm = showForm
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(PointsTableContentViewModel model)
        {
            if (model == null || model.PointsTableEntries == null)
            {
                return BadRequest("Invalid data submitted.");
            }

            var tournamentId = model.TournamentId;

            Console.WriteLine("start Save");
            Console.WriteLine("start Save");

            foreach(var formstatus in ModelState.Values)
            {
                foreach(var error in formstatus.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                foreach (var entry in model.PointsTableEntries)
                {
                    if (entry.Id == 0)
                    {
                        Console.WriteLine("start Adding 2");

                        _pointsTableRepo.Add(entry,model.TournamentId);
                    }
                    else
                    {
                        _pointsTableRepo.Update(entry);
                    }
                }

                return RedirectToAction("Content","PointsTable", new { id = model.TournamentId });
            }

            // Reload page if invalid
            model.Teams = _teamRepo.GetTeamsByTournametId(model.TournamentId);
            model.ShowForm = true;
            return View("Content", model);
        }
    }
}
