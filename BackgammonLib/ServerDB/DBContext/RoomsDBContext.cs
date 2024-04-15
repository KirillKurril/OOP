using Entities.GameServices;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ServerDB.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Room>()
                .HasOne(room => room.Game)
                .WithOne(netGame => netGame.Room)
                .HasForeignKey<NetGame>(netGame => netGame.RoomId)
                .IsRequired();

            modelBuilder.Entity<NetGame>()
                .HasOne(game => game.Board)
                .WithOne(board => board.NetGame)
                .HasForeignKey<GameBoard>(board => board.NetGameId)
                .IsRequired();

            modelBuilder.Entity<NetGame>()
                .HasMany(game => game.Players)
                .WithOne(player => player.NetGame)
                .HasForeignKey(player => player.NetGameId)
                .IsRequired();

            modelBuilder.Entity<GameBoard>()
                .HasMany(board => board.WhiteField)
                .WithOne(cell => cell.WhiteField)
                .IsRequired();

            modelBuilder.Entity<GameBoard>()
                .HasMany(board => board.BlackField)
                .WithOne(cell => cell.BlackField)
                .IsRequired();

            modelBuilder.Entity<NetGame>()
                .HasMany(game => game.СurField)
                .WithOne(cell => cell.NetGame)
                .HasForeignKey(cell => cell.NetGameId)
                .IsRequired();
        }
        public void SaveData()
        {
            SaveChanges();
        }
    }
}