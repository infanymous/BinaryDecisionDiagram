using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryDecisionDiagram
{
    class Node
    {
        public int layer, myNumber;
        public static int number=0;
        public string name;
        public string tracking = "";
        public Tabela tabela;
        public Node parent;
        public List<Node> children;
        public static List<Node> leaves = new List<Node>();
        public string results;

        //Root
        public Node(Tabela tabela)
        {
            this.name = "root";
            this.layer = 0;
            this.tabela = tabela;
            children = new List<Node>();
        }

        //public Node(Tabela tabela, List<Node> children) : this (tabela)
        //{
        //    this.name = "root";
        //    this.layer = 0;
        //    number = 0;
        //    this.tabela = tabela;
        //    this.children = children;
        //}

        //No roots
        public Node(Tabela tabela, Node parent, List<Node> children): this (tabela)
        {
            this.layer = parent.layer + 1;
            number++;
            myNumber = number;
            this.children = children;
            this.name = "Node" + layer.ToString() + "," + number.ToString();
            this.parent = parent;
        }

        public void GenerateChildren()
        {
            
            Tabela[] children = new Tabela[2];
            children = tabela.SplitDataByRow();
            if (children == null)
            {
                leaves.Add(this);
                return;
            }
            //PrintInfo();
            this.children.Add(new Node(children[0], this, new List<Node>()));
            this.children.Add(new Node(children[1], this, new List<Node>()));
        }

        public void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine(String.Format("node from layer {0}, numer {1}, name {2}", layer, myNumber, name));
            tabela.PrintTab();
            Console.WriteLine();

        }

        public void PrintFinalInfo()
        {
            Console.WriteLine();
            Console.WriteLine(String.Format("node from layer {0}, numer {1}, name {2}", layer, myNumber, name));
            Node trackedNode = this;
            while (trackedNode.parent!=null)
            {
                tracking += trackedNode.tabela.trackingData;
                trackedNode = trackedNode.parent;
            }
            Console.WriteLine("route: " + tracking);
            tabela.PrintFinalTab();
            Console.WriteLine();

        }

        public void DrawTree()
        {



        }

        private string GenerateResults()
        {
            int[] sumsOfClues = new int[16];


            for (int i = 0; i < tabela.tabelka.GetLength(0)-4; i++)
            {
                for (int j = 0; j < tabela.tabelka.GetLength(1); j++)
                {
                    if (tabela.tabelka[i,j]==1)
                    {
                        sumsOfClues[i]++;
                    }
                }
            }


            return "";
        }
    }
}
