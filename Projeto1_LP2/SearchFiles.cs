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

            Console.WriteLine("Write what you would like to search!");
            searchText = Console.ReadLine();
            input = searchText;

            int numTitles = 0;

            allGenres = new HashSet<string>();

            string folderWithFiles = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), appName);

            string fileTitleBasicsFull = Path.Combine(folderWithFiles,
                fileTitleBasics);

            GZipReader(fileTitleBasicsFull, (line) => numTitles++);

            titles = new List<Title>(numTitles);

            GZipReader(fileTitleBasicsFull, LineToTitle);

            mUI.ShowMemory();
            mUI.ShowGenres(this);

            queryResults =
                (from title in titles
                 where title.PrimaryTitle.ToLower().Contains(searchText.ToLower())
                 select title)
                 .OrderBy(title => title.StartYear)
                 .ThenBy(title => title.PrimaryTitle)
                 .ToArray();

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