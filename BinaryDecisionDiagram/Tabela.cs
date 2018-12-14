using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryDecisionDiagram
{
    class Tabela
    {
        int columns = 288, rows = 20;
        public int[,] tabelka;
        public int n, n1, n2, n3, n4;
        public double I;
        

        public Tabela()
        {

        }

        public Tabela(int cols, int [,] values)
        {
            columns = cols;
            tabelka = new int[rows, columns];

            if (values.Length != tabelka.Length)
            {
                throw new Exception("nie zgadzaja sie wymiary!");
            }

            tabelka = values;
        }

        public void Generate()
        {
            int towarzysz = 2, pogoda = 3, kondycja = 3, temperatura = 4, wiek = 4; //rozmiar przeslanki (np. tak/nie -> 2)
            int iT = 0, iP = 0, iK = 0, iTe = 0, iW = 0; //iteratory potrzebne pozniej
            int size = towarzysz * pogoda * kondycja * temperatura * wiek; //wszystkie mozliwosci, 288
            int[,] tab = new int[rows, size]; //20 wierszy bo tyle przeslanek+konkluzji(konkluzje beda potem), 288 kolumn
            int[] helper = new int[rows];

            for (int i = 0; i < size; i++) //lecimy po kolei po kazdej kolumnie
            {
                helper = new int[rows]; //na poczatku musimy wyzerowac uzywana tablice, bo sie zapelnia jedynkami po kazdym przejsciu
                
                if (iW == 4) //przeslanka wiek ma 4 opcje, co 4 mozliwosci zerujemy, ale przesuwamy zmienna, ktora jest "o jeden wyzej w hierarchii" zeby zbadac wszystkie mozliwosci 
                {
                    iTe++;
                    iW = 0;
                }
                if (iTe == 4)
                {
                    iK++;
                    iTe = 0;
                }
                if (iK == 3)
                {
                    iP++;
                    iK = 0;
                }
                if (iP == 3)
                {
                    iT++;
                    iP = 0;
                }
                //iT rosnie najwolniej - zwiekszy sie o 1 dokladnie w polowie calej petli (144 przejscie), to jest nasze tak/nie(towarzysz)
                helper[0 + iT] = 1;//przez cala tabelke to jeden bedzie albo na indeksie 0 helpera (helper to kazda kolejna kolumna) albo na 1 - na 1 wskoczy w polowie
                helper[2 + iP] = 1;//iP rosnie szybciej od iT, ale wolniej od iK
                helper[5 + iK] = 1;//etc
                helper[8 + iTe] = 1;
                //iW rosnie najszybciej - co kazde przejscie petli, w ten sposob badamy kazda mozliwa kombinacje (kazdy z kazdym)
                helper[12 + iW] = 1;
                iW++;

                for (int j = 0; j < rows; j++)
                {
                    tab[j,i] = helper[j]; //wpisujemy helpera jako kolumne naszej tabelki
                }
            }

            tabelka = new int[rows, size];
            tabelka = tab;
            SetRandomConclusions();
        }

        private void SetRandomConclusions()
        {
            Random rnd = new Random();
            for (int i = 0; i < 288; i++)
            {
                int row = rnd.Next(16, 20);
                tabelka[row, i] = 1; 
            }
            //for (int i = 0; i < 20; i++)
            //{
            //    for (int j = 0; j < 170; j++)
            //    {
            //        Console.Write(tabelka[i, j]);
            //    }
            //    Console.WriteLine();
            //}
        }

        public int CountWantedColumns(int desiredRow, bool plusOrMinus)
        {
            int cols = 0;
            if (plusOrMinus)
            {
                for (int i = 0; i < tabelka.GetLength(1); i++)
                {
                    if (tabelka[desiredRow, i] == 1)
                    {
                        cols++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < tabelka.GetLength(1); i++)
                {
                    if (tabelka[desiredRow, i] == 0)
                    {
                        cols++;
                    }
                }
            }
            

            return cols;
        }

        public void PrintTab(int cols)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j  = 0; j < cols; j++)
                {
                    Console.Write(tabelka[i, j]);
                }
                if (i==15)
                {
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void FindQuantities()
        {
            n = tabelka.GetLength(1);
            n1 = 0; n2 = 0; n3 = 0; n4 = 0;

            for (int i = 0; i < tabelka.GetLength(1); i++)
            {
                if (tabelka[16,i]==1)
                {
                    n1++;
                }
                else if (tabelka[17,i]==1)
                {
                    n2++;
                }
                else if (tabelka[18, i] == 1)
                {
                    n3++;
                }
                else 
                {
                    n4++;
                }
            }
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.Write(n1 + " " + n2 + " " + n3 + " " + n4);
        }

        public double[] GetEntropyForRow(int row)
        {
            int countOnes = 0, countZeroes = 0, n1plus = 0, n2plus = 0, n3plus = 0, n4plus = 0, n1minus = 0, n2minus = 0, n3minus = 0, n4minus = 0;

            for (int i = 0; i < tabelka.GetLength(1); i++)
            {
                if (tabelka[row, i] == 1)
                {
                    countOnes++;
                    n1plus += tabelka[rows - 4, i];
                    n2plus += tabelka[rows - 3, i];
                    n3plus += tabelka[rows - 2, i];
                    n4plus += tabelka[rows - 1, i]; //mamy info, dla ilu potwierdzen warunku1 mamy spelnionych przeslanek 1, 2, 3, 4
                }
                else
                {
                    countZeroes++;
                    n1minus += tabelka[rows - 4, i];
                    n2minus += tabelka[rows - 3, i];
                    n3minus += tabelka[rows - 2, i];
                    n4minus += tabelka[rows - 1, i];//mamy info, dla ilu zaprzeczen warunku1 mamy spelnionych kazdych przeslanek
                }
            }

            double entropyPlus = Entropy(n1plus, countOnes) + Entropy(n2plus, countOnes) + Entropy(n3plus, countOnes) + Entropy(n4plus, countOnes);
            double entropyMinus = Entropy(n1minus, countZeroes) + Entropy(n2minus, countZeroes) + Entropy(n3minus, countZeroes) + Entropy(n4minus, countZeroes);
            double Ej = GetEj(entropyPlus, countOnes, entropyMinus, countZeroes);

            double IminusEj = I - Ej;

            return new double[] { entropyPlus, entropyMinus, IminusEj };
        }

        public double GetEj(double entropyPlus, int countOnes, double entropyMinus, int countZeroes)
        {
            return (((double)countOnes / (double)columns) * entropyPlus + ((double)countZeroes / (double)columns) * entropyMinus);
        }

        public double Entropy(int n1, int n)
        {
            return (-(double)n1 / (double)n * Math.Log((double)n1 / (double)n, 2));
        }
        }
    }

