using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class FileAcess : MonoBehaviour
{

    private string appName = "MyIMDBSearcher/title.basics.tsv.gz";

    public void Decompress()
    {
        string folderWithFiles = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        appName);

        GZipStream gz = new GZipStream(File.OpenRead(folderWithFiles), CompressionMode.Decompress);

        StreamReader sr = new StreamReader(gz);

        using (sr)
        {
            while (sr.ReadLine() != null)
            {
                string line = sr.ReadLine();
                Debug.Log(line);
            }
        }


    }

}