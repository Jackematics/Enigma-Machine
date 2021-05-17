using System;
using System.Collections.Generic;
using System.Linq;

namespace EnigmaMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Enigma enigma = new Enigma();

            string test = "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG";

            string testOne = "EAH EBGBG JKRUK GQT PBALCO NBHX EIT SGCS TBQ";

            Console.WriteLine(enigma.Transform(new int[] { 0, 1, 2 }, test));
            Console.WriteLine(enigma.Transform(new int[] { 0, 1, 2 }, testOne));

            Console.ReadLine();
        }
    }
}
