using System;
using System.Collections.Generic;

namespace Projeto1_LP2
{
    /// <summary>
    /// Class Program that runs the program
    /// </summary>
    class Program
    {
        // Instantiates the MenuUi class
        static MenuUI mUI = new MenuUI();
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Calls ShowMenu method
            mUI.ShowMenu();
        }
    }
}
