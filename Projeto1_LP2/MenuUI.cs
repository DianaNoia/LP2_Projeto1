using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace Projeto1_LP2
{
    /// <summary>
    /// Class for the menu and UI
    /// </summary>
    class MenuUI
    {
        private static SearchFiles sf = new SearchFiles();
        private const int numTitlesToShowOnScreen = 10;

        /// <summary>
        /// Method that shows the current value of memory the program is occupying
        /// </summary>
        public void ShowMemory()
        {
            Console.WriteLine("\t Occupying " +
                        ((Process
                        .GetCurrentProcess()
                        .WorkingSet64) / (1024 * 1024)) +
                        " megabytes of memory.");
            Console.WriteLine();
        }

        /// <summary>
        /// Void method that shows all the genres available
        /// </summary>
        /// <param name="sf"></param>
        public void ShowGenres(SearchFiles sf)
        {
            Console.Write($"=> Known Genres ({sf.allGenres.Count}): \n");
            foreach (string genre in sf.allGenres)
                Console.Write($"\t{genre}\n");
            Console.WriteLine();
        }

        /// <summary>
        /// Method that shows all the results that have the searched keyword
        /// </summary>
        /// <param name="queryResults"></param>
        /// <param name="searchText"></param>
        public void ShowSearchResults(Title[] queryResults, string searchText)
        {
            int numTitlesShown = 0;
                       
            Console.Clear();
            
            //Show all reuslts found
            if (searchText != null)
                Console.WriteLine($"=> There are {queryResults.Count()} titles"
                    + $" with {searchText}.");

            //Navigate between the results, showing the next or previous 10
            while (numTitlesShown < queryResults.Length)
            {
                Console.WriteLine(
                    $"\nPrevious {numTitlesToShowOnScreen} " +
                    $"titles <= [Arrow Keys] => " +
                    $"Next {numTitlesToShowOnScreen} titles");
                Console.WriteLine();

                //Navigate the results and print them in groups of 10
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

                    // Shows the genres that pertains to the current tilte
                    foreach (string genre in title.Genres)
                    {
                        if (!firstGenre) Console.Write(" / ");
                        Console.Write($"{genre}");
                        firstGenre = false;
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("<=Press the backspace key to go back to" +
                    " the main menu");
                Console.WriteLine("<=Press the space key to" +
                    " order results");

                ConsoleKeyInfo pressedKey;
                pressedKey = Console.ReadKey();

                //If left arrow key is pressed shows previous 10 results
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    Console.Clear();
                    numTitlesShown -= numTitlesToShowOnScreen;

                    if (numTitlesShown < 0)
                        numTitlesShown = 0;

                    Console.WriteLine(
                        $"Page: {numTitlesShown / numTitlesToShowOnScreen} " +
                        $"of {queryResults.Length / numTitlesToShowOnScreen}");
                }
                //If right arrow key is pressed shows next 10 results
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    Console.Clear();
                    numTitlesShown += numTitlesToShowOnScreen;

                    if (numTitlesShown > queryResults.Length)
                        numTitlesShown -= numTitlesToShowOnScreen;

                    Console.WriteLine(
                        $"Page: {numTitlesShown / numTitlesToShowOnScreen} " +
                        $"of {queryResults.Length / numTitlesToShowOnScreen}");
                }
                //If backspace key is pressed goes back to main menu
                else if (pressedKey.Key == ConsoleKey.Backspace)
                {
                    Console.Clear();
                    ShowMenu();
                }
                //If spacebar key is pressed allows to specific search terms
                else if (pressedKey.Key == ConsoleKey.Spacebar)
                {
                    Console.Clear();
                    Console.WriteLine("\nYou can order by name, type, " +
                        "adult, startYear, endYear and genre...");
                    Console.Write("Order by: ");

                    string temporary = Console.ReadLine();

                    if (temporary == "name")
                    {
                        ShowSearchResults(
                            sf.OrderByName(queryResults), null);
                    }
                    if (temporary == "type")
                    {
                        ShowSearchResults(
                            sf.OrderByType(queryResults), null);
                    }
                    if (temporary == "adult")
                    {
                        ShowSearchResults(
                            sf.OrderByAdult(queryResults), null);
                    }
                    if (temporary == "startYear")
                    {
                        ShowSearchResults(
                            sf.OrderByStartYear(queryResults), null);
                    }
                    if (temporary == "endYear")
                    {
                        ShowSearchResults(
                            sf.OrderByEndYear(queryResults), null);
                    }
                    if (temporary == "genre")
                    {
                        ShowSearchResults(
                            sf.OrderByGenre(queryResults), null);
                    }
                }
                //If input wrong, asks again
                else
                {
                    Console.WriteLine("Input not correct, try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Method that prints the menu
        /// </summary>
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

        /// <summary>
        /// Method that shows the credits
        /// </summary>
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

        /// <summary>
        /// Method to navigate the menu
        /// </summary>
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
                        break;

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
