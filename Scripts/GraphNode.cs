using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication
{
    class GraphNode<T> : Node<T>
    {
        public GraphNode(T data) : base(data) { }
        public GraphNode(T data, NodeList<T> former, NodeList<T> latter) : base(data, former, latter) { }

        new public NodeList<T> FormerNodes
        {
            get
            {
                if (base.FormerNodes == null)
                {
                    base.FormerNodes = new NodeList<T>();
                }

                return base.FormerNodes;
            }
        }

        new public NodeList<T> LatterNodes
        {
            get
            {
                if (base.LatterNodes == null)
                {
                    base.LatterNodes = new NodeList<T>();
                }

                return base.LatterNodes;
            }
        }
    }
}
