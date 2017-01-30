using System;

namespace algs.Sort
{
    class Quick3Sort<T> : SortBase<T> where T : IComparable
    {
        public Quick3Sort(T[] array) : base(array)
        {
        }

        protected override void ExecuteSort()
        {
            DoSort(_array, 0, _array.Length - 1);
        }

        private static void DoSort(T[] array, int start, int end)
        {
            if (end <= start) return;
            int lt = start, gt = end;
            T v = array[start];
            int i = start;
            while (i <= gt)
            {
                int cmp = array[i].CompareTo(v);
                if (cmp < 0) Exch(array, lt++, i++);
                else if (cmp > 0) Exch(array, i, gt--);
                else i++;
            }

            DoSort(array, start, lt - 1);
            DoSort(array, gt + 1, end);
        }

        private static void Exch(T[] array, int i, int y)
        {
            T temp = array[i];
            array[i] = array[y];
            array[y] = temp;
        }

        protected override string Algorythm => "3WayQuickSort";
    }
}