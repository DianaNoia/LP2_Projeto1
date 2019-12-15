using System;
using System.Collections.Generic;

namespace Projeto1_LP2
{
    class Program
    {
        static MenuUI mUI = new MenuUI();
        static void Main(string[] args)
        {
            string input;
            SearchFiles sf = new SearchFiles();
            input = Console.ReadLine();
            sf.FileSearch(input);
        }
    }
}
