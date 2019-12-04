using System;
using System.Collections.Generic;
using System.Text;

namespace LP1
{
    public class InputEnter
    {
        public void SearchInput()
        {
            Console.WriteLine("Welcome to the IMDB Searcher! \nPlease enter your search terms:");
            string searchTerm = Console.ReadLine();
            Console.WriteLine("You searched for: " + searchTerm);
        } 
    }
}
