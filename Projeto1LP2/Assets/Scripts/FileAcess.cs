using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileAcess : MonoBehaviour
{

    string folderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        appName);

    private const string appName = "MyIMDBSearcher";
    private const string fileNameBasics = "name.basics.tsv.gz";
    private const string fileTitleAkas = "title.akas.tsv.gz";
    private const string fileTitleBasics = "title.basics.tsv.gz";
    private const string fileTitleCrew = "title.crew.tsv.gz";
    private const string fileTitleEpisode = "title.episode.tsv.gz";
    private const string fileTitlePrincipals = "title.principals.tsv.gz";
    private const string fileTitleRatings = "title.ratings.tsv.gz";
}
    
    /* using (StreamReader sr = new StreamReader("TestFile.txt")) 
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null) 
                {
                    Debug.Log(line);
                }
            } 
    */

}
