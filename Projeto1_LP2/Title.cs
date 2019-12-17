using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto1_LP2
{
    /// <summary>
    /// Struct that instantiates the search parameters for the titles
    /// </summary>
    public struct Title
    {
        /// <summary>
        /// Property that gets the title 
        /// </summary>
        public string PrimaryTitle { get; }
        /// <summary>
        /// Property that gets the type of the title
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Property that gets if the title is for adults
        /// </summary>
        public bool IsAdult { get; }
        /// <summary>
        /// Property that gets the star year of the title
        /// </summary>
        public short? StartYear { get; }
        /// <summary>
        /// Property that gets the end year of the title
        /// </summary>
        public short? EndYear { get; }
        /// <summary>
        /// IEnumerable of strings that gets the genre info for the title
        /// </summary>
        public IEnumerable<string> Genres { get; }

        /// <summary>
        /// Constructor of the struct
        /// </summary>
        /// <param name="primaryTitle"></param>
        /// <param name="type"></param>
        /// <param name="isAdult"></param>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="genres"></param>
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
