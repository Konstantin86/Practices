using System;
using System.Collections.Generic;
using System.Linq;
using algs.DataStructures;
using algs.Interview;
using algs.Search;
using algs.Sort;

namespace algs
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sorting Demo (http://www.sorting-algorithms.com/)
            var sortedArraySize = 10;

            new List<SortBase<int>>
            {
                new BubbleSort<int>(GenerateArray(sortedArraySize)),
                new MergeSort<int>(GenerateArray(sortedArraySize)),
                new QuickSort<int>(GenerateArray(sortedArraySize)),
                new Quick3Sort<int>(GenerateArray(sortedArraySize)),
                new HeapSort<int>(GenerateArray(sortedArraySize)),
                new InsertionSort<int>(GenerateArray(sortedArraySize))
            }.ForEach(s => { s.Sort(); s.WriteOutput(); });

            // Search Demo:
            var arr1 = GenerateArray(10000, max: 10000);

            var arr2 = GenerateArray(10000, max: 10000);
            new BubbleSort<int>(arr2).Sort();

            var linearSearch = new LinearSearch<int[], int>(arr1);
            linearSearch.Search(arr1[9000]);
            linearSearch.WriteOutput();

            var binarySearch = new BinarySearch<int[], int>(arr2);
            binarySearch.Search(arr2[9000]);
            binarySearch.WriteOutput();

            // Tree Search Demo:
            TreeNode<string> treeNode = new TreeNode<string>("root");
            var children = treeNode.AddChildren("cars", "bikes");
            children[0].AddChildren("hyundai", "mazda", "toyota", "audi");
            // Iterative Search:
            var node = treeNode.Flatten().FirstOrDefault(n => n.Value == "mazda");
            // Recursive Search:
            node = treeNode.Find(n => n.Value == "mazda");

            // Red-Black-Tree:
            RedBlackTree tree = new RedBlackTree();
            tree.Insert(5);
            tree.Insert(3);
            tree.Insert(7);
            tree.Insert(1);
            tree.Insert(9);
            tree.Insert(-1);
            tree.Insert(11);
            tree.Insert(6);
            tree.DisplayTree();
            tree.Delete(-1);
            tree.DisplayTree();
            tree.Delete(9);
            tree.DisplayTree();
            tree.Delete(5);
            tree.DisplayTree();

            string result = ClassicTasks.IntToString(23456);
            
            Console.ReadKey();
        }

        private static int[] GenerateStaticArray()
        {
            return new[] { 5, 3, 7, 2, 1, 9, 4, 8, 6, 10, 17, 13, 2, 3, 4 };
        }

        public static int[] GenerateArray(int size = 100, int min = 0, int max = 10)
        {
            var randNum = new Random();
            return Enumerable
                .Repeat(0, size)
                .Select(i => randNum.Next(min, max))
                .ToArray();
        }
    }
}
