using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 1: Sonar Sweep ---
    /// You're minding your own business on a ship at sea when the overboard alarm goes off! You rush to see if you can help. Apparently, one of the Elves tripped and accidentally sent the sleigh keys flying into the ocean!
    /// Before you know it, you're inside a submarine the Elves keep ready for situations like this. It's covered in Christmas lights(because of course it is), and it even has an experimental antenna that should be able to track the keys if you can boost its signal strength high enough; there's a little meter that indicates the antenna's signal strength by displaying 0-50 stars.
    /// Your instincts tell you that in order to save Christmas, you'll need to get all fifty stars by December 25th.
    /// Collect stars by solving puzzles.Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first.Each puzzle grants one star. Good luck!
    /// As the submarine drops below the surface of the ocean, it automatically performs a sonar sweep of the nearby sea floor.On a small screen, the sonar sweep report (your puzzle input) appears: each line is a measurement of the sea floor depth as the sweep looks further and further away from the submarine.
    /// </summary>
    public class Day01
    {
        private readonly string _misure = "";

        public Day01()
        {
            _misure = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day01Input.txt"));
        }

        public int ContaMisureQuiz1()
        {
            var numeri = _misure.Split('\n');
            var precendente = numeri.First().Trim();
            var intPrecedente = int.Parse(precendente);
            var contatore = 0;
            foreach (var numero in numeri)
            {
                var intCorrente = int.Parse(numero.Trim());
                if (intCorrente > intPrecedente)
                    contatore++;
                intPrecedente = intCorrente;
            }

            return contatore;
        }
        public int ContaMisureQuiz2()
        {
            var numeri = _misure.Split('\n').Select(x => x.Trim()).Select(x => int.Parse(x)).ToArray();
            var finestra1precedente = numeri[0];
            var finestra2precedente = numeri[1];
            var finestra3precedente = numeri[2];

            var sommaPrecedente = finestra1precedente + finestra2precedente + finestra3precedente;
            int contatore = 0;

            for (int i = 0; i < (numeri.Length - 2); i++)
            {
                var finestra1corrente = numeri[i];
                var finestra2corrente = numeri[i + 1];
                var finestra3corrente = numeri[i + 2];
                var sommaCorrente = finestra1corrente + finestra2corrente + finestra3corrente;
                if (sommaPrecedente < sommaCorrente)
                    contatore++;
                sommaPrecedente = sommaCorrente;
            }
            return contatore;
        }
    }
}
