using crickinfo_mvc_ef_core.Models.Interfaces;

namespace crickinfo_mvc_ef_core.Models.Repository
{
    public class PointstableRepository : IPointsTableRepo
    {
        private readonly CrickDbContext _context;

        public PointstableRepository(CrickDbContext crickDbContext)
        {
            _context = crickDbContext;
        }

        public Pointstable Add(Pointstable  pointstable, int tournamet_id)
        {
            Console.WriteLine("start Adding");
            _context.Pointstables.Add(pointstable);
            _context.SaveChanges(); 
            return pointstable;
        }

        public string Delete(int id)
        {
            Pointstable  u = _context.Pointstables.Find(id);
            if (u != null)
            {
                _context.Pointstables.Remove(u);
                _context.SaveChanges();
            }
            return "pointstable deleted successfully";
        }

        public IEnumerable<Pointstable> GetPointsTables()
        {
            return _context.Pointstables;
        }

        public Pointstable GetPointsTableById(int pointstable_id)
        {
            return _context.Pointstables.Find(pointstable_id);
        }

        public IQueryable<Pointstable> GetPointstablesByTournamentId(int tournament_id)
        {
            return _context.Pointstables
                .Where(tt => tt.TournamentId == tournament_id);
        }

        public IEnumerable<Pointstable> GetPointstablesByTournamentId2(int tournament_id)
        {
            return _context.Pointstables
                .Where(tt => tt.TournamentId == tournament_id);
        }
        public Pointstable GetPointTableByTeamIdAndTournamentId(int team_id,int tournament_id)
        {
            return _context.Pointstables.Where(pt => pt.TournamentId == tournament_id && pt.TeamId == team_id).FirstOrDefault();
        }
        
        public Pointstable  Update(Pointstable pointstable)
        {
            var u = _context.Pointstables.Attach(pointstable);
            u.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return pointstable;
        }

    }
}
