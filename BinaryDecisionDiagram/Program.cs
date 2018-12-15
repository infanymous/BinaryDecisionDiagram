using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace BinaryDecisionDiagram
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(190, 35);
            Tabela firstTab = new Tabela();
            int pivot = 0;
            double maxValue = 0.0;

            firstTab.Generate(); //generuje iloczyn kartezjanski przeslanek i wczytuje wartosci konkluzji


            firstTab.FindQuantities(); //znajduje ile jest kolumn z kazda konkluzja
            firstTab.PrintTab(150);
            firstTab.SetI();

            double[] entropies = new double[firstTab.tabelka.GetLength(0) - 4];
            for (int i = 0; i < entropies.Length; i++)
            {
                entropies[i] = firstTab.GetEntropyForRow(i)[2];
                if (maxValue < entropies[i])
                {
                    pivot = i;
                    maxValue = entropies[i];
                }

                Console.WriteLine(entropies[i]);
            }
            Console.WriteLine();
            Console.WriteLine(pivot);

            Tabela[] firstsplit = firstTab.SplitDataByRow(pivot);
            firstsplit[0].FindQuantities();
            firstsplit[0].SetI();
            firstsplit[0].PrintTab(firstsplit[0].tabelka.GetLength(1));
            entropies = new double[firstsplit[0].tabelka.GetLength(0) - 4];
            pivot = 0; maxValue = 0.0;
            for (int i = 0; i < entropies.Length; i++)
            {
                entropies[i] = firstsplit[0].GetEntropyForRow(i)[2];
                if (maxValue < entropies[i])
                {
                    pivot = i;
                    maxValue = entropies[i];
                }

                Console.WriteLine(entropies[i]);
            }
            Console.WriteLine();
            Console.WriteLine(pivot);
            Tabela[] secondsplit = firstsplit[0].SplitDataByRow(pivot);
            secondsplit[0].PrintTab(secondsplit[0].tabelka.GetLength(1));
            secondsplit[1].PrintTab(secondsplit[1].tabelka.GetLength(1));
            //firstsplit[1].PrintTab(30);

        }
    }
}





//int ileKolumn = 150;
//Console.WriteLine(g.Entropy(g.n1, g.n) + "    = Entropia, dla " + g.n1 + " konkluzji nr 1 (szlak czerwony)" );
//            Console.WriteLine(g.Entropy(g.n2, g.n) + "    = Entropia, dla " + g.n2 + " konkluzji nr 2 (szlak niebieski)");
//            Console.WriteLine(g.Entropy(g.n3, g.n) + "    = Entropia, dla " + g.n3 + " konkluzji nr 3 (szlak zielony)");
//            Console.WriteLine(g.Entropy(g.n4, g.n) + "    = Entropia,  dla " + g.n4 + " konkluzji nr 4 (szlak czarny)");

//            Console.WriteLine("nasze I: " + naszeI);
//            Console.WriteLine();

//            int desiredRow = 0;
//int columns = g.CountWantedColumns(desiredRow, true);
//Console.WriteLine(columns + " tyle mamy kolumn z potwierdzonym warunkiem 1 (wiersz 1 tablicy), (dokladnie polowa, bo to warunek tak/nie a mamy iloczyn kartezjanski)");
//            int nplus = 0, n1plus = 0, n2plus = 0, n3plus = 0, n4plus = 0;
//int nminus = 0, n1minus = 0, n2minus = 0, n3minus = 0, n4minus = 0;


//            for (int i = 0; i<g.tabelka.GetLength(1); i++)
//            {
//                if (g.tabelka[0, i]==1)
//                {
//                    nplus++;
//                    n1plus += g.tabelka[16, i];
//                    n2plus += g.tabelka[17, i];
//                    n3plus += g.tabelka[18, i];
//                    n4plus += g.tabelka[19, i]; //mamy info, dla ilu potwierdzen warunku1 mamy spelnionych przeslanek 1, 2, 3, 4
//                }
//                else
//                {
//                    nminus++;
//                    n1minus += g.tabelka[16, i];
//                    n2minus += g.tabelka[17, i];
//                    n3minus += g.tabelka[18, i];
//                    n4minus += g.tabelka[19, i];//mamy info, dla ilu zaprzeczen warunku1 mamy spelnionych kazdych przeslanek
//                }
//            }

//            double entropyWarunek1plus = (g.Entropy(n1plus, nplus) + g.Entropy(n2plus, nplus) + g.Entropy(n3plus, nplus) + g.Entropy(n4plus, nplus)); //entropia dla potwierdzonego warunku 1
//double entropyWarunek1minus = (g.Entropy(n1minus, nminus) + g.Entropy(n2minus, nminus) + g.Entropy(n3minus, nminus) + g.Entropy(n4minus, nminus)); //entropia dla zaprzeczonego warunku 1

//Console.WriteLine("ilosc kazdej konkluzji po potwierdzeniu warunku1 (na koncu laczna liczba potwierdzonych warunkow 1): " + n1plus  + " " + n2plus + " " + n3plus + " " + n4plus + " " + nplus);
//            Console.WriteLine("ilosc kazdej konkluzji po zaprzeczeniu warunku1: " + n1minus + " " + n2minus + " " + n3minus + " " + n4minus + " " + nminus);
//            Console.WriteLine("entropia po potwierdzeniu, czy mamy towarzysza (warunek 1+): " + entropyWarunek1plus + " " + g.GetEntropyForRow(0)[0]);
//            Console.WriteLine("entropia po zaprzeczeniu, czy mamy towarzysza (warunek 1-): " + entropyWarunek1minus + " " + g.GetEntropyForRow(0)[1]);

//            double naszeE1 = (144.0 / 288.0 * entropyWarunek1minus + 144.0 / 288.0 * entropyWarunek1plus);
//Console.WriteLine("suma tych entropii * liczebnosc (nasze E1): " + naszeE1);//144/288 dlatego, ze w iloczynie kartezjanskim mamy od razu polowe warunku 1 w jedynkach, a polowe w zerach

//            Console.WriteLine("I - E1: " + (naszeI - naszeE1) + " " + g.GetEntropyForRow(0)[2]);
