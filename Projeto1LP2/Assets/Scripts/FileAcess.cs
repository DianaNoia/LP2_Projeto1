using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class FileAcess : MonoBehaviour
{

    private string appName = "MyIMDBSearcher";

    public void Decompress()
    {
        string folderWithFiles = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    appName);
        
        GZipStream gz = new GZipStream(File.OpenRead() );


    }


}