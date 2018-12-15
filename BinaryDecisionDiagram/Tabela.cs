using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace BinaryDecisionDiagram
{
    class Tabela
    {
        public static int splitNumber = 0;
        int columns = 288, rows = 20;
        public int[,] tabelka;
        public int[] conditionNumbers;
        public string[] tracking = new string[] { "Tow.TAK: ", "Tow.NIE: ", "SLONCE: ", "DESZCZ: ", "SNIEG: ", "SLABA: ", "SREDNIA: ", "DOBRA: ", "temp <-10: ", "temp -10-10: ", "temp 10-20: ", "temp >20: ", "wiek <10: ", "wiek 10-18: ", "wiek 18-40: ", "wiek >40: ", "CZERWONY: ", "NIEBIESKI: ", "ZIELONY: ", "CZARNY: " };
        public string trackingData;
        public int n, n1, n2, n3, n4;
        public double I;
        

        public Tabela()
        {

        }

        public Tabela(int cols, int [,] values, string trackingData)
        {
            columns = cols;
            tabelka = new int[rows, columns];
            this.trackingData = trackingData;

            if (values.Length != tabelka.Length)
            {
                throw new Exception("nie zgadzaja sie wymiary!");
            }

            tabelka = values;
        }


        #region funkcje startowe: iloczyn kartezjanski + randomowe konkluzje
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
            SetConclusions();
        }

        private void SetConclusions()
        {
            //Random rnd = new Random();
            //for (int i = 0; i < 288; i++)
            //{
            //    int row = rnd.Next(16, 20);
            //    tabelka[row, i] = 1;
            //}
            string[] conclusionsFromTxt = new string[4];
            int i = 0;
            using (StreamReader reader = new StreamReader(@"conclusions.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = RemoveWhitespace(line);
                    conclusionsFromTxt[i] = line;
                    i++;
                    //Console.WriteLine(line);
                }
            }
            for (int j = 0; j < conclusionsFromTxt.Length; j++)
            {
                for (int k = 0; k < tabelka.GetLength(1); k++)
                {
                    tabelka[rows - 4 + j, k] = (int)conclusionsFromTxt[j][k] == 48 ? 0 : 1;
                }
            }

        }
        #endregion

        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public Tabela[] SplitDataByRow()
        {
            FindQuantities();
            SetI();
            double[] entropies = new double[rows - 4];
            int howManyOnes = 0, howManyZeroes = 0, pivot = 0;
            double maxValue = 0.0;
            if (CheckEnd())
            {
                return null;
            }

            for (int i = 0; i < entropies.Length; i++)
            {
                entropies[i] = GetEntropyForRow(i)[2];
                if (maxValue < entropies[i])
                {
                    pivot = i;
                    maxValue = entropies[i];
                }

                Console.WriteLine(entropies[i]);
            }
            Console.WriteLine();
            Console.WriteLine(pivot);

            if (maxValue == 0)
            {
                pivot = 2;

            }

            

            for (int i = 0; i < columns; i++)//ten for jest tu tylko po to, zeby policzyc ile jest kolumn z wartoscia 1 dla wybranego warunku (zeby zadeklarowac tablice o odpowiednim rozmiarze tuz ponizej)
            {
                if (tabelka[pivot,i]==1)
                {
                    howManyOnes++;
                }
                else
                {
                    howManyZeroes++;
                }
            }

            int[,] splittedWithOnes = new int[rows, howManyOnes];
            int[,] splittedWithZeroes = new int[rows, howManyZeroes];

            int iOne = 0, iZero = 0; //iteratory po tablicach splitted
            for (int i = 0; i < columns; i++)
            {
                if (tabelka[pivot, i] == 1)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        splittedWithOnes[j, iOne] = tabelka[j, i];
                    }
                    iOne++;
                }
                else
                {
                    for (int j = 0; j < rows; j++)
                    {
                        splittedWithZeroes[j, iZero] = tabelka[j, i];
                    }
                    iZero++;
                }
            }
            Tabela splitted1 = new Tabela(howManyOnes, splittedWithOnes, tracking[pivot]+" TAK; ");
            Tabela splitted2 = new Tabela(howManyZeroes, splittedWithZeroes, tracking[pivot]+ " NIE; ");
            return new Tabela[] { splitted1, splitted2 };

        }

        private bool CheckEnd()
        {
            int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;

            for (int i = 0; i < tabelka.GetLength(1); i++)
            {
                if (tabelka[rows-4,i]==1)
                {
                    sum1++;
                }
                else if (tabelka[rows - 3, i] == 1)
                {
                    sum2++;
                }
                else if (tabelka[rows - 2, i] == 1)
                {
                    sum3++;
                }
                else if (tabelka[rows - 1, i] == 1)
                {
                    sum4++;
                }
            }
            if (sum1 == tabelka.GetLength(1) || sum2 == tabelka.GetLength(1) || sum3 == tabelka.GetLength(1) || sum4 == tabelka.GetLength(1))
            {
                return true;
            }
            return false;
        }

        public void PrintTab()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(tabelka[i, j]);
                }
                if (i == 15)
                {
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void PrintFinalTab()
        {
            string[] questions = new string[20] { "Towarzysz obecny TAK: ", "Towarzysz obecny NIE: ", "pogoda SLONCE:        ", "pogoda DESZCZ:        ", "pogoda SNIEG:         ", "kondycja SLABA:       ", "kondycja SREDNIA:     ", "kondycja DOBRA:       ", "temp <-10:            ", "temp -10-10:          ", "temp 10-20:           ", "temp >20:             ", "wiek <10:             ", "wiek 10-18:           ", "wiek 18-40:           ", "wiek >40:             ", "szlak CZERWONY:       ", "szlak NIEBIESKI:      ", "szlak ZIELONY:        ", "szlak CZARNY:         " };

            for (int i = 0; i < 20; i++)
            {
                Console.Write(questions[i]);
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(tabelka[i, j]);
                }
                if (i == 15)
                {
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
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

        public void SetI()
        {
            this.I = (Entropy(n1, n) + Entropy(n2, n) + Entropy(n3, n) + Entropy(n4, n));
        }

        public double Entropy(int n1, int n)
        {
            if (n1==0)
            {
                return 0.0;
            }
            return (-(double)n1 / (double)n * Math.Log((double)n1 / (double)n, 2));
        }
        }
    }

