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
        //SearchFiles sf = new SearchFiles();
        private const int numTitlesToShowOnScreen = 10;

        public void ShowMemory()
        {
            Console.WriteLine("\t Occupying " +
                        ((Process.GetCurrentProcess().VirtualMemorySize64) / 1024 / 1024) +
                        " megabytes of memory.");
            Console.WriteLine();
        }

        public void ShowGenres(SearchFiles sf)
        {
            Console.Write($"\t => Known Genres (total {sf.allGenres.Count}): ");
            foreach (string genre in sf.allGenres.OrderBy(g => g))
                Console.Write($"{genre}");
            Console.WriteLine();
        }

        public void ShowSearchResults(Title[] queryResults)
        {
            int numTitlesShown = 0;

            //Console.WriteLine("Insert search term");
            //searchText = Console.ReadLine();
            //sf.FileSearch(searchText);

            while (numTitlesShown < queryResults.Length)
            {
                Console.WriteLine($"\t => Press key to see next " +
                    $"{numTitlesToShowOnScreen} titles");

                for (int i = numTitlesShown; 
                    i < numTitlesShown + numTitlesToShowOnScreen && 
                    i < queryResults.Length; i++)
                {
                    bool firstGenre = true;

                    Title title = queryResults[i];

                    Console.Write("\t\t* ");
                    Console.Write($"\"{title.PrimaryTitle}\" ");
                    Console.Write($"({title.StartYear?.ToString() ?? "unknown year"}): ");

                    foreach(string genre in title.Genres)
                    {
                        if (!firstGenre) Console.Write("/");
                        Console.Write($"{genre} ");
                        firstGenre = false;
                    }
                    Console.WriteLine();
                }

                Console.ReadKey(true);
                Console.Clear();
                ShowMemory();
                numTitlesShown += numTitlesToShowOnScreen;
            }
        }

    }
}
