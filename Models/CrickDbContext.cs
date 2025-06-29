using Microsoft.EntityFrameworkCore;

namespace crickinfo_mvc_ef_core.Models
{
    public class CrickDbContext : DbContext
    {
        public CrickDbContext(DbContextOptions<CrickDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamTournament> TeamsTournaments { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Pointstable> Pointstables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>()
                .HasIndex(tt => new { tt.Name, tt.DateOfTournament });

            modelBuilder.Entity<TeamTournament>()
                .HasOne(tt => tt.Team)
                .WithMany(t => t.TeamTournamets)
                .HasForeignKey(tt => tt.TeamId);

            modelBuilder.Entity<TeamTournament>()
                .HasOne(tt => tt.Tournament)
                .WithMany(t => t.TeamTournamets)
                .HasForeignKey(tt => tt.TournamentId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany()
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamA)
                .WithMany()
                .HasForeignKey(m => m.TeamAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamB)
                .WithMany()
                .HasForeignKey(m => m.TeamBId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}