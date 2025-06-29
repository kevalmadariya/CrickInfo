namespace crickinfo_mvc_ef_core.Models.Interfaces
{
    public interface IPointsTableRepo
    {
        Pointstable GetPointsTableById(int id);
        IEnumerable<Pointstable> GetPointsTables();
        IQueryable<Pointstable> GetPointstablesByTournamentId(int tournament_id);
        IEnumerable<Pointstable> GetPointstablesByTournamentId2(int tournament_id);
        Pointstable Add(Pointstable pointstable,int tournament_id);
        Pointstable Update(Pointstable pointstable);
        Pointstable GetPointTableByTeamIdAndTournamentId(int team_id,int tournament_id);
        string Delete(int id);
    }
}
