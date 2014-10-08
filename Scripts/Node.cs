using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace ConsoleApplication
{
    public class Node<T>
    {
        private T data;
        private NodeList<T> formerNodes;
        private NodeList<T> latterNodes;

        public Node(T data) : this(data, null, null){}

        public Node(T data, NodeList<T> former, NodeList<T> latter)
        {
            this.data = data;
            this.formerNodes = former;
            this.latterNodes = latter;
        }

        public T Value {
            get {
                return data;
            }
            set {
                data = value;
            }
        }

        protected NodeList<T> FormerNodes {
            get {
                return formerNodes;
            }
            set {
                formerNodes = value;
            }
        }

        protected NodeList<T> LatterNodes {
            get {
                return latterNodes;
            }
            set {
                latterNodes = value;
            }
        }
    }

    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base()
        {
        }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++) {
                base.Items.Add(default(Node<T>));
            }
        }

        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (Node<T> node in Items) {
                if (node.Value.Equals(value)) {
                    return node;
                }
            }

            // if we reached here, we didn't find a matching node
            return null;
        }
    }
}
