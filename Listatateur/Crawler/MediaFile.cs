using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class MediaFile
    {
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AlbumArtist { get; set; }
        public string Album { get; set; }
        public string Genres { get; set; }

        public uint Disc { get; set; }
        public uint DiscCount { get; set; }
        public uint Bpm { get; set; }
        public uint Year { get; set; }
    }
}
