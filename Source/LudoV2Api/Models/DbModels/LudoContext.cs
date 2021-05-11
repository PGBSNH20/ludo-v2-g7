using Microsoft.EntityFrameworkCore;
using System;

namespace LudoV2Api.Models.DbModels
{
    public class LudoContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PawnSavePoint> PawnSavePoints { get; set; }
        public DbSet<PlayerGame> GamePlayers { get; set; }
        public LudoContext(DbContextOptions<LudoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerGame>().HasKey(x => new { x.GameId, x.PlayerId });

            modelBuilder.Entity<Game>().HasKey(x => x.Id);
        }

    }
}
