using System;
using Engine;

namespace Snake_Game_V2
{
    class Program
    {
        static void Main(string[] args)
        {
            KestrelApp.Start(
                Globals.WindowWidth,
                Globals.WindowHeight, 
                "Snake Game",
                OnInit);
        }

        private static void OnInit()
        {
            
        }
    }
}