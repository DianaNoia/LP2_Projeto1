using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FileAcess : MonoBehaviour
{
    
    //Nome da pasta
    private const string appName = "MyIMDBSearcher";

    [SerializeField]
    private InputField searchTerm;

    //Nome do ficheiro que quero
    private const string fileTitleBasics = "title.basics.tsv.gz";

    //Numero de titulos a mostrar por vez
    private const int numTitlesToShowOnScreen = 10;

    //Coleção de títulos
    private ICollection<Title> titles;

    //Diferentes géneros
    private ISet<string> allGenres;



    public void Start()
    {

    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(searchTerm.text != "")
            {
                TestingIfItWorks(searchTerm.text);
            }
        }  
    }

    public void TestingIfItWorks(string doIt)
    {
        //Variável auxiliar para as pesquisas
        Title[] queryResults;

        //Número de títulos
        int numTitles = 0;

        //Número de títulos já mostrados ao user
        int numTitlesShown = 0;

        /*Inicializar conjunto com os diferentes géneros na
         base de dados (ñ permite géneros repetidos) */
        allGenres = new HashSet<string>();

        //Caminho completo da pasta com os ficheiros de dados
        string folderWithFiles = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            appName);

        //Caminho completo de cada ficheiro de dados
        string fileTitleBasicsFull =
            Path.Combine(folderWithFiles, fileTitleBasics);

        //Contar número de linhas (aka número de títulos)
        GZipReader(fileTitleBasicsFull, (line) => numTitles++);

        /*Instanciar lista c/ tamanho pré-definido para o número
        de titulos existentes */
        titles = new List<Title>(numTitles);

        //Preencher lista de títulos com info lida no ficheiro
        GZipReader(fileTitleBasicsFull, LineToTitle);

        //Mostrar todos os géneros conhecidos, ordenados por eles próprios
        Debug.Log($"\t=> Known genres (total {allGenres.Count}): ");
        foreach (string genre in allGenres.OrderBy(g => g))
            Debug.Log($"{genre} ");

        /* Pesquisar pelo título, ordenando os resultados por ano e depois por
           título e convertendo os resultados num array para depois poder ser
           percorrido de forma eficiente */

        queryResults =
            (from title in titles
             where title.PrimaryTitle.ToLower().Contains(doIt.ToLower())
             select title)
             .OrderBy(title => title.StartYear)
             .ThenBy(title => title.PrimaryTitle)
             .ToArray();

        //Dizer quantos títulos foram encontrados
        Debug.Log($"\t=> There are {queryResults.Count()} titles with \"{doIt}");

        // Mostrar 10 títulos de cada vez
        while (numTitlesShown < queryResults.Length)
        {
            //mostrar próximos 10
            for (int i = numTitlesShown;
                i < numTitlesShown + numTitlesToShowOnScreen
                && i < queryResults.Length;
                i++)
            {
                //Usar para melhorar a forma como mostramos os géneros
                bool firstGenre = true;

                //Obter título atual
                Title title = queryResults[i];

                //Mostrar info sobre o título
                Debug.Log("\t\t* ");
                Debug.Log($"\"{title.PrimaryTitle}\" ");
                Debug.Log($"({title.StartYear?.ToString() ?? "unknown year"}): ");
                foreach (string genre in title.Genres)
                {
                    if (!firstGenre) Console.Write("/");
                    Debug.Log($"{genre} ");
                    firstGenre = false;
                }
            }
            //Próximos 10 títulos
            numTitlesShown += numTitlesToShowOnScreen;
        }
    }

    private static void GZipReader(
        string file, Action<string> actionForEachLine)
    {
        //Abrir ficheiro em modo leitura
        using (FileStream fs = new FileStream(
            file, FileMode.Open, FileAccess.Read))
        {
            //Decorar o ficheiro com um compressor p/ formato GZip
            using (GZipStream gzs = new GZipStream(
            fs, CompressionMode.Decompress))
            {
                //Usar um StreamReader p/ simpliciar a leitura
                using (StreamReader sr = new StreamReader(gzs))
                {
                    //Linha a ler
                    string line;

                    //Ignorar primeira linha do cabeçalho
                    sr.ReadLine();

                    //Percorrer linhas
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Aplicar ação à linha atual
                        actionForEachLine.Invoke(line);
                    }
                }
            }
        }
    }

    private void LineToTitle(string line)
    {
        short aux;
        string[] fields = line.Split('\t');
        string[] titleGenres = fields[8].Split(',');
        ICollection<string> cleanTitleGenres = new List<string>();
        short? startYear;

        //tentar determinar ano de lançamento, se possível
        try
        {
            startYear = short.TryParse(fields[5], out aux)
                ? (short?)aux
                : null;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"Tried to parse {line}, but got exception '{e.Message}'"
                + $" with this stack trace: {e.StackTrace}");
        }

        //Remover géneros inválidos
        foreach (string genre in titleGenres)
            if (genre != null && genre.Length > 0 && genre != @"\N")
                cleanTitleGenres.Add(genre);

        //Adicionar géneros válidos ao conjunto de géneros da base de dados
        foreach (string genre in cleanTitleGenres)
            allGenres.Add(genre);

        //Criar novo título usando a info obtida na linha
        Title t = new Title(
            fields[2], startYear, cleanTitleGenres.ToArray());

        //Adicionar Título à coleção de títulos
        titles.Add(t);
    }

}