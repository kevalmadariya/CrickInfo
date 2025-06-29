namespace crickinfo_mvc_ef_core.Models.Interfaces
{
    public interface ITeamRepo
    {
        Team GetTeamById(int id);
        IEnumerable<Team> GetTeamsByTournametId(int tournament_id);
        Team Add(Team team, int tournament_id);
        Team Update(Team team);
        string Delete(int id);
    }
}
