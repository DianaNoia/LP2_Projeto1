using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Title
{
    public string PrimaryTitle { get; }
    public short? StartYear { get; }

        public IEnumerable<string> Genres { get; }

    public Title(string primarytitle, short? startYear, IEnumerable<string> genres)
    {
        PrimaryTitle = primarytitle;
        StartYear = startYear;
        Genres = genres;
    }

}
