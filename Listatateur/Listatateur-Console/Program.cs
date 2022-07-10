using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Listatateur.Console;

class Program
{
    static readonly string Delimiter = "\t";

    static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole();
        });
        
        var files = CrawlFiles(args, loggerFactory);
        ExportFiles(files);
    }

    private static IEnumerable<Crawler.MediaFile> CrawlFiles(string[] args, ILoggerFactory loggerFactory)
    {
        var crawler = new Crawler.Crawler(loggerFactory.CreateLogger<Crawler.Crawler>());

        IEnumerable<Crawler.MediaFile> files;
        if (args.Length > 0)
        {
            files = crawler.ProcessPaths(args, true);
        }
        else
        {
            files = crawler.ProcessPaths(new string[] { Environment.CurrentDirectory }, true);
        }

        return files;
    }

    static void ExportFiles(IEnumerable<Crawler.MediaFile> files)
    {
        using var outFile = File.CreateText(string.Format("Listateur-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")));
        string[] header = { "Filename", "Title", "Artist", "Album artist", "Album", "Bpm", "Disc", "Disc count", "Year", "Genres" };
        outFile.WriteLine(string.Join(Delimiter, header));

        foreach (var file in files)
        {
            string[] line = { file.Filename, file.Title, file.Artist, file.AlbumArtist, file.Album, file.Bpm.ToString(), file.Disc.ToString(), file.DiscCount.ToString(), file.Year.ToString(), file.Genres };
            outFile.WriteLine(string.Join(Delimiter, line));
        }
    }

}
