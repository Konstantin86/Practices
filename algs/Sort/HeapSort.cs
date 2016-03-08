using System;

namespace algs.Sort
{
    // https://en.wikipedia.org/wiki/Heapsort
    class HeapSort<T> : SortBase<T> where T : IComparable
    {
        public HeapSort(T[] array) : base(array)
        {
        }

        protected override void ExecuteSort()
        {
            DoSort(_array);
        }

        public static void DoSort(T[] input)
        {
            //Build-Max-Heap
            int heapSize = input.Length;
            for (int p = (heapSize - 1) / 2; p >= 0; p--)
                MaxHeapify(input, heapSize, p);

            for (int i = input.Length - 1; i > 0; i--)
            {
                //Swap
                T temp = input[i];
                input[i] = input[0];
                input[0] = temp;

                heapSize--;
                MaxHeapify(input, heapSize, 0);
            }
        }

        private static void MaxHeapify(T[] input, int heapSize, int index)
        {
            int left = (index + 1) * 2 - 1;
            int right = (index + 1) * 2;
            int largest = 0;

            if (left < heapSize && input[left].CompareTo(input[index]) > 0)
                largest = left;
            else
                largest = index;

            if (right < heapSize && input[right].CompareTo(input[largest]) > 0)
                largest = right;

            if (largest != index)
            {
                T temp = input[index];
                input[index] = input[largest];
                input[largest] = temp;

                MaxHeapify(input, heapSize, largest);
            }
        }

        protected override string Algorythm => "HeapSort";
    }
}