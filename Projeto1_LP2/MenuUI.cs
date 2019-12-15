using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace Projeto1_LP2
{
    class MenuUI
    {
        public void ShowMemory()
        {
            Console.WriteLine("\t Occupying" +
                        ((Process.GetCurrentProcess().VirtualMemorySize64) / 1024 / 1024) +
                        "megabytes of memory");
        }

        public void ShowGenres(SearchFiles sf)
        {
            Console.Write($"\t => Known Genres (total {sf.allGenres.Count}): ");
            foreach (string genre in sf.allGenres.OrderBy(g => g))
                Console.Write($"{genre}");
            Console.WriteLine();
        }

        public void ShowSearchResults(Title[] queryResults, string searchText)
        {
            Console.WriteLine($"\t=> there are {queryResults.Count()} titles"
                + $"with {searchText}");
        }

    }
}
