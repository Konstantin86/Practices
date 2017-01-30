using System;

namespace algs.DataStructures
{
    // https://simpledevcode.wordpress.com/2014/12/25/red-black-tree-in-c/
    //         Average	Worst case
    // Space   O(n)       O(n)
    // Search  O(log n)   O(log n)
    // Insert  O(log n)   O(log n)
    // Delete  O(log n)   O(log n)
    public class RedBlackTree
    {
        /// <summary>
        /// Object of type Node contains 4 properties
        /// Colour
        /// Left
        /// Right
        /// Parent
        /// Data
        /// </summary>
        /// <summary>
        /// Root node of the tree (both reference & pointer)
        /// </summary>
        private Node _root;

        /// <summary>
        /// Left Rotate
        /// </summary>
        /// <param name="x"></param>
        /// <returns>void</returns>
        private void LeftRotate(Node x)
        {
            Node y = x.Right; // set Y
            x.Right = y.Left;//turn Y's left subtree into X's right subtree
            if (y.Left != null)
            {
                y.Left.Parent = x;
            }
            if (y != null)
            {
                y.Parent = x.Parent;//link X's parent to Y
            }
            if (x.Parent == null)
            {
                _root = y;
            }
            if (x.Parent != null && x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                if (x.Parent != null) x.Parent.Right = y;
            }
            y.Left = x; //put X on Y's left
            x.Parent = y;
        }
        /// <summary>
        /// Rotate Right
        /// </summary>
        /// <param name="y"></param>
        /// <returns>void</returns>
        private void RightRotate(Node y)
        {
            // right rotate is simply mirror code from left rotate
            Node x = y.Left;
            y.Left = x.Right;
            if (x.Right != null)
            {
                x.Right.Parent = y;
            }
            if (x != null)
            {
                x.Parent = y.Parent;
            }
            if (y.Parent == null)
            {
                _root = x;
            }
            if (y.Parent != null && y == y.Parent.Right)
            {
                y.Parent.Right = x;
            }
            if (y.Parent != null && y == y.Parent.Left)
            {
                y.Parent.Left = x;
            }

            x.Right = y;//put Y on X's right
            y.Parent = x;
        }
        /// <summary>
        /// Display Tree
        /// </summary>
        public void DisplayTree()
        {
            if (_root == null)
            {
                Console.WriteLine("Nothing in the tree!");
                return;
            }
            if (_root != null)
            {
                InOrderDisplay(_root);
            }
        }
        /// <summary>
        /// Find item in the tree
        /// </summary>
        /// <param name="key"></param>
        public Node Find(int key)
        {
            bool isFound = false;
            Node temp = _root;
            while (!isFound)
            {
                if (temp == null)
                {
                    break;
                }
                if (key < temp.Data)
                {
                    temp = temp.Left;
                }
                if (key > temp.Data)
                {
                    temp = temp.Right;
                }
                if (key == temp.Data)
                {
                    isFound = true;
                }
            }
            if (isFound)
            {
                Console.WriteLine("{0} was found", key);
                return temp;
            }
            else
            {
                Console.WriteLine("{0} not found", key);
                return null;
            }
        }
        /// <summary>
        /// Insert a new object into the RB Tree
        /// </summary>
        /// <param name="item"></param>
        public void Insert(int item)
        {
            Node newItem = new Node(item);
            if (_root == null)
            {
                _root = newItem;
                _root.Colour = Color.Black;
                return;
            }
            Node y = null;
            Node x = _root;
            while (x != null)
            {
                y = x;
                x = newItem.Data < x.Data ? x.Left : x.Right;
            }

            newItem.Parent = y;

            if (y == null)
            {
                _root = newItem;
            }
            else if (newItem.Data < y.Data)
            {
                y.Left = newItem;
            }
            else
            {
                y.Right = newItem;
            }

            newItem.Left = null;
            newItem.Right = null;
            newItem.Colour = Color.Red;//colour the new node red
            InsertFixUp(newItem);//call method to check for violations and fix
        }
        private void InOrderDisplay(Node current)
        {
            if (current != null)
            {
                InOrderDisplay(current.Left);
                Console.Write("({0}) ", current.Data);
                InOrderDisplay(current.Right);
            }
        }
        private void InsertFixUp(Node item)
        {
            //Checks Red-Black Tree properties
            while (item != _root && item.Parent.Colour == Color.Red)
            {
                /*We have a violation*/
                if (item.Parent == item.Parent.Parent.Left)
                {
                    Node y = item.Parent.Parent.Right;
                    if (y != null && y.Colour == Color.Red)//Case 1: uncle is red
                    {
                        item.Parent.Colour = Color.Black;
                        y.Colour = Color.Black;
                        item.Parent.Parent.Colour = Color.Red;
                        item = item.Parent.Parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (item == item.Parent.Right)
                        {
                            item = item.Parent;
                            LeftRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.Parent.Colour = Color.Black;
                        item.Parent.Parent.Colour = Color.Red;
                        RightRotate(item.Parent.Parent);
                    }

                }
                else
                {
                    //mirror image of code above
                    Node x;

                    x = item.Parent.Parent.Left;
                    if (x != null && x.Colour == Color.Black)//Case 1
                    {
                        item.Parent.Colour = Color.Red;
                        x.Colour = Color.Red;
                        item.Parent.Parent.Colour = Color.Black;
                        item = item.Parent.Parent;
                    }
                    else //Case 2
                    {
                        if (item == item.Parent.Left)
                        {
                            item = item.Parent;
                            RightRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.Parent.Colour = Color.Black;
                        item.Parent.Parent.Colour = Color.Red;
                        LeftRotate(item.Parent.Parent);

                    }

                }
                _root.Colour = Color.Black;//re-colour the root black as necessary
            }
        }

        /// <summary>
        /// Deletes a specified value from the tree
        /// </summary>
        /// <param name="key"></param>
        public void Delete(int key)
        {
            //first find the node in the tree to delete and assign to item pointer/reference
            Node item = Find(key);
            Node x;
            Node y;

            if (item == null)
            {
                Console.WriteLine("Nothing to delete!");
                return;
            }
            if (item.Left == null || item.Right == null)
            {
                y = item;
            }
            else
            {
                y = TreeSuccessor(item);
            }
            x = y.Left ?? y.Right;
            if (x != null)
            {
                x.Parent = y;
            }
            if (y.Parent == null)
            {
                _root = x;
            }
            else if (y == y.Parent.Left)
            {
                y.Parent.Left = x;
            }
            else
            {
                y.Parent.Left = x;
            }
            if (y != item)
            {
                item.Data = y.Data;
            }
            if (y.Colour == Color.Black)
            {
                DeleteFixUp(x);
            }

        }
        /// <summary>
        /// Checks the tree for any violations after deletion and performs a fix
        /// </summary>
        /// <param name="x"></param>
        private void DeleteFixUp(Node x)
        {

            while (x != null && x != _root && x.Colour == Color.Black)
            {
                if (x == x.Parent.Left)
                {
                    Node w = x.Parent.Right;
                    if (w.Colour == Color.Red)
                    {
                        w.Colour = Color.Black; //case 1
                        x.Parent.Colour = Color.Red; //case 1
                        LeftRotate(x.Parent); //case 1
                        w = x.Parent.Right; //case 1
                    }
                    if (w.Left.Colour == Color.Black && w.Right.Colour == Color.Black)
                    {
                        w.Colour = Color.Red; //case 2
                        x = x.Parent; //case 2
                    }
                    else if (w.Right.Colour == Color.Black)
                    {
                        w.Left.Colour = Color.Black; //case 3
                        w.Colour = Color.Red; //case 3
                        RightRotate(w); //case 3
                        w = x.Parent.Right; //case 3
                    }
                    w.Colour = x.Parent.Colour; //case 4
                    x.Parent.Colour = Color.Black; //case 4
                    w.Right.Colour = Color.Black; //case 4
                    LeftRotate(x.Parent); //case 4
                    x = _root; //case 4
                }
                else //mirror code from above with "right" & "left" exchanged
                {
                    Node w = x.Parent.Left;
                    if (w.Colour == Color.Red)
                    {
                        w.Colour = Color.Black;
                        x.Parent.Colour = Color.Red;
                        RightRotate(x.Parent);
                        w = x.Parent.Left;
                    }
                    if (w.Right.Colour == Color.Black && w.Left.Colour == Color.Black)
                    {
                        w.Colour = Color.Black;
                        x = x.Parent;
                    }
                    else if (w.Left.Colour == Color.Black)
                    {
                        w.Right.Colour = Color.Black;
                        w.Colour = Color.Red;
                        LeftRotate(w);
                        w = x.Parent.Left;
                    }
                    w.Colour = x.Parent.Colour;
                    x.Parent.Colour = Color.Black;
                    w.Left.Colour = Color.Black;
                    RightRotate(x.Parent);
                    x = _root;
                }
            }
            if (x != null)
                x.Colour = Color.Black;
        }
        private Node Minimum(Node x)
        {
            while (x.Left.Left != null)
            {
                x = x.Left;
            }

            if (x.Left.Right != null)
            {
                x = x.Left.Right;
            }

            return x;
        }
        private Node TreeSuccessor(Node x)
        {
            if (x.Left != null)
            {
                return Minimum(x);
            }

            Node y = x.Parent;

            while (y != null && x == y.Right)
            {
                x = y;
                y = y.Parent;
            }
            return y;
        }
    }

    public class Node
    {
        public Color Colour { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Parent { get; set; }
        public int Data { get; set; }

        public Node(int data) { Data = data; }
        public Node(Color colour) { Colour = colour; }
        public Node(int data, Color colour) { Data = data; Colour = colour; }
    }

    public enum Color
    {
        Red,
        Black
    }
}