using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Listatateur_Console
{
    class Program
    {
        static readonly string Delimiter = "\t";

        static void Main(string[] args)
        {
            IEnumerable<Crawler.MediaFile> files;
            if (args.Length > 0)
            { 
                files = Crawler.Crawler.ProcessPaths(args, true);
            }
            else
            {
                files = Crawler.Crawler.ProcessPaths(new string[] { Environment.CurrentDirectory }, true);
            }

            ExportFiles(files);
        }

        static void ExportFiles(IEnumerable<Crawler.MediaFile> files)
        {
            if (files.Count() > 0)
            {
                using (var outFile = File.CreateText(string.Format("Listateur-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"))))
                {
                    string[] header = {"Filename", "Title", "Artist", "Album artist", "Album", "Bpm", "Disc", "Disc count", "Year", "Genres"};
                    outFile.WriteLine(string.Join(Delimiter, header));

                    foreach (var file in files)
                    {
                        string[] line = { file.Filename, file.Title, file.Artist, file.AlbumArtist, file.Album, file.Bpm.ToString(), file.Disc.ToString(), file.DiscCount.ToString(), file.Year.ToString(), file.Genres };
                        outFile.WriteLine(string.Join(Delimiter, line));
                    }
                }
            }
            else
            {

            }
        }

    }
}
