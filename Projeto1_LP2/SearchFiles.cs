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
            string genresForSearch = "";

            int numTitles = 0;

            allGenres = new HashSet<string>();

            mUI.ShowGenres(this);

            Console.WriteLine("Write the name of the title you would like to " +
                "search and then the genres you would like to see, all " +
                "separeted just by a comma.");
            Console.WriteLine("Exemple: nemo,animation,action");

            searchText = Console.ReadLine();
            string[] toFilter = searchText.Split(",");

            for (int i = 0; i < toFilter.Length; i++)
            {
                if (i > 0)
                {
                    genresForSearch = toFilter[i];
                }
                else
                {
                    titleForSearch = toFilter[0];
                }
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

            queryResults =
                (from title in titles
                 where title.PrimaryTitle.ToLower().Contains(titleForSearch.ToLower())
                 where title.Genres.Contains(genresForSearch)
                 select title)
                 .OrderBy(title => title.StartYear)
                 .ThenBy(title => title.PrimaryTitle)
                 .ToArray();

            Console.Write($"{titleForSearch}\t");
            Console.Write(genresForSearch);

            return queryResults;
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
            short? startYear;

            try
            {
                startYear = short.TryParse(fields[5], out aux)
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

            Title t = new Title(fields[2], startYear, cleanTitlesGenres.ToArray());

            titles.Add(t);
        }
    }
}