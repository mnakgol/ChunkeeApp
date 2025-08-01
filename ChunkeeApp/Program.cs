using ChunkeeApp;
using ChunkeeApp.Data.Database;
using ChunkeeApp.Interfaces;
using ChunkeeApp.Models;
using ChunkeeApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

class Program
{
    static async Task Main(string[] args)
    {
       
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();

        
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        Console.WriteLine("Çalışma dizini: " + Environment.CurrentDirectory);


        var serviceProvider = serviceCollection.BuildServiceProvider();

        
        var app = serviceProvider.GetRequiredService<App>();
        await app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        string basePath = AppContext.BaseDirectory;
        string dbPath = Path.Combine(basePath, "..", "..", "..", "chunkee.db");
        dbPath = Path.GetFullPath(dbPath);
        services.AddDbContext<ChunkeeDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });

       
        services.AddTransient<App>();
        services.AddScoped<IFileChunker, FileChunker>();
        services.AddTransient<IFileMerger, FileMerger>();
        services.AddScoped<IRepository<Chunk>, GenericRepository<Chunk>>();
        services.AddScoped<IRepository<OriginalFile>, GenericRepository<OriginalFile>>();
        services.AddScoped<IChunkService, ChunkService>();
        services.AddScoped<IOriginalFileService, OriginalFileService>();
        services.AddScoped<IChunkeeDbContext, ChunkeeDbContext>();


    }
}