namespace crickinfo_mvc_ef_core.Models.Interfaces
{
    public interface IMatchRepo
    {
        Match GetMatchById(int id);
        IEnumerable<Match> GetMatchByTournamentId(int tournament_id);
        Match Add(Match match,int tournament_id);
        Match Update(Match match);
        string Delete(int id);
    }
}
