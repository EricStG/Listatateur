using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Crawler
{
    public class Crawler
    {
        public Crawler(ILogger<Crawler> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<Crawler> _logger;

        private static readonly HashSet<string> _extensions = new HashSet<string>(new[] { ".mp3", ".aac", ".ogg" }, StringComparer.OrdinalIgnoreCase); // TODO: make this configurable?

        private MediaFile ProcessFile(string filename)
        {
            try
            {
                if (_extensions.Contains(Path.GetExtension(filename)))
                {
                    using (var file = TagLib.File.Create(filename, TagLib.ReadStyle.Average))
                    {
                        return new MediaFile
                        {
                            Filename = filename,
                            Title = file.Tag.Title,
                            Artist = string.Join(",", file.Tag.Performers),
                            AlbumArtist = string.Join(",", file.Tag.AlbumArtists),
                            Album = file.Tag.Album,
                            Bpm = file.Tag.BeatsPerMinute,
                            Disc = file.Tag.Disc,
                            DiscCount = file.Tag.DiscCount,
                            Year = file.Tag.Year,
                            Genres = string.Join(",", file.Tag.Genres),
                        };
                    }
                }
                else
                {
                    _logger.LogDebug("File '{Filename}' does not match any of the configured extensions. Skipping.", filename);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not read tag for file '{Filename}'", filename);
                return null;
            }
        }

        private IEnumerable<MediaFile> ProcessFolder(string folder, bool recursive)
        {
            SearchOption options;
            if (recursive)
            {
                options = SearchOption.AllDirectories;
            }
            else
            {
                options = SearchOption.TopDirectoryOnly;
            }
            foreach(var filename in Directory.EnumerateFiles(folder, "*", options))
            {
                var file = ProcessFile(filename);
                if (file != null)
                {
                    yield return file;
                }
            }
        }

        public IEnumerable<MediaFile> ProcessPaths(IEnumerable<string> paths, bool recursive)
        {            
            foreach(var path in paths)
            {
                var attribs = File.GetAttributes(path);
                if (attribs.HasFlag(FileAttributes.Directory))
                {
                    foreach (var media in ProcessFolder(path, recursive))
                    {
                        yield return media;
                    }
                }
                else
                {
                    yield return ProcessFile(path);
                }
            }
        }
    }
}
