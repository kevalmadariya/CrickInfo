using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Algorithms;
using crickinfo_mvc_ef_core.Models.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class PredictController : Controller
    {
        public readonly ITournamentRepo _tournamentRepo;
        public readonly IPointsTableRepo _pointsTableRepo;
        public readonly ITeamRepo _teamsRepo;
        public readonly IMatchRepo _matchesRepo;
        public int tournament_id;

        public PredictController(ITournamentRepo tournamentRepo, IPointsTableRepo pointsTableRepo, ITeamRepo teamsRepo, IMatchRepo matchesRepo)
        {
            _tournamentRepo = tournamentRepo;
            _pointsTableRepo = pointsTableRepo;
            _teamsRepo = teamsRepo;
            _matchesRepo = matchesRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tournaments = _tournamentRepo.GetTournaments().ToList();
            return View(tournaments);
        }

        [HttpPost]
        public IActionResult RenderTeams(int tournamentId)
        {
            if (tournamentId == null)
            {
                var teams = _teamsRepo.GetTeamsByTournametId(tournament_id).ToList();
                ViewBag.TournamentId = tournamentId;  // Pass additional value via ViewBag
                return View(teams);
            }
            else
            {
                tournament_id = tournamentId;
                var teams = _teamsRepo.GetTeamsByTournametId(tournamentId).ToList();
                ViewBag.TournamentId = tournamentId;  // Pass additional value via ViewBag
                return View(teams);
            }
            //PointsTable pt = _pointsTableRepo.GetPointTableById(tournament_id);
            
        }

        [HttpPost]
        public IActionResult Predict(int teamId, int tournamentId)
        {
            Console.WriteLine(teamId + " " + tournamentId);
            Team t1 = _teamsRepo.GetTeamById(teamId);
            Console.WriteLine("teammmmmmmmmmmmmmmm"+t1.Name);
            //teamId = 2; 
            int ppm = 2;//points per match

            Pointstable pt1 = _pointsTableRepo.GetPointTableByTeamIdAndTournamentId(teamId, tournamentId);

            Console.WriteLine(pt1.Points);

            var mt1 = _matchesRepo.GetMatchByTournamentId(tournamentId).Where(m => m.Result == "Pending").ToList();//all remaining matches in tournament

            int teamspoints = 0;

            if (pt1 != null)
            {
                teamspoints = pt1.Points/ppm;
            }

            var pointstables = _pointsTableRepo.GetPointstablesByTournamentId(tournamentId).Include(p => p.Team).Where(p => p.TeamId != teamId).ToList();
            var matches = _matchesRepo.GetMatchByTournamentId(tournamentId).Where(m => m.TeamAId != teamId && m.TeamBId != teamId && m.Result == "Pending").ToList();
            var teams = _teamsRepo.GetTeamsByTournametId(tournamentId).Where(t => t.Id != teamId).ToList();

            Console.WriteLine(tournamentId);
            Console.WriteLine(" ddddddd ");
            Console.WriteLine("matches :" + matches.Count);
            Console.WriteLine("all matches :" + mt1.Count);
            int remMatchPoints = (matches.Count);//without selected team
            int maxPoints = remMatchPoints + teamspoints;
            Console.WriteLine("remMatchPoints" + remMatchPoints);
            Console.WriteLine("teamspoints" + teamspoints);
            Console.WriteLine("maxPoints :" + maxPoints);

            Console.WriteLine("teams with out select :" + teams.Count);

            //Console.WriteLine("teams with out select :" + teams.Count);
            foreach (var m in matches)
            {
                Console.WriteLine(m.TeamA.Name);
                Console.WriteLine(m.TeamB.Name);
                Console.WriteLine(" ddddddd ");
            }

            Graph crickMaxFlowGraph = new Graph(maxPoints, t1, teams, matches, pointstables);
            crickMaxFlowGraph.DisplayCapacityMatrix();

            var maxFlowAlgorithm = new MaxFlowAlgorithm(crickMaxFlowGraph.GetGraph());

            int maxFlow = maxFlowAlgorithm.MaxFlow(0, crickMaxFlowGraph.GetSinkIndex());

            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Console.WriteLine("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
            Console.WriteLine("sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss");
            Console.WriteLine(maxFlow);

            var graphMatrix = crickMaxFlowGraph.GetGraph();

            //int rows = graphMatrix.Count;
            //int cols = graphMatrix[0].Count;

            //int[,] matrixArray = new int[rows, cols];

            //for (int i = 0; i < rows; i++)
            //{
            //    for (int j = 0; j < cols; j++)
            //    {
            //        matrixArray[i][j] = graphMatrix[i][j];
            //    }
            //}

            // Convert the matrix to JSON
            //string serializedMatrix = JsonConvert.SerializeObject(graphMatrix);

            //// Pass the serialized matrix to the view via ViewBag
            //ViewBag.GraphMatrixJson = serializedMatrix;



            //ViewBag.GraphMatrix = crickMaxFlowGraph.GetGraph();


            if(remMatchPoints == 0)
            {
                ViewBag.Message = "Sorry Tournament is Complete now,no Changes occures!";
            }
            else if (maxFlow == remMatchPoints)
            {
                ViewBag.Message = "🎉 Yes,Team (" + t1.Name + ") Can reach the Top of the Table!";
                Console.WriteLine("Yes,it can be possible to acchive top position");
            }
            else
            {
                ViewBag.Message = "😞 No,Team (" + t1.Name + ") Can't reach the Top of the Table!";
                Console.WriteLine("Not possible at all!");
            }
            return View(pointstables);
        }
    }
}
