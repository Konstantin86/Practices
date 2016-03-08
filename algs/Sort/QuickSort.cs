using System;

namespace algs.Sort
{
    public class QuickSort<T> : SortBase<T> where T : IComparable
    {
        public QuickSort(T[] array) : base(array)
        {
        }

        protected override void ExecuteSort()
        {
            DoSort(_array, 0, _array.Length - 1);
        }

        // Alg Video Introduction: https://www.youtube.com/watch?v=COk73cpQbFQ
        // Fastest quicksort implementation: http://stackoverflow.com/questions/3719719/fastest-safe-sorting-algorithm-implementation
        private static void DoSort(T[] array, int start, int end)
        {
            if (start >= end)
                return;

            var partitionIndex = Partition(array, start, end);
            DoSort(array, start, partitionIndex - 1);
            DoSort(array, partitionIndex + 1, end);
        }

        private static int Partition(T[] array, int start, int end)
        {
            T pivot = array[end];
            int partitionIndex = start;

            for (int i = start; i < end; i++)
            {
                if (array[i].CompareTo(pivot) < 0)
                {
                    T temp1 = array[i];
                    array[i] = array[partitionIndex];
                    array[partitionIndex] = temp1;
                    partitionIndex++;
                }
            }

            T temp2 = array[partitionIndex];
            array[partitionIndex] = array[end];
            array[end] = temp2;

            return partitionIndex;
        }

        public override string Algorythm => "QuickSort";
    }
}