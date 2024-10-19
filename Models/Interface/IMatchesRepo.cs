namespace crickinfo_mvc_ef_core.Models.Interface
{
	public interface IMatchesRepo
	{
		Matches GetMatchesById(int id);
		Matches Add(Matches matches);
		IEnumerable<Matches> GetAllMatches();
		Matches Update(Matches matches);
		Matches Delete(int id);
	}
}
