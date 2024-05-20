using Entities.GameServices;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ServerDB.Models;
using System.Reflection.Metadata;

namespace ServerDB.DBContext
{
    public class RoomsDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<NetGame> NetGames { get; set; }
        public DbSet<Cell> Cell { get; set; }
        public DbSet<GameBoard> GameBoards { get; set; }
        public DbSet<Player> Players { get; set; }

        public RoomsDBContext(DbContextOptions options) : base(options) 
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .HasOne(r => r.NetGame)
                .WithOne(ng => ng.Room)
                .HasForeignKey<NetGame>(ng => ng.RoomId);

            modelBuilder.Entity<NetGame>()
                .HasOne(ng => ng.Board)
                .WithOne(b => b.NetGame)
                .HasForeignKey<GameBoard>(b => b.NetGameId);

            modelBuilder.Entity<NetGame>()
                .HasMany(ng => ng.Players)
                .WithOne(p => p.NetGame)
                .HasForeignKey(p => p.NetGameId);

        }
        public void SaveData()
        {
            SaveChanges();
        }
    }
}