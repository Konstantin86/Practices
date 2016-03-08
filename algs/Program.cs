using System;
using System.Collections.Generic;
using System.Linq;
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
                new HeapSort<int>(GenerateArray(sortedArraySize)),
                new InsertionSort<int>(GenerateArray(sortedArraySize))
            }.ForEach(s => { s.Sort(); s.WriteOutput(); });

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
