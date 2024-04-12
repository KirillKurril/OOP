using Microsoft.AspNetCore.ResponseCompression;
using ServerDB.Repositories;
using ServerDB.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Network.Services.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                      new[] { "application/octet-stream" });
            });
            builder.Services.AddCors();
            builder.Services.AddSignalR();

            builder.Services.AddDbContext<RoomsDBContext>(opts =>
            {
                var connectionString = "Data Source=Rooms.db";
                opts.UseSqlite(connectionString);    
            });
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();



            var app = builder.Build();

            #region Использование сервиса SignalR
            app.UseResponseCompression();
            app.UseCors(opt => opt
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());
            app.UseRouting(); //////////////////

            app.MapHub<GameHub>("/game");
            #endregion Использование сервиса SignalR


            app.MapGet("/", () => "Hello World!");
            app.Run();

        }
    }
}
