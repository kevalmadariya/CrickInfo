using crickinfo_mvc_ef_core.Models.Interface;
using System.Runtime.CompilerServices;

namespace crickinfo_mvc_ef_core.Models.SQL
{
    public class SQLUnitOfWorkRepo : IUnitOfWork
    {
        private readonly CrickInfoContext _context;
        private ITournamentRepo _tournamentRepo;
        private ITeamsRepo _teamsRepo;
        public SQLUnitOfWorkRepo(CrickInfoContext context) {
                    _context = context;
        }

        public ITournamentRepo Tournament => throw new NotImplementedException();

        //public ITeamsRepo Team => throw new NotImplementedException();
        public ITeamsRepo Team
        {
            get
            {
                return _teamsRepo = _teamsRepo ?? new SQLTeamsRepo(_context);
            }
        }

        //TeamTournament IUnitOfWork.Add(TeamTournament teamTournament)
        //{
        //    _context.TeamTournaments.Add(teamTournament);
        //    _context.SaveChanges();
        //    return teamTournament;
        //}

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
