using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace Projeto1_LP2
{
    /// <summary>
    /// Class that searches the files 
    /// </summary>
    class SearchFiles
    {
        //Constant sring with the folder name
        private const string appName = "MyIMDBSearcher";

        //Constant string with file name
        private const string fileTitleBasics = "title.basics.tsv.gz";

        //Icollection of titles
        private ICollection<Title> titles;

        //ISet of strigns for all genres available
        public ISet<string> allGenres;

        //Instance of menu UI
        private MenuUI mUI = new MenuUI();

        /// <summary>
        /// Method that searches the files with the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Title[] FileSearch(out string input)
        {
            string searchText;
            Title[] queryResults;
            string titleForSearch = "";
            string typeForSearch = "";
            string adultForSearch = "";
            string startYearForSearch = "";
            string endYearForSearch = "";
            string genreForSearch = "";

            int numTitles = 0;
             // Instance of an HashSet of strings
            allGenres = new HashSet<string>();
            
            // Info on how to search for titles 
            Console.WriteLine("How to Search: \n");
            Console.WriteLine("Write words separated by commas as so:");
            Console.WriteLine("NAME,TYPE,ADULT,STARTYEAR,ENDYEAR,GENRES");
            Console.WriteLine("Example: nemo,movie,FALSE,2001,2005,animation");
            Console.WriteLine("(You can skip parameters)");
            Console.WriteLine("Example: nemo,movie,,,,animation");

            // Receive user input
            searchText = Console.ReadLine();
            
            // Splits the search inputs in miultiple strings
            string[] toFilter = searchText.Split(",");

            if (toFilter.Length == 6)
            {
                titleForSearch = toFilter[0];
                typeForSearch = toFilter[1];
                adultForSearch = toFilter[2];
                startYearForSearch = toFilter[3];
                endYearForSearch = toFilter[4];
                genreForSearch = toFilter[5];
            }

            input = searchText;

            // Navigates to folder
            string folderWithFiles = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), appName);

            // Finds specific file
            string fileTitleBasicsFull = Path.Combine(folderWithFiles,
                fileTitleBasics);

            // Calls method GZipReader
            GZipReader(fileTitleBasicsFull, (line) => numTitles++);

            // Sorts title in a list of titles
            titles = new List<Title>(numTitles);

            GZipReader(fileTitleBasicsFull, LineToTitle);

            // Calls method ShowMemory from MenuUi
            mUI.ShowMemory();
            // Calls method ShowGenres from MenuUi
            mUI.ShowGenres(this);

            // Asks for input to show results
            Console.WriteLine("\nPress any key to show results...");
            Console.ReadKey();

            // Filters title list and searches for the input given
            queryResults =
                (from title in titles
                 where
                    title
                    .PrimaryTitle
                    .ToLower()
                    .Contains(titleForSearch.ToLower()) &&

                    title
                    .Type
                    .ToLower()
                    .Contains(typeForSearch.ToLower()) &&

                    title
                    .IsAdult
                    .ToString()
                    .ToLower()
                    .Contains(adultForSearch.ToLower()) &&

                    title
                    .StartYear
                    .ToString()
                    .ToLower()
                    .Contains(startYearForSearch.ToLower()) &&

                    title
                    .EndYear
                    .ToString()
                    .ToLower()
                    .Contains(endYearForSearch.ToLower()) &&

                    title
                    .Genres
                    .IEnumerableToString()
                    .ToLower()
                    .Contains(genreForSearch.ToLower()) 

                 select title)
                    .ToArray();

            return queryResults;
        }

        /// <summary>
        /// Orders Results by name
        /// </summary>
        /// <param name="queryResults"></param>
        /// <returns></returns>
        public Title[] OrderByName(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.PrimaryTitle).ToArray();
        }

        /// <summary>
        /// Orders results by Type
        /// </summary>
        /// <param name="queryResults"></param>
        /// <returns></returns>
        public Title[] OrderByType(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.Type).ToArray();
        }

        /// <summary>
        /// Orders results by if they are for adults
        /// </summary>
        /// <param name="queryResults"></param>
        /// <returns></returns>
        public Title[] OrderByAdult(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.IsAdult).ToArray();
        }

        /// <summary>
        /// Order results by Start year
        /// </summary>
        /// <param name="queryResults"></param>
        /// <returns></returns>
        public Title[] OrderByStartYear(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.StartYear).ToArray();
        }

        /// <summary>
        /// Order results by End year
        /// </summary>
        /// <param name="queryResults"></param>
        /// <returns></returns>
        public Title[] OrderByEndYear(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.EndYear).ToArray();
        }

        /// <summary>
        /// Orders results by Genre
        /// </summary>
        /// <param name="queryResults"></param>
        /// <returns></returns>
        public Title[] OrderByGenre(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(
                    title => title.Genres.IEnumerableToString()).ToArray();
        }

        /// <summary>
        /// Method for the GZipReader, that unpacks  the files
        /// </summary>
        /// <param name="file"></param>
        /// <param name="actionForEachLine"></param>
        private static void GZipReader(string file, Action<string> actionForEachLine)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (GZipStream gzs = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (StreamReader sr = new StreamReader(gzs))
                    {
                        string line;
                        sr.ReadLine();
                        while ((line = sr.ReadLine()) != null)
                        {
                            actionForEachLine.Invoke(line);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method that reads all file lines
        /// </summary>
        /// <param name="line"></param>
        private void LineToTitle(string line)
        {
            short aux;
            string[] fields = line.Split("\t");
            string[] titleGenres = fields[8].Split(",");
            ICollection<string> cleanTitlesGenres = new List<string>();
            bool isAdult = false;
            short? startYear = null;
            short? endYear = null;

            // Tries to catch exceptions
            try
            {
                if (fields[4] == "0")
                    isAdult = false;
                else if (fields[4] == "1")
                    isAdult = true;

                    startYear = short.TryParse(fields[5], out aux)
                    ? (short?)aux
                    : null;

                endYear = short.TryParse(fields[6], out aux)
                    ? (short?)aux
                    : null;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Tried to parse '{line}'" +
                    $", but got exception '{e.Message}'"
                    + $" with this stack trace: {e.StackTrace}");
            }

            // Sees what genres are valid for the title
            foreach (string genre in titleGenres)
                if (genre != null && genre.Length > 0 && genre != @"\N")
                    cleanTitlesGenres.Add(genre);

            // Adds valid genre on genre list
            foreach (string genre in cleanTitlesGenres)
                allGenres.Add(genre);

            // Instantiate title
            Title t = 
                new Title(
                    fields[2],
                    fields[1],
                    isAdult,
                    startYear,
                    endYear,
                    cleanTitlesGenres);

            titles.Add(t);
        }
    }
}