using Microsoft.AspNetCore.ResponseCompression;

namespace Network.Services.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Настройка сервиса SignalR
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                      new[] { "application/octet-stream" });
            });
            builder.Services.AddCors();
            builder.Services.AddSignalR();
            #endregion Настройка сервиса SignalR


            var app = builder.Build();

            #region Использование сервиса SignalR
            app.UseResponseCompression();
            app.UseCors(opt => opt
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());

            app.MapHub<GameHub>("/chat");
            #endregion Использование сервиса SignalR


            app.MapGet("/", () => "Hello World!");
            app.Run();

        }
    }
}
