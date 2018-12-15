using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryDecisionDiagram
{
    class Node
    {
        public int layer;
        public static int number;
        public string name;
        public Tabela tabela;
        public Node parent;
        public List<Node> children;
        
        //Root
        public Node(Tabela tabela, List<Node> children)
        {
            this.name = "root";
            this.layer = 0;
            number = 0;
            this.tabela = tabela;
            this.children = children;
        }

        //No roots
        public Node(Tabela tabela, Node parent, List<Node> children): this (tabela, children)
        {
            this.layer = parent.layer + 1;
            number++;
            this.name = "Node" + layer.ToString() + "," + number.ToString();
            this.parent = parent;
        }
    }
}
