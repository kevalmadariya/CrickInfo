namespace crickinfo_mvc_ef_core.Models.Interfaces
{
    public interface ITournamentRepo
    {
        Tournament GetTournamentById(int id);
        IEnumerable<Tournament> GetTournaments();
        //IEnumerable<Tournament> GetPointsTables();
        //IEnumerable<Tournament> GetTournamentsByUserId();
        Tournament Add(Tournament tournament,int? user_id);
        Tournament Update(Tournament tournament);
        string Delete(int id);
    }
}
