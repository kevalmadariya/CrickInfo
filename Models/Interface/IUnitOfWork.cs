namespace crickinfo_mvc_ef_core.Models.Interface
{
	public interface IUnitOfWork
	{
		ITournamentRepo Tournament {  get; }
		ITeamsRepo Team { get; }
		//TeamTournament Add(TeamTournament teamTournament);
		void Save();
	}
}
