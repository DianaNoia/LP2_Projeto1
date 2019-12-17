using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace Projeto1_LP2
{
    class SearchFiles
    {
        private const string appName = "MyIMDBSearcher";

        private const string fileTitleBasics = "title.basics.tsv.gz";

        private ICollection<Title> titles;

        public ISet<string> allGenres;

        private MenuUI mUI = new MenuUI();

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

            allGenres = new HashSet<string>();

            Console.WriteLine("How to Search: \n");
            Console.WriteLine("Write words separated by commas as so:");
            Console.WriteLine("NAME,TYPE,ADULT,STARTYEAR,ENDYEAR,GENRES");
            Console.WriteLine("Example: nemo,movie,FALSE,2001,2005,animation");
            Console.WriteLine("(You can skip parameters)");
            Console.WriteLine("Example: nemo,movie,,,,animation");

            searchText = Console.ReadLine();
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

            string folderWithFiles = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), appName);

            string fileTitleBasicsFull = Path.Combine(folderWithFiles,
                fileTitleBasics);

            GZipReader(fileTitleBasicsFull, (line) => numTitles++);

            titles = new List<Title>(numTitles);

            GZipReader(fileTitleBasicsFull, LineToTitle);

            mUI.ShowMemory();

            mUI.ShowGenres(this);

            Console.WriteLine("\nPress any key to show results...");
            Console.ReadKey();

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

        public Title[] OrderByName(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.PrimaryTitle).ToArray();
        }

        public Title[] OrderByType(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.Type).ToArray();
        }

        public Title[] OrderByAdult(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.IsAdult).ToArray();
        }

        public Title[] OrderByStartYear(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.StartYear).ToArray();
        }

        public Title[] OrderByEndYear(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(title => title.EndYear).ToArray();
        }

        public Title[] OrderByGenre(Title[] queryResults)
        {
            return queryResults =
                (from title in queryResults
                 select title).OrderBy(
                    title => title.Genres.IEnumerableToString()).ToArray();
        }

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

        private void LineToTitle(string line)
        {
            short aux;
            string[] fields = line.Split("\t");
            string[] titleGenres = fields[8].Split(",");
            ICollection<string> cleanTitlesGenres = new List<string>();
            bool isAdult = false;
            short? startYear = null;
            short? endYear = null;

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

            foreach (string genre in titleGenres)
                if (genre != null && genre.Length > 0 && genre != @"\N")
                    cleanTitlesGenres.Add(genre);

            foreach (string genre in cleanTitlesGenres)
                allGenres.Add(genre);

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