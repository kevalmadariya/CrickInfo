using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interfaces;
using crickinfo_mvc_ef_core.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchRepo _matchRepo;
        private readonly ITeamRepo _teamRepo;

        public MatchController(IMatchRepo matchRepo, ITeamRepo teamRepo)
        {
            _matchRepo = matchRepo;
            _teamRepo = teamRepo;
        }

        public IActionResult Content(int id,int tournamentId, int? matchId = null, bool showForm = false)
        {
            tournamentId = id;
            var matches = _matchRepo.GetMatchByTournamentId(tournamentId);
            var teams = _teamRepo.GetTeamsByTournametId(tournamentId);
            Console.WriteLine("Contetnt");
            Console.WriteLine(teams);
            Console.WriteLine(tournamentId);
            MatchFormModel formModel;

            if (matchId.HasValue && matchId.Value != 0)
            {
                var match = _matchRepo.GetMatchById(matchId.Value);
                if (match == null)
                    return NotFound();

                formModel = new MatchFormModel
                {
                    Id = match.Id,
                    SelectedTeamIds = new List<int> { match.TeamAId, match.TeamBId },
                    MatchNo = match.MatchNo,
                    MatchDate = match.MatchDate,
                    Result = match.Result
                };
            }
            else
            {
                formModel = new MatchFormModel
                {
                    MatchDate = DateTime.Today
                };
            }

            var viewModel = new MatchContentViewModel
            {
                TournamentId = tournamentId,
                Matches = (List<Match>)matches,
                Teams = (List<Team>)teams,
                FormModel = formModel,
                ShowForm = showForm
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Save(MatchContentViewModel model)
        {
            if (model.FormModel.SelectedTeamIds.Count != 2)
            {
                ModelState.AddModelError("", "Please select exactly two teams.");
            }

            if (ModelState.IsValid)
            {
                if (model.FormModel.Id == 0)
                {
                    // Add new match
                    var match = new Match
                    {
                        TeamAId = model.FormModel.SelectedTeamIds[0],
                        TeamBId = model.FormModel.SelectedTeamIds[1],
                        MatchNo = model.FormModel.MatchNo,
                        MatchDate = model.FormModel.MatchDate,
                        Result = model.FormModel.Result,
                        TournamentId = model.TournamentId
                    };

                    _matchRepo.Add(match,model.TournamentId);
                }
                else
                {
                    // Update existing match
                    var match = _matchRepo.GetMatchById(model.FormModel.Id);
                    if (match == null)
                        return NotFound();

                    match.TeamAId = model.FormModel.SelectedTeamIds[0];
                    match.TeamBId = model.FormModel.SelectedTeamIds[1];
                    match.MatchNo = model.FormModel.MatchNo;
                    match.MatchDate = model.FormModel.MatchDate;
                    match.Result = model.FormModel.Result;

                    _matchRepo.Update(match);
                }

                return RedirectToAction("Content","Match", new { id = model.TournamentId , showForm = true });
            }

            // Reload page with errors
            model.Teams = (List<Team>?)_teamRepo.GetTeamsByTournametId(model.TournamentId);
            model.Matches = (List<Match>?)_matchRepo.GetMatchByTournamentId(model.TournamentId);
            model.ShowForm = true;

            return View("Content", model);
        }

        [HttpPost]
        public IActionResult Delete(int id, int tournamentId)
        {
            _matchRepo.Delete(id);
            return RedirectToAction("Content", new { tournamentId = tournamentId });
        }
    }
}
