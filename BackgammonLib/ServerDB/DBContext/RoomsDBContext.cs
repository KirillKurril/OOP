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
            Database.EnsureCreated();
        }
/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameBoard>()
                .HasMany(board => board.WhiteField)
                .WithOne(cell => cell.GameBoard)
                .HasForeignKey(cell => cell.GameBoardId);
        }*/
        public void SaveData()
        {
            SaveChanges();
        }
    }
}