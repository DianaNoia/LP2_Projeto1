using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto1_LP2
{
    public struct Title
    {
        public string PrimaryTitle { get; }
        public string Type { get; }
        public bool IsAdult { get; }
        public short? StartYear { get; }
        public short? EndYear { get; }
        public IEnumerable<string> Genres { get; }

        public Title(
            string primaryTitle,
            string type,
            bool isAdult,
            short? startYear,
            short? endYear, 
            IEnumerable<string> genres)
        {
            PrimaryTitle = primaryTitle;
            Type = type;
            IsAdult = isAdult;
            StartYear = startYear;
            EndYear = endYear;
            Genres = genres;
        }
    }
}
