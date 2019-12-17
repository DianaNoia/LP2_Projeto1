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
        private static SearchFiles sf = new SearchFiles();
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
            Console.Write($"=> Known Genres (28): \n");
            foreach (GenresEnum genre in Enum.GetValues(typeof(GenresEnum)))
                Console.Write($"\t{genre}\n");
            Console.WriteLine();
        }

        public void ShowSearchResults(Title[] queryResults, string searchText)
        {
            int numTitlesShown = 0;

            Console.WriteLine();
            Console.WriteLine($"=> There are {queryResults.Count()} titles"
                + $" with {searchText}.");

            while (numTitlesShown < queryResults.Length)
            {
                Console.WriteLine($"<=> Press the arrow keys to see the next " +
                    $"{numTitlesToShowOnScreen} titles");
                Console.WriteLine();

                for (int i = numTitlesShown;
                    i < numTitlesShown + numTitlesToShowOnScreen &&
                    i < queryResults.Length; i++)
                {
                    bool firstGenre = true;

                    Title title = queryResults[i];

                    Console.Write("\t\t* ");
                    Console.Write($"\"{title.PrimaryTitle}\" ");
                    Console.Write(
                        $"({title.StartYear?.ToString() ?? "unknown year"}): ");

                    foreach (string genre in title.Genres)
                    {
                        if (!firstGenre) Console.Write("/");
                        Console.Write($"{genre} ");
                        firstGenre = false;
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("<=Press the backspace key to go back to" +
                    " the main menu");

                ConsoleKeyInfo pressedKey;
                pressedKey = Console.ReadKey();

                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    Console.Clear();
                    numTitlesShown -= numTitlesToShowOnScreen;
                    if (numTitlesShown < queryResults.Length)
                        numTitlesShown = numTitlesToShowOnScreen;
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    Console.Clear();
                    numTitlesShown += numTitlesToShowOnScreen;
                }
                else if (pressedKey.Key == ConsoleKey.Backspace)
                {
                    Console.Clear();
                    ShowMenu();
                }
                else
                {
                    Console.WriteLine("Input not correct, try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        public void ShowMenu()
        {
            Console.WriteLine("Welcome to our IMDb Searcher!");
            Console.WriteLine();
            Console.WriteLine("1. Search title");
            Console.WriteLine("2. Credits");
            Console.WriteLine("3. Quit");
            Console.WriteLine();
            Console.WriteLine("Write the correspondent number to proceed.\n");

            ReadEntry();
        }

        public void ShowCredits()
        {
            Console.WriteLine("This Searcher was made by:");
            Console.WriteLine();
            Console.WriteLine("André Pedro      a21701115");
            Console.WriteLine("Diana Nóia       a21703004");
            Console.WriteLine("Inês Gonçalves   a21702076");
            Console.WriteLine();
            Console.WriteLine("Special thanks to our teacher Nuno Fachada!");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to the main menu\n");
            Console.ReadKey();
            Console.Clear();
            ShowMenu();
        }

        public void ReadEntry()
        {
            // A loop that will run while the condition is true, and while the
            // player doesn't choose to leave
            while (true)
            {
                // A switch case that will work based on the player's input
                switch (Console.ReadLine())
                {
                    // If the player chooses 1
                    case "1":
                        Console.Clear();
                        string inputSupport;
                        Title[] queryResults = sf.FileSearch(out inputSupport);
                        ShowSearchResults(queryResults, inputSupport);
                        break; ;

                    // If the player chooses 2
                    case "2":
                        Console.Clear();
                        ShowCredits();
                        break;

                    // If the player chooses 3
                    case "3":
                        Console.Clear();
                        // Will break the loop and exit the program
                        Environment.Exit(0);
                        break;

                    // A default message in case the player chooses an
                    // unavailable number
                    default:
                        // Will print an error message, and ask them to type a
                        // number again
                        Console.WriteLine("Not a valid number. Try again.");
                        // Reads the input
                        Console.ReadLine();
                        // Will clear the console
                        Console.Clear();
                        // Will call the menu
                        ShowMenu();
                        break;
                }
            }
        }
    }
}
